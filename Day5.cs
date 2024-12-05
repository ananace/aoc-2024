namespace AdventOfCode2024;

public class Day5 : IAoC
{
  public int Day => 5;

  Dictionary<int, List<int>> ordering = new Dictionary<int, List<int>>();
  List<int[]> updates = new List<int[]>();

  class OrderComparer : IComparer<int>
  {
    Dictionary<int, List<int>> ordering;

    public OrderComparer(Dictionary<int, List<int>> ordering)
    {
      this.ordering = ordering;
    }

    public int Compare(int x, int y)
    {
      if (ordering.ContainsKey(x))
      {
        if (ordering[x].Contains(y))
          return -1;
      }
      if (ordering.ContainsKey(y))
      {
        if (ordering[y].Contains(x))
          return 1;
      }
      return 0;
    }
  }

  public void Input(IEnumerable<string> lines)
  {
    foreach (var pair in lines.TakeWhile(l => l.Contains('|')).Select(l => l.Split('|').Select(w => int.Parse(w))))
    {
      if (!ordering.ContainsKey(pair.First()))
        ordering[pair.First()] = new List<int>();
      ordering[pair.First()].Add(pair.Last());
    }
    foreach (var update in lines.SkipWhile(s => s.Contains('|') || string.IsNullOrWhiteSpace(s)))
      updates.Add(update.Split(',').Select(w => int.Parse(w)).ToArray());
  }

  public void Part1()
  {
    int correct = 0;
    var comparer = new OrderComparer(ordering);
    foreach (var update in updates)
    {
      var ordered = update.Order(comparer);
      if (update.SequenceEqual(ordered))
        correct += ordered.Skip(ordered.Count() / 2).First();
    }

    Console.WriteLine($"Sum: {correct}");
  }
  public void Part2()
  {
    int incorrect = 0;
    var comparer = new OrderComparer(ordering);
    foreach (var update in updates)
    {
      var ordered = update.Order(comparer);
      if (!update.SequenceEqual(ordered))
        incorrect += ordered.Skip(ordered.Count() / 2).First();
    }

    Console.WriteLine($"Sum: {incorrect}");
  }

}
