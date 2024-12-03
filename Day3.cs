using System.Text.RegularExpressions;

namespace AdventOfCode2024;

public class Day3 : IAoC
{
  public int Day => 3;

  List<string> instructions = new List<string>();


  public void Input(IEnumerable<string> lines)
  {
    foreach (var line in lines)
      instructions.AddRange(Regex.Matches(line, "(mul)\\((\\d+),(\\d+)\\)|(do)\\(\\)|(don't)\\(\\)").Select(m => m.Value));
  }

  public void Part1()
  {
    var sum = instructions.Select(mul => Regex.Match(mul, "(\\d+),(\\d+)").Groups.Values.Skip(1).Select(g => int.Parse(g.Value))).Select(cc => cc.Aggregate(1, (acc, val) => acc * val)).Sum();
    Console.WriteLine($"Sum: {sum}");
  }
  public void Part2()
  {
    bool enabled = true;
    long sum = 0;
    foreach(var inst in instructions)
    {
      if (inst.StartsWith("don't"))
        enabled = false;
      else if (inst.StartsWith("do"))
        enabled = true;
      else if (enabled)
        sum += Regex.Match(inst, "(\\d+),(\\d+)").Groups.Values.Skip(1).Select(g => int.Parse(g.Value)).Aggregate(1, (acc, val) => acc * val);
    }
    Console.WriteLine($"Sum: {sum}");
  }
}
