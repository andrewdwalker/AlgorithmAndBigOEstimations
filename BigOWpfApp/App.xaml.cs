using BigOWpfApp.ViewModels;
using BigOWpfApp.Views;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace BigOWpfApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            
            SimpleView app = new SimpleView();
            ViewModel context = new ViewModel();
            app.DataContext = context;
            app.Show();
        }
    }
}
