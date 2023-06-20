using System;
using System.IO;
using System.Windows.Input;

namespace CascadePass.TrailBot.UI.Feature.WelcomeScreen
{
    public class SetupDataStorageTaskViewModel : SetupTaskViewModel
    {
        private string xmlFolderAccessMessage, xmlFolderPath;
        private bool canWriteToXmlFolder;
        private DelegateCommand viewXmlFolderInWebBrowserCommand;

        public Settings Settings { get; set; }

        public bool CanWriteToXmlFolder
        {
            get => this.canWriteToXmlFolder;
            set
            {
                if (this.canWriteToXmlFolder != value)
                {
                    this.canWriteToXmlFolder = value;
                    this.OnPropertyChanged(nameof(this.CanWriteToXmlFolder));
                }
            }
        }

        public string XmlFolderAccessMessage
        {
            get => this.xmlFolderAccessMessage; protected set
            {
                if (!string.Equals(this.xmlFolderAccessMessage, value, StringComparison.Ordinal))
                {
                    this.xmlFolderAccessMessage = value;
                    this.OnPropertyChanged(nameof(this.XmlFolderAccessMessage));
                }
            }
        }

        public string XmlFolderPath
        {
            get => this.xmlFolderPath;
            set
            {
                if (!string.Equals(this.xmlFolderPath, value, StringComparison.Ordinal))
                {
                    this.xmlFolderPath = value;
                    this.Settings.XmlFolder = value;
                    this.Validate();
                    this.OnPropertyChanged(nameof(this.XmlFolderPath));
                }
            }
        }

        public ICommand ViewInBrowserCommand => this.viewXmlFolderInWebBrowserCommand ??= new DelegateCommand(this.LaunchTripReportInBrowser);

        private void LaunchTripReportInBrowser()
        {
            this.ViewInBrowser(this.XmlFolderPath);
        }

        public override void Validate()
        {
            string path = this.XmlFolderPath;

            this.CanWriteToXmlFolder = Directory.Exists(path);

            if (!this.CanWriteToXmlFolder)
            {
                this.XmlFolderAccessMessage = "Folder does not exist.";
                this.IsComplete = false;
            }
            else
            {
                if (!FileStore.TestWriteAccess(this.XmlFolderPath))
                {
                    this.CanWriteToXmlFolder = false;
                    this.XmlFolderAccessMessage = "Trail Bot does not have write access.";
                    this.IsComplete = false;
                }
                else
                {
                    this.XmlFolderAccessMessage = string.Empty;
                    this.IsComplete = true;
                }
            }
        }

    }
}
