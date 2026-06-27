using Zhlt.Options;

namespace Zhlt;

public record CompileOptions
{
    public CsgOptions   Csg   { get; init; } = new();
    public BspOptions   Bsp   { get; init; } = new();
    public VisOptions   Vis   { get; init; } = new();
    public RadOptions   Rad   { get; init; } = new();
    public CompileSteps Steps { get; init; } = CompileSteps.All;
}
