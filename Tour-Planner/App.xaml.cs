using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using log4net;
using Tour_Planner.Services;

namespace Tour_Planner
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /*
         * We see that the Images and Resources folders reside in the startup project.
         * While this is customary, they can technically reside in any project or even in their own project.
         * However, I prefer to keep them in this project because it provides a marginal performance benefit.
         * Typically, when using MVVM, the only other files in the startup project will be the MainWindow.xaml, App.xaml (and their constituent code behind files), and app.config files.
         * The Images folder contains the images and icons that are displayed in the UI controls,
         * whereas the Resources folder normally contains any resource files, such as XML schemas or text or data files that are used by the application.
         */
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod()!.DeclaringType);

        public App()
        {
            // todo test
            Log.Info("Starting app and RestService");
            RestService rest = new RestService();
        }
    }
}
