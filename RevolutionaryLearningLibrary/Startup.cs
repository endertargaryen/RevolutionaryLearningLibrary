using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(RevolutionaryLearningLibrary.Startup))]
namespace RevolutionaryLearningLibrary
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
