using System;
using System.Collections.Generic;

namespace CascadePass.TrailBot.UI
{
    public class QuickSettingsViewModel : ViewModel
    {
        public Settings Settings { get; set; }
        public WebProviderManager WebProviderManager { get; set; }

        #region Browser

        public SupportedBrowser SelectedBrowser
        {
            get => this.WebProviderManager?.Browser ?? SupportedBrowser.ValueNotSet;
            set
            {
                if (this.WebProviderManager.Browser != value)
                {
                    //if (Enum.TryParse(value, out SupportedBrowser selected))
                    {
                        this.WebProviderManager.Browser = value;
                        this.OnPropertyChanged(nameof(this.SelectedBrowser));
                    }
                }
            }
        }

        public List<SupportedBrowser> AvailableBrowsers
        {
            get
            {
                List<SupportedBrowser> all = new();
                foreach (var browserType in Enum.GetValues(typeof(SupportedBrowser)))
                {
                    var value = (SupportedBrowser)browserType;

                    if (value != SupportedBrowser.ValueNotSet)
                    {
                        // TODO: Check to see if the browser is installed?
                        all.Add(value);
                    }
                }

                return all;
            }
        }

        #endregion

        #region PreservationRule

        //public PreservationRule PreservationRule
        //{
        //    get => this.WebProviderManager.PreservationRule;
        //    set
        //    {
        //        if (this.WebProviderManager.PreservationRule != value)
        //        {
        //            //if (Enum.TryParse(value, out SupportedBrowser selected))
        //            {
        //                this.WebProviderManager.PreservationRule = value;
        //                //this.WebProviderManager.br
        //                this.OnPropertyChanged(nameof(this.PreservationRule));
        //            }
        //        }
        //    }
        //}

        public List<PreservationRule> AvailablePreservationRules
        {
            get
            {
                List<PreservationRule> all = new();
                foreach (var browserType in Enum.GetValues(typeof(PreservationRule)))
                {
                    var value = (PreservationRule)browserType;

                    if (value != PreservationRule.ValueNotSet)
                    {
                        // TODO: Check to see if the browser is installed?
                        all.Add(value);
                    }
                }

                return all;
            }
        }

        #endregion

        internal void NotifyComboBoxes()
        {
            this.OnPropertyChanged(nameof(this.SelectedBrowser));
            //this.OnPropertyChanged(nameof(this.PreservationRule));
        }
    }
}
