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
using System.Linq;
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
        private List<AssetBundle> m_bundleList = new List<AssetBundle>();
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
        //----------------------------------------------------------------------------
        private void LoadUIRoot()
        {
            string m_strPersistenDir;
            string m_strStreamingAssetsDir;
            string strHead;
            switch (Application.platform)
            {
                case RuntimePlatform.WebGLPlayer:
                    {
                        m_strStreamingAssetsDir = "StreamingAssets";
                    }
                    break;
                case RuntimePlatform.IPhonePlayer:
                    {
                        strHead = "file://";
                        m_strStreamingAssetsDir = strHead + Application.streamingAssetsPath;
                        m_strPersistenDir = Application.temporaryCachePath + "/StreamingAssets";
                        Directory.CreateDirectory(m_strPersistenDir);
                    }
                    break;
                case RuntimePlatform.Android:
                    {
                        m_strStreamingAssetsDir = Application.dataPath + "!assets/";
                        m_strPersistenDir = Application.persistentDataPath;
                    }
                    break;
                default:
                    {
                        m_strStreamingAssetsDir = Application.streamingAssetsPath+"/";
                        m_strPersistenDir = Application.persistentDataPath + "/StreamingAssets";
                        Directory.CreateDirectory(m_strPersistenDir);
                    }
                    break;
            }
            var depsPath = m_strStreamingAssetsDir + "uiasset";
            AssetBundle depsAb = AssetBundle.LoadFromFile(depsPath);
            AssetBundleManifest manifest = depsAb.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
            string[] deps = manifest.GetAllDependencies("ui/prefab/ui_root.unity3d");
            for (int i = 0; i < deps.Length; i++)
            {
                var depBundle = AssetBundle.LoadFromFile(m_strStreamingAssetsDir + deps[i]);
                m_bundleList.Add(depBundle);
            }
            var path = m_strStreamingAssetsDir + "ui/prefab/ui_root.unity3d";
            AssetBundle bundle = AssetBundle.LoadFromFile(path);
            GameObject objTemp = bundle.LoadAsset<GameObject>("ui_root");
            GameObject objUI = Object.Instantiate(objTemp);
            if (objUI != null)
            {
                objUI.name = "ui_root";
            }
            bundle.Unload(false);
            depsAb.Unload(false);
            for (int i = 0; i < m_bundleList.Count; i++)
            {
                m_bundleList[i].Unload(false);
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
        public void DownLoadUIFinished()
        {
            if (m_uiDownload != null)
            {
                m_uiDownload.Hide();
                m_uiDownload.OnDestroy();
                m_uiDownload = null;
            }
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
