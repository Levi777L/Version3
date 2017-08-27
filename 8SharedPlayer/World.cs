using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class World {
    [SerializeField]
    public string name;

    [SerializeField]
    public List<Zone> zones = new List<Zone>();

    [SerializeField]
    public List<DioramaObjectSaver> baseObj = new List<DioramaObjectSaver>();

    public World(string name) {
        this.name = name;
    }
}

[Serializable]
public class Zone {

    public Zone(int zoneId) {
        this.zoneId = zoneId;
        
    }

    [SerializeField]
    public string name = "";

    [SerializeField]
    public int zoneId = 0;

    [SerializeField]
    public MapTag mapType = MapTag.None;

    [SerializeField]
    public List<DioramaObjectSaver> dos = new List<DioramaObjectSaver>();

    [SerializeField]
    public List<ViewPoint> viewpoints = new List<ViewPoint>();

}

[Serializable]
public class ViewPoint {
    [SerializeField]
    public SerializableVector3 pos;
    [SerializeField]
    public SerializableQuaternion rot;
    [SerializeField]
    public float scale;

    public ViewPoint(Vector3 pos, Quaternion rot, float scale) {
        this.pos = pos;
        this.rot = rot;
        this.scale = scale;
    }
}

[Serializable]
public enum MapTag {
    None = 0,
    Environment = 1,
    Adventure_Zone = 2,

    Legacy = 99,
}

