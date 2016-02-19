using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Cash.Autofac.Sample.Web.Startup))]
namespace Cash.Autofac.Sample.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
