using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DungeonGrid {

    private DungeonBlock[,,] dungeonGrid;
    private List<DungeonBlock> mainPath = new List<DungeonBlock>();
    private List<DungeonBlock> correctPath = new List<DungeonBlock>();
    private int size;
    private int cx = 0, cy = 0, cz = 0;
    public int numOfRooms = 0;
    private int minRooms = 100;
    System.Random r;
    DungeonBlock.Direction from;
    bool go = true;

    int one = 0;
    int two = 0;
    int three = 0;
    int four = 0;
    int five = 0;
    int six = 0;
    int seven = 0;

    List<DungeonBlock.Direction> possibleDir = new List<DungeonBlock.Direction>();

    public DungeonBlock GetBlock(Vector3 v) {
        int x, y, z;
        x = (int)v.x;
        y = (int)v.y;
        z = (int)v.z;
        try
        {
            return dungeonGrid[x, y, z];
        }
        catch {
            Debug.Log("Hit null in GetBlock::DungeonGrid");
            return null;
        }
    }

    public DungeonGrid(int size, int seed, int deadEnds, int maxPath) {
        dungeonGrid = new DungeonBlock[size, size, size];
        this.size = size - 1;
        r = new System.Random(seed);
        
        BuildDungeon(deadEnds, maxPath);
    }

    private void BuildDungeon(int deadEnds, int maxPath) {
        if (numOfRooms == 0) {
            DungeonBlock db = new DungeonBlock(DungeonBlock.Direction.start, cx, cy, cz);
            dungeonGrid[cx, cy, cz] = db;
            mainPath.Add(db);
            numOfRooms++;
        }

        //Build the correct path.
        BuildPath(maxPath);
        mainPath[mainPath.Count - 1].MarkEnd();

        foreach (DungeonBlock d in mainPath) {
            correctPath.Add(d);
        }

        BuildDeadends(deadEnds, maxPath / 2);

        PrintStats();
        PrintPossibles();
        
    }

    private void BuildDeadends(int max, int deadPath) {
        for (int i = 0; i < max; i++)
        {
            DungeonBlock deadEndStart = mainPath[r.Next(mainPath.Count - 1)];
            cx = deadEndStart.x;
            cy = deadEndStart.y;
            cz = deadEndStart.z;

            go = true;
            BuildPath(deadPath);
            mainPath[mainPath.Count - 1].MarkDeadEnd();
        }
    }

    private void PrintPossibles() {
        List<string> allPossible = new List<string>();
        foreach (DungeonBlock d in mainPath) {
            allPossible.Add(d.ComboTest());
        }

        List<string> dist = allPossible.Distinct().ToList();
       
        foreach (string possible in dist) {
            int count = allPossible.Count(x => x == possible);
            Console.WriteLine(possible + " " + count);
        }
    }

    private void PrintStats() {
        foreach (DungeonBlock d in mainPath)
        {
            switch (d.GetExitCount())
            {
                case 1:
                    one++;
                    break;
                case 2:
                    two++;
                    break;
                case 3:
                    three++;
                    break;
                case 4:
                    four++;
                    break;
                case 5:
                    five++;
                    break;
                case 6:
                    six++;
                    break;
                default:
                    d.Print();
                    seven++;
                    break;
            }

        }
        Console.WriteLine("1 Exit: " + one);
        Console.WriteLine("2 Exit: " + two);
        Console.WriteLine("3 Exit: " + three);
        Console.WriteLine("4 Exit: " + four);
        Console.WriteLine("5 Exit: " + five);
        Console.WriteLine("6 Exit: " + six);
        Console.WriteLine("More Exit: " + seven);
        Console.WriteLine("Shortest Path " + correctPath.Count + " Rooms.");
    }

    private void GetPossible() {
        int py = cy + 1;
        int px = cx + 1;
        int pz = cz + 1;
        int mx = cx - 1;
        int my = cy - 1;
        int mz = cz - 1;

        //Check Up
        if (py > size)
            py = 0;
        if (dungeonGrid[cx, py, cz] == null)
            possibleDir.Add(DungeonBlock.Direction.top);

        //Check Down
        if (my < 0)
            my = size;
        if (dungeonGrid[cx, my, cz] == null)
            possibleDir.Add(DungeonBlock.Direction.bottom);

        //Check Right
        if (px > size)
            px = 0;
        if (dungeonGrid[px, cy, cz] == null)
            possibleDir.Add(DungeonBlock.Direction.right);

        //Check Left
        if (mx < 0)
            mx = size;
        if (dungeonGrid[mx, cy, cz] == null)
            possibleDir.Add(DungeonBlock.Direction.left);

        //Check Forward
        if (pz > size)
            pz = 0;
        if (dungeonGrid[cx, cy, pz] == null)
            possibleDir.Add(DungeonBlock.Direction.front);

        //Check Back
        if (mz < 0)
            mz = size;
        if (dungeonGrid[cx, cy, mz] == null)
            possibleDir.Add(DungeonBlock.Direction.back);
    }

    private void BuildPath(int maxPath) {
        int pathRooms = 0;
        while (go && pathRooms < maxPath)
        {
            possibleDir.Clear();
            GetPossible();
            if (possibleDir.Count == 0)
            {
                go = false;
                
            }
            else
            {
                pathRooms++;
                int nextRand = r.Next(possibleDir.Count);

                dungeonGrid[cx, cy, cz].AddExit(possibleDir[nextRand]);

                switch (possibleDir[nextRand])
                {
                    case DungeonBlock.Direction.right:
                        cx++;
                        from = DungeonBlock.Direction.left;
                        if (cx > size)
                            cx = 0;
                        break;
                    case DungeonBlock.Direction.left:
                        cx--;
                        from = DungeonBlock.Direction.right;
                        if (cx < 0)
                            cx = size;
                        break;
                    case DungeonBlock.Direction.top:
                        cy++;
                        from = DungeonBlock.Direction.bottom;
                        if (cy > size)
                            cy = 0;
                        break;
                    case DungeonBlock.Direction.bottom:
                        cy--;
                        from = DungeonBlock.Direction.top;
                        if (cy < 0)
                            cy = size;
                        break;
                    case DungeonBlock.Direction.front:
                        cz++;
                        from = DungeonBlock.Direction.back;
                        if (cz > size)
                            cz = 0;
                        break;
                    case DungeonBlock.Direction.back:
                        cz--;
                        from = DungeonBlock.Direction.front;
                        if (cz < 0)
                            cz = size;
                        break;
                }
                
                DungeonBlock db = new DungeonBlock(from, cx, cy, cz);
                dungeonGrid[cx, cy, cz] = db;
                mainPath.Add(db);
                numOfRooms++;
            }
        }
    }

}
