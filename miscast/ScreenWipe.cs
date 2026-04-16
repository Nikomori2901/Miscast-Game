using Godot;
using System;
using System.Threading.Tasks;

public partial class ScreenWipe : Sprite2D
{
    #region Variables
    // --- Export Variables ---
    [Export] private float wipeDuration = 0.5f;
    [Export] private Curve wipeCurve;
    [Export] private Texture2D wipeInTexture;
    [Export] private Texture2D wipeOutTexture;
    [Export] private Vector2 leftRestPosition = new Vector2(-640, 0);
    [Export] private Vector2 rightRestPosition = new Vector2(640, 0);
    [Export] private Vector2 centerPosition = new Vector2(0, 0);
    [Export] private bool snapToPixel = false;

    // --- Variables ---
    private bool isAnimating = false;

    // --- Signals ---
    [Signal] public delegate void WipeInFinishedEventHandler();
    [Signal] public delegate void WipeOutFinishedEventHandler();

    #endregion

    #region Inherited Methods

    public override void _Ready()
    {
        Visible = false;
    }

    public override void _UnhandledInput(InputEvent inputEvent)
    {
        if (inputEvent is InputEventKey keyEvent && keyEvent.Pressed)
        {
            if (keyEvent.Keycode == Key.I)
            {
                WipeIn(WipeDirection.Right);
            }

            else if (keyEvent.Keycode == Key.O)
            {
                WipeOut(WipeDirection.Left);
            }
        }
    }

    #endregion

    #region Methods

    public async void WipeIn(WipeDirection direction)
    {
        if (isAnimating) return;

        Vector2 startPosition = direction == WipeDirection.Left ? leftRestPosition : rightRestPosition;
        Texture = wipeInTexture;
        Visible = true;
        isAnimating = true;
        await AnimateWipe(startPosition, centerPosition);
        isAnimating = false;
        EmitSignal(SignalName.WipeInFinished);
    }

    public async void WipeOut(WipeDirection direction)
    {
        if (isAnimating) return;

        Vector2 endPosition = direction == WipeDirection.Right ? rightRestPosition : leftRestPosition;
        Texture = wipeOutTexture;
        Visible = true;
        isAnimating = true;
        await AnimateWipe(centerPosition, endPosition);
        Visible = false;
        isAnimating = false;
        EmitSignal(SignalName.WipeOutFinished);
    }

    private async Task AnimateWipe(Vector2 from, Vector2 to)
    {
        Position = from;
        float progress = 0f;

        while (progress < 1f)
        {
            await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);

            progress += (float)GetProcessDeltaTime() / wipeDuration;
            if (progress > 1f) progress = 1f;

            float curveValue = wipeCurve?.Sample(progress) ?? progress;
            Position = from.Lerp(to, curveValue);
            if (snapToPixel) Position = Position.Round();
        }
    }

    #endregion
}

public enum WipeDirection { Left, Right }