﻿using System.Collections.Generic;
//------------------------------------------------------------------------------
/**
	\file	UILayer.cs

	Copyright (c) 2019, BoYue. All rights reserved.

	<PRE>

	模块名称：<文件所属的模块名称>
	文件名称：UILayer.cs
	摘    要：<描述该文件实现的主要功能>

	当前版本：1.0
	建立日期：2019/12/6
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
//	UILayer.cs
//------------------------------------------------------------------------------

namespace Filterartifact
{
    //----------------------------------------------------------------------------
    public class UILayer : BaseLayer
    {
        //----------------------------------------------------------------------------
        private UISystem m_uiSystem;
        private Dictionary<string, string> m_assetClassMap;
        //----------------------------------------------------------------------------
        public override bool Initialize()
        {

            m_assetClassMap = new Dictionary<string, string>();
            CreateUISystem();
            return true;
        }
        //----------------------------------------------------------------------------
        public void RegisterUIControl()
        {

        }
        //----------------------------------------------------------------------------
        public TCtrl Register<TCtrl, TBase>(string strAssetID, IMsgPipe pipe, bool bLocal = true, eUIImpower _impower = eUIImpower.Default)
            where TCtrl : UIController, new()
            where TBase : UIBase, new()
        {
            UISystem system = GetUISystem();
            if (system.HasUIClass(strAssetID))
            {
                return default;
            }
            sAssetInfo info = sAssetInfo.zero;
            system.GetAssetInfo(strAssetID, ref info);
            system.AddUITypeEnvToDict(typeof(TBase), info.eEnvirUse);
            bool bflag = system.HasUIController(strAssetID);
            if (bflag)
            {
                return null;
            }
            TBase t_base = new TBase();
            t_base.strAssetID = strAssetID;
            TCtrl t = new TCtrl();
            t.impower = _impower;
            t.SetBaseInfo(strAssetID, system, bLocal, typeof(TBase), typeof(TCtrl).Name);
            t.InitCtrl();

            //Newbie
            m_assetClassMap[strAssetID] = t.strCtrl;

            sUIUseInfo useInfo;
            useInfo.strAsset = strAssetID;
            useInfo.uiCtrl = t;
            useInfo.eEnvirUse = info.eEnvirUse;
            useInfo.bReload = info.bPreLoad;
            system.AddUIController(strAssetID, useInfo);
            pipe.PlugInMsgPipe(t);

            return t;
        }
        //----------------------------------------------------------------------------
        public UISystem GetUISystem()
        {
            return m_uiSystem;
        }
        //----------------------------------------------------------------------------
        private bool CreateUISystem()
        {
            if (m_uiSystem == null)
            {
                m_uiSystem = new UISystem();
                m_uiSystem.Initialize();
            }
            return true;
        }
        //----------------------------------------------------------------------------
        //----------------------------------------------------------------------------
        //----------------------------------------------------------------------------
        //----------------------------------------------------------------------------
        //----------------------------------------------------------------------------
    }
    //----------------------------------------------------------------------------

}
