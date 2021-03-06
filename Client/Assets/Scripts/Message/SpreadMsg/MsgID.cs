﻿//------------------------------------------------------------------------------
/**
	\file	MsgID.cs

	Copyright (c) 2020, BoYue. All rights reserved.

	<PRE>

	模块名称：<文件所属的模块名称>
	文件名称：MsgID.cs
	摘    要：<描述该文件实现的主要功能>

	当前版本：1.0
	建立日期：2020/1/3
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
//	MsgID.cs
//------------------------------------------------------------------------------

namespace Filterartifact
{
    //----------------------------------------------------------------------------
    public enum MsgID
    {
        //New Bie 新手
        Msg_NewBie_ChangeUIParent = 0x00011003,
        Msg_NewBie_RestoreUIParent = 0x00011004,
        Msg_NewBie_DyUIScrollViewEvent = 0x00011005,
        MSG_MSG_GOTOLOGIN = 0x00011006,
        MSG_MSG_GOTOLOBBY = 0x00011007,
        MSG_LOADING_ALLREADY = 0x00011008,
    }
    //----------------------------------------------------------------------------
}
