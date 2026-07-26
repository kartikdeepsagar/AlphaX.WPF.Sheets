using System.Windows;
using System.Windows.Controls;

namespace DevBrewLabs.WPF.Spreadsheet.Components
{
    public class SuggestionListBox : ListBox
    {
        static SuggestionListBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SuggestionListBox), new FrameworkPropertyMetadata(typeof(SuggestionListBox)));
        }
    }
}
