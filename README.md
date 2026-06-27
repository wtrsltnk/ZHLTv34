# Zhlt

.NET wrapper voor **Vluzacn's ZHLT v34** — de Half-Life map compiler toolchain.

Bundelt `hlcsg`, `hlbsp`, `hlvis` en `hlrad` (Windows x64) in één NuGet-pakket en biedt een async .NET API om `.map` bestanden te compileren naar `.bsp`.

## Installatie

```
dotnet add package Zhlt
```

De vier native executables worden automatisch naar de output-map gekopieerd via het meegeleverde MSBuild `.targets` bestand.

## Snel voorbeeld

```csharp
using Zhlt;

var compiler = new ZhltCompiler();

var result = await compiler.CompileAsync(
    @"C:\maps\mijnkaart.map",
    progress: new Progress<CompileProgress>(p =>
        Console.WriteLine($"[{p.Step}] {p.Message}")));

if (result.Success)
    Console.WriteLine($"BSP gereed: {result.BspFile}");
else
    Console.WriteLine("Compilatie mislukt.");
```

## Uitgebreid voorbeeld

```csharp
using Zhlt;
using Zhlt.Options;

var compiler = new ZhltCompiler(binDir: @"C:\tools\zhlt");

var options = new CompileOptions
{
    Steps = CompileSteps.All,

    Csg = new CsgOptions
    {
        WadAutoDetect = true,
        ClipType      = ClipType.Precise,
        Threads       = 4,
    },

    Bsp = new BspOptions(),

    Vis = new VisOptions
    {
        Mode = VisMode.Full,
    },

    Rad = new RadOptions
    {
        Extra      = true,
        Bounce     = 8,
        Scale      = 1.0f,
        Gamma      = 0.5f,
        VisMatrix  = VisMatrixMode.Sparse,
    },
};

using var cts = new CancellationTokenSource(TimeSpan.FromMinutes(30));

var result = await compiler.CompileAsync(
    @"C:\maps\mijnkaart.map",
    options,
    progress: new Progress<CompileProgress>(p =>
        Console.WriteLine($"[{p.Step}] {p.Message}")),
    ct: cts.Token);

foreach (var step in result.Steps)
    Console.WriteLine($"{step.Step}: exitcode {step.ExitCode}, succes={step.Success}");

if (!result.Success)
{
    var mislukt = result.Steps.Last();
    Console.Error.WriteLine($"Gestopt bij stap {mislukt.Step}:\n{mislukt.Output}");
}
```

## Alleen bepaalde stappen uitvoeren

```csharp
// Alleen CSG + BSP (geen vis/rad — bruikbaar voor snelle geometrie-check)
var options = new CompileOptions
{
    Steps = CompileSteps.Csg | CompileSteps.Bsp,
};

await compiler.CompileAsync(@"C:\maps\test.map", options);
```

## API-overzicht

### `ZhltCompiler`

| Lid | Omschrijving |
|-----|--------------|
| `ZhltCompiler(binDir?)` | Maakt een compiler aan. `binDir` is de map met de executables; standaard `AppContext.BaseDirectory`. |
| `CompileAsync(mapFile, options?, progress?, ct?)` | Voert de pipeline uit en geeft een `CompileResult` terug. |
| `Csg`, `Bsp`, `Vis`, `Rad` | Toegang tot de individuele tools voor directe aanroep. |

### `CompileOptions`

| Property | Type | Omschrijving |
|----------|------|--------------|
| `Steps` | `CompileSteps` | Welke stappen worden uitgevoerd (vlag-enum, standaard `All`). |
| `Csg` | `CsgOptions` | Opties voor `hlcsg`. |
| `Bsp` | `BspOptions` | Opties voor `hlbsp`. |
| `Vis` | `VisOptions` | Opties voor `hlvis`. |
| `Rad` | `RadOptions` | Opties voor `hlrad`. |

### `CompileResult`

| Property | Omschrijving |
|----------|--------------|
| `Success` | `true` als alle uitgevoerde stappen geslaagd zijn. |
| `BspFile` | Pad naar het gegenereerde `.bsp` bestand. |
| `Steps` | Lijst van `ToolResult` per uitgevoerde stap. |

### `ToolResult`

| Property | Omschrijving |
|----------|--------------|
| `Step` | Welke stap (`Csg`, `Bsp`, `Vis` of `Rad`). |
| `Success` | Of de tool met exitcode 0 is gestopt. |
| `ExitCode` | Ruwe exitcode van het proces. |
| `Output` | Gecombineerde stdout + stderr uitvoer. |

## Annuleren

Geef een `CancellationToken` mee. Als het token geannuleerd wordt, stopt het actieve child-process en gooit `CompileAsync` een `OperationCanceledException`.

## Vereisten

- .NET 10.0 of hoger
- Windows x64

## Licentie

De ZHLT-tools zijn het werk van Vluzacn. Raadpleeg de originele ZHLT-licentie voor gebruiksvoorwaarden.
