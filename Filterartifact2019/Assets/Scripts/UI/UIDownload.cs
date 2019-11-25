//------------------------------------------------------------------------------
/**
	\file	UIDownload.cs

	Copyright (c) 2019, BoYue. All rights reserved.

	<PRE>

	模块名称：<UI显示类>
	文件名称：UIDownload.cs
	摘    要：<游戏下载界面>

	当前版本：1.0
	建立日期：2019/11/22
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
//	UIDownload.cs
//------------------------------------------------------------------------------
using UnityEngine;
using UnityEngine.UI;

public class UIDownload
{
    private GameObject m_root;
    private Transform m_tans;
    private GameObject m_uiDownload;
    private Text m_version;
    private Slider m_progressBar;
    private Text m_proTips;
    private Main m_parent;
    public UIDownload(Main _parent)
    {
        m_parent = _parent;
        m_root = GameObject.Find("ui_root");
        m_uiDownload = m_root.transform.Find("Camera/Canvas/ui_download").gameObject;
        m_tans = m_uiDownload.transform;
        m_version = m_tans.Find("version").GetComponent<Text>();
        m_progressBar = m_tans.Find("progressBar").GetComponent<Slider>();
        m_proTips = m_progressBar.transform.Find("schedule").GetComponent<Text>();
    }

    public void Show()
    {
        if (m_uiDownload)
        {
            m_uiDownload.SetActive(true);
        }
    }

}

