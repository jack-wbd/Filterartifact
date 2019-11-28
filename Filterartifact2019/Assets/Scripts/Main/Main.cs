//------------------------------------------------------------------------------
using DG.Tweening;
/**
	\file	Main.cs

	Copyright (c) 2019, BoYue. All rights reserved.

	<PRE>

	模块名称：<程序启动类>
	文件名称：Main.cs
	摘    要：<程序启动主类>

	当前版本：1.0
	建立日期：2019/11/8
	作    者：wangbodong
	电子邮件：wangbodong@BoYue.com
	备    注：<其它说明>

	</PRE>
*/
//------------------------------------------------------------------------------
// Using
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
// Class
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	Main.cs
//------------------------------------------------------------------------------
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
namespace Filterartifact
{
    public class Main : MonoBehaviour
    {
        UILogoView m_logoView = null;
        UIDownload m_uiDownload = null;
        GameApp gameApp = null;
        //----------------------------------------------------------------------------
        void Start()
        {
            gameApp = new GameApp();
            DontDestroyOnLoad(gameObject);
            StartCoroutine(GetRootAssetBundle());
        }
        //----------------------------------------------------------------------------
        void Update()
        {
            if (gameApp != null)
            {
                gameApp.Update();
            }
        }
        //----------------------------------------------------------------------------
        IEnumerator GetRootAssetBundle()
        {
            var depsPath = Application.streamingAssetsPath + "/StreamingAssets";
            AssetBundle depsAb = AssetBundle.LoadFromFile(depsPath);
            AssetBundleManifest manifest = depsAb.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
            string[] deps = manifest.GetAllDependencies("ui_root.unity3d");
            for (int i = 0; i < deps.Length; i++)
            {
                AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/" + deps[i]);
            }
            var path = Application.streamingAssetsPath + "/ui_root.unity3d";
            UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle(path);
            yield return www.SendWebRequest();
            AssetBundle ab = DownloadHandlerAssetBundle.GetContent(www);
            GameObject temp = ab.LoadAsset<GameObject>("ui_root");
            GameObject m_root = Instantiate(temp);
            m_root.name = "ui_root";
            m_logoView = new UILogoView(this);
            m_uiDownload = new UIDownload(this);

        }
        //----------------------------------------------------------------------------
        public void ShowUIDownload()
        {
            m_uiDownload.Show();
        }
        //----------------------------------------------------------------------------
        private void OnDestroy()
        {

        }
    }
}
