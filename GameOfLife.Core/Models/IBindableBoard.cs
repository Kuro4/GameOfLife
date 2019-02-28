using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace GameOfLife.Core.Models
{
    public interface IBindableBoard : IBoard, INotifyPropertyChanged
    {
        new ReadOnlyObservableCollection<IBindableCell> Cells { get; }
        new void Initialize<T>(int columnCount, int rowCount, Func<int, int, T> generate) where T : IBindableCell;
        new void Initialize<T>(int size, Func<int, int, T> generate) where T : IBindableCell;
    }
}
