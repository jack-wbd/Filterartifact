//------------------------------------------------------------------------------
/**
	\file	CopyStateFactory.cs

	Copyright (c) 2020, BoYue. All rights reserved.

	<PRE>

	模块名称：<文件所属的模块名称>
	文件名称：CopyStateFactory.cs
	摘    要：<描述该文件实现的主要功能>

	当前版本：1.0
	建立日期：2020/3/18
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
//	CopyStateFactory.cs
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filterartifact
{
    class CopyStateFactory
    {
        //----------------------------------------------------------------------------
        public static State CreateState(CopyStateType type, State parent)
        {
            State subState = null;
            switch (type)
            {
                case CopyStateType.CST_NONE:
                    break;
                case CopyStateType.CST_MAIN:
                    break;
                case CopyStateType.CST_LOADING:
                    {
                        subState = new CopyState_Loading((int)CopyStateType.CST_LOADING, parent);
                    }
                    break;
                case CopyStateType.CST_READY:
                    break;
                case CopyStateType.CST_END:
                    break;
                case CopyStateType.CST_SWITCHSCENE:
                    break;
                case CopyStateType.CST_NORMAL:
                    break;
                case CopyStateType.CST_SLOWCAMERA:
                    break;
                case CopyStateType.CST_ENDSHOWING:
                    break;
                case CopyStateType.CST_PLAYERDEAD:
                    break;
                case CopyStateType.CST_CITY_NORMAL:
                    break;
                case CopyStateType.CST_PVM_NORMAL:
                    break;
                case CopyStateType.CST_PVM_READY:
                    break;
                case CopyStateType.CST_PVM_ROOMLOADING:
                    break;
                case CopyStateType.CST_PVM_ENEMYBREAK:
                    break;
                case CopyStateType.CST_PVM_SCORE:
                    break;
                case CopyStateType.CST_PVM_WAITFORSCORE:
                    break;
                case CopyStateType.CST_PVM_COPYOVER:
                    break;
                case CopyStateType.CST_PVP_NORMAL:
                    break;
                case CopyStateType.CST_PVP_SCORE:
                    break;
                case CopyStateType.CST_PVP_READY:
                    break;
                case CopyStateType.CST_PVP_LOADING:

                    break;
                case CopyStateType.CST_PVP_ENDSHOWING:
                    break;
                case CopyStateType.CST_NEWBIE_NORMAL:
                    break;
                case CopyStateType.CST_LOBBY_NORMAL:
                    {
                        subState = new LobbyCopyState_Normal((int)CopyStateType.CST_LOBBY_NORMAL, parent);
                        break;
                    }
                case CopyStateType.CST_TEAM_WAITFORSCORE:
                    break;
                case CopyStateType.CST_TEAM_SCORE:
                    break;
                case CopyStateType.CST_TEAMCOPY_LOADING:
                    break;
                case CopyStateType.CST_TEAM_PLAYERDEAD:
                    break;
                case CopyStateType.CST_KOF_ROUNDOVER:
                    break;
                case CopyStateType.CST_KOF_ENDSHOWING:
                    break;
                case CopyStateType.CST_KOF_LOADING:
                    break;
                case CopyStateType.CST_QUICKRUN_WAITFORSCORE:
                    break;
                case CopyStateType.CST_QUICKRUN_SCORE:
                    break;
                case CopyStateType.CST_QUICKRUN_NORMAL:
                    break;
                case CopyStateType.CST_QUICKRUN_END:
                    break;
                case CopyStateType.CST_OFFLINEPK_SCORE:
                    break;
                case CopyStateType.CST_OFFLINEPK_ENDSHOWING:
                    break;
                case CopyStateType.CST_NIGHTMARE_PLAYERDEAD:
                    break;
                case CopyStateType.CST_GUILD_PLAYERDEAD:
                    break;
                case CopyStateType.CST_PROMOTION_LOADING:
                    break;
                case CopyStateType.CST_PROMOTION_ROUNDOVER:
                    break;
                case CopyStateType.CST_PROMOTION_ROUNDSCORE:
                    break;
                case CopyStateType.CST_PROMOTION_ENDSHOWING:
                    break;
                case CopyStateType.CST_PROMOTION_SCORE:
                    break;
                case CopyStateType.CST_PROMOTION_WATCHSCORE:
                    break;
                default:
                    break;
            }

            return subState;

        }
        //----------------------------------------------------------------------------

        //----------------------------------------------------------------------------
        //----------------------------------------------------------------------------
        //----------------------------------------------------------------------------
        //----------------------------------------------------------------------------
    }
}
