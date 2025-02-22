public struct CoordRule
{
    public int Quadrant { get; }

    public int Shape { get; }

    public int Solution { get; }

    public CoordRule(int quadrant, int shape, int solution)
    {
        Quadrant = quadrant;
        Shape = shape;
        Solution = solution;
    }
}