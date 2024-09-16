using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Логика взаимодействия для RegisterScreen.xaml
    /// </summary>
    public partial class RegisterScreen : Window
    {
        private SoccerContext db;
        public RegisterScreen()
        {
            InitializeComponent();
            db = new SoccerContext();
            db.Users.Load();
            App.LanguageChanged += LanguageChanged;

            CultureInfo currLang = App.Language;

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
        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (txtUserName.Text == "" || txtLastName.Text == "" || txtNumber.Text == "" || txtMail.Text=="" || txtPassword.Password == "" || txtPassword1.Password == "")
            {
                MessageBox.Show("Please fill mandatory fields!");
            }
            else if (txtPassword1.Password != txtPassword.Password)
            {
                MessageBox.Show("Password do not match");
            }
            
            else
            {
                string pattern = @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$";
                string pattern1 = @"^((8|\+375)[\-]?)?(\(?\d{3}\)?[\-]?)?[\d\-]{7,10}$";
                if (Regex.IsMatch(txtMail.Text, pattern, RegexOptions.IgnoreCase))
                {
                    if (Regex.IsMatch(txtNumber.Text, pattern1, RegexOptions.IgnoreCase))
                    {
                        if (db.Users.FirstOrDefault(x => x.Mail == txtMail.Text.ToLower()) != null || db.Users.FirstOrDefault(x => x.Contact == txtNumber.Text) != null)
                        {
                            MessageBox.Show("User with this email or number already exists", "Error");
                        }
                        else
                        {
                            using (SqlConnection sqlCon = new SqlConnection(@"Data Source=LAPTOP-ORDDEIA2; Initial Catalog=LoginDB;Integrated Security=True"))
                            {
                                string Pass = Encrypt(txtPassword.Password, "1337");
                                sqlCon.Open();
                                SqlCommand sqlCmd = new SqlCommand(@"AddUser", sqlCon);
                                sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                                sqlCmd.Parameters.AddWithValue("@UserName", txtUserName.Text.Trim());
                                sqlCmd.Parameters.AddWithValue("@Password", Pass);
                                sqlCmd.Parameters.AddWithValue("@Contact", txtNumber.Text.Trim());
                                sqlCmd.Parameters.AddWithValue("@Mail", txtMail.Text.Trim().ToLower());
                                sqlCmd.Parameters.AddWithValue("@LastName", txtLastName.Text.Trim());
                                sqlCmd.ExecuteNonQuery();
                                MessageBox.Show("registration is successfull");
                                Clear();
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Your number phone isn't valid!", "Error");
                    }
                }
                else
                {
                    MessageBox.Show("Your email isn't valid!", "Error");
                }
                
                    

                void Clear()
                {
                    txtUserName.Text = txtPassword.Password = txtPassword1.Password = txtMail.Text = txtLastName.Text = txtNumber.Text = "";
                }
            }
        }
        public static string Encrypt(string plainText, string password,
                    string salt = "Kosher", string hashAlgorithm = "SHA1",
                    int passwordIterations = 2, string initialVector = "OFRna73m*aze01xY",
                    int keySize = 256)
        {
            if (string.IsNullOrEmpty(plainText))
                return "";

            byte[] initialVectorBytes = Encoding.ASCII.GetBytes(initialVector);
            byte[] saltValueBytes = Encoding.ASCII.GetBytes(salt);
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            PasswordDeriveBytes derivedPassword = new PasswordDeriveBytes(password, saltValueBytes, hashAlgorithm, passwordIterations);
            byte[] keyBytes = derivedPassword.GetBytes(keySize / 8);
            RijndaelManaged symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;

            byte[] cipherTextBytes = null;

            using (ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initialVectorBytes))
            {
                using (MemoryStream memStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memStream, encryptor, CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                        cryptoStream.FlushFinalBlock();
                        cipherTextBytes = memStream.ToArray();
                        memStream.Close();
                        cryptoStream.Close();
                    }
                }
            }

            symmetricKey.Clear();
            return Convert.ToBase64String(cipherTextBytes);
        }
        private void Power_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void GridBottomBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            LoginScreen loginScreen = new LoginScreen();
            loginScreen.Show();
            this.Close();
        }

        private void txtNumber_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
