using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public Vector2 Position;
    public bool isWalkable;

    public int gCost;
    public int hCost;

    public Node Parent;

    public Node(Vector2 nPosition, bool nWalkable)
    {
        Position = nPosition;
        isWalkable = nWalkable;
    }

    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }
}
