using System;
using System.Collections.Generic;
using System.Linq;

namespace GameOfLife
{
    /// <summary>
    /// 平坦トーラスを適用したライフゲームの盤面
    /// </summary>
    public class TorusBoard : Board
    {
        public override IEnumerable<ICell> GetSurroundingCells(ICell targetCell)
        {
            var index = targetCell.X + (this.ColumnCount * targetCell.Y);
            var col = this.ColumnCount - 1;
            var row = this.RowCount - 1;
            yield return 0 < targetCell.Y && 0 < targetCell.X ? this.cells[index - this.ColumnCount - 1] : this.cells.Last();
            yield return 0 < targetCell.Y ? this.cells[index - this.ColumnCount] : this.cells[targetCell.X + this.ColumnCount * row];
            yield return 0 < targetCell.Y && targetCell.X < col ? this.cells[index - this.ColumnCount + 1] : this.cells[this.ColumnCount * row];
            yield return 0 < targetCell.X ? this.Cells[index - 1] : this.cells[index + col];
            yield return targetCell.X < col ? this.Cells[index + 1] : this.cells[index - col];
            yield return targetCell.Y < row && 0 < targetCell.X ? this.cells[index + this.ColumnCount - 1] : this.cells[col];
            yield return targetCell.Y < row ? this.cells[index + this.ColumnCount] : this.cells[targetCell.X];
            yield return targetCell.Y < row && targetCell.X < col ? this.cells[index + this.ColumnCount + 1] : this.cells.First();
        }
    }
}