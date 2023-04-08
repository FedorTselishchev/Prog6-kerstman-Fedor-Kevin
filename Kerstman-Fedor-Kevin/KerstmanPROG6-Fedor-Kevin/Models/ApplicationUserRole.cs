using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace KerstmanPROG6_Fedor_Kevin.models
{
    public class ApplicationUserRole : IdentityRole<Guid>
    {
        public string Role { get; set; }
    }
}
