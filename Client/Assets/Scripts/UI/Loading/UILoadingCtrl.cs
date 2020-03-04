//------------------------------------------------------------------------------
/**
	\file	UILoadingCtrl.cs

	Copyright (c) 2020, BoYue. All rights reserved.

	<PRE>

	模块名称�?文件所属的模块名称>
	文件名称：UILoadingCtrl.cs
	�?   要：<描述该文件实现的主要功能>

	当前版本�?.0
	建立日期�?020/3/3
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
//	UILoadingCtrl.cs
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filterartifact
{
    class UILoadingCtrl : UIController
    {
        //----------------------------------------------------------------------------
        private bool isOK = false;
        //----------------------------------------------------------------------------
        public void OnConnectSocialSuc()
        {
            isOK = true;
        }
        //----------------------------------------------------------------------------

        //----------------------------------------------------------------------------
        public override void Update()
        {
            base.Update();
            if (isOK)
            {
                GotoLobby();
            }
        }
        //----------------------------------------------------------------------------
        private void GotoLobby()
        {
            isOK = false;
            CheckLoadingType(GameStateType.GST_Lobby);

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
