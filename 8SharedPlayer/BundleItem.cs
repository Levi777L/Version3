using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BundleItem {

    public int Id { get { return id; } set { id = value; } }
    public string DisplayName { get { return name; } set { name = value; } }
    public string PrefabName { get { return prefabName; } set { prefabName = value; } }
    public BundleNames BundleName { get { return bundle; } set { bundle = value; } }
    public TopCategory TopFolder { get { return topFolder; } set { topFolder = value; } }
    public string FolderPath { get { return path; } set { path = value; } }
    public bool HasParticle { get { return hasParticle; } set { hasParticle = value; } }
    public bool HasLight { get { return hasLight; } set { hasLight = value; } }
    public bool HasAnimation { get { return hasAnimation; } set { hasAnimation = value; } }

    public float GridScale { get { return gridScale; } set { gridScale = value; } }
    public bool MultiStamp { get { return multiStamp; } set { multiStamp = value; } }
    public bool RebuildFlag { get { return rebuildFlag; } set { rebuildFlag = value; } }
    public bool LoadFromResources { get { return loadFromResources; } set { loadFromResources = value; } }
    public int SavedInt { get { return savedInt; } set { savedInt = value; } }
    public float SavedFloat { get { return savedFloat; } set { savedFloat = value; } }
    public int RecLayer { get { return recLayer; } set { recLayer = value;} }

    public int id;
    public string name;
    public string prefabName;
    public BundleNames bundle;
    public TopCategory topFolder;
    public string path;
    public bool hasParticle;
    public bool hasLight;
    public bool hasAnimation;

    public float gridScale;
    public bool multiStamp;
    public bool rebuildFlag;
    public bool loadFromResources;
    public int savedInt;
    public float savedFloat;
    public int recLayer;

    public SerializableVector3 offsetPos;
    public SerializableQuaternion offsetRot;
    public float offsetScale;


    public enum TopCategory
    {
        Staging = 0,
        SciFi = 3,
        Fantasy = 1,
        Modern = 2,
        Environment = 4,
        PostApocalyptic = 5,
        Characters = 20,
        Effects = 30,
        Decals = 40,
        Terrain = 50,

        Events = 80,
        Other = 99,

        PolygonArt = 100,
    }

    public enum BundleNames
    {
        Staging = 0,
        Characters = 2,
        Triggers = 3,
        Chests = 4,

        HQ_Interiors = 10,

        HQ_Dungeons1 = 20,
        HQ_Enemies = 21,
        HQ_Weapons = 30,

        Custom_Ships_1 = 100,
        HQ_Space = 101,

        Custom_Rooms_1 = 200,

        Low_Poly_Town = 300,
        Low_Poly_Apocalypse = 305,

        LP_Enemies = 325,

        Polygon_Fantasy = 350,
        Polygon_Pirates = 351,
        Polygon_Adventure = 352,
        Polygon_Knights = 353,
        Polygon_Samurai = 354,
        Polygon_Vikings = 355,

        Cartoon_Apocalypse = 400,

        Pigeons_War = 450,

        Nature = 800,
        Terrain = 801,
        SFTerrain = 802,
        SFCaves = 803,

        Assorted_Low_Poly = 900,

        Effects = 950,
        Chat_Bubbles = 951,
        Decals = 952,
        Maps = 953,

        Voxels = 1,
        HandPainted = 999,
        Yughues = 998,
    }

    public BundleItem() { }
}
