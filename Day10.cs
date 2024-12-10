namespace AdventOfCode2024;

public class Day10 : IAoC
{
  public int Day => 10;

  int[] heights = new int[0];
  (int, int) size = (0, 0);

  public void Input(IEnumerable<string> lines)
  {
    size = (lines.First().Length, lines.Count());
    heights = string.Concat(lines).Select(c => int.Parse(c.ToString())).ToArray();
  }

  int trails = 0, trailheads = 0;
  public void PreCalc()
  {
    for (int y = 0; y < size.Item2; ++y)
      for (int x = 0; x < size.Item1; ++x)
        if (heights[y * size.Item1 + x] == 0)
        {
          var unique = new HashSet<(int, int)>();
          trails += CountTrails((x, y), unique);
          trailheads += unique.Count;
        }
  }

  public void Part1()
  {
    Console.WriteLine($"Trailheads: {trailheads}");
  }
  public void Part2()
  {
    Console.WriteLine($"Trails: {trails}");
  }

  int CountTrails((int, int) from, HashSet<(int,int)> unique)
  {
    int found = 0;

    List<(int,int)> toSearch = new List<(int, int)>();
    toSearch.Add(from);

    while (toSearch.Any())
    {
      var cur = toSearch.First();
      toSearch.RemoveAt(0);

      int height = heights[cur.Item2 * size.Item1 + cur.Item1];
      for (int y = -1; y <= 1; ++y)
        for (int x = -1; x <= 1; ++x)
        {
          if ((y != 0 && x != 0) || (y == 0 && x == 0))
            continue;

          var newAt = (cur.Item1 + x, cur.Item2 + y);
          if (newAt.Item1 < 0 || newAt.Item1 >= size.Item1 || newAt.Item2 < 0 || newAt.Item2 >= size.Item2)
            continue;

          int newHeight = heights[newAt.Item2 * size.Item1 + newAt.Item1];
          if (newHeight - height != 1)
            continue;

          if (newHeight == 9)
          {
            unique.Add(newAt);
            found++;
            continue;
          }

          toSearch.Add(newAt);
        }
    }

    return found;
  }
}
