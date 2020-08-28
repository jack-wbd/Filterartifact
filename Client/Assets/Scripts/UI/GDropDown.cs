//------------------------------------------------------------------------------
/**
	\file	GDropDown.cs

	Copyright (c) 2020, BoYue. All rights reserved.

	<PRE>

	模块名称：<文件所属的模块名称>
	文件名称：GDropDown.cs
	摘    要：<描述该文件实现的主要功能>

	当前版本：1.0
	建立日期：2020/7/21
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
//	GDropDown.cs
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
namespace Filterartifact
{
    [AddComponentMenu("UI/GDropdown", 100)]
    [RequireComponent(typeof(RectTransform))]
    public class GDropDown : MonoBehaviour
    {

        [Tooltip("Button of Whole Component")]
        [SerializeField]
        private Toggle m_CaptionToggle;
        /// <summary>
        /// Button of Whole Component
        /// </summary>
        public Toggle CaptionToggle { get { return m_CaptionToggle; } set { SetCaptionButton(value); } }

        [Tooltip("Display Text of Selected Item")]
        [SerializeField]
        private Text m_CaptionText;
        /// <summary>
        /// Display Text of Selected Item
        /// </summary>
        public Text CaptionText { get { return m_CaptionText; } set { m_CaptionText = value; } }

        [Tooltip("Display Image of Selected Item")]
        [SerializeField]
        private Image m_CaptionImage;
        /// <summary>
        /// Display Image of Selected Item
        /// </summary>
        public Image CaptionImage { get { return m_CaptionImage; } set { m_CaptionImage = value; } }

        [Space]

        [Tooltip("Drop List")]
        [SerializeField]
        private ScrollRect m_ScrollRect;
        /// <summary>
        /// Drop List 
        /// </summary>
        public ScrollRect ScrollRect { get { return m_ScrollRect; } set { m_ScrollRect = value; } }

        [Tooltip("Template of Drop List's Item")]
        [SerializeField]
        private GameObject m_DropItem;
        /// <summary>
        /// Template of Drop List's Item
        /// </summary>
        public GameObject DropItem { get { return m_DropItem; } set { SetDropItem(value); } }

        [Tooltip("Current Select Index")]
        [SerializeField]
        private int m_Index;
        /// <summary>
        /// Current Select Index
        /// </summary>
        public int Index { get { return m_Index; } set { SetSelectIndex(value); } }
        public string Text { get { return m_DropData.Count > Index ? m_DropData[Index].text : ""; } }

        /// <summary>
        /// Drop Down Value Changed
        /// </summary>
        public UnityAction<int> OnValueChanged;

        [Tooltip("Drop Data ")]
        [SerializeField]
        List<GItemData> m_DropData = new List<GItemData>();

        private ScrollList m_ScrollList;
        private Dictionary<Transform, GItem> m_Items = new Dictionary<Transform, GItem>();
        private RectTransform m_PointerMask;
        private Transform m_Canvas;

        void Awake()
        {
            m_ScrollList = GetOrAddComponent<ScrollList>(ScrollRect.gameObject);
            m_ScrollList.onItemRender = OnItemRender;
            SetSelectIndex(m_Index);
            SetDropItem(m_DropItem);
            RefreshShowValue();

            CaptionToggle = GetComponent<Toggle>();

            SetCaptionButton(m_CaptionToggle);

            m_CaptionToggle.isOn = false;
            CloseMask();
        }

        public void AddOptions(List<string> options)
        {
            for (int i = 0; i < options.Count; i++)
                m_DropData.Add(new GItemData(options[i]));

            if (m_CaptionToggle.isOn)
                OpenMask();
        }

        public void AddOptions(Sprite[] options)
        {
            for (int i = 0; i < options.Length; i++)
                this.m_DropData.Add(new GItemData(options[i]));

            if (m_CaptionToggle.isOn)
                OpenMask();
        }

        public void AddOptions(GItemData[] options)
        {
            for (int i = 0; i < options.Length; i++)
                this.m_DropData.Add(options[i]);

            if (m_CaptionToggle.isOn)
                OpenMask();
        }

        public void RemoveAt(int index)
        {
            if (this.m_DropData.Count > index)
            {
                this.m_DropData.RemoveAt(index);
                if (index == Index)
                {
                    SetSelectIndex(index - 1);
                }
            }
        }

        public void ClearOptions()
        {
            this.m_DropData.Clear();
            if (m_CaptionToggle.isOn)
                OpenMask();
        }


        /// <summary>
        /// Refresh Display View
        /// </summary>
        public void RefreshShowValue()
        {
            if (m_DropData.Count > Index)
            {
                GItemData data = m_DropData[Index];
                if (CaptionText != null)
                    CaptionText.text = m_DropData[Index].text;
                if (CaptionImage != null)
                    CaptionImage.sprite = data.image;
            }
            else
            {
                if (CaptionText != null)
                    CaptionText.text = "";
                if (CaptionImage != null)
                    CaptionImage.sprite = null;
            }
        }

        /// <summary>
        /// Render Item in List
        /// </summary>
        /// <param name="index"></param>
        /// <param name="child"></param>
        private void OnItemRender(int index, Transform child)
        {
            GItem item;
            if (!m_Items.TryGetValue(child, out item))
                item = new GItem(this, child);

            if (m_DropData.Count > index)
            {
                item.Reset(m_DropData[index].text, m_DropData[index].image, index == Index);
            }
        }

        /// <summary>
        /// Set Cur Select Index When Click
        /// </summary>
        /// <param name="p"></param>
        internal void SetSelectIndex(int p)
        {
            if (p < m_DropData.Count) //if exist data
            {
                m_Index = p;
                RefreshShowValue();
                OnValueChanged?.Invoke(m_Index);
            }
            else
            {
                if (m_DropData.Count > 0)//Back To First
                {
                    m_Index = 0;
                    RefreshShowValue();
                    OnValueChanged?.Invoke(m_Index);
                }
            }

            foreach (var value in m_Items.Values)
            {
                value.SetActive(false);
            }
            m_CaptionToggle.isOn = false;
        }

        /// <summary>
        /// Open Mask to Poniters Out of List
        /// </summary>
        private void OpenMask()
        {

            if (m_PointerMask == null)//Create Mask
            {
                GameObject o = new GameObject("Pointer Mask");
                o.transform.SetParent(transform);
                Image mask = o.AddComponent<Image>();
                mask.color = new Color(1, 1, 1, 0);
                m_PointerMask = o.transform as RectTransform;
                m_PointerMask.sizeDelta = new Vector2(Screen.width, Screen.height);
                Button btnMask = o.AddComponent<Button>();
                btnMask.onClick.AddListener(CloseMask);
            }

            if (m_Canvas == null)//Find Canvas
            {
                Canvas canvas = FindObjectOfType<Canvas>();
                m_Canvas = canvas.transform;
            }

            m_PointerMask.gameObject.SetActive(true);
            m_PointerMask.SetParent(m_Canvas);
            m_PointerMask.localPosition = Vector3.zero;

            if (m_ScrollRect != null)
            {
                m_ScrollRect.transform.SetParent(m_Canvas);
                m_ScrollRect.gameObject.SetActive(true);
                m_ScrollList.ChildCount = m_DropData.Count;
                m_ScrollList.DropData = m_DropData;
            }
        }

        /// <summary>
        /// Close Mask to Other Pointers
        /// </summary>
        private void CloseMask()
        {
            m_CaptionToggle.isOn = false;
            if (m_PointerMask != null)
            {
                m_PointerMask.transform.SetParent(transform);
                m_PointerMask.gameObject.SetActive(false);
            }

            if (m_ScrollRect != null)
            {
                m_ScrollRect.transform.SetParent(transform);
                m_ScrollRect.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// Set Drop Item, Set Anchor Left-Top
        /// </summary>
        /// <param name="item"></param>
        private void SetDropItem(GameObject item)
        {
            RectTransform rect = item.transform as RectTransform;
            rect.anchorMin = Vector2.up;
            rect.anchorMax = Vector2.up;
            m_DropItem = item;
            m_ScrollList.Child = item;
        }

        /// <summary>
        /// Set Outter Button of Whole Component
        /// </summary>
        /// <param name="btn"></param>
        private void SetCaptionButton(Toggle btn)
        {
            if (m_CaptionToggle != null)
                m_CaptionToggle.onValueChanged.RemoveListener(OnCaptionButtonClicked);

            m_CaptionToggle = btn;
            if (m_CaptionToggle != null)
                m_CaptionToggle.onValueChanged.AddListener(OnCaptionButtonClicked);
        }

        /// <summary>
        /// On Caption Button Clicked
        /// </summary>
        private void OnCaptionButtonClicked(bool active)
        {
            if (active)
                OpenMask();
            else
                CloseMask();
        }

        /// <summary>
        /// Get Or Add Component on O
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="o"></param>
        /// <returns></returns>
        T GetOrAddComponent<T>(GameObject o) where T : Component
        {
            T com = o.GetComponent<T>();
            if (com == null)
                com = o.AddComponent<T>();
            return com;
        }

        /// <summary>
        /// Release All
        /// </summary>
        public void OnDestroy()
        {
            //If Release on Drop State, Delete Mask
            if (m_PointerMask != null)
                m_PointerMask.SetParent(transform);
        }

        /// <summary>
        /// Renderer Item in Endless List
        /// </summary>
        protected internal class GItem
        {
            GDropDown dropDown;
            Transform item;
            Button btn;

            Text text;
            Image image;
            GameObject selected;
            bool activeSelf;

            public string m_Text { get { return text.text; } set { text.text = value; } }
            public Sprite m_Image { get { return image.sprite; } set { image.sprite = value; } }

            public GItem(GDropDown parent, Transform item)
            {
                this.dropDown = parent;
                this.item = item;
                activeSelf = false;

                Transform t_trans = item.Find("text");
                if (t_trans)
                {
                    text = t_trans.gameObject.GetComponent<Text>();
                }
                Transform t_image = item.Find("image");
                if (t_image)
                {
                    image = t_image.gameObject.GetComponent<Image>();
                }
                Transform t_selected = item.Find("selected");
                if (t_selected)
                {
                    selected = t_selected.gameObject;
                }

                btn = item.GetComponent<Button>();
                if (btn == null)
                {
                    Transform t_btn = item.Find("btn");
                    if (t_btn != null)
                        btn = dropDown.GetOrAddComponent<Button>(t_btn.gameObject);
                    else
                        btn = dropDown.GetOrAddComponent<Button>(item.gameObject);
                }

                btn.onClick.AddListener(OnBtnItemClicked);
            }

            private void OnBtnItemClicked()
            {
                if (!activeSelf)
                {
                    SetActive(true);
                    dropDown.SetSelectIndex(int.Parse(item.name));
                }
            }

            internal void SetActive(bool active)
            {
                this.activeSelf = active;
                if (selected != null)
                    selected.SetActive(active);
            }


            internal void Reset(string txt, Sprite sprite, bool active)
            {
                if (text != null)
                    m_Text = txt;
                if (image != null)
                    m_Image = sprite;
                SetActive(active);
            }
        }

        /// <summary>
        /// Cache Data
        /// </summary>
        [Serializable]
        public class GItemData
        {
            [SerializeField]
            private string m_Text;
            [SerializeField]
            private Sprite m_Image;
            public string text { get { return m_Text; } set { m_Text = value; } }
            public Sprite image { get { return m_Image; } set { m_Image = value; } }
            public GItemData() { }

            public GItemData(string text)
            {
                this.text = text;
            }

            public GItemData(Sprite image)
            {
                this.image = image;
            }

            public GItemData(string text, Sprite image)
            {
                this.text = text;
                this.image = image;
            }
        }
    }
}