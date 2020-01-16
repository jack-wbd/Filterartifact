//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
// Using
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
// Class
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	UIController.cs
//------------------------------------------------------------------------------
/**
\file	UIController.cs

Copyright (c) 2019, BoYue. All rights reserved.

<PRE>

模块名称：<文件所属的模块名称>
文件名称：UIController.cs
摘    要：完成UI的业务强逻辑，提供通信和数据服务

当前版本：1.0
建立日期：2019/12/6
作    者：wangbodong
电子邮件：wangbodong@BoYue.com
备    注：<其它说明>

</PRE>
*/
using System;
using UnityEngine;
namespace Filterartifact
{
    public class UIController : IMsgPipe, ILoadResult
    {
        //----------------------------------------------------------------------------
        public UIBase viewer = null;
        public string strAssetID;
        protected UISystem m_uiSystem = null;
        protected bool m_bLocal = true;
        protected Type uiClass;
        public string strCtrl;
        public bool bEffect = true;
        private bool m_bIsShow = false;
        protected bool isViewLoadedComplete = false;
        private object m_paramData;
        public bool m_bHasAddAtlas = false;
        private bool m_bNeededResLoadOK = false;
        public bool m_bWaitToCricle = false;
        int m_connectCount = 0;
        public GameObject m_Obj = null;
        public bool m_bNeedDelayHidePreUI = false;
        //----------------------------------------------------------------------------
        public void LoadFinishedEx(string strAssetID, UnityEngine.Object obj)
        {
            if (obj != null && obj.GetType() != typeof(AssetBundle) && viewer != null)
            {
                GameObject m_Obj = UnityEngine.Object.Instantiate(obj) as GameObject;
                viewer.strName = obj.name;
                viewer.SetObjUI(m_Obj);
                DeActiveForWait();
                viewer.SetSystem(m_uiSystem);
                viewer.LoadNeedRes(this);
            }
            else
            {
                if (viewer != null)
                {
                    viewer.OnDestroy();
                }
                else
                {
                    Debug.LogError(string.Format("LoadFinishedEx(); strAssetID:{0}", strAssetID));
                }
            }
        }
        //----------------------------------------------------------------------------
        public virtual void Update()
        {
            if (m_bNeededResLoadOK && m_connectCount <= 0)
            {
                SetLoadedOK(false);
                DoLoadUIFinish();
            }
        }
        //----------------------------------------------------------------------------
        public void DoComplete()
        {
            isViewLoadedComplete = true;
            initViewerComplete(m_paramData, m_bIsShow);
        }
        //----------------------------------------------------------------------------
        protected virtual void initViewerComplete(object arg, bool bflag = true)
        {
            if (bflag)
            {
                Show(arg);
            }
            else
                Hide();
        }
        //----------------------------------------------------------------------------
        public virtual void Hide()
        {
            m_bIsShow = false;
            if (!isViewLoadedComplete)
            {
                return;
            }
            viewer.bEffect = bEffect;
            viewer.Hide();
        }
        //----------------------------------------------------------------------------
        public virtual void FinalViewer()
        {
            if (viewer != null)
            {
                viewer = null;
                isViewLoadedComplete = false;
            }
        }
        //----------------------------------------------------------------------------
        public bool IsViewLoadedComplete()
        {
            return isViewLoadedComplete;
        }
        //----------------------------------------------------------------------------
        public void SetLoadedOK(bool bOK)
        {
            m_bNeededResLoadOK = bOK;
        }
        //----------------------------------------------------------------------------
        public void DoLoadUIFinish()
        {
            m_bWaitToCricle = false;
            DoCreate();
            DoComplete();
        }
        //----------------------------------------------------------------------------
        public void DoCreate()
        {
            if (m_bLocal)
            {
                m_uiSystem.AddUIClass(strAssetID, viewer);
                viewer.strAssetID = strAssetID;
                if (!viewer.Create())
                {
                    viewer.OnDestroy();
                    return;
                }
            }
            DoCreateEx();
        }
        //----------------------------------------------------------------------------
        public virtual void DoCreateEx()
        {

        }
        //----------------------------------------------------------------------------
        public bool NeedDelayHide()
        {
            return m_bNeedDelayHidePreUI;
        }
        //----------------------------------------------------------------------------
        private eUIImpower _impower = eUIImpower.Default;
        public eUIImpower impower
        {
            set
            {
                _impower = value;
                if (viewer != null)
                {
                    viewer.impower = impower;
                }
            }
            get
            {
                return _impower;
            }
        }
        //----------------------------------------------------------------------------
        public virtual void SetBaseInfo(string strAssetName, UISystem system, bool bLocal, Type uiType, string strCtrlName)
        {
            strAssetID = strAssetName;
            m_uiSystem = system;
            m_bLocal = bLocal;
            uiClass = uiType;
            strCtrl = strCtrlName;
        }
        //----------------------------------------------------------------------------
        public void SetDelayHide(bool value)
        {
            m_bNeedDelayHidePreUI = value;
        }
        //----------------------------------------------------------------------------
        public virtual void InitCtrl()
        {

        }
        //----------------------------------------------------------------------------
        public virtual void InitViewer(object arg, bool bflag = false)
        {
            m_paramData = arg;
            m_bIsShow = bflag;
            if (viewer == null)
            {
                if (!m_bHasAddAtlas)
                {
                    AddUseAtlas();
                }
                viewer = Activator.CreateInstance(uiClass) as UIBase;
                viewer.strCtrl = strCtrl;
                viewer.impower = impower;
                viewer.RegisterViewMsg();
                viewer.SetCtrl(this);
                viewer.m_ctrl = this;
                PlugInMsgPipe(viewer);
                UnityEngine.Object obj = m_uiSystem.GetAssetBundle(strAssetID);
                if (obj != null)
                {
                    m_Obj = UnityEngine.Object.Instantiate(obj) as GameObject;
                    viewer.strName = obj.name;
                    viewer.SetObjUI(m_Obj);
                    DeActiveForWait();
                    viewer.SetSystem(m_uiSystem);
                    viewer.LoadNeedRes(this);
                }
                else
                {
                    if (!IsPreLoadView() && !LoadingUIManager.isInLoading && !("UICircleCtrl" == strCtrl))
                    {
                        CheckLoadWaitStart();
                    }
                    m_uiSystem.LoadUIAsset(strAssetID, LoadFinishedEx);
                }

            }
        }
        //----------------------------------------------------------------------------
        private void CheckLoadWaitStart()
        {
            SetLoadedOK(false);
        }
        //----------------------------------------------------------------------------
        public bool IsPreLoadView()
        {
            ResourceListData data = FileSystem.Instance().GetResData();
            if (data == null)
            {
                return false;
            }
            sAssetInfo info = sAssetInfo.zero;
            data.GetAssetBundleInfo(strAssetID, ref info);
            return info.bPreLoad;
        }
        //----------------------------------------------------------------------------
        private void DeActiveForWait()
        {
            if (viewer == null || viewer.GetUIObject() == null)
            {
                return;
            }
            if (viewer.GetUIObject().activeSelf != false)
            {
                viewer.GetUIObject().SetActive(false);
            }

        }
        //----------------------------------------------------------------------------
        private void AddUseAtlas()
        {
            m_bHasAddAtlas = true;
            ResourceListData data = FileSystem.Instance().GetResData();
            sAssetInfo info = sAssetInfo.zero;
            data.GetAssetBundleInfo(strAssetID, ref info);
            if (info.childListAssetID != null && info.childListAssetID.Count > 0)
            {
                int nTotalCount = info.childListAssetID.Count;
                for (int i = 0; i < nTotalCount; i++)
                {
                    m_uiSystem.AddUseAtlas(info.childListAssetID[i]);
                }
            }

        }
        //----------------------------------------------------------------------------
        public virtual void Show(object arg = null)
        {
            m_bIsShow = true;
            if (!isViewLoadedComplete)
            {
                InitViewer(arg, true);
            }
            else
            {
                viewer.bEffect = bEffect;
                viewer.strAssetID = this.strAssetID;
                viewer.Show(arg);
                initData(arg);
            }
        }
        //----------------------------------------------------------------------------
        protected virtual void initData(object arg)
        {
            viewer.initData(arg);
        }
        //----------------------------------------------------------------------------
        public virtual void ShowOrHideUI(object arg =null)
        {
            if (!isViewLoadedComplete)
            {
                InitViewer(arg, true);
                return;
            }
            if (!viewer.IsShow)
            {
                viewer.bEffect = bEffect;
                viewer.strAssetID = this.strAssetID;
                viewer.Show(arg);
            }
            else
            {
                viewer.bEffect = bEffect;
                viewer.Hide();
            }
        }
        //----------------------------------------------------------------------------
        public virtual void StageLoadViewer()
        {
            m_uiSystem.AddPreLoadAsset(strAssetID);
            ResourceListData data = FileSystem.Instance().GetResData();
            sAssetInfo info = sAssetInfo.zero;
            data.GetAssetBundleInfo(strAssetID, ref info);
            if (info.childListAssetID != null && info.childListAssetID.Count > 0)
            {
                int nTotalCount = info.childListAssetID.Count;
                for (int i = 0; i < nTotalCount; i++)
                {
                    m_uiSystem.AddPreLoadAsset(info.childListAssetID[i]);
                    m_uiSystem.AddUseAtlas(info.childListAssetID[i]);
                }
            }
            m_bHasAddAtlas = true;
        }
        //----------------------------------------------------------------------------
    }
}
