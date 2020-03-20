//------------------------------------------------------------------------------
/**
	\file	LobbyCopyStateCtrl.cs

	Copyright (c) 2020, BoYue. All rights reserved.

	<PRE>

	模块名称：<文件所属的模块名称>
	文件名称：LobbyCopyStateCtrl.cs
	摘    要：<描述该文件实现的主要功能>

	当前版本：1.0
	建立日期：2020/3/17
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
//	LobbyCopyStateCtrl.cs
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filterartifact
{
    class LobbyCopyStateCtrl : CopyStateCtrl
    {
        //----------------------------------------------------------------------------
        public override void Init(object obj)
        {
            base.Init(obj);
            m_copyState = new LobbyCopyMainState((int)CopyStateType.CST_MAIN, null, this);
            PlugInMsgPipe(m_copyState);
            m_copyState.Init();
        }
        //----------------------------------------------------------------------------
        public override void Update()
        {
            base.Update();
        }
        //----------------------------------------------------------------------------
        public override void Destroy()
        {
            base.Destroy();
        }
        //----------------------------------------------------------------------------
    }
}
