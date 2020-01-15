﻿//------------------------------------------------------------------------------
/**
	\file	GameUI.cs

	Copyright (c) 2019, BoYue. All rights reserved.

	<PRE>

	模块名称：<文件所属的模块名称>
	文件名称：GameUI.cs
	摘    要：<描述该文件实现的主要功能>

	当前版本：1.0
	建立日期：2019/12/30
	作    者：SYSTEM
	电子邮件：SYSTEM@BoYue.com
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
//	GameUI.cs
//------------------------------------------------------------------------------

namespace Filterartifact
{
    //----------------------------------------------------------------------------
    public class GameUI : IGameApp
    {
        //----------------------------------------------------------------------------
        private UILayer m_layerUI = null;
        //----------------------------------------------------------------------------
        public override bool Initialize()
        {
            m_layerUI = new UILayer();
            m_layerUI.Initialize();
            Messenger.AddListener(DgMsgID.DgMsg_RegisterAllUI, OnRegisterAllUI);
            Messenger.AddListener(DgMsgID.DgMsg_InitAfterMain, OnInitAferMain);
            Messenger.AddListener<string>(DgMsgID.DgUI_ShowUI, OnShowUI);
            return true;
        }
        //----------------------------------------------------------------------------
        private void OnShowUI(string strCtrl)
        {
            if (m_layerUI!=null)
            {
                m_layerUI.Show(strCtrl);
            }
        }
        //----------------------------------------------------------------------------
        public override void Update()
        {
           if (m_layerUI!=null)
           {
                m_layerUI.Update();
           }
        }
        //----------------------------------------------------------------------------
        private void OnRegisterAllUI()
        {
            if (m_layerUI!=null)
            {
                m_layerUI.RegisterUIControl();
            }
        }
        //----------------------------------------------------------------------------
        public override void Destroy()
        {
            base.Destroy();
            Messenger.RemoveListener<string>(DgMsgID.DgUI_ShowUI, OnShowUI);
            Messenger.RemoveListener(DgMsgID.DgMsg_RegisterAllUI, OnRegisterAllUI);
            Messenger.RemoveListener(DgMsgID.DgMsg_InitAfterMain, OnInitAferMain);
        }
        //----------------------------------------------------------------------------
        private void OnHideCountUI()
        {

        }
        //----------------------------------------------------------------------------
        private void OnInitAferMain()
        {
            if (m_layerUI!=null)
            {
                m_layerUI.InitializeAfterMain();
            }
        }
        //----------------------------------------------------------------------------

    }
    //----------------------------------------------------------------------------
}
