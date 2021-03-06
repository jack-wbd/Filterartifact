﻿//------------------------------------------------------------------------------
/**
	\file	GameApp.cs

	Copyright (c) 2019, BoYue. All rights reserved.

	<PRE>

	模块名称：<App启动类>
	文件名称：GameApp.cs
	摘    要：<启动App>

	当前版本：1.0
	建立日期：2019/11/14
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
//	GameApp.cs
//------------------------------------------------------------------------------
namespace Filterartifact
{
    public class GameApp : IGameApp
    {
        //----------------------------------------------------------------------------
        public override bool Initialize()
        {
            base.Initialize();
            if (WorldManager.CreateInstance() != null)
            {
                WorldManager.Instance().CreateWorld();
            }
            return true;
        }
        //----------------------------------------------------------------------------
        public override void Finalized()
        {
            base.Finalized();
            if (WorldManager.Instance() != null)
            {
                WorldManager.Instance().Destroy();
            }
        }
        //----------------------------------------------------------------------------
        public override void Update()
        {
            base.Update();
            if (WorldManager.Instance() != null)
            {
                WorldManager.Instance().Update();
            }
        }
        //----------------------------------------------------------------------------
        public override void LateUpdate()
        {
            base.LateUpdate();
        }
        //----------------------------------------------------------------------------
        public override void Destroy()
        {
            base.Destroy();
        }
        //----------------------------------------------------------------------------
    }
}

