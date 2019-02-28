using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife.Core.Models
{
    /// <summary>
    /// <see cref="INotifyPropertyChanged"/>を実装した<see cref="Cell"/>のラッパークラス
    /// </summary>
    public class BindableCell : BindableBase, IBindableCell
    {
        private readonly Cell cell;
        /// <summary>
        /// X座標
        /// </summary>
        public int X => this.cell.X;
        /// <summary>
        /// Y座標
        /// </summary>
        public int Y => this.cell.Y;
        private bool _isAlive;
        /// <summary>
        /// 生きているか
        /// </summary>
        public bool IsAlive
        {
            get => _isAlive;
            set
            {
                this.cell.IsAlive = value;
                SetProperty(ref _isAlive, value);
            }
        }
        /// <summary>
        /// 次の世代で生きているか
        /// </summary>
        public bool Future => this.cell.Future;

        /// <summary>
        /// x,y座標を指定して<see cref="Cell"/>を生成する
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public BindableCell(int x, int y)
        {
            this.cell = new Cell(x, y);
        }
        /// <summary>
        /// 次の世代での生死を決定する
        /// </summary>
        /// <param name="cells"></param>
        public virtual void Prediction(IEnumerable<ICell> cells)
        {
            this.cell.Prediction(cells);
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
            this.cell.Initialize();
            this.IsAlive = false;
        }
        /// <summary>
        /// 引数として指定した<see cref="ICell"/>の座標と自身の座標が一致するかどうかを返す
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        public virtual bool IsSamePosition(ICell cell)
        {
            return this.cell.IsSamePosition(cell);
        }
    }
}
