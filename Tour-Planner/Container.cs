using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using Tour_Planner.Services;
using Tour_Planner.Services.Interfaces;
using Tour_Planner.ViewModels;
using Tour_Planner.Views;

namespace Tour_Planner
{
    public class Container
    {
        private readonly ServiceProvider serviceProvider;

        public Container()
        {
            var services = new ServiceCollection();
            services.AddSingleton<IRestService>(new RestService());
            services.AddSingleton<IDialogService>(x => new DialogService());
            services.AddSingleton(x => new ListToursViewModel(x.GetService<IDialogService>()!, x.GetService<IRestService>()!));
            services.AddSingleton<HomeViewModel>();
            services.AddSingleton<TourDataViewModel>();
            serviceProvider = services.BuildServiceProvider();
            serviceProvider.GetService<IDialogService>()!.Register<AddTourViewModel, AddTourDialogWindow>();
            serviceProvider.GetService<IDialogService>()!.Register<AddTourLogViewModel, AddTourLogDialogWindow>();        
        }

        public ListToursViewModel ListToursViewModel => serviceProvider.GetService<ListToursViewModel>()!;
        public HomeViewModel HomeViewModel => serviceProvider.GetService<HomeViewModel>()!;
        public TourDataViewModel TourDataViewModel => serviceProvider.GetService<TourDataViewModel>()!;
    }
}
