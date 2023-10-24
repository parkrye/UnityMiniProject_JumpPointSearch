using System;
using System.Collections;
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
            { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            { true , true , true , true , true , true , true , true , true , true , true , true , true , true , true , true , true , true , true , true , true , true , true , true , true , true , true , true , true , true , true , true , true , true , true , true , true , true , true , true , true , true , true , true , true , true , true , true , true , true , true , true , true , true , true , true , true , true , true , true , true , true , true , true , true , true , true , true , true , true , true , true , true , true , true , true , true , true , true , true , true , true , true , true , true , true , true , true , true , true , true , true , true , true , true , true , true , true , true },
            { true , false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            { true , true , true , true , false, true , true , true , true , true , true , true , false, true , true , true , true , false, true , true , true , true , true , true , true , true , false, true , true , true , true , true , true , true , true , false, true , true , true , true , true , true , true , true , false, true , false, true , true , true , true , true , true , false, false, true , true , true , true , true , true , true , false, true , true , true , true , true , true , true , true , false, true , true , true , true , true , true , true , true , false, true , true , true , true , false, true , false, true , true , true , true , true , true , true , true , false, true , true },
            { false, false, false, true , false, true , true , false, true , true , true , false, false, true , false, false, true , false, true , true , false, true , true , false, false, true , false, true , true , false, true , true , false, false, true , false, true , true , false, true , true , false, false, true , false, true , false, false, true , true , false, false, true , false, false, true , false, true , true , false, false, true , false, true , true , false, true , true , false, false, true , false, true , true , false, true , true , false, false, true , false, true , true , false, true , false, true , false, true , true , false, true , true , false, false, true , true , false, true },
            { true , true , true , true , false, true , true , false, true , true , true , false, false, true , true , true , true , false, true , true , false, true , true , true , true , true , false, true , true , false, true , true , true , true , true , false, true , true , false, true , true , true , true , true , false, true , false, false, true , true , true , true , true , false, false, true , false, true , true , true , true , true , false, true , true , false, true , true , true , true , true , false, true , true , false, true , true , true , true , true , false, true , true , false, true , false, true , false, true , true , false, true , true , true , true , true , false, true , true },
            { true , true , true , true , false, true , true , false, false, true , true , false, false, true , true , true , true , false, true , true , false, false, true , true , true , true , false, true , true , false, false, true , true , true , true , false, true , true , false, false, true , true , true , true , false, true , false, false, false, true , true , true , true , false, false, true , false, false, true , true , true , true , false, true , true , false, false, true , true , true , true , false, true , true , false, false, true , true , true , true , false, true , true , false, false, false, true , false, true , true , false, false, true , true , true , true , false, false, true },
            { false, true , false, false, false, true , true , false, true , true , true , false, false, true , true , false, false, false, true , true , false, true , true , true , false, false, false, true , true , false, true , true , true , false, false, false, true , true , false, true , true , true , false, false, false, true , false, false, true , true , true , false, false, false, false, true , false, true , true , true , false, false, false, true , true , false, true , true , true , false, false, false, true , true , false, true , true , true , false, false, false, true , true , false, true , false, true , false, true , true , false, true , true , true , false, false, false, true , false},
            { true , true , true , true , false, true , true , false, true , true , true , false, false, true , true , true , true , false, true , true , false, true , true , true , true , true , false, true , true , false, true , true , true , true , true , false, true , true , false, true , true , true , true , true , false, true , false, false, true , true , true , true , true , false, false, true , false, true , true , true , true , true , false, true , true , false, true , true , true , true , true , false, true , true , false, true , true , true , true , true , false, true , true , false, true , false, true , false, true , true , false, true , true , true , true , true , false, false, true },
            { true , true , true , true , true , true , true , false, true , true , true , false, false, true , true , true , true , true , true , true , false, true , true , true , true , true , true , true , true , false, true , true , true , true , true , true , true , true , false, true , true , true , true , true , true , true , false, false, true , true , true , true , true , true , false, true , false, true , true , true , true , true , true , true , true , false, true , true , true , true , true , true , true , true , false, true , true , true , true , true , true , true , true , false, true , false, true , true , true , true , false, true , true , true , true , true , true , false, true },
            { true , true , true , true , false, true , true , false, true , true , true , false, false, true , true , true , true , false, true , true , false, true , true , true , true , true , false, true , true , false, true , true , true , true , true , false, true , true , false, true , true , true , true , true , false, true , true , false, true , true , true , true , true , false, false, true , false, true , true , true , true , true , false, true , true , false, true , true , true , true , true , false, true , true , false, true , true , true , true , true , false, true , true , false, true , false, true , false, true , true , false, true , true , true , true , true , false, false, true },
            { true , true , true , true , false, true , true , false, true , true , true , false, false, true , true , true , true , false, true , true , false, true , true , true , true , true , false, true , true , false, true , true , true , true , true , false, true , true , false, true , true , true , true , true , false, true , true , false, true , true , true , true , true , false, false, true , false, true , true , true , true , true , false, true , true , false, true , true , true , true , true , false, true , true , false, true , true , true , true , true , false, true , true , false, true , false, true , false, true , true , false, true , true , true , true , true , false, true , true },
            { true , true , true , true , false, true , true , true , true , true , true , true , false, true , true , true , true , false, true , true , true , true , true , true , true , true , false, true , true , true , true , true , true , true , true , false, true , true , true , true , true , true , true , true , false, true , true , true , true , true , true , true , true , false, false, true , true , true , true , true , true , true , false, true , true , true , true , true , true , true , true , false, true , true , true , true , true , true , true , true , false, true , true , true , true , false, true , false, true , true , true , true , true , true , true , true , false, true , true },
            { false, false, false, true , false, true , true , false, true , true , true , false, false, true , false, false, true , false, true , true , false, true , true , false, false, true , false, true , true , false, true , true , false, false, true , false, true , true , false, true , true , false, false, true , false, true , true , false, true , true , false, false, true , false, false, true , false, true , true , false, false, true , false, true , true , false, true , true , false, false, true , false, true , true , false, true , true , false, false, true , false, true , true , false, true , false, false, false, true , true , false, true , true , false, false, true , false, true , true },
            { true , true , true , true , false, true , true , false, true , true , true , false, false, true , true , true , true , false, true , true , false, true , true , true , true , true , false, true , true , false, true , true , true , true , true , false, true , true , false, true , true , true , true , true , false, true , true , false, true , true , true , true , true , false, false, true , false, true , true , true , true , true , false, true , true , false, true , true , true , true , true , false, true , true , false, true , true , true , true , true , false, true , true , false, true , true , true , false, true , true , false, true , true , true , true , true , false, true , true },
            { true , true , true , true , false, true , true , false, false, true , true , false, false, true , true , true , true , false, true , true , false, false, true , true , true , true , false, true , true , false, false, true , true , true , true , false, true , true , false, false, true , true , true , true , false, true , true , false, false, true , true , true , true , false, false, true , false, false, true , true , true , true , false, true , true , false, false, true , true , true , true , false, true , true , false, false, true , true , true , true , false, true , true , false, false, false, true , false, true , true , false, false, true , true , true , true , false, true , true },
            { false, false, false, false, false, false, false, false, false, false, true , false, false, true , true , false, false, false, true , true , false, true , true , true , false, false, false, true , true , false, true , true , true , false, false, false, true , true , false, true , true , true , false, false, false, true , true , false, true , true , true , false, false, false, false, true , false, true , true , true , false, false, false, true , true , false, true , true , true , false, false, false, true , true , false, true , true , true , false, false, false, true , true , false, true , false, true , false, true , true , false, true , true , true , false, false, false, true , true },
            { true , true , true , true , false, true , true , false, true , true , true , false, true , true , true , true , true , false, true , true , false, true , true , true , true , true , false, true , true , false, true , true , true , true , true , false, true , true , false, true , true , true , true , true , false, true , false, false, true , true , true , true , true , false, false, true , false, true , true , true , true , true , false, true , true , false, true , true , true , true , true , false, true , true , false, true , true , true , true , true , false, true , true , false, true , false, true , false, true , true , false, true , true , true , true , true , false, true , true },
            { true , true , true , true , false, true , true , true , true , true , true , false, true , true , true , true , true , false, true , true , true , true , true , true , true , true , false, true , true , false, true , true , true , true , true , false, true , true , false, true , true , true , true , true , false, true , true , true , true , true , true , true , true , false, false, true , true , true , true , true , true , true , false, true , true , true , true , true , true , true , true , false, true , true , true , true , true , true , true , true , false, true , true , true , true , false, true , false, true , true , true , true , true , true , true , true , false, true , true },
            { false, false, false, true , false, true , true , false, true , true , true , false, true , true , false, false, true , false, true , true , false, true , true , false, false, true , false, true , true , false, true , true , false, false, true , false, true , true , false, true , true , false, false, true , false, true , true , false, true , true , false, false, true , false, false, true , false, true , true , false, false, true , false, true , true , false, false, true , false, false, true , false, true , true , false, true , true , false, false, true , false, true , true , false, true , false, true , false, true , true , false, true , true , false, false, true , false, true , true },
            { true , true , true , true , true , true , true , false, true , true , true , false, true , true , true , true , true , false, true , true , false, true , true , true , true , true , false, true , true , false, true , true , true , true , true , false, true , true , false, true , true , true , true , true , false, true , false, false, true , true , true , true , true , false, false, true , false, true , true , true , true , true , false, true , true , false, true , true , true , true , true , false, true , true , false, true , true , true , true , true , false, true , true , false, true , false, true , false, true , true , false, true , true , true , true , true , false, true , true },
            { true , true , true , true , false, false, false, false, false, false, false, false, false, true , true , true , true , false, true , true , false, false, true , true , true , true , false, true , true , false, false, true , true , true , true , false, true , true , false, false, true , true , true , true , false, true , false, false, false, true , true , true , true , false, false, true , false, false, true , true , true , true , false, true , true , false, false, true , true , true , true , false, true , true , false, false, true , true , true , true , false, true , true , false, false, false, true , false, true , true , false, false, true , true , true , true , false, true , true },
            { false, true , false, false, false, true , true , false, true , true , true , false, true , true , true , false, false, false, true , true , false, true , true , true , false, false, false, true , true , false, true , true , true , false, false, false, true , true , false, true , true , true , false, false, false, true , false, false, true , true , true , false, false, false, false, true , false, true , true , true , false, false, false, true , true , false, true , true , true , false, false, false, true , true , false, true , true , true , false, false, false, true , true , false, true , false, true , false, true , true , false, true , true , true , false, false, false, true , true },
            { true , true , true , true , false, true , true , false, true , true , true , false, true , true , true , true , true , false, true , true , false, true , true , true , true , true , false, true , true , false, true , true , true , true , true , false, true , true , false, true , true , true , true , true , false, true , false, false, true , true , true , true , true , false, false, true , false, true , true , true , true , true , false, true , true , false, true , true , true , true , true , false, true , true , false, true , true , true , true , true , false, true , true , false, true , false, true , false, true , true , false, true , true , true , true , true , false, false, true },
            { true , true , true , true , true , true , true , false, true , true , true , false, true , true , true , true , true , true , true , true , false, true , true , true , true , true , true , true , true , false, true , true , true , true , true , true , true , true , false, true , true , true , true , true , true , true , false, false, true , true , true , true , true , true , false, true , false, true , true , true , true , true , true , true , true , false, true , true , true , true , true , true , true , true , false, true , true , true , true , true , true , true , true , false, true , false, true , true , true , true , false, true , true , true , true , true , true , false, true },
            { true , true , true , true , false, true , true , false, true , true , true , false, true , true , true , true , true , false, true , true , false, true , true , true , true , true , false, true , true , false, true , true , true , true , true , false, true , true , false, true , true , true , true , true , false, true , false, false, true , true , true , true , true , false, false, true , false, true , true , true , true , true , false, true , true , false, true , true , true , true , true , false, true , true , false, true , true , true , true , true , false, true , true , false, true , false, true , false, true , true , false, true , true , true , true , true , false, false, true },
            { false, true , false, false, false, true , true , false, true , true , true , false, true , true , true , false, false, false, true , true , false, true , true , true , false, false, false, true , true , false, true , true , true , false, false, false, true , true , false, true , true , true , false, false, false, true , false, false, true , true , true , false, false, false, false, true , false, true , true , true , false, false, false, true , true , false, true , true , true , false, false, false, true , true , false, true , true , true , false, false, false, true , true , false, true , false, true , false, true , true , false, true , true , true , false, false, false, true , false},
            { true , true , true , true , false, true , true , false, true , true , true , false, true , true , true , true , true , false, true , true , false, true , true , true , true , true , false, true , true , false, true , true , true , true , true , false, true , true , false, true , true , true , true , true , false, true , false, false, true , true , true , true , true , false, false, true , false, true , true , true , true , true , false, true , true , false, true , true , true , true , true , false, true , true , false, true , true , true , true , true , false, true , true , false, true , false, true , false, true , true , false, true , true , true , true , true , false, true , true },
            { true , true , true , true , false, true , true , true , true , true , true , true , true , true , true , true , true , false, true , true , true , true , true , true , true , true , false, true , true , false, true , true , true , true , true , false, true , true , true , true , true , true , true , true , false, true , true , true , true , true , true , true , true , false, false, true , true , true , true , true , true , true , false, true , true , true , true , true , true , true , true , false, true , true , true , true , true , true , true , true , false, true , true , true , true , false, true , false, true , true , true , true , true , true , true , true , false, true , true },
            { false, false, false, true , false, true , true , false, true , true , true , false, true , true , false, false, true , false, true , true , false, true , true , false, false, true , false, true , true , false, true , true , false, false, true , false, true , true , false, true , true , false, false, true , false, true , false, false, true , true , false, false, true , false, false, true , false, true , true , false, false, true , false, true , true , false, true , true , false, false, true , false, true , true , false, true , true , false, false, true , false, true , true , false, true , false, false, false, true , true , false, true , true , false, false, true , false, true , true },
            { true , true , true , true , false, true , true , false, true , true , true , false, true , true , true , true , true , false, true , true , false, true , true , true , true , true , false, true , true , false, true , true , true , true , true , false, true , true , false, true , true , true , true , true , false, true , false, false, true , true , true , true , true , false, false, true , false, true , true , true , true , true , false, true , true , false, true , true , true , true , true , false, true , true , false, true , true , true , true , true , false, true , true , false, true , false, true , false, true , true , false, true , true , true , true , true , false, true , true },
            { true , true , true , true , false, true , true , false, false, true , true , false, false, true , true , true , true , false, true , true , false, false, true , true , true , true , false, true , true , false, false, true , true , true , true , false, true , true , false, false, true , true , true , true , false, true , false, false, false, true , true , true , true , false, false, true , false, false, true , true , true , true , false, true , true , false, false, true , true , true , true , false, true , true , false, false, true , true , true , true , false, true , true , false, false, false, true , false, true , true , false, false, true , true , true , true , false, true , true },
            { false, true , false, false, false, true , true , false, true , true , true , false, true , true , true , false, false, false, true , true , false, true , true , true , false, false, false, true , true , false, true , true , true , false, false, false, true , true , false, true , true , true , false, false, false, true , false, false, true , true , true , false, false, true , true , true , false, true , true , true , false, false, false, true , true , false, true , true , true , false, false, false, true , true , false, true , true , true , false, false, false, true , true , false, true , false, true , false, true , true , false, true , true , true , false, false, false, true , true },
            { true , true , true , true , false, true , true , false, true , true , true , false, true , true , true , true , true , false, true , true , false, true , true , true , true , true , false, true , true , false, true , true , true , true , true , false, true , true , false, true , true , true , true , true , false, true , false, false, true , true , true , true , false, true , false, true , false, true , true , true , true , true , false, true , true , false, true , true , true , true , true , false, true , true , false, true , true , true , true , true , false, true , true , false, true , false, true , false, true , true , false, true , true , true , true , true , false, true , true },
            { true , true , true , true , true , true , true , false, true , true , true , false, true , true , true , true , true , true , true , true , false, true , true , true , true , true , true , true , true , false, true , true , true , true , true , true , true , true , false, true , true , true , true , true , true , true , false, false, true , true , true , true , true , true , false, true , false, true , true , true , true , true , true , true , true , false, true , true , true , true , true , true , true , true , false, true , true , true , true , true , true , true , true , false, true , false, true , true , true , true , false, true , true , true , true , true , false, false, true },
            { true , true , true , true , false, true , true , false, true , true , true , false, true , true , true , true , true , false, true , true , false, true , true , true , true , true , false, true , true , false, true , true , true , true , true , false, true , true , false, true , true , true , true , true , false, true , false, false, true , true , true , true , true , false, true , true , false, true , true , true , true , true , false, true , true , false, true , true , true , true , true , false, true , true , false, true , true , true , true , true , false, true , true , false, true , false, true , false, true , true , false, true , true , true , true , true , false, true , true },
            { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false}
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

        Node startNode = new(1, 0, 0, 0);
        Node endNode = new(map.GetLength(0) - 2, map.GetLength(1) - 1, 0, 0);
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
                if (findNode.x >= map.GetLength(0) || findNode.y >= map.GetLength(1) || findNode.x < 0 || findNode.y < 0)
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

        StartCoroutine(DrawMove(answer, false));
    }

    void JPS(bool[,] map)
    {
        DateTime nowTime = DateTime.Now;
        PriorityQueue<Node, int> priorityQueue = new();
        Dictionary<(int x, int y), bool> visited = new();
        Dictionary<Node, Node> parents = new();
        Stack<Node> answer = new();

        Node startNode = new(1, 0, 0, 0);
        Node endNode = new(map.GetLength(0) - 2, map.GetLength(1) - 1, 0, 0);
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
                if (IsParentDirection(i, nowNode, parents[nowNode]) && !IsSameNode(parents[nowNode], nowNode))
                    continue;

                Node findNode = nowNode;

                int moveDistance = 0;
                while (map[findNode.x, findNode.y])
                {
                    findNode.x += directions[i].x;
                    findNode.y += directions[i].y;

                    if (findNode.x >= map.GetLength(0) || findNode.y >= map.GetLength(1) || findNode.x < 0 || findNode.y < 0 || !map[findNode.x, findNode.y])
                    {
                        findNode.x -= directions[i].x;
                        findNode.y -= directions[i].y;

                        if (!parents.ContainsKey(findNode) || nowNode.f < parents[findNode].f)
                        {
                            AddNewNode(priorityQueue, parents, findNode, nowNode, moveDistance, endNode);
                        }
                    }

                    moveDistance++;

                    int corner = i < 4 ? 4 : 8;
                    for (int j = 0; j < corner; j++)
                    {
                        if (findNode.x + directions[j].x >= map.GetLength(0) || findNode.y + directions[j].y >= map.GetLength(1) || findNode.x + directions[j].x < 0 || findNode.y + directions[j].y < 0)
                            continue;
                        if (!map[findNode.x + directions[j].x, findNode.y + directions[j].y])
                        {
                            if (!parents.ContainsKey(findNode) || nowNode.f < parents[findNode].f)
                            {
                                AddNewNode(priorityQueue, parents, findNode, nowNode, moveDistance, endNode);
                            }
                        }
                    }
                }

                if (!parents.ContainsKey(findNode) || nowNode.f < parents[findNode].f)
                {
                    AddNewNode(priorityQueue, parents, findNode, nowNode, moveDistance, endNode);
                }
            }
        }

        Debug.Log($"JPS : {DateTime.Now - nowTime}");


        StartCoroutine(DrawMove(answer, true));
    }

    void AddNewNode(PriorityQueue<Node, int> priorityQueue, Dictionary<Node, Node> parents, Node findNode, Node nowNode, int moveDistance, Node endNode)
    {
        findNode.g += moveDistance;
        findNode.h = GetDistance(findNode, endNode);
        findNode.f = findNode.g + findNode.h;
        parents[findNode] = nowNode;
        priorityQueue.Enqueue(findNode, findNode.f);
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
        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                if (!map[i, j])
                {
                    Instantiate(BGPrefab, Vector2.left * 50 + Vector2.right * i + Vector2.up * j, Quaternion.identity, transform);
                    Instantiate(BGPrefab, Vector2.right * 50 + Vector2.right * i + Vector2.up * j, Quaternion.identity, transform);
                }
            }
        }
    }

    IEnumerator DrawMove(Stack<Node> answer, bool isRight)
    {
        WaitForSeconds ws = new WaitForSeconds(0.05f);
        while (answer.Count > 0)
        {
            Node nowNode = answer.Pop();
            if(!isRight)
                Instantiate(MovePrefab, Vector2.left * 50 + Vector2.right * nowNode.x + Vector2.up * nowNode.y, Quaternion.identity, transform);
            else
                Instantiate(MovePrefab, Vector2.right * 50 + Vector2.right * nowNode.x + Vector2.up * nowNode.y, Quaternion.identity, transform);
            yield return ws;
        }
    }

    bool IsParentDirection(int direction, Node target, Node parent)
    {
        switch(direction)
        {
            case 0:
                if (target.x < parent.x)
                    return false;
                return true;
            case 1:
                if (target.x > parent.x)
                    return false;
                return true;
            case 2:
                if (target.y < parent.y)
                    return false;
                return true;
            case 3:
                if (target.y > parent.y)
                    return false;
                return true;
            case 4:
                if (target.x < parent.x || target.y < parent.y)
                    return false;
                return true;
            case 5:
                if (target.x < parent.x || target.y > parent.y)
                    return false;
                return true;
            case 6:
                if (target.x > parent.x || target.y < parent.y)
                    return false;
                return true;
            case 7:
                if (target.x > parent.x || target.y > parent.y)
                    return false;
                return true;
            default:
                return true;
        }
    }
}
