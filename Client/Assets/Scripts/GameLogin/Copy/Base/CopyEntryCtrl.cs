//------------------------------------------------------------------------------
/**
	\file	CopyEntryCtrl.cs

	Copyright (c) 2020, BoYue. All rights reserved.

	<PRE>

	模块名称�?文件所属的模块名称>
	文件名称：CopyEntryCtrl.cs
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
//	CopyEntryCtrl.cs
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filterartifact
{
    public class CopyEntryCtrl : IMsgPipe
    {
        //----------------------------------------------------------------------------
        public virtual bool Initialize()
        {
            //默认副本
            Messenger.AddListener(DgMsgID.DgMsg_GoToCity, OnGoToCity);
            return true;
        }
        //----------------------------------------------------------------------------
        public void Finalized()
        {
            Messenger.RemoveListener(DgMsgID.DgMsg_GoToCity, OnGoToCity);
        }
        //----------------------------------------------------------------------------
        private void OnGoToCity()
        {
            OnGoToLobby();
        }
        //----------------------------------------------------------------------------
        private void OnGoToLobby()
        {
            sPVMParamInfo sInfo = sPVMParamInfo.Clear;
            sInfo.nGroupTempID = 1;
            sInfo.nCopyTempID = 11;
            sInfo.nStageTempID = 111;
            sInfo.nStageIndex = 0;
            sInfo.strName = "";
            CheckLoadingType(GameStateType.GST_Lobby);
            WorldManager.Instance().ProMsgToState((int)MsgID.MSG_MSG_GOTOLOBBY, new object[] { sInfo, false });
        }
        //----------------------------------------------------------------------------
        public void CheckLoadingType(GameStateType stateType)
        {
            eSceneType type = eSceneType.BATTLE_SCENE;
            switch (stateType)
            {
                case GameStateType.GST_Invaild:
                    break;
                case GameStateType.GST_Main:
                    break;
                case GameStateType.GST_Loading:
                    break;
                case GameStateType.GST_Login:
                    break;
                case GameStateType.GST_Update:
                    break;
                case GameStateType.GST_Logo:
                    break;
                case GameStateType.GST_Lobby:
                    type = eSceneType.LOBBY_SCENE;
                    break;
                default:
                    type = eSceneType.BATTLE_SCENE;
                    break;
            }
            Messenger.Broadcast(DgMsgID.DgMsg_ShowLoadingUIByType, type);
        }
        //----------------------------------------------------------------------------
    }
}
