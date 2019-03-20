using GameOfLife.Core.Models;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife.WPF.Core.EventAggregators
{
    public class BoardInitializedEvent : PubSubEvent<IBindableBoard>
    {
    }
}
