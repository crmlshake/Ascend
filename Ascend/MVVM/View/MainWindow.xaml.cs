using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using Ascend.Data;
using Ascend.Helpers;
using Ascend.MVVM.ViewModel;

namespace Ascend.MVVM.View
{
    public partial class MainWindow : Window
    {
        private string _userName;
        private TodoViewModel _todoViewModel;
        public static MainWindow Instance { get; private set; }

        public MainWindow(string userName)
        {
            InitializeComponent();
            Instance = this; // <-- Singleton setzen
            _userName = userName;
            SetGreeting();
            UserNameLabel.Content = _userName;
            SmallUserNameText.Text = _userName;
            LoadProfileImage();
            LoadProfileFrame();
            LoadProfileBackground();
            _todoViewModel = new TodoViewModel();
            DataContext = _todoViewModel;
           
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void ButtonMinimize_Click(object sender, RoutedEventArgs e) =>
            WindowState = WindowState.Minimized;

        private void ButtonMaximize_Click(object sender, RoutedEventArgs e) =>
            WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;

        private void ButtonClose_Click(object sender, RoutedEventArgs e) =>
            Close();

        private void SetGreeting()
        {
            int hour = DateTime.Now.Hour;
            string greeting = hour switch
            {
                >= 5 and < 12 => "Good Morning",
                >= 12 and < 18 => "Good Afternoon",
                _ => "Good Evening"
            };
            GreetingTextBlock.Text = $"{greeting}, {_userName}!";
        }

        private void SearchTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (SearchTextBox.Text == "⌕ Search")
            {
                SearchTextBox.Text = "";
                SearchTextBox.Foreground = Brushes.White;
            }
        }

        private void SearchTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SearchTextBox.Text))
            {
                SearchTextBox.Text = "⌕ Search";
                SearchTextBox.Foreground = Brushes.Gray;
            }
        }

        private void LoadHomeView() =>
            MainContentControl.Content = new HomeView();

        private void HomeButton_Checked(object sender, RoutedEventArgs e)
        {
            DeselectOtherNavButtons((ToggleButton)sender);
            LoadHomeView();
        }

        private void LoadNewsView() =>
            MainContentControl.Content = new NewsView();

        private void NewsButton_Checked(object sender, RoutedEventArgs e)
        {
            DeselectOtherNavButtons((ToggleButton)sender);
            LoadNewsView();
        }

        private void LoadTodosView()
        {
            MainContentControl.Content = new TodosView { DataContext = _todoViewModel };
        }

        private void TodosButton_Checked(object sender, RoutedEventArgs e)
        {
            DeselectOtherNavButtons((ToggleButton)sender);
            LoadTodosView();
        }

        private void LoadFriendsView() =>
            MainContentControl.Content = new FriendsView();

        private void FriendsButton_Checked(object sender, RoutedEventArgs e)
        {
            DeselectOtherNavButtons((ToggleButton)sender);
            LoadFriendsView();
        }

        private void LoadNeptune7View() =>
            MainContentControl.Content = new Neptune7View();

        private void Neptune7Button_Checked(object sender, RoutedEventArgs e)
        {
            DeselectOtherNavButtons((ToggleButton)sender);
            LoadNeptune7View();
        }

        private void LoadSettingsView() =>
            MainContentControl.Content = new SettingsView(_userName);

        private void SettingsButton_Checked(object sender, RoutedEventArgs e)
        {
            DeselectOtherNavButtons((ToggleButton)sender);
            LoadSettingsView();
        }

        private void LoadHelpView() =>
            MainContentControl.Content = new HelpView();

        private void HelpButton_Checked(object sender, RoutedEventArgs e)
        {
            DeselectOtherNavButtons((ToggleButton)sender);
            LoadHelpView();
        }

        private void DeselectOtherNavButtons(ToggleButton clickedButton)
        {
            foreach (var button in new[]
            {
                HomeToggleButton, NewsToggleButton, TodosToggleButton,
                FriendsToggleButton, Neptune7ToggleButton,
                SettingsToggleButton, HelpToggleButton
            })
            {
                if (button != clickedButton)
                    button.IsChecked = false;
            }
        }

        private void LoadProfileImage()
        {
            string imagePath = GetProfileImagePathFromDatabase();

            if (!string.IsNullOrEmpty(imagePath))
            {
                try
                {
                    string fullPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Icons", imagePath);
                    ProfilePicture.Source = new BitmapImage(new Uri(fullPath, UriKind.Absolute));
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading profile image: {ex.Message}");
                }
            }
            else
            {
                ProfilePicture.Source = new BitmapImage(new Uri("Icons/DefaultPP.png", UriKind.Relative));
            }

            if (!string.IsNullOrEmpty(imagePath))
            {
                try
                {
                    string fullPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Icons", imagePath);
                    SmallProfilePicture.Source = new BitmapImage(new Uri(fullPath, UriKind.Absolute));
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading profile image: {ex.Message}");
                }
            }
            else
            {
                SmallProfilePicture.Source = new BitmapImage(new Uri("Icons/DefaultPP.png", UriKind.Relative));
            }
        }

        private string GetProfileImagePathFromDatabase()
        {
            using var context = new LoginDbContext();
            return context.Users.FirstOrDefault(u => u.UserName == _userName)?.ProfileImagePath;
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
                    MessageBox.Show($"Error loading profile frame: {ex.Message}");
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
            return context.Users.FirstOrDefault(u => u.UserName == _userName)?.FramePath;
        }

        private void LoadProfileBackground()
        {
            string backgroundPath = GetBackgroundPathFromDatabase();

            if (!string.IsNullOrEmpty(backgroundPath))
            {
                try
                {
                    string fullPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Icons", backgroundPath);
                    var brush = new ImageBrush
                    {
                        ImageSource = new BitmapImage(new Uri(fullPath, UriKind.Absolute)),
                        Stretch = Stretch.UniformToFill,
                        AlignmentX = AlignmentX.Center,
                        AlignmentY = AlignmentY.Top
                    };
                    ProfileBackgroundBorder.Background = brush;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading profile background: {ex.Message}");
                }
            }
            else
            {
                //Fallback auf Standard-Hintergrund
                var brush = new ImageBrush
                {
                    ImageSource = new BitmapImage(new Uri("Icons/DefaultBackground.png", UriKind.Relative)),
                    Stretch = Stretch.UniformToFill,
                    AlignmentX = AlignmentX.Center,
                    AlignmentY = AlignmentY.Top
                };
                ProfileBackgroundBorder.Background = brush;
            }
        }

        private string GetBackgroundPathFromDatabase()
        {
            using var context = new LoginDbContext();
            return context.Users.FirstOrDefault(u => u.UserName == _userName)?.BackgroundPath;
        }

        public void RefreshProfileImageFrameAndBackground()
        {
            LoadProfileImage();
            LoadProfileFrame();
            LoadProfileBackground();
        }

        private void CopyIcon_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var random = new Random();

            //Länge des Codes (z. B. 5-stellig)!
            int length = 5;

            //Zufällige Zahlen generieren!
            string friendCode = "#";
            for (int i = 0; i < length; i++)
            {
                friendCode += random.Next(0, 10); //Zahl zwischen 0–9
            }

            //In Zwischenablage kopieren!
            Clipboard.SetText(friendCode);
        }

        private void StatusSettingsButton_Click(object sender, MouseButtonEventArgs e)
        {
            StatusSettingsPopup.IsOpen = !StatusSettingsPopup.IsOpen;
        }

    }
}