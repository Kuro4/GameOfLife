using GameOfLife.WPF.Core.EventAggregators;
using GameOfLife.Core.Models;
using Prism.Events;
using Prism.Mvvm;
using Reactive.Bindings;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Reactive.Linq;
using System.Linq;
using GameOfLife.WPF.BoardModule.Models;
using System;
using Reactive.Bindings.Extensions;
using System.Reactive.Disposables;
using GameOfLife.Core.Extensions;
using Prism.Regions;
using Unity.Attributes;

namespace GameOfLife.WPF.ViewModels
{
    public class MainWindowViewModel : BindableBase, IDisposable
    {
        private string _title = "Prism Application";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public ReactiveProperty<int> Size { get; private set; } = new ReactiveProperty<int>(10);
        public ReactiveProperty<bool> IsStarting { get; private set; } = new ReactiveProperty<bool>(false);
        public ReactiveProperty<int> FPS { get; private set; } = new ReactiveProperty<int>(60);

        public ReactiveProperty<int> Columns { get; } = new ReactiveProperty<int>(10);
        public ReactiveProperty<int> Rows { get; } = new ReactiveProperty<int>(10);
        public ReactiveProperty<string> StartStopLabel { get; } = new ReactiveProperty<string>("Start");
        public ReactiveProperty<BoardType> SelectedBoardType { get; private set; } = new ReactiveProperty<BoardType>(BoardType.PureWPF);
        public ReactiveProperty<bool> IsTorus { get; private set; }
        public ReactiveProperty<Pattern> SelectedPattern { get; private set; } = new ReactiveProperty<Pattern>(Pattern.Random);
        public ReactiveProperty<int> Generation { get; private set; } = new ReactiveProperty<int>();

        public ReactiveCommand Initialize { get; private set; } = new ReactiveCommand();

        public ReactiveCommand SwitchStartStop { get; private set; } = new ReactiveCommand();
        public ReactiveCommand<Pattern> Edit { get; } = new ReactiveCommand<Pattern>();

        public ReactiveCommand Start { get; private set; } = new ReactiveCommand();
        public ReactiveCommand Next { get; private set; } = new ReactiveCommand();

        private BindableTorusableBoard Board { get; } = new BindableTorusableBoard(true);
        private CompositeDisposable Disposable { get; } = new CompositeDisposable();
        private readonly IRegionManager regionManager;
        private readonly IEventAggregator eventAggregator;

        public MainWindowViewModel(IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            this.regionManager = regionManager;
            this.eventAggregator = eventAggregator;
            //BindableTorusableBoardのプロパティと接続
            this.IsTorus = this.Board.ObserveProperty(x => x.IsTorus)
                .ToReactiveProperty()
                .AddTo(this.Disposable);
            this.Generation = this.Board.ObserveProperty(x => x.Generation)
                .ToReactiveProperty()
                .AddTo(this.Disposable);

            this.Board.Initialize(50, (x, y) => new BindableCell(x, y));

            //実行条件
            this.Initialize = this.IsStarting.Select(x => !x).ToReactiveCommand();

            //実行処理
            this.Initialize.Subscribe(() =>
            {
                this.Board.Initialize(this.Columns.Value, this.Rows.Value, (x, y) => new BindableCell(x, y));
                this.Edit.Execute(this.SelectedPattern.Value);
                //this.ChangeBoardType(BoardType.PureWPF);
                this.eventAggregator.GetEvent<BoardInitializedEvent>().Publish(this.Board);
            });

            this.SelectedBoardType.Subscribe(x => this.ChangeBoardType(x));

            this.SwitchStartStop.Subscribe(async () =>
            {
                this.IsStarting.Value = !this.IsStarting.Value;
                this.StartStopLabel.Value = this.IsStarting.Value ? "Stop" : "Start";
                while (this.IsStarting.Value)
                {
                    this.Board.Next();
                    await Task.Delay(this.FPS.Value);
                }
            });

            this.Edit.Subscribe(x =>
            {
                this.Board.Reset();
                switch (x)
                {
                    case Pattern.Random:
                        this.Board.Random(20);
                        break;
                    case Pattern.HorizontalCenterLine:
                        this.Board.WriteHorizontalCenterLine();
                        break;
                    case Pattern.VerticalCenterLine:
                        this.Board.WriteVerticalCenterLine();
                        break;
                    case Pattern.Cross:
                        this.Board.WriteCross();
                        break;
                    case Pattern.HorizontalStripe:
                        this.Board.WriteHorizontalStripe(this.Board.RowCount / 50);
                        break;
                    case Pattern.VerticalStripe:
                        this.Board.WriteVerticalStripe(this.Board.ColumnCount / 50);
                        break;
                    case Pattern.GinghamCheck:
                        this.Board.WriteGinghamCheck(this.Board.RowCount / 50);
                        break;
                    default:
                        break;
                }
            });

            this.Next.Subscribe(() => this.Board.Next());
        }

        /// <summary>
        /// Boardの種類を変更する
        /// </summary>
        /// <param name="boardType"></param>
        private void ChangeBoardType(BoardType boardType)
        {
            var param = new NavigationParameters()
            {
                { "board", this.Board }
            };
            switch (boardType)
            {
                case BoardType.PureWPF:
                    //if (10000 < this.Board.Cells.Count)
                    //{
                    //    this.SelectedBoardType.Value = BoardType.OpenTK;
                    //    return;
                    //}
                    this.regionManager.RequestNavigate("ContentRegion", nameof(BoardModule.Views.Board), param);
                    this.eventAggregator.GetEvent<BoardInitializedEvent>().Publish(this.Board);
                    break;
                case BoardType.OpenTK:
                    this.regionManager.RequestNavigate("ContentRegion", nameof(BoardModule.Views.GPUBoard), param);
                    //this.eventAggregator.GetEvent<BoardInitializedEvent>().Publish(this.Board);
                    break;
                default:
                    break;
            }
        }

        public void Dispose()
        {
            this.Disposable.Dispose();
        }
    }
}
