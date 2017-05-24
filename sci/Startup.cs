using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(sci.Startup))]
namespace sci
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.MapSignalR();
        }
    }
}
