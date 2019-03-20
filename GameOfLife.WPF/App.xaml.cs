﻿using GameOfLife.WPF.BoardModule;
using GameOfLife.WPF.Views;
using Prism.Ioc;
using Prism.Modularity;
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
    }
}
//TODO:OpenTKを導入して爆速の実装を目指す