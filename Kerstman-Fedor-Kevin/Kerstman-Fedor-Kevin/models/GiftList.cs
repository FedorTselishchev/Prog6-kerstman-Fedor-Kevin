﻿using Microsoft.AspNetCore.Mvc;
using SantasWishlist.Domain;
using System.ComponentModel.DataAnnotations;

namespace Kerstman_Fedor_Kevin.models
{
    public class GiftList : IValidatableObject
    {
        public IGiftRepository giftRepository;

        public Gift Gift;

        public List<Gift> allGifts;

        public User user;

        public int Age;

        public string Behaviour;

        public string Explanation;

        public IEnumerable<ValidationResult> Validate(ValidationContext Context)
        {
            yield return ValidationResult.Success;
        }
    }
}
