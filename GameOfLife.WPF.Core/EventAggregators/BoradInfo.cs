using GameOfLife.Core.Models;
using System.Collections.ObjectModel;

namespace GameOfLife.WPF.Core.EventAggregators
{
    public class BoradInfo
    {
        public int ColumnCount { get; set; }
        public int RowCount { get; set; }
        public ReadOnlyObservableCollection<IBindableCell> Cells { get; set; }
    }
}
