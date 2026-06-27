using Zhlt.Options;

namespace Zhlt.Tools;

public sealed class HlbspTool : ToolBase
{
    internal HlbspTool(string binDir) : base(binDir) { }

    protected override string      ExeName => "hlbsp";
    protected override CompileStep Step    => CompileStep.Bsp;

    public Task<ToolResult> RunAsync(
        string                      bspFile,
        BspOptions?                 options  = null,
        IProgress<CompileProgress>? progress = null,
        CancellationToken           ct       = default)
    {
        var args = (options ?? new BspOptions()).BuildArgs();
        return RunAsync(args, bspFile, progress, ct);
    }
}
