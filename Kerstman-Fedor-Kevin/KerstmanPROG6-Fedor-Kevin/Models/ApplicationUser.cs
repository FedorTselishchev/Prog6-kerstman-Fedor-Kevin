using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KerstmanPROG6_Fedor_Kevin.models
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        [Required]
        [Display(Name = "Voornaam")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "Het naam moet tussen 2 en 20 karakters zijn.")]
        [RegularExpression("[A-Za-z,]+", ErrorMessage = "Het naam bevat alleen letters")]
        public string Name { get; set; }
        
        [NotMapped]
        [Required]
        [Display(Name = "Wachtwoord")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Het wachtwoord moet tussen 5 en 20 karakters zijn.")]
        [RegularExpression("[A-Za-z]+", ErrorMessage = "Wachtwoord bevat alleen letters")]
        public string Password { get; set; }


        [Required]
        public bool IsRegistered { get; set; }

        [Required]
        public bool IsBehaved { get; set; }

        [NotMapped]
        public string Role { get; set; } = "";

    }
}
