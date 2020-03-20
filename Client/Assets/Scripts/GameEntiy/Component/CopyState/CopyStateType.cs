//------------------------------------------------------------------------------
/**
	\file	CopyStateType.cs

	Copyright (c) 2020, BoYue. All rights reserved.

	<PRE>

	模块名称：<文件所属的模块名称>
	文件名称：CopyStateType.cs
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
//	CopyStateType.cs
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filterartifact
{
    public enum CopyStateType
    {
        CST_NONE,
        CST_MAIN,
        CST_LOADING,
        CST_READY,
        CST_END,
        CST_SWITCHSCENE,
        CST_NORMAL,

        CST_SLOWCAMERA,
        CST_ENDSHOWING,
        CST_PLAYERDEAD,
        //city
        CST_CITY_NORMAL,
        //pvm
        CST_PVM_NORMAL,
        CST_PVM_READY,
        CST_PVM_ROOMLOADING,
        CST_PVM_ENEMYBREAK,
        CST_PVM_SCORE,
        CST_PVM_WAITFORSCORE,
        CST_PVM_COPYOVER,
        //pvp
        CST_PVP_NORMAL,
        CST_PVP_SCORE,
        CST_PVP_READY,
        CST_PVP_LOADING,
        CST_PVP_ENDSHOWING,
        //newbie
        CST_NEWBIE_NORMAL,
        //lobby
        CST_LOBBY_NORMAL,

        //team 
        CST_TEAM_WAITFORSCORE,
        CST_TEAM_SCORE,
        CST_TEAMCOPY_LOADING,
        CST_TEAM_PLAYERDEAD,

        //kof
        CST_KOF_ROUNDOVER,
        CST_KOF_ENDSHOWING,
        CST_KOF_LOADING,

        //quick run
        CST_QUICKRUN_WAITFORSCORE,
        CST_QUICKRUN_SCORE,
        CST_QUICKRUN_NORMAL,
        CST_QUICKRUN_END,

        CST_OFFLINEPK_SCORE,
        CST_OFFLINEPK_ENDSHOWING,

        //nightmare
        CST_NIGHTMARE_PLAYERDEAD,

        //guild
        CST_GUILD_PLAYERDEAD,

        //promotion
        CST_PROMOTION_LOADING,
        CST_PROMOTION_ROUNDOVER,
        CST_PROMOTION_ROUNDSCORE,
        CST_PROMOTION_ENDSHOWING,
        CST_PROMOTION_SCORE,
        CST_PROMOTION_WATCHSCORE,
    }
}
