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

  public void Part1()
  {
    long sum = 0;
    HashSet<(int, int)> visited = new HashSet<(int, int)>();
    for (int y = 0; y < size.Y; ++y)
      for (int x = 0; x < size.X; ++x)
      {
        var at = (x, y);
        if (visited.Contains(at))
          continue;

        var size = Flood((x, y), visited);
        // Console.WriteLine($"Flooding {x},{y} for {size.Area} area, {size.Perim} perimeter");
        sum += size.Perim * size.Area;
      }

    Console.WriteLine($"Fencing total: {sum}");
  }
  public void Part2()
  {
  }

  readonly (int dX, int dY)[] links = new[] { (1, 0), (0, 1), (-1, 0), (0, -1) };
  public (int Area, int Perim) Flood((int X, int Y) from, HashSet<(int X, int Y)> visited)
  {
    char at = map[from.Y * size.X + from.X];
    visited.Add(from);
    
    (int Area, int Perim) ret = (1, 0);
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
        ret.Area += nextArea.Area;
        ret.Perim += nextArea.Perim;
      }
      else
      {
        ret.Perim += 1;
      }
    }

    return ret;
  }
}
