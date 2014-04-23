using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PoeItemInfo.Website.Startup))]
namespace PoeItemInfo.Website
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
        }
    }
}
