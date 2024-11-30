namespace AdventOfCode2024;

public interface IAoC
{
  int Day { get; }

  void Input(IEnumerable<string> line);

  void Part1();
  void Part2();
}
