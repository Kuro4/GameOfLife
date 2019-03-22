using GameOfLife.Core.Models;
using GameOfLife.WPF.Core.EventAggregators;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameOfLife.WPF.BoardModule.ViewModels
{
    public class GPUBoardViewModel : BindableBase, INavigationAware
    {
        public ReactiveProperty<IBindableBoard> Board { get; private set; } = new ReactiveProperty<IBindableBoard>();
        private readonly IEventAggregator eventAggregator;

        public GPUBoardViewModel(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;

            this.eventAggregator.GetEvent<BoardInitializedEvent>().Subscribe(board =>
            {
                this.Board.Value = board;
            }, ThreadOption.PublisherThread);
            Console.WriteLine("GPUBoardViewModel");
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            var board = navigationContext.Parameters["board"] as IBindableBoard;
            this.ApplyBoard(board);
            this.eventAggregator.GetEvent<BoardInitializedEvent>().Subscribe(this.ApplyBoard);
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            this.eventAggregator.GetEvent<BoardInitializedEvent>().Unsubscribe(this.ApplyBoard);
        }

        private void ApplyBoard(IBindableBoard board)
        {
            if (board is null) return;
            this.Board.Value = board;
        }
    }
}
