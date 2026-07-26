namespace DevBrewLabs.Spreadsheet.Core
{
    public interface IStylePalette
    {
        IStyle GetStyle(ushort styleId);
        ushort GetOrAdd(IStyle style);
        void Clear();
    }
}
