using Zhlt.Options;

namespace Zhlt.Tools;

public sealed class HlvisTool : ToolBase
{
    internal HlvisTool(string binDir) : base(binDir) { }

    protected override string      ExeName => "hlvis";
    protected override CompileStep Step    => CompileStep.Vis;

    public Task<ToolResult> RunAsync(
        string                      bspFile,
        VisOptions?                 options  = null,
        IProgress<CompileProgress>? progress = null,
        CancellationToken           ct       = default)
    {
        var args = (options ?? new VisOptions()).BuildArgs();
        return RunAsync(args, bspFile, progress, ct);
    }
}
