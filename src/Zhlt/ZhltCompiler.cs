using Zhlt.Tools;

namespace Zhlt;

public sealed class ZhltCompiler
{
    public HlcsgTool Csg { get; }
    public HlbspTool Bsp { get; }
    public HlvisTool Vis { get; }
    public HlradTool Rad { get; }

    /// <param name="binDir">Map met de tool-executables. Standaard: AppContext.BaseDirectory.</param>
    public ZhltCompiler(string? binDir = null)
    {
        var dir = binDir ?? AppContext.BaseDirectory;
        Csg = new HlcsgTool(dir);
        Bsp = new HlbspTool(dir);
        Vis = new HlvisTool(dir);
        Rad = new HlradTool(dir);
    }

    /// <summary>
    /// Voert de volledige compilatiepipeline uit: CSG → BSP → VIS → RAD.
    /// </summary>
    /// <param name="mapFile">Pad naar het .map bestand.</param>
    /// <param name="options">Pipeline-opties; null gebruikt alle standaardwaarden.</param>
    /// <param name="progress">Ontvangt uitvoerregels tijdens compilatie.</param>
    /// <param name="ct">Annuleringstoken; stopt het actieve process en gooit OperationCanceledException.</param>
    public async Task<CompileResult> CompileAsync(
        string                      mapFile,
        CompileOptions?             options  = null,
        IProgress<CompileProgress>? progress = null,
        CancellationToken           ct       = default)
    {
        options ??= new CompileOptions();

        var baseName = Path.GetFileNameWithoutExtension(mapFile);
        var bspFile  = Path.ChangeExtension(mapFile, ".bsp");
        var steps    = new List<ToolResult>();

        if (options.Steps.HasFlag(CompileSteps.Csg))
        {
            var r = await Csg.RunAsync(mapFile, options.Csg, progress, ct);
            steps.Add(r);
            if (!r.Success) return new CompileResult(false, bspFile, steps);
        }

        if (options.Steps.HasFlag(CompileSteps.Bsp))
        {
            var r = await Bsp.RunAsync(baseName, options.Bsp, progress, ct);
            steps.Add(r);
            if (!r.Success) return new CompileResult(false, bspFile, steps);
        }

        if (options.Steps.HasFlag(CompileSteps.Vis))
        {
            var r = await Vis.RunAsync(baseName, options.Vis, progress, ct);
            steps.Add(r);
            if (!r.Success) return new CompileResult(false, bspFile, steps);
        }

        if (options.Steps.HasFlag(CompileSteps.Rad))
        {
            var r = await Rad.RunAsync(baseName, options.Rad, progress, ct);
            steps.Add(r);
            if (!r.Success) return new CompileResult(false, bspFile, steps);
        }

        return new CompileResult(true, bspFile, steps);
    }
}
