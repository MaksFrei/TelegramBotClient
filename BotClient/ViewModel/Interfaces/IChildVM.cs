
namespace BotClient.ViewModel.Interfaces
{

    public delegate void VMClosedHandler(ChatViewModel viewModel);
    interface IChildVM
    {
        void Close(ChatViewModel viewModel);
    }
}
