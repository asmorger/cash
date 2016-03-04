namespace Cash.Core.Extensions
{
    public static class CacheItemPriorityExtensions
    {
        public static System.Runtime.Caching.CacheItemPriority Convert(this CacheItemPriority priority)
        {
            switch (priority)
            {
                case CacheItemPriority.NeverRemove:
                    return System.Runtime.Caching.CacheItemPriority.NotRemovable;
                default:
                    return System.Runtime.Caching.CacheItemPriority.Default;
            }
        }
    }
}
