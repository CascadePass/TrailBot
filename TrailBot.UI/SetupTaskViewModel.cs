using System;
using System.Windows;
using System.Windows.Media;

namespace CascadePass.TrailBot.UI.Feature
{
    public abstract class SetupTaskViewModel : ViewModel
    {
        /// <summary>
        /// Event which is raised when a task becomes completed, or when
        /// required information is removed or invalidated, making the
        /// task not completed.
        /// </summary>
        public event EventHandler CompletenessChanged;

        private bool isComplete;

        /// <summary>
        /// Gets or sets a value indicating whether the task is completed.
        /// </summary>
        public bool IsComplete
        {
            get => this.isComplete;
            set
            {
                if (this.isComplete != value)
                {
                    this.isComplete = value;
                    this.OnPropertyChanged(nameof(this.IsComplete));
                    this.OnPropertyChanged(nameof(this.BackgroundBrush));

                    this.OnCompletenessChanged(this, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Gets a <see cref="Brush"/> to draw a section background with, based
        /// on the value of <see cref="IsComplete"/>.
        /// </summary>
        public Brush BackgroundBrush => (Brush)(this.isComplete ? Application.Current.MainWindow.Resources["TaskCompleted"] : Application.Current.MainWindow.Resources["TaskNotValid"]);

        /// <summary>
        /// Determines whether a setup task has been completed, meaning
        /// that all required information is present, any any other
        /// conditions have been met.
        /// </summary>
        /// <remarks>
        /// <see cref="Validate"/> should set <see cref="IsComplete"/>.
        /// </remarks>
        public abstract void Validate();

        /// <summary>
        /// Raises the <see cref="CompletenessChanged"/> event.
        /// </summary>
        /// <param name="sender">The object raising the event.</param>
        /// <param name="e">An <see cref="EventArgs"/>, not used.</param>
        public void OnCompletenessChanged(object sender, EventArgs e)
        {
            this.CompletenessChanged?.Invoke(sender, e);
        }
    }
}
