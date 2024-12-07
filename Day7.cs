
namespace AdventOfCode2024;

public class Day7 : IAoC
{
  public int Day => 7;

  List<(long, int[])> data = new List<(long, int[])>();

  public void Input(IEnumerable<string> lines)
  {
    foreach (var line in lines)
    {
      var parts = line.Split(':', StringSplitOptions.TrimEntries);

      data.Add((long.Parse(parts.First()), parts.Last().Split(' ').Select(int.Parse).ToArray()));
    }
  }

  public void Part1()
  {
    var correct = data.Where(kv => CalcPart(kv.Item1, kv.Item2)).Select(kv => kv.Item1).Sum();

    Console.WriteLine($"Correct: {correct}");
  }
  public void Part2()
  {
    var correct = data.AsParallel().Where(kv => CalcPart2(kv.Item1, kv.Item2)).Select(kv => kv.Item1).Sum();

    Console.WriteLine($"Correct: {correct}");
  }

  public bool CalcPart(long res, Span<int> num, long carried = 0)
  {
    var next = num[0];
    if (num.Length == 1)
      return res == carried + next || res == carried * next;
    return CalcPart(res, num.Slice(1), carried + next) || CalcPart(res, num.Slice(1), carried * next);
  }

  public bool CalcPart2(long res, Span<int> num, long carried = 0)
  {
    var next = num[0];
    // Get the next 10 logarithm for the number, expand the carried by 10^<next 10log>, add the two together
    // For 123 || 45
    // 45 => 10log(1) + 1 => 2
    // 123 * 10^2 => 12300 + 45 => 12345
    long combined = carried * (long)Math.Pow(10, Math.Floor(Math.Log10(next) + 1)) + next;
    if (num.Length == 1)
      return res == carried + next || res == carried * next || res == combined;
    return CalcPart2(res, num.Slice(1), carried + next) || CalcPart2(res, num.Slice(1), carried * next) || CalcPart2(res, num.Slice(1), combined);
  }
}
