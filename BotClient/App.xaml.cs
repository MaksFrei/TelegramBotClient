using BotClient.Utils.View;
using BotClient.ViewModel;
using System;
using System.Diagnostics;
using System.Windows;
using TelegramBot.Models;

namespace BotClient
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var bot = new Bot();
            var mainWindow = new MainWindow();
            var dialogManager = new DialogManager(mainWindow);

            dialogManager.Register<ChatViewModel, ChatView>();

            mainWindow.Closed += Close;
            mainWindow.DataContext = new MainViewModel(dialogManager, bot);
            mainWindow.Show();
        }

        private void Close(object sender, EventArgs e)
        {
            Process.GetCurrentProcess().Kill();
        }
    }
}
