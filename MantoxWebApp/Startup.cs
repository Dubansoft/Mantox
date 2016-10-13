using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MantoxWebApp.Startup))]
namespace MantoxWebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            
        }
    }
}
