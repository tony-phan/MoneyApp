#!/usr/bin/env node
import * as cdk from 'aws-cdk-lib';
import { DbStack } from '../lib/db-stack';
import { ApiStack } from '../lib/api-stack';
import { UiStack } from '../lib/ui-stack';

const app = new cdk.App();
const env = { account: '536984667307', region: 'us-west-1' };

const dbStack = new DbStack(app, 'DbStack', { env });

const apiStack = new ApiStack(app, 'ApiStack', { env });

const uiStack = new UiStack(app, 'UiStack', { env });

apiStack.addDependency(dbStack);
uiStack.addDependency(apiStack);