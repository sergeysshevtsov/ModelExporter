namespace ModelExporter.Exporter.OBJExporter;

interface ICustomFace
{
    int CustomFace(Face face, Color color, int shininess, int transparency);
    int FaceCount();
    int TriangleCount();
    int VertexCount();
}
