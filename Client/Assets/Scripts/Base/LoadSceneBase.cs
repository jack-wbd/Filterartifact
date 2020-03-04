//------------------------------------------------------------------------------
/**
	\file	LoadSceneBase.cs

	Copyright (c) 2020, BoYue. All rights reserved.

	<PRE>

	模块名称�?文件所属的模块名称>
	文件名称：LoadSceneBase.cs
	�?   要：<描述该文件实现的主要功能>

	当前版本�?.0
	建立日期�?020/3/4
	�?   者：SYSTEM
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
//	LoadSceneBase.cs
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filterartifact
{
    public class LoadSceneBase : IPreLoad
    {
        public static int CurAssetCount = 0;
        public static int MaxAssetCount = 0;

        public virtual void AddPreLoadAsset(string strAssetID) { }
        public virtual void AddPreLoadEffect(string strEffectID) { }
        public virtual void AddPreLoadCommander(int nCommanderID) { }
        public virtual void AddPreLoadAudio(string strAudioID) { }
        public virtual void AcitvePreLoad() { }
    }
}
