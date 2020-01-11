//------------------------------------------------------------------------------
/**
	\file	GameState_Login.cs

	Copyright (c) 2019, BoYue. All rights reserved.

	<PRE>

	模块名称：<文件所属的模块名称>
	文件名称：GameState_Login.cs
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
//	GameState_Login.cs
//------------------------------------------------------------------------------

namespace Filterartifact
{
    //----------------------------------------------------------------------------
    public class GameState_Login : GameState
    {
        //----------------------------------------------------------------------------
        public GameState_Login(int stateid, State parent) : base(stateid, parent)
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
            Messenger.AddListener(DgMsgID.DgMsg_NtyUILoading_Finish, OnUILoadOk);
            Messenger.Broadcast(DgMsgID.DgMsg_PreLoadUI, eUseEnvir.login);
            Messenger.Broadcast(DgMsgID.DgMsg_ActiveLoadUI);
        }
        //----------------------------------------------------------------------------
        protected override void OnStateEnd()
        {

        }
        //----------------------------------------------------------------------------
        private void OnUILoadOk()
        {
            Messenger.RemoveListener(DgMsgID.DgMsg_NtyUILoading_Finish, OnUILoadOk);
            Messenger.Broadcast(DgMsgID.DgUI_ShowUI, "UILoginCtrl");
        }
        //----------------------------------------------------------------------------
        //----------------------------------------------------------------------------
    }
}
