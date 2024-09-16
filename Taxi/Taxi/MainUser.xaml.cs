using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Логика взаимодействия для MainUser.xaml
    /// </summary>
    public partial class MainUser : Window
    {
        string path = "";
        User us = new User();
        private SoccerContext db;
        public MainUser()
        {
            InitializeComponent();
        }

        public MainUser(int id)
        {
            InitializeComponent();

            db = new SoccerContext();
            db.Users.Load();
            db.Orders.Load();
            us = db.Users.Find(id);
            
            ProfileName.Text = us.UserName;
            ProfileLastName.Text = us.LastName;
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
            //BitmapImage bitmap = new BitmapImage();
            //bitmap.BeginInit();
            //bitmap.UriSource = new Uri(us.Avatar, UriKind.Relative);
            //bitmap.CacheOption = BitmapCacheOption.OnLoad;
            //bitmap.EndInit();
            //Avatar.ImageSource = bitmap;

            ItemOrders1.ItemsSource = db.Orders.Where(x => x.UserID == us.UserID && x.Status == "Active").ToList();
            ItemOrders2.ItemsSource = db.Orders.Where(x => x.UserID == us.UserID && (x.Status == "Done" || x.Status == "Cancel")).ToList();
            App.LanguageChanged += LanguageChanged;

            CultureInfo currLang = App.Language;
            if(Convert.ToString(currLang) == "ru-RU")
            {
                ButtonHello.Content = "Привет, " + us.UserName + "\n" + us.Mail;
            }
            else
            {
                ButtonHello.Content = "Hello, " + us.UserName + "\n" + us.Mail;
            }
            //Заполняем меню смены языка:
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


        private void EconomyType_Click(object sender, RoutedEventArgs e)
        {
            CultureInfo currLang = App.Language;
            if (Convert.ToString(currLang) == "ru-RU")
            {
                AutoLabel.Text = "Эконом";
            }
            else
            {
                AutoLabel.Text = "Economy";
            }
        }

        private void ComfortType_Click(object sender, RoutedEventArgs e)
        {
            CultureInfo currLang = App.Language;
            if (Convert.ToString(currLang) == "ru-RU")
            {
                AutoLabel.Text = "Комфорт";
            }
            else
            {
                AutoLabel.Text = "Comfort";
            }
        }


        private void CalendarButton_Click(object sender, RoutedEventArgs e)
        {
            calendar.Visibility = Visibility.Visible;
        }
        private void calendar1_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedDate = calendar.SelectedDate.Value.ToShortDateString();
            DataLabel.Text = selectedDate;
            calendar.Visibility = Visibility.Hidden;
        }

        private void AddOrderButton_Click(object sender, RoutedEventArgs e)
        {
            ScrollOrder.Visibility =  Visibility.Visible;
            ScrollSettings.Visibility = ScrollProfile.Visibility = ScrollHistoryMyOrders.Visibility  = ScrollMyOrders.Visibility = Visibility.Hidden;
        }

        private void MyProfile_Click(object sender, RoutedEventArgs e)
        {
            ScrollSettings.Visibility = ScrollOrder.Visibility = ScrollMyOrders.Visibility = ScrollHistoryMyOrders.Visibility = Visibility.Hidden;
            ScrollProfile.Visibility = Visibility.Visible;
        }
        private void MyOrdersButton_Click(object sender, RoutedEventArgs e)
        {
           ScrollSettings.Visibility = ScrollOrder.Visibility = ScrollProfile.Visibility = ScrollHistoryMyOrders.Visibility = Visibility.Hidden;
            ScrollMyOrders.Visibility = Visibility.Visible;
        }
        private void HistoryButton_Click(object sender, RoutedEventArgs e)
        {
           ScrollSettings.Visibility = ScrollOrder.Visibility = ScrollProfile.Visibility = ScrollMyOrders.Visibility =  Visibility.Hidden;
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
            us.UserName = ProfileName.Text;
            us.LastName = ProfileLastName.Text;
            us.Mail = ProfileMail.Text;
            db.SaveChanges();
        }

        private void CashType_Click(object sender, RoutedEventArgs e)
        {
            CultureInfo currLang = App.Language;
            if (Convert.ToString(currLang) == "ru-RU")
            {
                PaymentLabel.Text = "Наличные";
            }
            else
            {
                PaymentLabel.Text = "Cash";
            }
        }

        private void CardType_Click(object sender, RoutedEventArgs e)
        {
            CultureInfo currLang = App.Language;
            if (Convert.ToString(currLang) == "ru-RU")
            {
                PaymentLabel.Text = "Карта";
            }
            else
            {
                PaymentLabel.Text = "Card";
            }
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
            $"UPDATE Users" +
            $" set Avatar = (SELECT * from OpenRowSet(bulk N'{this.path}',single_blob)as Файл) where UserID = {us.UserID}";

            using (SqlConnection connection = new SqlConnection(@"Data Source=LAPTOP-ORDDEIA2\EXPRESS; Initial Catalog=LoginDB;Integrated Security=True"))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                int number = command.ExecuteNonQuery();
            }
        }

        private void OrderButton_Click(object sender, RoutedEventArgs e)
        {
            if (PresetTimePicker.Text != "" && Address1Box.Text != "" && Address2Box.Text != "" && PaymentLabel.Text != "Payment")
            {
                Order order = new Order();
                order.Address1 = Address1Box.Text;
                order.Address2 = Address2Box.Text;
                if(PaymentLabel.Text == "Карта")
                {
                    order.Category = "Card";
                }
                else if(PaymentLabel.Text == "Наличные")
                {
                    order.Category = "Cash";
                }
                else 
                {
                    order.Category = PaymentLabel.Text;
                }
                if (AutoLabel.Text == "Комфорт")
                {
                    order.Type = "Comfort";
                }
                else if (AutoLabel.Text == "Эконом")
                {
                    order.Type = "Economy";
                }
                else
                {
                    order.Type = PaymentLabel.Text;
                }
                order.Comment = CommentBox.Text;
                order.Price = Convert.ToDecimal(PriceBox.Text);

                DateTime date1 = Convert.ToDateTime(DataLabel.Text);
                string date2 = date1.ToString("MM-dd-yyyy");
                string date3 = PresetTimePicker.Text;
                string iString = "MM-dd-yyyy H:mm";
                var Date = DateTime.ParseExact(date2 + " " + date3, iString, CultureInfo.InvariantCulture);
                order.DateOrder = Date;



                order.Status = "Active";
                order.UserID = us.UserID;
                db.Orders.Add(order);
                db.SaveChanges();
                Address1Box.Clear(); Address2Box.Clear(); CommentBox.Clear(); PriceBox.Clear();
                ItemOrders1.ItemsSource = db.Orders.Where(x => x.UserID == us.UserID && x.Status == "Active").ToList();
                ItemOrders2.ItemsSource = db.Orders.Where(x => x.UserID == us.UserID && (x.Status == "Done" || x.Status == "Cancel")).ToList();
            }
            else
            {
                CultureInfo currLang = App.Language;
                if (Convert.ToString(currLang) == "ru-RU")
                {
                    MessageBox.Show("Вы не заполнили все поля", "Ошибка");
                }
                else
                {
                    MessageBox.Show("You don't fill all ", "Error");
                }
            }
        }

        private void Address2Box_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(Address1Box.Text != "" && Address2Box.Text != "")
            {
                int num = RandomNumber();
                PriceBox.Text = Convert.ToString(num);
            }
        }
        public int RandomNumber(int min = 10, int max = 500)
        {
            Random random = new Random();
            return random.Next(min, max);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
