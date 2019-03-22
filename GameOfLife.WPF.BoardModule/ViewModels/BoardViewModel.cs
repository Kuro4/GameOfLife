using GameOfLife.Core.Models;
using GameOfLife.WPF.Core.EventAggregators;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Reactive.Bindings;
using System.Collections.ObjectModel;

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
            this.Cells.Value = null;
            this.columnCount.Value = 0;
            this.rowCount.Value = 0;
        }

        private void ApplyBoard(IBindableBoard board)
        {
            if (board is null) return;
            this.columnCount.Value = board.ColumnCount;
            this.rowCount.Value = board.RowCount;
            this.Cells.Value = board.Cells;
        }
    }
}