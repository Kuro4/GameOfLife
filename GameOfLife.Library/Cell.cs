using System.Collections.Generic;
using System.Linq;

namespace GameOfLife
{
    public class Cell : ICell
    {
        /// <summary>
        /// X座標
        /// </summary>
        public int X { get; }
        /// <summary>
        /// Y座標
        /// </summary>
        public int Y { get; }
        /// <summary>
        /// 生きているか
        /// </summary>
        public bool IsAlive { get; set; }
        /// <summary>
        /// 次の世代で生きているか
        /// </summary>
        public bool Future { get; private set; }
        /// <summary>
        /// x,y座標を指定して<see cref="Cell"/>を生成する
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public Cell(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
        /// <summary>
        /// 次の世代での生死を決定する
        /// </summary>
        /// <param name="cells"></param>
        public virtual void Prediction(IEnumerable<ICell> cells)
        {
            var aliveCount = cells.Where(x => x.IsAlive).Count();
            this.Future = this.IsAlive ? (2 <= aliveCount && aliveCount <= 3) : aliveCount == 3;
        }
        /// <summary>
        /// 次の世代へ進める
        /// </summary>
        public virtual void Next()
        {
            this.IsAlive = this.Future;
        }
        /// <summary>
        /// 自身を初期化する
        /// </summary>
        public virtual void Initialize()
        {
            this.IsAlive = false;
            this.Future = false;
        }
        /// <summary>
        /// 引数として指定した<see cref="ICell"/>の座標と自身の座標が一致するかどうかを返す
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        public virtual bool IsSamePosition(ICell cell)
        {
            return this.X == cell.X && this.Y == cell.Y;
        }
    }
}
