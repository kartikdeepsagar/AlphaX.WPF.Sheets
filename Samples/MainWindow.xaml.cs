using AlphaXSpreadSamplesExplorer.Samples;
using System;
using System.Windows;

namespace AlphaXSpreadSamplesExplorer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            _samplesSideBar.RegisterSample("Testing", typeof(Testing));
            _samplesSideBar.RegisterSample("Styling", typeof(Styling));
            _samplesSideBar.RegisterSample("Cell Types", typeof(CellTypes));
            _samplesSideBar.RegisterSample("Scroll Modes", typeof(ScrollModes));
            _samplesSideBar.RegisterSample("Sorting", typeof(Sorting));
            _samplesSideBar.RegisterSample("DataBinding", typeof(DataBinding));
        }

        private void OnSampleSelected(object sender, SampleSelectedEventArgs e)
        {
            if (_samplesViewerBdr.Child != null)
                _samplesViewerBdr.Child = null;

            _samplesViewerBdr.Child = (FrameworkElement)Activator.CreateInstance(e.Sample);
        }
    }
}
