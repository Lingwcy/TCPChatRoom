using Client.Hub;
using Client.Model;
using ChatProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

namespace Client.Pages
{
    /// <summary>
    /// MainContent.xaml 的交互逻辑
    /// </summary>
    public partial class MainContent : Window
    {
        public ChatClient _ChatClient { get; set; }
        public UserOnlineModel UserOnlineModel { get; set; }
        public UserMessageModel UserMessageModel { get; set; }
        public User ConversationUser { get; set; }
        public int ConversationMessageIndex { get; set; }   
        public string UserName { get; set; }
        public MainContent(string userName,ChatClient hub)
        {
            InitializeComponent();
            UserOnlineModel = new UserOnlineModel();
            UserOnlineView.ItemsSource = UserOnlineModel.Users;
            UserMessageModel = new UserMessageModel();
            UserName = userName;
            _ChatClient = hub;
        }

        private async void OnSendMessage(object sender, RoutedEventArgs e)
        {
            Message message = new Message(MsgBox.Text, MessageType.发出);
            UserMessageModel.Conversations[ConversationMessageIndex].Messages.Add(message);
            if (ConversationUser == null) return;
            ChatPacket chatPacket = new ChatPacket(UserName, ConversationUser.Name, Command.Chat, MsgBox.Text);
            await _ChatClient.SendChatPacket(chatPacket);
            MsgBox.Text = string.Empty;
        }

        private void UserList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            User selectedUser = (User)UserOnlineView.SelectedItem;
            int index = UserMessageModel.FindConversation(selectedUser);
            ConversationMessageIndex = index;
            SelectedConversation.Content = UserMessageModel.Conversations[index].User.Name;
            ConversationUser = UserMessageModel.Conversations[index].User;
            UserMsgContentView.ItemsSource = UserMessageModel.Conversations[index].Messages;
        }
    }
}
