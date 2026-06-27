using Zhlt.Options;

namespace Zhlt.Tools;

public sealed class HlcsgTool : ToolBase
{
    internal HlcsgTool(string binDir) : base(binDir) { }

    protected override string      ExeName => "hlcsg";
    protected override CompileStep Step    => CompileStep.Csg;

    public Task<ToolResult> RunAsync(
        string                      mapFile,
        CsgOptions?                 options  = null,
        IProgress<CompileProgress>? progress = null,
        CancellationToken           ct       = default)
    {
        var args = (options ?? new CsgOptions()).BuildArgs();
        return RunAsync(args, mapFile, progress, ct);
    }
}
