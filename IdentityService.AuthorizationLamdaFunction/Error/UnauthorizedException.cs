namespace IdentityService.AuthorizationLambdaFunction.Error
{
    internal class UnauthorizedException : System.Exception
    {
        public UnauthorizedException() : base("Unauthorized")
        {
        }
    }
}
