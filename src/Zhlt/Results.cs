namespace Zhlt;

public record ToolResult(
    CompileStep Step,
    bool        Success,
    int         ExitCode,
    string      Output);

public record CompileResult(
    bool                      Success,
    string                    BspFile,
    IReadOnlyList<ToolResult> Steps);

public record CompileProgress(
    CompileStep Step,
    string      Message);
