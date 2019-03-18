using System;
using System.Collections.Generic;
using System.Text;

namespace GameOfLife.Core.Models
{
    /// <summary>
    /// <see cref="INotifyPropertyChanged"/>を実装した<see cref="ITorusableBoard"/>のラッパークラスのベース
    /// </summary>
    public abstract class BindableTorusableBoardBase<Type> : BindableBoardBase<Type> where Type : ITorusableBoard
    {
        private bool _isTorus;
        /// <summary>
        /// 平坦トーラスを適用するかどうか
        /// </summary>
        public bool IsTorus
        {
            get => this._isTorus;
            set
            {
                this.Board.IsTorus = value;
                this.SetProperty(ref this._isTorus, value);
            }
        }
        /// <summary>
        /// 周囲のセルを取得する
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        public virtual IEnumerable<ICell> GetAdjacentTorusCells(ICell cell)
        {
            return this.Board.GetAdjacentTorusCells(cell);
        }
    }
}
