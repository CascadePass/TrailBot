using System;
using System.Diagnostics;

namespace CascadePass.TrailBot.UI
{
    /// <summary>
    /// Base class for all TrailBot ViewModels.
    /// </summary>
    public abstract class ViewModel : ObservableObject
    {
        internal void ViewInBrowser(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return;
            }

            try
            {
                Process.Start(url);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }
    }
}
