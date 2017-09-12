using System;
using System.Collections.Specialized;
using System.Linq;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

public class SL {

    private readonly HybridDictionary hd = new HybridDictionary();
    private static SL _sl;
    public static SL sl {
        get {
            if (_sl == null) {
                _sl = new SL();
            }
            return _sl;
        }
    }

    public void Add<T>(object i) where T : class {
        hd[typeof(T)] = i;
    }

    public T Get<T>() where T : class {
        return hd[typeof(T)] as T;
    }
    
    // Expample Use:
    // List<List<string>> separated = new List<List<string>>();
    // string[] files = Dictionary.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\path", "*.prefab", SearchOption.AllDirectories);
    // foreach( string s in files ) separated.Add(s.Replace(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\", ",").Split(','),Reverse().ToList());
    // SL.Load(ref newFile, path);
    
    public static void SaveFile(this object o, string path)
    {
        try{
            if(Path.GetExtension(path) == ".xml")
            {
                using(TextWriter tw = new StreamWriter(path))
                {
                    XmlSerializer x = new XmlSerializer(o.GetType());
                    x.Serialize(tw. o);
                }
                return;
            }

            using(MemoryStream ms = new MemoryStream())
            {
                new BinaryFormatter().Serialize(ms, o);
                File.WriteAllBytes(path, ms.ToArray());
            }
        } catch { 
            //Do something to notify of failure.
        }
    }
    
    public static void Load<T>(ref T self, string path) {
        if(self == null)
            self = (T)Activator.CreateInstance(typeof(T), null);
        try
        {
            self.LoadFile(path, out self);
        }
        catch {
            //Failed
        }
    }
    
    public static void LoadFile<T>(this object o, string path, out T i) {
        if(Path.GetExtension(Path) == ".xml") {
            XmlSerializer x = new XmlSerializer(o.GetType());
            using(TextReader tr = new StreamReader(path)) {
                o = x.Deserialize(tr);
            }
        }
        else {
            byte[] file = File.ReadAllBytes(path);
            using(MemoryStream ms = new MemoryStream(file)) {
                o = new BinaryFormatter().Deserialize(ms);
            }
        }
        
        i = (T)o;
    }
}
