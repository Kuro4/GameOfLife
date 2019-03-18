using System;
using System.Collections.Generic;
using System.Text;

namespace GameOfLife
{
    public interface ITorusableBoard : IBoard
    {
        /// <summary>
        /// 平坦トーラスを適用するかどうか
        /// </summary>
        bool IsTorus { get; set; }
        /// <summary>
        /// 周囲のセルを取得する(平坦トーラスを適用)
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        IEnumerable<ICell> GetAdjacentTorusCells(ICell cell);
    }
}
