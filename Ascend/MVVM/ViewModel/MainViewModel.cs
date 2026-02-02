using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ascend.MVVM.ViewModel
{
    public partial class MainViewModel
    {
        public string Greeting { get; }

        public MainViewModel()
        {
            int hour = DateTime.Now.Hour;

            if (hour >= 5 && hour < 12)
                Greeting = "Good Morning!";
            else if (hour >= 12 && hour < 18)
                Greeting = "Good Afternoon!";
            else
                Greeting = "Good Evening!";
        }
    }
}
