using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cash.Autofac.Sample.Web.Models
{
    public class AddressModel
    {
        public int Id { get; set; }

        public string PrimaryAddress { get; set; }

        public string SecondaryAddress { get; set; }
    }
}
