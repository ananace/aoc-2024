
namespace AdventOfCode2024;

public class Day7 : IAoC
{
  public int Day => 7;

  Dictionary<long, int[]> data = new Dictionary<long, int[]>();

  public void Input(IEnumerable<string> lines)
  {
    foreach (var line in lines)
    {
      var parts = line.Split(':');

      data[long.Parse(parts.First())] = parts.Last().Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(p => int.Parse(p.Trim())).ToArray();
    }
  }

  public void Part1()
  {
    var correct = data.Where(kv => CalcPart(kv.Key, kv.Value, 0, 0)).Select(kv => kv.Key).Sum();

    Console.WriteLine($"Correct: {correct}");
  }
  public void Part2()
  {
    var correct = data.AsParallel().Where(kv => CalcPart2(kv.Key, kv.Value, 0, 0)).Select(kv => kv.Key).Sum();

    Console.WriteLine($"Correct: {correct}");
  }

  public bool CalcPart(long res, int[] num, int it, long carried)
  {
    var next = num[it];
    if (it == num.Length - 1)
      return res == carried + next || res == carried * next;
    return CalcPart(res, num, it + 1, carried + next) || CalcPart(res, num, it + 1, carried * next);
  }

  public bool CalcPart2(long res, int[] num, int it, long carried)
  {
    var next = num[it];
    // Get the next 10 logarithm for the number, expand the carried by 10^<next 10log>, add the two together
    // For 123 || 45
    // 45 => 10log(1) + 1 => 2
    // 123 * 10^2 => 12300 + 45 => 12345
    long combined = carried * (long)Math.Pow(10, Math.Floor(Math.Log10(next) + 1)) + next;
    if (it == num.Length - 1)
      return res == carried + next || res == carried * next || res == combined;
    return CalcPart2(res, num, it + 1, carried + next) || CalcPart2(res, num, it + 1, carried * next) || CalcPart2(res, num, it + 1, combined);
  }
}
