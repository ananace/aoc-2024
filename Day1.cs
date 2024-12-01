using System;

namespace AdventOfCode2024;

public class Day1 : IAoC
{
  public int Day => 1;

  List<int> _LeftList = new List<int>();
  List<int> _RightList = new List<int>();

  public void Input(IEnumerable<string> lines)
  {
    foreach (var line in lines)
    {
      var split = line.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(s => int.Parse(s));

      _LeftList.Add(split.First());
      _RightList.Add(split.Last());
    }
  }

  public void Part1()
  {
    Console.WriteLine($"Sum: {_LeftList.Order().Zip(_RightList.Order()).Select(v => Math.Abs(v.First - v.Second)).Sum()}");
  }
  public void Part2()
  {
    Console.WriteLine($"Sum: {_LeftList.Select(l => _RightList.Where(i => i == l).Count() * l).Sum()}");
  }
}
