using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Miscast;
using GodotDict = Godot.Collections.Dictionary;

public partial class MagicCasting : Singleton<MagicCasting>, IDebuggable
{
    #region Variables
    // --- Export Variables ---
    [Export] public bool debugEnabled { get; set; } = false;

    // --- Variables ---
    public MagicNode[,] nodeGrid = new MagicNode[3, 3];

    public readonly Vector2I[] gridDirections = [new(0, -1), new(0, 1), new(-1, 0), new(1, 0), new(-1, -1), new(1, -1), new(-1, 1), new(1, 1), new(0, -2), new(0, 2), new(-2, 0), new(2, 0)];

    public List<MagicNode> activeNodes = new List<MagicNode>();
    public List<Vector2I[]> activeConnections = new List<Vector2I[]>();
    public Dictionary<(Vector2I, Vector2I), AnimatedSprite2D> connectionSprites = new();

    public bool casting = false;

    public List<Vector2I[]> targetPattern = new List<Vector2I[]>();

    #endregion

    #region Events

    #endregion

    #region Inherited Methods
    // ----- Lifecycle Methods -----
    public override void _Ready()
    {
        DebugManager.DebugPrint(this, "MagicCasting Ready");
        base._Ready();

        // --- Global Subscribe Events ---
        MagicNode.NodeActivatedEvent += NodeActivated;

        // --- Initialisation ---
        List<MagicNode> magicNodes = new List<MagicNode>();
        foreach (Node node in GetChildren())
        {
            if (node is MagicNode magicNode)
            {
                magicNodes.Add(magicNode);
            }
        }

        for (int i = 0; i < magicNodes.Count; i++)
        {
            Vector2I pos = magicNodes[i].gridPosition;
            nodeGrid[pos.X, pos.Y] = magicNodes[i];
        }

        DebugManager.DebugPrint(this, $"Node Grid Length: {nodeGrid.Length}");
    }

    public override void _ExitTree()
    {
        DebugManager.DebugPrint(this, "MagicCasting ExitTree");

        // --- Unsubscribe Events ---
        MagicNode.NodeActivatedEvent -= NodeActivated;
    }

    // ----- Update Methods -----
    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("Left Mouse"))
        {
            StartCasting();
        }

        if (Input.IsActionJustReleased("Left Mouse"))
        {
            StopCasting();
        }
    }

    #endregion

    #region Methods

    public List<MagicNode> GetNeighbours(MagicNode magicNode)
    {
        List<MagicNode> neighbours = new List<MagicNode>();
        Vector2I originPosition = magicNode.gridPosition;

        foreach (Vector2I direction in gridDirections)
        {
            Vector2I neighbourPosition = originPosition + direction;

            if (neighbourPosition.X < 0 || neighbourPosition.X > 2)
            {
                continue;
            }

            if (neighbourPosition.Y < 0 || neighbourPosition.Y > 2)
            {
                continue;
            }

            neighbours.Add(nodeGrid[neighbourPosition.X, neighbourPosition.Y]);
        }

        return neighbours;
    }

    public bool IsNeighbour(MagicNode magicNode1, MagicNode magicNode2)
    {
        DebugManager.DebugPrint(this, $"{magicNode1.gridPosition} - {magicNode2.gridPosition}");

        if (magicNode1.gridPosition != new Vector2I(1, 1) && magicNode2.gridPosition != new Vector2I(1, 1))
        {
            return false;
        }

        return GetNeighbours(magicNode1).Contains(magicNode2);
    }

    public void NodeActivated(MagicNode magicNode)
    {
        if (activeNodes.Contains(magicNode)) return;

        if (activeNodes.Count >= 1)
        {
            if (!IsNeighbour(magicNode, activeNodes.Last()))
            {
                InvalidConnection();
                return;
            }

            ActivateConnection(activeNodes.Last().gridPosition, magicNode.gridPosition);
        }

        activeNodes.Add(magicNode);
        magicNode.ShowSprite();

        // if (CastValid())
        // {
        //     StopCasting();
        //     CastSuccess();
        // }
    }

    public void ActivateConnection(Vector2I nodePosition1, Vector2I nodePosition2)
    {
        activeConnections.Add(new Vector2I[] {nodePosition1, nodePosition2});

        // Activate Visual Somehow
    }

    public void StartCasting()
    {
        DebugManager.DebugPrint(this, "StartCasting");
        casting = true;
    }

    public void InvalidConnection()
    {
        DebugManager.DebugPrint(this, "InvalidConnection");
        StopCasting();
    }

    public void StopCasting()
    {
        DebugManager.DebugPrint(this, "StopCasting");

        foreach (MagicNode magicNode in activeNodes)
        {
            magicNode.Deactivate();
        }

        activeNodes.Clear();
        activeConnections.Clear();

        casting = false;
    }

    public bool CastValid()
    {
        foreach (Vector2I[] connection in targetPattern)
        {
            Vector2I connectionPos1 = connection[0];
            Vector2I connectionPos2 = connection[1];

            Vector2I[] activeConnection = [connectionPos1, connectionPos2];
            Vector2I[] activeConnectionReversed = [connectionPos2, connectionPos1];

            if (activeConnections.Contains(activeConnection) || activeConnections.Contains(activeConnectionReversed))
            {
                continue;
            }

            return false;
        }

        return true;
    }

    public void CastSuccess()
    {

    }

    #endregion
}
