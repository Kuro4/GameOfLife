using GameOfLife.WPF.BoardModule.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using Unity.Attributes;

namespace GameOfLife.WPF.BoardModule
{
    public class BoardModule : IModule
    {
        [Dependency]
        public IRegionManager RegionManager { get; set; }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<Views.Board>();
            containerRegistry.RegisterForNavigation<GPUBoard>();
        }
    }
}