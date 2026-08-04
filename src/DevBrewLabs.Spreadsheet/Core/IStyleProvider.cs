using DevBrewLabs.Spreadsheet.Styling;

namespace DevBrewLabs.Spreadsheet
{
    public interface IStyleProvider
    {
        /// <summary>
        /// Adds a new named style object.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        void AddNamedStyle(string name, CellStyle style);
        /// <summary>
        /// Gets the named style.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        CellStyle GetNamedStyle(string name);
    }
}
