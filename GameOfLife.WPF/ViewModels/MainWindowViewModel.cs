using GameOfLife.WPF.Core.EventAggregators;
using GameOfLife.Core.Models;
using Prism.Events;
using Prism.Mvvm;
using Reactive.Bindings;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Reactive.Linq;
using System.Linq;

namespace GameOfLife.WPF.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "Prism Application";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public ReactiveProperty<int> Size { get; private set; } = new ReactiveProperty<int>(10);
        public ReactiveProperty<bool> IsTimeFlowing { get; private set; } = new ReactiveProperty<bool>(false);
        public ReactiveProperty<int> TimeSpeed { get; private set; } = new ReactiveProperty<int>(50);
        public ReactiveCommand Initialize { get; private set; } = new ReactiveCommand();
        public ReactiveCommand Random { get; private set; } = new ReactiveCommand();
        public ReactiveCommand Start { get; private set; } = new ReactiveCommand();
        public ReactiveCommand Stop { get; private set; } = new ReactiveCommand();
        public ReactiveCommand Next { get; private set; } = new ReactiveCommand();
        public ReactiveCommand Reset { get; private set; } = new ReactiveCommand();

        public IBindableBoard Board { get; private set; }

        private readonly IEventAggregator eventAggregator;

        public MainWindowViewModel(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;

            this.Board = new BindableTorusBoard();

            //実行条件
            this.Initialize = this.IsTimeFlowing.Select(x => !x).ToReactiveCommand();
            this.Random = this.IsTimeFlowing.Select(x => !x).ToReactiveCommand();
            this.Start = this.IsTimeFlowing.Select(x => !x).ToReactiveCommand();
            this.Stop = this.IsTimeFlowing.ToReactiveCommand();

            //実行処理
            this.Initialize.Subscribe(() =>
            {
                this.Board.Initialize(this.Size.Value, (x, y) => new BindableCell(x, y));
                this.eventAggregator.GetEvent<BoardInitializedEvent>().Publish(new BoradInfo()
                {
                    ColumnCount = this.Board.ColumnCount,
                    RowCount = this.Board.RowCount,
                    Cells = this.Board.Cells,
                });
                //DEBUG
                foreach (var cell in this.Board.Cells.Where(x => x.Y == (int)this.Board.RowCount / 2))
                {
                    cell.IsAlive = true;
                }
                foreach (var cell in this.Board.Cells.Where(x => x.X == (int)this.Board.ColumnCount / 2))
                {
                    cell.IsAlive = true;
                }
            });
            this.Random.Subscribe(() => this.Board.Random(20));
            this.Start.Subscribe(async () =>
            {
                this.IsTimeFlowing.Value = true;
                while (this.IsTimeFlowing.Value)
                {
                    this.Board.Next();
                    await Task.Delay(this.TimeSpeed.Value);
                }
            });
            this.Stop.Subscribe(() => this.IsTimeFlowing.Value = false);
            this.Next.Subscribe(() => this.Board.Next());
            this.Reset.Subscribe(() =>
            {
                this.IsTimeFlowing.Value = false;
                this.Board.Reset();
            });
        }
    }
}
