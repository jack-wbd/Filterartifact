//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
// Using
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
// Class
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	StageLayer.cs
//------------------------------------------------------------------------------
/**
\file	StageLayer.cs

Copyright (c) 2020, BoYue. All rights reserved.

<PRE>

模块名称：<文件所属的模块名称>
文件名称：StageLayer.cs
摘    要：<描述该文件实现的主要功能>

当前版本：1.0
建立日期：2020/1/3
作    者：SYSTEM
电子邮件：SYSTEM@BoYue.com
备    注：<其它说明>

</PRE>
*/
using System;
using UnityEngine;
namespace Filterartifact
{
    //----------------------------------------------------------------------------
    public class StageLayer : BaseLayer
    {
        //----------------------------------------------------------------------------
        public StageLoad m_stageLoad = null;
        private CopyEntryCtrl m_EntryCtrl = null;
        private Copy m_CurCopy = null;
        private CopyFactory m_copyFactory = null;

        //----------------------------------------------------------------------------
        public StageLayer()
        {
            m_stageLoad = new StageLoad(null);
            m_EntryCtrl = new CopyEntryCtrl();
            m_EntryCtrl.Initialize();
            m_copyFactory = new CopyFactory();
            Messenger.AddListener(DgMsgID.DgMsg_UnloadAssetFalse, OnUnloadAssetFalse);

        }
        //----------------------------------------------------------------------------
        private void OnUnloadAssetFalse()
        {
            FileSystem.Instance().DoUnloadAssetBundle();
            GC.Collect();
        }
        //----------------------------------------------------------------------------
        public override void Finalized()
        {
            base.Finalized();
            if (m_EntryCtrl != null)
            {
                m_EntryCtrl.Finalized();
                m_EntryCtrl = null;
            }
            m_copyFactory = null;
            Messenger.RemoveListener(DgMsgID.DgMsg_UnloadAssetFalse, OnUnloadAssetFalse);
        }
        //----------------------------------------------------------------------------
        public bool SwitchScene(int nGroupTempID, int nCopyTempID, int nSceneTempID, string strCopy, int nStageIndex)
        {
            //停用所有特效
            //清除声音
            if (string.IsNullOrEmpty(strCopy) || nSceneTempID == 0)
            {
                Debug.LogError("SwitchScene : name empty");
            }

            //第一步 ：切换stage(如果是空或者不是当前stage)
            if (m_CurCopy == null || !m_CurCopy.GetCopyName().Equals(strCopy))
            {
                Copy cNewCopy = m_copyFactory.Constract_Copy(strCopy, this);
                if (cNewCopy != null)
                {
                    if (m_CurCopy != null)
                    {
                        PullOutMsgPipe(m_CurCopy);
                        m_CurCopy.Destroy();
                        m_CurCopy = null;
                    }
                    m_CurCopy = cNewCopy;
                    m_CurCopy.CopyTempID = nCopyTempID;
                    m_CurCopy.GroupTempID = nGroupTempID;
                    PlugInMsgPipe(m_CurCopy);
                    m_CurCopy.SetLayer(this);
                    m_CurCopy.Init();
                    m_CurCopy.ActiveCopy();

                }
                else
                {
                    return false;
                }
            }

            //第二步：切换scene
            if (!m_CurCopy.ActiveScene(nSceneTempID))
            {
                return false;
            }
            return true;
        }
        //----------------------------------------------------------------------------
        public override void Update()
        {
            base.Update();
            if (m_CurCopy != null)
            {
                m_CurCopy.Update();
            }
        }
        //----------------------------------------------------------------------------
    }
}
