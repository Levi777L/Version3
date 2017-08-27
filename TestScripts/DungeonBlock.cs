using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonBlock {

    public enum Direction {
        top,
        bottom,
        left,
        right,
        front,
        back,
        portal,
        start,
        end,
        deadend,
    }

    private Direction enter;
    private List<Direction> exit = new List<Direction>();


    public int x, y, z;

    public DungeonBlock(Direction enter, int x, int y, int z) {
        this.x = x;
        this.y = y;
        this.z = z;
        this.enter = enter;
    }
    
    public void AddExit(Direction d) {
        exit.Add(d);
    }

    public void MarkEnd() {
        exit.Clear();
        exit.Add(Direction.end);
    }

    public void MarkDeadEnd() {
        exit.Clear();
        exit.Add(Direction.deadend);
    }

    public int GetExitCount() {
        return exit.Count;
    }

    public void Print() {
        foreach (Direction d in exit) {
            Console.WriteLine(d.ToString());
        }
    }

    public string ComboTest() {
        string ret = "";
        if (enter == Direction.start) {
            ret += "start, ";
        }
        if (enter == Direction.top || exit.FindIndex(x => x == Direction.top) >= 0) {
            ret += "top, ";
        }
        if (enter == Direction.bottom || exit.FindIndex(x => x == Direction.bottom) >= 0)
        {
            ret += "bottom, ";
        }
        if (enter == Direction.front || exit.FindIndex(x => x == Direction.front) >= 0)
        {
            ret += "front, ";
        }
        if (enter == Direction.back || exit.FindIndex(x => x == Direction.back) >= 0)
        {
            ret += "back, ";
        }
        if (enter == Direction.left || exit.FindIndex(x => x == Direction.left) >= 0)
        {
            ret += "left, ";
        }
        if (enter == Direction.right || exit.FindIndex(x => x == Direction.right) >= 0)
        {
            ret += "right, ";
        }
        if (exit.FindIndex(x => x == Direction.end) >= 0)
        {
            ret += "end, ";
        }
        if (exit.FindIndex(x => x == Direction.deadend) >= 0)
        {
            ret += "deadend, ";
        }
        ret = ret.Substring(0, ret.Length - 2);

        return ret;
    }

    public bool GetNorth() {
        return enter == Direction.front || exit.FindIndex(x => x == Direction.front) >= 0;
    }
    public bool GetEast()
    {
        return enter == Direction.right || exit.FindIndex(x => x == Direction.right) >= 0;
    }
    public bool GetSouth()
    {
        return enter == Direction.back || exit.FindIndex(x => x == Direction.back) >= 0;
    }
    public bool GetWest()
    {
        return enter == Direction.left || exit.FindIndex(x => x == Direction.left) >= 0;
    }
    public bool GetUp()
    {
        return enter == Direction.top || exit.FindIndex(x => x == Direction.top) >= 0;
    }
    public bool GetDown()
    {
        return enter == Direction.bottom || exit.FindIndex(x => x == Direction.bottom) >= 0;
    }
    public bool GetStart() {
        return enter == Direction.start;
    }
    public bool GetEnd()
    {
        return exit.FindIndex(x => x == Direction.end) >= 0;
    }
    public bool GetDeadEnd() {
        return exit.FindIndex(x => x == Direction.deadend) >= 0;
    }
}
