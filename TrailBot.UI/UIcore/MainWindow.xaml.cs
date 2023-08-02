using System;
using System.Windows;
using System.Windows.Controls.Ribbon;

namespace CascadePass.TrailBot.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : RibbonWindow
    {
        public event EventHandler ContentChanged;

        public MainWindow()
        {
            InitializeComponent();
        }

        public Settings Settings { get; set; }

        public object CurrentContent
        {
            get => this.MainContent?.Content;
            set
            {
                if (this.MainContent.Content != value)
                {
                    this.MainContent.Content = value;
                    this.OnContentChanged();
                }
            }
        }

        internal void OnContentChanged()
        {
            this.ContentChanged?.Invoke(this, EventArgs.Empty);
        }

        public object CurrentViewModel => (this.CurrentContent as FrameworkElement)?.DataContext;
    }
}
