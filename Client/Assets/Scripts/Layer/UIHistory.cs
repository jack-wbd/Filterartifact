﻿//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
// Using
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
// Class
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	UIHistory.cs
//------------------------------------------------------------------------------

using System.Collections.Generic;
/**
\file	UIHistory.cs

Copyright (c) 2020, BoYue. All rights reserved.

<PRE>

模块名称：<文件所属的模块名称>
文件名称：UIHistory.cs
摘    要：<描述该文件实现的主要功能>

当前版本：1.0
建立日期：2020/1/3
作    者：SYSTEM
电子邮件：SYSTEM@BoYue.com
备    注：<其它说明>

</PRE>
*/
namespace Filterartifact
{
    //----------------------------------------------------------------------------
    public class UIHistory
    {
        //----------------------------------------------------------------------------
        static public string winKey = "";
        static public List<string> showList = new List<string>();
        static private List<string> allShowUI = new List<string>();
        static private List<string> uiQuene = new List<string>();
        static private Dictionary<string, UIHistoryData> ctrlDicts = new Dictionary<string, UIHistoryData>();
        static private Dictionary<string, Dictionary<string, UIHistoryData>> panelDicts = new Dictionary<string, Dictionary<string, UIHistoryData>>();
        //----------------------------------------------------------------------------
        static public void Show(string key, object arg, eUIImpower impower = eUIImpower.Default)
        {
            if (key != "UIErrorCtrl" &&
                key != "UIEffectCtrl" &&
                key != "UIItemGetCtrl" &&
                key != "UIMonthCardCtrl" &&
                key != "UDisConnectionCtrl" &&
                key != "UIDialogToggleCtrl" &&
                key != "UICircleCtrl" &&
                key != "UIChatSimpleViewOldCtrl" &&
                key != "UIPlayerUpgradeCtrl" &&
                key != "UIGuideVeteranModeCtrl" &&
                key != "UIChatSimpleCtrl" &&
                key != "UIAdvertisingCtrl" &&
                key != "UINewPlayerRewardCtrl" &&
                key != "UILoopLoginRewardPanelCtrl")
            {
                if (allShowUI.IndexOf(key) == -1)
                {
                    allShowUI.Add(key);
                }
                else
                {
                    allShowUI.Remove(key);
                    allShowUI.Add(key);
                }
            }

            switch (impower)
            {
                case eUIImpower.Window:
                    if (winKey != "" && winKey != key)
                        Messenger.Broadcast(DgMsgID.DgUI_HideLastUI, winKey);
                    int index = uiQuene.IndexOf(key);
                    if (index > -1)
                    {
                        uiQuene.RemoveAt(index);
                    }
                    uiQuene.Add(key);
                    AddWindow(key, arg, impower);
                    showList.Clear();
                    break;
                case eUIImpower.Panel:
                    AddPanel(key, arg, impower);
                    break;
                case eUIImpower.Default:
                    return;
                default:
                    break;
            }
            if (!showList.Contains(key))
            {
                showList.Add(key);
            }
        }
        //----------------------------------------------------------------------------
        private static UIHistoryData AddWindow(string key, object arg, eUIImpower impower = eUIImpower.Default)
        {
            winKey = key;
            UIHistoryData uiHistoryData;
            ctrlDicts.TryGetValue(key, out uiHistoryData);
            if (uiHistoryData == null)
            {
                uiHistoryData = new UIHistoryData();
                ctrlDicts.Add(key, uiHistoryData);
            }
            else
            {
                Dictionary<string, UIHistoryData> dict;
                panelDicts.TryGetValue(key, out dict);
                if (dict != null)
                {
                    dict.Clear();
                    dict = null;
                    panelDicts[key] = null;
                    panelDicts.Remove(key);
                }
            }

            uiHistoryData.UpdateData(key, arg, impower);
            return uiHistoryData;

        }
        //----------------------------------------------------------------------------
        private static UIHistoryData AddPanel(string key, object arg, eUIImpower impower = eUIImpower.Default)
        {
            if (winKey == "")
            {
                return null;
            }
            Dictionary<string, UIHistoryData> dict;
            panelDicts.TryGetValue(winKey, out dict);
            if (dict == null)
            {
                dict = new Dictionary<string, UIHistoryData>();
                panelDicts.Add(winKey, dict);
            }

            UIHistoryData uiHistoryData = null;
            dict.TryGetValue(key, out uiHistoryData);
            if (uiHistoryData == null)
            {
                uiHistoryData = new UIHistoryData();
                dict.Add(key, uiHistoryData);
            }
            uiHistoryData.UpdateData(key, arg, impower);
            return uiHistoryData;

        }
        //----------------------------------------------------------------------------
        //----------------------------------------------------------------------------
    }
    //----------------------------------------------------------------------------
    public class UIHistoryData
    {
        public string key;
        public object data;
        public eUIImpower impower;
        public void UpdateData(string _key, object _data, eUIImpower _impower)
        {
            key = _key;
            data = _data;
            impower = _impower;
        }
    }
}
