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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        //----------------------------------------------------------------------------
        protected override bool OnCreate()
        {
            bool bResult = GetUIObject();
            if (bResult)
            {
                BindEvent(m_centerAnchorPath + "back").AddListener(() => OnClose());
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
        private void TweenMove()
        {
            var moveSize = ScreenUnit.fWidth;
            m_moveTween = m_uiTrans.GetComponent<RectTransform>().DOLocalMove(new Vector2(-moveSize, 0), 0.1f);
            m_moveTween.onComplete = OnMoveComplete;
        }
        //----------------------------------------------------------------------------
        private void OnMoveComplete()
        {
            Hide();

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
