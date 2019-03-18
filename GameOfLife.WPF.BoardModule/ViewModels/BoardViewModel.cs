using GameOfLife.Core.Models;
using GameOfLife.WPF.Core.EventAggregators;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Unity.Attributes;

namespace GameOfLife.WPF.BoardModule.ViewModels
{
    public class BoardViewModel : BindableBase
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

            this.eventAggregator.GetEvent<BoardInitializedEvent>().Subscribe(info =>
            {
                this.columnCount.Value = info.ColumnCount;
                this.rowCount.Value = info.RowCount;
                this.Cells.Value = info.Cells;
            },ThreadOption.PublisherThread);
        }
    }
}