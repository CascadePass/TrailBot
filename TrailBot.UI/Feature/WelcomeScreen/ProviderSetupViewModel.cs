using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;

namespace CascadePass.TrailBot.UI.Feature.WelcomeScreen
{
    public class ProviderSetupViewModel : SetupTaskViewModel
    {
        private ProviderSetupViewModel() { }

        public ProviderSetupViewModel(WebProviderManager providerManager)
        {
            this.WebProviderManager = providerManager;
            this.Providers = new();
            this.GetProviders();
        }

        public WebProviderManager WebProviderManager { get; set; }

        public List<WebDataProviderViewModel> Providers { get; set; }

        private void GetProviders()
        {
            foreach (var provider in this.WebProviderManager.DataProviders)
            {
                WebDataProviderViewModel providerVM = new() { Provider = provider };
                this.Providers.Add(providerVM);

                this.Subscribe(providerVM);

                providerVM.PreservationRule = PreservationRule.Matching;
                providerVM.DestinationFolder = Path.Combine(Environment.CurrentDirectory, "TripReports");
            }
        }

        public override void Validate()
        {
            this.IsComplete = this.Providers.Any() && !this.Providers.Any(m => !m.IsComplete);
        }

        protected void Subscribe(object eventSource)
        {
            if (eventSource is WebDataProviderViewModel providerVM)
            {
                providerVM.PropertyChanged += this.ProviderVM_PropertyChanged;
            }
        }

        private void ProviderVM_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.Validate();
        }
    }
}
