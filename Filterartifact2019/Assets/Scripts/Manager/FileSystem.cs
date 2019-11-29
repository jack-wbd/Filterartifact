//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.IO;
/**
\file	FileSystem.cs

Copyright (c) 2019, BoYue. All rights reserved.

<PRE>

模块名称：<系统类>
文件名称：FileSystem.cs
摘    要：<描述该文件实现的主要功能>

当前版本：1.0
建立日期：2019/11/22
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
//	FileSystem.cs
//------------------------------------------------------------------------------
using UnityEngine;
using UnityEngine.Networking;

namespace Filterartifact
{
    public class FileSystem
    {
        public enum FileSystemState
        {
            File_Init,
            File_LoadingGamedata,
            File_LoadedGamedata,
            File_DeCompress,
            File_OK
        }
        private static FileSystem m_tSingleton = null;
        private static ResourceListData m_ResourceList = null;
        private int m_MaxDepsCount;
        private int m_nNeedLoadDepsCount;
        private static FileSystemState m_state = FileSystemState.File_Init;
        private Dictionary<string, CLoadData> m_dicLoad = null;
        private Queue<sNeedLoadData> m_queueNeedLoad = null;
        private bool m_bHaveNeedLoad = false;
        public static AssetBundle m_abGameData = null;
        private ConfigData m_ConfigData;
        private int m_devLoadGameDataLeftCount = -1;
        private AssetsManager m_assetManager = null;
        //----------------------------------------------------------------------------
        public bool InitFileSystem(string strPath = null)
        {
            m_ResourceList = new ResourceListData();
            m_ResourceList.Initialize();
            m_dicLoad = new Dictionary<string, CLoadData>();
            m_queueNeedLoad = new Queue<sNeedLoadData>();
            m_ConfigData = new ConfigData();
            string strData = null;
            UnityEngine.Object objTemp = Resources.Load("config");
            if (ReferenceEquals(objTemp, null))
            {
                return false;
            }
            strData = objTemp.ToString();
            Resources.UnloadAsset(objTemp);
            if (Application.isPlaying)
            {
                m_ConfigData.InitConfigFile(strData);
            }
            else
            {
                m_ConfigData.m_strDataDir = strPath;
            }
            if (RuntimePlatform.IPhonePlayer == Application.platform)
            {
                InitPhone();
            }
            else if (RuntimePlatform.Android == Application.platform)
            {
                InitAndroid();
            }
            else if (RuntimePlatform.WindowsPlayer == Application.platform || RuntimePlatform.WebGLPlayer == Application.platform)
            {
                InitWeb();
            }
            else
            {
                InitDev();
            }
            return true;
        }
        //----------------------------------------------------------------------------
        public void InitPhone()
        {

        }
        //----------------------------------------------------------------------------
        public void InitAndroid()
        {

        }
        //----------------------------------------------------------------------------
        public void InitWeb()
        {

        }
        //----------------------------------------------------------------------------
        public void InitDev()
        {
            Messenger.Broadcast(DgMsgID.DgMsg_InitStatChange, UpdateState.Load_Gamedata);
            m_devLoadGameDataLeftCount = 1;
        }
        //----------------------------------------------------------------------------
        public static FileSystem CreateInstance()
        {
            if (m_tSingleton != null)
            {
                m_tSingleton = null;
            }
            m_tSingleton = new FileSystem();
            return m_tSingleton;
        }
        //----------------------------------------------------------------------------
        public static FileSystem Instance()
        {
            return m_tSingleton;
        }
        //----------------------------------------------------------------------------
        public static void ReleaseInstance()
        {
            if (m_tSingleton != null)
            {
                m_tSingleton = null;
            }
        }
        //----------------------------------------------------------------------------
        public virtual void Update()
        {
            if (RuntimePlatform.IPhonePlayer == Application.platform || RuntimePlatform.Android == Application.platform)
            {
                UpdatePhone();
            }
            else if (RuntimePlatform.WindowsPlayer == Application.platform)
            {
                UpdateWeb();
            }
            else
            {
                UpdateDev();
            }

            UpdateCheckFileOK();

        }
        //----------------------------------------------------------------------------
        private void UpdatePhone()
        {

        }
        //----------------------------------------------------------------------------
        private void UpdateWeb()
        {

        }
        //----------------------------------------------------------------------------
        private void UpdateDev()
        {
            if (m_devLoadGameDataLeftCount > 0)
            {
                m_devLoadGameDataLeftCount--;
            }
            else if (m_devLoadGameDataLeftCount == 0)
            {
                m_ResourceList.LoadResourceListFileDev();
                m_devLoadGameDataLeftCount = -1;
            }
        }
        //----------------------------------------------------------------------------
        private void UpdateCheckFileOK()
        {
            GoState(FileSystemState.File_OK);
            LoadDepRes();
        }
        //----------------------------------------------------------------------------
        public static void GoState(FileSystemState state)
        {
            m_state = state;
        }
        //----------------------------------------------------------------------------
        private void LoadDepRes()
        {
            List<string> depsList = GetDepsResList();
            m_MaxDepsCount = depsList.Count;
            m_nNeedLoadDepsCount = 0;
            for (int i = 0; i < m_MaxDepsCount; i++)
            {
                AssetsManager.LoadAssetRes<string, UnityEngine.Object>(depsList[i], LoadFinishedCallBack);
            }
        }
        //----------------------------------------------------------------------------
        public void LoadFinishedCallBack(string strAssetID, UnityEngine.Object obj)
        {
            ++m_nNeedLoadDepsCount;
            if (m_nNeedLoadDepsCount >= m_MaxDepsCount)
            {
                GoState(FileSystemState.File_DeCompress);

            }
        }
        //----------------------------------------------------------------------------
        private List<string> GetDepsResList()
        {
            return m_ResourceList.GetDepsResourceList();
        }
        //----------------------------------------------------------------------------
        public bool StartLoad<T, U>(string strAssetID, Callback<T, U> result)
        {
            if (m_state != FileSystemState.File_OK)
            {

                Debug.LogError("StartLoad m_state " + m_state.ToString() + " " + strAssetID + "! FileSystemState.File_OK");
                return false;
            }
            if (result == null)
            {
                Debug.LogError(string.Format("{0} is not hava callback!", strAssetID));
                return false;
            }

            sAssetInfo info = sAssetInfo.zero;
            m_ResourceList.GetAssetBundleInfo(strAssetID, ref info);
            if (string.IsNullOrEmpty(info.strFile))
            {
                Debug.LogError(strAssetID + "  Is not exist in anywhere");
                return false;
            }

            object obj = AssetsManager.Instance().GetAssetObjByID(strAssetID);
            if (obj != null)
            {
                Callback<string, object> callback = result as Callback<string, object>;
                callback(strAssetID, obj);
                return true;
            }
            CLoadData data = null;
            if (m_dicLoad.TryGetValue(strAssetID, out data))
            {
                data.loadBack = (Callback<T, U>)data.loadBack + result;
            }
            else
            {
                sNeedLoadData needLoad;
                needLoad.strAssetID = strAssetID;
                needLoad.callback = result as Callback<string, UnityEngine.Object>;
                m_queueNeedLoad.Enqueue(needLoad);
                m_bHaveNeedLoad = true;
            }
            return true;
        }
        //----------------------------------------------------------------------------
        public string LoadXml(string strFile)
        {
            string strFileName = strFile;
            if (!strFile.EndsWith(".xml"))
            {
                strFileName = strFile + ".xml";
            }
            return LoadFile(strFileName);
        }
        //----------------------------------------------------------------------------
        public string LoadFile(string strFile)
        {
            if (m_abGameData != null)
            {
                string strFileName = GameUtil.GetAssetName(m_abGameData, strFile);
                Debug.LogFormat("strFile:{0} Load strFileName:{1}", strFile, strFileName);
                if (!string.IsNullOrEmpty(strFileName))
                {
                    UnityEngine.Object objTemp = m_abGameData.LoadAsset(strFileName);
                    if (ReferenceEquals(objTemp, null))
                    {
                        Debug.LogError("LoadXml not Exist" + strFileName);
                        return null;
                    }

                    string strData = objTemp.ToString();
                    return strData;
                }
            }
            else
            {
                Debug.Log("m_abGameData is null");
                if (m_ConfigData.m_strDataDir != "")
                {
                    if (File.Exists(m_ConfigData.m_strDataDir + strFile))
                    {
                        return File.ReadAllText(m_ConfigData.m_strDataDir + strFile);
                    }
                    else
                    {
                        Debug.LogError(m_ConfigData.m_strDataDir + strFile + " is not exist!");
                    }
                }
            }
            return null;
        }
        //----------------------------------------------------------------------------
        public AssetsManager assetsManager
        {
            get
            {
                if (m_assetManager == null)
                {
                    m_assetManager =WorldManager.Instance().GetLayer<AssetLayer>().
                }
            }
        }
        //----------------------------------------------------------------------------
    }
    //需要下载的数据缓存
    public struct sNeedLoadData
    {
        public string strAssetID;
        public Callback<string, UnityEngine.Object> callback;
    }
    public class CLoadData
    {
        public UnityWebRequest www = null;
        public bool bAllCache = false;
        public bool bLoadedOK = false;
        public Delegate loadBack;
        public string strAssetID = string.Empty;
        public string strAssetName = string.Empty;
        public EAssetType eType;
        private AssetBundleRequest m_syncReq = null;
        private AssetBundle bundle = null;
        private bool m_bLoadWWW = false;
        private bool m_bLoadAsset = false;
        private bool m_bLoadAllOK = false;

        public Type GetType(EAssetType eType)
        {
            switch (eType)
            {
                case EAssetType.eGameObject:
                    return typeof(UnityEngine.Object);
                case EAssetType.eAudio:
                    return typeof(AudioClip);
                case EAssetType.eTexture:
                    return typeof(Texture);
                default:
                    return typeof(UnityEngine.Object);
            }
        }
        public bool LoadAssetOK()
        {
            if (m_syncReq == null && www != null)
            {
                m_syncReq = DownloadHandlerAssetBundle.GetContent(www).LoadAssetAsync(strAssetID, GetType(eType));
            }
            if (m_syncReq != null)
            {
                if (m_syncReq.isDone)
                {
                    return true;
                }
            }
            return false;
        }

        public bool LoadOK()
        {
            if (m_bLoadWWW)
            {
                if (www.isDone)
                {
                    m_bLoadWWW = false;
                    bundle = DownloadHandlerAssetBundle.GetContent(www);
                    m_bLoadAllOK = true;
                }
            }

            return m_bLoadAllOK;

        }
        public UnityEngine.Object GetAsset()
        {
            if (bundle != null)
            {
                if (bundle == null)
                {
                    return bundle;
                }
                else
                {
                    return bundle;
                }
            }
            else
                return null;
        }
        public void Disponse()
        {
            if (www != null)
            {
                var assetbundle = DownloadHandlerAssetBundle.GetContent(www);
                if (assetbundle != null && !strAssetID.Equals("REDP_texturedeps"))
                {
                    assetbundle.Unload(false);
                }
                www.Dispose();
                www = null;
                bundle = null;
                m_syncReq = null;
            }
        }
    }
}

