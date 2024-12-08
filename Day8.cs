namespace AdventOfCode2024;

public static class LINQExt
{
  public static IEnumerable<(T,T)> PermutatePairs<T>(this IEnumerable<T> source) {
    return source.SelectMany(k => source.Where(v => !v?.Equals(k) ?? false).Select(v => (k, v)));
  }
}

public class Day8 : IAoC
{
  struct Antenna
  {
    public int X, Y;
    public char Frequency;
  }

  public int Day => 8;

  List<Antenna> antennae = new List<Antenna>();
  int width, height;

  public void Input(IEnumerable<string> lines)
  {
    char[] map = string.Join("", lines).ToCharArray();
    width = lines.First().Length;
    height = lines.Count();

    for (int y = 0; y < height; ++y)
      for (int x = 0; x < width; ++x)
      {
        char at = map[y * width + x];
        if (at == '.')
          continue;

        antennae.Add(new Antenna{ X = x, Y = y, Frequency = at });
      }
  }

  public void Part1()
  {
    HashSet<(int, int)> antinodes = new HashSet<(int, int)>();

    foreach (var antinode in antennae.GroupBy(k => k.Frequency).SelectMany(g => g.PermutatePairs()).SelectMany(v => GetOpposing(v.Item1, v.Item2)).Where(InRange))
      antinodes.Add(antinode);

    Console.WriteLine($"Unique antinodes: {antinodes.Count}");
  }
  public void Part2()
  {
    HashSet<(int, int)> antinodes = new HashSet<(int, int)>();

    foreach (var antennaePair in antennae.GroupBy(k => k.Frequency).SelectMany(g => g.PermutatePairs()))
    {
      // Iterate separately, to make the handling of bound exit easier
      foreach (var antinode in GetAllOpposing(antennaePair.Item1, antennaePair.Item2).TakeWhile(InRange))
        antinodes.Add(antinode);
      foreach (var antinode in GetAllOpposing(antennaePair.Item2, antennaePair.Item1).TakeWhile(InRange))
        antinodes.Add(antinode);
    }
    Console.WriteLine($"Unique antinodes: {antinodes.Count}");
  }

  bool InRange((int, int) point) {
    return point.Item1 >= 0 && point.Item1 < width && point.Item2 >= 0 && point.Item2 < height;
  }
  (int, int)[] GetOpposing(Antenna a, Antenna b) {
    return new[] { (a.X + (a.X - b.X), a.Y + (a.Y - b.Y)), (b.X + (b.X - a.X), b.Y + (b.Y - a.Y)) };
  }
  IEnumerable<(int, int)> GetAllOpposing(Antenna a, Antenna b) {
    (int, int) diff = (a.X - b.X, a.Y - b.Y);
    (int, int) at = (a.X, a.Y);
    yield return at;

    while (true)
    {
      at.Item1 += diff.Item1;
      at.Item2 += diff.Item2;

      yield return at;
    }
  }
}
