using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Miscast;
using GodotDict = Godot.Collections.Dictionary;

public enum GridPosition
{
    TopLeft,
    Top,
    TopRight,
    Left,
    Center,
    Right,
    BottomLeft,
    Bottom,
    BottomRight
}

public partial class MagicCasting : Singleton<MagicCasting>, IDebuggable
{
    #region Variables
    // --- Export Variables ---
    [Export] public bool debugEnabled { get; set; } = false;

    // --- Variables ---
    public MagicNode[,] nodeGrid = new MagicNode[3, 3];

    public readonly Vector2I[] gridDirections = [new(0, -1), new(0, 1), new(-1, 0), new(1, 0), new(-1, -1), new(1, -1), new(-1, 1), new(1, 1), new(0, -2), new(0, 2), new(-2, 0), new(2, 0)];

    public List<MagicNode> activeNodes = new List<MagicNode>();
    public List<NodeConnection> activeConnections = new List<NodeConnection>();
    public Dictionary<(Vector2I, Vector2I), NodeConnection> connectionMap = new();

    public bool casting = false;
    public MagicNode hoveredNode;

    [Export] public SpellPattern targetPattern;
    private List<NodeConnection> targetConnections = new List<NodeConnection>();

    #endregion

    #region Inherited Methods
    // ----- Lifecycle Methods -----
    public override void _Ready()
    {
        DebugManager.DebugPrint(this, "MagicCasting Ready");
        base._Ready();

        // --- Initialisation ---
        List<MagicNode> magicNodes = new List<MagicNode>();
        foreach (Node node in GetChildren())
        {
            if (node is MagicNode magicNode)
            {
                magicNodes.Add(magicNode);
                magicNode.NodeActivated += NodeActivated;
            }

            if (node is NodeConnection nodeConnection)
            {
                connectionMap[(nodeConnection.nodePosition1, nodeConnection.nodePosition2)] = nodeConnection;
                connectionMap[(nodeConnection.nodePosition2, nodeConnection.nodePosition1)] = nodeConnection;
            }
        }

        for (int nodeIndex = 0; nodeIndex < magicNodes.Count; nodeIndex++)
        {
            Vector2I pos = magicNodes[nodeIndex].gridPosition;
            nodeGrid[pos.X, pos.Y] = magicNodes[nodeIndex];
        }

        DebugManager.DebugPrint(this, $"Node Grid Length: {nodeGrid.Length}");
        DebugManager.DebugPrint(this, $"Connection Map Size: {connectionMap.Count}");
        
        ResolveTargetPattern();
    }

    public override void _ExitTree()
    {
        DebugManager.DebugPrint(this, "MagicCasting ExitTree");

        // --- Unsubscribe Signals ---
        foreach (Node node in GetChildren())
        {
            if (node is MagicNode magicNode)
            {
                magicNode.NodeActivated -= NodeActivated;
            }
        }
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
            Vector2I difference = magicNode2.gridPosition - magicNode1.gridPosition;
            if ((difference.X == 0 && Math.Abs(difference.Y) == 1) || (difference.Y == 0 && Math.Abs(difference.X) == 1))
            {
                return false;
            }
        }

        return GetNeighbours(magicNode1).Contains(magicNode2);
    }

    public void NodeActivated(MagicNode magicNode)
    {
        if (activeNodes.Contains(magicNode))
        {
            if (activeNodes.Last() != magicNode && IsNeighbour(magicNode, activeNodes.Last()))
            {
                ActivateConnection(activeNodes.Last().gridPosition, magicNode.gridPosition);
                activeNodes.Remove(magicNode);
                activeNodes.Add(magicNode);
            }
            return;
        }

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
        if (!connectionMap.TryGetValue((nodePosition1, nodePosition2), out NodeConnection connection))
        {
            return;
        }

        if (activeConnections.Contains(connection))
        {
            return;
        }

        activeConnections.Add(connection);
        connection.ShowConnection(nodePosition1);
    }

    public void StartCasting()
    {
        DebugManager.DebugPrint(this, "StartCasting");
        casting = true;

        if (hoveredNode != null)
        {
            NodeActivated(hoveredNode);
        }
    }

    public void InvalidConnection()
    {
        DebugManager.DebugPrint(this, "InvalidConnection");
        StopCasting();
    }

    public void StopCasting()
    {
        if (!casting)
        {
            return;
        }

        DebugManager.DebugPrint(this, "StopCasting");

        if (CastValid())
        {
            CastSuccess();
        }

        else
        {
            CastFail();
        }

        foreach (MagicNode magicNode in activeNodes)
        {
            magicNode.Deactivate();
        }

        foreach (NodeConnection connection in activeConnections)
        {
            connection.HideConnection();
        }

        activeNodes.Clear();
        activeConnections.Clear();

        casting = false;
    }

    public void ResolveTargetPattern()
    {
        targetConnections.Clear();

        if (targetPattern == null)
        {
            return;
        }

        foreach (GridConnection pair in targetPattern.connections)
        {
            Vector2I fromPosition = GridPosToVec(pair.from);
            Vector2I toPosition = GridPosToVec(pair.to);

            if (connectionMap.TryGetValue((fromPosition, toPosition), out NodeConnection connection) && !targetConnections.Contains(connection))
            {
                targetConnections.Add(connection);
            }
        }

        DebugManager.DebugPrint(this, $"Target Pattern: {targetPattern.patternName} - {targetConnections.Count} Connections");
    }

    public bool CastValid()
    {
        if (targetConnections.Count == 0)
        {
            return false;
        }

        if (activeConnections.Count != targetConnections.Count)
        {
            return false;
        }

        foreach (NodeConnection targetConnection in targetConnections)
        {
            if (!activeConnections.Contains(targetConnection))
            {
                return false;
            }
        }

        return true;
    }

    public void CastSuccess()
    {
        DebugManager.DebugPrint(this, "CastSuccess");
    }

    public void CastFail()
    {
        DebugManager.DebugPrint(this, "CastFail");
    }

    public Vector2I GridPosToVec(GridPosition position)
    {
        switch (position)
        {
            case GridPosition.TopLeft:
                return new Vector2I(0, 0);
            case GridPosition.Top:
                return new Vector2I(1, 0);
            case GridPosition.TopRight:
                return new Vector2I(2, 0);
            case GridPosition.Left:
                return new Vector2I(0, 1);
            case GridPosition.Center:
                return new Vector2I(1, 1);
            case GridPosition.Right:
                return new Vector2I(2, 1);
            case GridPosition.BottomLeft:
                return new Vector2I(0, 2);
            case GridPosition.Bottom:
                return new Vector2I(1, 2);
            case GridPosition.BottomRight:
                return new Vector2I(2, 2);
            default:
                return new Vector2I(1, 1);
        }
    }
    #endregion
}
