using System.ComponentModel;

namespace DataVisualizationDemos
{
    static class Helpers
    {
        // Invoke the PropertyChanged event for an object
        public static void InvokePropertyChanged(PropertyChangedEventHandler propertyChanged, object sender, string propertyName)
        {
            var handler = propertyChanged;
            if (null != handler)
            {
                handler.Invoke(sender, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
