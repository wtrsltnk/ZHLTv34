using System.Diagnostics;
using System.Text;

namespace Zhlt.Tools;

public abstract class ToolBase
{
    private readonly string _binDir;

    protected ToolBase(string binDir) => _binDir = binDir;

    protected abstract string ExeName { get; }
    protected abstract CompileStep Step { get; }

    protected async Task<ToolResult> RunAsync(
        IEnumerable<string>          args,
        string                       targetFile,
        IProgress<CompileProgress>?  progress,
        CancellationToken            ct)
    {
        var exePath = Path.Combine(_binDir, ExeName + ".exe");
        if (!File.Exists(exePath))
            throw new FileNotFoundException($"Tool not found: {exePath}");

        var allArgs = args.Append(targetFile).ToArray();
        var argString = BuildArgumentString(allArgs);

        var psi = new ProcessStartInfo(exePath, argString)
        {
            RedirectStandardOutput = true,
            RedirectStandardError  = true,
            UseShellExecute        = false,
            CreateNoWindow         = true
        };

        using var process = new Process { StartInfo = psi, EnableRaisingEvents = true };
        var output = new StringBuilder();

        process.OutputDataReceived += (_, e) =>
        {
            if (e.Data is null) return;
            output.AppendLine(e.Data);
            progress?.Report(new CompileProgress(Step, e.Data));
        };
        process.ErrorDataReceived += (_, e) =>
        {
            if (e.Data is null) return;
            output.AppendLine(e.Data);
            progress?.Report(new CompileProgress(Step, e.Data));
        };

        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

        try
        {
            await process.WaitForExitAsync(ct);
        }
        catch (OperationCanceledException)
        {
            process.Kill(entireProcessTree: true);
            throw;
        }

        return new ToolResult(Step, process.ExitCode == 0, process.ExitCode, output.ToString());
    }

    // Quotes arguments that contain spaces; simple tools expect space-separated args
    private static string BuildArgumentString(IEnumerable<string> args) =>
        string.Join(' ', args.Select(a => a.Contains(' ') ? $"\"{a}\"" : a));
}
