using ModelExporter.Commands;

namespace ModelExporter.Models;

class ColorTransparency : Dictionary<int, int>
{
    int current;
    public ColorTransparency()
    {
        current = ModelExporter.Exporter.OBJExporter.Utils.ColorTransparencyToInt(CmdModelExporter.DefaultColor, 0);
    }

    public bool AddColorTransparency(Color color, int shininess, int transparency)
    {
        int trgb = ModelExporter.Exporter.OBJExporter.Utils.ColorTransparencyToInt(color, transparency);

        if (!ContainsKey(trgb))
            this[trgb] = Count;

        bool rc = !current.Equals(trgb);
        current = trgb;
        return rc;
    }
}
