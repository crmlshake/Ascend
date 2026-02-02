using System;
using System.Windows;

namespace Ascend.Helpers
{
    public partial class ThemeManager
    {
        private static readonly Uri WhiteModeUri = new Uri("/Themes/WhiteMode.xaml", UriKind.Relative);
        private static ResourceDictionary _whiteModeDictionary;

        public static void SwitchToWhiteMode(bool enable)
        {
            if (_whiteModeDictionary == null)
            {
                _whiteModeDictionary = new ResourceDictionary { Source = WhiteModeUri };
            }

            if (enable)
            {
                //Prüfen, ob das Theme bereits aktiv ist!
                if (!Application.Current.Resources.MergedDictionaries.Contains(_whiteModeDictionary))
                {
                    Application.Current.Resources.MergedDictionaries.Add(_whiteModeDictionary);
                }
            }
            else
            {
                Application.Current.Resources.MergedDictionaries.Remove(_whiteModeDictionary);
            }
        }
    }
}