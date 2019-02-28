using GameOfLife.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameOfLife.Core.Models
{
    /// <summary>
    /// <see cref="INotifyPropertyChanged"/>を実装した<see cref="TorusBoard"/>のラッパークラス
    /// </summary>
    public class BindableTorusBoard : BindableBoardBase<TorusBoard>
    {
        protected override TorusBoard CreateBoard()
        {
            return new TorusBoard();
        }
    }
}
