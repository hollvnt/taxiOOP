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
using Taxi.Models;
using System.Data.Entity;

namespace Taxi
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SoccerContext db;
        public MainWindow()
        {
            InitializeComponent();
            db = new SoccerContext();
            db.Users.Load(); // загружаем данные
            phonesGrid.ItemsSource = db.Users.Local.ToBindingList(); // устанавливаем привязку к кэшу
        }
        private void updateButton_Click(object sender, RoutedEventArgs e)
        {
            db.SaveChanges();
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (phonesGrid.SelectedItems.Count > 0)
            {
                for (int i = 0; i < phonesGrid.SelectedItems.Count; i++)
                {
                    User user = phonesGrid.SelectedItems[i] as User;
                    if (user != null)
                    {
                        db.Users.Remove(user);
                    }
                }
            }
            db.SaveChanges();
        }
    }
}
