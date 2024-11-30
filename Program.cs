var runner = new AdventOfCode2024.Runner();

bool executed = false;
foreach (var arg in args)
{
  if (arg == "-s" || arg == "--sample")
  {
    Console.WriteLine("Forcing use of sample data.");
    runner.ForceSample = true;
  }
  else if (arg == "all")
  {
    executed = true;
    runner.RunAll();
  }
  else if (int.TryParse(arg, out int day))
  {
    executed = true;
    runner.RunDay(day);
  }
  else
  {
    Console.WriteLine($"Usage: {System.AppDomain.CurrentDomain.FriendlyName} [-s] [DAYS...]");
    Console.WriteLine();
    Console.WriteLine("  -s --sample    Use sample data even if real data is available");
    Console.WriteLine();
    Console.WriteLine("The keyword 'all' can be used to execute all implemented days.\nIf no day is specified then the last one will be run.");
    return;
  }
}

if (!executed)
  runner.RunLast();
