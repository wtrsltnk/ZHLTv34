using Zhlt.Options;

namespace Zhlt.Tools;

public sealed class HlradTool : ToolBase
{
    internal HlradTool(string binDir) : base(binDir) { }

    protected override string      ExeName => "hlrad";
    protected override CompileStep Step    => CompileStep.Rad;

    public Task<ToolResult> RunAsync(
        string                      bspFile,
        RadOptions?                 options  = null,
        IProgress<CompileProgress>? progress = null,
        CancellationToken           ct       = default)
    {
        var args = (options ?? new RadOptions()).BuildArgs();
        return RunAsync(args, bspFile, progress, ct);
    }
}
