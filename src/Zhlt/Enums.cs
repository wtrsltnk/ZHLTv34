namespace Zhlt;

[Flags]
public enum CompileSteps
{
    Csg = 1,
    Bsp = 2,
    Vis = 4,
    Rad = 8,
    All = Csg | Bsp | Vis | Rad
}

public enum CompileStep { Csg, Bsp, Vis, Rad }

public enum ClipType { Legacy, Smallest, Normalized, Simple, Precise }

public enum VisMode { Normal, Full, Fast }

public enum VisMatrixMode { Normal, Sparse, Off }

public enum ToolPriority { Low, Normal, High }
