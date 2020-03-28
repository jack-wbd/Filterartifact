//------------------------------------------------------------------------------
/**
	\file	UIGMMainCtrl.cs

	Copyright (c) 2020, BoYue. All rights reserved.

	<PRE>

	模块名称：<文件所属的模块名称>
	文件名称：UIGMMainCtrl.cs
	摘    要：<描述该文件实现的主要功能>

	当前版本：1.0
	建立日期：2020/3/27
	作    者：SYSTEM
	电子邮件：SYSTEM@BoYue.com
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
//	UIGMMainCtrl.cs
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Filterartifact
{
#if GMORDER
    public class UIGMMainCtrl : UIController
    {
        //----------------------------------------------------------------------------
        public enum eUIGMMainType
        {
            Main = -1,
            Res,
            Battle,
            Native,
            Log,
            Login,
            Guide,
            Scenario,
            Other,
            Count
        }
        //----------------------------------------------------------------------------
        public enum eUIGMMainItemType
        {
            Button,
            InputButton,
            Text
        }
        //----------------------------------------------------------------------------
        private Dictionary<eUIGMMainType, List<UIGMMainInfo>> m_infoDict = new Dictionary<eUIGMMainType, List<UIGMMainInfo>>();
        private UIGMMainView m_view;
        //----------------------------------------------------------------------------
        public override void InitViewer(object arg, bool bflag = false)
        {
            base.InitViewer(arg, bflag);
            InitList();
        }
        //----------------------------------------------------------------------------
        public override void FinalViewer()
        {
            base.FinalViewer();
            m_infoDict.Clear();
            m_view = null;
        }
        //----------------------------------------------------------------------------
        public override void Show(object arg = null)
        {
            base.Show(arg);
            m_view = viewer as UIGMMainView;
            if (m_view != null && IsViewLoadedComplete())
            {
                ShowPanel(eUIGMMainType.Main);
            }
        }
        //----------------------------------------------------------------------------
        private void ShowPanel(eUIGMMainType type)
        {
            if (m_view == null)
            {
                m_view = new UIGMMainView();
            }
            if (m_infoDict.ContainsKey(type))
            {
                m_view.ShowList(m_infoDict[type]);
            }
        }
        //----------------------------------------------------------------------------
        private void InitList()
        {
            AddBtn(eUIGMMainType.Main, "Close", Close);

            for (int i = 0; i < (int)eUIGMMainType.Count; i++)
            {
                eUIGMMainType type = (eUIGMMainType)i;

                if (type == eUIGMMainType.Login)
                {
                    AddBtn(eUIGMMainType.Main, "快速登录", FastLogin);
                }
                else
                {
                    AddBtn(eUIGMMainType.Main, GetTypeName(type), () => ShowPanel(type));

                    AddBtn(type, "Back", () => ShowPanel(eUIGMMainType.Main));
                }

            }

            AddBtn(eUIGMMainType.Log, "屏幕日志", ScreenLog);
        }
        //----------------------------------------------------------------------------
        private void FastLogin()
        {

        }
        //----------------------------------------------------------------------------
        private void ScreenLog()
        {
            GameObject go = GameObject.Find("Launch").FindChildGO("Reporter");
            if (go.activeSelf)
            {
                go.Visible(false);
            }
            else
            {
                go.Visible(true);
                go.SendMessage("SendShowLog");
            }

            Close();
        }
        //----------------------------------------------------------------------------
        public static void Close()
        {
            Messenger.Broadcast(DgMsgID.DgUI_HideUI, "UIGMMainCtrl");
        }
        //----------------------------------------------------------------------------
        private void AddBtn(eUIGMMainType type, string name, Action act, Vector2 pos = default)
        {
            UIGMMainInfo info = new UIGMMainInfo { name = name, action = act, pos = pos };
            GetInfoList(type).Add(info);
        }
        //----------------------------------------------------------------------------
        private List<UIGMMainInfo> GetInfoList(eUIGMMainType type)
        {
            if (!m_infoDict.ContainsKey(type))
            {
                m_infoDict.Add(type, new List<UIGMMainInfo>());

            }
            return m_infoDict[type];
        }
        //----------------------------------------------------------------------------
        private string GetTypeName(eUIGMMainType type)
        {
            switch (type)
            {
                case eUIGMMainType.Res:
                    return "资源";
                case eUIGMMainType.Battle:
                    return "战斗";
                case eUIGMMainType.Native:
                    return "原生";
                case eUIGMMainType.Log:
                    return "日志";
                case eUIGMMainType.Login:
                    return "登录";
                case eUIGMMainType.Other:
                    return "其他";
                case eUIGMMainType.Guide:
                    return "引导";
                case eUIGMMainType.Scenario:
                    return "剧情";
                default:
                    return type.ToString();
            }
        }

        //----------------------------------------------------------------------------
        public class UIGMMainInfo
        {
            public eUIGMMainItemType type = eUIGMMainItemType.Button;
            public string name;
            public Action action;
            public Action<string> stringAction;
            public Func<string> stringFunc;
            public Vector2 pos;
        }
        //----------------------------------------------------------------------------
    }
#endif
}
