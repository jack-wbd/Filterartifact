//------------------------------------------------------------------------------
/**
	\file	IBase.cs

	Copyright (c) 2020, BoYue. All rights reserved.

	<PRE>

	模块名称�?文件所属的模块名称>
	文件名称：IBase.cs
	�?   要：<描述该文件实现的主要功能>

	当前版本�?.0
	建立日期�?020/3/14
	�?   者：lenovo
	电子邮件�?username%@BoYue.com
	�?   注：<其它说明>

	</PRE>
*/
//------------------------------------------------------------------------------
// Using
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
// Class
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	IBase.cs
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filterartifact
{
    public class IBase : IMsgPipe
    {
        //----------------------------------------------------------------------------
        private Dictionary<Type, IComponent> m_dictCom = null;
        private Dictionary<Type, IComponent>.Enumerator m_itor;
        //----------------------------------------------------------------------------
        public T AddComponent<T>(object obj = null) where T : IComponent, new()
        {
            if (m_dictCom == null)
            {
                m_dictCom = new Dictionary<Type, IComponent>();
            }
            Type type = typeof(T);
            if (!m_dictCom.ContainsKey(type))
            {
                T t = new T();
                m_dictCom.Add(type, t);
                t.m_Parent = this;
                PlugInMsgPipe(t);
                t.Init(obj);
                t.bEnable = true;
                return t;
            }
            return null;
        }
        //----------------------------------------------------------------------------
        public virtual void Update()
        {
            if (m_dictCom != null)
            {
                m_itor = m_dictCom.GetEnumerator();
                while (m_itor.MoveNext())
                {
                    if (m_itor.Current.Value != null)
                    {
                        m_itor.Current.Value.Update();
                    }
                }
            }
        }
        //----------------------------------------------------------------------------
        public T AddComponent<T, Tbase>(object obj = null)
            where T : IComponent, new()
            where Tbase : IComponent, new()
        {
            if (m_dictCom == null)
            {
                m_dictCom = new Dictionary<Type, IComponent>();
            }

            Type type = typeof(Tbase);
            if (!m_dictCom.ContainsKey(type))
            {
                T t = new T();
                m_dictCom.Add(type, t);
                t.m_Parent = this;
                PlugInMsgPipe(t);
                t.Init(obj);
                t.bEnable = true;
                return t;
            }
            return null;
        }
        //----------------------------------------------------------------------------
        //----------------------------------------------------------------------------
        //----------------------------------------------------------------------------
        //----------------------------------------------------------------------------
        //----------------------------------------------------------------------------
    }
}
