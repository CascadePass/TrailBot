using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CascadePass.TrailBot.UI.Feature.Found
{
    /// <summary>
    /// Interaction logic for ReportPreview.xaml
    /// </summary>
    public partial class ReportPreview : UserControl
    {
        public ReportPreview()
        {
            InitializeComponent();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.Settings = ApplicationData.Settings;
        }

        public Settings Settings { get; set; }

        public void NavigateTo(string url)
        {
            dynamic activeX = this.WebBrowserControl.GetType().InvokeMember("ActiveXInstance",
                                BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                                null, this.WebBrowserControl, new object[] { });

            activeX.Silent = true;

            this.WebBrowserControl.Navigate(url);
        }

        private void ContextMenu_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            if (sender is FlowDocumentScrollViewer docViewer)
            {
                TextRange selection = new(docViewer.Selection.Start, docViewer.Selection.End);

                if (this.DataContext is MatchedTripReportViewModel vm)
                {
                    vm.SelectedPreviewText = selection.Text;
                }
            }
        }
    }
}
