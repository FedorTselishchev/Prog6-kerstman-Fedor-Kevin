using KerstmanPROG6_Fedor_Kevin.models;
using Moq;
using SantasWishlist.Domain;
using System.ComponentModel.DataAnnotations;
//using Gift = KerstmanPROG6_Fedor_Kevin.models.GiftList

namespace Kerstman_Fedor_Kevin.test
{
    public class GiftRuleTests
    {
        [Fact]
        public async Task Wishlist_Rule1()
        {
            var giftList = new List<Gift>();
            giftList.Add(new Gift() { Name = "Lego", Category = GiftCategory.WANT });
            giftList.Add(new Gift() { Name = "Pokemonkaartjes", Category = GiftCategory.WANT });
            giftList.Add(new Gift() { Name = "Bordspel", Category = GiftCategory.WANT });
            giftList.Add(new Gift() { Name = "Poppen", Category = GiftCategory.WANT });
            var giftListModel = new GiftList();
            giftListModel.allGifts = giftList;
            giftListModel.Behaviour = "goodbehaviour";
            giftListModel.user = new ApplicationUser() { UserName = "Kevin", Name = "Kevin", IsBehaved = true };
            ValidationContext validationContext = new ValidationContext(giftListModel);
            var results = giftListModel.Validate(validationContext);
            Assert.Equal("Have been nice, but do not choose too many gifts! (max 3)", results.First().ErrorMessage);
        }

        [Fact]
        public async Task Wishlist_Rule1_Exception1()
        {
            var giftList = new List<Gift>();
            giftList.Add(new Gift() { Name = "Lego", Category = GiftCategory.WANT });
            var giftListModel = new GiftList();
            giftListModel.allGifts = giftList;
            giftListModel.Behaviour = "goodbehaviour";
            giftListModel.user = new ApplicationUser() { UserName = "Kevin", Name = "Kevin", IsBehaved = true };
            ValidationContext validationContext = new ValidationContext(giftListModel);
            var results = giftListModel.Validate(validationContext);
            Assert.Equal("You have been nice! Choose at least 1 gift per category!", results.First().ErrorMessage);
        }

        [Fact]
        public async Task Wishlist_Rule1_Exception2()
        {
            var giftList = new List<Gift>();
            giftList.Add(new Gift() { Name = "Lego", Category = GiftCategory.WANT });
            giftList.Add(new Gift() { Name = "Pokemonkaartjes", Category = GiftCategory.WANT });
            var giftListModel = new GiftList();
            giftListModel.allGifts = giftList;
            giftListModel.Behaviour = "goodbehaviour";
            giftListModel.user = new ApplicationUser() { UserName = "Kevin", Name = "Kevin", IsBehaved = false };
            ValidationContext validationContext = new ValidationContext(giftListModel);
            var results = giftListModel.Validate(validationContext);
            Assert.Equal("You've been naughty AND been lying about it. You may ony choose 1 gift!", results.First().ErrorMessage);
        }

        [Fact]
        public async Task GiftTest2()
        {
            var giftList = new List<Gift>();
            giftList.Add(new Gift() { Name = "Lego", Category = GiftCategory.WANT });
            giftList.Add(new Gift() { Name = "K`nex", Category = GiftCategory.WANT });

            var giftListModel = new GiftList();
            giftListModel.allGifts = giftList;
            ValidationContext validationContext = new ValidationContext(giftListModel);
            var results = giftListModel.Validate(validationContext);
            Assert.Equal("Both lego AND knex is unacceptable!", results.First().ErrorMessage);
        }

        [Fact]
        public async Task GiftTest3()
        {
            var giftList = new List<Gift>();
            giftList.Add(new Gift() { Name = "Computerspel", Category = GiftCategory.WANT });
            giftList.Add(new Gift() { Name = "Lego", Category = GiftCategory.WANT });
            var giftListModel = new GiftList();
            giftListModel.Age = 1;
            giftListModel.allGifts = giftList;
            giftListModel.giftRepository = new Mock<GiftRepository>().Object;
            ValidationContext validationContext = new ValidationContext(giftListModel);
            var results = giftListModel.Validate(validationContext);
            Assert.Equal("You may only have 1 gift that does not match your age.", results.First().ErrorMessage);
        }

