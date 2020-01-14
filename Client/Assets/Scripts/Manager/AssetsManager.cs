//------------------------------------------------------------------------------
/**
	\file	AssetsManager.cs

	Copyright (c) 2019, BoYue. All rights reserved.

	<PRE>

	模块名称：<文件所属的模块名称>
	文件名称：AssetsManager.cs
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
//	AssetsManager.cs
//------------------------------------------------------------------------------
using System.Collections.Generic;
using UnityEngine;
namespace Filterartifact
{
    public class AssetsManager : Manager
    {
        //----------------------------------------------------------------------------
        private struct AssetCube
        {
            public Object objAsset;
            public EAssetType typeAsset;
            public static AssetCube Clear
            {
                get
                {
                    AssetCube cube;
                    cube.objAsset = null;
                    cube.typeAsset = EAssetType.eGameObject;
                    return cube;
                }
            }
        }
        //----------------------------------------------------------------------------
        private Dictionary<string, AssetCube> m_dictAsset;//当前所有的资源
        private Dictionary<string, AssetBundle> m_dictStreamedAsset;
        private Dictionary<string, byte> m_dictToBeDelAsset;//待删除的资源
        //------------------------------------------------------------------------
        public override bool Initialized()
        {
            m_dictAsset = new Dictionary<string, AssetCube>();
            m_dictStreamedAsset = new Dictionary<string, AssetBundle>();
            m_dictToBeDelAsset = new Dictionary<string, byte>();
            return true;
        }
        //----------------------------------------------------------------------------
        public Object GetAssetObjByID(string strAssetID)
        {
            if (string.IsNullOrEmpty(strAssetID))
            {
                Debug.LogWarning("the assetID is null");
                return null;
            }
            AssetCube cube;

            if (m_dictAsset.TryGetValue(strAssetID, out cube))
            {
                return cube.objAsset;
            }
            return null;
        }
        //----------------------------------------------------------------------------
        public bool LoadAssetRes<T, U>(string strAssetID, Callback<T, U> call)
        {
            if (call == null)
                return false;

            if (string.IsNullOrEmpty(strAssetID))
                return false;

            AssetCube cube;
            if (m_dictAsset.TryGetValue(strAssetID, out cube))
            {
                Callback<string, Object> callBack = call as Callback<string, Object>;
                callBack(strAssetID, cube.objAsset);
                return true;
            }
            return FileSystem.Instance().StartLoad(strAssetID, call);

        }
        //----------------------------------------------------------------------------
        public int GetAssetNum()
        {
            if (m_dictAsset != null)
            {
                return m_dictAsset.Count;
            }
            return 0;
        }
        //----------------------------------------------------------------------------
        public void UnLoadAsset(string strAsset)
        {
            if (string.IsNullOrEmpty(strAsset))
            {
                return;
            }
            AssetCube cube;
            if (m_dictAsset.TryGetValue(strAsset, out cube))
            {
                UnLoadAsset(strAsset, cube);
                m_dictAsset.Remove(strAsset);
            }

        }
        //------------------------------------------------------------------------
        public void Destroy()
        {
            foreach (KeyValuePair<string, AssetCube> temp in m_dictAsset)
            {
                UnLoadAsset(temp.Key, temp.Value);
            }
            m_dictAsset.Clear();
        }
        //------------------------------------------------------------------------
        private void UnLoadAsset(string strAssetID, AssetCube cube)
        {
            switch (cube.typeAsset)
            {
                case EAssetType.eGameObject:
                case EAssetType.eAtlas:
                    Object.DestroyImmediate(cube.objAsset, true);
                    break;
                case EAssetType.eObject:
                case EAssetType.eTexture:
                    Resources.UnloadAsset(cube.objAsset);
                    break;
                case EAssetType.eAudio:
                    UnloadAudioAsset(strAssetID, cube);
                    break;
                default:
                    break;
            }
        }
        //----------------------------------------------------------------------------
        public void UnloadAudioAsset()
        {
            List<string> unloadList = new List<string>();
            ResourceListData data = FileSystem.Instance().GetResData();
            var it = m_dictAsset.GetEnumerator();
            while (it.MoveNext())
            {
                if (it.Current.Value.typeAsset == EAssetType.eAudio)
                {
                    unloadList.Add(it.Current.Key);
                }
            }
            it.Dispose();

            for (int i = 0; i < unloadList.Count; i++)
            {
                UnLoadAsset(unloadList[i]);
            }

        }
        //----------------------------------------------------------------------------
        private void UnloadAudioAsset(string strAssetID, AssetCube cube)
        {
            AudioClip clip = cube.objAsset as AudioClip;
            AssetBundle bundle = null;
            if (clip.loadType == AudioClipLoadType.Streaming)
            {
                if (m_dictStreamedAsset.TryGetValue(strAssetID, out bundle))
                {
                    if (bundle != null)
                    {
                        bundle.Unload(true);
                    }
                    m_dictStreamedAsset.Remove(strAssetID);
                }
            }
            else
            {
                Resources.UnloadAsset(cube.objAsset);
            }
        }
        //----------------------------------------------------------------------------
        public void AddAssetList(string strAssetID, Object obj, EAssetType type = EAssetType.eGameObject)
        {

            if (m_dictAsset.ContainsKey(strAssetID))
            {
                return;
            }
            if (obj == null)
            {
                Debug.LogError("load assetid: " + strAssetID + " no bundle");
                return;
            }

            if (obj.GetType() == typeof(AssetBundle))
            {
                Debug.LogError("load assetid: " + strAssetID + "only bundle");
                return;
            }

            AssetCube cube;
            cube.objAsset = obj;
            cube.typeAsset = type;
            m_dictAsset.Add(strAssetID, cube);
            ProcessToBeDeletList(strAssetID);
        }
        //----------------------------------------------------------------------------
        public void AddAssetBundleList(string strAssetID, AssetBundle bundle)
        {
            if (m_dictStreamedAsset.ContainsKey(strAssetID))
            {
                Debug.LogError("why has the same audio key");
            }
            else
            {
                m_dictStreamedAsset.Add(strAssetID, bundle);
            }
        }
        //----------------------------------------------------------------------------
        private void ProcessToBeDeletList(string strAssetID)
        {
            m_dictToBeDelAsset.Remove(strAssetID);
        }
        //----------------------------------------------------------------------------
        public void TobeDelAsset(string strAsset)
        {
            m_dictToBeDelAsset[strAsset] = 0;
        }
        //----------------------------------------------------------------------------
    }
}

