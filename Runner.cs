using System;
using System.Reflection;

namespace AdventOfCode2024;

public class Runner
{
  public bool ForceSample { get; set; } = false;

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

    Console.WriteLine($"Running {dayNum}...");
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

    if (day.GetType().GetMethod("PreCalc") is MethodInfo meth)
    {
      Console.WriteLine("- Running pre-calculate...");
      meth.Invoke(day, null);
    }

    Console.WriteLine("- Part 1...");
    day.Part1();
    Console.WriteLine("- Part 2...");
    day.Part2();
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
}
