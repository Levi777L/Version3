using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Common : MonoBehaviour
{

    AssetBundleCreateRequest abcr;
    

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public GameObject CreateObj()
    {
        return new GameObject("Empty");
    }

    private void SetTag(Transform t, string tag) {
        t.tag = tag;
        foreach(Transform c in t)
            SetTag(c, tag);
    }

    public static GameObject CopyGo(GameObject go, Vector3 scale, Transform parent, bool addCollider, bool resetPos)
    {
        Transform savedParent = go.transform.parent;
        go.transform.parent = null;

        GameObject newObj = GameObject.Instantiate(go);

        newObj.transform.parent = parent;
        newObj.transform.localScale = scale;


        if (resetPos)
            newObj.transform.localPosition = Vector3.zero;


        go.transform.parent = savedParent;

        return newObj;
    }

    public static void UpdateLightScale(GameObject go, float scale)
    {
        Light[] ls = go.GetComponentsInChildren<Light>();
        foreach (Light l in ls)
        {
            l.intensity *= scale;
            l.range *= scale;
        }

    }
    
    public static void DestroyAll(GameObject go) {

            
        /*
        Animation[] ani = go.GetComponentsInChildren<Animation>();
        foreach (Animation a in ani)
            Destroy(a);

        Animator[] ani2 = go.GetComponentsInChildren<Animator>();
        foreach (Animator a in ani2)
            Destroy(a);
        */  


        LineRenderer[] lr = go.GetComponentsInChildren<LineRenderer>();
        foreach(LineRenderer r in lr)
        {
            Destroy(r);
        }
            
    }
    
    public static void ScaleAll(GameObject go, float scale) {
        UpdateLightScale(go, scale);
        UpdateParticleScale(go, new Vector3(scale,scale,scale));
    }

    public static void UpdateParticleScale(GameObject go, Vector3 scale)
    {
        ParticleSystem[] particles = go.GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem p in particles)
        {
            p.Clear();
            p.startSize *= scale.magnitude;

            p.startSpeed *= scale.magnitude;
            ParticleSystemRenderer[] renderer = p.GetComponentsInChildren<ParticleSystemRenderer>();
            foreach (ParticleSystemRenderer r in renderer)
            {
                r.lengthScale *= scale.magnitude;
                r.velocityScale *= scale.magnitude;
                r.maxParticleSize *= scale.magnitude;
            }
            p.Simulate(p.duration);
            p.Play();
        }

    }

    public static float MAXCOLOR = 8f;
    public static Color GetColor(float f) {
        if(f < 1)
            return Color.Lerp(Color.white, Color.yellow, f);
        else if (f < 2)
            return Color.Lerp(Color.yellow, Color.red, f-1);
        else if (f < 3)
            return Color.Lerp(Color.red, Color.magenta, f-2);
        else if (f < 4)
            return Color.Lerp(Color.magenta, Color.blue, f-3);
        else if (f < 5)
            return Color.Lerp(Color.blue, Color.cyan, f-4);   
        else if (f < 6)
            return Color.Lerp(Color.cyan, Color.green, f-5); 
        else if (f < 7)
            return Color.Lerp(Color.green, Color.yellow, f-6); 
        else if (f < 8)
            return Color.Lerp(Color.yellow, Color.white, f-7);     
        
        return Color.white;
        
    }

    public static void ReassignTextures(GameObject go, bool blockToon)
    {
        try
        {
            Renderer[] renderers;
            Material[] materials;
            string[] shaders;
            renderers = go.GetComponentsInChildren<Renderer>();
            foreach (var rend in renderers)
            {
                materials = rend.sharedMaterials;
                shaders = new string[materials.Length];

                for (int i = 0; i < materials.Length; i++)
                {
                    shaders[i] = materials[i].shader.name;
                    //Debug.Log(materials[i].shader.name);
                }

                for (int i = 0; i < materials.Length; i++)
                materials[i].shader = Shader.Find(shaders[i]);
            }
        }
        catch { }
    }

}
