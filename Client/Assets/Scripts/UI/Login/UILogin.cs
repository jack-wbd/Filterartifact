using UnityEngine.UI;
//------------------------------------------------------------------------------
/**
	\file	UILogin.cs

	Copyright (c) 2020, BoYue. All rights reserved.

	<PRE>

	模块名称：<文件所属的模块名称>
	文件名称：UILogin.cs
	摘    要：<描述该文件实现的主要功能>

	当前版本：1.0
	建立日期：2020/1/8
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
//	UILogin.cs
//------------------------------------------------------------------------------

namespace Filterartifact
{
    public class UILogin : UIBase
    {
        //----------------------------------------------------------------------------
        private InputField m_accountInput;
        private InputField m_passwordInput;
        private Button m_beginBtn;
        UILoginCtrl m_curtrl;
        //----------------------------------------------------------------------------
        protected override bool OnCreate()
        {
            bool bResult = GetUIObject();
            if (bResult)
            {
                m_beginBtn.onClick.AddListener(() => OnEnterClick());
            }
            return true;
        }
        //----------------------------------------------------------------------------
        public override void Show(object arg = null)
        {
            base.Show(arg);
        }
        //----------------------------------------------------------------------------
        public override void Hide()
        {
            base.Hide();
        }
        //----------------------------------------------------------------------------
        new bool GetUIObject()
        {
            if (m_objUI != null)
            {
                m_curtrl = (UILoginCtrl)m_ctrl;
                m_accountInput = m_uiTrans.Find("anchor/CenterAnchorPath/account/InputField").GetComponent<InputField>();
                m_passwordInput = m_uiTrans.Find("anchor/CenterAnchorPath/password/InputField").GetComponent<InputField>();
                m_beginBtn = m_uiTrans.Find("anchor/CenterAnchorPath/oldbutton").GetComponent<Button>();
            }
            return true;
        }
        //----------------------------------------------------------------------------
        private void OnEnterClick()
        {
            if (!string.IsNullOrEmpty(m_accountInput.text))
            {
                Hide();
                m_curtrl.OnConnectSocialSuc();
            }
        }
        //----------------------------------------------------------------------------
        protected override void OnUpdate()
        {
            base.OnUpdate();

        }
        //----------------------------------------------------------------------------
    }
}
