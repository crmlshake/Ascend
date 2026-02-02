using Ascend.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Ascend.MVVM.View
{
    public partial class TodosView : UserControl
    {
        public TodosView()
        {
            InitializeComponent();
            DateTextBlock.Text = FormatAmericanDate(DateTime.Now);
            DataContext = new TodoViewModel();
        }

        private string FormatAmericanDate(DateTime date)
        {
            string daySuffix = GetDaySuffix(date.Day);
            return date.ToString($"MMMM d'{daySuffix}' yyyy", CultureInfo.InvariantCulture);
        }

        private string GetDaySuffix(int day)
        {
            return (day % 10 == 1 && day != 11) ? "st" :
                   (day % 10 == 2 && day != 12) ? "nd" :
                   (day % 10 == 3 && day != 13) ? "rd" : "th";
        }
    }
}
