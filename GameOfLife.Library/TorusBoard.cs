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
        protected override IEnumerable<ICell> GetSurroundingCells(ICell targetCell)
        {
            return this.cells.Where(cell =>
            {
                if (targetCell.IsSamePosition(cell)) return false;
                var xDiff = Math.Abs(cell.X - targetCell.X);
                var yDiff = Math.Abs(cell.Y - targetCell.Y);
                return (xDiff <= 1 || xDiff == (this.ColumnCount - 1)) && (yDiff <= 1 || yDiff == (this.RowCount - 1));
            });
        }
    }
}