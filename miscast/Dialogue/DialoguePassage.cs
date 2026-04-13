using Godot;
using System;

public enum PortraitSide
{
    Left,
    Right
}

[GlobalClass]
public partial class DialoguePassage : Resource
{
    [Export] public string characterName;
    [Export] public Texture2D characterPortrait;
    [Export] public PortraitSide portraitSide = PortraitSide.Left;
    [Export(PropertyHint.MultilineText)] public string dialogueText;
    [Export] public DialoguePassage nextPassage;
}
