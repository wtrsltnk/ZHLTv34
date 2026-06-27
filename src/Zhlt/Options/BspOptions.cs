namespace Zhlt.Options;

public record BspOptions : CommonOptions
{
    public bool LeakOnly          { get; init; }
    public int? Subdivide         { get; init; }
    public int? MaxNodeSize       { get; init; }
    public bool NoTJunc           { get; init; }
    public bool NoClip            { get; init; }
    public bool NoFill            { get; init; }
    public bool NoInsideFill      { get; init; }
    public bool NoOpt             { get; init; }
    public bool NoClipNodeMerge   { get; init; }
    public bool NoHull2           { get; init; }
    public bool NoDetail          { get; init; }
    public bool NoNullTex         { get; init; }

    internal IEnumerable<string> BuildArgs()
    {
        foreach (var a in BuildCommonArgs()) yield return a;

        if (LeakOnly)         yield return "-leakonly";
        if (NoTJunc)          yield return "-notjunc";
        if (NoClip)           yield return "-noclip";
        if (NoFill)           yield return "-nofill";
        if (NoInsideFill)     yield return "-noinsidefill";
        if (NoOpt)            yield return "-noopt";
        if (NoClipNodeMerge)  yield return "-noclipnodemerge";
        if (NoHull2)          yield return "-nohull2";
        if (NoDetail)         yield return "-nodetail";
        if (NoNullTex)        yield return "-nonulltex";

        if (Subdivide.HasValue)   { yield return "-subdivide";   yield return Subdivide.Value.ToString(); }
        if (MaxNodeSize.HasValue) { yield return "-maxnodesize"; yield return MaxNodeSize.Value.ToString(); }
    }
}
