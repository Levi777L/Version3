using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdventureTester : IMode
{
    private static AdventureTester instance;
    private static GameManager manager;
    private static IVRControl control;
    private static WorldBuilderMain shared;
    private static GameObject[] placeholders;

    private static bool characterSpawned = false;

    public static AdventureTester Instance()
    {
        if (instance == null)
        {
            instance = new AdventureTester();
            manager = SL.sl.Get<GameManager>();
            control = SL.sl.Get<IVRControl>();
            shared = SL.sl.Get<WorldBuilderMain>();
        }
        return instance;
    }

    public void ButtonActivate(Node n, bool shift = false)
    {
        WorldBuilderMain.Instance().ButtonActivate(n, shift);
    }

    public void SetupMode()
    {
        manager.mode.SoftUnload();
        manager.mode = instance;
        manager.KeepActive(null);
    }

    public void SoftUnload()
    {
        manager.mainPointer.GetComponent<Renderer>().enabled = true;
        characterSpawned = false;
        manager.KeepActive(manager.builderObjects);
        manager.SetToolTip("Leaving Play Test Mode");
    }

    public void IUpdate()
    {
        Time.timeScale = control.RSH() ? 0 : 1;
    }

    public void IControlUpdate()
    {
        manager.modeStringEnum = GameManager.ModeString.Play_Test;
        
        if (characterSpawned)
        {
            if (control.PMExit())
            {
                DestroyPlayers();
                ReactivePH();
                characterSpawned = false;
                manager.mainPointer.GetComponent<Renderer>().enabled = true;
                manager.SetToolTip("Player Destroyed");
                return;
            }

            //Handled by the CC;
            //Action = control.PMAction();
            //Jump = control.PMJump();
            //Movement = control.PMAxis();

        }
        else {
            if (control.AR()) {
                SpawnCharacter();
                manager.mainPointer.GetComponent<Renderer>().enabled = false;
                return;
            }

            if (control.AL()) {
                ReturnToBuild();
                return;
            }
        }

    }

    private void SpawnCharacter()
    {
        manager.StartCoroutine(ReSpawnPlayer(-1, manager.mainPointer.transform.position, Quaternion.identity));
    }

    private IEnumerator ReSpawnPlayer(int lastZone, Vector3 pos, Quaternion rot)
    {
        while (manager.loader.LoaderRunning)
        {
            yield return new WaitForSeconds(0f);
        }

        placeholders = GameObject.FindGameObjectsWithTag("PlayerPH");
        ReplacePH();

        ZoneChangeEvent[] allZones = manager.currentModeParent.gameObject.GetComponentsInChildren<ZoneChangeEvent>();
        foreach (ZoneChangeEvent z in allZones)
        {
            int match = -1;
            if (int.TryParse(z.tmp.text, out match))
            {
                if (match == lastZone)
                {
                    pos = z.gameObject.transform.position;
                    rot = z.gameObject.transform.rotation;
                }

            }
        }
        manager.testChar = MonoBehaviour.Instantiate(manager.testCharPrefab, pos, rot, manager.currentModeParent);
        characterSpawned = true;
    }

    private void ReplacePH()
    {
        foreach (GameObject ph in placeholders)
        {
            DioramaObject dio = ph.GetComponent<DioramaObject>();
            BundleItem t = TODV2.GetItemByID(dio.todID);

            string path = t.TopFolder.ToString() + @"\";
            path += t.BundleName + @"\" + (t.FolderPath.Replace(",", @"\")) + @"\";
            path += t.prefabName.Replace(".prefab", "");
            path += " Rigged";
            GameObject next = GameObject.Instantiate(Resources.Load(path, typeof(GameObject)), ph.transform.position, ph.transform.rotation, ph.transform.parent) as GameObject;
            ph.SetActive(false);
        }
    }
    
    public void PlayerZoneChangeEvent(int i)
    {
        Vector3 pos = manager.testChar.transform.position;
        Quaternion rot = manager.testChar.transform.rotation;

        DestroyPlayers();
        int lastZone = WorldBuilderMain.currentZoneId;

        WorldBuilderMain.currentZoneId = i;
        manager.ClearZone();
        WorldBuilderMain.LoadZone(false);


        manager.StartCoroutine(ReSpawnPlayer(lastZone, pos, rot));
    }

    private void ReturnToBuild()
    {
        GameObject[] invisibleObjects = GameObject.FindGameObjectsWithTag("Invisible");
        foreach (GameObject go in invisibleObjects)
        {
            Renderer r = go.GetComponent<Renderer>();
            r.enabled = true;
        }

        SelectObjectMode.Instance().SetupMode();
    }

    private void DestroyPlayers()
    {
        //Destroy players and unhide placeholders 
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            manager.DestroyNow(player);
        }
    }

    private void ReactivePH()
    {
        foreach (GameObject ph in placeholders)
        {
            ph.SetActive(true);
        }
    }
}