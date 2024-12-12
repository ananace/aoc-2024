namespace AdventOfCode2024;

public class Day12 : IAoC
{
  public int Day => 12;

  char[] map = new char[0];
  (int X, int Y) size = (0, 0);

  public void Input(IEnumerable<string> lines)
  {
    map = string.Concat(lines).ToCharArray();
    size = (lines.First().Length, lines.Count());
  }

  Dictionary<HashSet<(int,int)>,int> areas = new Dictionary<HashSet<(int,int)>,int>();
  public void PreCalc()
  {
    HashSet<(int, int)> visited = new HashSet<(int, int)>();
    for (int y = 0; y < size.Y; ++y)
      for (int x = 0; x < size.X; ++x)
      {
        var at = (x, y);
        if (visited.Contains(at))
          continue;

        var area = Flood((x, y), visited);
        areas[area.Area] = area.Perim;
      }
  }

  public void Part1()
  {
    int sum = areas.Select(kv => kv.Key.Count * kv.Value).Sum();

    Console.WriteLine($"Fencing total: {sum}");
  }
  public void Part2()
  {
    int sum = areas.Select(kv => kv.Key.Count * countCorners(kv.Key)).Sum();

    Console.WriteLine($"Fencing total: {sum}");
  }

  readonly (int dX, int dY)[] links = new[] { (1, 0), (0, 1), (-1, 0), (0, -1) };
  (HashSet<(int,int)> Area, int Perim) Flood((int X, int Y) from, HashSet<(int X, int Y)> visited)
  {
    char at = map[from.Y * size.X + from.X];

    (HashSet<(int,int)> Area, int Perim) ret = (new HashSet<(int,int)>(), 0);
    visited.Add(from);
    ret.Area.Add(from);

    foreach (var link in links)
    {
      (int X, int Y) newAt = (from.X + link.dX, from.Y + link.dY);
      char offset ;
      if (newAt.X < 0 || newAt.X >= size.X || newAt.Y < 0 || newAt.Y >= size.Y)
        offset = '\0';
      else
        offset = map[newAt.Y * size.X + newAt.X];

      if (offset == at)
      {
        if (visited.Contains(newAt))
          continue;

        var nextArea = Flood(newAt, visited);
        ret.Area.UnionWith(nextArea.Area);
        ret.Perim += nextArea.Perim;
      }
      else
      {
        ret.Perim += 1;
      }
    }

    return ret;
  }

  readonly (int dX, int dY)[] cornerLinks = new[] { (0, 0), (1, 0), (1, 1), (0, 1) };
  readonly int[] diagonalValues = new[] { (2 << 0) + (2 << 2), (2 << 1) + (2 << 3) };
  int countCorners(HashSet<(int X, int Y)> points)
  {
    int corners = 0;
    var bounds = findBounds(points);
    for (int y = bounds.minY; y < bounds.maxY; ++y)
      for (int x = bounds.minX; x < bounds.maxX; ++x)
      {
        var atPoint = cornerLinks.Select(c => points.Contains((x + c.dX, y + c.dY)));
        var before = corners;
        if (atPoint.Where(c => c).Count() % 2 == 1)
          corners++;
        else if (diagonalValues.Contains(atPoint.Select((c, i) => c ? (2 << i) : 0).Sum()))
          corners += 2;
      }

    return corners;
  }

  (int minX, int minY, int maxX, int maxY) findBounds(HashSet<(int X, int Y)> points)
  {
    (int minX, int minY, int maxX, int maxY) ret = (int.MaxValue, int.MaxValue, int.MinValue, int.MinValue);
    foreach (var point in points)
    {
      ret.minX = Math.Min(ret.minX, point.X);
      ret.minY = Math.Min(ret.minY, point.Y);
      ret.maxX = Math.Max(ret.maxX, point.X);
      ret.maxY = Math.Max(ret.maxY, point.Y);
    }

    ret.minX--;
    ret.minY--;
    ret.maxX++;
    ret.maxY++;

    return ret;
  }
}
