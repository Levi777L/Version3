using System;
using System.Collections.Specialized;
using System.Linq;

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
}
