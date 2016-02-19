using Cash.Autofac.Sample.Web.Models;

namespace Cash.Autofac.Sample.Web.Services
{
    public interface IUserService
    {
        UserModel GetUserById(int id);

        AddressModel GetByUser(UserModel user);
    }
}
