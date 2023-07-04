using System;
using System.Xml.Serialization;

namespace CascadePass.TrailBot.UI
{
    [Serializable]
    public class Settings : ObservableObject
    {
        private string xmlFolder, indexFilename;
        private bool showPreviewPane, isDirty, debugMode, suggestAdditionalTerms;

        #region Properties

        [XmlIgnore]
        public bool IsDirty => this.isDirty;

        public string IndexFilename
        {
            get => this.indexFilename;
            set
            {
                // File paths are case insensitive in Windows
                if (!string.Equals(this.indexFilename, value, StringComparison.OrdinalIgnoreCase))
                {
                    this.indexFilename = value;
                    this.OnPropertyChanged(nameof(this.IndexFilename));
                }
            }
        }

        public string XmlFolder
        {
            get => this.xmlFolder;
            set
            {
                // File paths are case insensitive in Windows
                if (!string.Equals(this.xmlFolder, value, StringComparison.OrdinalIgnoreCase))
                {
                    this.xmlFolder = value;
                    this.OnPropertyChanged(nameof(this.XmlFolder));
                }
            }
        }

        public bool ShowPreviewPane
        {
            get => this.showPreviewPane;
            set
            {
                if (this.showPreviewPane != value)
                {
                    this.showPreviewPane = value;
                    this.OnPropertyChanged(nameof(this.ShowPreviewPane));
                }
            }
        }

        public bool SuggestAdditionalTerms
        {
            get => this.suggestAdditionalTerms;
            set
            {
                if (this.suggestAdditionalTerms != value)
                {
                    this.suggestAdditionalTerms = value;
                    this.OnPropertyChanged(nameof(this.SuggestAdditionalTerms));
                }
            }
        }

        public bool DebugMode
        {
            get => this.debugMode;
            set
            {
                if (this.debugMode != value)
                {
                    this.debugMode = value;
                    this.OnPropertyChanged(nameof(this.DebugMode));
                }
            }
        }

        #endregion

        #region Methods

        internal override void OnPropertyChanged(string propertyName)
        {
            this.isDirty = true;
            base.OnPropertyChanged(propertyName);
        }

        #endregion
    }
}
