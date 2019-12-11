using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class LoadAssetBunild : MonoBehaviour
{

    public string url;
    public string outPath;

	void Start ()
	{
        // StartCoroutine(RequestAssetBundlemanifest(url));

	    outPath = "G:/ABTest/";
	    StartCoroutine(DownAB(url, outPath));
	}
	
	void Update () {
		
	}



    private IEnumerator DownAB(string url, string path)
    {
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.isHttpError || request.isNetworkError)
        {
            Debug.Log(request.downloadHandler.text);
        }
        else
        {
            if (!File.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            Debug.Log(request.downloadHandler.text);
            File.WriteAllBytes(path+"/AAA",request.downloadHandler.data);
            string str = File.ReadAllText(path + "/AAA");
            AssetBundleManifestData abmd = new AssetBundleManifestData(str);
        }



    }

    private IEnumerator InstantiateObject(string url)
    {
        Debug.Log("BBB");
        UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(url);
        yield return request.SendWebRequest();
        AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(request);

        AssetBundleManifest abm = bundle.LoadAsset<AssetBundleManifest>("StandaloneWindows");

        Debug.Log(abm.ToString());
        GameObject a = bundle.LoadAsset<GameObject>("A");
        Instantiate(a).transform.position = Vector3.zero;

        Debug.Log("AAA");
    }

    private IEnumerator RequestAssetBundlemanifest(string url)
    {
        UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(url);
        yield return request.SendWebRequest();
        AssetBundle ab = DownloadHandlerAssetBundle.GetContent(request);



        AssetBundleManifest abm = ab.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        Debug.Log(abm);

        if (abm)
        {
            DebugLogString(abm.GetAllAssetBundles());
            Debug.Log("+________");
            DebugLogString(abm.GetAllAssetBundlesWithVariant());
        }

        Object[] objs = ab.LoadAllAssets();
        for (int i = 0; i < objs.Count(); i++)
        {
            Debug.Log(objs[i].GetType());
        }
    }

    private void DebugLogString(string[] value)
    {
        for (int i = 0; i < value.Count(); i++)
        {
            Debug.Log(value[i]);
        }
    }


    private IEnumerator RequestLocatLoadFromMemoryAsync(string url)
    {

        AssetBundleCreateRequest createRequest = AssetBundle.LoadFromMemoryAsync(File.ReadAllBytes(url));
        yield return createRequest;
        AssetBundle ab = createRequest.assetBundle;
        Debug.Log(ab.isStreamedSceneAssetBundle);

        SceneManager.LoadScene("Main 1");
    }



}
