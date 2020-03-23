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
        //----------------------------------------------------------------------------
        public static bool bNotInLoading = false;
        //----------------------------------------------------------------------------
        public enum FileSystemState
        {
            File_Init,
            File_LoadingGamedata,
            File_LoadedGamedata,
            File_ParseGameData,
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
        private bool m_bBundleDataLoaded = false;
        private int m_nNeedParseDataCount = 0;
        private int m_nCurPaseDataCount = 0;
        private int m_nNeedLoadMaxPerFrame = 120; //每帧加载最大负载
        private Dictionary<string, List<string>> m_bundleDependenciseDict;
        private Dictionary<string, AssetBundle> m_assetBundleDict = null;
        public static string m_strStreamAssetsDir = null;
        public static string m_strPersistenDir = null;
        private List<string> m_audioAssetList = null;
        private TextAsset m_assetText = null;
        private Queue<DataBase> m_queNeedParse = null;
        private bool m_bSerializeDataLoaded = false;
        private int m_nCurCountPerFrame = 20;
        public static readonly int MaxCountPerFramInCity = 20;
        //----------------------------------------------------------------------------
        public bool InitFileSystem(string strPath = null)
        {
            m_bundleDependenciseDict = new Dictionary<string, List<string>>();
            m_assetBundleDict = new Dictionary<string, AssetBundle>();
            m_ResourceList = new ResourceListData();
            m_ResourceList.Initialize();
            m_dicLoad = new Dictionary<string, CLoadData>();
            m_queueNeedLoad = new Queue<sNeedLoadData>();
            m_ConfigData = new ConfigData();
            m_audioAssetList = new List<string>();
            InitAllAssetManifest();
            if (Application.isPlaying)
            {
                m_ConfigData = LauncherManager.Instance.GetConfigData();
            }
            else
            {
                m_ConfigData = new ConfigData();
                m_ConfigData.m_strDataDir = strPath;
            }

            CheckPlatform();
            UnityEngine.Object objTemp = Resources.Load("config");
            if (ReferenceEquals(objTemp, null))
            {
                return false;
            }

            var strData = objTemp.ToString();
            Resources.UnloadAsset(objTemp);

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
        public void CheckPlatform()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    m_strStreamAssetsDir = Application.streamingAssetsPath + "/";
                    m_strPersistenDir = Application.persistentDataPath + "/";
                    break;
                case RuntimePlatform.IPhonePlayer:
                    m_strStreamAssetsDir = Application.streamingAssetsPath + "/";
                    m_strPersistenDir = Application.persistentDataPath + "/StreamingAssets/";
                    break;
                case RuntimePlatform.WebGLPlayer:
                    m_strStreamAssetsDir = "StreamingAssets/";
                    break;
                default:
                    m_strStreamAssetsDir = Application.streamingAssetsPath + "/";
                    m_strPersistenDir = Application.persistentDataPath + "/StreamingAssets/";
                    break;
            }
        }
        //----------------------------------------------------------------------------
        public Dictionary<string, List<string>> GetBundleDepsDict()
        {
            return m_bundleDependenciseDict;
        }
        //----------------------------------------------------------------------------
        void InitAllAssetManifest()
        {
            InitAssetManifest("uiasset");
        }
        //----------------------------------------------------------------------------
        void InitAssetManifest(string manifestName)
        {
            AssetBundle bundleManifest = null;
            var path = GameUtil.GetDataPath(manifestName);
            bundleManifest = AssetBundle.LoadFromFile(path);
            if (bundleManifest != null)
            {
                AssetBundleManifest manifest = bundleManifest.LoadAsset("AssetBundleManifest") as AssetBundleManifest;
                string[] assetBundles = manifest.GetAllAssetBundles();
                for (int i = 0; i < assetBundles.Length; i++)
                {
                    string[] tempDependencise = manifest.GetAllDependencies(assetBundles[i]);
                    List<string> dependeciseList = new List<string>(tempDependencise);
                    //if (!assetBundles[i].Contains("ui_root") && dependeciseList != null && manifestName == "uiasset")
                    //{
                    //    if (dependeciseList.Count > 0)
                    //    {
                    //        for (int j = dependeciseList.Count - 1; j >= 0; j--)
                    //        {
                    //            if (dependeciseList[j].Contains("gameusefont"))
                    //            {
                    //                dependeciseList.Remove(dependeciseList[j]);
                    //            }
                    //        }
                    //    }
                    //}
                    if (!m_bundleDependenciseDict.ContainsKey(assetBundles[i]))
                    {
                        m_bundleDependenciseDict.Add(assetBundles[i], dependeciseList);
                    }
                    else
                    {
                        Debug.LogError("have same key: " + assetBundles[i]);
                    }

                }
                bundleManifest.Unload(false);
                bundleManifest = null;
            }
        }
        //----------------------------------------------------------------------------
        public void DoUnloadAssetBundle()
        {
            Dictionary<string, AssetBundle>.Enumerator enumerator = m_assetBundleDict.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (enumerator.Current.Value == null)
                {
                    Debug.LogError("***********************资源已被释放: " + enumerator.Current.Key);
                }
                else
                {
                    if (!m_audioAssetList.Contains(enumerator.Current.Key))
                    {
                        enumerator.Current.Value.Unload(false);
                    }
                }
            }
            m_audioAssetList.Clear();
            m_assetBundleDict.Clear();
        }
        //----------------------------------------------------------------------------
        public ResourceListData GetResData()
        {
            return m_ResourceList;
        }
        //----------------------------------------------------------------------------
        public byte[] GetSerializeByte()
        {
            if (m_assetText != null)
            {
                return m_assetText.bytes;
            }
            return null;
        }
        //----------------------------------------------------------------------------
        public void ClearSerializeByte()
        {
            UnityEngine.Object.DestroyImmediate(m_assetText, true);
            m_assetText = null;
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
            InitSerializeData();
            if (m_ConfigData.m_strDataDir == "")
            {

                if (m_ConfigData.m_strResDir == "")
                {
                    StartInitData(m_strStreamAssetsDir + "gamedata/gamedata.unity3d");
                }
                else
                {
                    StartInitData("file:///" + m_strStreamAssetsDir + "gamedata/gamedata.unity3d");
                }

                GoState(FileSystemState.File_LoadingGamedata);

            }
            else
            {
                Messenger.Broadcast(DgMsgID.DgMsg_UpdateGameProgress, 1.0f);
                Messenger.Broadcast(DgMsgID.DgMsg_InitStatChange, UpdateState.Load_Gamedata);
                m_devLoadGameDataLeftCount = 1;
            }


        }
        //----------------------------------------------------------------------------
        public bool StartInitData(string strBundlePath)
        {
            Messenger.Broadcast(DgMsgID.DgMsg_InitStatChange, UpdateState.Load_Gamedata);
            m_abGameData = AssetBundle.LoadFromFile(strBundlePath);
            m_ResourceList.LoadResourceListFileFromBundle();
            m_bBundleDataLoaded = true;
            return true;
        }
        //----------------------------------------------------------------------------
        private void InitSerializeData()
        {
            AssetBundle bundleData = null;
            string strBineryFilePath = m_strPersistenDir + "gamedata/binery.unity3d";
            if (File.Exists(strBineryFilePath))
            {
                bundleData = AssetBundle.LoadFromFile(strBineryFilePath);
            }
            else
            {
                strBineryFilePath = m_strStreamAssetsDir + "gamedata/binery.unity3d";
                if (File.Exists(strBineryFilePath))
                {
                    bundleData = AssetBundle.LoadFromFile(strBineryFilePath);

                }
            }

            if (bundleData != null)
            {
                m_assetText = bundleData.LoadAsset<TextAsset>("binery");
                bundleData.Unload(false);
                bundleData = null;
            }

            m_bSerializeDataLoaded = true;
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
            switch (m_state)
            {
                case FileSystemState.File_Init:
                    break;
                case FileSystemState.File_LoadingGamedata:
                    break;
                case FileSystemState.File_LoadedGamedata:
                    break;
                case FileSystemState.File_ParseGameData:
                    UpdateParseGameData();
                    break;
                case FileSystemState.File_DeCompress:
                    break;
                case FileSystemState.File_OK:
                    break;
                default:
                    break;
            }
        }
        //----------------------------------------------------------------------------
        private DataBase m_dataTemp = null;
        //----------------------------------------------------------------------------
        private void UpdateParseGameData()
        {
            if (m_nCurPaseDataCount >= m_nNeedParseDataCount)
            {
                WorldManager.Instance().CreateLayer();
                GoState(FileSystemState.File_OK);
                LoadDepRes();
                return;
            }
            m_dataTemp = m_queNeedParse.Dequeue();
            m_dataTemp.Initialize();
            ++m_nCurPaseDataCount;
            Messenger.Broadcast(DgMsgID.DgMsg_UpdateGameProgress, (float)m_nCurPaseDataCount / m_nNeedParseDataCount);
        }
        //----------------------------------------------------------------------------
        private void UpdateLoadRes()
        {
            if (m_state != FileSystemState.File_OK)
            {
                return;
            }
            CheckNeedLoadData();
        }
        //----------------------------------------------------------------------------
        private void CheckNeedLoadData()
        {
            if (m_bHaveNeedLoad)
            {
                int nCount = m_queueNeedLoad.Count;
                if (nCount == 0)
                {
                    m_bHaveNeedLoad = false;
                }
                nCount = nCount > m_nNeedLoadMaxPerFrame ? m_nNeedLoadMaxPerFrame : nCount;
                for (int i = 0; i < nCount; i++)
                {
                    StartLoad(m_queueNeedLoad.Dequeue());
                }
            }
        }
        //----------------------------------------------------------------------------
        private void StartLoad(sNeedLoadData needLoad)
        {
            string strAssetID = needLoad.strAssetID;
            UnityEngine.Object obj = assetsManager.GetAssetObjByID(strAssetID);
            if (obj != null)
            {
                Callback<string, UnityEngine.Object> callback = needLoad.callback as Callback<string, UnityEngine.Object>;
                callback(strAssetID, obj);
                return;
            }

            sAssetInfo info = sAssetInfo.zero;
            m_ResourceList.GetAssetBundleInfo(strAssetID, ref info);
            string strBaseDir = GetResDirByPath(info.strFile);
            string path = info.strFile;

            if (m_bundleDependenciseDict.ContainsKey(path))
            {
                List<string> strDependecise = m_bundleDependenciseDict[path];
                for (int i = 0; i < strDependecise.Count; i++)
                {
                    if (!m_assetBundleDict.ContainsKey(strDependecise[i]))
                    {
                        AssetBundle tempBundle = AssetBundle.LoadFromFile(GetResPathByName(strDependecise[i]));
                        if (tempBundle != null)
                        {
                            m_assetBundleDict.Add(strDependecise[i], tempBundle);
                        }
                        else
                        {
                            Debug.LogErrorFormat("资源加载失败：{0}", strDependecise[i]);
                        }
                    }
                }
                path = strBaseDir + path;
                AssetBundle bundle = null;
                if (!m_assetBundleDict.ContainsKey(info.strFile))
                {
                    bundle = AssetBundle.LoadFromFile(path);
                    if (bundle != null)
                    {
                        m_assetBundleDict.Add(info.strFile, bundle);
                    }
                    else
                    {
                        Debug.LogErrorFormat("资源加载失败:{0}", path);
                    }
                }
                else
                {
                    bundle = m_assetBundleDict[info.strFile];
                }
                string loadAssetName = string.IsNullOrEmpty(info.assetName) ? info.strName : info.assetName;
                UnityEngine.Object[] tempObjList = bundle.LoadAllAssets();
                var count = tempObjList.Length;
                for (int i = 0; i < tempObjList.Length; i++)
                {
                    ProcessAsset(strAssetID, tempObjList[i], needLoad.callback, info.eAssetType, count, i);
                    ProcessStreamedAsset(info, tempObjList[i], bundle);
                }
                return;
            }

            needLoad.callback?.Invoke(strAssetID, null);
            Debug.LogError("配置了不存在的资源：" + path);
            return;

        }
        //----------------------------------------------------------------------------
        public void ProcessAsset(string strAssetID, UnityEngine.Object objAsset, Delegate call, EAssetType type, int count, int index)
        {
            assetsManager.AddAssetList(strAssetID, objAsset, type);

            if ((count - 1 == index) && count > 1)
            {
                (call as Callback<string, UnityEngine.Object>)?.Invoke(strAssetID, objAsset);
                call = null;
            }
            else if (count == 1)
            {
                (call as Callback<string, UnityEngine.Object>)?.Invoke(strAssetID, objAsset);
                call = null;
            }


        }
        //----------------------------------------------------------------------------
        public void ProcessStreamedAsset(sAssetInfo info, UnityEngine.Object objAsset, AssetBundle bundle)
        {

            if (string.IsNullOrEmpty(info.strFile) || objAsset == null || bundle == null)
            {
                return;
            }
            if (info.eAssetType == EAssetType.eAudio)
            {
                AudioClip clip = objAsset as AudioClip;
                if (clip.loadType == AudioClipLoadType.Streaming)
                {
                    if (!m_audioAssetList.Contains(info.strFile))
                    {
                        m_audioAssetList.Add(info.strFile);
                    }
                    assetsManager.AddAssetBundleList(info.strID, bundle);
                }
            }
        }
        //----------------------------------------------------------------------------
        public string GetResPathByName(string strName)
        {
            return GetResDirByPath(strName) + strName;
        }
        //----------------------------------------------------------------------------
        public string GetResDirByPath(string path)
        {
            string strBaseDir;
            if (RuntimePlatform.Android == Application.platform)
            {
                if (m_ConfigData.m_strLocal == "1")
                {
                    strBaseDir = m_strStreamAssetsDir;
                }
                else if (File.Exists(m_strPersistenDir + path))
                {
                    strBaseDir = m_strPersistenDir;
                }
                else
                {
                    strBaseDir = m_strStreamAssetsDir;
                }
            }
            else if (RuntimePlatform.IPhonePlayer == Application.platform)
            {
                if (m_ConfigData.m_strLocal == "1")
                {
                    strBaseDir = m_strStreamAssetsDir;
                }
                else if (File.Exists(m_strPersistenDir + path))
                {
                    strBaseDir = m_strPersistenDir;
                }
                else
                {
                    strBaseDir = m_strStreamAssetsDir;
                }

            }
            else if (RuntimePlatform.WebGLPlayer == Application.platform)
            {
                strBaseDir = m_strStreamAssetsDir;
            }
            else
            {
                if (m_ConfigData.m_strResDir == "")
                {
                    strBaseDir = m_strStreamAssetsDir;
                }
                else
                {
                    if (File.Exists(m_ConfigData.m_strResDir + path))
                    {
                        strBaseDir = m_ConfigData.m_strResDir;
                    }
                    else
                    {
                        strBaseDir = m_strStreamAssetsDir;
                    }
                }
            }

            return strBaseDir;

        }
        //----------------------------------------------------------------------------
        private void UpdateWeb()
        {

        }
        //----------------------------------------------------------------------------
        private void UpdateDev()
        {
            switch (m_state)
            {
                case FileSystemState.File_Init:
                    break;
                case FileSystemState.File_LoadingGamedata:
                    break;
                case FileSystemState.File_LoadedGamedata:
                    break;
                case FileSystemState.File_ParseGameData:
                    UpdateParseGameData();
                    break;
                case FileSystemState.File_DeCompress:
                    break;
                case FileSystemState.File_OK:
                    UpdateLoadRes();
                    break;
                default:
                    break;
            }
            if (m_devLoadGameDataLeftCount > 0)
            {
                m_devLoadGameDataLeftCount--;
            }
            else if (m_devLoadGameDataLeftCount == 0)
            {
                m_ResourceList.LoadResourceListFileDev();
                m_devLoadGameDataLeftCount = -1;
                m_bBundleDataLoaded = true;
            }
        }
        //----------------------------------------------------------------------------
        private void UpdateCheckFileOK()
        {
            if (m_bBundleDataLoaded && m_bSerializeDataLoaded)
            {
                m_bBundleDataLoaded = false;
                GoState(FileSystemState.File_ParseGameData);
                Messenger.Broadcast(DgMsgID.DgMsg_InitStatChange, UpdateState.Parse_GameData);
                WorldManager.Instance().IniDataLayer();
                m_queNeedParse = WorldManager.Instance().GetLayer<DataLayer>().GetNeedParseDataQue();
                if (m_queNeedParse != null)
                {
                    m_nNeedParseDataCount = m_queNeedParse.Count;
                    m_nCurPaseDataCount = 0;
                    Messenger.Broadcast(DgMsgID.DgMsg_UpdateGameProgress, 0f);

                }
                else
                {
                    GoState(FileSystemState.File_OK);
                    LoadDepRes();
                }
            }
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
                assetsManager.LoadAssetRes<string, UnityEngine.Object>(depsList[i], LoadFinishedCallBack);
            }
        }
        //----------------------------------------------------------------------------
        public void LoadFinishedCallBack(string strAssetID, UnityEngine.Object obj)
        {
            ++m_nNeedLoadDepsCount;
            if (m_nNeedLoadDepsCount >= m_MaxDepsCount)
            {
                LoadLoadingTextureFirstTime();
            }
        }
        //----------------------------------------------------------------------------
        public void LoadLoadingTextureFirstTime()
        {
            LoadingUIManager.Instance().PreLoadLoadingTexture(delegate (string strAssetID, UnityEngine.Object obj)
                {
                    GoInitGame();
                }, true);
        }
        //----------------------------------------------------------------------------
        private void GoInitGame()
        {
            GoState(FileSystemState.File_OK);
            Messenger.Broadcast(DgMsgID.DgMsg_UpdateGameProgress, 1f);
            Messenger.Broadcast(DgMsgID.DgMsg_InitStatChange, UpdateState.Init_Game);
            Messenger.Broadcast(DgMsgID.DgMsg_FileSystemOK);
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

            UnityEngine.Object obj = assetsManager.GetAssetObjByID(strAssetID);
            if (obj != null)
            {
                Callback<string, object> callback = result as Callback<string, object>;
                callback(strAssetID, obj);
                return true;
            }
            AssetBundle bundle = null;
            if (m_assetBundleDict.TryGetValue(info.strFile, out bundle))
            {
                Callback<string, UnityEngine.Object> callback = result as Callback<string, UnityEngine.Object>;
                string loadAssetName = string.IsNullOrEmpty(info.assetName) ? info.strName : info.assetName;
                UnityEngine.Object[] objlist = bundle.LoadAllAssets();
                var count = objlist.Length;
                for (int i = 0; i < objlist.Length; i++)
                {
                    ProcessAsset(strAssetID, objlist[i], callback, info.eAssetType, count, i);
                    ProcessStreamedAsset(info, objlist[i], bundle);
                }
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
                        Debug.LogError(m_ConfigData.m_strDataDir + "  strFile:  " + strFile + " is not exist!");
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
                    m_assetManager = WorldManager.Instance().GetLayer<AssetLayer>().GetManager();
                }
                return m_assetManager;
            }
        }
        //----------------------------------------------------------------------------
        public void SetCountFrame(int nFrame)
        {
            m_nCurCountPerFrame = nFrame;
        }
        //----------------------------------------------------------------------------
        public string LoadXml_Resourselist_other(string strFile)
        {
            string strFileName = strFile;
            if (!strFile.EndsWith(".xml"))
            {
                strFileName = strFile + ".xml";
            }
            if (File.Exists(strFileName))
            {
                return File.ReadAllText(strFileName);
            }
            else
            {
                Debug.LogError(strFileName + "is not exist!");
            }

            return null;
        }
        //----------------------------------------------------------------------------
    }
    //需要下载的数据缓存
    //----------------------------------------------------------------------------
    public struct sNeedLoadData
    {
        public string strAssetID;
        public Callback<string, UnityEngine.Object> callback;
    }
    //----------------------------------------------------------------------------
    public class CLoadData
    {
        //----------------------------------------------------------------------------
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
        //----------------------------------------------------------------------------
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
        //----------------------------------------------------------------------------
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
        //----------------------------------------------------------------------------
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
        //----------------------------------------------------------------------------
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
        //----------------------------------------------------------------------------
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
        //----------------------------------------------------------------------------
    }
}

