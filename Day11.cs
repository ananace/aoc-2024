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
    var mut = stones.ToList();

    for (int i = 0; i < 25; ++i)
      Blink(mut);

    Console.WriteLine($"Stones: {mut.Count}");
  }
  public void Part2()
  {
    var mut = stones.ToList();

    Console.Write("Calc: ");
    for (int i = 0; i < 75; ++i)
    {
      var before = mut.Count;
      Console.Write($"{i} ({mut.Count} ");
      Blink(mut);

      var after = mut.Count;
      Console.Write($"{after - before}) ");
    }
    Console.WriteLine();

    Console.WriteLine($"Stones: {mut.Count}");
  }

  public void Blink(List<long> stones)
  {
    for (int i = 0; i < stones.Count; ++i)
    {
      var stone = stones[i];
      var str = stone.ToString();
      if (stone == 0)
        stones[i] = 1;
      else if (str.Length % 2 == 0)
      {
        stones.RemoveAt(i);
        stones.Insert(i, long.Parse(str[..(str.Length/2)]));
        ++i;
        stones.Insert(i, long.Parse(str[(str.Length/2)..]));
      }
      else
        stones[i] *= 2024;
    }
  }
}
