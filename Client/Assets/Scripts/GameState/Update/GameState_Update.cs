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
        private bool m_bFileSystemInit = false;
        private bool m_bFileSystemOK = false;
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
            FileSystem.CreateInstance();
            FileSystem.Instance().InitFileSystem();
        }
        //----------------------------------------------------------------------------
        protected override void OnStateEnd()
        {
            LauncherManager.Instance.DownLoadUIFinished();
        }
        //----------------------------------------------------------------------------
        protected override void OnUpdate()
        {
            DoSkip();
            if (m_bFileSystemOK)
            {
                m_bFileSystemOK = false;
                m_bFileSystemInit = true;
                Resources.UnloadUnusedAssets();//释放未使用的资源
                WorldManager.Instance().InitGame();
                Debug.LogError("资源已被释放");
                Messenger.Broadcast(DgMsgID.DgMsg_PreLoadUI, eUseEnvir.none);
                Messenger.Broadcast(DgMsgID.DgMsg_ActiveLoadUI);
                Debug.LogError("InitGame End");
            }
        }
        //----------------------------------------------------------------------------
        private void DoSkip()
        {
            if (m_bFileSystemInit)
            {
                if (UISystem.LoadUIOK())
                {
                    //OnPreLoadBGM();//加载背景音乐
                    m_bFileSystemInit = false;
                    WorldManager.Instance().ProMsgToState((int)MsgID.MSG_MSG_GOTOLOGIN);
                }
            }
        }
        //----------------------------------------------------------------------------
        private void OnPreLoadBGM()
        {
            AssetsManager m_maniAsset = WorldManager.Instance().GetLayer<AssetLayer>().GetManager();
            m_maniAsset.LoadAssetRes<string, Object>("RAB_bgm_opening", OnLoadCallBack);
        }
        //----------------------------------------------------------------------------
        private void OnLoadCallBack(string strAssetID, Object obj)
        {

        }
        //----------------------------------------------------------------------------
        private void FileSystemOK()
        {
            m_bFileSystemOK = true;
        }
        //----------------------------------------------------------------------------
    }
}
