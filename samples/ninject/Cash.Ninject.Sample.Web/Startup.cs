using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Cash.Ninject.Sample.Web.Startup))]
namespace Cash.Ninject.Sample.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
