namespace AlphaX.Sheets
{
    public interface IStyleProvider
    {
        /// <summary>
        /// Adds a new named style object.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        void AddNamedStyle(string name, NamedStyle style);
        /// <summary>
        /// Gets the named style.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        NamedStyle GetNamedStyle(string name);
    }
}
