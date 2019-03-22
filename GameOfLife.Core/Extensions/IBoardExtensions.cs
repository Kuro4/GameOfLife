using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameOfLife.Core.Extensions
{
    public static class IBoardExtensions
    {
        /// <summary>
        /// <see cref="IBoard"/>のY軸の横方向を生存セルにする
        /// </summary>
        /// <param name="board"></param>
        /// <param name="rows">生存セルにする行インデックス(指定しなければ中央)</param>
        public static void WriteHorizontalLine(this IBoard board, params int[] rows)
        {
            if (!rows.Any())
            {
                board.WriteHorizontalCenterLine();
                return;
            }
            board.Edit(() =>
            {
                foreach (var row in rows)
                {
                    for (int column = 0; column < board.ColumnCount; column++)
                    {
                        board[column, row] = true;
                    }
                }
            });
        }
        /// <summary>
        /// <see cref="IBoard"/>のY軸の中央横方向を生存セルにする
        /// </summary>
        /// <param name="board"></param>
        public static void WriteHorizontalCenterLine(this IBoard board)
        {
            board.Edit(() =>
            {
                int rowCenter = board.RowCount / 2;
                var cells = board.RowCount % 2 == 1 ?
                    board.Cells.Where(cell => cell.Y == rowCenter) :
                    board.Cells.Where(cell => rowCenter - 1 <= cell.Y && cell.Y <= rowCenter);
                cells.AsParallel().ForAll(cell => cell.IsAlive = true);
            });
        }
        /// <summary>
        /// <see cref="IBoard"/>のX軸の縦方向を生存セルにする
        /// </summary>
        /// <param name="board"></param>
        /// <param name="columns">生存セルにする列インデックス(指定しなければ中央)</param>
        public static void WriteVerticalLine(this IBoard board, params int[] columns)
        {
            if (!columns.Any())
            {
                board.WriteVerticalCenterLine();
                return;
            }
            board.Edit(() =>
            {
                foreach (var column in columns)
                {
                    for (int row = 0; row < board.RowCount; row++)
                    {
                        board[column, row] = true;
                    }
                }
            });
        }
        /// <summary>
        /// <see cref="IBoard"/>のX軸の中央縦方向を生存セルにする
        /// </summary>
        /// <param name="board"></param>
        public static void WriteVerticalCenterLine(this IBoard board)
        {
            board.Edit(() =>
            {
                int columnCenter = board.ColumnCount / 2;
                var cells = board.ColumnCount % 2 == 1 ?
                    board.Cells.Where(cell => cell.X == columnCenter) :
                    board.Cells.Where(cell => columnCenter - 1 <= cell.X && cell.X <= columnCenter);
                cells.AsParallel().ForAll(cell => cell.IsAlive = true);
            });
        }
        /// <summary>
        /// <see cref="IBoard"/>のX,Y軸の中央縦横方向を生存セルにする
        /// </summary>
        /// <param name="board"></param>
        public static void WriteCross(this IBoard board)
        {
            board.WriteHorizontalCenterLine();
            board.WriteVerticalCenterLine();
        }

        public static void WriteHorizontalStripe(this IBoard board, int span = 1)
        {
            var s = span < 1 ? 1 : span + 1;
            board.WriteHorizontalLine(Enumerable.Range(0, board.RowCount).Where(x => x % s == 0).ToArray());
        }

        public static void WriteVerticalStripe(this IBoard board, int span = 1)
        {
            var s = span < 1 ? 1 : span + 1;
            board.WriteVerticalLine(Enumerable.Range(0, board.ColumnCount).Where(x => x % s == 0).ToArray());
        }

        public static void WriteGinghamCheck(this IBoard board, int span = 1)
        {
            board.WriteHorizontalStripe(span);
            board.WriteVerticalStripe(span);
        }
    }
}