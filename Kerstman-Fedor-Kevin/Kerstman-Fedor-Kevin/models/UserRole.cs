using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Kerstman_Fedor_Kevin.models
{
    public class UserRole : IdentityRole<Guid>
    {
        public string Role { get; set; }
    }
}
