using Godot;
using System;

[GlobalClass]
public partial class DialoguePassage : Resource
{
    [Export] public string characterName;
    [Export] public Texture2D characterPortrait;
    [Export(PropertyHint.MultilineText)] public string dialogueText;
    [Export] public DialoguePassage nextPassage;
}
