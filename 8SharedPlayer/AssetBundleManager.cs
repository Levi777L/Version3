using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AssetBundleManager
{
    private static AssetBundleCreateRequest polygon_adventure;
    public static AssetBundleCreateRequest Polygon_Adventure
    {
        get
        {
            if (polygon_adventure == null)
                polygon_adventure = AssetBundle.LoadFromFileAsync(Globals.BUNDLEPATH + "polygon_adventure");
            return polygon_adventure;
        }
    }

    private static AssetBundleCreateRequest polygon_pirates;
    public static AssetBundleCreateRequest Polygon_Pirates
    {
        get
        {
            if (polygon_pirates == null)
                polygon_pirates = AssetBundle.LoadFromFileAsync(Globals.BUNDLEPATH + "polygon_pirates");
            return polygon_pirates;
        }
    }

    private static AssetBundleCreateRequest terrain;
    public static AssetBundleCreateRequest Terrain
    {
        get
        {
            if (terrain == null)
                terrain = AssetBundle.LoadFromFileAsync(Globals.BUNDLEPATH + "terrain");
            return terrain;
        }
    }

    private static AssetBundleCreateRequest effects;
    public static AssetBundleCreateRequest Effects
    {
        get
        {
            if (effects == null)
                effects = AssetBundle.LoadFromFileAsync(Globals.BUNDLEPATH + "effects");
            return effects;
        }
    }
    private static AssetBundleCreateRequest chat_bubbles;
    public static AssetBundleCreateRequest Chat_Bubbles
    {
        get
        {
            if (chat_bubbles == null)
                chat_bubbles = AssetBundle.LoadFromFileAsync(Globals.BUNDLEPATH + "chat_bubbles");
            return chat_bubbles;
        }
    }
    private static AssetBundleCreateRequest decals;
    public static AssetBundleCreateRequest Decals
    {
        get
        {
            if (decals == null)
                decals = AssetBundle.LoadFromFileAsync(Globals.BUNDLEPATH + "decals");
            return decals;
        }
    }

    private static AssetBundleCreateRequest characters;
    public static AssetBundleCreateRequest Characters
    {
        get
        {
            if (characters == null)
                characters = AssetBundle.LoadFromFileAsync(Globals.BUNDLEPATH + "characters");
            return characters;
        }
    }

    private static AssetBundleCreateRequest staging;
    public static AssetBundleCreateRequest Staging
    {
        get
        {
            if (staging == null)
                staging = AssetBundle.LoadFromFileAsync(Globals.BUNDLEPATH + "staging");
            return staging;
        }
    }

    private static AssetBundleCreateRequest assorted_low_poly;
    public static AssetBundleCreateRequest Assorted_Low_Poly
    {
        get
        {
            if (assorted_low_poly == null)
                assorted_low_poly = AssetBundle.LoadFromFileAsync(Globals.BUNDLEPATH + "assorted_low_poly");
            return assorted_low_poly;
        }
    }

    private static AssetBundleCreateRequest custom_rooms_1;
    public static AssetBundleCreateRequest Custom_Rooms_1
    {
        get
        {
            if (custom_rooms_1 == null)
                custom_rooms_1 = AssetBundle.LoadFromFileAsync(Globals.BUNDLEPATH + "custom_rooms_1");
            return custom_rooms_1;
        }
    }

    private static AssetBundleCreateRequest custom_ships_1;
    public static AssetBundleCreateRequest Custom_Ships_1
    {
        get
        {
            if (custom_ships_1 == null)
                custom_ships_1 = AssetBundle.LoadFromFileAsync(Globals.BUNDLEPATH + "custom_ships_1");
            return custom_ships_1;
        }

    }

    private static AssetBundleCreateRequest hq_interiors;
    public static AssetBundleCreateRequest Hq_Interiors
    {
        get
        {
            if (hq_interiors == null)
                hq_interiors = AssetBundle.LoadFromFileAsync(Globals.BUNDLEPATH + "hq_interiors");
            return hq_interiors;
        }
    }

    private static AssetBundleCreateRequest hq_dungeons1;
    public static AssetBundleCreateRequest Hq_Dungeons1
    {
        get
        {
            if (hq_dungeons1 == null)
                hq_dungeons1 = AssetBundle.LoadFromFileAsync(Globals.BUNDLEPATH + "hq_dungeons1");
            return hq_dungeons1;
        }
    }

    private static AssetBundleCreateRequest hq_space;
    public static AssetBundleCreateRequest Hq_Space
    {
        get
        {
            if (hq_space == null)
                hq_space = AssetBundle.LoadFromFileAsync(Globals.BUNDLEPATH + "hq_space");
            return hq_space;
        }
    }

    private static AssetBundleCreateRequest low_poly_town;
    public static AssetBundleCreateRequest Low_Poly_Town
    {
        get
        {
            if (low_poly_town == null)
                low_poly_town = AssetBundle.LoadFromFileAsync(Globals.BUNDLEPATH + "low_poly_town");
            return low_poly_town;
        }
    }

    private static AssetBundleCreateRequest low_poly_apocalypse;
    public static AssetBundleCreateRequest Low_Poly_Apocalypse
    {
        get
        {
            if (low_poly_apocalypse == null)
                low_poly_apocalypse = AssetBundle.LoadFromFileAsync(Globals.BUNDLEPATH + "low_poly_apocalypse");
            return low_poly_apocalypse;
        }
    }

    private static AssetBundleCreateRequest polygon_fantasy;
    public static AssetBundleCreateRequest Polygon_Fantasy
    {
        get
        {
            if (polygon_fantasy == null)
                polygon_fantasy = AssetBundle.LoadFromFileAsync(Globals.BUNDLEPATH + "polygon_fantasy");
            return polygon_fantasy;
        }
    }

    private static AssetBundleCreateRequest nature;
    public static AssetBundleCreateRequest Nature
    {
        get
        {
            if (nature == null)
                nature = AssetBundle.LoadFromFileAsync(Globals.BUNDLEPATH + "nature");
            return nature;
        }
    }

    private static AssetBundleCreateRequest voxels;
    public static AssetBundleCreateRequest Voxels
    {
        get
        {
            if (voxels == null)
                voxels = AssetBundle.LoadFromFileAsync(Globals.BUNDLEPATH + "voxels");
            return voxels;
        }
    }

    private static AssetBundleCreateRequest cartoon_apocalypse;
    public static AssetBundleCreateRequest Cartoon_Apocalypse
    {
        get
        {
            if (cartoon_apocalypse == null)
                cartoon_apocalypse = AssetBundle.LoadFromFileAsync(Globals.BUNDLEPATH + "cartoon_apocalypse");
            return cartoon_apocalypse;
        }
    }

    private static AssetBundleCreateRequest pigeons_war;
    public static AssetBundleCreateRequest Pigeons_War
    {
        get
        {
            if (pigeons_war == null)
                pigeons_war = AssetBundle.LoadFromFileAsync(Globals.BUNDLEPATH + "pigeons_war");
            return pigeons_war;
        }
    }

    public static AssetBundleCreateRequest GetAssetBundle(BundleItem.BundleNames bn)
    {
        
        switch (bn)
        {
            case BundleItem.BundleNames.Staging:
                return Staging;
            case BundleItem.BundleNames.Assorted_Low_Poly:
                return Assorted_Low_Poly;
            case BundleItem.BundleNames.Custom_Rooms_1:
                return Custom_Rooms_1;
            case BundleItem.BundleNames.Custom_Ships_1:
                return Custom_Ships_1;
            case BundleItem.BundleNames.HQ_Interiors:
                return Hq_Interiors;
            case BundleItem.BundleNames.HQ_Space:
                return Hq_Space;
            case BundleItem.BundleNames.Low_Poly_Town:
                return Low_Poly_Town;
            case BundleItem.BundleNames.Nature:
                return Nature;
            case BundleItem.BundleNames.Voxels:
                return Voxels;
            case BundleItem.BundleNames.Cartoon_Apocalypse:
                return Cartoon_Apocalypse;
            case BundleItem.BundleNames.Polygon_Fantasy:
                return Polygon_Fantasy;
            case BundleItem.BundleNames.Low_Poly_Apocalypse:
                return Low_Poly_Apocalypse;
            case BundleItem.BundleNames.HQ_Dungeons1:
                return Hq_Dungeons1;
            case BundleItem.BundleNames.Characters:
                return Characters;
            case BundleItem.BundleNames.Pigeons_War:
                return Pigeons_War;
            case BundleItem.BundleNames.Effects:
                return Effects;
            case BundleItem.BundleNames.Decals:
                return Decals;
            case BundleItem.BundleNames.Chat_Bubbles:
                return Chat_Bubbles;
            case BundleItem.BundleNames.Polygon_Pirates:
                return Polygon_Pirates;
            case BundleItem.BundleNames.Terrain:
                return Terrain;
            default:
                return Polygon_Adventure;
        }
    }

    public static void Prewarm() {
        AssetBundleCreateRequest a = Custom_Rooms_1;
        try
        {
            a = Chat_Bubbles;
        }
        catch
        {

        }
        try
        {
            a = Effects;
        }
        catch
        {

        }
        try
        {
            a = Decals;
        }
        catch
        {

        }

        try
        {
            a = Custom_Ships_1;
        }
        catch {

        }

        try
        {
            a = Hq_Interiors;
        }
        catch
        {

        }

        try
        {
            a = Hq_Space;
        }
        catch
        {

        }

        try
        {
            a = Cartoon_Apocalypse;

        }
        catch
        {

        }

        try
        {
            a = Polygon_Fantasy;
        }
        catch
        {

        }

        try
        {
            a = Polygon_Pirates;
        }
        catch
        {

        }

        try
        {
            a = Low_Poly_Apocalypse;
        }
        catch
        {

        }

        try
        {
            a = Hq_Dungeons1;
        }
        catch { }


        try
        {
            a = Characters;
        }
        catch { }


        try
        {
            a = Pigeons_War;
        }
        catch { }

        try
        {
            a = Terrain;
        }
        catch { }
    }

}
