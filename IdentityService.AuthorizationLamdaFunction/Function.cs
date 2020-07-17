/*
* Copyright 2015-2016 Amazon.com, Inc. or its affiliates. All Rights Reserved.
*
* Licensed under the Apache License, Version 2.0 (the "License"). You may not use this file except in compliance with the License. A copy of the License is located at
*
*     http://aws.amazon.com/apache2.0/
*
* or in the "license" file accompanying this file. This file is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
*/

// Author: Caleb Petrick

using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using IdentityService.AuthorizationLambdaFunction.Constants;
using IdentityService.AuthorizationLambdaFunction.Error;
using IdentityService.AuthorizationLambdaFunction.Model;
using IdentityService.AuthorizationLambdaFunction.Model.Auth;
using Newtonsoft.Json;

namespace IdentityService.AuthorizationLambdaFunction
{
    public class Function
    {
        private static readonly HttpClient _client = new HttpClient();
        /// <summary>
        /// A simple function that takes the token authorizer and returns a policy based on the authentication token included.
        /// </summary>
        /// <param name="input">token authorization received by api-gateway event sources</param>
        /// <param name="context"></param>
        /// <returns>IAM Auth Policy</returns>
        [LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]
        public async Task<AuthPolicy> FunctionHandler(TokenAuthorizerContext input, ILambdaContext context)
        {
            try
            {
                var authResponse = new AuthPolicy();
                context.Logger.LogLine($"{nameof(input.AuthorizationToken)}: {input.AuthorizationToken}");
                context.Logger.LogLine($"{nameof(input.MethodArn)}: {input.MethodArn}");

                var userInfoEndpointUrl = string.Format(IdentityServiceEndpointConst.UserInfoEndpoint, System.Environment.GetEnvironmentVariable(EnvironmentVariableConsts.IdentiyServiceBaseUrl));

                context.Logger.LogLine($"{nameof(userInfoEndpointUrl)}: {userInfoEndpointUrl}");

                //The principalId is a required property on your authorizer response.
                var principalId = Guid.NewGuid().ToString();
                var authenticationHeaderSchema = "Bearer";
                _client.DefaultRequestHeaders.Clear();
                var token = input.AuthorizationToken.Replace(authenticationHeaderSchema, string.Empty).Trim();
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(authenticationHeaderSchema, token);

                var result = _client.GetAsync(userInfoEndpointUrl).Result;
                var responseBody = await result.Content.ReadAsStringAsync();

                if (result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    context.Logger.LogLine($"responseBody : {responseBody}");
                    var userInfo = JsonConvert.DeserializeObject<AuthenticatedUser>(responseBody);


                    // build apiOptions for the AuthPolicy
                    var methodArn = ApiGatewayArn.Parse(input.MethodArn);
                    var apiOptions = new ApiOptions(methodArn.Region, methodArn.RestApiId, methodArn.Stage);
                    var policyBuilder = new AuthPolicyBuilder(principalId, methodArn.AwsAccountId, apiOptions);

                    //policyBuilder.DenyAllMethods();

                    if (userInfo.is_admin.HasValue && userInfo.is_admin.Value)
                    {
                        context.Logger.LogLine($"allow : {responseBody}");
                        //allow access to Auction Resource
                        policyBuilder.AllowAllMethods();
                    }
                    else
                    {
                        context.Logger.LogLine($"deny allow : {responseBody}");
                        policyBuilder.DenyAllMethods();
                    }

                    // finally, build the policy
                    authResponse = policyBuilder.Build();

                }
                else
                {
                    context.Logger.LogLine($"unauthorized : {responseBody}");
                    throw new UnauthorizedException();
                }


                return authResponse;
            }
            catch (Exception ex)
            {
                if (ex is UnauthorizedException)
                    throw;

                // log the exception and return a 401
                context.Logger.LogLine(ex.ToString());
                throw new UnauthorizedException();
            }
        }
    }
}
