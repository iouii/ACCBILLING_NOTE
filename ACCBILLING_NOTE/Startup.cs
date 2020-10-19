using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ACCBILLING_NOTE.Startup))]
namespace ACCBILLING_NOTE
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
