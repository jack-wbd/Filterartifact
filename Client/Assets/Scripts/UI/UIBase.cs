//------------------------------------------------------------------------------
/**
	\file	UIBase.cs

	Copyright (c) 2019, BoYue. All rights reserved.

	<PRE>

	模块名称：<文件所属的模块名称>
	文件名称：UIBase.cs
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
//	UIBase.cs
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
    //----------------------------------------------------------------------------
    public enum eUIImpower
    {
        Default = 0,//默认，可以穿梭于任意层
        MainUI = 1,
        Window = 2, //窗口一级
        Panel = 3, //窗口二级
        Dialog = 4,//提示对话框
        Guid = 5, //引导
        Promt = 6,//提示层，反馈，战力等
        FightUI = 7, //战斗UI
    }
    //----------------------------------------------------------------------------
    public class UIBase : IMsgPipe
    {
        //----------------------------------------------------------------------------
        protected UIBase parent = null;
        public GameObject m_objUI;
        protected Transform m_uiTrans;
        public Image panel;
        protected UISystem m_system;
        static private bool useFullScreenBg = true;
        public string strAssetID = "";
        //----------------------------------------------------------------------------
        private eUIImpower _impower = eUIImpower.Default;
        public eUIImpower impower
        {
            set
            {
                _impower = value;
                InitDepth();
            }
            get { return _impower; }
        }
        //----------------------------------------------------------------------------
        public void SetParent(UIBase _parent)
        {
            parent = _parent;
        }
        //----------------------------------------------------------------------------
        public UIBase GetParent()
        {
            return parent;
        }
        //----------------------------------------------------------------------------
        public virtual void SetObjUI(GameObject objUI)
        {
            if (objUI == null)
            {
                Debug.LogError("SetObjUI objUI ==null");
                return;
            }
            m_objUI = objUI;
            m_uiTrans = m_objUI.transform;
            if (m_uiTrans == null)
            {
                Debug.LogError("SetObjUI m_uiTrans == null");
                return;
            }

            panel = m_uiTrans.GetComponent<Image>();
            InitLayout();
            InitAdaption();
            InitDepth();
        }
        //----------------------------------------------------------------------------
        private bool bInitLayout = false;
        //----------------------------------------------------------------------------
        protected virtual void InitLayout()
        {
            if (m_system == null)
            {
                m_system = UISystem.m_sys;
            }

        }
        //----------------------------------------------------------------------------
        protected virtual void InitDepth()
        {

        }
        //----------------------------------------------------------------------------
        protected virtual void InitAdaption()
        {
            Transform bgTF = m_uiTrans.Find("bgSpr");
            if (bgTF != null)
            {
                Vector3 vec3;
                if (!useFullScreenBg)
                {
                    vec3 = WorldManager.Instance().screenUnit.GetAstrictScreenVec3();
                }
                else
                {
                    vec3 = WorldManager.Instance().screenUnit.GetFullVec3();
                }
                RectTransform bgSpr = bgTF.GetComponent<RectTransform>();
                if (bgSpr != null)
                {
                    bgSpr.sizeDelta = new Vector2((int)vec3.x, (int)vec3.y);
                }
                BoxCollider boxCollider = bgTF.GetComponent<BoxCollider>();
                if (boxCollider != null)
                {
                    boxCollider.size = vec3;
                }
            }
        }
        //----------------------------------------------------------------------------
        //----------------------------------------------------------------------------
        //----------------------------------------------------------------------------

    }
}
