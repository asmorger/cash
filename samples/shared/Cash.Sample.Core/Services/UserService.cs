using System.Collections.Generic;
using System.Linq;
using Cash.Core.Attributes;
using Cash.Sample.Core.Models;

namespace Cash.Sample.Core.Services
{
    public class UserService : IUserService
    {
        private IList<UserModel> _users;
        private IList<AddressModel> _addresses;

        [Cache]
        public UserModel GetUserById(int id)
        {
            // make this generate every time so if the cache fails we notice
            _users = GetUsers();

            var output = _users.FirstOrDefault(x => x.Id == id);
            return output;
        }

        [Cache]
        public AddressModel GetByUser(UserModel user)
        {
            // make this generate every time so if the cache fails we notice
            _addresses = GetAddresses();

            // yes, I know this is bad and I feel bad.  But hey, it's a contrived example.
            var output = _addresses.FirstOrDefault(x => x.Id == user.Id);
            return output;
        }

        private IList<UserModel> GetUsers()
        {
            var output = new List<UserModel>();

            for (int i = 0; i < 10; i++)
            {
                var model = new UserModel
                                {
                                    Id = i,
                                    FirstName = Faker.Name.First(),
                                    LastName = Faker.Name.Last()
                };


                output.Add(model);
            }

            return output;
        }

        private IList<AddressModel> GetAddresses()
        {
            var output = new List<AddressModel>();

            for (int i = 0; i < 10; i++)
            {
                var model = new AddressModel
                {
                    Id = i,
                    PrimaryAddress = Faker.Address.StreetAddress(),
                    SecondaryAddress = Faker.Address.SecondaryAddress()
                };


                output.Add(model);
            }

            return output;
        }
    }
}
