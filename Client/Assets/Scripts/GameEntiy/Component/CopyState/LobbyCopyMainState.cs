//------------------------------------------------------------------------------
/**
	\file	LobbyCopyMainState.cs

	Copyright (c) 2020, BoYue. All rights reserved.

	<PRE>

	模块名称：<文件所属的模块名称>
	文件名称：LobbyCopyMainState.cs
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
//	LobbyCopyMainState.cs
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Filterartifact
{
    class LobbyCopyMainState : CopyMainState
    {
        //----------------------------------------------------------------------------
        private float m_fLastTime = 0f;
        private bool m_bIsActive = false;
        //----------------------------------------------------------------------------
        public LobbyCopyMainState(int stateid, State parent) : base(stateid, parent)
        {

        }
        //----------------------------------------------------------------------------
        public LobbyCopyMainState(int stateid, State parent, CopyStateCtrl baseCtrl)
            : base(stateid, parent, baseCtrl)
        {


        }
        //----------------------------------------------------------------------------
        protected override void OnStateInit()
        {
            base.OnStateInit();
            RegisterMsg((int)MsgID.MSG_LOADING_ALLREADY, OnLoadingAllReady);
            AddSubState(CopyStateFactory.CreateState(CopyStateType.CST_LOADING, this));
            AddSubState(CopyStateFactory.CreateState(CopyStateType.CST_LOBBY_NORMAL, this));
            SetSubState((int)CopyStateType.CST_LOADING, null);
        }
        //----------------------------------------------------------------------------
        private void OnLoadingAllReady()
        {
            m_bIsActive = true;
            m_fLastTime = 1f;
        }
        //----------------------------------------------------------------------------
        protected override void OnStateDestroy()
        {
            base.OnStateDestroy();
        }
        //----------------------------------------------------------------------------
        protected override void OnUpdate()
        {
            base.OnUpdate();
            if (m_bIsActive)
            {
                if (m_fLastTime > 0)
                {
                    m_fLastTime -= Time.deltaTime;
                    return;
                }
                m_fLastTime = 0f;
                m_bIsActive = false;
                _OnLoadingAllReady();
            }
        }
        //----------------------------------------------------------------------------
        private void _OnLoadingAllReady()
        {
            if (GetCurrentChildStateID() != CopyStateType.CST_LOBBY_NORMAL)
            {
                GotoSubState((int)CopyStateType.CST_LOBBY_NORMAL);
            }
        }
        //----------------------------------------------------------------------------

        //----------------------------------------------------------------------------

    }
}
