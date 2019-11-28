﻿//------------------------------------------------------------------------------
/**
	\file	WorldManager.cs

	Copyright (c) 2019, BoYue. All rights reserved.

	<PRE>

	模块名称：<文件所属的模块名称>
	文件名称：WorldManager.cs
	摘    要：<描述该文件实现的主要功能>

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
//	WorldManager.cs
//------------------------------------------------------------------------------

namespace Filterartifact
{
    public class WorldManager
    {
        //----------------------------------------------------------------------------
        public static WorldManager m_tSingleton = null;
        //----------------------------------------------------------------------------
        public static WorldManager CreateInstance()
        {
            if (m_tSingleton != null)
            {
                m_tSingleton = null;
            }
            m_tSingleton = new WorldManager();

            return m_tSingleton;
        }
        //----------------------------------------------------------------------------
        public static WorldManager Instance()
        {
            return m_tSingleton;
        }
        //----------------------------------------------------------------------------
        public static void ReleaseInstance()
        {
            if (m_tSingleton != null)
            {
                m_tSingleton = null;
            }
        }
        //----------------------------------------------------------------------------
        public virtual void Destroy()
        {
            ReleaseInstance();
        }
        //----------------------------------------------------------------------------
        public virtual void Update()
        {
            if (FileSystem.Instance() == null)
            {
                FileSystem.CreateInstance();
                FileSystem.Instance().InitFileSystem();
            }
            if (FileSystem.Instance() != null)
            {
                FileSystem.Instance().Update();
            }
        }
    }
}

