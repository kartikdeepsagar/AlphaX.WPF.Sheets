using AlphaXSpreadSamplesExplorer.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Data;

namespace AlphaXSpreadSamplesExplorer
{
    /// <summary>
    /// Interaction logic for SamplesSideBar.xaml
    /// </summary>
    public partial class SamplesSideBar : UserControl
    {
        private ObservableCollection<SampleItem> _samplesList;
        private ICollectionView _collectionView;

        public event EventHandler<SampleSelectedEventArgs> SampleSelected;

        public SamplesSideBar()
        {
            InitializeComponent();
            _samplesList = new ObservableCollection<SampleItem>();
            _collectionView = CollectionViewSource.GetDefaultView(_samplesList);
            _collectionView.GroupDescriptions.Add(new PropertyGroupDescription("Category"));
            _collectionView.Filter = FilterSampleItem;

            _lbSamples.ItemsSource = _collectionView;
            _lbSamples.Loaded += (s, e) =>
            {
                if (_lbSamples.Items.Count > 0)
                    _lbSamples.SelectedIndex = 0;
            };
        }

        public void RegisterSample(string key, string title, string description, string category, string iconData, Type sample)
        {
            var item = new SampleItem(key, title, description, category, iconData, sample);
            _samplesList.Add(item);
        }

        public void RegisterSample(string header, Type sample)
        {
            RegisterSample(header, header, "Sample demonstration", "General", null, sample);
        }

        private bool FilterSampleItem(object item)
        {
            if (string.IsNullOrWhiteSpace(_txtSearch?.Text))
                return true;

            if (item is SampleItem sampleItem)
            {
                string search = _txtSearch.Text.Trim();
                return (sampleItem.Title != null && sampleItem.Title.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0) ||
                       (sampleItem.Description != null && sampleItem.Description.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0) ||
                       (sampleItem.Category != null && sampleItem.Category.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0);
            }

            return false;
        }

        private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            _collectionView?.Refresh();
            if (_lbSamples.SelectedIndex < 0 && _lbSamples.Items.Count > 0)
            {
                _lbSamples.SelectedIndex = 0;
            }
        }

        private void OnSampleSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_lbSamples.SelectedItem is SampleItem selectedItem)
            {
                SampleSelected?.Invoke(this, new SampleSelectedEventArgs(selectedItem));
            }
        }
    }

    public class SampleSelectedEventArgs : EventArgs
    {
        public SampleItem SampleItem { get; }
        public Type Sample => SampleItem?.SampleType;

        public SampleSelectedEventArgs(SampleItem item)
        {
            SampleItem = item;
        }

        public SampleSelectedEventArgs(Type sample)
        {
            SampleItem = new SampleItem(sample.Name, sample.Name, string.Empty, "General", null, sample);
        }
    }
}
