using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

public enum ABBuildTarget
{
    StandaloneWindows,
}
public static class Config
{
    public const string suffix_manifest = ".manifest";
    
    private static ABBuildTarget target;
    private static string url;

    /// <summary>
    /// URL为服务器AB包与manifest文件存放路径
    /// </summary>
    /// <param name="target"></param>
    /// <param name="url"></param>
    public static void Init(ABBuildTarget target,string url)
    {
        Config.target = target;
        Config.url = url+"/";
    }



    /// <summary>
    /// 本地AB包路径
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    public static string LocalPath()
    {
        switch (Config.target)
        {
            case ABBuildTarget.StandaloneWindows:
                return Application.streamingAssetsPath + "/" + Config.target.ToString()+"/";
        }
        return "";
    }
    /// <summary>
    /// 前加file://本地ab文件存放目录
    /// </summary>
    /// <returns></returns>
    public static string LocalFileManifestUnityWebRequest()
    {
        return "file://" + LocalPath();
    }
    

    /// <summary>
    /// 前加file://本地平台清单存放路径名称
    /// </summary>
    /// <returns></returns>
    public static string LocalFileMainManifestUnityWebRequest()
    {

        return "file://" + LocalPath() + MainManifestFileName();
    }
    /// <summary>
    /// 平台清单名称
    /// </summary>
    /// <returns></returns>
    public static string MainManifestFileName()
    {
        return Config.target.ToString()+suffix_manifest;
    }


    /// <summary>
    /// 网络AB包存放地址
    /// </summary>
    /// <returns></returns>
    public static string WebPath()
    {
        switch (Config.target)
        {
            case ABBuildTarget.StandaloneWindows:
                return url + Config.target.ToString() + "/";

        }
        return "";
    }

    /// <summary>
    /// 服务器 平台清单文件
    /// </summary>
    /// <returns></returns>
    public static string WebFileMainManifest()
    {
        switch (Config.target)
        {
            case ABBuildTarget.StandaloneWindows:
                return WebPath() + MainManifestFileName();
        }
        return "";
    }
    /// <summary>
    /// 获取网上manifest文件路径 
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public static string GetWebFileManifest(string fileName)
    {
        return WebPath() + fileName;
    }





}
