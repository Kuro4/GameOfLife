using GameOfLife.Core.Models;
using GameOfLife.WPF.Core.EventAggregators;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Unity.Attributes;

namespace GameOfLife.WPF.BoardModule.ViewModels
{
    [RegionMemberLifetime(KeepAlive = false)]
    public class BoardViewModel : BindableBase, INavigationAware
    {
        private readonly IEventAggregator eventAggregator;

        private readonly ReactiveProperty<int> columnCount = new ReactiveProperty<int>();
        public ReadOnlyReactiveProperty<int> ColumnCount { get; private set; }
        private readonly ReactiveProperty<int> rowCount = new ReactiveProperty<int>();
        public ReadOnlyReactiveProperty<int> RowCount { get; private set; }

        public ReactiveProperty<ReadOnlyObservableCollection<IBindableCell>> Cells { get; private set; } = new ReactiveProperty<ReadOnlyObservableCollection<IBindableCell>>();

        public BoardViewModel(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            this.ColumnCount = this.columnCount.ToReadOnlyReactiveProperty();
            this.RowCount = this.rowCount.ToReadOnlyReactiveProperty();
            Console.WriteLine("BoardViewModel");
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            Console.WriteLine("OnNavigatedTo");
            var board = navigationContext.Parameters["board"] as IBindableBoard;
            this.columnCount.Value = board.ColumnCount;
            this.rowCount.Value = board.RowCount;
            this.Cells.Value = board.Cells;
            this.eventAggregator.GetEvent<BoardInitializedEvent>().Subscribe(this.ApplyBoard);
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            Console.WriteLine("IsNavigationTarget");
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            Console.WriteLine("OnNavigatedFrom");
            this.eventAggregator.GetEvent<BoardInitializedEvent>().Unsubscribe(this.ApplyBoard);
        }

        private void ApplyBoard(IBindableBoard board)
        {
            this.columnCount.Value = board.ColumnCount;
            this.rowCount.Value = board.RowCount;
            this.Cells.Value = board.Cells;
            Console.WriteLine("BoardViewModelSubscribe");
        }
    }
}