//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
// Using
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
// Class
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	UnityCompManager.cs
//------------------------------------------------------------------------------

using UnityEngine;
/**
\file	UnityCompManager.cs

Copyright (c) 2020, BoYue. All rights reserved.

<PRE>

模块名称：<文件所属的模块名称>
文件名称：UnityCompManager.cs
摘    要：<描述该文件实现的主要功能>

当前版本：1.0
建立日期：2020/1/15
作    者：SYSTEM
电子邮件：SYSTEM@BoYue.com
备    注：<其它说明>

</PRE>
*/
namespace Filterartifact
{
    public class UnityCompManager
    {
        //----------------------------------------------------------------------------
        private static UnityCompManager m_tSingleton = null;
        public Transform m_rootAttachTrans { private set; get; }
        public GameObject m_objUIRoot { set; get; }
        private GameObject objMain = null;
        //----------------------------------------------------------------------------
        public static UnityCompManager CreateInstance()
        {

            if (m_tSingleton != null)
            {
                m_tSingleton = null;
            }
            m_tSingleton = new UnityCompManager();

            return m_tSingleton;

        }
        //----------------------------------------------------------------------------
        public bool Initionlization()
        {
            objMain = GameObject.Find("Launch");
#if GMORDER
            AddComponentToMain<HUDFPS>();
#endif
            return true;
        }
        //----------------------------------------------------------------------------
        public void InitUIRoot()
        {
            Object rootBundle = GameObject.Find("ui_root");
            m_objUIRoot = rootBundle as GameObject;
            m_rootAttachTrans = m_objUIRoot.transform.Find("UICamera/Canvas/CenterPageNode");
        }
        //----------------------------------------------------------------------------
        public static UnityCompManager Instance()
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
        public T AddComponentToMain<T>() where T : MonoBehaviour
        {
            if (objMain != null)
            {
                return objMain.AddComponent<T>();

            }
            return null;
        }
        //----------------------------------------------------------------------------
    }
}
