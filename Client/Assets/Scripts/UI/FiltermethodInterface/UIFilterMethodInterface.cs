//------------------------------------------------------------------------------
/**
	\file	UIFilterMethodInterface.cs

	Copyright (c) 2020, BoYue. All rights reserved.

	<PRE>

	模块名称：<文件所属的模块名称>
	文件名称：UIFilterMethodInterface.cs
	摘    要：<描述该文件实现的主要功能>

	当前版本：1.0
	建立日期：2020/4/2
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
//	UIFilterMethodInterface.cs
//------------------------------------------------------------------------------
using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Filterartifact
{
    public class UIFilterMethodInterface : UIBase
    {
        //----------------------------------------------------------------------------
        private Tween m_moveTween;
        private Transform selectedRedBallParent;
        private Transform selectedBlueBallParent;
        private GameObject selectedRedBallTemp;
        private GameObject selectedBlueBallTemp;
        private Dictionary<string, GameObject> redBallCloneDict = new Dictionary<string, GameObject>();
        private Dictionary<string, GameObject> blueBallCloneDict = new Dictionary<string, GameObject>();
        private LoopScrollerView loopScrollView;
        private Toggle m_ballRotation;
        private Text m_redBallLab;
        private Text m_blueBallLab;
        private Text m_totalLab;
        private int curRedBallSelNumber;
        private int curBlueBallSelNumber;
        private const int Six = 6;
        //----------------------------------------------------------------------------
        protected override bool OnCreate()
        {
            bool bResult = GetUIObject();
            if (bResult)
            {
                BindEvent(m_centerAnchorPath + "back").AddListener(() => OnClose());
                BindEvent(m_downAnchorPath + "btn6").AddListener(() => SixInSix());
                BindEvent(m_downAnchorPath + "btn5").AddListener(() => SixInFive());
                BindEvent(m_downAnchorPath + "next").AddListener(() => OnNext());
            }
            return true;
        }
        //----------------------------------------------------------------------------
        public bool GetUIObject()
        {
            if (m_objUI != null)
            {
                selectedRedBallParent = GetUIComponent<Transform>(m_centerAnchorPath + "selectRedBallGroup");
                selectedRedBallTemp = selectedRedBallParent.FindChildGO("Button");
                selectedBlueBallParent = GetUIComponent<Transform>(m_centerAnchorPath + "selectBlueBallGroup");
                selectedBlueBallTemp = selectedBlueBallParent.FindChildGO("Button");
                loopScrollView = GetUIComponent<LoopScrollerView>(m_centerAnchorPath + "scrollView");
                m_ballRotation = GetUIComponent<Toggle>(m_downAnchorPath + "toggle");
                m_ballRotation.onValueChanged.AddListener((bool isOn) => IsToggleOn(isOn));
                m_redBallLab = GetUIComponent<Text>(m_centerAnchorPath + "redBallNumber");
                m_blueBallLab = GetUIComponent<Text>(m_centerAnchorPath + "blueBallNumber");
                m_totalLab = GetUIComponent<Text>(m_centerAnchorPath + "total");
            }
            return true;
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
            var redDataList = drawData.redBallSelNumberList;
            for (int i = 0; i < redDataList.Count; i++)
            {
                var name = redDataList[i].ToString();
                if (redBallCloneDict.Count > 0 && redBallCloneDict != null)
                {

                    if (redBallCloneDict.ContainsKey(name))
                    {
                        redBallCloneDict[name].Visible(true);
                    }
                    else
                    {
                        var redTemp = Object.Instantiate(selectedRedBallTemp, selectedRedBallParent) as GameObject;
                        redTemp.name = name;
                        redTemp.transform.localScale = Vector3.one;
                        redTemp.transform.localRotation = Quaternion.identity;
                        redTemp.transform.Find("Label").GetComponent<Text>().text = name;
                        redTemp.Visible(true);
                        redBallCloneDict.Add(name, redTemp);
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
                    redBallCloneDict.Add(name, redTemp);
                }
            }

            var blueDataList = drawData.blueBallSelNumberList;
            for (int i = 0; i < blueDataList.Count; i++)
            {
                var name = blueDataList[i].ToString();
                if (blueBallCloneDict.Count > 0 && blueBallCloneDict != null)
                {

                    if (blueBallCloneDict.ContainsKey(name))
                    {
                        blueBallCloneDict[name].Visible(true);
                    }
                    else
                    {
                        var redTemp = Object.Instantiate(selectedBlueBallTemp, selectedBlueBallParent) as GameObject;
                        redTemp.name = name;
                        redTemp.transform.localScale = Vector3.one;
                        redTemp.transform.localRotation = Quaternion.identity;
                        redTemp.transform.Find("Label").GetComponent<Text>().text = name;
                        redTemp.Visible(true);
                        blueBallCloneDict.Add(name, redTemp);
                    }
                }
                else
                {
                    var redTemp = Object.Instantiate(selectedBlueBallTemp, selectedBlueBallParent) as GameObject;
                    redTemp.name = name;
                    redTemp.transform.localScale = Vector3.one;
                    redTemp.transform.localRotation = Quaternion.identity;
                    redTemp.transform.Find("Label").GetComponent<Text>().text = name;
                    redTemp.Visible(true);
                    blueBallCloneDict.Add(name, redTemp);
                }
            }
            m_redBallLab.text = string.Format(PromptData.GetPrompt("redBallColor"), drawData.redBallSelNumberList.Count);
            m_redBallLab.color = UseColor.red;
            m_blueBallLab.text = string.Format(PromptData.GetPrompt("blueBallColor"), drawData.blueBallSelNumberList.Count);
            m_blueBallLab.color = UseColor.blue;
        }
        //----------------------------------------------------------------------------
        protected override void UpdateLoopView(List<List<byte>> list, LoopScrollerView scrollView)
        {
            base.UpdateLoopView(list, scrollView);
        }
        //----------------------------------------------------------------------------
        public override void Hide()
        {
            base.Hide();
            HideAll();
        }
        private void HideAll()
        {
            if (redBallCloneDict.Count > 0 && redBallCloneDict != null)
            {
                foreach (var item in redBallCloneDict)
                {
                    item.Value.Visible(false);
                }
            }

            if (blueBallCloneDict.Count > 0 && blueBallCloneDict != null)
            {
                foreach (var item in blueBallCloneDict)
                {
                    item.Value.Visible(false);
                }
            }
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
            Messenger.Broadcast<string, object>(DgMsgID.DgUI_ShowNewOneParam, "UIPopularNumFilterInterfaceCtrl", drawData.resultList);
        }
        //----------------------------------------------------------------------------
        private void IsToggleOn(bool isOn)
        {

        }
        //----------------------------------------------------------------------------
        private void SixInSix()
        {
            curRedBallSelNumber = drawData.redBallSelNumberList.Count;
            curBlueBallSelNumber = drawData.blueBallSelNumberList.Count;
            var blueBallCount = drawData.blueBallSelNumberList.Count;
            long totalCount;
            List<List<byte>> combinResult;
            if (m_ballRotation.isOn)
            {
                totalCount = Util.C(drawData.redBallSelNumberList.Count, 6);
                combinResult = Util.GetCombination(drawData.redBallSelNumberList, Six, drawData.blueBallSelNumberList, true);

            }
            else
            {
                totalCount = Util.C(drawData.redBallSelNumberList.Count, 6) * blueBallCount;
                combinResult = Util.GetCombination(drawData.redBallSelNumberList, Six, drawData.blueBallSelNumberList, false);
            }

            m_totalLab.text = string.Format(PromptData.GetPrompt("totalbets"), totalCount);
            m_totalLab.color = UseColor.pink;
            drawData.resultList.Clear();
            drawData.resultList = combinResult;
            UpdateLoopView(combinResult, loopScrollView);

        }
        //----------------------------------------------------------------------------
        private void SixInFive()
        {
            curRedBallSelNumber = drawData.redBallSelNumberList.Count;

            if (curRedBallSelNumber < 8 || curRedBallSelNumber > 12)
            {
                Messenger.Broadcast<string, object>(DgMsgID.DgMsg_ShowUIOneParam, "UIErrorCtrl", PromptData.GetPrompt("selNumLimit"));
                return;
            }

            curBlueBallSelNumber = drawData.blueBallSelNumberList.Count;
            var blueBallCount = drawData.blueBallSelNumberList.Count;
            long totalCount;

            var list = RotationMatrix.GetRotationMatrixResult(curRedBallSelNumber, drawData.redBallSelNumberList);
            if (m_ballRotation.isOn)
            {
                totalCount = list.Count;
            }
            else
            {
                totalCount = list.Count * blueBallCount;
            }
            m_totalLab.text = string.Format(PromptData.GetPrompt("totalbets"), totalCount);
            m_totalLab.color = UseColor.pink;
            drawData.resultList.Clear();
            drawData.resultList = list;
            UpdateLoopView(list, loopScrollView);
        }
        //----------------------------------------------------------------------------
        private void OnClose()
        {
            Messenger.Broadcast(DgMsgID.DgUI_HideNew, "UIFilterMethodInterfaceCtrl");
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
