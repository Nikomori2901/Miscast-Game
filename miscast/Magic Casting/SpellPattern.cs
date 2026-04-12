using Godot;

[GlobalClass]
public partial class SpellPattern : Resource
{
    [Export] public string patternName;
    [Export] public GridConnection[] connections;
}
