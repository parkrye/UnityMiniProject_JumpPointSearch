using System;
using System.Collections.Generic;
using UnityEngine;

public class JumpPointSearch : MonoBehaviour
{
    struct Node
    {
        public int x, y;
        public int f, g, h;

        public Node(int _x, int _y, int _g, int _h)
        {
            x = _x;
            y = _y;
            g = _g;
            h = _h;
            f = g + h;
        }
    }

    [SerializeField] GameObject BGPrefab, MovePrefab;
    (int x, int y)[] directions;

    void Start()
    {
        directions = new (int, int)[8] { (-1, 0), (1, 0), (0, -1), (0, 1), (-1, -1), (-1, 1), (1, -1), (1, 1) };

        bool[,] map = new bool[,]
        {
            { true , true , true , true , false, true , true , false, true , true  },
            { true , true , true , true , false, true , true , false, true , true  },
            { true , true , true , true , false, true , true , true , true , true  },
            { false, false, false, true , false, true , true , false, true , true  },
            { true , true , true , true , false, true , true , false, true , true  },
            { true , true , true , true , false, true , true , false, false, true  },
            { false, true , false, false, false, true , true , false, true , true  },
            { true , true , true , true , false, true , true , false, true , true  },
            { true , true , true , true , true , true , true , false, true , true  },
            { true , true , true , true , false, true , true , false, true , true  }
        };

        DrawMap(map);
        AStar(map);
        JPS(map);
    }

    void AStar(bool[,] map)
    {

        DateTime nowTime = DateTime.Now;
        PriorityQueue<Node, int> priorityQueue = new();
        Dictionary<(int x, int y), bool> visited = new();
        Dictionary<Node, Node> parents = new();
        Stack<Node> answer = new();

        Node startNode = new(0, 0, 0, 9);
        Node endNode = new(9, 9, 0, 0);
        parents.Add(startNode, startNode);
        priorityQueue.Enqueue(startNode, 0);
        answer.Push(startNode);

        while(priorityQueue.Count > 0)
        {
            Node nowNode = priorityQueue.Dequeue();
            if(visited.ContainsKey((nowNode.x, nowNode.y)))
                continue;
            visited.Add((nowNode.x, nowNode.y), true);

            if (IsSameNode(nowNode, endNode))
            {
                while (!IsSameNode(parents[nowNode], nowNode))
                {
                    answer.Push(nowNode);
                    nowNode = parents[nowNode];
                }
                break;
            }

            for(int i = 0; i < 8; i++)
            {
                Node findNode = new(nowNode.x + directions[i].x, nowNode.y + directions[i].y, nowNode.g + 1, 0);
                if (visited.ContainsKey((findNode.x, findNode.y)))
                    continue;
                if (findNode.x >= 10 || findNode.y >= 10 || findNode.x < 0 || findNode.y < 0)
                    continue;
                if (!map[findNode.x, findNode.y])
                    continue;

                findNode.h = GetDistance(findNode, endNode);
                findNode.f = findNode.g + findNode.h;

                priorityQueue.Enqueue(findNode, findNode.f);
                if (parents.ContainsKey(findNode))
                {
                    if (nowNode.f < parents[findNode].f)
                        parents[findNode] = nowNode;
                }
                else
                    parents.Add(findNode, nowNode);
            }
        }

        Debug.Log($"AStar : {DateTime.Now - nowTime}");

        while(answer.Count > 0)
        {
            Node nowNode = answer.Pop();
            Instantiate(MovePrefab, Vector2.left * 8 + Vector2.right * nowNode.x + Vector2.up * nowNode.y, Quaternion.identity, transform);
        }
    }

