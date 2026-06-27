namespace Zhlt.Options;

public record RadOptions : CommonOptions
{
    public bool          Fast        { get; init; }
    public bool          Extra       { get; init; }
    public VisMatrixMode VisMatrix   { get; init; } = VisMatrixMode.Normal;
    public int?          Bounce      { get; init; }
    public (float R, float G, float B)? Ambient { get; init; }
    public float?        Scale       { get; init; }
    public float?        Gamma       { get; init; }
    public int?          Smooth      { get; init; }
    public float?        Chop        { get; init; }
    public float?        TexChop     { get; init; }
    public float?        Fade        { get; init; }
    public float?        Coring      { get; init; }
    public float?        Sky         { get; init; }
    public int?          MinLight    { get; init; }
    public string?       LightsFile  { get; init; }
    public string?       WadDir      { get; init; }
    public bool          NoSkyFix    { get; init; }
    public bool          NoPaque     { get; init; }
    public bool          NoSpread    { get; init; }
    public bool          NoTexScale  { get; init; }
    public bool          NoLerp      { get; init; }
    public bool          Incremental { get; init; }
    public (float R, float G, float B)? ColourGamma  { get; init; }
    public (float R, float G, float B)? ColourScale  { get; init; }

    internal IEnumerable<string> BuildArgs()
    {
        foreach (var a in BuildCommonArgs()) yield return a;

        if (Fast)        yield return "-fast";
        if (Extra)       yield return "-extra";
        if (NoSkyFix)    yield return "-noskyfix";
        if (NoPaque)     yield return "-nopaque";
        if (NoSpread)    yield return "-nospread";
        if (NoTexScale)  yield return "-notexscale";
        if (NoLerp)      yield return "-nolerp";
        if (Incremental) yield return "-incremental";

        if (VisMatrix == VisMatrixMode.Sparse) { yield return "-vismatrix"; yield return "sparse"; }
        if (VisMatrix == VisMatrixMode.Off)    { yield return "-vismatrix"; yield return "off"; }

        if (Bounce.HasValue)      { yield return "-bounce";      yield return Bounce.Value.ToString(); }
        if (Scale.HasValue)       { yield return "-scale";       yield return Scale.Value.ToString("G"); }
        if (Gamma.HasValue)       { yield return "-gamma";       yield return Gamma.Value.ToString("G"); }
        if (Smooth.HasValue)      { yield return "-smooth";      yield return Smooth.Value.ToString(); }
        if (Chop.HasValue)        { yield return "-chop";        yield return Chop.Value.ToString("G"); }
        if (TexChop.HasValue)     { yield return "-texchop";     yield return TexChop.Value.ToString("G"); }
        if (Fade.HasValue)        { yield return "-fade";        yield return Fade.Value.ToString("G"); }
        if (Coring.HasValue)      { yield return "-coring";      yield return Coring.Value.ToString("G"); }
        if (Sky.HasValue)         { yield return "-sky";         yield return Sky.Value.ToString("G"); }
        if (MinLight.HasValue)    { yield return "-minlight";    yield return MinLight.Value.ToString(); }
        if (LightsFile is not null) { yield return "-lights";   yield return LightsFile; }
        if (WadDir is not null)     { yield return "-waddir";   yield return WadDir; }

        if (Ambient.HasValue)
        {
            yield return "-ambient";
            yield return Ambient.Value.R.ToString("G");
            yield return Ambient.Value.G.ToString("G");
            yield return Ambient.Value.B.ToString("G");
        }
        if (ColourGamma.HasValue)
        {
            yield return "-colourgamma";
            yield return ColourGamma.Value.R.ToString("G");
            yield return ColourGamma.Value.G.ToString("G");
            yield return ColourGamma.Value.B.ToString("G");
        }
        if (ColourScale.HasValue)
        {
            yield return "-colourscale";
            yield return ColourScale.Value.R.ToString("G");
            yield return ColourScale.Value.G.ToString("G");
            yield return ColourScale.Value.B.ToString("G");
        }
    }
}
