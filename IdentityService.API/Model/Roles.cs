using System;
using System.Collections.Generic;

namespace IdentityService.API.Model
{
    public partial class Roles
    {
        public int Id { get; set; }
        public int? Level { get; set; }
        public string Name { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
    }
}
