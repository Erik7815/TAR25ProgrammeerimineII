using Microsoft.Extensions.DependencyInjection;
using ShopTARpe25.Core.ServiceInterface;
using ShopTARpe25.ApplicationServices.Services;
using ShopTARpe25.Data;
using ShopTARpe25.RealEstateTest.Macros;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace ShopTARpe25.RealEstateTest
{
    public abstract class TestBase
    {
        //Teenusepakkuja, mille kaudu saab kasutada registreeritud teenuseid
        protected IServiceProvider ServiceProvider { get; set; }

        // Testide baasklassi konstruktor.
        protected TestBase()
        {
            //loob uue teenuse kogu vajalikuks teenuste testimiseks
            var services = new ServiceCollection();
            //kutsub välja meetodi mis lisab teenused ja vajalkud sõltuvused
            SetupServices(services);
            //selle kaudu saab ttenuseid küsida
            ServiceProvider = services.BuildServiceProvider();
        }

        // Meetod teenuste registreerimiseks ja seadistamiseks.
        // Virtual võimaldab seda meetodit vajadusel alamklassides üle kirjutada.
        public virtual void SetupServices(IServiceCollection services)
        {
            services.AddScoped<IRealEstateServices, RealEstateServices>();

            //registreerib andmebaasi konteksti
            services.AddDbContext<ShopTARpe25Context>(x =>
            {
                //seadistab kasutama mälulisest andmebasist nimega TEST, et testimisel
                //kasutada ilma päris andmebaasita
                x.UseInMemoryDatabase("TEST");
                //eirab hoiatust, et mälusisene DB ei toeta tehinguid samamoodi nagu päris andmebaas
                x.ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            });
            //kutsub välja macrode klassi
            RegisterMacros(services);
        }
        //vabastab ressursid peale kasutamist
        public void Dispose()
        {

        }
        //abimeetod millega klass ja selle alamklassid saavad teenuse tööbi järgi küsida
        protected T Svc<T>()
        {
            //otsib teenusepakkuja tüüpi
            return ServiceProvider.GetService<T>();
        }
        //abimeetod macrode leidmiseks ja registreerimiseks
        private void RegisterMacros(IServiceCollection services)
        {
            //saaks reflektsioooniga otsingus kasutada
            var macroBaseType = typeof(IMacros);

            //loeb kokku kõik tüübid sellest assemblist, kus Macro on määratletud
            var macros = macroBaseType.Assembly.GetTypes()
                //valib tüübid mis rakendavad IMacros kiidest aga ei ole ise liidesed
                //ega abstraksed klassid
                .Where(t => macroBaseType.IsAssignableFrom(t)
                && !t.IsInterface && !t.IsAbstract);

            //käib leitud makrode classid ükshaaval läbi
            foreach (var macro in macros)
            {
                //kasutab ühte makro eksemplari
                services.AddSingleton(macro);
            }
        }
    }
}
