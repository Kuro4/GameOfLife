using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameOfLife
{
    /// <summary>
    /// ライフゲームの盤面
    /// </summary>
    public class Board : IBoard
    {
        /// <summary>
        /// 列数
        /// </summary>
        public int ColumnCount { get; private set; }
        /// <summary>
        /// 行数
        /// </summary>
        public int RowCount { get; private set; }
        /// <summary>
        /// 世代
        /// </summary>
        public int Generation { get; private set; }
        /// <summary>
        /// セル
        /// </summary>
        protected readonly List<ICell> cells = new List<ICell>();
        /// <summary>
        /// セル(読取専用)
        /// </summary>
        public IReadOnlyList<ICell> Cells { get; }
        /// <summary>
        /// セルの生死状態を取得,設定する
        /// </summary>
        /// <param name="x">X座標</param>
        /// <param name="y">Y座標</param>
        /// <returns></returns>
        public bool this[int x, int y]
        {
            get{ return this.cells[x + (this.ColumnCount * y)].IsAlive; }
            set { this.cells[x + (this.ColumnCount * y)].IsAlive = value; }
        }
        /// <summary>
        /// <see cref="Board"/>のインスタンスを生成する
        /// </summary>
        public Board()
        {
            this.Cells = this.cells.AsReadOnly();
        }
        /// <summary>
        /// 盤を初期化する
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="columnCount">列数(0以下であれば1に矯正する)</param>
        /// <param name="rowCount">行数(0以下であれば1に矯正する)</param>
        /// <param name="generate">X,Y座標を引数に<typeparamref name="T"/>型のセルを生成するデリゲート</param>
        public virtual void Initialize<T>(int columnCount, int rowCount, Func<int, int, T> generate) where T : ICell
        {
            this.ColumnCount = columnCount <= 0 ? 1 : columnCount;
            this.RowCount = rowCount <= 0 ? 1 : rowCount;
            this.Generation = 0;
            this.cells.Clear();
            for (int i = 0; i < this.RowCount; i++)
            {
                for (int j = 0; j < this.ColumnCount; j++)
                {
                    this.cells.Add(generate(j, i));
                }
            }
        }
        /// <summary>
        /// 盤を初期化する
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="size">行列数(0以下であれば1に矯正する)</param>
        /// <param name="generate">X,Y座標を引数に<typeparamref name="T"/>型のセルを生成するデリゲート</param>
        public void Initialize<T>(int size, Func<int, int, T> generate) where T : ICell
        {
            this.Initialize<T>(size, size, generate);
        }
        /// <summary>
        /// 各セルの生死をランダムに変化させる
        /// </summary>
        /// <param name="survivalRate">生存率(省略可)</param>
        public virtual void Random(int survivalRate = 50)
        {
            if (!this.Cells.Any()) return;
            var random = new Random();
            this.cells.ForEach(cell => cell.IsAlive = random.Next(0, 100) < survivalRate);
            this.Generation = 0;
        }
        /// <summary>
        /// 各セルの生死を反転させる
        /// </summary>
        public virtual void Reverse()
        {
            if (!this.Cells.Any()) return;
            this.cells.ForEach(cell => cell.IsAlive = !cell.IsAlive);
        }
        /// <summary>
        /// 次の世代へ進める
        /// </summary>
        public virtual void Next()
        {
            if (!this.Cells.Any()) return;
            var parallel = this.cells.AsParallel();
            parallel.ForAll(cell => cell.Prediction(this.GetSurroundingCells(cell)));
            parallel.ForAll(cell => cell.Next());
            this.Generation++;
        }
        /// <summary>
        /// 周囲のセルを取得する
        /// </summary>
        /// <param name="targetCell"></param>
        /// <returns></returns>
        public virtual IEnumerable<ICell> GetSurroundingCells(ICell targetCell)
        {
            //軽量化のためWhereを使わないで実装
            var index = targetCell.X + (this.ColumnCount * targetCell.Y);
            var col = this.ColumnCount - 1;
            var row = this.RowCount - 1;
            //左上
            if (0 < targetCell.Y && 0 < targetCell.X) yield return this.cells[index - this.ColumnCount - 1];
            //上
            if (0 < targetCell.Y) yield return this.cells[index - this.ColumnCount];
            //右上
            if (0 < targetCell.Y && targetCell.X < col) yield return this.cells[index - this.ColumnCount + 1];
            //左
            if (0 < targetCell.X) yield return this.Cells[index - 1];
            //右
            if (targetCell.X < col) yield return this.Cells[index + 1];
            //左下
            if (targetCell.Y < row && 0 < targetCell.X) yield return this.cells[index + this.ColumnCount - 1];
            //下
            if (targetCell.Y < row) yield return this.cells[index + this.ColumnCount];
            //右下
            if (targetCell.Y < row && targetCell.X < col) yield return this.cells[index + this.ColumnCount + 1];
        }
        /// <summary>
        /// 盤の状態をリセットする
        /// </summary>
        public virtual void Reset()
        {
            this.cells.ForEach(cell => cell.IsAlive = false);
            this.Generation = 0;
        }
        /// <summary>
        /// 盤の状態を文字列に出力する
        /// </summary>
        /// <param name="aliveChar">生存セルとして表示する文字</param>
        /// <param name="deadChar">死滅セルとして表示する文字</param>
        /// <param name="separator">セル間に挿入する文字列</param>
        /// <returns></returns>
        public virtual string ToString(char aliveChar = '0', char deadChar = '-', string separator = " ")
        {
            if (!this.Cells.Any()) return "Uninitialized";
            var conv = this.cells.Select(cell => cell.IsAlive ? aliveChar : deadChar);
            var builder = new StringBuilder();
            for (int i = 0; i < this.RowCount; i++)
            {
                builder.AppendLine(string.Join(separator, conv.Skip(i * this.ColumnCount).Take(this.ColumnCount)));
            }
            return builder.ToString();
        }
    }
}
