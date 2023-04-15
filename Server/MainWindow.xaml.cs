using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
using Server.Hub;
using ChatProtocol;

namespace Server
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SystemMessages _chatMessages = new SystemMessages();
        private static ChatHub _chatHub;

        public MainWindow()
        {
            InitializeComponent();
            //添加初始聊天消息
            _chatMessages.Messages = new ObservableCollection<string>();
            MsgContent.ItemsSource = _chatMessages.Messages;
            _chatHub = new ChatHub(_chatMessages);
        }

        private void OnSendMessage(object sender, RoutedEventArgs e)
        {
            _chatMessages.Messages.Add(MsgBox.Text);
        }

        private void Start(object sender, RoutedEventArgs e)
        {
            _chatHub.StartListener();
        }
    }




}
