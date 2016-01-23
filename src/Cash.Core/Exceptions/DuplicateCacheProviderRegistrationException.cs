using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cash.Core.Exceptions
{
    public class DuplicateCacheProviderRegistrationException : Exception
    {
        public DuplicateCacheProviderRegistrationException(Type type)
            :base($"A registration for the type {type.Name} has already been registered with the system.")
        {
            
        }
    }
}
