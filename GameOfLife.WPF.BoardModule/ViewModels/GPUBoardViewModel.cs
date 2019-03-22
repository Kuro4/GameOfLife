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
    public class GPUBoardViewModel : BindableBase
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
    }
}
