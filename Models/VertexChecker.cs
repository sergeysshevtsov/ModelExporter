namespace ModelExporter.Models;

class VertexChecker : Dictionary<PointDouble, int>
{
    class PointDoubleEqualityComparer : IEqualityComparer<PointDouble>
    {
        public bool Equals(PointDouble p, PointDouble q) => 0 == p.CompareTo(q);
        public int GetHashCode(PointDouble p) => $"{p.X},{p.Y},{p.Z}".GetHashCode();
    }

    public VertexChecker() : base(new PointDoubleEqualityComparer()) { }

    public int AddVertex(PointDouble p) => ContainsKey(p) ? this[p] : this[p] = Count;
}
