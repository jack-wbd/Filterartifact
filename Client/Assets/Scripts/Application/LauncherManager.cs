//------------------------------------------------------------------------------
/**
	\file	LauncherManager.cs

	Copyright (c) 2019, BoYue. All rights reserved.

	<PRE>

	模块名称：<文件所属的模块名称>
	文件名称：LauncherManager.cs
	摘    要：<描述该文件实现的主要功能>

	当前版本：1.0
	建立日期：2019/11/29
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
//	LauncherManager.cs
//------------------------------------------------------------------------------

using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace Filterartifact
{
    //----------------------------------------------------------------------------
    public class LauncherManager
    {
        //----------------------------------------------------------------------------
        UILogoView m_logoView = null;
        UIDownload m_uiDownload = null;
        public static LauncherManager Instance;
        UnityWebRequest m_wwwUIRoot;
        private bool m_bLauncherFinish = true;
        //----------------------------------------------------------------------------
        public LauncherManager()
        {
            Instance = this;
        }
        //----------------------------------------------------------------------------
        public bool Initialization()
        {
            LoadUIRoot();
            return true;
        }
        //----------------------------------------------------------------------------
        private void LoadUIRoot()
        {
            var depsPath = Application.streamingAssetsPath + "/uiasset";
            AssetBundle depsAb = AssetBundle.LoadFromFile(depsPath);
            AssetBundleManifest manifest = depsAb.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
            string[] deps = manifest.GetAllDependencies("prefab/ui_root.unity3d");
            for (int i = 0; i < deps.Length; i++)
            {
                AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/" + deps[i]);
            }
            var path = Application.streamingAssetsPath + "/prefab/ui_root.unity3d";
            if (File.Exists(path))
            {
                AssetBundle bundle = AssetBundle.LoadFromFile(path);
                GameObject objTemp = bundle.LoadAsset<GameObject>("ui_root");
                GameObject objUI = Object.Instantiate(objTemp);
                if (objUI != null)
                {
                    objUI.name = "ui_root";
                }
                bundle.Unload(false);
                depsAb.Unload(false);
            }
            GoToRun();
        }
        //----------------------------------------------------------------------------
        public void Update()
        {
            //UpdateHotUpdate();
        }
        //----------------------------------------------------------------------------
        private void GoToRun()
        {
            m_logoView = new UILogoView();
            m_uiDownload = new UIDownload();
        }
        //----------------------------------------------------------------------------
        public void ShowUIDownload()
        {
            m_uiDownload.Show();
        }
        //----------------------------------------------------------------------------
        private void UpdateHotUpdate()
        {
            m_bLauncherFinish = true;
        }
        //----------------------------------------------------------------------------
        public bool IsLauncherFinish()
        {
            return m_bLauncherFinish;
        }
        //----------------------------------------------------------------------------
        public void SetLauncherFinish(bool bFinish)
        {
            m_bLauncherFinish = bFinish;
        }
        //----------------------------------------------------------------------------
    }
}
