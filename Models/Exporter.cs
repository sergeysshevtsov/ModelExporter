using ModelExporter.Commands;
using ModelExporter.Exporter.OBJExporter;

namespace ModelExporter.Models;

class Exporter
{
    public Exporter(ICustomFace customFace, FilteredElementCollector collector, Options options)
    {
        int elementCount = 0;
        int solidCount = 0;

        foreach (Element e in collector)
        {
            elementCount += ExportElement(customFace, e, options, ref solidCount);
        }
    }

    int ExportElement(ICustomFace customFace, Element element, Options options, ref int solidCount)
    {
        if (element is Group group)
        {
            int count = 0;
            foreach (ElementId id in group.GetMemberIds())
            {
                var groupElement = element.Document.GetElement(id);
                count += ExportElement(customFace, groupElement, options, ref solidCount);
            }

            return count;
        }

        Utils.ElementDescription(element);

        var category = element.Category;

        if (null == category)
        {
            return 0;
        }

        var material = category.Material;

        var color = material?.Color;
        var transparency = null == material ? 0 : material.Transparency;
        var shininess = null == material ? 0 : material.Shininess;

        solidCount += ExportSolids(customFace, element, options, color, shininess, transparency);

        return 1;
    }

    int ExportSolids(ICustomFace customFace, Element element, Options options, Color color, int shine, int transparency)
    {
        int solidCount = 0;
        var geometry = element.get_Geometry(options);
        Solid solid;

        if (null != geometry)
        {
            Document doc = element.Document;
            if (element is FamilyInstance)
            {
                geometry = geometry.GetTransformed(Transform.Identity);
            }
            GeometryInstance instance = null;

            foreach (GeometryObject geometryObject in geometry)
            {
                solid = geometryObject as Solid;

                if (null != solid &&
                    0 < solid.Faces.Size &&
                    ExportSolid(customFace, doc, solid, color, shine, transparency))
                {
                    ++solidCount;
                }

                instance = geometryObject as GeometryInstance;
            }

            if (0 == solidCount &&
                null != instance)
            {
                geometry = instance.GetSymbolGeometry();
                //t = inst.Transform;

                foreach (GeometryObject obj in geometry)
                {
                    solid = obj as Solid;
                    if (null != solid &&
                        0 < solid.Faces.Size &&
                        ExportSolid(customFace, doc, solid, color, shine, transparency))
                    {
                        ++solidCount;
                    }
                }
            }
        }
        return solidCount;
    }

    bool ExportSolid(ICustomFace customFace, Document document, Solid solid, Color color, int shine, int transparency)
    {
        Material material;
        Color clr;
        int shn;
        int trs;

        foreach (Face face in solid.Faces)
        {
            material = document.GetElement(face.MaterialElementId) as Material;
            clr = null == material ? color : material.Color;
            shn = null == material ? shine : material.Shininess;
            trs = null == material ? transparency : material.Transparency;

            customFace.CustomFace(face, clr ?? CmdModelExporter.DefaultColor, shn, trs);
        }

        return true;
    }
}
