using System;
using System.IO;
using System.Linq;

namespace CascadePass.TrailBot.UI.Feature.WelcomeScreen
{
    public class WebDataProviderViewModel : SetupTaskViewModel
    {
        private WebDataProvider provider;
        private PreservationRule[] allowedPreservationRules;
        private bool requiresFilePath, useThisProvider;

        public WebDataProviderViewModel()
        {
            this.useThisProvider = true;
        }

        public WebDataProvider Provider
        {
            get => this.provider;
            set
            {
                if (this.provider != value)
                {
                    this.provider = value;
                    this.OnPropertyChanged(nameof(Provider));
                }
            }
        }

        public string DestinationFolder
        {
            get => this.provider?.DestinationFolder;
            set
            {
                if (this.provider == null)
                {
                    return;
                }

                if (!string.Equals(this.provider.DestinationFolder, value, StringComparison.OrdinalIgnoreCase))
                {
                    this.provider.DestinationFolder = value;
                    this.Validate();
                    this.OnPropertyChanged(nameof(this.DestinationFolder));
                }
            }
        }

        public PreservationRule PreservationRule
        {
            get => this.provider?.PreservationRule ?? PreservationRule.ValueNotSet;
            set
            {
                if (this.provider == null)
                {
                    return;
                }

                if (this.provider.PreservationRule != value)
                {
                    this.provider.PreservationRule = value;

                    this.CheckNeedForFilePath();
                    this.Validate();

                    this.OnPropertyChanged(nameof(this.Provider));

                    this.OnPropertyChanged(nameof(this.PreservationRuleDescription));
                    this.OnPropertyChanged(nameof(this.PreservationRuleWarning));
                    this.OnPropertyChanged(nameof(this.PreservationRuleInfo));
                }
            }
        }

        public PreservationRule[] AvailablePreservationRules => this.allowedPreservationRules ??= this.GetPreservationRules();

        public string PreservationRuleDescription => Guid.NewGuid().ToString();
        public string PreservationRuleWarning => Guid.NewGuid().ToString();
        public string PreservationRuleInfo => Guid.NewGuid().ToString();

        public bool NeedsFilePath
        {
            get => this.requiresFilePath;
            set
            {
                if (this.requiresFilePath != value)
                {
                    this.requiresFilePath = value;
                    this.OnPropertyChanged(nameof(this.NeedsFilePath));
                }
            }
        }

        public bool InUse
        {
            get => this.useThisProvider;
            set
            {
                if (this.useThisProvider != value)
                {
                    this.useThisProvider = value;
                    this.CheckNeedForFilePath();
                    this.OnPropertyChanged(nameof(this.InUse));
                }
            }
        }

        private PreservationRule[] GetPreservationRules()
        {
            var allRules = (PreservationRule[])Enum.GetValues(typeof(PreservationRule));

            return allRules.Where(m => m != PreservationRule.ValueNotSet).ToArray();
        }

        public override void Validate()
        {
            if (this.PreservationRule == PreservationRule.None)
            {
                this.IsComplete = true;
                return;
            }
            else if (this.PreservationRule == PreservationRule.Matching || this.PreservationRule == PreservationRule.All)
            {
                if (string.IsNullOrWhiteSpace(this.DestinationFolder))
                {
                    this.IsComplete = false;
                }
                else
                {
                    if (Directory.Exists(this.DestinationFolder))
                    {
                        this.IsComplete = FileStore.TestWriteAccess(this.DestinationFolder);
                    }
                    else
                    {
                        this.IsComplete = false;
                    }
                }
            }
        }

        private void CheckNeedForFilePath()
        {
            this.NeedsFilePath = this.InUse && this.provider.PreservationRule != PreservationRule.None;
        }
    }
}
