namespace AdventOfCode2024;

public class Day11 : IAoC
{
  public int Day => 11;

  List<long> stones = new List<long>();

  public void Input(IEnumerable<string> lines)
  {
    stones = string.Concat(lines).Split(' ').Select(v => long.Parse(v)).ToList();
  }

  public void Part1()
  {
    var expanded = TryExpand(stones, 25);

    Console.WriteLine($"Stones: {expanded}");
  }
  public void Part2()
  {
    var expanded = TryExpand(stones, 75);

    Console.WriteLine($"Stones: {expanded}");
  }

  public long TryExpand(IEnumerable<long> stones, int steps)
  {
    if (steps == 0)
      return stones.Count();

    return stones.Select(s => TryExpand(s, steps)).Sum();
  }
  Dictionary<(long, int), long> cache = new Dictionary<(long, int), long>();
  public long TryExpand(long stone, int steps)
  {
    var key = (stone, steps);
    if (cache.ContainsKey(key))
      return cache[key];

    var result = TryExpand(Blink(stone), steps - 1);
    cache[key] = result;
    return result;
  }

  public IEnumerable<long> Blink(long stone)
  {
    if (stone == 0)
    {
      yield return 1;
      yield break;
    }
    var str = stone.ToString();
    if (str.Length % 2 == 0)
    {
      yield return long.Parse(str[..(str.Length / 2)]);
      yield return long.Parse(str[(str.Length / 2)..]);
      yield break;
    }
    yield return stone * 2024;
  }
}
