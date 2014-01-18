using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AspNetEmailExample.Startup))]
namespace AspNetEmailExample
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
