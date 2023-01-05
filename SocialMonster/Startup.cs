using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SocialMonster.Startup))]
namespace SocialMonster
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
