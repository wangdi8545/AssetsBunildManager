using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class DownLoaderMain : MonoBehaviour {



	void Start ()
	{
	    StartCoroutine(Init());
	}
	
	void Update () {
		
	}
    /// <summary>
    /// 服务器存放AB包目录
    /// </summary>
    public string url;

    public ABBuildTarget target;

    public ABSchedule schedule =new ABSchedule();
    [ContextMenu("Init")]
    public IEnumerator Init()
    {
        Dictionary<string, AssetBundleManifestData> webList = null;
        Dictionary<string,AssetBundleManifestData> localList = null;

        AssetBundleManifestData webMainManifest = null;
        AssetBundleManifestData localMainManifest = null;

        Config.Init(target,url);



        //获取本地和服务器的MainManifest清单
        yield return StartCoroutine(DownMainManifestFile(Config.WebFileMainManifest(),Config.MainManifestFileName(), (d) =>
        {
            webMainManifest = d;
        }));
        yield return StartCoroutine(DownMainManifestFile(Config.LocalFileMainManifestUnityWebRequest(), Config.MainManifestFileName(), (d) =>
        {
            localMainManifest = d;
        }));

        //网上无MainManifest清单则退出下载程序
        if (webMainManifest.isNO)
        {
            Debug.LogError("网上无AB包!!!");
            yield break;
        }

        //本地无MainManifest清单则创建
        if (!localMainManifest.isNO)
        {
            if (webMainManifest.CRC == localMainManifest.CRC)
            {
                Debug.Log("本地为最新AB包,无需下载");
                yield break;
            } 
        }




        //根据主MainManifest文件获取其他manifest清单
        yield return StartCoroutine(DownAllManifestFile(Config.WebPath(), webMainManifest.abList.Keys.ToList(), (d) =>
        {
            webList = d;
        }));

        yield return
            StartCoroutine(DownAllManifestFile(Config.LocalFileManifestUnityWebRequest(), localMainManifest.abList.Keys.ToList(),
                (d) =>
                {
                    localList = d;
                }));



        ///检验获取下载项
        //获取需要下载的列表
        List<AssetBundleManifestData> downList = new List<AssetBundleManifestData>();
        

        List<string> keys = webList.Keys.ToList();
        for (int i = 0; i < keys.Count; i++)
        {
            string key = keys[i];
            if (webList[key].isNO)
            {
                Debug.LogError("服务器上文件出现问题："+key);
                continue;
            }

            if (localList.ContainsKey(key))
            {
                if (webList[key].CRC != localList[key].CRC)
                {
                    downList.Add(webList[key]);
                }
                localList.Remove(key);
            }
            else
            {
                downList.Add(webList[key]);
            }
        }

        //根据下载清单 下载并保存清单文件与ab文件
        yield return StartCoroutine(DownUpdataFile(Config.WebPath(), Config.LocalPath(), downList));




        //下载文件
        //获取且删除本地无用文件
        List<AssetBundleManifestData> deleteList = localList.Values.ToList();
        for (int i = 0; i < deleteList.Count; i++)
        {
            if (!deleteList[i].isNO)
            {
                AssetBunildFileIO.DeleteABManifest(Config.LocalPath(),deleteList[i].fileName);
            }
        }



        //覆盖本地主清单文件,表示下载完成
        AssetBunildFileIO.SaveAssetBuildFile(Config.LocalPath(), Config.MainManifestFileName(),
                webMainManifest.ToString());


        Debug.Log("下载完成");
    }


    /// <summary>
    /// 下载加载Main manifest文件
    /// </summary>
    /// <param name="url"></param>
    /// <param name="abmd"></param>
    /// <returns></returns>
    private IEnumerator DownMainManifestFile(string url,string fileName, Action<AssetBundleManifestData> data)
    {
        Debug.Log(url);
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();
        AssetBundleManifestData abmd;
        if (request.isHttpError || request.isNetworkError)
        {
            Debug.LogError(request.downloadHandler.text);
            
            abmd = new AssetBundleManifestData(fileName);
        }
        else
        {
            abmd = new AssetBundleManifestData(request.downloadHandler.text, fileName);
        }
        if (data != null)
        {
            data.Invoke(abmd);
        }
        request.Dispose();
    }

    /// <summary>
    /// 下载.manifest文件
    /// </summary>
    /// <param name="fileList"></param>
    /// <param name="list"></param>
    /// <returns></returns>
    private  IEnumerator DownAllManifestFile(string url,List<string> fileList,Action<Dictionary<string,AssetBundleManifestData>> action )
    {
        Dictionary<string, AssetBundleManifestData> list = new Dictionary<string, AssetBundleManifestData>();
        for (int i = 0; i < fileList.Count; i++)
        {
            string fileName = fileList[i];

            UnityWebRequest request = UnityWebRequest.Get(url+fileName+Config.suffix_manifest);
            yield return request.SendWebRequest();
            if (request.isHttpError || request.isNetworkError)
            {
                Debug.LogError(request.downloadHandler.text);
                list.Add(fileName,new AssetBundleManifestData(fileName));
            }
            else
            {
                AssetBundleManifestData abmd = new AssetBundleManifestData(request.downloadHandler.text, fileName);
                list.Add(fileName,abmd);
            }
            request.Dispose();
        }
        if (action != null)
        {
            action.Invoke(list);
        }
    }

    /// <summary>
    /// 下载保存ab文件
    /// </summary>
    /// <param name="url"></param>
    /// <param name="downFileList"></param>
    /// <returns></returns>
    private IEnumerator DownUpdataFile(string url,string localPath, List<AssetBundleManifestData> downFileList)
    {
        for (int i = 0; i < downFileList.Count; i++)
        {
            AssetBundleManifestData data = downFileList[i];
            UnityWebRequest request = UnityWebRequest.Get(url+data.fileName);
            yield return request.SendWebRequest();
            if (request.isHttpError || request.isNetworkError)
            {
                Debug.LogError(request.downloadHandler.text);
                Debug.LogError("下载AB文件时报错：" + data.fileName);
            }
            else
            {
                AssetBunildFileIO.SaveAssetBuildFile(localPath, data.fileName, request.downloadHandler.data);
                AssetBunildFileIO.SaveAssetBuildFile(localPath, data.manifestName, data.ToString());
            }
            request.Dispose();
        }
    }
}
[System.Serializable]
public class ABSchedule
{
    public enum ScheduleType
    {
        prepare,
        DownLoadVerifyFile,
        DownLoadLocalVerifyFile,
        VerifyFile,
        DownLoadAB,
        End
    }

    public ScheduleType type { get; private set; }
    public float schedule { get; private set; }

    public bool isOK { get; set; }

    public ABSchedule()
    {
        Init();
    }

    public void Init()
    {
        type = ScheduleType.prepare;
        schedule = 0;
    }


    /// <summary>
    ///  进度改变事件
    /// </summary>
    public Action<ScheduleType, float> ScheduleChangeEvent;


    /// <summary>
    /// 设置进度状态
    /// </summary>
    /// <param name="type"></param>
    public void SetScheduleType(ScheduleType type )
    {
        if (this.type != type)
        {
            this.type = type;
            this.schedule = 0f;
            this.isOK = false;
            InovkeEventAction();
        }
    }
    /// <summary>
    /// 设置当前进度
    /// </summary>
    /// <param name="schedule"></param>
    public void SetSchedule(float schedule)
    {
        
        this.schedule = schedule >1?1:schedule;
        InovkeEventAction();
    }

    private void InovkeEventAction()
    {
        if (ScheduleChangeEvent != null)
        {
            ScheduleChangeEvent.Invoke(type,schedule);
        }
    }
}
