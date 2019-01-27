namespace Cash.Sample.Shared.Services
{
    public interface IRandomDataService
    {
        int GetCachedRandomNumber();

        int GetNonCachedRandomNumber();
    }
}
