using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AssetBunildFileIO
{
    public static bool SaveAssetBuildFile(string path,string name,Byte[] ab)
    {
        try
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            File.WriteAllBytes(path+"/"+name,ab);
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError(e.ToString());
            return false;
            throw;
        }
    }
    public static bool SaveAssetBuildFile(string path, string name, string ab)
    {
        try
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            File.WriteAllText(path + "/" + name, ab);
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError(e.ToString());
            return false;
            throw;
        }
    }



    public static AssetBundleManifestData ReadManifestFile(string pathName)
    {
        try
        {
            if (File.Exists(pathName))
            {
                string v = File.ReadAllText(pathName);
                AssetBundleManifestData manifestData = new AssetBundleManifestData(v);
                return manifestData;
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e.ToString());
            throw;
        }
        return null;
    }

    /// <summary>
    /// 删除指定ab包文件
    /// </summary>
    /// <param name="path"></param>
    /// <param name="name"></param>
    public static void DeleteABManifest(string path, string name)
    {
        try
        {
            string deleteFile = path + name + Config.suffix_manifest;
            if (File.Exists(deleteFile))
            {
                File.Delete(deleteFile);
                
            }
            deleteFile = path + name;
            if (File.Exists(deleteFile))
            {
                File.Delete(deleteFile);
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e.ToString());
            throw;
        }
    }



}
