using System.ComponentModel;

namespace CascadePass.TrailBot.UI
{
    /// <summary>
    /// Base class for objects implementing <see cref="INotifyPropertyChanged"/>.
    /// </summary>
    public class ObservableObject : INotifyPropertyChanged
    {
        /// <summary>
        /// Event raised when the value of a property changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="propertyName">The name of the property whose
        /// value has changed.</param>
        internal virtual void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
