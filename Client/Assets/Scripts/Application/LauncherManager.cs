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

using System.Collections.Generic;
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
        private static ConfigData m_ConfigData = null;
        //----------------------------------------------------------------------------
        public LauncherManager()
        {
            Instance = this;
        }
        //----------------------------------------------------------------------------
        public bool Initialization()
        {
            ParseConfig();
            LoadUIRoot();
            return true;
        }
        private List<AssetBundle> bundleList = new List<AssetBundle>();
        //----------------------------------------------------------------------------
        private void LoadUIRoot()
        {
            bundleList.Clear();
            var depsPath = Application.streamingAssetsPath + "/uiasset";
            AssetBundle depsAb = AssetBundle.LoadFromFile(depsPath);
            AssetBundleManifest manifest = depsAb.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
            string[] deps = manifest.GetAllDependencies("ui/prefab/ui_root.unity3d");
            for (int i = 0; i < deps.Length; i++)
            {
                var depBundle = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/" + deps[i]);
                bundleList.Add(depBundle);
            }
            var path = Application.streamingAssetsPath + "/ui/prefab/ui_root.unity3d";
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
                bundle = null;
                depsAb = null;
            }

            for (int i = 0; i < bundleList.Count; i++)
            {
                bundleList[i].Unload(false);
                bundleList[i] = null;
            }

            Resources.UnloadUnusedAssets();
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
        public ConfigData GetConfigData()
        {
            return m_ConfigData;
        }
        //----------------------------------------------------------------------------
        public void ParseConfig()
        {
            if (m_ConfigData == null)
            {
                m_ConfigData = new ConfigData();
            }
            string strData = null;
            Object objTemp = Resources.Load("config");
            if (!ReferenceEquals(objTemp, null))
            {
                strData = objTemp.ToString();
                Resources.UnloadAsset(objTemp);
            }
            m_ConfigData.InitConfigFile(strData);
        }
        //----------------------------------------------------------------------------
    }
}
