import * as cdk from 'aws-cdk-lib';
import { Construct } from 'constructs';
import * as ec2 from 'aws-cdk-lib/aws-ec2';
import * as ecs from 'aws-cdk-lib/aws-ecs';
import * as ecr from 'aws-cdk-lib/aws-ecr';
import * as iam from 'aws-cdk-lib/aws-iam';
import * as secretsmanager from 'aws-cdk-lib/aws-secretsmanager';

export class ApiStack extends cdk.Stack {
    constructor(scope: Construct, id: string, props?: cdk.StackProps) {
        super(scope, id, props);

        const vpc = ec2.Vpc.fromLookup(this, 'MoneyAppVpc', {
            vpcId: 'vpc-0229fc250b4d5cec4', 
        });

        const rdsSecurityGroup = ec2.SecurityGroup.fromSecurityGroupId(
            this,
            'RdsSecurityGroup',
            cdk.Fn.importValue('RdsSecurityGroupId')
        );

        const ecsSecurityGroup = new ec2.SecurityGroup(this, 'EcsSecurityGroup', {
            vpc,
            allowAllOutbound: true,
        });

        rdsSecurityGroup.addIngressRule(
            ecsSecurityGroup,
            ec2.Port.tcp(3306),
            'Allow ECS to connect to RDS MySQL'
        );

        const dbSecret = secretsmanager.Secret.fromSecretCompleteArn(
            this,
            'DbSecret',
            cdk.Fn.importValue('RdsSecretArn')
        );

        const dbHost = cdk.Fn.importValue('RdsEndpoint');

        const cluster = new ecs.Cluster(this, 'money-api-cluster', { vpc });
        const repo = ecr.Repository.fromRepositoryName(this, 'Repo', 'money-api');

        const taskDef = new ecs.FargateTaskDefinition(this, 'TaskDef', {
            memoryLimitMiB: 1024,
            cpu: 512,
        });

        taskDef.addToExecutionRolePolicy(new iam.PolicyStatement({
            actions: ['secretsmanager:GetSecretValue'],
            resources: ['*'],
        }));


        const container = taskDef.addContainer('AppContainer', {
            image: ecs.ContainerImage.fromEcrRepository(repo),
            logging: ecs.LogDrivers.awsLogs({ streamPrefix: 'MoneyApp' }),
            environment: {
                ASPNETCORE_ENVIRONMENT: 'Production',
                ASPNETCORE_URLS: 'http://+:80',
                DB_HOST: dbHost,
                DB_NAME: 'moneyapp',
            },
            secrets: {
                DB_USER: ecs.Secret.fromSecretsManager(dbSecret, 'username'),
                DB_PASSWORD: ecs.Secret.fromSecretsManager(dbSecret, 'password'),
                DB_PORT: ecs.Secret.fromSecretsManager(dbSecret, 'port')
            }
        });

        container.addPortMappings({
            containerPort: 80,
        });

        new ecs.FargateService(this, 'Service', {
            cluster,
            taskDefinition: taskDef,
            desiredCount: 1,
            securityGroups: [ecsSecurityGroup],
            assignPublicIp: true,
            vpcSubnets: { 
                subnetType: ec2.SubnetType.PUBLIC 
            },
        });
    }
}