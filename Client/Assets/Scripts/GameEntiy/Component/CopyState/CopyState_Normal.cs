//------------------------------------------------------------------------------
/**
	\file	CopyState_Normal.cs

	Copyright (c) 2020, BoYue. All rights reserved.

	<PRE>

	模块名称：<文件所属的模块名称>
	文件名称：CopyState_Normal.cs
	摘    要：<描述该文件实现的主要功能>

	当前版本：1.0
	建立日期：2020/3/18
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
//	CopyState_Normal.cs
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filterartifact
{
    class CopyState_Normal:CopyState
    {
        //----------------------------------------------------------------------------
        public CopyState_Normal(int stateid,State parent):base(stateid,parent)
        {

        }
        //----------------------------------------------------------------------------
        public CopyState_Normal(int stateid,State parent,CopyStateCtrl baseCtrl):base(stateid,parent,baseCtrl)
        {

        }
        //----------------------------------------------------------------------------
        protected override void OnStateInit()
        {
            base.OnStateInit();
        }
        //----------------------------------------------------------------------------
    }
}
