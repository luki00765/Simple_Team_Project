using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SimpleTeamPlayers.Startup))]
namespace SimpleTeamPlayers
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
