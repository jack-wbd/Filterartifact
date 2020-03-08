//------------------------------------------------------------------------------
/**
	\file	ScreenUnit.cs

	Copyright (c) 2019, BoYue. All rights reserved.

	<PRE>

	模块名称：<文件所属的模块名称>
	文件名称：ScreenUnit.cs
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
//	ScreenUnit.cs
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

    //----------------------------------------------------------------------------
    public class ScreenUnit
    {
        public static int width = Screen.width;
        public static int height = Screen.height;
        public static float fScreenScale = (float)width / height;
        private CanvasScaler m_rootCanvasScale = null;
        //----------------------------------------------------------------------------
        public Vector3 GetAstrictScreenVec3()
        {
            int fw = GetFullWidth();
            float radio = (float)9 / 16;
            return new Vector3(fw, fw * radio, 0);
        }
        //----------------------------------------------------------------------------
        public Vector3 GetFullVec3()
        {
            int height = GetFullHeight();
            int width = GetFullWidth();
            return new Vector3(width, height, 0);
        }
        //----------------------------------------------------------------------------
        public int GetFullHeight()
        {
            int height = Mathf.CeilToInt(GetUIRootActiveHeight());
            return height;
        }
        //----------------------------------------------------------------------------
        public int GetFullWidth()
        {
            int width = Mathf.CeilToInt(fScreenScale * GetUIRootActiveWidth());
            return width;
        }
        //----------------------------------------------------------------------------
        private float GetUIRootActiveHeight()
        {
            float fHeight = 720;
            if (m_rootCanvasScale == null)
            {
                m_rootCanvasScale = GameObject.Find("ui_root/UICamera/Canvas").GetComponent<CanvasScaler>();
            }
            if (m_rootCanvasScale != null)
            {
                fHeight = m_rootCanvasScale.referenceResolution.y;

            }
            return fHeight;
        }
        //----------------------------------------------------------------------------
        private float GetUIRootActiveWidth()
        {
            float fWidth = 960;
            if (m_rootCanvasScale == null)
            {
                m_rootCanvasScale = GameObject.Find("m_root/Camera/Canvas").GetComponent<CanvasScaler>();
            }
            if (m_rootCanvasScale != null)
            {
                fWidth = m_rootCanvasScale.referenceResolution.x;

            }
            return fWidth;
        }
        //----------------------------------------------------------------------------
    }
}
