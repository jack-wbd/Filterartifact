//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
// Using
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
// Class
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	StageLoad.cs
//------------------------------------------------------------------------------
/**
\file	StageLoad.cs

Copyright (c) 2020, BoYue. All rights reserved.

<PRE>

模块名称：<文件所属的模块名称>
文件名称：StageLoad.cs
摘    要：<描述该文件实现的主要功能>

当前版本：1.0
建立日期：2020/1/3
作    者：SYSTEM
电子邮件：SYSTEM@BoYue.com
备    注：<其它说明>

</PRE>
*/
using System.Collections.Generic;
using UnityEngine;

namespace Filterartifact
{
    //----------------------------------------------------------------------------
    public class StageLoad : LoadSceneBase
    {
        //----------------------------------------------------------------------------
        private AssetsManager m_maAsset;
        private int m_nAssetCount = 0;
        private int m_nTotalCount = 0;
        private List<string> m_listStageRes = null;
        private List<string> m_listStageResCommon = null;
        private bool m_bLoadOK = false;
        private bool m_bInBaseLoading = true;
        private bool m_bUpdate = false;
        private bool m_bInit = false;

        private bool m_bUILoadOK = false;
        private bool m_bLoadPresserOK = true;

        private bool m_bCheckDelete = false;
        private int m_nWaitCount = 0;
        private int m_nNeedWaitCount = 2;
        Dictionary<string, string> preLoadAssets;
        private Copy m_curCopy = null;
        private Scene m_curScene = null;
        //----------------------------------------------------------------------------
        public StageLoad()
        {
            m_maAsset = new AssetsManager();
            m_listStageRes = new List<string>();
            m_listStageResCommon = new List<string>();
            preLoadAssets = new Dictionary<string, string>();
            m_bUpdate = false;
            m_bInit = false;
            Messenger.AddListener(DgMsgID.DgMsg_NtyUILoading_Finish, OnUILoadOK);
            Messenger.AddListener<bool>(DgMsgID.DgMsg_NtyLoadProsser_Finish, OnLoadPresserOK);
        }
        //----------------------------------------------------------------------------
        private void RemoveMsgListener()
        {
            Messenger.RemoveListener(DgMsgID.DgMsg_NtyUILoading_Finish, OnUILoadOK);
            Messenger.RemoveListener<bool>(DgMsgID.DgMsg_NtyLoadProsser_Finish, OnLoadPresserOK);
        }
        //----------------------------------------------------------------------------
        private void OnUILoadOK()
        {

        }
        //----------------------------------------------------------------------------
        public void Update()
        {
            CheckDoDeleteUnUse();
            if (m_bInBaseLoading)
            {
                return;
            }
            CheckAllFinish();
            if (!m_bUpdate)
            {
                return;
            }
            m_bUpdate = false;

        }
        //----------------------------------------------------------------------------
        public void CheckDoDeleteUnUse()
        {
            if (m_bCheckDelete)
            {
                if (m_nWaitCount >= m_nNeedWaitCount)
                {
                    m_nWaitCount = 0;
                    m_bCheckDelete = false;
                    AcitvePreLoad();
                }
                ++m_nWaitCount;
            }
        }
        //----------------------------------------------------------------------------
        private void CheckAllFinish()
        {
            if (!m_bInBaseLoading && m_bLoadPresserOK && m_bUILoadOK)
            {
                m_bUILoadOK = true;
                SetLoadOK(true);
                m_bLoadPresserOK = false;
            }
        }
        //----------------------------------------------------------------------------
        public override void AcitvePreLoad()
        {
            Messenger.Broadcast(DgMsgID.DgMsg_ActiveLoadUI);
            int nCount = 0;
            if (m_bInit == false)
            {
                nCount = m_listStageResCommon.Count;
                for (int i = 0; i < nCount; i++)
                {
                    if (!m_maAsset.LoadAssetRes<string, Object>(m_listStageResCommon[i], OnLoadCallBack))
                    {
                        CheckFinish();
                    }
                }

            }
            nCount = m_listStageRes.Count;
            for (int i = 0; i < nCount; i++)
            {
                if (!m_maAsset.LoadAssetRes<string, Object>(m_listStageRes[i], OnLoadCallBack))
                {
                    CheckFinish();
                }
            }

            m_bInit = false;
            m_bUpdate = false;

        }
        //----------------------------------------------------------------------------
        public void OnLoadCallBack(string strAssetID, Object obj)
        {
            CheckFinish();
        }
        //----------------------------------------------------------------------------
        private void OnLoadPresserOK(bool LoadOk)
        {
            m_bLoadPresserOK = LoadOk;
        }
        //----------------------------------------------------------------------------
        private void SetLoadOK(bool bLoad)
        {
            m_bLoadOK = bLoad;
        }
        //----------------------------------------------------------------------------
        public bool IsLoadOK()
        {
            return m_bLoadOK;
        }
        //----------------------------------------------------------------------------
        public void CheckFinish()
        {
            ++m_nAssetCount;
            CurAssetCount = m_nAssetCount;
            if (m_nAssetCount >= m_nTotalCount)
            {
                m_bInBaseLoading = false;
                m_nAssetCount = 0;
                m_nTotalCount = 0;
            }
        }
        //----------------------------------------------------------------------------
        public void UnLoad(bool bDo = false)
        {
            int nCount = m_listStageRes.Count;
            for (int i = 0; i < nCount; i++)
            {
                m_maAsset.TobeDelAsset(m_listStageRes[i]);
            }
            m_listStageRes.Clear();
            if (bDo)
            {
                m_bInit = false;
                nCount = m_listStageResCommon.Count;
                for (int i = 0; i < nCount; ++i)
                {
                    m_maAsset.TobeDelAsset(m_listStageResCommon[i]);
                }
                m_listStageResCommon.Clear();
            }
            preLoadAssets.Clear();
        }
        //----------------------------------------------------------------------------
        private void Clear(bool bDo = false)
        {
            preLoadAssets.Clear();
            m_listStageRes.Clear();
            if (bDo)
            {
                m_maAsset = null;
                m_listStageRes = null;
                preLoadAssets = null;
            }
        }
        //----------------------------------------------------------------------------
        //副本优化接口，先是房间式
        public void StartSceneLoad(Scene scene)
        {

            if (m_maAsset == null)
            {
                m_maAsset = WorldManager.Instance().GetLayer<AssetLayer>().GetManager();
            }

            //2.保存要创建房间的资源
            m_curScene = scene;
            if (scene != null)
            {
                m_bUILoadOK = false;
                FileSystem.bNotInLoading = false;
                eSceneType type = scene.GetSceneType();





            }

            //3.两个房间通用的资源保留，接着做销毁操作

            m_bInBaseLoading = true;

            StartProgress();

        }
        //----------------------------------------------------------------------------
        private void StartProgress()
        {
            Messenger.Broadcast(DgMsgID.DgMsg_NtyStageLoadingProcess, true);
            if (m_bInit == false)
            {
                m_nTotalCount = m_listStageRes.Count + m_listStageResCommon.Count;
                MaxAssetCount = m_nTotalCount;
                CurAssetCount = 0;
            }
            else
            {
                m_nTotalCount = m_listStageRes.Count;
                MaxAssetCount = m_nTotalCount;
                CurAssetCount = 0;
            }
            Debug.Log("stageLoad asset count: " + MaxAssetCount);
        }
        //----------------------------------------------------------------------------


    }
}
