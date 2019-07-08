using BotClient.Interfaces.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace BotClient.Utils.View
{
    public class DialogManager : IDialogManager
    {
        #region Properties
        private readonly IDictionary<string, FrameworkElement> _viewsByKey = new Dictionary<string, FrameworkElement>();

        private List<Window> OpenedWindows { get; set; } = new List<Window>();

        public Window Owner { get; set; }
        #endregion

        #region Constructors
        public DialogManager(Window owner)
        {
            Owner = owner;
            Owner.LocationChanged += OwnerDrag;
        }
        #endregion

        #region Methods
        public void Close(string key, object viewModel)
        {
            if (!_viewsByKey.ContainsKey(key))
                throw new ArgumentException($"Key {key} wasn't registered", nameof(key));
            var window = OpenedWindows.Single(w => w.DataContext == viewModel);
            window.Close();
            OpenedWindows.Remove(window);
        }

        public void Register<TView>(string key, TView view) where TView : FrameworkElement
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));
            if (view == null)
                throw new ArgumentNullException(nameof(view));
            if (_viewsByKey.ContainsKey(key))
                throw new ArgumentException($"Key {key} has already registered", nameof(key));
            _viewsByKey[key] = view;
        }

        public void Show(string key, object viewModel)
        {
            if (!_viewsByKey.ContainsKey(key))
                throw new ArgumentException($"Key {key} wasn't registered", nameof(key));
            var view = _viewsByKey[key];
            var window = new Window()
            {
                Content = view,
                DataContext = viewModel,
                Height = 450,
                Width = 800,
                WindowStyle = WindowStyle.None,
                ShowInTaskbar = false,
                Left = Owner.Left + Owner.Width,
                Top = Owner.Top 
            };
            OpenedWindows.Add(window);
            window.MouseLeftButtonDown += Drag;
            window.Show();
        }

        private void Drag(object window, MouseButtonEventArgs e)
        {
            ((Window)window).DragMove();
        }

        private void OwnerDrag(object window, EventArgs e)
        {
            Owner.Left = ((Window)window).Left;
            Owner.Top = ((Window)window).Top;
        }
        #endregion
    }

    public static class DialogManagerExtensions
    {
        public static void Register<TViewModel, TView>(this DialogManager dialogManager)
            where TView : FrameworkElement, new()
        {
            var key = typeof(TViewModel).FullName;
            dialogManager.Register(key, new TView());
        }
    }
}
