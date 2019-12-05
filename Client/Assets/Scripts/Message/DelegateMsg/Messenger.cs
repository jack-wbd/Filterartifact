//------------------------------------------------------------------------------
/**
	\file	Messenger.cs

	Copyright (c) 2019, BoYue. All rights reserved.

	<PRE>

	模块名称：<事件注册基类>
	文件名称：Messenger.cs
	摘    要：<委托注册广播基类>

	当前版本：1.0
	建立日期：2019/11/28
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
//	Messenger.cs
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Filterartifact
{
    static public class Messenger
    {
        //----------------------------------------------------------------------------
        #region Internal variables
        static public Dictionary<int, Delegate> eventTable = new Dictionary<int, Delegate>();
        static public List<DgMsgID> permanentMessages = new List<DgMsgID>();
        #endregion
        //----------------------------------------------------------------------------
        #region AddListener
        //No parameters
        //----------------------------------------------------------------------------
        static public void AddListener(DgMsgID eventType, Callback handler)
        {
            OnListenerAdding(eventType, handler);
            eventTable[(int)eventType] = (Callback)eventTable[(int)eventType] + handler;
        }
        //One parameters
        //----------------------------------------------------------------------------
        static public void AddListener<T>(DgMsgID eventType, Callback<T> handler)
        {
            OnListenerAdding(eventType, handler);
            eventTable[(int)eventType] = (Callback<T>)eventTable[(int)eventType] + handler;
        }
        //Two parameters
        //----------------------------------------------------------------------------
        static public void AddListener<T, U>(DgMsgID eventType, Callback<T, U> handler)
        {
            OnListenerAdding(eventType, handler);
            eventTable[(int)eventType] = (Callback<T, U>)eventTable[(int)eventType] + handler;
        }
        //Three parameters
        //----------------------------------------------------------------------------
        static public void AddListener<T, U, V>(DgMsgID eventType, Callback<T, U, V> handler)
        {
            OnListenerAdding(eventType, handler);
            eventTable[(int)eventType] = (Callback<T, U, V>)eventTable[(int)eventType] + handler;
        }
        #endregion
        #region RemoveListener
        //----------------------------------------------------------------------------
        //No parameters
        //----------------------------------------------------------------------------
        static public void RemoveListener(DgMsgID eventType, Callback handler)
        {
            if (eventTable.ContainsKey((int)eventType))
            {
                OnListenerRemoving(eventType, handler);
                eventTable[(int)eventType] = (Callback)eventTable[(int)eventType] - handler;
                OnListenerRemoved(eventType);
            }
        }
        //One parameters
        //----------------------------------------------------------------------------
        static public void RemoveListener<T>(DgMsgID eventType, Callback<T> handler)
        {
            OnListenerRemoving(eventType, handler);
            eventTable[(int)eventType] = (Callback<T>)eventTable[(int)eventType] - handler;
            OnListenerRemoved(eventType);
        }
        //Two parameters
        //----------------------------------------------------------------------------
        static public void RemoveListener<T, U>(DgMsgID eventType, Callback<T, U> handler)
        {
            OnListenerRemoving(eventType, handler);
            eventTable[(int)eventType] = (Callback<T, U>)eventTable[(int)eventType] - handler;
            OnListenerRemoved(eventType);
        }
        //Three parameters
        //----------------------------------------------------------------------------
        static public void RemoveListener<T, U, V>(DgMsgID eventType, Callback<T, U, V> handler)
        {
            OnListenerRemoving(eventType, handler);
            eventTable[(int)eventType] = (Callback<T, U, V>)eventTable[(int)eventType] - handler;
            OnListenerRemoved(eventType);
        }
        //----------------------------------------------------------------------------
        static private void OnListenerRemoving(DgMsgID eventType, Delegate listenerBeingRemoved)
        {
            if (eventTable.ContainsKey((int)eventType))
            {
                Delegate d = eventTable[(int)eventType];
                if (d == null)
                {
                    Debug.LogWarning(string.Format("Attempting to remove listener with for event type \"{0}\" but current listener is null.", eventType));
                }
                else if (d.GetType() != listenerBeingRemoved.GetType())
                {
                    Debug.LogWarning(string.Format("Attempting to remove listener with inconsistent signature for event type {0}. Current listeners have type {1} and listener being removed has type {2}", eventType, d.GetType().Name, listenerBeingRemoved.GetType().Name));
                }
            }
            else
            {
                Debug.LogWarning(string.Format("Attempting to remove listener for type \"{0}\" but Messenger doesn't know about this event type.", eventType));
            }
        }
        //----------------------------------------------------------------------------
        static private void OnListenerRemoved(DgMsgID eventType)
        {
            if (eventTable[(int)eventType] == null)
            {
                eventTable.Remove((int)eventType);
            }
        }
        //----------------------------------------------------------------------------
        #endregion
        //----------------------------------------------------------------------------
        #region  OnListenerAdding
        static private void OnListenerAdding(DgMsgID eventType, Delegate listenerBeingAdded)
        {
            if (!eventTable.ContainsKey((int)eventType))
            {
                eventTable.Add((int)eventType, null);
            }
            Delegate d = eventTable[(int)eventType];
            if (d != null && d.GetType() != listenerBeingAdded.GetType())
            {
                Debug.LogError(string.Format("Attempting to add listener with inconsistent signature for event type {0}. " + "Current listeners have type {1} and listener being added has type {2}", eventType, d.GetType().Name, listenerBeingAdded.GetType().Name));
            }
        }
        #endregion
        //----------------------------------------------------------------------------
        #region Broadcast
        //No parameters
        //----------------------------------------------------------------------------
        static public void Broadcast(DgMsgID eventType)
        {
            Delegate d;
            if (eventTable.TryGetValue((int)eventType, out d))
            {
                (d as Callback)?.Invoke();
            }
            else
            {
                CreateBroadcastSingnatureException(eventType);

            }
        }
        //One parameters
        //----------------------------------------------------------------------------
        static public void Broadcast<T>(DgMsgID eventType, T arg1)
        {
            Delegate d;
            if (eventTable.TryGetValue((int)eventType, out d))
            {
                (d as Callback<T>)?.Invoke(arg1);
            }
            else
            {
                CreateBroadcastSingnatureException(eventType);
            }
        }
        //Two parameters
        //----------------------------------------------------------------------------
        static public void Broadcast<T, U>(DgMsgID eventType, T arg1, U arg2)
        {
            Delegate d;
            if (eventTable.TryGetValue((int)eventType, out d))
            {
                (d as Callback<T, U>)?.Invoke(arg1, arg2);
            }
            else
            {
                CreateBroadcastSingnatureException(eventType);
            }
        }
        //Three parameters
        //----------------------------------------------------------------------------
        static public void Broadcast<T, U, V>(DgMsgID eventType, T arg1, U arg2, V arg3)
        {
            Delegate d;
            if (eventTable.TryGetValue((int)eventType, out d))
            {
                (d as Callback<T, U, V>)?.Invoke(arg1, arg2, arg3);
            }
            else
            {
                CreateBroadcastSingnatureException(eventType);
            }
        }
        //----------------------------------------------------------------------------
        #endregion

        #region CreateBroadcastSingnatureException
        //----------------------------------------------------------------------------
        static public void CreateBroadcastSingnatureException(DgMsgID eventType)
        {
            Debug.LogError(string.Format("Broadcasting message \"{0}\" but listeners have a different signature than the broadcaster.", eventType));
        }
        #endregion
    }
}

