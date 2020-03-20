//------------------------------------------------------------------------------
/**
	\file	CopyState.cs

	Copyright (c) 2020, BoYue. All rights reserved.

	<PRE>

	模块名称：<文件所属的模块名称>
	文件名称：CopyState.cs
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
//	CopyState.cs
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filterartifact
{
    public class CopyState : State
    {
        //----------------------------------------------------------------------------
        private CopyStateCtrl _stateCtrl;
        public bool m_bEnterFirst = true;//是否是第一次进入(默认是)
        //----------------------------------------------------------------------------
        public CopyState(CopyStateCtrl baseCtrl) : base(-1, null)
        {
            _stateCtrl = baseCtrl;
        }
        //----------------------------------------------------------------------------
        public CopyState(int stateid =-1,State parent=null,CopyStateCtrl baseCtrl =null):base(stateid,parent)
        {
            _stateCtrl = baseCtrl;
        }
        //----------------------------------------------------------------------------
        public void SetNextSubState(int stateid)
        {
            NextSubStateID = stateid;
        }
        //----------------------------------------------------------------------------
        public bool GotoSubState(int stateid)
        {
            return SetSubState(stateid, null);
        }
        //----------------------------------------------------------------------------
        public bool GotoSubState(int stateid,object[]command)
        {
            return SetSubState(stateid, command);
        }
        //----------------------------------------------------------------------------
        public bool GotoBrotherState(int stateid)
        {
            return GetParent().GotoSubState(stateid, null);
        }
        //----------------------------------------------------------------------------
        public bool GotoBrotherState(int stateid,object[] command)
        {
            return GetParent().GotoSubState(stateid, command);
        }
        //----------------------------------------------------------------------------
        public CopyStateType GetCurrentChildStateID()
        {
            return (CopyStateType)GetCurSubStateID();
        }
        //----------------------------------------------------------------------------
        public CopyStateType GetLastChildStateID()
        {
            return (CopyStateType)LastSubStateID;
        }
        //----------------------------------------------------------------------------
        protected CopyState GetParent()
        {
            return (CopyState)GetParentState();
        }
        //----------------------------------------------------------------------------
    }
}
