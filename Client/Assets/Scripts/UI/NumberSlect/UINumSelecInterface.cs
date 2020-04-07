//------------------------------------------------------------------------------
/**
	\file	UINumSelecInterface.cs

	Copyright (c) 2020, BoYue. All rights reserved.

	<PRE>

	模块名称�?文件所属的模块名称>
	文件名称：UINumSelecInterface.cs
	�?   要：<描述该文件实现的主要功能>

	当前版本�?.0
	建立日期�?020/3/29
	�?   者：SYSTEM
	电子邮件�?username%@BoYue.com
	�?   注：<其它说明>

	</PRE>
*/
//------------------------------------------------------------------------------
// Using
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
// Class
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	UINumSelecInterface.cs
//------------------------------------------------------------------------------
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.Dropdown;
using Object = UnityEngine.Object;

namespace Filterartifact
{
    public class UINumSelecInterface : UIBase
    {
        public readonly int RedBallMaxNumber = 33;
        public readonly int BlueBallMaxNumber = 16;
        private Transform m_redParent;
        private GameObject redTemp;
        private Transform m_blueParent;
        private GameObject blueTemp;
        private List<int> redList = new List<int>(33);
        private Transform selectedRedBallParent;
        private GameObject selectedRedBallTemp;
        private GameObject selectedBlueBallTemp;
        private Transform selectedBlueBallParent;
        private Dictionary<string, GameObject> redBallCloneDic = new Dictionary<string, GameObject>();
        private Dictionary<string, GameObject> blueBallCloneDic = new Dictionary<string, GameObject>();
        private Text m_totalLab;
        private Toggle m_selectRedAll;
        private Toggle m_selectBlueAll;
        private List<Toggle> redBallToggleList = new List<Toggle>();
        private List<Toggle> blueBallToggleList = new List<Toggle>();
        private Tween m_moveTween;
        private Dropdown dropdown;
        private Text m_date;
        private Text m_selectDate;
        private int index = 0;
        //----------------------------------------------------------------------------
        protected override bool OnCreate()
        {
            bool bResult = GetUIObject();
            if (bResult)
            {
                BindEvent(m_centerAnchorPath + "back").AddListener(() => OnClose());
                BindEvent(m_centerAnchorPath + "next").AddListener(() => OnNext());
            }
            base.OnCreate();
            return true;
        }
        //----------------------------------------------------------------------------
        public bool GetUIObject()
        {
            m_redParent = m_uiTrans.Find(m_centerAnchorPath + "redball");
            redTemp = m_redParent.Find("Button").gameObject;
            m_blueParent = m_uiTrans.Find(m_centerAnchorPath + "blueball");
            blueTemp = m_blueParent.Find("Button").gameObject;
            selectedRedBallParent = m_uiTrans.Find(m_centerAnchorPath + "selectedRedball");
            selectedRedBallTemp = selectedRedBallParent.Find("Button").gameObject;
            selectedBlueBallParent = m_uiTrans.Find(m_centerAnchorPath + "selectedBlueball");
            selectedBlueBallTemp = selectedBlueBallParent.Find("Button").gameObject;
            m_totalLab = m_uiTrans.Find(m_centerAnchorPath + "total").GetComponent<Text>();
            m_totalLab.text = string.Empty;
            m_date = m_uiTrans.Find(m_centerAnchorPath + "drawdate").GetComponent<Text>();
            m_selectDate = m_uiTrans.Find(m_centerAnchorPath + "period").GetComponent<Text>();
            m_selectRedAll = m_uiTrans.Find(m_centerAnchorPath + "selRedAll").GetComponent<Toggle>();
            m_selectRedAll.onValueChanged.AddListener((bool isOn) => OnSelRedAllChange(m_selectRedAll, isOn));
            m_selectBlueAll = m_uiTrans.Find(m_centerAnchorPath + "selBlueAll").GetComponent<Toggle>();
            m_selectBlueAll.onValueChanged.AddListener((bool isOn) => OnSelBlueAllChange(m_selectBlueAll, isOn));
            dropdown = GetUIComponent<Dropdown>(m_centerAnchorPath + "dropdown");
            dropdown.onValueChanged.AddListener((int value) => OnDropValueChanged(value));
            dropdown.options = new List<OptionData>();
            if (drawData.tcbStatiDataList != null && drawData.tcbStatiDataList.Count > 0)
            {
                for (int i = 0; i < drawData.tcbStatiDataList.Count; i++)
                {
                    OptionData data = new OptionData();
                    data.text = drawData.tcbStatiDataList[i].numperiods;
                    dropdown.options.Add(data);
                }
            }
            for (int i = 0; i < RedBallMaxNumber; i++)
            {
                var redToggle = Object.Instantiate(redTemp, m_redParent) as GameObject;
                redToggle.name = (i + 1).ToString();
                redToggle.transform.localScale = Vector3.one;
                redToggle.transform.localRotation = Quaternion.identity;
                redToggle.transform.Find("Label").GetComponent<Text>().text = (i + 1).ToString();
                redList.Add(i + 1);
                var toggle = redToggle.GetComponent<Toggle>();
                toggle.onValueChanged.AddListener((bool isOn) => OnRedBallToggleChange(toggle, isOn));
                redBallToggleList.Add(toggle);
            }
            Object.DestroyImmediate(redTemp);
            for (int i = 0; i < BlueBallMaxNumber; i++)
            {
                var blueToggle = Object.Instantiate(blueTemp, m_blueParent) as GameObject;
                blueToggle.name = (i + 1).ToString();
                blueToggle.transform.localScale = Vector3.one;
                blueToggle.transform.localRotation = Quaternion.identity;
                blueToggle.transform.Find("Label").GetComponent<Text>().text = (i + 1).ToString();
                Toggle toggle = blueToggle.GetComponent<Toggle>();
                toggle.onValueChanged.AddListener((bool isOn) => OnBlueBallToggleChange(toggle, isOn));
                blueBallToggleList.Add(toggle);
            }
            Object.DestroyImmediate(blueTemp);
            return true;
        }
        //----------------------------------------------------------------------------
        private void OnDropValueChanged(int value)
        {
            index = value;
            ShowView();
        }
        //----------------------------------------------------------------------------
        public override void Show(object arg = null)
        {
            base.Show(arg);
            ShowView();
        }
        //----------------------------------------------------------------------------
        private void ShowView()
        {
            if (drawData.tcbStatiDataList != null && drawData.tcbStatiDataList.Count > 0)
            {
                m_date.text = drawData.tcbStatiDataList[index].date;
                m_selectDate.text = string.Format(PromptData.GetPrompt("numperiod"), drawData.tcbStatiDataList[index].numperiods);
            }
        }
        //----------------------------------------------------------------------------
        private bool OnSelRedAllChange(Toggle toggle, bool bstate)
        {
            if (bstate)
            {
                SetRedBallToggleIsOn(true);
                for (int i = 1; i <= RedBallMaxNumber; i++)
                {
                    if (!drawData.redBallSelNumberList.Contains(i))
                    {
                        drawData.redBallSelNumberList.Add(i);
                    }
                }
                for (int i = 0; i < drawData.redBallSelNumberList.Count; i++)
                {
                    UpdateSelectedRedBallNumShow(drawData.redBallSelNumberList[i].ToString());
                }
            }
            else
            {
                SetRedBallToggleIsOn(false);
                if (drawData.redBallSelNumberList.Count > 0 && drawData.redBallSelNumberList != null)
                {
                    for (int i = 0; i < drawData.redBallSelNumberList.Count; i++)
                    {
                        HideSelectRedBallNumShow(drawData.redBallSelNumberList[i].ToString());
                    }
                    drawData.redBallSelNumberList.Clear();
                }
            }
            UpdateTotalShow();
            return true;
        }
        //----------------------------------------------------------------------------
        private bool OnSelBlueAllChange(Toggle toggle, bool bstate)
        {
            if (bstate)
            {
                SetBlueBallToggleIsOn(true);
                for (int i = 1; i <= BlueBallMaxNumber; i++)
                {
                    if (!drawData.blueBallSelNumberList.Contains(i))
                    {
                        drawData.blueBallSelNumberList.Add(i);
                    }
                }
                for (int i = 0; i < drawData.blueBallSelNumberList.Count; i++)
                {
                    UpdateSelectedBlueBallNumShow(drawData.blueBallSelNumberList[i].ToString());
                }
            }
            else
            {
                SetBlueBallToggleIsOn(false);
                if (drawData.blueBallSelNumberList.Count > 0 && drawData.blueBallSelNumberList != null)
                {
                    for (int i = 0; i < drawData.blueBallSelNumberList.Count; i++)
                    {
                        HideSelectBlueBallNumShow(drawData.blueBallSelNumberList[i].ToString());
                    }
                    drawData.blueBallSelNumberList.Clear();
                }
            }
            UpdateTotalShow();
            return true;
        }
        //----------------------------------------------------------------------------
        private void SetRedBallToggleIsOn(bool bstate)
        {
            for (int i = 0; i < redBallToggleList.Count; i++)
            {
                redBallToggleList[i].isOn = bstate;
            }
        }
        //----------------------------------------------------------------------------
        private void SetBlueBallToggleIsOn(bool bstate)
        {
            for (int i = 0; i < blueBallToggleList.Count; i++)
            {
                blueBallToggleList[i].isOn = bstate;
            }
        }
        //----------------------------------------------------------------------------
        private bool OnRedBallToggleChange(Toggle toggle, bool bstate)
        {
            int num = int.Parse(toggle.name);
            if (bstate)
            {
                if (!drawData.redBallSelNumberList.Contains(num))
                {
                    drawData.redBallSelNumberList.Add(num);
                }
                UpdateSelectedRedBallNumShow(toggle.name);
            }
            else
            {
                if (drawData.redBallSelNumberList.Contains(num))
                {
                    drawData.redBallSelNumberList.Remove(num);
                }
                HideSelectRedBallNumShow(toggle.name);
            }
            UpdateTotalShow();
            return true;
        }
        //----------------------------------------------------------------------------
        private void UpdateTotalShow()
        {
            var redBallCount = drawData.redBallSelNumberList.Count;
            var blueBallCount = drawData.blueBallSelNumberList.Count;
            if (redBallCount >= 6 && blueBallCount > 0)
            {
                //m_totalLab.text = "总注数：" + Util.GetCombination(drawData.redBallSelNumberList, 6).Count * blueBallCount;
                m_totalLab.text = string.Format(PromptData.GetPrompt("totalbets"), Util.C(drawData.redBallSelNumberList.Count, 6) * blueBallCount);
                if (drawData.resultList != null || drawData.resultList.Count > 0)
                {
                    drawData.resultList.Clear();
                }
                var list = Util.GetCombination(drawData.redBallSelNumberList, 6);
                for (int i = 0; i < list.Count; i++)
                {
                    string str = string.Empty;
                    var strList = list[i];
                    for (int j = 0; j < strList.Count; j++)
                    {
                        str += strList[j].ToString() + " ";
                    }
                    drawData.resultList.Add(str);
                }
            }
            else
            {
                m_totalLab.text = string.Empty;
            }
        }
        //----------------------------------------------------------------------------
        private void UpdateSelectedRedBallNumShow(string name)
        {
            if (redBallCloneDic.Count > 0 && redBallCloneDic != null)
            {
                if (redBallCloneDic.ContainsKey(name))
                {
                    redBallCloneDic[name].Visible(true);
                }
                else
                {
                    var redTemp = Object.Instantiate(selectedRedBallTemp, selectedRedBallParent) as GameObject;
                    redTemp.name = name;
                    redTemp.transform.localScale = Vector3.one;
                    redTemp.transform.localRotation = Quaternion.identity;
                    redTemp.transform.Find("Label").GetComponent<Text>().text = name;
                    redTemp.Visible(true);
                    redBallCloneDic.Add(name, redTemp);
                }
            }
            else
            {
                var redTemp = Object.Instantiate(selectedRedBallTemp, selectedRedBallParent) as GameObject;
                redTemp.name = name;
                redTemp.transform.localScale = Vector3.one;
                redTemp.transform.localRotation = Quaternion.identity;
                redTemp.transform.Find("Label").GetComponent<Text>().text = name;
                redTemp.Visible(true);
                redBallCloneDic.Add(name, redTemp);
            }
        }
        //----------------------------------------------------------------------------
        private void UpdateSelectedBlueBallNumShow(string name)
        {
            if (blueBallCloneDic.Count > 0 && blueBallCloneDic != null)
            {
                if (blueBallCloneDic.ContainsKey(name))
                {
                    blueBallCloneDic[name].Visible(true);
                }
                else
                {
                    var blueTemp = Object.Instantiate(selectedBlueBallTemp, selectedBlueBallParent) as GameObject;
                    blueTemp.name = name;
                    blueTemp.transform.localScale = Vector3.one;
                    blueTemp.transform.localRotation = Quaternion.identity;
                    blueTemp.transform.Find("Label").GetComponent<Text>().text = name;
                    blueTemp.Visible(true);
                    blueBallCloneDic.Add(name, blueTemp);
                }
            }
            else
            {
                var blueTemp = Object.Instantiate(selectedBlueBallTemp, selectedBlueBallParent) as GameObject;
                blueTemp.name = name;
                blueTemp.transform.localScale = Vector3.one;
                blueTemp.transform.localRotation = Quaternion.identity;
                blueTemp.transform.Find("Label").GetComponent<Text>().text = name;
                blueTemp.Visible(true);
                blueBallCloneDic.Add(name, blueTemp);
            }
        }
        //----------------------------------------------------------------------------
        private void HideSelectRedBallNumShow(string name)
        {
            if (redBallCloneDic.ContainsKey(name))
            {
                redBallCloneDic[name].Visible(false);
            }
        }
        //----------------------------------------------------------------------------
        private void HideSelectBlueBallNumShow(string name)
        {
            if (blueBallCloneDic.ContainsKey(name))
            {
                blueBallCloneDic[name].Visible(false);
            }
        }
        //----------------------------------------------------------------------------
        private bool OnBlueBallToggleChange(Toggle toggle, bool bstate)
        {
            int num = int.Parse(toggle.name);
            if (bstate)
            {
                if (!drawData.blueBallSelNumberList.Contains(num))
                {
                    drawData.blueBallSelNumberList.Add(num);
                }
                UpdateSelectedBlueBallNumShow(toggle.name);
            }
            else
            {
                if (drawData.blueBallSelNumberList.Contains(num))
                {
                    drawData.blueBallSelNumberList.Remove(num);
                }
                HideSelectBlueBallNumShow(toggle.name);
            }
            UpdateTotalShow();
            return true;
        }
        //----------------------------------------------------------------------------
        public void OnClose()
        {
            Messenger.Broadcast(DgMsgID.DgUI_HideNew, "UINumSelecInterfaceCtrl");
        }
        //----------------------------------------------------------------------------
        private void OnNext()
        {
            var moveSize = ScreenUnit.fWidth;
            m_moveTween = m_uiTrans.GetComponent<RectTransform>().DOLocalMove(new Vector2(-moveSize, 0), 0.1f);
            m_moveTween.onComplete = OnMoveComplete;
        }
        //----------------------------------------------------------------------------
        private void OnMoveComplete()
        {
            Hide();
            Messenger.Broadcast(DgMsgID.DgUI_ShowNew, "UIFilterMethodInterfaceCtrl");
        }
        //----------------------------------------------------------------------------
        public override void Hide()
        {
            base.Hide();
        }
        //----------------------------------------------------------------------------
        DrawData drawData
        {
            get
            {
                return WorldManager.Instance().GetDataCollection<DrawData>();
            }
        }
        //----------------------------------------------------------------------------
    }
}
