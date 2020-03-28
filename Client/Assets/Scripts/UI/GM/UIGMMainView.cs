//------------------------------------------------------------------------------
/**
	\file	UIGMMainView.cs

	Copyright (c) 2020, BoYue. All rights reserved.

	<PRE>

	模块名称：<文件所属的模块名称>
	文件名称：UIGMMainView.cs
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
//	UIGMMainView.cs
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Filterartifact
{
#if GMORDER
    public class UIGMMainView : UIBase
    {
        private GameObject m_btn;

        private Transform m_gridTf;

        private List<Item> m_btnList = new List<Item>();

        private int m_posIndex;

        private Vector2 m_btnSize;

        private Vector2 m_borderSize;

        protected override bool OnCreate()
        {
            base.OnCreate();
            FindUIObject();

            return true;
        }

        private void FindUIObject()
        {
            m_btnSize = new Vector2(300, 150);
            m_borderSize = new Vector2(100, 50);
            m_btn = m_topLeftAnchor.Find("Btn").gameObject;
            m_gridTf = m_topLeftAnchor.Find("table");
        }
        //----------------------------------------------------------------------------
        protected override void OnLateUpdate()
        {
            base.OnLateUpdate();

            for (int i = 0; i < m_btnList.Count; i++)
            {
                m_btnList[i].Update();
            }
        }

        public void ShowList(List<UIGMMainCtrl.UIGMMainInfo> infos)
        {
            Rest();

            while (m_btnList.Count < infos.Count)
            {
                CreateBtn();
            }

            for (int i = 0; i < m_btnList.Count; i++)
            {
                Item btnItem = m_btnList[i];

                if (i < infos.Count)
                {
                    var info = infos[i];
                    btnItem.Show(info);
                    btnItem.SetPos(GetNextPos());
                }
                else
                {
                    btnItem.Show(false);
                }
            }

        }

        private void CreateBtn()
        {
            GameObject go = m_btn.Clone(m_gridTf);
            m_btnList.Add(new Item(go.transform));
        }

        private void Rest()
        {
            m_posIndex = 0;
        }

        private int GetNextPos()
        {
            int height = -100 * m_posIndex++;
            return height;
        }

        private class Item
        {
            private Transform m_transform;
            private RectTransform m_rect;
            private ButtonItem m_button;
            private InputButtonItem m_inputBtn;
            private TextItem m_text;

            private bool isUpdate;

            public Item(Transform tf)
            {
                m_transform = tf;
                m_rect = m_transform.GetComponent<RectTransform>();
                m_button = new ButtonItem(tf.Find("Button"));
                m_inputBtn = new InputButtonItem(tf.Find("Input"));
                m_text = new TextItem(tf.Find("Text"));
            }

            public void Show(bool open)
            {
                m_transform.Visible(open);
            }

            public void Show(UIGMMainCtrl.UIGMMainInfo info)
            {
                Show(true);

                m_button.Show(info.type == UIGMMainCtrl.eUIGMMainItemType.Button);
                m_inputBtn.Show(info.type == UIGMMainCtrl.eUIGMMainItemType.InputButton);
                m_text.Show(info.type == UIGMMainCtrl.eUIGMMainItemType.Text);

                BtnBase btn = GetItem(info.type);
                if (btn != null)
                {
                    btn.Show(info);
                }
            }

            private BtnBase GetItem(UIGMMainCtrl.eUIGMMainItemType type)
            {
                isUpdate = type == UIGMMainCtrl.eUIGMMainItemType.Text;

                switch (type)
                {
                    case UIGMMainCtrl.eUIGMMainItemType.Button:
                        return m_button;
                    case UIGMMainCtrl.eUIGMMainItemType.InputButton:
                        return m_inputBtn;
                    case UIGMMainCtrl.eUIGMMainItemType.Text:
                        return m_text;
                    default:
                        return null;
                }
            }
            //----------------------------------------------------------------------------
            public void SetPos(int pos)
            {
                m_rect.anchoredPosition3D = new Vector3(0, pos, 0);
            }
            //----------------------------------------------------------------------------
            public void Update()
            {
                if (isUpdate && m_text != null)
                {
                    m_text.Update();
                }
            }
        }

        private class BtnBase
        {
            protected Transform m_transform;

            protected Text m_text;

            protected Button m_btn;

            public BtnBase(Transform tf)
            {
                m_transform = tf;
            }

            public void Show(bool open)
            {
                m_transform.Visible(open);
            }

            public virtual void Show(UIGMMainCtrl.UIGMMainInfo info)
            {
                Show(true);

                if (m_text)
                {
                    m_text.text = info.name;
                }
            }
        }

        private class ButtonItem : BtnBase
        {
            public ButtonItem(Transform tf) : base(tf)
            {
                m_text = tf.Find("label").GetComponent<Text>();

                m_btn = tf.GetComponent<Button>();
            }
            //----------------------------------------------------------------------------
            public override void Show(UIGMMainCtrl.UIGMMainInfo info)
            {
                base.Show(info);
                m_btn.onClick.AddListener(() => Action(info));
            }
            //----------------------------------------------------------------------------
            private void Action(UIGMMainCtrl.UIGMMainInfo info)
            {
                info.action?.Invoke();
            }
            //----------------------------------------------------------------------------
        }

        private class InputButtonItem : BtnBase
        {
            private InputField m_input;

            public InputButtonItem(Transform tf) : base(tf)
            {
                m_text = tf.Find("Button/label").GetComponent<Text>();

                m_input = tf.Find("InputField").GetComponent<InputField>();

                m_btn = tf.Find("Button").GetComponent<Button>();
            }

            public override void Show(UIGMMainCtrl.UIGMMainInfo info)
            {
                base.Show(info);
                m_btn.onClick.AddListener(() =>
                {
                    info.stringAction?.Invoke(m_input.text);
                });
            }
            //----------------------------------------------------------------------------
        }

        private class TextItem : BtnBase
        {
            private Text m_text;

            private string m_name;

            private Func<string> func;

            public TextItem(Transform tf) : base(tf)
            {
                m_text = tf.Find("background/label").GetComponent<Text>();
            }

            public override void Show(UIGMMainCtrl.UIGMMainInfo info)
            {
                base.Show(info);

                m_name = info.name;
                func = info.stringFunc;
            }

            public void Update()
            {
                if (!m_text || func == null)
                {
                    return;
                }

                m_text.text = m_name + " : " + func();
            }
        }
    }
#endif
}