    void JPS(bool[,] map)
    {
        DateTime nowTime = DateTime.Now;
        PriorityQueue<Node, int> priorityQueue = new();
        Dictionary<(int x, int y), bool> visited = new();
        Dictionary<Node, Node> parents = new();
        Stack<Node> answer = new();

        Node startNode = new(0, 0, 0, 9);
        Node endNode = new(9, 9, 0, 0);
        parents.Add(startNode, startNode);
        priorityQueue.Enqueue(startNode, 0);
        answer.Push(startNode);

        while (priorityQueue.Count > 0)
        {
            Node nowNode = priorityQueue.Dequeue();

            if (visited.ContainsKey((nowNode.x, nowNode.y)))
                continue;
            visited.Add((nowNode.x, nowNode.y), true);

            if (IsSameNode(nowNode, endNode))
            {
                while (!IsSameNode(parents[nowNode], nowNode))
                {
                    answer.Push(nowNode);
                    nowNode = parents[nowNode];
                }
                break;
            }

            for(int i = 0; i < 8; i++)
            {
                if (IsParentDirection(directions[i], nowNode, parents[nowNode]))
                    continue;

                Node findNode = new(nowNode.x + directions[i].x, nowNode.y + directions[i].y, nowNode.g + 1, 0);
                if (findNode.x >= 10 || findNode.y >= 10 || findNode.x < 0 || findNode.y < 0)
                    continue;

                bool isCorner = false;
                while (map[findNode.x, findNode.y] && !isCorner)
                {
                    findNode.x += directions[i].x;
                    findNode.y += directions[i].y;

                    if (findNode.x >= 10 || findNode.y >= 10 || findNode.x < 0 || findNode.y < 0)
                    {
                        findNode.x -= directions[i].x;
                        findNode.y -= directions[i].y;
                        break;
                    }

                    if (!map[findNode.x, findNode.y])
                    {
                        findNode.x -= directions[i].x;
                        findNode.y -= directions[i].y;
                        break;
                    }

                    for (int j = 0; j < 8; j++)
                    {
                        if (findNode.x + directions[j].x >= 10 || findNode.y + directions[j].y >= 10 || findNode.x + directions[j].x < 0 || findNode.y + directions[j].y < 0)
                            continue;
                        if (!map[findNode.x, findNode.y])
                        {
                            isCorner = true;
                            break;
                        }
                    }
                }

                if (visited.ContainsKey((findNode.x, findNode.y)))
                    continue;

                findNode.h = GetDistance(findNode, endNode);
                findNode.f = findNode.g + findNode.h;

                priorityQueue.Enqueue(findNode, findNode.f);
                if (parents.ContainsKey(findNode))
                {
                    if (nowNode.f < parents[findNode].f)
                        parents[findNode] = nowNode;
                }
                else
                    parents.Add(findNode, nowNode);
            }
        }

        while (answer.Count > 0)
        {
            Node nowNode = answer.Pop();
            Instantiate(MovePrefab, Vector2.right * 8 + Vector2.right * nowNode.x + Vector2.up * nowNode.y, Quaternion.identity, transform);
        }


        Debug.Log($"AStar : {DateTime.Now - nowTime}");
    }


    bool IsSameNode(Node target, Node other)
    {
        if (target.x == other.x && target.y == other.y)
            return true;
        return false;
    }

    int GetDistance(Node target, Node other)
    {
        return (target.x - other.x > 0 ? target.x - other.x : other.x - target.x) + (target.y - other.y > 0 ? target.y - other.y : other.y - target.y);
    }

    void DrawMap(bool[,] map)
    {
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                if (!map[i, j])
                {
                    Instantiate(BGPrefab, Vector2.left * 8 + Vector2.right * i + Vector2.up * j, Quaternion.identity, transform);
                    Instantiate(BGPrefab, Vector2.right * 8 + Vector2.right * i + Vector2.up * j, Quaternion.identity, transform);
                }
            }
        }
    }

    bool IsParentDirection((int x, int y) direction, Node target, Node parent)
    {
        if(direction.x < 0 && target.x - parent.x > 0)
            return true;
        else if(direction.x > 0 && target.x - parent.x < 0)
            return true;

        if (direction.y < 0 && target.y - parent.y > 0)
            return true;
        else if (direction.y > 0 && target.y - parent.y < 0)
            return true;

        return false;
    }
}
