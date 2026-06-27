namespace Zhlt.Options;

public record VisOptions : CommonOptions
{
    public VisMode Mode        { get; init; } = VisMode.Normal;
    public float?  MaxDistance { get; init; }

    internal IEnumerable<string> BuildArgs()
    {
        foreach (var a in BuildCommonArgs()) yield return a;

        if (Mode == VisMode.Full) yield return "-full";
        if (Mode == VisMode.Fast) yield return "-fast";

        if (MaxDistance.HasValue) { yield return "-maxdistance"; yield return MaxDistance.Value.ToString("G"); }
    }
}
