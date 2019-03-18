using System;
using System.Collections.Generic;
using System.Text;

namespace GameOfLife.Core.Models
{
    public class BindableTorusableBoard : BindableTorusableBoardBase<TorusableBoard>
    {
        public BindableTorusableBoard()
        {
        }
        public BindableTorusableBoard(bool isTorus)
        {
            this.IsTorus = isTorus;
        }
        protected override TorusableBoard CreateBoard()
        {
            return new TorusableBoard();
        }
    }
}
