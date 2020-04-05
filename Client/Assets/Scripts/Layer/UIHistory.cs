//------------------------------------------------------------------------------

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
        static private Dictionary<string, string> m_strDelayHide = new Dictionary<string, string>();
        static private List<string> m_listHisoryUIName = new List<string>();
        static private List<UIHistoryData> UIHistoryStack = new List<UIHistoryData>();
        //----------------------------------------------------------------------------
        static public string GetDelayHideUIName(string showedUI)
        {
            if (m_strDelayHide.ContainsKey(showedUI))
            {
                return m_strDelayHide[showedUI];
            }
            return "";
        }
        //----------------------------------------------------------------------------
        static public void RemoveShowKey(string key)
        {
            allShowUI.Remove(key);
        }
        //----------------------------------------------------------------------------
        static public void RemoveDelayHideUI(string showedUI)
        {

            if (showedUI == null)
            {
                return;
            }
            if (m_strDelayHide.ContainsKey(showedUI))
            {
                m_strDelayHide.Remove(showedUI);
            }

        }
        //----------------------------------------------------------------------------
        static public void AddShowKey(string key)
        {
            if (key == "UIErrorCtrl") return;
            if (key == "UIEffectCtrl") return;
            if (key == "UIItemGetCtrl") return;
            if (key == "UIDialogToggleCtrl") return;
            if (key == "UDisConnectionCtrl") return;
            if (key == "UICircleCtrl") return;
            if (key == "UIChatSimpleViewOldCtrl") return;
            if (key == "UIPlayerUpgradeCtrl") return;
            if (key == "UIGuideVeteranModeCtrl") return;
            if (key == "UIChatSimpleCtrl") return;

            if (allShowUI.IndexOf(key) == -1)
                allShowUI.Add(key);
            else
            {
                allShowUI.Remove(key);
                allShowUI.Add(key);
            }
        }
        //----------------------------------------------------------------------------
        static public void ShowNew(string key, object arg, eUIImpower impower = eUIImpower.Default)
        {
            UIHistoryData data = GetPenuItimateView();
            if (data != null)
            {
                if (key == data.key)
                {
                    UIHistoryStack.Remove(data);
                }
            }
            UIHistoryData uIHistoryData = new UIHistoryData();
            uIHistoryData.UpdateData(key, arg, impower);
            UIHistoryStack.Add(uIHistoryData);
            m_listHisoryUIName.Add(key);

            if (key == "UIErrorCtrl") return;
            if (key == "UIEffectCtrl") return;
            if (key == "UIItemGetCtrl") return;
            if (key == "UIDialogToggleCtrl") return;
            if (key == "UDisConnectionCtrl") return;
            if (key == "UICircleCtrl") return;
            if (key == "UIChatSimpleViewOldCtrl") return;
            if (key == "UIPlayerUpgradeCtrl") return;
            if (key == "UIGuideVeteranModeCtrl") return;
            if (key == "UIChatSimpleCtrl") return;
            
            if (allShowUI.IndexOf(key)==-1)
            {
                allShowUI.Add(key);
            }
            else
            {
                allShowUI.Remove(key);
                allShowUI.Add(key);
            }
        }
        //------------------------------------------------------------------------------
        static public void ClearTotalHistory()
        {
            if (UIHistoryStack == null || UIHistoryStack.Count == 0)
            {
                return;
            }
            else
            {
                UIHistoryStack.RemoveRange(0, UIHistoryStack.Count);
                m_listHisoryUIName.RemoveRange(0, m_listHisoryUIName.Count);
            }
            UIHistoryStack.Clear();
            m_listHisoryUIName.Clear();
            allShowUI.Clear();
        }
        //----------------------------------------------------------------------------
        static public UIHistoryData HideNew(string key)
        {
            if (UIHistoryStack == null || UIHistoryStack.Count == 0)
            {
                return null;
            }
            if (UIHistoryStack[UIHistoryStack.Count - 1].key != key)
            {
                return null;
            }
            UIHistoryStack.RemoveAt(UIHistoryStack.Count - 1);
            m_listHisoryUIName.RemoveAt(m_listHisoryUIName.Count - 1);
            allShowUI.Remove(key);

            if (UIHistoryStack == null || UIHistoryStack.Count == 0)
            {
                return null;
            }
            return UIHistoryStack[UIHistoryStack.Count - 1];
        }
        //----------------------------------------------------------------------------
        static public UIHistoryData GetPenuItimateView()//获取倒数第二个界面
        {
            if (UIHistoryStack == null || UIHistoryStack.Count < 2)
            {
                return null;

            }
            return UIHistoryStack[UIHistoryStack.Count - 2];
        }
        //----------------------------------------------------------------------------
        static public List<string> GetHistoryUINamelist()
        {
            return m_listHisoryUIName;
        }
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
        static public void Hide(string key)
        {
            allShowUI.Remove(key);
            if (uiQuene.IndexOf(key) != -1)
            {
                winKey = "";
            }
            while (showList.IndexOf(key) != -1)
            {
                showList.Remove(key);
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
        static public UIHistoryData GetLastView()
        {
            if (UIHistoryStack != null && UIHistoryStack.Count == 0)
            {
                return null;
            }
            return UIHistoryStack[UIHistoryStack.Count - 1];
        }
        //----------------------------------------------------------------------------
        static public void AddDelayHideUI(string showedUI, string hideUI)
        {
            if (showedUI == null || hideUI == null)
            {
                return;
            }
            if (!m_strDelayHide.ContainsKey(showedUI))
            {
                m_strDelayHide.Add(showedUI, hideUI);
            }
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
