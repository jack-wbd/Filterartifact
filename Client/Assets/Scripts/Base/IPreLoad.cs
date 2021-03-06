﻿//------------------------------------------------------------------------------
/**
	\file	IPreLoad.cs

	Copyright (c) 2019, BoYue. All rights reserved.

	<PRE>

	模块名称：<文件所属的模块名称>
	文件名称：IPreLoad.cs
	摘    要：<描述该文件实现的主要功能>

	当前版本：1.0
	建立日期：2019/12/6
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
//	IPreLoad.cs
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filterartifact
{
    //----------------------------------------------------------------------------
    public interface IPreLoad
    {
        void AddPreLoadAsset(string strAssetID);
        void AddPreLoadEffect(string strEffectID);
        void AddPreLoadCommander(int nCommanderID);
        void AddPreLoadAudio(string strAudioID);
        void AcitvePreLoad();
    }
    //----------------------------------------------------------------------------
}
