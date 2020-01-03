//------------------------------------------------------------------------------s
/**
	\file	Main.cs

	Copyright (c) 2019, BoYue. All rights reserved.

	<PRE>

	模块名称：<程序启动类>
	文件名称：Main.cs
	摘    要：<程序启动主类>

	当前版本：1.0
	建立日期：2019/11/8
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
//	Main.cs
//------------------------------------------------------------------------------
using UnityEngine;
namespace Filterartifact
{
    public class Main : MonoBehaviour
    {
        GameApp gameApp = null;
        GameUI gameUI = null;
        LauncherManager launcherManager = null;
        //----------------------------------------------------------------------------
        void Start()
        {
            DontDestroyOnLoad(gameObject);
            StartLauncher();
        }
        //----------------------------------------------------------------------------
        void StartLauncher()
        {
            launcherManager = new LauncherManager();
            launcherManager.Initialization();
        }
        //----------------------------------------------------------------------------
        void Update()
        {
            if (launcherManager != null)
            {
                launcherManager.Update();
                if (launcherManager.IsLauncherFinish())
                {
                    launcherManager.SetLauncherFinish(false);
                    StartAppGame();
                }
            }
            if (gameApp != null)
            {
                gameApp.Update();
            }
        }
        //----------------------------------------------------------------------------
        void StartAppGame()
        {
            gameApp = new GameApp();
            if (gameApp != null)
            {
                gameApp.Initialize();
            }

            gameUI = new GameUI();

            if (gameUI != null)
            {
                gameUI.Initialize();
            }
        }
        //----------------------------------------------------------------------------
        private void OnDestroy()
        {

        }
    }
}
