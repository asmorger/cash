using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cash.Autofac.Sample.Web.Services
{
    public interface IRandomDataService
    {
        int GetCachedRandomNumber();

        int GetNonCachedRandomNumber();
    }
}
