using System;
using System.Collections.Generic;
using System.Linq;

namespace GameOfLife
{
    /// <summary>
    /// 平坦トーラスを適用することができるライフゲームの盤面
    /// </summary>
    public class TorusableBoard : Board, ITorusableBoard
    {
        /// <summary>
        /// 平坦トーラスを適用するかどうか
        /// </summary>
        public bool IsTorus { get; set; } = false;
        /// <summary>
        /// 次の世代へ進める
        /// </summary>
        public override void Next()
        {
            if (!this.Cells.Any()) return;
            var parallel = this.cells.AsParallel();
            parallel.ForAll(cell => cell.Prediction(this.IsTorus ? this.GetAdjacentTorusCells(cell) : this.GetAdjacentCells(cell)));
            parallel.ForAll(cell => cell.Next());
            this.Generation++;
        }
        /// <summary>
        /// 周囲のセルを取得する(平坦トーラスを適用)
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        public IEnumerable<ICell> GetAdjacentTorusCells(ICell cell)
        {
            var index = cell.X + (this.ColumnCount * cell.Y);
            var col = this.ColumnCount - 1;
            var row = this.RowCount - 1;
            yield return 0 < cell.Y && 0 < cell.X ? this.cells[index - this.ColumnCount - 1] : this.cells.Last();
            yield return 0 < cell.Y ? this.cells[index - this.ColumnCount] : this.cells[cell.X + this.ColumnCount * row];
            yield return 0 < cell.Y && cell.X < col ? this.cells[index - this.ColumnCount + 1] : this.cells[this.ColumnCount * row];
            yield return 0 < cell.X ? this.Cells[index - 1] : this.cells[index + col];
            yield return cell.X < col ? this.Cells[index + 1] : this.cells[index - col];
            yield return cell.Y < row && 0 < cell.X ? this.cells[index + this.ColumnCount - 1] : this.cells[col];
            yield return cell.Y < row ? this.cells[index + this.ColumnCount] : this.cells[cell.X];
            yield return cell.Y < row && cell.X < col ? this.cells[index + this.ColumnCount + 1] : this.cells.First();
        }
    }
}