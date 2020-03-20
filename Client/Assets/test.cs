using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var bundleFont = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/font/gameusefont.unity3d");
        var font = bundleFont.LoadAsset<Font>("gameusefont");
        var bundle = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/ui/prefab/ui_maininterface.unity3d");
        GameObject go = bundle.LoadAsset<GameObject>("ui_maininterface");
        go.transform.parent = transform;
    }

}
