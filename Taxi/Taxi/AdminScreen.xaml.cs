using System;
using System.Collections.Generic;
using System.Data.Entity;
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
using System.Windows.Shapes;
using Taxi.Models;

namespace Taxi
{
    /// <summary>
    /// Логика взаимодействия для AdminScreen.xaml
    /// </summary>
    public partial class AdminScreen : Window
    {
        private SoccerContext db;
        public AdminScreen(string Login)
        {
            InitializeComponent();
            db = new SoccerContext();
            db.Users.Load();
            db.Drivers.Load();
            db.Orders.Load();
            CurrentUsersGrid.ItemsSource = db.Users.Local.ToBindingList();
            CurrentDriversGrid.ItemsSource = db.Drivers.Local.ToBindingList();
            CurrentOrdersGrid.ItemsSource = db.Orders.Local.ToBindingList();
            App.LanguageChanged += LanguageChanged;
            CultureInfo currLang = App.Language;
            menuLanguage.Items.Clear();
            foreach (var lang in App.Languages)
            {
                MenuItem menuLang = new MenuItem();
                menuLang.Header = lang.DisplayName;
                menuLang.Tag = lang;
                menuLang.IsChecked = lang.Equals(currLang);
                menuLang.Click += ChangeLanguageClick;
                menuLanguage.Items.Add(menuLang);
            }

        }
        private void LanguageChanged(Object sender, EventArgs e)
        {
            CultureInfo currLang = App.Language;

            //Отмечаем нужный пункт смены языка как выбранный язык
            foreach (MenuItem i in menuLanguage.Items)
            {
                CultureInfo ci = i.Tag as CultureInfo;
                i.IsChecked = ci != null && ci.Equals(currLang);
            }
        }

        private void ChangeLanguageClick(Object sender, EventArgs e)
        {
            MenuItem mi = sender as MenuItem;
            if (mi != null)
            {
                CultureInfo lang = mi.Tag as CultureInfo;
                if (lang != null)
                {
                    App.Language = lang;
                }
            }

        }

        private void OrdersButton_Click(object sender, RoutedEventArgs e)
        {
            ScrollSettings.Visibility = ScrollDrivers.Visibility = ScrollUsers.Visibility = Visibility.Hidden;
            ScrollOrders.Visibility = Visibility.Visible;
        }

        private void DriversButton_Click(object sender, RoutedEventArgs e)
        {
            ScrollSettings.Visibility = ScrollOrders.Visibility = ScrollUsers.Visibility = Visibility.Hidden;
            ScrollDrivers.Visibility = Visibility.Visible;
        }

        private void UsersButton_Click(object sender, RoutedEventArgs e)
        {
            ScrollSettings.Visibility = ScrollDrivers.Visibility = ScrollOrders.Visibility = Visibility.Hidden;
            ScrollUsers.Visibility = Visibility.Visible;
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            ScrollUsers.Visibility = ScrollDrivers.Visibility = ScrollOrders.Visibility = Visibility.Hidden;
            ScrollSettings.Visibility = Visibility.Visible;
        }
  
        private void Power_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void GridBottomBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
        private void Exit_Click(object sender, RoutedEventArgs e)
        {

            LoginScreen loginScreen = new LoginScreen();
            loginScreen.Show();
            this.Close();
        }
        private void updateButton_Click(object sender, RoutedEventArgs e)
        {
            db.SaveChanges();
        }
        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentUsersGrid.SelectedItems.Count > 0)
            {
                for (int i = 0; i < CurrentUsersGrid.SelectedItems.Count; i++)
                {
                    User user = CurrentUsersGrid.SelectedItems[i] as User;
                    if (user != null)
                    {
                        db.Users.Remove(user);
                    }
                }
            }
            db.SaveChanges(); 
        }
        private void deleteButton1_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentDriversGrid.SelectedItems.Count > 0)
            {
                for (int i = 0; i < CurrentDriversGrid.SelectedItems.Count; i++)
                {
                    Driver driver = CurrentDriversGrid.SelectedItems[i] as Driver;
                    if (driver != null)
                    {
                        db.Drivers.Remove(driver);
                    }
                }
            }
            db.SaveChanges();
        }
        private void deleteButton2_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentOrdersGrid.SelectedItems.Count > 0)
            {
                for (int i = 0; i < CurrentOrdersGrid.SelectedItems.Count; i++)
                {
                    Order order = CurrentOrdersGrid.SelectedItems[i] as Order;
                    if (order != null)
                    {
                        db.Orders.Remove(order);
                    }
                }
            }
            db.SaveChanges();
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            if (SearchText.Text == "")
            {
                CurrentOrdersGrid.ItemsSource = db.Orders.ToList();
            }
            else
            {
                var list = db.Orders.Where(x => x.Address1 == SearchText.Text || x.Address2 == SearchText.Text).ToList();
                CurrentOrdersGrid.ItemsSource = list;
            }
        }
        private void Search1_Click(object sender, RoutedEventArgs e)
        {
            if (SearchText1.Text == "")
            {
                CurrentDriversGrid.ItemsSource = db.Drivers.Local.ToBindingList();
            }
            else
            {
                var list = db.Drivers.Where(x => x.Mail == SearchText1.Text || x.DriverName == SearchText1.Text).ToList();
                CurrentDriversGrid.ItemsSource = list;
            }
        }
        private void Search2_Click(object sender, RoutedEventArgs e)
        {
            if (SearchText2.Text == "")
            {
                CurrentUsersGrid.ItemsSource = db.Users.Local.ToBindingList();
            }
            else
            {
                var list = db.Users.Where(x => x.Mail == SearchText2.Text || x.UserName == SearchText2.Text).ToList();
                CurrentUsersGrid.ItemsSource = list;
            }
        }
    }
}
