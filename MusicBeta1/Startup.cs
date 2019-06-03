using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MusicBeta1.Startup))]
namespace MusicBeta1
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
