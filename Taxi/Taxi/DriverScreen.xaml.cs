using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
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
    /// Логика взаимодействия для DriverScreen.xaml
    /// </summary>
    public partial class DriverScreen : Window
    {
        string path = "";
        Driver us = new Driver();
        private SoccerContext db;
        List<JoinResult> orders;
        Order order = new Order();
        Order order1 = new Order();
        public DriverScreen(int IdDriver)
        {
            InitializeComponent();
           
            db = new SoccerContext();
            db.Users.Load();
            db.Orders.Load();
            db.Drivers.Load();
            us = db.Drivers.Find(IdDriver);
           
            ProfileName.Text = us.DriverName;
            ProfileLastName.Text = us.DriverLastName;
            ProfileNumber.Text = us.Contact.ToString();
            ProfileMail.Text = us.Mail;
            //MemoryStream strmImg = new MemoryStream(us.Avatar);
            //BitmapImage myBitmapImage = new BitmapImage();
            //myBitmapImage.BeginInit();
            //myBitmapImage.StreamSource = strmImg;
            //myBitmapImage.DecodePixelWidth = 280;
            //myBitmapImage.DecodePixelHeight = 265;
            //myBitmapImage.EndInit();
            //Avatar.ImageSource = myBitmapImage;
            //Avatar1.ImageSource = myBitmapImage;
            orders =( from p in db.Users
                     join c in db.Orders on p.UserID equals c.UserID
                     select new JoinResult { Address1 = c.Address1, Address2 = c.Address2, DateOrder = c.DateOrder, DriverID = c.DriverID, Status = c.Status, UserID = c.UserID, Contact = p.Contact, Price = c.Price }).ToList();
            ordersGrid.ItemsSource = orders.Where(x => x.Status == "Active" && x.DriverID == null).ToList();
            CurrentOrdersGrid.ItemsSource = orders.Where(x => x.Status == "Confirmed").ToList();
            ItemOrders2.ItemsSource = orders.Where(x => x.Status == "Done" || x.Status == "Cancel").ToList();
            App.LanguageChanged += LanguageChanged;

            CultureInfo currLang = App.Language;
            if(Convert.ToString(currLang) == "ru-RU")
            {
                ButtonHello.Content = "Привет, " + us.DriverName + "\n" + us.Mail;
            }
            else
            {
                ButtonHello.Content = "Hello, " + us.DriverName + "\n" + us.Mail;
            }
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

        private void Exit_Click(object sender, RoutedEventArgs e)
        {

            LoginScreen loginScreen = new LoginScreen();
            loginScreen.Show();
            this.Close();
        }

        private void Power_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void GridBottomBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        } 


        private void AddOrderButton_Click(object sender, RoutedEventArgs e)
        {
            ScrollSettings.Visibility = ScrollOrder.Visibility = Visibility.Visible;
            ScrollProfile.Visibility = ScrollHistoryMyOrders.Visibility = ScrollMyOrders.Visibility = Visibility.Hidden;
        }

        private void MyProfile_Click(object sender, RoutedEventArgs e)
        {
            ScrollSettings.Visibility = ScrollOrder.Visibility = ScrollMyOrders.Visibility = ScrollHistoryMyOrders.Visibility = Visibility.Hidden;
            ScrollProfile.Visibility = Visibility.Visible;
        }
        private void MyOrdersButton_Click(object sender, RoutedEventArgs e)
        {
            ScrollSettings.Visibility =  ScrollOrder.Visibility = ScrollProfile.Visibility = ScrollHistoryMyOrders.Visibility = Visibility.Hidden;
            ScrollMyOrders.Visibility = Visibility.Visible;
        }
        private void HistoryButton_Click(object sender, RoutedEventArgs e)
        {
           ScrollSettings.Visibility = ScrollOrder.Visibility = ScrollProfile.Visibility = ScrollMyOrders.Visibility = Visibility.Hidden;
            ScrollHistoryMyOrders.Visibility = Visibility.Visible;
        }
        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            ScrollHistoryMyOrders.Visibility = ScrollOrder.Visibility = ScrollProfile.Visibility = ScrollMyOrders.Visibility = Visibility.Hidden;
            ScrollSettings.Visibility = Visibility.Visible;
        }
        private void SaveProfileButton_Click(object sender, RoutedEventArgs e)
        {
            us.Contact = ProfileNumber.Text;
            us.DriverName = ProfileName.Text;
            us.DriverLastName = ProfileLastName.Text;
            us.Mail = ProfileMail.Text;
            db.SaveChanges();
        }


        private void AddAvatar_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "JPG Format (*.jpg)|*.jpg";

            if (openFile.ShowDialog() == true)
            {
                this.path = openFile.FileName;
                Avatar.ImageSource = BitmapFrame.Create(new Uri(openFile.FileName));
                Avatar1.ImageSource = BitmapFrame.Create(new Uri(openFile.FileName));
            }

        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            string sqlExpression =
           $"UPDATE Drivers" +
           $" set Avatar = (SELECT * from OpenRowSet(bulk N'{this.path}',single_blob)as Файл) where DriverID = {us.DriverID}";

            using (SqlConnection connection = new SqlConnection(@"Data Source=LAPTOP-ORDDEIA2\SQLEXPRESS; Initial Catalog=LoginDB;Integrated Security=True"))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                int number = command.ExecuteNonQuery();
            }
        }

        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult Result;
            CultureInfo currLang = App.Language;
            if (Convert.ToString(currLang) == "ru-RU")
            {
                Result = MessageBox.Show("Вы уверены,что хотите подтвердить заказ?", "Подтвердить", MessageBoxButton.YesNo, MessageBoxImage.Question);
            }
            else
            {
                Result = MessageBox.Show("Are you sure you want to Accept?", "Accept Message", MessageBoxButton.YesNo, MessageBoxImage.Question);
            }
          
            if (Result == MessageBoxResult.Yes)
            {
                if (ordersGrid.SelectedItems.Count > 0)
                {
                    for (int i = 0; i < ordersGrid.SelectedItems.Count; i++)
                    {
                        var hello = ordersGrid.SelectedItems[i];
                       
                        JoinResult joinResult = ordersGrid.SelectedItems[i] as JoinResult;
                        if (joinResult != null)
                        {
                          order = db.Orders.FirstOrDefault(x => x.Address1 == joinResult.Address1 && x.Address2 == joinResult.Address2 && x.DateOrder == joinResult.DateOrder);
                          order.DriverID = us.DriverID;
                          order.Status = "Confirmed";
                          ordersGrid.ItemsSource = orders.Where(x => x.Status == "Active" && x.DriverID == null).ToList();
                        }
                        
                    }
                }
                db.SaveChanges();
                orders = (from p in db.Users
                          join c in db.Orders on p.UserID equals c.UserID
                          select new JoinResult { Address1 = c.Address1, Address2 = c.Address2, DateOrder = c.DateOrder, DriverID = c.DriverID, Status = c.Status, UserID = c.UserID, Contact = p.Contact, Price = c.Price }).ToList();
                ordersGrid.ItemsSource = orders.Where(x => x.Status == "Active" && x.DriverID == null).ToList();
                CurrentOrdersGrid.ItemsSource = orders.Where(x => x.Status == "Confirmed").ToList();
                ItemOrders2.ItemsSource = orders.Where(x => x.Status == "Done" || x.Status == "Cancel").ToList();
            }

        }

        private void IgnoreButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult Result;
            CultureInfo currLang = App.Language;
            if (Convert.ToString(currLang) == "ru-RU")
            {
                Result = MessageBox.Show("Вы уверены,что хотите проигнорировать заказ?","Игнорировать", MessageBoxButton.YesNo, MessageBoxImage.Question);
            }
            else
            {
                Result = MessageBox.Show("Are you sure you want to Ignore?", "Ignore Message", MessageBoxButton.YesNo, MessageBoxImage.Question);
            }
           
            if (Result == MessageBoxResult.Yes)
            {
                if (ordersGrid.SelectedItems.Count > 0)
                {
                    for (int i = 0; i < ordersGrid.SelectedItems.Count; i++)
                    {
                        JoinResult joinResult = ordersGrid.SelectedItems[i] as JoinResult;
                        if (joinResult != null)
                        {
                            orders.Remove(joinResult);
                            ordersGrid.ItemsSource = orders.Where(x => x.Status == "Active" && x.DriverID == null).ToList();
                        }
                    }
                }
               
            }
        }


        private void CompleteButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult Result;
            CultureInfo currLang = App.Language;
            if (Convert.ToString(currLang) == "ru-RU")
            {
                Result = MessageBox.Show("Вы уверены,что хотите подтвердить выполнение заказа?", "Сообщение о подтверждении", MessageBoxButton.YesNo, MessageBoxImage.Question);
            }
            else {
                Result = MessageBox.Show("Are you sure you want to Complete?", "Complete Message", MessageBoxButton.YesNo, MessageBoxImage.Question);
            }

            if (Result == MessageBoxResult.Yes)
            {
                if (CurrentOrdersGrid.SelectedItems.Count > 0)
                {
                    for (int i = 0; i < CurrentOrdersGrid.SelectedItems.Count; i++)
                    {
                        JoinResult joinResult = CurrentOrdersGrid.SelectedItems[i] as JoinResult;
                        if (joinResult != null)
                        {
                            order1 = db.Orders.FirstOrDefault(x => x.Address1 == joinResult.Address1 && x.Address2 == joinResult.Address2 && x.DateOrder == joinResult.DateOrder);
                            order1.Status = "Done";
                            CurrentOrdersGrid.ItemsSource = orders.Where(x => x.Status == "Confirmed").ToList();
                        }
                        
                    }
                }
                db.SaveChanges();
                orders = (from p in db.Users
                          join c in db.Orders on p.UserID equals c.UserID
                          select new JoinResult { Address1 = c.Address1, Address2 = c.Address2, DateOrder = c.DateOrder, DriverID = c.DriverID, Status = c.Status, UserID = c.UserID, Contact = p.Contact, Price = c.Price }).ToList();
                ordersGrid.ItemsSource = orders.Where(x => x.Status == "Active" && x.DriverID == null).ToList();
                CurrentOrdersGrid.ItemsSource = orders.Where(x => x.Status == "Confirmed").ToList();
                ItemOrders2.ItemsSource = orders.Where(x => x.Status == "Done" || x.Status == "Cancel").ToList();
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult Result;
            CultureInfo currLang = App.Language;
            if (Convert.ToString(currLang) == "ru-RU")
            {
                Result = MessageBox.Show("Вы уверены,что хотите отменить заказ?", "Сообщение об отклонении", MessageBoxButton.YesNo, MessageBoxImage.Question);
            }
            else
            {
                Result = MessageBox.Show("Are you sure you want to Cancel?", "Cancel Message", MessageBoxButton.YesNo, MessageBoxImage.Question);
            }
               
            if (Result == MessageBoxResult.Yes)
            {
                if (CurrentOrdersGrid.SelectedItems.Count > 0)
                {
                    for (int i = 0; i < CurrentOrdersGrid.SelectedItems.Count; i++)
                    {
                        JoinResult joinResult = CurrentOrdersGrid.SelectedItems[i] as JoinResult;
                        if (joinResult != null)
                        {
                            order1 = db.Orders.FirstOrDefault(x => x.Address1 == joinResult.Address1 && x.Address2 == joinResult.Address2 && x.DateOrder == joinResult.DateOrder);
                            order1.Status = "Cancel";
                            CurrentOrdersGrid.ItemsSource = orders.Where(x => x.Status == "Confirmed").ToList();
                        }
                        ordersGrid.ItemsSource = orders.Where(x => x.Status == "Active" && x.DriverID == null).ToList();
                        CurrentOrdersGrid.ItemsSource = orders.Where(x => x.Status == "Confirmed").ToList();
                        ItemOrders2.ItemsSource = orders.Where(x => x.Status == "Done" || x.Status == "Cancel").ToList();
                    }
                }

            }
            db.SaveChanges();
        }
    }
}
