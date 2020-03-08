//------------------------------------------------------------------------------
/**
	\file	UILoginCtrl.cs

	Copyright (c) 2020, BoYue. All rights reserved.

	<PRE>

	模块名称：<UI层控制类>
	文件名称：UILoginCtrl.cs
	摘    要：<登录界面控制器>

	当前版本：1.0
	建立日期：2020/1/8
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
//	UILoginCtrl.cs
//------------------------------------------------------------------------------

namespace Filterartifact
{
    class UILoginCtrl : UIController
    {
        private bool isOK = false;
        //----------------------------------------------------------------------------
        public void OnConnectSocialSuc()
        {
            isOK = true;
        }
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
