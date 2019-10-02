using SimpleInjector;
using SimpleInjector.Integration.Web.Mvc;
using System.Web.Mvc;
using TelephoneKeyPad.Domain;

namespace TelephoneKeyPad.Web
{
    public class SimpleInjectorConfig
    {
        internal static void Register()
        {
            var container = new Container();
            container.RegisterSingleton<ICombinationGenerator>(
                () => new CombinationGenerator(Keypad.E161));

            container.Verify();
            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
        }
    }
}
