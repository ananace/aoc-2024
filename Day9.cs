namespace AdventOfCode2024;

public class Day9 : IAoC
{
  public int Day => 9;

  int[] layout = new int[0];
  public void Input(IEnumerable<string> lines)
  {
    layout = string.Join("", lines).ToCharArray().Select(c => int.Parse(c.ToString())).ToArray();
  }

  public void Part1()
  {
    ushort?[] blocks = BuildBlockmap().ToArray();
    // Console.WriteLine($"Blockmap: {string.Concat(blocks.Select(s => s?.ToString() ?? "."))}");

    var it = 0;
    for (var i = blocks.Length - 1; i > it; i--)
    {
      if (blocks[i] == null)
        continue;

      while (it < blocks.Length && blocks[it] != null)
        ++it;

      if (it >= blocks.Length)
        break;

      (blocks[it], blocks[i]) = (blocks[i], null);
    // Console.WriteLine($"Blockmap: {string.Concat(blocks.Select(s => s?.ToString() ?? "."))}");
    }

    // Console.WriteLine($"Blockmap: {string.Concat(blocks.Select(s => s?.ToString() ?? "."))}");

    long checksum = 0;
    foreach (var part in blocks.OfType<ushort>().Select((b, i) => i * b))
      checksum += part;

    Console.WriteLine($"Checksum: {checksum}");
  }
  public void Part2()
  {
    var sparse = BuildSparsemap().ToList();

    // Console.WriteLine($"Sparsemap: {string.Join(" ", sparse.Select(s => $"({s.Item1}, {s.Item2})"))}");
    // Console.WriteLine($"Blockmap: {string.Concat(sparse.Select(s => String.Concat(Enumerable.Repeat(s.Item1?.ToString()[0] ?? '.', s.Item2))))}");

    for (var i = sparse.Count - 1; i >= 0; i--)
    {
      if (sparse[i].Item1 == null)
        continue;

      for (var j = 0; j < i; ++j)
      {
        if (sparse[j].Item1 != null)
          continue;

        if (sparse[i].Item2 > sparse[j].Item2)
          continue;

        var size = sparse[j].Item2;
        size -= sparse[i].Item2;

        (sparse[j], sparse[i]) = (sparse[i], (null, sparse[i].Item2));

        if (i + 1 < sparse.Count && sparse[i + 1].Item1 == null)
        {
          sparse[i] = (null, (ushort)(sparse[i].Item2 + sparse[i + 1].Item2));
          sparse.RemoveAt(i + 1);
        }

        if (sparse[i - 1].Item1 == null)
        {
          sparse[i - 1] = (null, (ushort)(sparse[i - 1].Item2 + sparse[i].Item2));
          sparse.RemoveAt(i);
        }

        if (size > 0)
          sparse.Insert(j + 1, (null, size));

        j = i + 1;
      }
    }

    int ind = 0;
    long checksum = 0;
    foreach (var (val, cnt) in sparse)
      for (var i = 0; i < cnt; ++i)
      {
        checksum += (val ?? 0) * ind;
        ++ind;
      }

    Console.WriteLine($"Checksum: {checksum}");
  }

  IEnumerable<ushort?> BuildBlockmap()
  {
    ushort blockit = 0;
    bool block = true;
    foreach (var value in layout)
    {
      for (int i = 0; i < value; ++i)
        yield return block ? blockit : null;
      if (block)
        blockit++;
      block = !block;
    }
  }

  IEnumerable<(ushort?, ushort)> BuildSparsemap()
  {
    ushort blockit = 0;
    bool block = true;
    foreach (var value in layout)
    {
      if (block)
        yield return (blockit++, (ushort)value);
      else
        yield return (null, (ushort)value);
      block = !block;
    }
  }
}
