using System;
using System.Collections.Generic;
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
using WpfAnimatedGif;

namespace Ascend.MVVM.View
{
    public partial class NewsView : UserControl
    {
        public NewsView()
        {
            InitializeComponent();

            /*var gifPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Gifs", "TestGif1.gif");
            var gifUri = new Uri(gifPath, UriKind.Absolute);
            var image = new BitmapImage(gifUri);

            ImageBehavior.SetAnimatedSource(AnimatedGif, image);*/

            var gifPath2 = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Gifs", "NewsFrame.gif");
            var gifUri2 = new Uri(gifPath2, UriKind.Absolute);
            var image2 = new BitmapImage(gifUri2);

            ImageBehavior.SetAnimatedSource(AnimatedGif2, image2);
        }
    }
}
