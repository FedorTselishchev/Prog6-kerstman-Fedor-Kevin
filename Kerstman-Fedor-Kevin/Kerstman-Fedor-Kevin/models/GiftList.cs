using Microsoft.AspNetCore.Mvc;
using SantasWishlist.Domain;
using System.ComponentModel.DataAnnotations;

namespace Kerstman_Fedor_Kevin.models
{
    public class GiftList : IValidatableObject
    {
        public IGiftRepository giftRepo;

        public Gift gift;

        public List<Gift> allGifts;

        public User user;

        public int age;

        public string behaviour;

        public string explanation;

        public IEnumerable<ValidationResult> Validate(ValidationContext Context)
        {
            yield return ValidationResult.Success;
        }
    }
}
