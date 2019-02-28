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

        public ReactiveCommand<IBindableCell> Reverse { get; private set; } = new ReactiveCommand<IBindableCell>();
        public ReactiveProperty<bool> IsMouseDown { get; private set; } = new ReactiveProperty<bool>(false);
        public ReactiveCommand<IBindableCell> MouseDown { get; private set; } = new ReactiveCommand<IBindableCell>();
        public ReactiveCommand MouseUp { get; private set; } = new ReactiveCommand();

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
                Console.WriteLine("Subscribed");
            },ThreadOption.PublisherThread);

            this.MouseDown.Subscribe(cell =>
            {
                this.IsMouseDown.Value = true;
                this.Reverse.Execute(cell);
            });
            this.MouseUp.Subscribe(() => this.IsMouseDown.Value = false);

            this.Reverse.Subscribe(cell =>
            {
                if (!this.IsMouseDown.Value) return;
                //cell.IsAlive = !cell.IsAlive;
                cell.IsAlive = true;
                Console.WriteLine($"X:{cell.X},Y:{cell.Y}");
            });
        }
    }
}
//TODO:現状では100*100=10000セル程度が限界(描画の問題)なので、ItemsSourceのItem数に合わせて自動的にItemsPanel自体を分割してもつことができるPanelコントロールを自作すると高速化できそう