﻿//------------------------------------------------------------------------------
/**
	\file	Manager.cs

	Copyright (c) 2019, BoYue. All rights reserved.

	<PRE>

	模块名称：<文件所属的模块名称>
	文件名称：Manager.cs
	摘    要：<描述该文件实现的主要功能>

	当前版本：1.0
	建立日期：2019/11/22
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
//	Manager.cs
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Manager
{
    //----------------------------------------------------------------------------
    public virtual bool Initialized()
    {
        return true;
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
    public virtual void Finalized()
    {

    }
    //----------------------------------------------------------------------------
}

