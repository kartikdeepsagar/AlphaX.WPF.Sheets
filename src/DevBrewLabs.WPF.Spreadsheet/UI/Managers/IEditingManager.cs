using System;
using System.Windows;

namespace DevBrewLabs.WPF.Spreadsheet.UI.Managers
{
    public interface IEditingManager
    {
        bool IsEditing { get; }
        /// <summary>
        /// Gets the active editor.
        /// </summary>
        FrameworkElement ActiveEditor { get; }
        /// <summary>
        /// Begins editing.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        void BeginEdit(int row, int column);
        /// <summary>
        /// Updates the active editor position, size, and font size based on ZoomFactor.
        /// </summary>
        void UpdateEditorLayout();
        /// <summary>
        /// Ends active editing.
        /// </summary>
        /// <param name="commitChanges"></param>
        /// <returns></returns>
        bool EndEdit(bool commitChanges);
    }
}
