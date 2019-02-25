using System.Collections.Generic;

namespace GameOfLife
{
    public interface ICell : ILife
    {
        /// <summary>
        /// X座標
        /// </summary>
        int X { get; }
        /// <summary>
        /// Y座標
        /// </summary>
        int Y { get; }
        /// <summary>
        /// 次の世代で生きているか
        /// </summary>
        bool Future { get; }
        /// <summary>
        /// 次の世代での生死を決定する
        /// </summary>
        /// <param name="cells">周囲のセル</param>
        void Prediction(IEnumerable<ICell> cells);
        /// <summary>
        /// 次の世代へ進める
        /// </summary>
        void Next();
        /// <summary>
        /// 自身を初期化する
        /// </summary>
        void Initialize();
        /// <summary>
        /// 引数として指定した<see cref="ICell"/>の座標と自身の座標が一致するかどうかを返す
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        bool IsSamePosition(ICell cell);
    }
}
