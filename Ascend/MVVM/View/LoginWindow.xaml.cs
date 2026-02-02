using System;
using Ascend.Data;
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
using System.Windows.Shapes;
using User = Ascend.Data.User;
using Ascend.Helpers;

namespace Ascend.MVVM.View
{
    public partial class LoginWindow : Window
    {

        private readonly LoginDbContext _dbContext;

        public LoginWindow()
        {
            InitializeComponent();
            _dbContext = new LoginDbContext();
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e) //Diese Methode bewirkt, dass wir unsere GUI, in dem wir die linke Maustaste gedrückt halten, bewegen können!
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void ButtonMinimize_Click(object sender, RoutedEventArgs e) //Mit dieser Methode können wir unsere GUI durch einen Klick auf den Button minimieren!
        {
            this.WindowState = WindowState.Minimized;
        }

        private void ButtonMaximize_Click(object sender, RoutedEventArgs e) //Diese Methode dient zum Maximieren oder Wiederherstellen der GUI!
        {
            if (this.WindowState != WindowState.Maximized)
            {
                this.WindowState = WindowState.Maximized;
            }
            else
            {
                this.WindowState = WindowState.Normal;
            }
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e) //Schließt die Anwendung!
        {
            this.Close();
        }

        //Logik um sich per Klick auf den Button zu registrieren oder einzuloggen. Die Daten (UserName und Password) werden in einer MSSQL Datenbank gespeichert!
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string username = userNametxt.Text;
                string password = passwordtxt.Password;

                var user = _dbContext.Users.FirstOrDefault(User => User.UserName == username && User.Password == password);

                if (user != null)
                {
                    SessionManager.CurrentUserID = user.UserID; //UserID wird der Eigenschaft CurrentUserID zugewiesen!

                    //MainWindow mit dem Benutzernamen öffnen!
                    MainWindow dashboard = new MainWindow(user.UserName);
                    dashboard.Show();
                    Close();
                }
                else
                {
                    MessageBox.Show("Invalid username or password!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                userNametxt.Text = "";
                passwordtxt.Password = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string username = userNametxt.Text;
                string password = passwordtxt.Password;

                if (_dbContext.Users.Any(User => User.UserName == username))
                {
                    MessageBox.Show("This username is already taken. Please choose another one.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                    userNametxt.Text = "";
                    passwordtxt.Password = "";

                    return;
                }

                _dbContext.Users.Add(new User { UserName = username, Password = password });
                _dbContext.SaveChanges();

                MessageBox.Show("Registration successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                userNametxt.Text = "";
                passwordtxt.Password = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
