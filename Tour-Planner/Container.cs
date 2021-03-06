using System.Reflection;
using log4net;
using Microsoft.Extensions.DependencyInjection;
using Tour_Planner.Extensions;
using Tour_Planner.Services;
using Tour_Planner.Services.Interfaces;
using Tour_Planner.ViewModels;
using Tour_Planner.ViewModels.TourLogs;
using Tour_Planner.ViewModels.Tours;
using Tour_Planner.Views;

namespace Tour_Planner
{
    public class Container
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod()!.DeclaringType);

        private readonly ServiceProvider _serviceProvider;

        public Container()
        {
            Log.Debug("INIT Container with all used SingleTonObjects");
            var services = new ServiceCollection();

            services.AddSingleton<IRestService>(new RestService());
            services.AddSingleton<IMediator>(new Mediator());
            services.AddSingleton<IDialogService>(_ => new DialogService());

            services.AddSingleton<Configuration>();
            services.AddSingleton<TourDataViewModel>();
            services.AddSingleton(x => new TourDataViewModel(
                x.GetService<IRestService>()!,
                x.GetService<IMediator>()!,
                Configuration.RouteImagePath));
            services.AddSingleton(x => new ListToursViewModel(
                x.GetService<IDialogService>()!,
                x.GetService<IRestService>()!,
                x.GetService<IMediator>()!,
                Configuration.AppImagePath));
            services.AddSingleton(x => new NavigationViewModel(
                x.GetService<IDialogService>()!,
                x.GetService<IMediator>()!,
                x.GetService<IRestService>()!,
                Configuration.RouteImagePath));
            services.AddSingleton<TourLogsViewModel>();

            Log.Debug("Build ServiceProvider");
            _serviceProvider = services.BuildServiceProvider();
            _serviceProvider.GetService<IDialogService>()!.Register<AddTourViewModel, AddTourDialogWindow>();
            _serviceProvider.GetService<IDialogService>()!.Register<AddTourLogViewModel, AddTourLogDialogWindow>();
            _serviceProvider.GetService<IDialogService>()!.Register<EditTourViewModel, EditTourDialogWindow>();
            _serviceProvider.GetService<IDialogService>()!.Register<EditTourLogViewModel, EditTourLogDialogWindow>();
        }

        public ListToursViewModel ListToursViewModel => _serviceProvider.GetService<ListToursViewModel>()!;
        public TourDataViewModel TourDataViewModel => _serviceProvider.GetService<TourDataViewModel>()!;
        public NavigationViewModel NavigationViewModel => _serviceProvider.GetService<NavigationViewModel>()!;
        public TourLogsViewModel TourLogsViewModel => _serviceProvider.GetService<TourLogsViewModel>()!;
    }
}
