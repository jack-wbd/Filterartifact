//------------------------------------------------------------------------------
/**
	\file	CopyStateCtrl.cs

	Copyright (c) 2020, BoYue. All rights reserved.

	<PRE>

	模块名称：<文件所属的模块名称>
	文件名称：CopyStateCtrl.cs
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
//	CopyStateCtrl.cs
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filterartifact
{
    public class CopyStateCtrl : GameComponent
    {
        //----------------------------------------------------------------------------
        protected CopyState m_copyState;
        public CopyState _copyState { get { return m_copyState; } }

        //----------------------------------------------------------------------------
        public override void Init(object obj)
        {
            base.Init(obj);
        }
        //----------------------------------------------------------------------------
        public override void Update()
        {
            base.Update();
            if (m_copyState != null)
            {
                m_copyState.Update();
            }
        }
        //----------------------------------------------------------------------------
        public override void Destroy()
        {
            base.Destroy();
            m_copyState.GotoSubState((int)CopyStateType.CST_NONE);
            m_copyState.Destory();
            m_copyState = null;
        }
        //----------------------------------------------------------------------------
    }
    //----------------------------------------------------------------------------
    public class CopyMainState : CopyState
    {
        //----------------------------------------------------------------------------
        //var
        public CopyMainState(int stateid, State parent) : base(stateid, parent)
        {

        }
        //----------------------------------------------------------------------------
        public CopyMainState(int stateid, State parent, CopyStateCtrl baseCtrl)
            : base(stateid, parent, baseCtrl)
        {

        }
        //----------------------------------------------------------------------------
        protected override void OnStateInit()
        {
            base.OnStateInit();
        }
        //----------------------------------------------------------------------------
        protected override void OnStateDestroy()
        {
            base.OnStateDestroy();
        }
        //----------------------------------------------------------------------------
        protected override void OnStateBegin(object[] parameter)
        {
            base.OnStateBegin(parameter);
        }
        //----------------------------------------------------------------------------
        protected override void OnUpdate()
        {
            base.OnUpdate();
        }
        //----------------------------------------------------------------------------
        protected override void OnStateEnd()
        {
            base.OnStateEnd();
        }
        //----------------------------------------------------------------------------

        //----------------------------------------------------------------------------
    }
    //----------------------------------------------------------------------------

}
