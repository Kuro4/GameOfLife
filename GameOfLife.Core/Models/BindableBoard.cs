using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife.Core.Models
{
    /// <summary>
    /// <see cref="INotifyPropertyChanged"/>を実装した<see cref="Board"/>のラッパークラス
    /// </summary>
    public class BindableBoard : BindableBoardBase<Board>
    {
        protected override Board CreateBoard()
        {
            return new Board();
        }
    }
}
