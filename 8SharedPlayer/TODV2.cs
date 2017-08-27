using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System.Linq;
using System.Xml.Serialization;

public static class TODV2
{
    //readonly?
    private static List<BundleItem> _tod;
    private static int id;

    static TODV2()
    {
        _tod = (List<BundleItem>)LoadTod();
        //id = _tod.Count;
        id = _tod.Max(t => t.id) + 1;
        Debug.Log(id + " Items in the TOD");
    }

    public static List<BundleItem> tod
    {
        get
        {
            return _tod;
        }
    }

    public static void DevAddTod(BundleItem i)
    {
        i.id = TODV2.id;
        TODV2.id++;
        _tod.Add(i);
        SaveTod();
    }
    
    public static void DevUpdateTod(BundleItem i)
    {
        BundleItem old = _tod.Find(x => x.id == i.id);
        old = i;
        SaveTod();
    }

    public static BundleItem GetItemByID(int id)
    {
        return _tod.Find(x => x.id == id);
    }

    public static List<BundleItem> GetItemByTopCategory(BundleItem.TopCategory tc) {
        return _tod.FindAll(x => x.topFolder == tc);
    }

    public static List<BundleItem> GetItemByBundle(BundleItem.BundleNames bn) {
        return _tod.FindAll(x => x.bundle == bn);
    }

    public static void SaveTod()
    {
        string path = Globals.TODPATH;
        using (MemoryStream stream = new MemoryStream())
        {
            new BinaryFormatter().Serialize(stream, (object)_tod);
            File.WriteAllBytes(path, stream.ToArray());
        }
        
        //try {
        //    var serializer = new XmlSerializer(typeof(List<BundleItem>));

        //    using(var writer = new StreamWriter(Application.dataPath + @"/tod.xml"))
        //    {
        //        serializer.Serialize(writer, _tod);
        //    }
        //} catch {
        //    Debug.Log("Failed to create XML");
        //}
    }

    public static void SaveGeneric<T>(string filename, T obj)
    {
        using (MemoryStream stream = new MemoryStream())
        {
            new BinaryFormatter().Serialize(stream, (object)obj);
            File.WriteAllBytes(filename, stream.ToArray());
        }
    }

    public static T LoadGeneric<T>(string filename) {
        try
        {
            byte[] file = File.ReadAllBytes(filename);
            using (MemoryStream stream = new MemoryStream(file))
            {
                object o = new BinaryFormatter().Deserialize(stream);
                return (T)o;
            }
        }
        catch
        {

        }
        return default(T);
    }


    public static object LoadTod()
    {
        string path = Globals.TODPATH;
        try
        {
            byte[] file = File.ReadAllBytes(path);
            using (MemoryStream stream = new MemoryStream(file))
            {
                return new BinaryFormatter().Deserialize(stream);
            }
        }
        catch
        {

        }
        return null;
    }

    

}
