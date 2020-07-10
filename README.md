# IdentityService
Authentication and Authorization server based on IdentityServer4

## Step to run the project:

1/Create mysql schema for identityserver DB 

2/Create table __efmigrationshistory by running script

CREATE TABLE `__EFMigrationsHistory` ( `MigrationId` nvarchar(150) NOT NULL, `ProductVersion` nvarchar(32) NOT NULL, PRIMARY KEY (`MigrationId`) )

3/Run the user_role_v1.sql to create table for account & role

4/Update connection string

5/Build and run project

## Create token using endpoint 'connect/token'

Post: connect/token

Request body (x-www-form-urlencoded)

client_id: AutoproffFrontend

client_secret: AutoproffFrontend

scope: openid email role_res (add scope to config what claim to include in token )

grant_type: password

username:

password:

## Get user indo by using endpoint 'connect/userinfo'

Get: connect/userinfo

Request Headers:

Authorization: Bearer (token)

