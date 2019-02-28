using System;
using System.Collections.Generic;
using System.Text;

namespace GameOfLife
{
    public interface IBoard
    {
        /// <summary>
        /// 列数
        /// </summary>
        int ColumnCount { get; }
        /// <summary>
        /// 行数
        /// </summary>
        int RowCount { get; }
        /// <summary>
        /// 世代
        /// </summary>
        int Generation { get; }
        /// <summary>
        /// セル
        /// </summary>
        IReadOnlyList<ICell> Cells { get; }
        /// <summary>
        /// セルの生死状態を取得,設定する
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        bool this[int x, int y] { get; set; }
        /// <summary>
        /// 盤を初期化する
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="columnCount">列数</param>
        /// <param name="rowCount">行数</param>
        /// <param name="generate">X,Y座標を引数に<typeparamref name="T"/>型のセルを生成するデリゲート</param>
        void Initialize<T>(int columnCount, int rowCount, Func<int, int, T> generate) where T : ICell;
        /// <summary>
        /// 盤を初期化する
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="size">行列数</param>
        /// <param name="generate">X,Y座標を引数に<typeparamref name="T"/>型のセルを生成するデリゲート</param>
        void Initialize<T>(int size, Func<int, int, T> generate) where T : ICell;
        /// <summary>
        /// 各セルの生死をランダムに変化させる
        /// </summary>
        /// <param name="survivalRate">生存率</param>
        void Random(int survivalRate = 50);
        /// <summary>
        /// 各セルの生死を反転させる
        /// </summary>
        void Reverse();
        /// <summary>
        /// 次の世代へ進める
        /// </summary>
        void Next();
        /// <summary>
        /// 周囲のセルを取得する
        /// </summary>
        /// <param name="targetCell"></param>
        /// <returns></returns>
        IEnumerable<ICell> GetSurroundingCells(ICell targetCell);
        /// <summary>
        /// 盤の状態をリセットする
        /// </summary>
        void Reset();
    }
}
