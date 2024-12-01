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
    var orderedLeft = _LeftList.Order();
    var orderedRight = _RightList.Order();
    var sum = 0;

    for (int i = 0; i < _LeftList.Count; ++i)
    {
      var left = orderedLeft.Skip(i).First();
      var right = orderedRight.Skip(i).First();
      var dist = Math.Abs(left - right);

      sum += dist;
    }

    Console.WriteLine($"Sum: {sum}");
  }
  public void Part2()
  {
    var sum = 0;
    foreach (var left in _LeftList)
    {
      var count = _RightList.Where(i => i == left).Count();

      sum += left * count;
    }

    Console.WriteLine($"Sum: {sum}");
  }
}
