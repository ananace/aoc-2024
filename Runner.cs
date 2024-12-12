using System;
using System.Reflection;

namespace AdventOfCode2024;

public class Runner
{
  public bool ForceSample { get; set; } = false;
  public bool PrintTimes { get; set; } = false;

  public void RunLast()
  {
    RunDay(null);
  }
  public void RunAll()
  {
    foreach (var day in ImplementedDays.OrderBy(d => d.Day))
      RunFor(day);
  }

  public void RunDay(int? day)
  {
    bool executed = false;
    foreach (var d in ImplementedDays.OrderBy(d => d.Day).Reverse())
    {
      if (day == null || d.Day == day)
      {
        executed = true;
        RunFor(d);
        break;
      }
    }

    if (!executed)
      Console.WriteLine($"Failed to find implementation for day {day}");
  }

  public void RunFor(IAoC day)
  {
    int dayNum = day.Day;

    Console.WriteLine($"Running day {dayNum}...");
    var inputExts = new List<string>{ "inp.real", "inp", "inp.sample" };

    if (ForceSample)
      inputExts.Insert(0, inputExts.Last());

    var input = inputExts.Select(ext => $"Day{dayNum}.{ext}").FirstOrDefault(file => File.Exists(file));
    if (string.IsNullOrEmpty(input))
    {
      Console.WriteLine($"No input data for day {dayNum}");
      Environment.Exit(1);
    }

    Console.WriteLine("- Reading input...");
    var data = File.ReadLines(input);
    day.Input(data.Select(l => l.Trim()));

    DateTime before, after;

    if (day.GetType().GetMethod("PreCalc") is MethodInfo meth)
    {
      Console.WriteLine("- Running pre-calculate...");
      before = DateTime.Now;
      meth.Invoke(day, null);
      after = DateTime.Now;
      if (PrintTimes)
        Console.WriteLine($"<- {prettyTime(after - before)} ->");
    }

    Console.WriteLine("- Part 1...");
    before = DateTime.Now;
    day.Part1();
    after = DateTime.Now;
    if (PrintTimes)
      Console.WriteLine($"<- {prettyTime(after - before)} ->");
    Console.WriteLine("- Part 2...");
    before = DateTime.Now;
    day.Part2();
    after = DateTime.Now;
    if (PrintTimes)
      Console.WriteLine($"<- {prettyTime(after - before)} ->");
  }

  IEnumerable<IAoC> ImplementedDays {
    get
    {
      var impls = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.GetInterface(nameof(IAoC)) != null);

      foreach (var impl in impls)
      {
        IAoC? instance = null;

        try
        {
          instance = Activator.CreateInstance(impl) as IAoC;
        }
        catch (Exception ex)
        {
          Console.WriteLine($"Failed to create instance of {impl.FullName}, {ex}");
          continue;
        }

        if (instance == null)
          continue;

        yield return instance;
      }
    }
  }

  string prettyTime(TimeSpan span)
  {
    var sb = new System.Text.StringBuilder();
    if (span.TotalDays > 1)
      sb.Append(span.Days).Append("d");
    if (span.TotalHours > 1)
      sb.Append(span.Hours).Append("h");
    if (span.TotalMinutes > 1)
      sb.Append(span.Minutes).Append("m");
    if (span.TotalSeconds > 1)
      sb.Append(span.Seconds).Append("s");
    sb.Append(span.Milliseconds).Append("ms");

    return sb.ToString();
  }
}
