//------------------------------------------------------------------------------
/**
	\file	GameStage_Lobby.cs

	Copyright (c) 2020, BoYue. All rights reserved.

	<PRE>

	模块名称�?文件所属的模块名称>
	文件名称：GameStage_Lobby.cs
	�?   要：<描述该文件实现的主要功能>

	当前版本�?.0
	建立日期�?020/3/14
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
//	GameStage_Lobby.cs
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filterartifact
{
    class GameStage_Lobby : GameState
    {
        //----------------------------------------------------------------------------
        public GameStage_Lobby(int stateid, State parent) : base(stateid, parent)
        {

        }
        //----------------------------------------------------------------------------
        protected override void OnStateInit()
        {
            base.OnStateInit();
        }
        //----------------------------------------------------------------------------
        protected override void OnStateBegin(object[] parameter)
        {
            base.OnStateBegin(parameter);
            FileSystem.Instance().SetCountFrame(FileSystem.MaxCountPerFramInCity);
            sPVMParamInfo sInfo = (sPVMParamInfo)parameter[0];
            if (!ReferenceEquals(sInfo, null))
            {
                sInfo.strName = "1";
                WorldManager.Instance().SetSwitchSceneParam(ref sInfo);
            }

        }
        //----------------------------------------------------------------------------
        protected override void OnStateEnd()
        {
            base.OnStateEnd();
        }
        //----------------------------------------------------------------------------
        protected override void OnUpdate()
        {
            base.OnUpdate();
        }
        //----------------------------------------------------------------------------
    }
}
