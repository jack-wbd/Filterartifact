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
        private GDropDown dropdown;
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
            m_selectRedAll = m_uiTrans.Find(m_centerAnchorPath + "selRedAll").GetComponent<Toggle>();
            m_selectRedAll.onValueChanged.AddListener((bool isOn) => OnSelRedAllChange(isOn));
            m_selectBlueAll = m_uiTrans.Find(m_centerAnchorPath + "selBlueAll").GetComponent<Toggle>();
            m_selectBlueAll.onValueChanged.AddListener((bool isOn) => OnSelBlueAllChange(isOn));
            dropdown = GetUIComponent<GDropDown>(m_centerAnchorPath + "periodSelection");
            dropdown.OnValueChanged = OnDropValueChanged;
            if (drawData.tcbStatiDataList != null && drawData.tcbStatiDataList.Count > 0)
            {
                List<string> str = new List<string>();
                for (int i = 0; i < drawData.tcbStatiDataList.Count; i++)
                {
                    str.Add(drawData.tcbStatiDataList[i].numperiods.ToString());
                }
                dropdown.AddOptions(str);
            }
            dropdown.RefreshShowValue();
            for (int i = 0; i < RedBallMaxNumber; i++)
            {
                var redToggle = Object.Instantiate(redTemp, m_redParent) as GameObject;
                redToggle.name = (i + 1).ToString();
                redToggle.transform.localScale = Vector3.one;
                redToggle.transform.localRotation = Quaternion.identity;
                redToggle.transform.Find("Label").GetComponent<Text>().text = (i + 1).ToString();
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
            drawData.curSelectPeriod = drawData.tcbStatiDataList[index].numperiods;
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
                drawData.curSelectPeriod = drawData.tcbStatiDataList[dropdown.Index].numperiods;
            }
        }
        //----------------------------------------------------------------------------
        private bool OnSelRedAllChange(bool bstate)
        {
            SetRedBallToggleIsOn(bstate);
            for (int i = 0; i < drawData.redBallSelNumberList.Count; i++)
            {
                UpdateSelectedRedBallNumShow(drawData.redBallSelNumberList[i].ToString());
            }
            UpdateTotalShow();
            return true;
        }
        //----------------------------------------------------------------------------
        private bool OnSelBlueAllChange(bool bstate)
        {
            SetBlueBallToggleIsOn(bstate);
            for (int i = 0; i < drawData.blueBallSelNumberList.Count; i++)
            {
                UpdateSelectedBlueBallNumShow(drawData.blueBallSelNumberList[i].ToString());
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
            var num = byte.Parse(toggle.name);
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
                m_selectRedAll.isOn = false;
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
                m_totalLab.text = string.Format(PromptData.GetPrompt("totalbets"), Util.C(drawData.redBallSelNumberList.Count, 6) * blueBallCount);
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
            var num = byte.Parse(toggle.name);
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
                m_selectBlueAll.isOn = false;
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
