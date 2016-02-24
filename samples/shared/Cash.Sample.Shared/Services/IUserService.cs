using Cash.Sample.Shared.Models;

namespace Cash.Sample.Shared.Services
{
    public interface IUserService
    {
        UserModel GetUserById(int id);

        AddressModel GetByUser(UserModel user);
    }
}
