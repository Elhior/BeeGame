using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Bee_game.Startup))]
namespace Bee_game
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //ConfigureAuth(app);
        }
    }
}
