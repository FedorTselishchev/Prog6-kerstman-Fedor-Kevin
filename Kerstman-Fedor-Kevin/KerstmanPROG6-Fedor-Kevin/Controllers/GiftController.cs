using KerstmanPROG6_Fedor_Kevin.models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SantasWishlist.Domain;
using System.ComponentModel.DataAnnotations;

namespace KerstmanPROG6_Fedor_Kevin.Controllers
{
    public class GiftController : Controller
    {
        private IGiftRepository _giftRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private GiftList _giftList;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public GiftController(IGiftRepository giftRepository, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _giftRepository = giftRepository;

            _giftList = new GiftList();
        }

        [HttpPost]
        //[Authorize]
        public IActionResult AboutMe(string username, string behaviour, int age, string explanation)
        {
            ViewBag.Username = username;
            ViewBag.Age = age;
            ViewBag.Explanation = explanation;
            ViewBag.Behaviour = behaviour;
            ViewBag.Gifts = _giftRepository.GetPossibleGifts();

            ViewBag.Errors = "";

            if (age <= 0)
            {
                TempData["alertMessage"] = "vul een geldige leeftijd!";
                return View("/Views/Home/Index.cshtml");
            }
            return View("/Views/Wishlist/Presents.cshtml", _giftList);
        }

        [HttpPost]
        //[Authorize]
        public async Task<IActionResult> Wishlist(List<string> gifts, int age, string behaviour, string username, string explanation, string extra)
        {
            var user = await _userManager.GetUserAsync(User);
            List<Gift> giftList = new List<Gift>();
            ICollection<ValidationResult> results = new List<ValidationResult>();
            List<string> errors = new List<string>();

            foreach (var gift in _giftRepository.GetPossibleGifts())
            {
                foreach (var chosengift in gifts)
                {
                    if (gift.Name == chosengift)
                    {
                        giftList.Add(gift);
                    }
                }
            }
            _giftList.allGifts = giftList;
            _giftList.Behaviour = behaviour;
            _giftList.Age = age;
            _giftList.user = user;
            _giftList.giftRepository = _giftRepository;
            _giftList.Extra = extra;

            if (explanation != null)
                _giftList.Explanation = explanation;

            if (Validator.TryValidateObject(_giftList
            , new ValidationContext(_giftList)
            , results, true))
            {
                ViewBag.Username = username;
                ViewBag.Age = age;
                ViewBag.Explanation = explanation;
                ViewBag.Behaviour = behaviour;
                ViewBag.Gifts = _giftRepository.GetPossibleGifts();
                ViewBag.ChosenGifts = gifts.Skip(1);

                return View("/Views/Wishlist/WishlistOverview.cshtml");
            }
            else
            {
                foreach (var result in results.Distinct().ToList())
                {
                    if (result != null)
                    { 
                        errors.Add(result.ErrorMessage);
                    }
                }

                ViewBag.Errors = errors.Distinct().ToList();
                ViewBag.Username = username;
                ViewBag.Age = age;
                ViewBag.Explanation = explanation;
                ViewBag.Behaviour = behaviour;
                ViewBag.Gifts = _giftRepository.GetPossibleGifts();

                return View("/Views/Wishlist/Presents.cshtml");
            }
        }

        [HttpPost]
        //[Authorize]
        public async Task<IActionResult> ConfirmAsync(List<string> gifts)
        {
            var user = await _userManager.GetUserAsync(User);

            WishList wishList = new WishList();
            wishList.Name = user.UserName;

            foreach (var gift in _giftRepository.GetPossibleGifts())
            {
                foreach (var chosengift in gifts)
                {
                    if (gift.Name == chosengift)
                    {
                        wishList.Wanted.Add(gift);
                    }
                }
            }
            _giftRepository.SendWishList(wishList);
            user.IsRegistered = true;
            
            await _userManager.UpdateAsync(user);
            await _signInManager.SignOutAsync();

            TempData["successMessage"] = "De kerstman heeft jouw verlanglijstje ontvangen!";
            return View("/Views/Santa/Login.cshtml");
        }
    }
}