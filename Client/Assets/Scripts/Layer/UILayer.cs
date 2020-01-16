using System.Collections.Generic;
using UnityEngine;
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
        public bool InitializeAfterMain()
        {
            if (m_uiSystem != null)
            {
                m_uiSystem.InitializeAferMain();
            }
            return true;
        }
        //----------------------------------------------------------------------------
        public void RegisterUIControl()
        {
            Register<UILoginCtrl, UILogin>("RUP_Login", this);
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
            system.AddUIController(t.strCtrl, useInfo);
            pipe.PlugInMsgPipe(t);

            return t;
        }
        //----------------------------------------------------------------------------
        public void HideLateUI(string UiKey)
        {

            if (string.IsNullOrEmpty(UiKey))
            {
                return;
            }
            string needDelayUIName = UIHistory.GetDelayHideUIName(UiKey);
            if (!string.IsNullOrEmpty(needDelayUIName))
            {
                UIController historyCtrl = m_uiSystem.GetUIControllerById(needDelayUIName);
                if (historyCtrl != null)
                {
                    historyCtrl.bEffect = true;
                    historyCtrl.Hide();
                    UIHistory.RemoveShowKey(needDelayUIName);
                    UIHistory.RemoveDelayHideUI(needDelayUIName);
                }
            }
        }
        //----------------------------------------------------------------------------
        public void Hide(string strkey)
        {
            UIController uiCtrl = m_uiSystem.GetUIControllerById(strkey);
            UIHistory.RemoveShowKey(strkey);
            if (uiCtrl!=null)
            {
                uiCtrl.Hide();
            }
            UIHistory.Hide(strkey);
        }
        //----------------------------------------------------------------------------
        public override void Update()
        {
            if (m_uiSystem != null)
            {
                m_uiSystem.Update();
            }
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
        public override void Finalized()
        {
            base.Finalized();
            if (m_uiSystem != null)
            {
                m_uiSystem.Finalized();
                m_uiSystem = null;
            }
        }
        //----------------------------------------------------------------------------
        public void Show(string strkey, Object arg = null, bool _bEffect = true)
        {
            if (SystemOpenHookMessage(strkey))
            {
                return;
            }

            UIController uiCtrl = m_uiSystem.GetUIControllerById(strkey);
            if (uiCtrl != null)
            {
                uiCtrl.bEffect = _bEffect;
                UIHistory.Show(strkey, arg, uiCtrl.impower);
                uiCtrl.Show(arg);
            }

        }
        //----------------------------------------------------------------------------
        //系统开放屏蔽VIP等上层入口比较多的功能
        private bool SystemOpenHookMessage(string controller)
        {

            if (WorldManager.Instance() == null)
            {
                return false;
            }

            if (controller == null)
            {
                return false;

            }

            return false;

        }
        //----------------------------------------------------------------------------
        //----------------------------------------------------------------------------
    }
    //----------------------------------------------------------------------------

}
