using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Principal;
namespace BindingModels
{
    public class UserPrincipal : GenericPrincipal
    {
        public string id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Initials { get; set; }
        public List<string> Roles { get; set; }
        public UserPrincipal(GenericIdentity User, string[] roles) : base(User, roles)
        {

        }
    }
}
