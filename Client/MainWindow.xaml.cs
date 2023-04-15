
using Client.Hub;
using Client.Pages;
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

namespace Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ChatClient ChatClient { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            ChatClient = new ChatClient();
        }
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Password;

            if (IsValidUser(username, password))
            {
                
                // 导航到主页面
                MainContent main =new MainContent(username,ChatClient);
                main.Show();

                // 关闭当前登录窗口
                Close();
            }
            else
            {
                MessageBox.Show("Invalid username or password. Please try again.");
            }
        }

        private bool IsValidUser(string username, string password)
        {
            _ =ChatClient.LoginAsync(txtUsername.Text.Trim());
            return true; 
        }

    }
}
