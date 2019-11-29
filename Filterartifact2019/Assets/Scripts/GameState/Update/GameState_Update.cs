//------------------------------------------------------------------------------
/**
	\file	GameState_Update.cs

	Copyright (c) 2019, BoYue. All rights reserved.

	<PRE>

	模块名称：<文件所属的模块名称>
	文件名称：GameState_Update.cs
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
//	GameState_Update.cs
//------------------------------------------------------------------------------

using UnityEngine;

namespace Filterartifact
{
    class GameState_Update : GameState
    {
        //----------------------------------------------------------------------------
        private bool m_bPreloadComplete = false;
        private bool m_bFileSystemInit = false;
        private bool m_bFileSystemOK = false;
        private bool m_bWait = false;
        private float m_fWaitTime = 1f;
        //----------------------------------------------------------------------------
        public GameState_Update(int stateid, State parent) : base(stateid, parent)
        {

        }
        //override function
        //----------------------------------------------------------------------------
        protected override void OnStateInit()
        {
            base.OnStateInit();
            Messenger.AddListener(DgMsgID.DgMsg_FileSystemOK, FileSystemOK);

        }
        //----------------------------------------------------------------------------
        protected override void OnStateDestroy()
        {
            base.OnStateDestroy();
            Messenger.RemoveListener(DgMsgID.DgMsg_FileSystemOK, FileSystemOK);
        }
        //----------------------------------------------------------------------------
        protected override void OnStateBegin(object[] parameter)
        {
            base.OnStateBegin(parameter);
            m_bWait = true;
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
            if (m_bWait)
            {
                m_fWaitTime -= Time.deltaTime;
                if (m_fWaitTime <= 0)
                {
                    m_fWaitTime = 1f;
                    m_bWait = false;
                    FileSystem.CreateInstance();
                    FileSystem.Instance().InitFileSystem();
                }
            }
            if (m_bFileSystemOK)
            {
                m_bFileSystemOK = false;
                Debug.Log("InitGame start");
            }
        }
        //----------------------------------------------------------------------------
        private void FileSystemOK()
        {
            m_bFileSystemOK = true;
        }
        //----------------------------------------------------------------------------
    }
}
