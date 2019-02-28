using System.ComponentModel;

namespace GameOfLife.Core.Models
{
    public interface IBindableCell : ICell, INotifyPropertyChanged
    {
    }
}
