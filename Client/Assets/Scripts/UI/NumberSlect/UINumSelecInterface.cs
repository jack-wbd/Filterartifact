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
    public class UINumSelecInterface : UIBase
    {
        public readonly int RedBallMaxNumber = 33;
        public readonly int BlueBallMaxNumber = 16;
        Transform m_redParent;
        GameObject redTemp;
        Transform m_blueParent;
        GameObject blueTemp;
        int[] redList = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33 };
        //----------------------------------------------------------------------------
        protected override bool OnCreate()
        {
            bool bResult = GetUIObject();
            if (bResult)
            {
                BindEvent(m_centerAnchorPath + "back").AddListener(() => Hide());
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
            for (int i = 0; i < RedBallMaxNumber; i++)
            {
                var button = Object.Instantiate(redTemp, m_redParent) as GameObject;
                button.name = (i + 1).ToString();
                button.transform.localScale = Vector3.one;
                button.transform.localRotation = Quaternion.identity;
                if (i < 17)
                {
                    button.transform.localPosition = redTemp.transform.localPosition + new Vector3(80 * i, 0, 0);
                }
                else
                {
                    button.transform.localPosition = redTemp.transform.localPosition + new Vector3(80 * (i - 17), -100, 0);
                }
                button.transform.Find("Text").GetComponent<Text>().text = (i + 1).ToString();
            }
            Object.DestroyImmediate(redTemp);
            for (int i = 0; i < BlueBallMaxNumber; i++)
            {
                var button = Object.Instantiate(blueTemp, m_blueParent) as GameObject;
                button.name = (i + 1).ToString();
                button.transform.localScale = Vector3.one;
                button.transform.localRotation = Quaternion.identity;
                button.transform.localPosition = blueTemp.transform.localPosition + new Vector3(80 * i, 0, 0);
                button.transform.Find("Text").GetComponent<Text>().text = (i + 1).ToString();
            }
            Object.DestroyImmediate(blueTemp);
            Test();
            return true;
        }
        //----------------------------------------------------------------------------
        private void Test()
        {
            for (int i = 1; i <= BlueBallMaxNumber; i++)
            {
                var count = Util.GetCombination(redList, 6).Count * i;
                Debug.Log("从33个红球中选出6个红球" + i + "个篮球" + "共有" + count + " 种选法");
            }
        }
        //----------------------------------------------------------------------------
        public override void Show(object arg = null)
        {
            base.Show(arg);
        }
        //----------------------------------------------------------------------------
        public override void Hide()
        {
            base.Hide();
        }
        //----------------------------------------------------------------------------
    }
}
