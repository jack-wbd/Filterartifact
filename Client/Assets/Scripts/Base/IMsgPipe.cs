//------------------------------------------------------------------------------
/**
	\file	IMsgPipe.cs

	Copyright (c) 2019, BoYue. All rights reserved.

	<PRE>

	模块名称：<文件所属的模块名称>
	文件名称：IMsgPipe.cs
	摘    要：<描述该文件实现的主要功能>

	当前版本：1.0
	建立日期：2019/11/29
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
//	IMsgPipe.cs
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Filterartifact
{
    public delegate void MsgCallback();
    public delegate void MsgCallback<T>(T arg1);
    public delegate void MsgCallback<T, U>(T arg1, U arg2);
    public delegate void MsgCallback<T, U, V>(T arg1, U arg2, V arg3);
    public delegate void MsgCallback<T, U, V, X>(T arg1, U arg2, V arg3, X arg4);
    public class IMsgPipe
    {
        //variable
        //----------------------------------------------------------------------------
        private Dictionary<int, MethodInfo> m_dictMsgFunc = null;
        private bool m_bDoEventReturn = true;
        private bool m_bSwitchMsgUp2Down = false;
        protected IMsgPipe m_pParentPipe;
        protected List<IMsgPipe> m_listChildPipe = null;
        private Dictionary<int, Delegate> m_dictMsgCall = null;
        //function
        //----------------------------------------------------------------------------
        virtual public bool ProcessMsg(IMsg msg, IMsgPipe fromWhere = null)
        {
            bool bResult = false;
            //处理消息
            bResult = ExecuteMsg(msg);
            //上转下
            if (!bResult)
            {
                bResult = SwitchMsgUp2Down(msg, fromWhere);
            }
            //分发
            if (!bResult)
            {
                bResult = DispatchMsg(msg);
                return bResult;
            }
            return bResult;
        }
        //---------------------------------------无参数-------------------------------
        public void RegisterMsg(int nMsgID, MsgCallback callback)
        {
            if (callback != null)
            {
                AddMsg(nMsgID, callback);
            }
        }
        //----------------------------------------------------------------------------
        public void RegisterMsg(int nMsgID, string strFuncName)
        {
            MethodInfo info = GetType().GetMethod(strFuncName);
            if (info != null)
            {
                if (m_dictMsgFunc == null)
                {
                    m_dictMsgFunc = new Dictionary<int, MethodInfo>();
                }
                if (m_dictMsgFunc.ContainsKey(nMsgID))
                {
                    Debug.LogError("the same ID already exists!" + nMsgID);
                }
                else
                    m_dictMsgFunc.Add(nMsgID, info);
            }
        }
        //----------------------------------------------------------------------------
        virtual public bool ProcessMsg(int msgId, MsgDispatchDir direction = MsgDispatchDir.MDD_Down, IMsgPipe fromWhere = null)
        {
            bool bResult = false;
            //处理消息
            bResult = ExecuteMsg(msgId);
            //上转下
            if (!bResult)
            {
                bResult = SwitchMsgUp2Down(msgId, ref direction, fromWhere);
            }
            //分发
            if (!bResult)
            {
                bResult = DispatchMsg(msgId, direction);
            }
            return bResult;

        }
        //----------------------------------------------------------------------------
        protected bool DispatchMsg(int msgId, MsgDispatchDir direction = MsgDispatchDir.MDD_Down)
        {
            bool bResult = false;
            if (direction == MsgDispatchDir.MDD_UP)
            {
                DispatchMsg_Up(msgId, direction);
            }
            else if (direction == MsgDispatchDir.MDD_Down)
            {
                DispatchMsg_Down(msgId, direction);
            }
            else
                Debug.LogError("Direction Error");
            return bResult;
        }
        //----------------------------------------------------------------------------
        protected virtual bool DispatchMsg_Up(int msgId, MsgDispatchDir direction = MsgDispatchDir.MDD_Down)
        {
            bool bResult = false;
            if (m_pParentPipe != null)
            {
                bResult = m_pParentPipe.ProcessMsg(msgId, direction, this);
            }
            return bResult;
        }
        //----------------------------------------------------------------------------
        protected bool SwitchMsgUp2Down(int msgId, ref MsgDispatchDir direction, IMsgPipe fromWhere)
        {
            if (direction != MsgDispatchDir.MDD_UP || !m_bSwitchMsgUp2Down)
            {
                return false;
            }
            bool bResult = false;
            direction = MsgDispatchDir.MDD_Down;
            bResult = DispatchMsg_Down(msgId, direction, fromWhere);
            direction = MsgDispatchDir.MDD_UP;
            return bResult;
        }
        protected virtual bool DispatchMsg_Down(int msgId, MsgDispatchDir direction, IMsgPipe except = null)
        {
            bool bResult = false;
            if (m_listChildPipe == null)
            {
                return bResult;
            }

            for (int i = 0; i < m_listChildPipe.Count; ++i)
            {
                if (m_listChildPipe[i] != except)
                {
                    bResult = m_listChildPipe[i].ProcessMsg(msgId, direction, this);
                    if (bResult)
                    {
                        break;
                    }
                }
            }
            return bResult;
        }
        //-----------------------------------T,U--------------------------------------
        public void RegisterMsg<T, U>(int nMsgID, MsgCallback<T, U> callback)
        {
            if (callback != null)
            {

            }
        }
        //----------------------------------------------------------------------------
        private void AddMsg(int nMsgID, Delegate callback)
        {
            if (m_dictMsgCall == null)
            {
                m_dictMsgCall = new Dictionary<int, Delegate>();
            }
            if (m_dictMsgCall.ContainsKey(nMsgID))
            {
                Debug.LogWarning("the same ID already exists!");
                m_dictMsgCall.Remove(nMsgID);
                m_dictMsgCall.Add(nMsgID, callback);
            }
            else
            {
                m_dictMsgCall.Add(nMsgID, callback);
            }

        }
        //----------------------------------------------------------------------------
        protected bool DispatchMsg(IMsg msg)
        {

            bool bResult = false;
            if (msg.m_Direction == MsgDispatchDir.MDD_UP)
            {
                DispatchMsg_Up(msg);
            }
            else if (msg.m_Direction == MsgDispatchDir.MDD_Down)
            {
                DispatchMsg_Down(msg);
            }
            else
                Debug.LogError("Direction error");
            return bResult;

        }
        //----------------------------------------------------------------------------
        protected virtual bool DispatchMsg_Up(IMsg msg)
        {
            bool bResult = false;
            if (m_pParentPipe != null)
            {
                bResult = m_pParentPipe.ProcessMsg(msg, this);
            }
            return bResult;
        }
        //----------------------------------------------------------------------------
        protected bool SwitchMsgUp2Down(IMsg msg, IMsgPipe fromWhere)
        {
            if (msg.m_Direction != MsgDispatchDir.MDD_UP || !m_bSwitchMsgUp2Down)
            {
                return false;
            }
            bool bResult = false;
            msg.m_Direction = MsgDispatchDir.MDD_Down;
            bResult = DispatchMsg_Down(msg, fromWhere);
            msg.m_Direction = MsgDispatchDir.MDD_UP;
            return bResult;
        }
        //----------------------------------------------------------------------------
        protected virtual bool DispatchMsg_Down(IMsg msg, IMsgPipe except = null)
        {
            bool bResult = false;
            if (m_listChildPipe == null)
            {
                return bResult;
            }
            int nChildNum = m_listChildPipe.Count;
            for (int i = 0; i < nChildNum; i++)
            {
                if (m_listChildPipe[i] != except)
                {
                    bResult = m_listChildPipe[i].ProcessMsg(msg, this);
                    if (bResult)
                    {
                        break;
                    }
                }
            }
            return bResult;
        }
        //----------------------------------------------------------------------------
        protected bool ExecuteMsg(IMsg msg)
        {
            bool bResult = FindAndExecute(msg);
            SetDoEventReturnFlag(true);
            return bResult;
        }
        //----------------------------------------------------------------------------
        protected bool ExecuteMsg(int msg)
        {
            bool bResult = FindAndExecute(msg);
            SetDoEventReturnFlag(true);
            return bResult;
        }
        //----------------------------------------------------------------------------
        protected bool FindAndExecute(int msgId)
        {
            if (m_dictMsgCall == null)
            {
                return false;
            }
            Delegate d;
            if (m_dictMsgCall.TryGetValue(msgId, out d))
            {
                MsgCallback callback = d as MsgCallback;
                if (callback != null)
                {
                    callback();
                    return GetDoEventReturnFlag();
                }
                else
                {
                    Debug.LogErrorFormat("msgId:{0} callback convert failed", msgId);
                }
            }

            return false;

        }
        //----------------------------------------------------------------------------
        public void SetDoEventReturnFlag(bool bFlag)
        {
            m_bDoEventReturn = bFlag;
        }
        //----------------------------------------------------------------------------
        protected bool FindAndExecute(IMsg msg)
        {
            if (m_dictMsgFunc == null)
            {
                return false;
            }
            if (m_dictMsgFunc.ContainsKey(msg.m_nMsgID))
            {
                if (msg.m_TypeCount == MsgParamCount.one)
                {
                    m_dictMsgFunc[msg.m_nMsgID].Invoke(this, new object[] { msg.m_objOne });
                }
                else if (msg.m_TypeCount == MsgParamCount.two)
                {
                    m_dictMsgFunc[msg.m_nMsgID].Invoke(this, new object[] { msg.m_objOne, msg.m_objTwo });
                }
                else if (msg.m_TypeCount == MsgParamCount.more)
                {
                    m_dictMsgFunc[msg.m_nMsgID].Invoke(this, new object[] { msg.m_objOne, msg.m_objTwo, msg.m_listObj });
                }
                else
                {
                    m_dictMsgFunc[msg.m_nMsgID].Invoke(this, null);
                }
                return GetDoEventReturnFlag();

            }
            return false;
        }
        //----------------------------------------------------------------------------
        public bool GetDoEventReturnFlag()
        {
            return m_bDoEventReturn;
        }
        //----------------------------------------------------------------------------
        public virtual void PlugInMsgPipe(IMsgPipe thePipe)
        {
            if (thePipe == null)
            {
                return;
            }
            if (thePipe.m_pParentPipe != null)
            {
                if (thePipe.m_pParentPipe == this)
                {
                    Debug.LogWarning("already Plug In!");
                    return;
                }
                thePipe.m_pParentPipe.PullOutMsgPipe(thePipe);
            }
            thePipe.m_pParentPipe = this;
            if (m_listChildPipe == null)
            {
                m_listChildPipe = new List<IMsgPipe>();
            }
            m_listChildPipe.Add(thePipe);
        }
        //----------------------------------------------------------------------------
        public void PullOutMsgPipe(IMsgPipe thePipe)
        {
            if (thePipe == null)
            {
                return;
            }
            if (thePipe.m_pParentPipe != null)
            {
                Debug.LogWarning("not exists");
                return;
            }
            if (m_listChildPipe != null && m_listChildPipe.Contains(thePipe))
            {
                thePipe.m_pParentPipe = null;
                m_listChildPipe.Remove(thePipe);
            }

        }
        //----------------------------------------------------------------------------
    }
    //----------------------------------------------------------------------------
    public class IMsg
    {
        public MsgParamCount m_TypeCount;
        public int m_nMsgID;
        public object m_objOne;
        public object m_objTwo;
        public object[] m_listObj;
        public MsgDispatchDir m_Direction;
    }
    //----------------------------------------------------------------------------
    public enum MsgParamCount
    {
        none,
        one,
        two,
        more
    }
    //----------------------------------------------------------------------------
    public enum MsgDispatchDir
    {
        MDD_UP,
        MDD_Down
    }
    //----------------------------------------------------------------------------
}
