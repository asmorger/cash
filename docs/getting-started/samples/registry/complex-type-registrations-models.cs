public class UserModel
{
    public int Id { get; set; }
    public string FirstName { get; set; }
}

public class AddressModel
{
    public int Id { get; set }
    public string Address1 { get; set; }
}

public interface IUserService
{
    AddressModel GetAddress(UserModel user);
}

public class UserService : IUserService
{
    [Cache]
    public AddressModel GetAddress(UserModel user)
    {
        // .. goes forth and gets the address
    }
}