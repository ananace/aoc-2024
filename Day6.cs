namespace AdventOfCode2024;

public class Day6 : IAoC
{
  public int Day => 6;

  public struct Point : IEquatable<Point>
  {
    public int X { get; set; }
    public int Y { get; set; }

    public Point(int x = 0, int y = 0) {
      X = x;
      Y = y;
    }

    public static Point operator+(Point l, Point r) {
      return new Point(l.X + r.X, l.Y + r.Y);
    }
    public static Point operator-(Point l, Point r) {
      return new Point(l.X - r.X, l.Y - r.Y);
    }
    public bool Equals(Point other)
    {
      return X == other.X && Y == other.Y;
    }
  }

  Point size = new Point(0, 0);
  HashSet<Point> obstructions = new HashSet<Point>();
  Point guard = new Point(0, 0);

  enum Direction
  {
    Up = 0,
    Right,
    Down,
    Left
  }

  public void Input(IEnumerable<string> lines)
  {
    size = new Point(lines.First().Length, lines.Count());
    char[] map = string.Join("", lines).ToCharArray();

    for (int y = 0; y < size.Y; ++y)
      for (int x = 0; x < size.X; ++x)
      {
        int pos = y * size.X + x;
        char at = map[pos];
        if (at == '#')
          obstructions.Add(new Point(x, y));
        else if (at == '^')
          guard = new Point(x, y);
      }
  }

  List<Point> path = new List<Point>();
  public void PreCalc()
  {
    path = WalkArea().path.Distinct().ToList();
  }

  public void Part1()
  {
    Console.WriteLine($"Visited {path.Count} points");
  }
  public void Part2()
  {
    HashSet<Point> loopPoints = new HashSet<Point>();
    foreach (var point in path)
    {
      if (point.Equals(guard))
        continue;

      if (WalkArea(point).loop)
        loopPoints.Add(point);
    }

    Console.WriteLine($"Valid loop points: {loopPoints.Count}");
  }

  (IEnumerable<Point> path, bool loop) WalkArea(Point? obstruction = null)
  {
    HashSet<(Point, Direction)> loopDetect = new HashSet<(Point, Direction)>();

    Point at = guard;
    Direction dir = Direction.Up;
    while (true)
    {
      if (!loopDetect.Add((at, dir)))
        return (loopDetect.Select(p => p.Item1), true);

      Point next = at;
      switch(dir)
      {
      case Direction.Up: next += new Point(0, -1); break;
      case Direction.Right: next += new Point(1, 0); break;
      case Direction.Down: next += new Point(0, 1); break;
      case Direction.Left: next += new Point(-1, 0); break;
      }

      if (next.X < 0 || next.Y < 0 || next.X >= size.X || next.Y >= size.Y)
        break;
      else if (obstructions.Contains(next) || (obstruction?.Equals(next) ?? false))
        dir = (Direction)(((int)dir + 1) % 4);
      else
        at = next;
    }

    return (loopDetect.Select(p => p.Item1), false);
  }
}
