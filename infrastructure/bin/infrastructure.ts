#!/usr/bin/env node
import * as cdk from 'aws-cdk-lib';
import { DbStack } from '../lib/db-stack';
import { ApiStack } from '../lib/api-stack';

const app = new cdk.App();
const env = { account: '536984667307', region: 'us-west-1' };

new DbStack(app, 'DbStack', { env });

new ApiStack(app, 'ApiStack', { env });