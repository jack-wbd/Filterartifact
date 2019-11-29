//------------------------------------------------------------------------------
/**
	\file	State.cs

	Copyright (c) 2019, BoYue. All rights reserved.

	<PRE>

	模块名称：<文件所属的模块名称>
	文件名称：State.cs
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
//	State.cs
//------------------------------------------------------------------------------
using System.Collections.Generic;
using UnityEngine;

namespace Filterartifact
{
    public class State : IMsgPipe
    {
        //member variable
        //----------------------------------------------------------------------------
        private Dictionary<int, State> SubStateDict;
        private Dictionary<int, State>.Enumerator itor;
        protected State _parentState = null;
        protected State _curSubState = null;

        public int LastSubStateID { get; set; }
        public int StateID { get; set; }
        public int NextSubStateID { get; set; }
        // constructor function
        //----------------------------------------------------------------------------
        public State(int stateid, State parent)
        {
            LastSubStateID = -1;
            NextSubStateID = -1;
            StateID = stateid;
            _parentState = parent;
            SubStateDict = new Dictionary<int, State>();
        }
        //----------------------------------------------------------------------------
        //protected member function
        protected virtual void OnStateInit()
        {
            _curSubState = null;
            itor = SubStateDict.GetEnumerator();//返回该对象以遍历字典中元素
            while (itor.MoveNext())
            {
                itor.Current.Value.OnStateInit();
            }
        }
        //----------------------------------------------------------------------------
        protected virtual void OnStateInit(object parameter)
        {
            _curSubState = null;
            itor = SubStateDict.GetEnumerator();
            while (itor.MoveNext())
            {

                itor.Current.Value.OnStateInit(parameter);
            }
        }
        //----------------------------------------------------------------------------
        protected virtual void OnStateDestroy()
        {
            _curSubState = null;
            itor = SubStateDict.GetEnumerator();
            while (itor.MoveNext())
            {
                itor.Current.Value.OnStateDestroy();
            }
        }
        //----------------------------------------------------------------------------
        protected virtual void OnStateBegin(object[] parameter)
        {

        }
        //----------------------------------------------------------------------------
        protected virtual void OnStateEnd()
        {

        }
        //----------------------------------------------------------------------------
        protected virtual void OnUpdate()
        {

        }
        //----------------------------------------------------------------------------
        protected virtual void OnSubStateChanged(object[] commond)
        {

        }
        //----------------------------------------------------------------------------
        protected void BeginState(object[] parameter)
        {
            OnStateBegin(parameter);
        }
        //----------------------------------------------------------------------------
        protected void EndState()
        {
            if (!ReferenceEquals(_curSubState, null))
            {
                _curSubState.EndState();
            }
            OnStateEnd();
        }
        //----------------------------------------------------------------------------
        protected void AddSubState(State substate)
        {
            SubStateDict.Add(substate.StateID, substate);
        }
        //----------------------------------------------------------------------------
        protected void RemoveSubState(State substate)
        {
            SubStateDict.Remove(substate.StateID);
        }
        //----------------------------------------------------------------------------
        protected bool SetSubState(int stateid, object[] parameter)
        {
            State state;
            if (SubStateDict.TryGetValue(stateid, out state))
            {
                if (!ReferenceEquals(_curSubState, null))
                {
                    LastSubStateID = _curSubState.StateID;
                    _curSubState.EndState();
                }
                NextSubStateID = state.StateID;
                _curSubState = state;
                _curSubState.BeginState(parameter);
                OnSubStateChanged(parameter);
                NextSubStateID = -1;
                return true;
            }
            return false;
        }
        //----------------------------------------------------------------------------
        public State GetCurState()
        {
            return _curSubState;
        }
        //----------------------------------------------------------------------------
        protected State GetParentState()
        {
            return _parentState;
        }
        //----------------------------------------------------------------------------
        //public member function
        //----------------------------------------------------------------------------
        public void Init()
        {
            OnStateInit();
        }
        //----------------------------------------------------------------------------
        public void Init(object parameter)
        {
            OnStateInit(parameter);
        }
        //----------------------------------------------------------------------------
        public void Destory()
        {
            OnStateDestroy();
        }
        //----------------------------------------------------------------------------
        public void Update()
        {
            OnUpdate();
            if (ReferenceEquals(_curSubState, null))
            {
                SetSubState(NextSubStateID, null);
            }
            else if (NextSubStateID != _curSubState.StateID)
            {
                SetSubState(NextSubStateID, null);
            }
            if (!ReferenceEquals(_curSubState, null))
            {
                _curSubState.Update();
            }
        }
        //----------------------------------------------------------------------------
        protected int GetCurSubStateID()
        {
            if (ReferenceEquals(_curSubState, null))
            {
                return -1;
            }
            return _curSubState.StateID;
        }
        //----------------------------------------------------------------------------
        //override function
        //----------------------------------------------------------------------------
        public virtual void OnAnimateEnd(string strName)
        {
            if (ReferenceEquals(_curSubState, null))
            {
                return;
            }
            _curSubState.OnAnimateEnd(strName);
        }
        //----------------------------------------------------------------------------
        public virtual void DirectInput(Vector3 dir, bool bIsRun = true)
        {
            if (ReferenceEquals(_curSubState, null))
            {
                return;
            }
            _curSubState.DirectInput(dir, bIsRun);
        }
        //----------------------------------------------------------------------------
        public override void PlugInMsgPipe(IMsgPipe thePipe)
        {
            itor = SubStateDict.GetEnumerator();
            while (itor.MoveNext())
            {
                if (ReferenceEquals(itor.Current.Value, thePipe))
                {
                    Debug.LogWarning("子状态不能插到父亲身上");
                    return;
                }

            }
            base.PlugInMsgPipe(thePipe);
        }
        //----------------------------------------------------------------------------
        protected override bool DispatchMsg_Up(IMsg msg)
        {
            bool bResult = false;
            bResult = base.DispatchMsg_Up(msg);
            if (!bResult && !ReferenceEquals(_parentState, null))
            {
                bResult = _parentState.ProcessMsg(msg);
            }
            return bResult;
        }
        //----------------------------------------------------------------------------
        protected override bool DispatchMsg_Down(IMsg msg, IMsgPipe except = null)
        {
            bool bResult = false;
            bResult = base.DispatchMsg_Down(msg, except);
            if (!bResult && ReferenceEquals(_curSubState, null) && @ReferenceEquals(_curSubState, except))
            {
                bResult = _curSubState.ProcessMsg(msg);
            }
            return bResult;
        }
        //----------------------------------------------------------------------------
        //----------------------------------------------------------------------------
        //----------------------------------------------------------------------------
        //----------------------------------------------------------------------------
    }
}
