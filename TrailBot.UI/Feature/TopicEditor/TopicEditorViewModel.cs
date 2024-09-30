using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace CascadePass.TrailBot.UI.Feature.TopicEditor
{
    public class TopicEditorViewModel : ViewModel
    {
        private ObservableCollection<TopicViewModel> topicViewModels;
        private Topic[] topics;
        private DelegateCommand addNewTopic;

        public TopicEditorViewModel()
        {
        }

        public Topic[] Topics {
            get => this.topics;
            set
            {
                if (this.topics != value)
                {
                    this.topics = value;
                    this.OnPropertyChanged(nameof(this.Topics));
                    this.TopicViewModels = this.GetTopicViewModels(value);
                }
            }
        }

        public ObservableCollection<TopicViewModel> TopicViewModels
        {
            get => this.topicViewModels;
            set
            {
                if (this.topicViewModels != value)
                {
                    this.topicViewModels = value;
                    this.OnPropertyChanged(nameof(this.TopicViewModels));
                }
            }
        }

        public ICommand AddTopicCommand => this.addNewTopic ??= new DelegateCommand(this.AddEmptyTopic);

        public ObservableCollection<TopicViewModel> GetTopicViewModels(IEnumerable<Topic> rawTopics)
        {
            ObservableCollection<TopicViewModel> result = new();

            foreach (Topic t in rawTopics)
            {
                TopicViewModel topicViewModel = new() { Topic = t, IsExpanded = rawTopics.Count() < 2 };

                topicViewModel.Deleted += this.TopicViewModel_Deleted;
                result.Add(topicViewModel);
            }

            return result;
        }

        public void ExpandTopic(string name)
        {
            var found = this.TopicViewModels.FirstOrDefault(m => string.Equals(m.Name, name, System.StringComparison.OrdinalIgnoreCase));

            if (found != null)
            {
                found.IsExpanded = true;
            }
        }

        public void AddEmptyTopic()
        {
            Topic topic = new();
            TopicViewModel topicViewModel = new() { Topic = topic, IsExpanded = true };
            topicViewModel.Deleted += this.TopicViewModel_Deleted;

            this.TopicViewModels.Add(topicViewModel);
            ApplicationData.WebProviderManager.Topics.Add(topic);
        }

        private void TopicViewModel_Deleted(object sender, System.EventArgs e)
        {
            TopicViewModel topicViewModel = sender as TopicViewModel;
            this.TopicViewModels.Remove(topicViewModel);
            ApplicationData.WebProviderManager.Topics.Remove(topicViewModel.Topic);
        }
    }
}
