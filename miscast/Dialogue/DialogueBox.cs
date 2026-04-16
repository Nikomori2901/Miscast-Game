using Godot;
using System;
using Miscast;

public partial class DialogueBox : Control, IDebuggable
{
    #region Variables
    // --- Export Variables ---
    [Export] public bool debugEnabled { get; set; } = false;

    [Export] private TextureRect characterPortrait;
    [Export] private Label characterNameLabel;
    [Export] private RichTextLabel dialogueLabel;
    [Export] private float scrollSpeed = 30.0f;
    [Export] private DialoguePassage testPassage;

    // --- Variables ---
    private DialoguePassage currentPassage;
    private bool isScrolling;
    private float scrollProgress;

    #endregion

    #region Events

    #endregion

    #region Inherited Methods
    // ----- Lifecycle Methods -----
    public override void _Ready()
    {
        DebugManager.DebugPrint(this, "DialogueBox Ready");

        if (testPassage != null)
        {
            PlayPassage(testPassage);
            return;
        }

        ClearDialogue();
    }

    public override void _UnhandledInput(InputEvent inputEvent)
    {
        if (currentPassage == null)
        {
            return;
        }

        if (inputEvent.IsActionPressed("Continue"))
        {
            OnContinue();
            GetViewport().SetInputAsHandled();
        }
    }

    public override void _Process(double delta)
    {
        if (!isScrolling)
        {
            return;
        }

        scrollProgress += (float)(scrollSpeed * delta);
        int characterCount = (int)scrollProgress;

        if (characterCount >= dialogueLabel.Text.Length)
        {
            dialogueLabel.VisibleCharacters = dialogueLabel.Text.Length;
            isScrolling = false;
            return;
        }

        dialogueLabel.VisibleCharacters = characterCount;
    }

    #endregion

    #region Methods

    public void PlayPassage(DialoguePassage passage)
    {
        currentPassage = passage;
        Visible = true;
        DisplayCurrentPassage();
    }

    public void OnContinue()
    {
        if (isScrolling)
        {
            SkipScrolling();
            return;
        }

        if (currentPassage.nextPassage != null)
        {
            currentPassage = currentPassage.nextPassage;
            DisplayCurrentPassage();
            return;
        }

        ClearDialogue();
    }

    private void DisplayCurrentPassage()
    {
        dialogueLabel.Text = currentPassage.dialogueText;

        if (characterNameLabel != null)
        {
            bool hasName = !string.IsNullOrEmpty(currentPassage.characterName);
            characterNameLabel.Visible = hasName;
            characterNameLabel.Text = hasName ? currentPassage.characterName : "";
        }

        if (characterPortrait != null)
        {
            bool hasPortrait = currentPassage.characterPortrait != null;
            characterPortrait.Visible = hasPortrait;
            characterPortrait.Texture = currentPassage.characterPortrait;
        }

        scrollProgress = 0;
        dialogueLabel.VisibleCharacters = 0;
        isScrolling = true;
    }

    private void SkipScrolling()
    {
        dialogueLabel.VisibleCharacters = dialogueLabel.Text.Length;
        isScrolling = false;
    }

    private void ClearDialogue()
    {
        Visible = false;
        currentPassage = null;
        isScrolling = false;
    }

    #endregion
}
