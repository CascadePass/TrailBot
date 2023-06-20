using System.Windows;
using System.Windows.Controls.Ribbon;

namespace CascadePass.TrailBot.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : RibbonWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public Settings Settings { get; set; }

        public object CurrentContent
        {
            get => this.MainContent.Content;
            set => this.MainContent.Content = value;
        }

        public object CurrentViewModel => (this.CurrentContent as FrameworkElement)?.DataContext;

        //public override void OnApplyTemplate()
        //{
        //    base.OnApplyTemplate();

            //ReaderViewModel vm = this.DataContext as ReaderViewModel;

            //if (Application.Current is App tripReportReaderApplication)
            //{
            //    this.Settings = tripReportReaderApplication.Settings;
            //}

            //BindingOperations.EnableCollectionSynchronization(vm.MatchedTripReports, new object());
        //}
    }
}
