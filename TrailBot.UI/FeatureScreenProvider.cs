using CascadePass.TrailBot.UI.Feature.Found;
using CascadePass.TrailBot.UI.Feature.TopicEditor;
using CascadePass.TrailBot.UI.Feature.WelcomeScreen;
using System.Windows;

namespace CascadePass.TrailBot.UI
{
    public static class FeaturecreenProvider
    {
        public static UIElement GetFirstScreen(bool setupRequired)
        {
            if (setupRequired)
            {
                return FeaturecreenProvider.GetWelcomeScreen();
            }

            return FeaturecreenProvider.GetFoundResults();
        }

        public static UIElement GetWelcomeScreen()
        {
            WelcomeSetupFeature view = new();

            return view;
        }

        public static UIElement GetFoundResults()
        {
            FoundFeature foundReportsGrid = new();

            if (foundReportsGrid.DataContext is ReaderViewModel foundReportsViewModel)
            {
                foundReportsViewModel.Settings = ApplicationData.Settings;
                foundReportsViewModel.WebProviderManager = ApplicationData.WebProviderManager;
            }

            return foundReportsGrid;
        }

        public static UIElement GetTopicEditorScreen()
        {
            TopicEditorFeature view = new();
            TopicEditorViewModel viewModel = (TopicEditorViewModel)view.DataContext;

            viewModel.Topics = ApplicationData.WebProviderManager.Topics.ToArray();

            return view;
        }
    }
}
