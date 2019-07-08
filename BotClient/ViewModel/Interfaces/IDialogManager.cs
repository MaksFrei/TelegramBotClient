using System.Threading.Tasks;

namespace BotClient.Interfaces.ViewModel
{
    public interface IDialogManager
    {
        void Show(string key, object viewModel);
        void Close(string key, object viewModel);
    }
    public static class DialogManagerExtensions
    {
        public static void Show<TViewModel>(this IDialogManager dialogManager, TViewModel viewModel)
        {
            var key = typeof(TViewModel).FullName;
            dialogManager.Show(key, viewModel);
        }

        public static void Close<TViewModel>(this IDialogManager dialogManager, TViewModel viewModel)
        {
            var key = typeof(TViewModel).FullName;
            dialogManager.Close(key, viewModel);
        }
    }
}
