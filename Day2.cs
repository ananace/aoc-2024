namespace AdventOfCode2024;

public class Day2 : IAoC
{
  public int Day => 2;

  int[][] reports = new int[0][];

  public void Input(IEnumerable<string> lines)
  {
    reports = lines.Select(l => l.Split(' ').Select(p => int.Parse(p)).ToArray()).ToArray();
  }

  public void Part1()
  {
    int safeCount = reports.Where(report => CheckReport(report)).Count();
    Console.WriteLine($"Safe: {safeCount}");
  }
  public void Part2()
  {
    int safeCount = reports.Where(report => {
      if (CheckReport(report))
        return true;

      for (int i = 0; i < report.Length; ++i)
        if (CheckReport(report.Where((_, j) => j != i)))
          return true;

      return false;
    }).Count();

    Console.WriteLine($"Safe: {safeCount}");
  }

  bool CheckReport(IEnumerable<int> report)
  {
    var diffs = report.SkipLast(1).Zip(report.Skip(1)).Select(v => v.Second - v.First);
    return diffs.All(v => Math.Abs(v) <= 3) && (diffs.All(v => v > 0) || diffs.All(v => v < 0));
  }
}
