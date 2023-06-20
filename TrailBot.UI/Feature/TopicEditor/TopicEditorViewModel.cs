using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

namespace CascadePass.TrailBot.UI.Feature.TopicEditor
{
    public class TopicEditorViewModel : ViewModel
    {
        private ObservableCollection<TopicViewModel> topicViewModels;
        private Topic[] topics;

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
                    this.TopicViewModels = TopicEditorViewModel.GetTopicViewModels(value);
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

        public static ObservableCollection<TopicViewModel> GetTopicViewModels(IEnumerable<Topic> rawTopics)
        {
            ObservableCollection<TopicViewModel> result = new();

            foreach (Topic t in rawTopics)
            {
                result.Add(new() { Topic = t });
            }

            return result;
        }
    }
}
