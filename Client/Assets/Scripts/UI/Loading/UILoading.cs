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
            }
            return true;
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
        }
        //----------------------------------------------------------------------------
        private void ShowView()
        {

        }
        //----------------------------------------------------------------------------
        //----------------------------------------------------------------------------
        //----------------------------------------------------------------------------
    }
}