        [Fact]
        public async Task GiftTest4()
        {
            var giftList = new List<Gift>();
            giftList.Add(new Gift() { Name = "Dolfje Weerwolfje", Category = GiftCategory.READ });
            giftList.Add(new Gift() { Name = "Knex for dummies", Category = GiftCategory.READ });
            giftList.Add(new Gift() { Name = "Hoe overleef ik de middelbare school!", Category = GiftCategory.READ });
            giftList.Add(new Gift() { Name = "Harry potter", Category = GiftCategory.READ });
            giftList.Add(new Gift() { Name = "Hitchhikersguide to the galaxy", Category = GiftCategory.READ });
            var giftListModel = new GiftList();
            giftListModel.user = new ApplicationUser() { UserName = "Stijn", Name = "Stijn" };
            giftListModel.allGifts = giftList;
            giftListModel.giftRepository = new Mock<GiftRepository>().Object;
            ValidationContext validationContext = new ValidationContext(giftListModel);
            var results = giftListModel.Validate(validationContext);
            var success = results.Where(r => r == ValidationResult.Success);
            Assert.True((bool)success.Any());
        }

        [Fact]
        public async Task GiftTest5()
        {
            var giftList = new List<Gift>();
            giftList.Add(new Gift() { Name = "K'nex", Category = GiftCategory.WANT });
            giftList.Add(new Gift() { Name = "Roblox", Category = GiftCategory.WANT });
            giftList.Add(new Gift() { Name = "Pokemonkaartjes", Category = GiftCategory.WANT });
            giftList.Add(new Gift() { Name = "Bordspel", Category = GiftCategory.WANT });
            giftList.Add(new Gift() { Name = "Knex for dummies", Category = GiftCategory.READ });
            giftList.Add(new Gift() { Name = "Deo", Category = GiftCategory.NEED });
            giftList.Add(new Gift() { Name = "Broek", Category = GiftCategory.WEAR });
            var giftListModel = new GiftList();
            giftListModel.user = new ApplicationUser() { UserName = "Stijn", Name = "Stijn" };
            giftListModel.allGifts = giftList;
            giftListModel.giftRepository = new Mock<GiftRepository>().Object;
            giftListModel.Explanation = "vrijwilligerswerk";
            giftListModel.Behaviour = "goodbehaviour";
            ValidationContext validationContext = new ValidationContext(giftListModel);
            var results = giftListModel.Validate(validationContext);
            var success = results.Where(r => r == ValidationResult.Success);
            Assert.True((bool)success.Any());
        }

        [Fact]
        public async Task WishList_Test_Rule6()
        {
            var giftList = new List<Gift>();
            giftList.Add(new Gift() { Name = "Nachtlampje", Category = GiftCategory.WANT });
            giftList.Add(new Gift() { Name = "Computerspel", Category = GiftCategory.WANT });
            var giftListModel = new GiftList();
            giftListModel.allGifts = giftList;
            ValidationContext validationContext = new ValidationContext(giftListModel);
            var results = giftListModel.Validate(validationContext);
            Assert.Equal("Want a night lamp? You need underwear with that!", results.First().ErrorMessage);
        }

        [Fact]
        public async Task WishList_Test_Rule7()
        {
            var giftList = new List<Gift>();
            giftList.Add(new Gift() { Name = "Muziekinstrument", Category = GiftCategory.WANT });
            giftList.Add(new Gift() { Name = "Computerspel", Category = GiftCategory.WANT });
            var giftListModel = new GiftList();
            giftListModel.allGifts = giftList;
            giftListModel.giftRepository = new Mock<GiftRepository>().Object;
            ValidationContext validationContext = new ValidationContext(giftListModel);
            var results = giftListModel.Validate(validationContext);
            Assert.Equal("You cannot get a music instrument without earplugs or vice versa", results.First().ErrorMessage);
        }

        [Fact]
        public async Task WishList_Test_Rule8()
        {
            var giftList = new List<Gift>();
            giftList.Add(new Gift() { Name = "Jurk", Category = GiftCategory.WANT });
            var giftListModel = new GiftList();
            giftListModel.allGifts = giftList;
            giftListModel.giftRepository = new Mock<GiftRepository>().Object;
            giftListModel.Extra = "Jurk";
            ValidationContext validationContext = new ValidationContext(giftListModel);
            var results = giftListModel.Validate(validationContext);
            if (giftListModel.Extra == giftList[0].Name)
            {
                Assert.Equal("Cannot pick a gift from this list.", results.First().ErrorMessage);

            }
        }

        [Fact]
        public async Task GiftTest9()
        {
            var giftList = new List<Gift>();
            giftList.Add(new Gift() { Name = "Computerspel", Category = GiftCategory.WANT });
            var giftListModel = new GiftList();
            giftListModel.allGifts = giftList;
            giftListModel.giftRepository = new Mock<GiftRepository>().Object;
            giftListModel.Behaviour = "badbehaviour";
            giftListModel.user = new ApplicationUser() { UserName = "Kevin", Name = "Kevin", IsBehaved = true };
            ValidationContext validationContext = new ValidationContext(giftListModel);
            var results = giftListModel.Validate(validationContext);
            Assert.Equal("I know you do not have a gaming console, Kevin. How will you play your game!", results.First().ErrorMessage);
        }
    }
}