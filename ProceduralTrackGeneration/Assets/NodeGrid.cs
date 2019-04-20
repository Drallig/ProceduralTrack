using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeGrid : MonoBehaviour
{
    public Vector2 gridSize;
    Node[,] grid;
    public int cornerNum = 15;
    public List<Node> cornerPos;
    Node endOfStraight, startOfStraight;
    public Node[] cornerOrder;

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridSize.x, 1, gridSize.y));
    }

    // Use this for initialization
    void Start()
    {
        //cornerPos = new Vector2[cornerNum];
        setCorners();
        setStartFinishStraight();

        grid = new Node[(int)gridSize.x, (int)gridSize.y];
        for (int y = 0; y < gridSize.y; y++)
        {
            for (int x = 0; x < gridSize.x; x++)
            {
                Vector2 nPos = new Vector2(x, y);
                grid[x, y] = new Node(nPos, true);
            }
        }
        cornerOrder = new Node[cornerNum + 2];
        cornerNodeOrder();
    }

    // Update is called once per frame
    void Update()
    {

    }

    List<Node> getNeighbours(Node node)
    {
        int xVal, yVal;

        List<Node> neighbours = new List<Node>();
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                {
                    continue;
                }

                xVal = (int)node.Position.x + x;
                yVal = (int)node.Position.y + y;

                if (xVal >= 0 && xVal < gridSize.x && yVal >= 0 && yVal < gridSize.y)
                {
                    neighbours.Add(grid[xVal, yVal]);
                }
            }
        }

        return neighbours;
    }

    void setCorners()
    {
        for (int i = 0; i < cornerNum; i++)
        {
            int _x = Random.Range(0, 200);
            int _y = Random.Range(0, 200);

            if (_y < 100)
            {
                int diff = 100 - _y;

                _y += Random.Range(diff-20, diff + 30);
            }

            if (_y == 100)
            {
                _y += Random.Range(30, 60);
            }

            cornerPos.Add(grid[_x, _y]);
        }


    }

    void setStartFinishStraight() //sets the start finish straight in the center of the map, traveling right to left
    {
        int finishX = Random.Range(50, 75);
        int startX = Random.Range(125, 150);
        startOfStraight = grid[startX, 100];
        endOfStraight = grid[finishX, 100];
    }

    void findPath(Vector2 startPos, Vector2 endPos)
    {
        Node initialNode = grid[(int)startPos.x, (int)startPos.y];
        Node targetNode = grid[(int)endPos.x, (int)endPos.y];

        List<Node> openList = new List<Node>();
        List<Node> closedList = new List<Node>();
        openList.Add(initialNode);

        while (openList.Count > 0)
        {
            Node currentNode = openList[0];
            for (int i = 0; i < openList.Count; i++)
            {
                if (openList[i].fCost < currentNode.fCost || openList[i].fCost == currentNode.fCost && openList[i].hCost < currentNode.hCost)
                {
                    currentNode = openList[i];
                    
                }
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            if (currentNode == targetNode)
            {

                return;
            }

            List<Node> nList = getNeighbours(currentNode);
            foreach (Node neighbour in nList)
            {
                if (!neighbour.isWalkable || closedList.Contains(neighbour))
                {
                    continue;
                }

                int newMovCost = currentNode.gCost + getDist(currentNode, neighbour);
                if (newMovCost < neighbour.gCost || !openList.Contains(neighbour))
                {
                    neighbour.gCost = newMovCost;
                    neighbour.hCost = getDist(neighbour, targetNode);

                    neighbour.Parent = currentNode;

                    if (!openList.Contains(neighbour))
                    {
                        openList.Add(neighbour);
                    }
                }
            }
        }
    }

    int getDist(Node n1, Node n2)
    {
        int distX = Mathf.Abs((int)n1.Position.x - (int)n2.Position.x);
        int distY = Mathf.Abs((int)n1.Position.y - (int)n2.Position.y);

        if (distX > distY)
            return 14 * distY + 10 * (distX - distY);
        else
            return 14 * distX + 10 * (distY - distX);
    }

    void retracePath(Node start, Node end)
    {
        List<Node> path = new List<Node>();

        Node current = end;
        while (current != start)
        {
            path.Add(current);
            current = current.Parent;
        }
        path.Reverse();


        //Alright you furr burger heres what you are going to do:
        //create a list path from point to point on the track.
        //create a list. any time you traverse to a new node, add the old node to this list. (prevent overlap)
        //alter algorithm that if node is in "trackposition" list(see previous create list line), it can not use it and must move on to another neighbor
    }

    void cornerNodeOrder() //creates an ordered list of nodes that are then to be pathfound between.
    {
        cornerOrder[0] = startOfStraight;
        cornerOrder[1] = endOfStraight;



        List<Node> FirstCorner = new List<Node>();

        foreach (Node n in cornerPos)
        {
            if (n.Position.y < 100)
            {
                FirstCorner.Add(n);
                cornerPos.Remove(n);
            }
        }

        if (FirstCorner != null)
        {
            int firstCornerX = (int)endOfStraight.Position.x;
            if (FirstCorner.Capacity < 2)
            {
                cornerOrder[3] = FirstCorner[0];
            }
            else
            {
                int currentChoice = 0;
                int lowestValue = 0;
                int distanceValue = 100;
                foreach (Node n in FirstCorner)
                {
                    int distX = Mathf.Abs(firstCornerX - (int)n.Position.x);
                    if (distX < distanceValue)
                    {
                        distanceValue = distX;
                        lowestValue = currentChoice;
                    }
                    else
                    {
                        currentChoice++;
                    }
                }


            }

            //cornerOrder[2] = FirstCorner.
        }
        else
        {

        }

        int listPos = 0;
        int listPosOfShortest;
        int curDistance = 1000;

        for (int i = 2; i < cornerNum + 2; i++)
        {
            int startVal = i - 1;

            foreach (Node n in cornerPos)
            {
                int newDist = getDist(cornerOrder[startVal], n);
                if (newDist < curDistance)
                {
                    curDistance = newDist;
                    listPosOfShortest = listPos;
                }
                listPos++;
            }

        }
    }

}
