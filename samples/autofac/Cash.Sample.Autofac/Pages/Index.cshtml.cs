using Cash.Sample.Core.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cash.Sample.Autofac.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly IRandomDataService _randomDataService;

        public int CachedRandomNumber { get; private set; }
        public int NonCachedRandomNumber { get; private set; }

        public string Username { get; private set; }
        public string Address { get; private set; }

        public IndexModel(IUserService userService, IRandomDataService randomDataService)
        {
            _userService = userService;
            _randomDataService = randomDataService;
        }

        public void OnGet()
        {
            CachedRandomNumber = _randomDataService.GetCachedRandomNumber();
            NonCachedRandomNumber = _randomDataService.GetNonCachedRandomNumber();

            var user = _userService.GetUserById(1);
            var address = _userService.GetByUser(user);

            Username = $"{user.FirstName} {user.LastName}";
            Address = $"{address.PrimaryAddress} {address.SecondaryAddress}";
        }
    }
}
