using System.Windows;
using System.Windows.Controls;

namespace AlphaX.WPF.Sheets.Components
{
    public class SuggestionListBox : ListBox
    {
        static SuggestionListBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SuggestionListBox), new FrameworkPropertyMetadata(typeof(SuggestionListBox)));
        }
    }
}
