//------------------------------------------------------------------------------
using DG.Tweening;
/**
	\file	UIError.cs

	Copyright (c) 2020, BoYue. All rights reserved.

	<PRE>

	模块名称：<文件所属的模块名称>
	文件名称：UIError.cs
	摘    要：<描述该文件实现的主要功能>

	当前版本：1.0
	建立日期：2020/4/16
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
//	UIError.cs
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Filterartifact
{
    class UIError : UIBase
    {
        Tween m_tweenMove;
        Text m_msg;
        //----------------------------------------------------------------------------
        protected override bool OnCreate()
        {
            bool bResult = GetUIObject();
            return bResult;
        }
        //----------------------------------------------------------------------------
        public bool GetUIObject()
        {
            if (m_objUI != null)
            {
                m_msg = GetUIComponent<Text>("Label");
                m_msg.text = string.Empty;
            }
            return true;
        }
        //----------------------------------------------------------------------------
        public override void Show(object arg = null)
        {
            if (arg == null)
                return;
            if (IsShow)
            {
                if (m_tweenMove != null)
                {
                    IsShow = false;
                    OnComplete();
                }
            }
            if (!IsShow)
            {
                base.Show(arg);
            }
            if (arg.GetType() == typeof(string))
            {
                m_msg.text = arg.ToString();
            }
            m_tweenMove = GetUIComponent<RectTransform>().DOLocalMoveY(300f, 1f);
            m_tweenMove.onComplete = OnComplete;

        }
        //----------------------------------------------------------------------------
        private void OnComplete()
        {
            Messenger.Broadcast(DgMsgID.DgUI_HideUI, "UIErrorCtrl");
            if (m_tweenMove != null)
            {
                m_tweenMove.Kill();
            }
        }
        //----------------------------------------------------------------------------
    }
}
