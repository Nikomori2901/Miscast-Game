using Godot;

[GlobalClass]
public partial class GridConnection : Resource
{
    [Export] public GridPosition from;
    [Export] public GridPosition to;
}
