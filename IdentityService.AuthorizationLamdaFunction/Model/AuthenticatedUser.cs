using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityService.AuthorizationLambdaFunction.Model
{

    public class AuthenticatedUser
    {
        public string name { get; set; }
        public string role { get; set; }
        public bool? is_admin { get; set; }
        public string sub { get; set; }
    }
}
