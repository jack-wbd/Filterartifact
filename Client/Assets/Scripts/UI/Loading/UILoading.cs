//------------------------------------------------------------------------------
/**
	\file	UILoading.cs

	Copyright (c) 2020, BoYue. All rights reserved.

	<PRE>

	模块名称�?文件所属的模块名称>
	文件名称：UILoading.cs
	�?   要：<描述该文件实现的主要功能>

	当前版本�?.0
	建立日期�?020/3/3
	�?   者：SYSTEM
	电子邮件�?username%@BoYue.com
	�?   注：<其它说明>

	</PRE>
*/
//------------------------------------------------------------------------------
// Using
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
// Class
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	UILoading.cs
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
    class UILoading : UIBase
    {
        private RawImage m_pic;
        private UILoadingCtrl m_loadCtrl = null;
        private Slider m_slider = null;
        private int m_nLoadpicCount = 0;
        private int m_nNeedLoadPicCount = 0;
        private float m_fStepScale = 1f;

        //----------------------------------------------------------------------------
        protected override bool OnCreate()
        {
            bool bResult = GetUIObject();
            if (bResult)
            {

            }
            return bResult;
        }
        //----------------------------------------------------------------------------
        public new bool GetUIObject()
        {
            if (m_objUI != null)
            {
                m_pic = m_uiTrans.Find("pic").GetComponent<RawImage>();
                m_slider = m_uiTrans.Find("load_di").GetComponent<Slider>();
                Hide();
            }
            return true;
        }
        //----------------------------------------------------------------------------
        public override void SetCtrl(UIController ctrl)
        {
            base.SetCtrl(ctrl);
            m_loadCtrl = ctrl as UILoadingCtrl;
        }
        //----------------------------------------------------------------------------
        public override void Show(object arg = null)
        {
            base.Show(arg);
            if (arg != null)
            {
                UnityEngine.Object obj = m_system.GetAssetBundle((string)arg);
                if (obj as Texture != null)
                {
                    m_pic.texture = obj as Texture;
                }
            }
            UISystem.CurAssetCount = 0;
            LoadSceneBase.CurAssetCount = 0;
            m_loadCtrl.m_nCurLoadedCount = 0;
            m_loadCtrl.m_nNeedLoadedCount = 0;
            m_loadCtrl.m_nCurFramFlag = 0;
            m_fStepScale = 1f;
            m_slider.value = 0;

        }
        //----------------------------------------------------------------------------
        private void ShowView()
        {
            //Messenger.Broadcast(DgMsgID.DgUI_ShowNew, "UIMainInterfaceCtrl");
        }
        //----------------------------------------------------------------------------
        protected override void OnUpdate()
        {
            if (m_loadCtrl.m_bLoad)
            {
                m_loadCtrl.m_nCurLoadedCount = LoadSceneBase.CurAssetCount + UISystem.CurAssetCount;
                m_loadCtrl.m_nNeedLoadedCount = LoadSceneBase.MaxAssetCount + UISystem.MaxAssetCount;
                if (m_loadCtrl.m_nCurFramFlag < m_loadCtrl.m_nCurLoadedCount)
                {
                    m_fStepScale = (m_loadCtrl.m_nCurLoadedCount - m_loadCtrl.m_nCurLoadedCount) / 3;
                    m_loadCtrl.m_nCurFramFlag += m_fStepScale < 1 ? 3 : (int)m_fStepScale + 3;
                    m_slider.value = (float)m_loadCtrl.m_nCurFramFlag / m_loadCtrl.m_nNeedLoadedCount;
                }
                if (m_loadCtrl.m_nCurFramFlag >= m_loadCtrl.m_nNeedLoadedCount)
                {
                    m_loadCtrl.m_nCurFramFlag = m_loadCtrl.m_nNeedLoadedCount;
                    m_slider.value = 1;
                    m_loadCtrl.m_bLoad = true;
                    Messenger.Broadcast<bool>(DgMsgID.DgMsg_NtyLoadProsser_Finish, true);
                }
            }
        }
        //----------------------------------------------------------------------------
        public override void Hide()
        {
            base.Hide();
            if (IsShow)
            {
                LoadingUIManager.Instance().PreLoadLoadingTexture(null);
            }
            m_slider.value = 0;
            m_loadCtrl.m_nCurFramFlag = 0;
            m_loadCtrl.m_nNeedLoadedCount = 0;
        }
        //----------------------------------------------------------------------------
        private void OnResComplete()
        {
            Hide();
        }
        //----------------------------------------------------------------------------
    }
}
