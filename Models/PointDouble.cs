namespace ModelExporter.Models;

class PointDouble : IComparable<PointDouble>
{
    public double X { get; set; }
    public double Y { get; set; }
    public double Z { get; set; }

    const double feetToMm = 25.4 * 12;

    public PointDouble(XYZ point)
    {
        X = ConvertFeetToMillimetres(point.X);
        Y = ConvertFeetToMillimetres(point.Y);
        Z = ConvertFeetToMillimetres(point.Z);
    }

    double ConvertFeetToMillimetres(double d) => feetToMm * d + 0.5;

    public int CompareTo(PointDouble a)
    {
        var d = X - a.X;
        if (0 == d)
        {
            d = Y - a.Y;
            if (0 == d)
                d = Z - a.Z;
        }

        return (int)d;
    }
}
