using CommunityToolkit.Mvvm.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;

using MPCMobile.Client.Interfaces;
using MPCMobile.Client.Models;
using MPCMobile.Client.Services;
using MPCMobile.Helpers;

using Xamarin.Forms;

namespace MPCMobile
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            Ioc.Default.ConfigureServices(
                    new ServiceCollection()
                    // Services
                    .AddSingleton<IAreaService<AreaModel>, AreaService>()
                    .AddSingleton<ICategorySerice<CategoryModel>, CategoryService>()
                    .AddSingleton<IEventService<EventModel>, EventService>()
                    .AddSingleton<IReqResInService, ReqResInService>()
                    .AddSingleton<IDisplayAlert, DisplayAlert>()

                    // ViewModels

                    .BuildServiceProvider()
                );

            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
