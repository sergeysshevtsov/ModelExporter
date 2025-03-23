namespace ModelExporter.Exporter.OBJExporter;

class Utils
{
    public static int ColorTransparencyToInt(Color color, int transparency)
    {
        uint trgb = ((uint)transparency << 24) | (uint)ColorToInt(color);
        return (int)trgb;
    }

    static int ColorToInt(Color color) => color.Red << 16 | color.Green << 8 | color.Blue;
    static Color IntToColor(int rgb) => new((byte)((rgb & 0xFF0000) >> 16), (byte)((rgb & 0xFF00) >> 8), (byte)(rgb & 0xFF));

    public static Color IntToColorTransparency(int trgb, out int transparency)
    {
        transparency = (int)((((uint)trgb) & 0xFF000000) >> 24);
        return IntToColor(trgb);
    }

    static string ColorString(Color color) => color.Red.ToString("X2") + color.Green.ToString("X2") + color.Blue.ToString("X2");
    public static string ColorTransparencyString(Color color, int transparency) => transparency.ToString("X2") + ColorString(color);

    public static string ElementDescription(Element e)
    {
        if (null == e)
        {
            return "<null>";
        }

        FamilyInstance fi = e as FamilyInstance;

        string typeName = e.GetType().Name;

        string categoryName = (null == e.Category) ? string.Empty : e.Category.Name + " ";
        string familyName = (null == fi) ? string.Empty : fi.Symbol.Family.Name + " ";
        string symbolName = (null == fi || e.Name.Equals(fi.Symbol.Name)) ? string.Empty : fi.Symbol.Name + " ";

        return string.Format("{0} {1}{2}{3}<{4} {5}>", typeName, categoryName, familyName, symbolName, e.Id.IntegerValue, e.Name);
    }

    public static string PluralSuffix(int n) => 1 == n ? "" : "s";
}
