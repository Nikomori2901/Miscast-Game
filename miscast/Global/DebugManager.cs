using Godot;
using System;
using Miscast;

[Tool]
public partial class DebugManager : Node
{
    [ExportToolButton("Enable All Debug")]
    private Callable enableAllButton => Callable.From(EnableAll);

    [ExportToolButton("Disable All Debug")]
    private Callable disableAllButton => Callable.From(DisableAll);

    public static void DebugPrint(IDebuggable source, string message)
    {
        if (source.debugEnabled)
        {
            GD.Print($"[{source.GetType().Name}] {message}");
        }
    }

    private void ToggleAll(Node node, bool enabled)
    {
        if (node is IDebuggable debuggable)
        {
            debuggable.debugEnabled = enabled;
        }

        foreach (Node child in node.GetChildren())
        {
            ToggleAll(child, enabled);
        }
    }

    private void EnableAll()
    {
        ToggleAll(GetTree().EditedSceneRoot, true);
        GD.Print("All Debug Enabled");
    }

    private void DisableAll()
    {
        ToggleAll(GetTree().EditedSceneRoot, false);
        GD.Print("All Debug Disabled");
    }
}
