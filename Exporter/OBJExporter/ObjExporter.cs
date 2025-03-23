using System.IO;

namespace ModelExporter.Exporter.OBJExporter;

class ObjExporter : ICustomFace
{
    static readonly bool addColor = true;

    int faceCount;
    int triangleCount;
    private readonly Models.VertexChecker vertices;
    readonly List<int> triangles;
    readonly Models.ColorTransparency colorTransparency;
    readonly static bool moreTransparent = false;

    const string newMaterial = "newmtl {0}\r\nKa {1} {2} {3}\r\nKd {1} {2} {3}\r\nd {4}";

    const string materialLib = "mtllib {0}";
    const string useMaterial = "usemtl {0}";
    const string vertex = "v {0} {1} {2}";
    const string face = "f {0} {1} {2}";

    public ObjExporter()
    {
        faceCount = 0;
        triangleCount = 0;
        vertices = [];
        triangles = [];

        if (addColor)
            colorTransparency = [];
    }

    public int CustomFace(Face face, Color color, int shine, int transparency)
    {
        ++faceCount;
        if (addColor && colorTransparency.AddColorTransparency(color, shine, transparency))
            StoreColorTransparency(color, transparency);

        var mesh = face.Triangulate();
        int numTriangles = mesh.NumTriangles;

        for (int i = 0; i < numTriangles; ++i)
        {
            ++triangleCount;
            MeshTriangle mt = mesh.get_Triangle(i);
            StoreTriangle(mt);
        }

        return numTriangles;
    }

    public int FaceCount() => faceCount;

    public int TriangleCount()
    {
        if (!addColor)
            _ = triangles.Count;
        return triangleCount;
    }

    public int VertexCount() => vertices.Count;

    void StoreColorTransparency(Color color, int transparency)
    {
        triangles.Add(-1);
        triangles.Add(Utils.ColorTransparencyToInt(color, transparency));
        triangles.Add(0);
    }

    void StoreTriangle(MeshTriangle triangle)
    {
        for (int i = 0; i < 3; ++i)
        {
            XYZ p = triangle.get_Vertex(i);
            var q = new Models.PointDouble(p);
            triangles.Add(vertices.AddVertex(q));
        }
    }

    public void ExportTo(string path)
    {
        string materialLibraryPath = null;

        if (addColor)
        {
            materialLibraryPath = Path.ChangeExtension(path, "mtl");
            using StreamWriter stream = new(materialLibraryPath);
            foreach (int key in colorTransparency.Keys)
                ColorTransparency(stream, key);
        }

        using (StreamWriter stream = new(path))
        {
            if (addColor)
                stream.WriteLine(materialLib, Path.GetFileName(materialLibraryPath));

            foreach (Models.PointDouble key in vertices.Keys)
                WriteVertex(stream, key);

            int i = 0;
            int trianglesCount = triangles.Count;

            while (i < trianglesCount)
            {
                int i1 = triangles[i++];
                int i2 = triangles[i++];
                int i3 = triangles[i++];

                if (-1 == i1)
                    WriteColorTransparency(stream, i2);
                else
                    WriteFace(stream, i1, i2, i3);
            }
        }
    }

    static void ColorTransparency(StreamWriter s, int clr)
    {
        var color = Utils.IntToColorTransparency(clr, out int transparency);
        var name = Utils.ColorTransparencyString(color, transparency);

        if (moreTransparent && 0 < transparency)
            transparency = 100;

        s.WriteLine(newMaterial, name, color.Red / 256.0, color.Green / 256.0, color.Blue / 256.0, (100 - transparency) / 100.0);
    }

    static void WriteVertex(StreamWriter s, Models.PointDouble p)
    {
        s.WriteLine(vertex, p.X, p.Y, p.Z);
    }

    static void WriteColorTransparency(StreamWriter s, int clr)
    {
        var color = Utils.IntToColorTransparency(clr, out int transparency);
        var name = Utils.ColorTransparencyString(color, transparency);
        s.WriteLine(useMaterial, name);
    }

    static void WriteFace(StreamWriter stream, int i, int j, int k)
    {
        stream.WriteLine(face, i + 1, j + 1, k + 1);
    }
}
