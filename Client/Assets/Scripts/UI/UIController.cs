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
using System;
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
        //----------------------------------------------------------------------------
        public void LoadFinishedEx(string strAssetID, UnityEngine.Object obj)
        {

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
        }
        //----------------------------------------------------------------------------
        //----------------------------------------------------------------------------
    }
}
