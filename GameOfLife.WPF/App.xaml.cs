using GameOfLife.WPF.BoardModule;
using GameOfLife.WPF.Views;
using Prism.Ioc;
using Prism.Modularity;
using System;
using System.Windows;

namespace GameOfLife.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {

        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<BoardModule.BoardModule>();
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            var viewModel = this.Container.Resolve<ViewModels.MainWindowViewModel>();
            viewModel.SelectedBoardType.Value = BoardModule.Models.BoardType.PureWPF;
            Console.WriteLine("OnInitialized");
        }

        protected override void OnDeactivated(EventArgs e)
        {
            //ViewModelのDispose
            foreach (Window window in this.Windows)
            {
                if (window.DataContext is IDisposable disposable) disposable.Dispose();
            }
            base.OnDeactivated(e);
        }
    }
}
//TODO:RequestNavigateした際のViewModelの参照残りを修正する