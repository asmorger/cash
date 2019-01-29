namespace Cash.Sample.Core.Services
{
    public interface IRandomDataService
    {
        int GetCachedRandomNumber();

        int GetNonCachedRandomNumber();
    }
}
