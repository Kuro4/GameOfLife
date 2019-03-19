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
        public ReactiveProperty<int> FPS { get; private set; } = new ReactiveProperty<int>(60);
        public ReactiveCommand Initialize { get; private set; } = new ReactiveCommand();
        public ReactiveCommand Random { get; private set; } = new ReactiveCommand();
        public ReactiveCommand Start { get; private set; } = new ReactiveCommand();
        public ReactiveCommand Stop { get; private set; } = new ReactiveCommand();
        public ReactiveCommand Next { get; private set; } = new ReactiveCommand();
        public ReactiveCommand Reset { get; private set; } = new ReactiveCommand();

        public BindableTorusableBoard Board { get; private set; }

        private readonly IEventAggregator eventAggregator;

        public MainWindowViewModel(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;

            this.Board = new BindableTorusableBoard(true);

            //実行条件
            this.Initialize = this.IsTimeFlowing.Select(x => !x).ToReactiveCommand();
            this.Random = this.IsTimeFlowing.Select(x => !x).ToReactiveCommand();
            this.Start = this.IsTimeFlowing.Select(x => !x).ToReactiveCommand();
            this.Stop = this.IsTimeFlowing.ToReactiveCommand();

            //実行処理
            this.Initialize.Subscribe(() =>
            {
                this.Board.Initialize(this.Size.Value, (x, y) => new BindableCell(x, y));
                //DEBUG
                this.Board.Edit(() =>
                {
                    var row = this.Board.RowCount / 2;
                    foreach (var cell in this.Board.Cells.Where(x => x.Y == row))
                    {
                        cell.IsAlive = true;
                    }
                });
                this.eventAggregator.GetEvent<BoardInitializedEvent>().Publish(this.Board);
            });
            this.Random.Subscribe(() => this.Board.Random(20));
            this.Start.Subscribe(async () =>
            {
                this.IsTimeFlowing.Value = true;
                while (this.IsTimeFlowing.Value)
                {
                    this.Board.Next();
                    await Task.Delay(System.TimeSpan.FromMilliseconds(1000 / this.FPS.Value));
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
