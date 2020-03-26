//------------------------------------------------------------------------------
/**
	\file	DebugUI.cs

	Copyright (c) 2020, BoYue. All rights reserved.

	<PRE>

	模块名称：<文件所属的模块名称>
	文件名称：DebugUI.cs
	摘    要：<描述该文件实现的主要功能>

	当前版本：1.0
	建立日期：2020/3/26
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
//	DebugUI.cs
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Filterartifact
{
    [Obsolete("Use UIGMMainCtrl")]
    public class DebugUI : MonoBehaviour
    {
        //----------------------------------------------------------------------------
        void Start()
        {

        }
        //----------------------------------------------------------------------------
        void Update()
        {
            CheckAllUI();
        }
        //----------------------------------------------------------------------------
        private void CheckAllUI()
        {
            if (MulPress_GUI() || MulKey_GUI())
            {
                //show GM UI
            }
        }
        //----------------------------------------------------------------------------
        private bool MulPress_GUI()
        {
            if (Input.touchCount == 3)
            {
                bool bOK = true;
                for (int i = 0; i < 2; ++i)
                {
                    Touch touch = Input.GetTouch(i);
                    if (touch.phase == TouchPhase.Began ||
                        touch.phase == TouchPhase.Moved ||
                        touch.phase == TouchPhase.Stationary)
                    {
                        continue;
                    }
                    bOK = false;
                }

                if (bOK)
                {
                    Touch touch = Input.GetTouch(2);
                    if (touch.phase == TouchPhase.Began)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        //----------------------------------------------------------------------------
        private bool MulKey_GUI()
        {
            if (Input.GetKey(KeyCode.Alpha4) && Input.GetKeyDown(KeyCode.Alpha5))
            {
                return true;
            }
            return false;
        }
        //----------------------------------------------------------------------------
        //----------------------------------------------------------------------------
        //----------------------------------------------------------------------------
    }
}
