using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Ascend.Data;

namespace Ascend.MVVM.View.SettingsViews
{
    public partial class ProfileSettingsView : UserControl
    {
        private readonly string _userName;

        private const double BackgroundCornerRadius = 10;

        public ProfileSettingsView(string userName)
        {
            InitializeComponent();
            _userName = userName;
            LoadProfileImage();
            LoadProfileFrame();
            LoadProfileBackground();
        }

        private void LoadProfileImage()
        {
            string profileImagePath = GetProfileImagePathFromDatabase();

            if (!string.IsNullOrEmpty(profileImagePath))
            {
                try
                {
                    string fullPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Icons", profileImagePath);
                    ProfilePicture.Source = new BitmapImage(new Uri(fullPath, UriKind.Absolute));
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error... Couldn't load the image: {ex.Message}");
                }
            }
            else
            {
                ProfilePicture.Source = new BitmapImage(new Uri("Icons/DefaultPP.png", UriKind.Relative));
            }
        }

        private string GetProfileImagePathFromDatabase()
        {
            using var context = new LoginDbContext();
            var user = context.Users.FirstOrDefault(u => u.UserName == _userName);
            return user?.ProfileImagePath;
        }

        private void SaveProfileImageToDatabase(string imageName)
        {
            using var context = new LoginDbContext();
            var user = context.Users.FirstOrDefault(u => u.UserName == _userName);
            if (user != null)
            {
                user.ProfileImagePath = imageName;
                context.SaveChanges();
            }
        }

        private void UpdateProfileImage(string imageName)
        {
            try
            {
                string fullPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Icons", imageName);
                ProfilePicture.Source = new BitmapImage(new Uri(fullPath, UriKind.Absolute));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating profile image: {ex.Message}");
            }
        }

        private void ToggleImageSelection(object sender, RoutedEventArgs e)
        {
            ImageSelectionPopup.IsOpen = true;
        }

        private void ChangeProfilePicture(object sender, MouseButtonEventArgs e)
        {
            if (sender is Image selectedImage)
            {
                string imagePath = selectedImage.Source.ToString().Split('/').Last();
                SaveProfileImageToDatabase(imagePath);
                UpdateProfileImage(imagePath);
                ImageSelectionPopup.Visibility = Visibility.Collapsed;

                //MainWindow aktualisieren
                MainWindow.Instance?.RefreshProfileImageFrameAndBackground();
            }
        }


        private void LoadProfileFrame()
        {
            string framePath = GetFramePathFromDatabase();

            if (!string.IsNullOrEmpty(framePath) && framePath != "None")
            {
                try
                {
                    string fullPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Icons", framePath);
                    ProfileFrame.Source = new BitmapImage(new Uri(fullPath, UriKind.Absolute));
                    ProfileFrame.Visibility = Visibility.Visible;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error... Couldn't load the frame: {ex.Message}");
                }
            }
            else
            {
                ProfileFrame.Visibility = Visibility.Collapsed;
            }
        }

        private string GetFramePathFromDatabase()
        {
            using var context = new LoginDbContext();
            var user = context.Users.FirstOrDefault(u => u.UserName == _userName);
            return user?.FramePath ?? "DefaultFrame.png";
        }

        private void SaveFramePathToDatabase(string framePath)
        {
            using var context = new LoginDbContext();
            var user = context.Users.FirstOrDefault(u => u.UserName == _userName);
            if (user != null)
            {
                user.FramePath = framePath;
                context.SaveChanges();
            }
        }

        private void UpdateFrame(string framePath)
        {
            try
            {
                if (framePath == "None")
                {
                    ProfileFrame.Visibility = Visibility.Collapsed;
                }
                else
                {
                    string fullPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Icons", framePath);
                    ProfileFrame.Source = new BitmapImage(new Uri(fullPath, UriKind.Absolute));
                    ProfileFrame.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating profile frame: {ex.Message}");
            }
        }

        private void ToggleFrameSelection(object sender, RoutedEventArgs e)
        {
            FrameSelectionPopup.IsOpen = true;
        }

        private void ApplyFrame(object sender, MouseButtonEventArgs e)
        {
            if (sender is Image selectedFrame)
            {
                string frameFileName = selectedFrame.Source.ToString().Split('/').Last();
                SaveFramePathToDatabase(frameFileName);
                UpdateFrame(frameFileName);
                FrameSelectionPopup.Visibility = Visibility.Collapsed;

                //MainWindow aktualisieren
                MainWindow.Instance?.RefreshProfileImageFrameAndBackground();
            }
        }


        private void LoadProfileBackground()
        {
            string backgroundPath = GetBackgroundPathFromDatabase();

            if (!string.IsNullOrEmpty(backgroundPath))
            {
                try
                {
                    string fullPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Icons", backgroundPath);
                    ProfileBackground.Source = new BitmapImage(new Uri(fullPath, UriKind.Absolute));
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error... Couldn't load the background: {ex.Message}");
                }
            }
            else
            {
                ProfileBackground.Source = new BitmapImage(new Uri("Icons/DefaultBackground.png", UriKind.Relative));
            }
        }

        private string GetBackgroundPathFromDatabase()
        {
            using var context = new LoginDbContext();
            var user = context.Users.FirstOrDefault(u => u.UserName == _userName);
            return user?.BackgroundPath;
        }

        private void SaveBackgroundPathToDatabase(string backgroundPath)
        {
            using var context = new LoginDbContext();
            var user = context.Users.FirstOrDefault(u => u.UserName == _userName);
            if (user != null)
            {
                user.BackgroundPath = backgroundPath;
                context.SaveChanges();
            }
        }

        private void UpdateBackground(string backgroundPath)
        {
            try
            {
                string fullPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Icons", backgroundPath);
                ProfileBackground.Source = new BitmapImage(new Uri(fullPath, UriKind.Absolute));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating profile background: {ex.Message}");
            }
        }

        private void ToggleBackgroundSelection(object sender, RoutedEventArgs e)
        {
            BackgroundSelectionPopup.IsOpen = true;
        }

        private void ApplyBackground(object sender, MouseButtonEventArgs e)
        {
            if (sender is Image selectedBackground)
            {
                string backgroundFileName = selectedBackground.Source.ToString().Split('/').Last();
                SaveBackgroundPathToDatabase(backgroundFileName);
                UpdateBackground(backgroundFileName);
                BackgroundSelectionPopup.Visibility = Visibility.Collapsed;

                //MainWindow aktualisieren
                MainWindow.Instance?.RefreshProfileImageFrameAndBackground();
            }
        }

        private void ProfileBackground_OnLoaded(object sender, RoutedEventArgs e)
        {
            ApplyRoundClip(sender as FrameworkElement, BackgroundCornerRadius);
        }

        private void ProfileBackground_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            ApplyRoundClip(sender as FrameworkElement, BackgroundCornerRadius);
        }

        private static void ApplyRoundClip(FrameworkElement fe, double radius)
        {
            if (fe == null) return;

            //Falls das Layout noch keine Größe hat, später erneut versuchen!
            if (fe.ActualWidth <= 0 || fe.ActualHeight <= 0)
            {
                fe.Dispatcher.InvokeAsync(() => ApplyRoundClip(fe, radius),
                                          System.Windows.Threading.DispatcherPriority.Loaded);
                return;
            }

            //Rechteck-Geometrie mit abgerundeten Ecken auf die aktuelle Größe legen!
            fe.Clip = new RectangleGeometry(
                new Rect(0, 0, fe.ActualWidth, fe.ActualHeight),
                radius, radius
            );
        }
    }
}