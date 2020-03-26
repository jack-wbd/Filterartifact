﻿//------------------------------------------------------------------------------
/**
	\file	IComponent.cs

	Copyright (c) 2020, BoYue. All rights reserved.

	<PRE>

	模块名称�?文件所属的模块名称>
	文件名称：IComponent.cs
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
//	IComponent.cs
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filterartifact
{
    public class IComponent : IMsgPipe
    {
        //----------------------------------------------------------------------------
        public IBase m_Parent = null;
        public bool bEnable = false;
        public bool bReady = false;
        //----------------------------------------------------------------------------
        public virtual void SetParent<T>(T t)
        {

        }
        //----------------------------------------------------------------------------
        public virtual void Init(object obj)
        {

        }
        //----------------------------------------------------------------------------
        public virtual void Update()
        {

        }
        //----------------------------------------------------------------------------
        public virtual void LateUpdate()
        {

        }
        //----------------------------------------------------------------------------
        public virtual void Destroy()
        {

        }
        //----------------------------------------------------------------------------
        public T AddComponent<T>(object obj = null) where T : IComponent, new()
        {
            if (m_Parent != null)
            {
                return m_Parent.AddComponent<T>(obj);
            }
            return null;
        }
        //----------------------------------------------------------------------------

        //----------------------------------------------------------------------------
        //----------------------------------------------------------------------------

    }
}