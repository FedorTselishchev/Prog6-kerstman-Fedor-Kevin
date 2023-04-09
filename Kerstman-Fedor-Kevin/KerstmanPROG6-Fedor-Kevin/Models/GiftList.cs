using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SantasWishlist.Domain;
using System.ComponentModel.DataAnnotations;

namespace KerstmanPROG6_Fedor_Kevin.models
{
    public class GiftList : IValidatableObject
    {
        public IGiftRepository giftRepository;

        public Gift Gift;

        public List<Gift> allGifts;

        public ApplicationUser user;

        public int Age;

        public string Behaviour;

        public string Explanation;

        public string Extra;

        public IEnumerable<ValidationResult> Validate(ValidationContext Context)
        {
            // Get the number of gifts
            int giftCounter = allGifts.Count();

            // Check if the user is a volunteer
            bool isVolunteer = false;

            // Rule 4
            if (user != null && user.UserName.ToLower() == "stijn") // To lower so either Stijn or stijn works
            {
                foreach (var rule4i1 in allGifts)
                {
                    if (rule4i1.Name.ToLower() == "dolfje weerwolfje")
                    {
                        giftCounter--;
                    }
                }
            }

            if (Behaviour != null)
            {
                // Check for good behavior
                if (Behaviour == "goodbehaviour")
                {
                    // Rule 5
                    if (Explanation != null && Explanation.ToLower() == "vrijwilligerswerk" && user.IsBehaved)
                    {
                        isVolunteer = true;
                    }

                    // Rule 1
                    if (user.IsBehaved &&
                        (allGifts.Where(c => c.Category == GiftCategory.WANT).Count() > 3 ||
                         allGifts.Where(c => c.Category == GiftCategory.NEED).Count() > 3 ||
                         allGifts.Where(c => c.Category == GiftCategory.WEAR).Count() > 3 ||
                         allGifts.Where(c => c.Category == GiftCategory.READ).Count() > 3) && !isVolunteer)
                    {
                        yield return new ValidationResult("Have been nice, but do not choose too many gifts! (max 3)");
                    }

                    //Rule 9
                    else if (user.IsBehaved == true)
                    {
                        if ((allGifts.Where(c => c.Category == GiftCategory.WANT).Count() == 0 ||
                            allGifts.Where(c => c.Category == GiftCategory.NEED).Count() == 0 ||
                            allGifts.Where(c => c.Category == GiftCategory.WEAR).Count() == 0 ||
                            allGifts.Where(c => c.Category == GiftCategory.READ).Count() == 0) && !isVolunteer)
                        {
                            yield return new ValidationResult("You have been nice! Choose at least 1 gift per category!");
                        }
                    }

                    // Rule 1
                    // Exception: Santa knows a kid has been bad, but the kid lied about it in about me.
                    // Only 1 present can be chosen.       
                    if (user.IsBehaved == false &&
                        allGifts.Where(c => c.Category == GiftCategory.WANT ||
                                            c.Category == GiftCategory.NEED ||
                                            c.Category == GiftCategory.WEAR ||
                                            c.Category == GiftCategory.READ).Count() > 1 && !isVolunteer)
                    {
                        yield return new ValidationResult("You've been naughty AND been lying about it. You may ony choose 1 gift!");
                    }
                }
                // Rule 1
                // Exception: A kid that has been bad may only choose 1 gift per category
                else if (Behaviour == "badbehaviour" && !user.IsBehaved)
                    if (allGifts.Where(c => c.Category == GiftCategory.WANT).Count() > 1 ||
                        allGifts.Where(c => c.Category == GiftCategory.NEED).Count() > 1 ||
                        allGifts.Where(c => c.Category == GiftCategory.WEAR).Count() > 1 ||
                        allGifts.Where(c => c.Category == GiftCategory.READ).Count() > 1 && !isVolunteer)
                        yield return new ValidationResult("Stout geweest, dat betekent maar max 1 cadeau per categorie!");
            }
            if (allGifts != null)
            {
                // Rule 6, oblige underwear with lamp
                foreach (var rule6gift in allGifts)
                    if (rule6gift.Name == "Nachtlampje")
                        foreach (var rule6i2 in allGifts)
                            if (rule6i2.Name != "Ondergoed")
                                yield return new ValidationResult("Want a night lamp? You need underwear with that!");

                // Rule 2, dont allow both knex and lego
                var lego = allGifts.Where(i => i.Name.ToLower().Contains("lego")).Count();
                var knex = allGifts.Where(i => i.Name.ToLower().Contains("k`nex") ||
                                i.Name.ToLower().Contains("knex")).Count();

                if (lego >= 1 && knex >= 1)
                    yield return new ValidationResult("Both lego AND knex is unacceptable!");

                // Rule 3, ony 1 gift alloweed that doesnt match age
                var skipAgeCounter = allGifts.Where(i => giftRepository.CheckAge(i.Name) > Age).Count();

                if (skipAgeCounter == 2)
                    yield return new ValidationResult("You may only have 1 gift that does not match your age.");

                // Rule 7, instrument requires earplugs
                var match1 = allGifts.Where(x => x.Name == "Muziekinstrument" || x.Name == "Oordopjes").Count();

                if (match1 != 0)
                    yield return new ValidationResult("You cannot get a music instrument without oordopjes");
            }

            // Rule 8 Cant pick extra from gift list
            if (Extra != null)
                foreach (var gift in giftRepository.GetPossibleGifts())
                    if (gift.Name.ToLower() == Extra.ToLower())
                        yield return new ValidationResult("Cannot pick a gift from this list.");

            yield return ValidationResult.Success;
        }
    }
}
