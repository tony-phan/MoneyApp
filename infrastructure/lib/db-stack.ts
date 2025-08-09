import * as cdk from 'aws-cdk-lib';
import { Construct } from 'constructs';
import { CfnOutput, RemovalPolicy, Tags } from 'aws-cdk-lib';
import * as ec2 from 'aws-cdk-lib/aws-ec2';
import * as rds from 'aws-cdk-lib/aws-rds';

export class DbStack extends cdk.Stack {
  public readonly dbInstance: rds.DatabaseInstance;
  public readonly dbCredentialsSecret: rds.DatabaseSecret;

  constructor(scope: Construct, id: string, props?: cdk.StackProps) {
    super(scope, id, props);

    const vpc = ec2.Vpc.fromLookup(this, 'MoneyAppVpc', {
      vpcId: 'vpc-0229fc250b4d5cec4',
    });

    const ec2SecurityGroup = ec2.SecurityGroup.fromSecurityGroupId(
      this,
      'Ec2SecurityGroup',
      'sg-0bbb22f82c48ba46b'
    );

    const rdsSecurityGroup = new ec2.SecurityGroup(this, 'RdsSecurityGroup', {
      vpc,
      description: 'Security group for RDS allowing ECS access',
      allowAllOutbound: true,
    });

    rdsSecurityGroup.addIngressRule(
      ec2SecurityGroup, 
      ec2.Port.tcp(3306), 
      'Allow EC2 access'
    );

    new cdk.CfnOutput(this, 'RdsSecurityGroupId', {
      value: rdsSecurityGroup.securityGroupId,
      exportName: 'RdsSecurityGroupId'
    });

    const dbCredentialsSecret = new rds.DatabaseSecret(this, 'DbCredentialsSecret', {
      username: 'moneyadmin',
    });

    const dbInstance = new rds.DatabaseInstance(this, 'MoneyAppDatabase', {
      engine: rds.DatabaseInstanceEngine.mysql({
        version: rds.MysqlEngineVersion.VER_8_0_42,
      }),
      vpc,
      vpcSubnets: {
        subnetType: ec2.SubnetType.PUBLIC,
      },
      instanceType: ec2.InstanceType.of(ec2.InstanceClass.T3, ec2.InstanceSize.MICRO),
      credentials: rds.Credentials.fromSecret(dbCredentialsSecret),
      allocatedStorage: 20,
      maxAllocatedStorage: 20,
      multiAz: false,
      securityGroups: [rdsSecurityGroup],
      publiclyAccessible: false,
      deletionProtection: false,
      removalPolicy: RemovalPolicy.DESTROY,
    });

    this.dbInstance = dbInstance;
    this.dbCredentialsSecret = dbCredentialsSecret;

    new CfnOutput(this, 'RdsEndpoint', {
      value: dbInstance.dbInstanceEndpointAddress,
      description: 'RDS MySQL endpoint',
    });

    new CfnOutput(this, 'RdsSecretArn', {
      value: dbCredentialsSecret.secretArn,
      description: 'Secrets Manager ARN for RDS credentials',
    });

    Tags.of(this).add('Project', 'MoneyApp');
  }
}
