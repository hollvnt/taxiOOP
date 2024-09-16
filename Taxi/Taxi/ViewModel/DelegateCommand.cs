using System;
using System.Windows.Input;

namespace Taxi.ViewModel
{
    internal class DelegateCommand : ICommand
    {
        private Action search;

        public DelegateCommand(Action search)
        {
            this.search = search;
        }
    }
}