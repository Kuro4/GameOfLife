using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace GameOfLife.Core.Models
{
    /// <summary>
    /// <see cref="INotifyPropertyChanged"/>を実装した<see cref="IBoard"/>のラッパークラスのベース
    /// </summary>
    public abstract class BindableBoardBase<Type> : BindableBase, IBindableBoard where Type : IBoard 
    {
        protected virtual Type Board { get; set; }
        /// <summary>
        /// 列数
        /// </summary>
        public int ColumnCount => this.Board.ColumnCount;
        /// <summary>
        /// 行数
        /// </summary>
        public int RowCount => this.Board.RowCount;
        /// <summary>
        /// 世代
        /// </summary>
        public int Generation => this.Board.Generation;
        /// <summary>
        /// セル
        /// </summary>
        private readonly ObservableCollection<IBindableCell> cells = new ObservableCollection<IBindableCell>();
        /// <summary>
        /// セル(読取専用)
        /// </summary>
        public ReadOnlyObservableCollection<IBindableCell> Cells { get; private set; }
        /// <summary>
        /// セルの生死状態を取得,設定する
        /// </summary>
        /// <param name="x">X座標</param>
        /// <param name="y">Y座標</param>
        /// <returns></returns>
        public virtual bool this[int x, int y]
        {
            get => this.Board[x, y];
            set => this.Board[x, y] = value;
        }
        /// <summary>
        /// <see cref="BindableBoardBase{Type}"/>のインスタンスを生成する
        /// </summary>
        public BindableBoardBase()
        {
            this.Cells = new ReadOnlyObservableCollection<IBindableCell>(this.cells);
            this.Board = this.CreateBoard();
        }
        /// <summary>
        /// <see cref="Type"/>型のインスタンスを生成する(このメソッドはコンストラクタで呼ばれ、<see cref="Board"/>プロパティにセットされる)
        /// </summary>
        /// <returns></returns>
        protected abstract Type CreateBoard();
        /// <summary>
        /// 盤を初期化する
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="columnCount">列数(0以下であれば1に矯正する)</param>
        /// <param name="rowCount">行数(0以下であれば1に矯正する)</param>
        /// <param name="generate">X,Y座標を引数に<typeparamref name="T"/>型のセルを生成するデリゲート</param>
        public virtual void Initialize<T>(int columnCount, int rowCount, Func<int, int, T> generate) where T : IBindableCell
        {
            this.Board.Initialize(columnCount, rowCount, generate);
            //this.cellsにはboard.cellsと同一のインスタンスを入れる為、this.boardの直接操作だけでOK
            this.cells.Clear();
            foreach (var cell in this.Board.Cells.Cast<T>())
            {
                this.cells.Add(cell);
            }
            this.RaisePropertyChanged(nameof(this.Generation));
        }
        /// <summary>
        /// 盤を初期化する
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="size">行列数(0以下であれば1に矯正する)</param>
        /// <param name="generate">X,Y座標を引数に<typeparamref name="T"/>型のセルを生成するデリゲート</param>
        public void Initialize<T>(int size, Func<int, int, T> generate) where T : IBindableCell
        {
            this.Initialize(size, size, generate);
        }
        /// <summary>
        /// 各セルの生死をランダムに変化させる
        /// </summary>
        /// <param name="survivalRate">生存率(省略可)</param>
        public virtual void Random(int survivalRate = 50)
        {
            this.Board.Random(survivalRate);
            this.RaisePropertyChanged(nameof(this.Generation));
        }
        /// <summary>
        /// 各セルの生死を反転させる
        /// </summary>
        public virtual void Reverse()
        {
            this.Board.Reverse();
        }
        /// <summary>
        /// 次の世代へ進める
        /// </summary>
        public virtual void Next()
        {
            this.Board.Next();
            this.RaisePropertyChanged(nameof(this.Generation));
        }
        /// <summary>
        /// 周囲のセルを取得する
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        public virtual IEnumerable<ICell> GetAdjacentCells(ICell cell)
        {
            return this.Board.GetAdjacentCells(cell);
        }
        /// <summary>
        /// 盤の状態をリセットする
        /// </summary>
        public virtual void Reset()
        {
            this.Board.Reset();
            this.RaisePropertyChanged(nameof(this.Generation));
        }
        /// <summary>
        /// 盤の状態を文字列に出力する
        /// </summary>
        /// <param name="aliveChar">生存セルとして表示する文字</param>
        /// <param name="deadChar">死滅セルとして表示する文字</param>
        /// <param name="separator">セル間に挿入する文字列</param>
        /// <returns></returns>
        public string ToString(char aliveChar = '0', char deadChar = '-', string separator = " ")
        {
            return this.Board.ToString(aliveChar, deadChar, separator);
        }
        #region IBoardインターフェイス実装の隠蔽
        IReadOnlyList<ICell> IBoard.Cells => this.Board.Cells;
        void IBoard.Initialize<T>(int columnCount, int rowCount, Func<int, int, T> generate) => this.Board.Initialize(columnCount, rowCount, generate);
        void IBoard.Initialize<T>(int size, Func<int, int, T> generate) => this.Board.Initialize(size, generate);
        #endregion
    }
}
