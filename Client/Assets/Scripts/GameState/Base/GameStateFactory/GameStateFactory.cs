//------------------------------------------------------------------------------
/**
	\file	GameStateFactory.cs

	Copyright (c) 2019, BoYue. All rights reserved.

	<PRE>

	模块名称：<文件所属的模块名称>
	文件名称：GameStateFactory.cs
	摘    要：<描述该文件实现的主要功能>

	当前版本：1.0
	建立日期：2019/11/29
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
//	GameStateFactory.cs
//------------------------------------------------------------------------------
namespace Filterartifact
{
    //----------------------------------------------------------------------------
    public enum GameStateType
    {
        GST_Invaild = -1,
        GST_Main = 0,
        GST_Loading = 1,
        GST_Login = 2,
        GST_Update = 3,
        GST_Logo =4,
    }
    //----------------------------------------------------------------------------
    class GameStateFactory
    {
        public static State CreateState(GameStateType type, State parent)
        {
            State subState = null;
            switch (type)
            {
                case GameStateType.GST_Invaild:
                    break;
                case GameStateType.GST_Main:
                    subState = new GameState_Main((int)GameStateType.GST_Main, parent);
                    break;
                case GameStateType.GST_Loading:
                    subState = new GameState_Loading((int)GameStateType.GST_Loading, parent);
                    break;
                case GameStateType.GST_Update:
                    subState = new GameState_Update((int)GameStateType.GST_Update, parent);
                    break;
                case GameStateType.GST_Logo:
                    subState = new GameState_Logo((int)GameStateType.GST_Logo, parent);
                    break;
                case GameStateType.GST_Login:
                    subState = new GameState_Login((int)GameStateType.GST_Login, parent);
                    break;
                default:
                    break;
            }
            return subState;
        }
    }
    //----------------------------------------------------------------------------
    //----------------------------------------------------------------------------
    //----------------------------------------------------------------------------
    //----------------------------------------------------------------------------

}
