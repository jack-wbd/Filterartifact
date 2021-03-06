﻿//------------------------------------------------------------------------------
/**
	\file	UILogoView.cs

	Copyright (c) 2019, BoYue. All rights reserved.

	<PRE>

	模块名称：<文件所属的模块名称>
	文件名称：UILogoView.cs
	摘    要：<描述该文件实现的主要功能>

	当前版本：1.0
	建立日期：2019/11/12
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
//	UILogoView.cs
//------------------------------------------------------------------------------
using DG.Tweening;
using UnityEngine;
namespace Filterartifact
{
    public class UILogoView
    {
        private GameObject m_root;
        private Transform m_logoTrans;
        private CanvasGroup m_logoGroup;
        private CanvasGroup m_healthGroup;
        private GameObject m_logoGo;
        private GameObject m_healthGo;
        //----------------------------------------------------------------------------
        public UILogoView()
        {
            m_root = GameObject.Find("ui_root");
            m_logoTrans = m_root.transform.Find("UICamera/Canvas/CenterPageNode/ui_logo");
            m_logoGo = m_logoTrans.Find("Logo").gameObject;
            m_logoGroup = m_logoGo.GetComponent<CanvasGroup>();
            m_healthGo = m_logoTrans.Find("health").gameObject;
            m_healthGroup = m_healthGo.GetComponent<CanvasGroup>();
            Tweener logoTweener = m_logoGroup.DOFade(1, 1);
            logoTweener.OnComplete(OnLogoImageShowFinish);
        }
        //----------------------------------------------------------------------------
        private void OnLogoImageShowFinish()
        {
            Tweener logoTweener = m_logoGroup.DOFade(0, 2);
            logoTweener.OnComplete(OnLogoImageHideFinish);

        }
        //----------------------------------------------------------------------------
        private void OnLogoImageHideFinish()
        {
            Tweener healthTweener = m_healthGroup.DOFade(1, 1);
            healthTweener.OnComplete(OnHealthTextShowFinish);
        }
        //----------------------------------------------------------------------------
        private void OnHealthTextShowFinish()
        {
            Tweener healthTweener = m_healthGroup.DOFade(0, 2);
            healthTweener.OnComplete(OnHealthTextHideFinish);
        }
        //----------------------------------------------------------------------------
        private void OnHealthTextHideFinish()
        {
            OnDestroy();
            LauncherManager.Instance.ShowUIDownload();
        }
        //----------------------------------------------------------------------------
        public void OnDestroy()
        {
            if (m_logoTrans != null)
            {
                Object.DestroyImmediate(m_logoTrans.gameObject);
            }
        }
    }
}
