using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace AlphaXSpreadSamplesExplorer
{
    /// <summary>
    /// Interaction logic for SamplesSideBar.xaml
    /// </summary>
    public partial class SamplesSideBar : UserControl
    {
        private Dictionary<string, Type> _samples;

        public event EventHandler<SampleSelectedEventArgs> SampleSelected;

        public SamplesSideBar()
        {
            InitializeComponent();
            _samples = new Dictionary<string, Type>();
            _lbSamples.ItemsSource = _samples;
            _lbSamples.Loaded += (s, e) => _lbSamples.SelectedIndex = 0;
        }

        public void RegisterSample(string header, Type sample)
        {
            _samples.Add(header, sample);           
        }

        private void OnSampleSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = (KeyValuePair<string, Type>)_lbSamples.SelectedItem;
            SampleSelected?.Invoke(this, new SampleSelectedEventArgs(selectedItem.Value));
        }
    }

    public class SampleSelectedEventArgs : EventArgs
    {
        public Type Sample { get; }

        public SampleSelectedEventArgs(Type sample)
        {
            Sample = sample;
        }
    }
}
