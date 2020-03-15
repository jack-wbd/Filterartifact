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
    public enum eSceneType
    {
        CITY_SCENE = 0, //城镇（已经不用）
        BATTLE_SCENE = 1,//主线副本
        CREATEROLE_SCENE = 2,//创建角色
        NEWBIE_SCENE_OLD = 3,//新手引导
        PVR_SCENE = 4,// 暂时无用
        PVP_SCENE = 5,// 自由PK副本
        LOBBY_SCENE = 6,//大厅（现在的城镇）
        ACTIVE_SCENE = 7,//活动组队副本  ACTIVE_SCENE==BATTLE_SCENE
        Elimit_SCENE = 8, //精英副本
        NEWBIE_SCENE = 9, //新手引导撞船场景
        QUICKRUNGAME_SCENE = 10, //跑酷场景
        COMMON_SCENE = 11, //通用场景
        OFFLINEPK_SCENE = 12,
        KOF_SCENE = 13,
        PROMOTION_SCENE = 14, //决赛
        PRELIM_SCENE = 15,   //预选赛
        MOBA = 16,
        TSPORTS = 17, //极限竞技
        VIDEO_SCENE = 18,
        UNKNOWN_REGION
    }
    //----------------------------------------------------------------------------
    public enum GameStateType
    {
        GST_Invaild = -1,
        GST_Main = 0,
        GST_Loading = 1,
        GST_Login = 2,
        GST_Update = 3,
        GST_Logo = 4,
        GST_Lobby = 5,
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
                case GameStateType.GST_Lobby:

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
