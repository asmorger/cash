using Cash.Sample.Core.Models;

namespace Cash.Sample.Core.Services
{
    public interface IUserService
    {
        UserModel GetUserById(int id);

        AddressModel GetByUser(UserModel user);
    }
}
