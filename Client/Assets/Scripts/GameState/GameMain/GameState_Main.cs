//------------------------------------------------------------------------------
/**
	\file	GameState_Main.cs

	Copyright (c) 2019, BoYue. All rights reserved.

	<PRE>

	模块名称：<文件所属的模块名称>
	文件名称：GameState_Main.cs
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
//	GameState_Main.cs
//------------------------------------------------------------------------------

namespace Filterartifact
{
    public class GameState_Main : GameState
    {
        //member variable
        private GameStateType nextGameType = GameStateType.GST_Invaild;
        private object[] state_parameter = null;
        //----------------------------------------------------------------------------
        //constructor 
        //----------------------------------------------------------------------------
        public GameState_Main(int stateid, State parent) : base(stateid, parent)
        {
            RegisterMsg((int)MsgID.MSG_MSG_GOTOLOGIN, OnGoToLogin);
            RegisterMsg<object[]>((int)MsgID.MSG_MSG_GOTOLOBBY, OnGoToLobby);
        }
        //----------------------------------------------------------------------------
        public void OnGoToLobby(object[] parameter)
        {
            SetNextGameType(GameStateType.GST_Lobby, parameter);
        }
        //----------------------------------------------------------------------------
        protected override void OnStateInit()
        {
            AddSubState(GameStateFactory.CreateState(GameStateType.GST_Loading, this));
            AddSubState(GameStateFactory.CreateState(GameStateType.GST_Login, this));
            AddSubState(GameStateFactory.CreateState(GameStateType.GST_Update, this));
            AddSubState(GameStateFactory.CreateState(GameStateType.GST_Logo, this));
            base.OnStateInit();
            SetSubState((int)GameStateType.GST_Logo, null);
        }
        //----------------------------------------------------------------------------
        protected override void OnStateDestroy()
        {
            base.OnStateDestroy();
        }
        //----------------------------------------------------------------------------
        public void OnGoToLogin()
        {
            SetNextGameType(GameStateType.GST_Login);
        }
        //----------------------------------------------------------------------------
        void SetNextGameType(GameStateType gameStateType, object[] parameter = null)
        {
            nextGameType = gameStateType;
            state_parameter = parameter;
        }
        //----------------------------------------------------------------------------
        protected override void OnUpdate()
        {
            base.OnUpdate();
            WaitingGotoSubState();
        }
        //----------------------------------------------------------------------------
        void WaitingGotoSubState()
        {
            if (nextGameType != GameStateType.GST_Invaild)
            {
                if (state_parameter != null)
                {
                    GotoSubState((int)nextGameType, state_parameter);
                }
                else
                {
                    GotoSubState((int)nextGameType);
                }
                nextGameType = GameStateType.GST_Invaild;
                state_parameter = null;
            }
        }
        //----------------------------------------------------------------------------
    }
}
