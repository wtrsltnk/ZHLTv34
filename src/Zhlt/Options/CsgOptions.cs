namespace Zhlt.Options;

public record CsgOptions : CommonOptions
{
    public bool     NoWadTextures { get; init; }
    public string[] WadInclude    { get; init; } = [];
    public bool     NoClip        { get; init; }
    public ClipType ClipType      { get; init; } = ClipType.Legacy;
    public bool     OnlyEnts      { get; init; }
    public bool     NoSkyClip     { get; init; }
    public string?  HullFile      { get; init; }
    public string?  WadConfigFile { get; init; }
    public string?  WadConfig     { get; init; }
    public float?   TinyThreshold { get; init; }
    public bool     WadAutoDetect { get; init; }
    public bool     NoDetail      { get; init; }
    public bool     NoNullTex     { get; init; }

    internal IEnumerable<string> BuildArgs()
    {
        foreach (var a in BuildCommonArgs()) yield return a;

        if (NoWadTextures)           yield return "-nowadtextures";
        foreach (var w in WadInclude) { yield return "-wadinclude"; yield return w; }
        if (NoClip)                  yield return "-noclip";
        if (OnlyEnts)                yield return "-onlyents";
        if (NoSkyClip)               yield return "-noskyclip";
        if (WadAutoDetect)           yield return "-wadautodetect";
        if (NoDetail)                yield return "-nodetail";
        if (NoNullTex)               yield return "-nonulltex";

        if (ClipType != ClipType.Legacy)
        {
            yield return "-cliptype";
            yield return ClipType.ToString().ToLowerInvariant();
        }
        if (HullFile is not null)      { yield return "-hullfile";    yield return HullFile; }
        if (WadConfigFile is not null) { yield return "-wadcfgfile";  yield return WadConfigFile; }
        if (WadConfig is not null)     { yield return "-wadconfig";   yield return WadConfig; }
        if (TinyThreshold.HasValue)    { yield return "-tiny";        yield return TinyThreshold.Value.ToString("G"); }
    }
}
