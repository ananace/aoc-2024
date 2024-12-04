
namespace AdventOfCode2024;

public class Day4 : IAoC
{
  public int Day => 4;

  string wordsearch = "";
  int width;
  int height;

  public void Input(IEnumerable<string> lines)
  {
    wordsearch = string.Join("", lines);
    height = lines.Count();
    width = lines.First().Length;
  }

  public void Part1()
  {
    int words = 0;
    for (int y = 0; y < height; y++)
      for (int x = 0; x < width; x++)
        words += SearchFrom(x, y);

    Console.WriteLine($"Words: {words}");
  }
  public void Part2()
  {
    int words = 0;
    for (int y = 1; y < height - 1; y++)
      for (int x = 1; x < width - 1; x++)
        words += SearchCross(x, y);

    Console.WriteLine($"Crosses: {words}");
  }

  public int SearchFrom(int x, int y)
  {
    char at = wordsearch[y * width + x];
    if (at != 'X')
      return 0;

    int words = 0;
    for (int ydir = -1; ydir <= 1; ++ydir)
      for (int xdir = -1; xdir <= 1; ++xdir)
      {
        if (xdir == 0 && ydir == 0)
          continue;

        if (SearchWord(x, y, xdir, ydir))
          words++;
      }

    return words;
  }

  private readonly string word = "XMAS";
  public bool SearchWord(int x, int y, int xdir, int ydir)
  {
    int wordit = 0;
    while (true)
    {
      char at = wordsearch[y * width + x];
      if (at != word[wordit])
        return false;

      if (wordit == word.Length - 1)
        return true;

      wordit++;

      x += xdir;
      y += ydir;

      if (x < 0 || y < 0 || x >= width || y >= height)
        return false;
    }
  }

  public int SearchCross(int x, int y)
  {
    if (x == 0 || y == 0 || x == width - 1 || y == width - 1)
      return 0;

    char at = wordsearch[y * width + x];
    if (at != 'A')
      return 0;

    int found = 0;
    for (int ydir = -1; ydir <= 1; ++ydir)
      for (int xdir = -1; xdir <= 1; ++xdir)
      {
        if (xdir == 0 || ydir == 0)
          continue;

        if (wordsearch[(y + ydir) * width + (x + xdir)] != 'M')
          continue;
        if (wordsearch[(y - ydir) * width + (x - xdir)] != 'S')
          continue;

        found++;
      }

    if (found == 2)
      return 1;

    return 0;
  }
}
