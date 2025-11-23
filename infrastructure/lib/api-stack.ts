import * as cdk from 'aws-cdk-lib';
import * as ec2 from 'aws-cdk-lib/aws-ec2';
import * as ecs from 'aws-cdk-lib/aws-ecs';
import * as ecr from 'aws-cdk-lib/aws-ecr';
import * as secretsmanager from 'aws-cdk-lib/aws-secretsmanager';
import * as ecsPatterns from 'aws-cdk-lib/aws-ecs-patterns';
import { Construct } from 'constructs';

export class ApiStack extends cdk.Stack {
    constructor(scope: Construct, id: string, props?: cdk.StackProps) {
        super(scope, id, props);

        const vpc = ec2.Vpc.fromLookup(this, 'MoneyAppVpc', {
            vpcId: 'vpc-0229fc250b4d5cec4', 
        });

        const ecsSecurityGroup = new ec2.SecurityGroup(this, 'EcsSecurityGroup', {
            vpc,
            allowAllOutbound: true,
        });

        // Use L1 construct to add ingress rule to RDS security group
        const rdsSecurityGroupId = cdk.Fn.importValue('RdsSecurityGroupId');
        
        new ec2.CfnSecurityGroupIngress(this, 'RdsIngressFromEcs', {
            groupId: rdsSecurityGroupId,
            ipProtocol: 'tcp',
            fromPort: 3306,
            toPort: 3306,
            sourceSecurityGroupId: ecsSecurityGroup.securityGroupId,
            description: 'Allow ECS to connect to RDS MySQL'
        });

        const dbSecret = secretsmanager.Secret.fromSecretCompleteArn(
            this,
            'DbSecret',
            cdk.Fn.importValue('RdsSecretArn')
        );

        const jwtSecret = secretsmanager.Secret.fromSecretNameV2(
            this,
            "JwtKey",
            "moneyapp/prod/jwt-signing-key"
        );

        const dbHost = cdk.Fn.importValue('RdsEndpoint');

        const cluster = new ecs.Cluster(this, 'money-api-cluster', { vpc });
        const repo = ecr.Repository.fromRepositoryName(this, 'Repo', 'money-api');

        const albService = new ecsPatterns.ApplicationLoadBalancedFargateService(this, 'ApiService', {
            cluster,
            cpu: 512,
            desiredCount: 1,
            memoryLimitMiB: 1024,
            publicLoadBalancer: true,
            securityGroups: [ecsSecurityGroup],
            taskImageOptions: {
                image: ecs.ContainerImage.fromEcrRepository(repo),
                containerPort: 80,
                environment: {
                    ASPNETCORE_ENVIRONMENT: 'Production',
                    ASPNETCORE_URLS: 'http://+:80',
                    DB_HOST: dbHost,
                    DB_NAME: 'moneyapp',
                    FRONTEND_URL: `https://d3eou1hh8j8iwc.cloudfront.net`
                },
                secrets: {
                    DB_USER: ecs.Secret.fromSecretsManager(dbSecret, 'username'),
                    DB_PASSWORD: ecs.Secret.fromSecretsManager(dbSecret, 'password'),
                    DB_PORT: ecs.Secret.fromSecretsManager(dbSecret, 'port'),
                    JWT_KEY: ecs.Secret.fromSecretsManager(jwtSecret, 'jwt_key'),
                },
                logDriver: ecs.LogDrivers.awsLogs({ streamPrefix: 'MoneyApp' }),
            },
            assignPublicIp: true
        });

        // Configure health check to use Swagger
        albService.targetGroup.configureHealthCheck({
            path: '/swagger',
            interval: cdk.Duration.seconds(30),
            timeout: cdk.Duration.seconds(5),
            healthyThresholdCount: 2,
            unhealthyThresholdCount: 3,
            healthyHttpCodes: '200,301,302'  // Include redirects in case Swagger redirects
        });

        const apiUrl = `http://${albService.loadBalancer.loadBalancerDnsName}`;

        new cdk.CfnOutput(this, 'ApiUrl', {
            value: apiUrl,
            description: 'Public URL for the MoneyApp API',
            exportName: 'MoneyAppApiUrl',
        });

        new cdk.CfnOutput(this, 'ClusterName', {
            value: cluster.clusterName,
            description: 'ECS Cluster Name',
            exportName: 'MoneyAppClusterName',
        });

        new cdk.CfnOutput(this, 'ServiceName', {
            value: albService.service.serviceName,
            description: 'ECS Service Name',
            exportName: 'MoneyAppServiceName',
        });
    }
}