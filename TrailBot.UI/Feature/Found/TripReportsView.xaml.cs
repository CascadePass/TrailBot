using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Xml.Serialization;

namespace CascadePass.TrailBot.UI.Feature.Found
{
    /// <summary>
    /// Interaction logic for TripReportsView.xaml
    /// </summary>
    public partial class TripReportsView : UserControl
    {
        public TripReportsView()
        {
            InitializeComponent();
        }
        public Settings Settings { get; set; }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.Settings = ApplicationData.Settings;
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dataGrid = (DataGrid)sender;

            if (dataGrid.SelectedItem == null)
            {
                return;
            }

            if (!this.Settings.ShowPreviewPane)
            {
                return;
            }

            Thread loaderThread = new(new ParameterizedThreadStart(this.UpdatePreviewContent))
            {
                Name = "Preview Content Loader",
                IsBackground = true,
                Priority = ThreadPriority.BelowNormal,
            };

            loaderThread.Start(dataGrid.SelectedItem);
        }

        private void UpdatePreviewContent(object state)
        {
            MatchedTripReportViewModel vm = (MatchedTripReportViewModel)state;

            TripReport tr = vm.Report;

            if (tr == null)
            {
                if (File.Exists(vm.MatchedTripReport.Filename))
                {
                    XmlSerializer serializer = new(typeof(WtaTripReport));

                    using StreamReader streamReader = new(vm.MatchedTripReport.Filename);
                    tr = (TripReport)serializer.Deserialize(streamReader);
                }
                else
                {
                    var provider = new WtaDataProvider() { LastTripReportRequest = DateTime.MinValue, LastGetRecentRequest = DateTime.MinValue };
                    tr = provider.GetTripReport(vm.MatchedTripReport.SourceUri);
                }

                vm.Report = tr;
            }

            vm.MatchedTripReport.HasBeenSeen = true;
        }
    }
}
