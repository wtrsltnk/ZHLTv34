namespace Zhlt.Options;

public abstract record CommonOptions
{
    public int?         Threads  { get; init; }
    public ToolPriority Priority { get; init; } = ToolPriority.Normal;
    public bool         Verbose  { get; init; }
    public bool         Chart    { get; init; }
    public bool         Log      { get; init; } = true;

    internal IEnumerable<string> BuildCommonArgs()
    {
        if (Threads.HasValue)    { yield return "-threads"; yield return Threads.Value.ToString(); }
        if (Priority == ToolPriority.Low)  yield return "-low";
        if (Priority == ToolPriority.High) yield return "-high";
        if (Verbose)             yield return "-verbose";
        if (Chart)               yield return "-chart";
        if (!Log)                yield return "-nolog";
    }
}
