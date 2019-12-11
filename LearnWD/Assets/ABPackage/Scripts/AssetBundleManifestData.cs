using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class AssetBundleManifestData
{
    public bool isNO;
    public string fileName;
    public string manifestName;
    public string CRC;
    public string ManifestFileVersion;
    public Dictionary<string,AssetBundleDataInfo> abList = new Dictionary<string, AssetBundleDataInfo>();
    public List<string> manifestList = new List<string>();

    public byte[] abBytes;
    private string value;

    const string cmd_ManifestFileVersion = "ManifestFileVersion";
    const string cmd_CRC = "CRC";
    const string cmd_AssetBundleManifest = "AssetBundleManifest";
    const string cmd_AssetBundleInfos = "AssetBundleInfos";
    const string cmd_Info = "Info";
    const string cmd_Name = "Name";
    private const char splitF = ':';

    public AssetBundleManifestData(string fileName)
    {
        this.fileName = fileName;
        this.manifestName = fileName + Config.suffix_manifest;
        isNO = true;
    }

    public override string ToString()
    {
        return value;
    }

    public AssetBundleManifestData(string value,string fileName)
    {
        this.value = value;
        this.fileName = fileName;
        this.manifestName = fileName + Config.suffix_manifest;
        abList.Clear();
        string[] splits = value.Split('\n');

        for (int i = 0; i < splits.Length; i++)
        {
            string curStr = splits[i];
            curStr = RegexS(curStr);
            string[] v = curStr.Split(splitF);
            if (v.Length > 1)
            {
                switch (v[0])
                {
                    case cmd_ManifestFileVersion:
                        ManifestFileVersion = v[1];
                        break;
                    case cmd_CRC:
                        CRC = v[1];
                        break;
                    case cmd_AssetBundleInfos:
                        while (true) {
                            i++;
                            if (splits.Length > i)
                            {
                                v = splits[i].Split(splitF);
                                if (v[0].Contains(cmd_Info))
                                {
                                    i++;
                                    v = splits[i].Split(splitF);
                                    v[0] = RegexS(v[0]);
                                    v[1] = RegexS(v[1]);
                                    if (v[0].Equals(cmd_Name))
                                    {
                                        manifestList.Add(v[1]+Config.suffix_manifest);
                                        abList.Add(v[1],new AssetBundleDataInfo(v[1]));
                                    }
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                        break;
                }

            }
            
        }
    }
    private string RegexS(string v)
    {
        return Regex.Replace(v, @"\s", "");
    }

}


public class AssetBundleDataInfo {
    public string Name;
    public AssetBundleDataInfo(string Name) {
        this.Name = Name;
    }
}
