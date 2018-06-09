using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(E_LearningApplication_Final.Startup))]
namespace E_LearningApplication_Final
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
