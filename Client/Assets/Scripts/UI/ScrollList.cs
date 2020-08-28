using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Filterartifact.GDropDown;

namespace Filterartifact
{
    /// <summary>
    /// Introduction: �����б�
    ///             Content�Ͻ�ֹ����ContentSizeFilter��LayOutGroup֮�����
    /// Author: 	Cheng
    /// Time: 
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(ScrollRect))]
    public class ScrollList : MonoBehaviour
    {
        public delegate void OnItemRender(int index, Transform child);

        public OnItemRender onItemRender;

        /// <summary>
        /// ����ʽ
        /// </summary>
        public enum Arrangement
        {
            /// <summary>
            /// ����
            /// </summary>
            Horizontal = 0,

            /// <summary>
            /// ����
            /// </summary>
            Vertical,
        }

        /// <summary>
        /// ˮƽ����
        /// </summary>
        public enum HorizontalAlign
        {
            /// <summary>
            /// ����
            /// </summary>
            Left,

            /// <summary>
            /// ����
            /// </summary>
            Middle,

            /// <summary>
            /// ����
            /// </summary>
            Right,
        }

        /// <summary>
        /// ��ֱ����
        /// </summary>
        public enum VerticalAlign
        {
            /// <summary>
            /// ����
            /// </summary>
            Top,

            /// <summary>
            /// ����
            /// </summary>
            Middle,

            /// <summary>
            /// ����
            /// </summary>
            Bottom,
        }


        public Arrangement arrangement = Arrangement.Vertical;

        /// <summary>
        /// ��ѡ��ˮƽ��ֱ���������ã�ָÿ��/��������
        /// </summary>
        public int MaxPerLine
        {
            get { return maxPerLine; }
            set { SetMaxPerLine(value); }
        }

        /// <summary>
        /// �о�
        /// </summary>
        public float rowSpace = 0;

        /// <summary>
        /// �о�
        /// </summary>
        public float columuSpace = 0;

        public HorizontalAlign horizontalAlign = HorizontalAlign.Left;
        public VerticalAlign verticalAlign = VerticalAlign.Top;

        /// <summary>
        /// ��Ե���� ��
        /// </summary>
        public float marginTop = 0;

        /// <summary>
        /// ��Ե���� ��
        /// </summary>
        public float marginBottom = 0;

        /// <summary>
        /// ��Ե���� ��
        /// </summary>
        public float marginLeft = 0;

        /// <summary>
        /// ��Ե���� ��
        /// </summary>
        public float marginRight = 0;

        /// <summary>
        /// ��Ⱦ�ӽڵ�
        /// </summary>
        public GameObject Child
        {
            get { return item; }
            set { SetItem(value); }
        }

        /// <summary>
        /// �ܸ���
        /// </summary>
        public int ChildCount
        {
            get { return childCount; }
            set { SetChildCount(value, true); }
        }
        public List<GItemData> DropData;

        /// <summary>
        /// ������ʾ���ڴ�С
        /// </summary>
        public Vector2 ViewPort
        {
            get { return viewPort; }
            set { SetViewPort(value); }
        }

        /// <summary>
        /// ��Ⱦ�ӽڵ�
        /// </summary>
        public GameObject Item
        {
            get { return item; }
            set { SetItem(value); }
        }

        public GameObject item;
        ScrollRect scrollRect;
        public Vector2 viewPort;
        RectTransform content;
        Vector2 itemSize;
        List<Transform> items;
        Dictionary<int, int> contains;
        List<int> outOfContains;
        public int childCount = 0; //��Ҫ��Ⱦ�������ݸ���
        int scrollLineIndex; //��ǰ��һ��Ԫ������
        int totalCount; //��UI����ʾ�ĸ���(������maxPerLine)
        Vector2 startPos; //��һ��Ԫ������λ��
        int startIndex; //��ǰ��Ⱦ��ʼ����
        int endIndex; //��ǰ��Ⱦ��������
        public int maxPerLine;

        void Start()
        {
            maxPerLine = maxPerLine == 0 ? 1 : maxPerLine;
            items = new List<Transform>();
            contains = new Dictionary<int, int>();
            outOfContains = new List<int>();
            scrollRect = transform.GetComponent<ScrollRect>();
            content = scrollRect.content;
            if (content == null)
            {
                Debug.Log("ScrollRect " + scrollRect.gameObject.name + " Has No Content, Please Check And Retry.");
                return;
            }
            //viewPort = scrollRect.viewport.rect.size;

            content.anchorMax = new Vector2(0, 1);
            content.anchorMin = new Vector2(0, 1);
            content.pivot = new Vector2(0, 1);
            ReBuild();
        }


        /// <summary>
        /// ���ӽڵ㡢Mask��maxPerLine
        /// </summary>
        public void ReBuild()
        {
            if (scrollRect == null || content == null || item == null) return;
            ResetChildren();

            Vector2 maskSize = viewPort;
            int count = 0;

            if (arrangement == Arrangement.Horizontal)
            {
                count = Mathf.CeilToInt(maskSize.x / itemSize.x) + 1; //��������
                startPos = Vector2.zero;
                startPos.x = marginLeft;
                if (verticalAlign == VerticalAlign.Top)
                {
                    startPos.y = -marginTop;
                }
                else if (verticalAlign == VerticalAlign.Middle)
                {
                    startPos.y = -(maskSize.y * 0.5f - (itemSize.y * maxPerLine + (maxPerLine - 1) * rowSpace) * 0.5f);
                }
                else if (verticalAlign == VerticalAlign.Bottom)
                {
                    startPos.y = -(maskSize.y - marginBottom - itemSize.y * maxPerLine - rowSpace * (maxPerLine - 1));
                }
                //�Ż�������һ��ʼ�������еĿɼ�����
                //for (int i = 0; i < count; i++)
                //{
                //    for (int j = 0; j < maxPerLine; j++)
                //    {
                //        RectTransform child = CreateItem(i*maxPerLine + j);
                //        child.localPosition = startPos +
                //                              new Vector2(i*itemSize.x + i*columuSpace, -j*itemSize.y - j*rowSpace);
                //    }
                //}
            }
            else if (arrangement == Arrangement.Vertical)
            {
                count = Mathf.CeilToInt(maskSize.y / itemSize.y) + 1; //��������
                startPos = Vector2.zero;
                startPos.y = -marginTop; //���ÿ�ʼ�ڵ�λ��
                if (horizontalAlign == HorizontalAlign.Left)
                {
                    startPos.x = marginLeft;
                }
                else if (horizontalAlign == HorizontalAlign.Middle)
                {
                    startPos.x = (maskSize.x * 0.5f - (itemSize.x * maxPerLine + (maxPerLine - 1) * columuSpace) * 0.5f);
                }
                else if (horizontalAlign == HorizontalAlign.Right)
                {
                    startPos.x = maskSize.x - marginRight - itemSize.x * maxPerLine - columuSpace * (maxPerLine - 1);
                }

                //for (int i = 0; i < count; i++)
                //{
                //    for (int j = 0; j < maxPerLine; j++)
                //    {
                //        RectTransform child = CreateItem(i*maxPerLine + j);
                //        child.localPosition = startPos +
                //                              new Vector2(j*itemSize.x + j*columuSpace, -i*itemSize.y - i*rowSpace);
                //    }
                //}
            }
            totalCount = count;

            SetChildCount(childCount, true);
            BackTop();

            scrollRect.onValueChanged.RemoveAllListeners();
            scrollRect.onValueChanged.AddListener(OnValueChanged);
        }

        /// <summary>
        /// �б����
        /// </summary>
        /// <param name="vec"></param>
        private void OnValueChanged(Vector2 vec)
        {
            switch (arrangement)
            {
                case Arrangement.Horizontal:
                    //   if (vec.x < 0.0f || vec.x >= 1.0f)
                    //       return;
                    vec.x = Mathf.Clamp(vec.x, 0, 1);
                    break;
                case Arrangement.Vertical:
                    //   if (vec.y <= 0.0f || vec.y >= 1.0f)
                    //       return;
                    vec.y = Mathf.Clamp(vec.y, 0, 1);
                    break;
            }

            int curLineIndex = GetCurLineIndex();
            if (curLineIndex != scrollLineIndex)
                UpdateRectItem(curLineIndex, false);
        }

        /// <summary>
        /// ��ȡҳ���һ������
        /// </summary>
        /// <returns></returns>
        private int GetCurLineIndex()
        {
            switch (arrangement)
            {
                case Arrangement.Horizontal:
                    return
                        Mathf.FloorToInt(Mathf.Abs(content.anchoredPosition.x < 0.1f ? content.anchoredPosition.x : 0.1f - marginLeft) /
                                         (columuSpace + itemSize.x));
                case Arrangement.Vertical:
                    return
                        Mathf.FloorToInt(Mathf.Abs(content.anchoredPosition.y > -0.1f ? content.anchoredPosition.y : -0.1f - marginTop) /
                                         (rowSpace + itemSize.y));
            }
            return 0;
        }

        /// <summary>
        /// �������ݣ����޸��ʳ��ֵĲ�ˢ�£�
        /// </summary>
        /// <param name="curLineIndex"></param>
        /// <param name="forceRender"></param>
        private void UpdateRectItem(int curLineIndex, bool forceRender)
        {
            if (curLineIndex < 0)
                return;
            startIndex = curLineIndex * maxPerLine;
            endIndex = (curLineIndex + totalCount) * maxPerLine;
            if (endIndex >= childCount)
                endIndex = childCount;

            contains.Clear(); //��Ⱦ���
            outOfContains.Clear(); //items������
            for (int i = 0; i < items.Count; i++)//�����ǰ����Ⱦ��item�а���
            {
                int index = int.Parse(items[i].gameObject.name);
                if (index < startIndex || index >= endIndex)
                {
                    outOfContains.Add(i);
                    items[i].gameObject.SetActive(false);
                }
                else
                {
                    items[i].gameObject.SetActive(true);
                    contains.Add(index, i);
                }
            }

            // *************������Ⱦ****************
            for (int i = startIndex; i < endIndex; i++)
            {
                if (!contains.ContainsKey(i))
                {
                    Transform child = items[outOfContains[0]];
                    outOfContains.RemoveAt(0);
                    child.gameObject.SetActive(true);
                    int row = i / maxPerLine;
                    int col = i % maxPerLine;
                    if (arrangement == Arrangement.Vertical)
                        child.localPosition = startPos +
                                              new Vector2(col * itemSize.x + (col) * columuSpace,
                                                  -row * itemSize.y - (row) * rowSpace);
                    else
                        child.localPosition = startPos +
                                              new Vector2(row * itemSize.x + (row) * columuSpace,
                                                  -col * itemSize.y - (col) * rowSpace);
                    child.gameObject.name = i.ToString();
                    child.Find("Text").GetComponent<Text>().text = DropData[i].text;
                    onItemRender?.Invoke(i, child);
                }
                else if (forceRender)
                {
                    onItemRender?.Invoke(i, items[contains[i]]);
                }
            }

            scrollLineIndex = curLineIndex;
        }

        /// <summary>
        /// �Ƴ���ǰ����
        /// </summary>
        private void ResetChildren()
        {
            items.Clear();
            for (int i = 0; i < content.childCount; i++)
            {
                Transform child = content.GetChild(i);
                child.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// �����½ڵ�
        /// </summary>
        /// <param name="index"></param>
        private RectTransform CreateItem(int index)
        {
            Transform child;
            if (content.childCount > index)
            {
                child = content.GetChild(index);
            }
            else
            {
                GameObject obj = Instantiate(item) as GameObject;
                obj.transform.SetParent(content);
                obj.transform.localScale = Vector3.one;
                child = obj.transform;
            }
            child.gameObject.name = index.ToString();
            child.Find("Text").GetComponent<Text>().text = DropData[index].text;
            items.Add(child);

            return child as RectTransform;
        }

        /// <summary>
        /// ������Դ
        /// </summary>
        /// <param name="child"></param>
        public void SetItem(GameObject child)
        {
            if (child == null) return;
            this.item = child;
            RectTransform itemTrans = child.transform as RectTransform;
            itemTrans.pivot = new Vector2(0, 1);
            itemSize = itemTrans.sizeDelta;
            ReBuild();
        }

        /// <summary>
        /// ������Ҫ��Ⱦ�ĸ���
        /// </summary>
        /// <param name="value"></param>
        public void SetChildCount(int value, bool forceRender)
        {
            if (value < 0) childCount = 0;
            else childCount = value;

            if (totalCount <= 0)//��δ��ʼ��
                return;
            if (value > items.Count && items.Count < maxPerLine * totalCount)
            {
                //��ǰ������������Ӧ���ɵ�����
                int count = items.Count;
                int max = value < maxPerLine * totalCount ? value : maxPerLine * totalCount;
                for (int i = count; i < max; i++)
                {
                    int row = i / maxPerLine;
                    int col = i % maxPerLine;
                    RectTransform child = CreateItem(i);
                    if (arrangement == Arrangement.Vertical)
                        child.localPosition = startPos +
                                              new Vector2(col * itemSize.x + (col) * columuSpace,
                                                  -row * itemSize.y - (row) * rowSpace);
                    else
                        child.localPosition = startPos +
                                              new Vector2(row * itemSize.x + (row) * columuSpace,
                                                  -col * itemSize.y - (col) * rowSpace);
                }
            }

            if (content == null) return;

            int rc = Mathf.CeilToInt((float)childCount / (float)maxPerLine); //����content�Ĵ�С
            if (arrangement == Arrangement.Horizontal)
            {
                content.sizeDelta = new Vector2(marginLeft + marginRight + itemSize.x * rc + columuSpace * (rc - 1),
                    viewPort.y);
                if (content.sizeDelta.x > viewPort.x && content.anchoredPosition.x < viewPort.x - content.sizeDelta.x)
                    content.anchoredPosition = new Vector2(viewPort.x - content.sizeDelta.x, content.anchoredPosition.y);
            }
            else
            {
                content.sizeDelta = new Vector2(viewPort.x, marginTop + marginBottom + itemSize.y * rc + rowSpace * (rc - 1));
                if (content.sizeDelta.y > viewPort.y && content.anchoredPosition.y > content.sizeDelta.y - viewPort.y)
                    content.anchoredPosition = new Vector2(content.anchoredPosition.x, content.sizeDelta.y - viewPort.y);
            }
            UpdateRectItem(GetCurLineIndex(), true);
        }

        /// <summary>
        /// ����ӽڵ�
        /// </summary>
        /// <param name="index"></param>
        public void AddChild(int index)
        {
            if (index < 0) return;
            startIndex = scrollLineIndex * maxPerLine;
            endIndex = (scrollLineIndex + totalCount) * maxPerLine;
            SetChildCount(childCount + 1, index >= startIndex && index < endIndex);
        }

        /// <summary>
        /// ɾ���ӽڵ�
        /// </summary>
        /// <param name="index"></param>
        public void RemoveChild(int index)
        {
            if (index < 0 || index >= childCount) return;
            startIndex = scrollLineIndex * maxPerLine;
            endIndex = (scrollLineIndex + totalCount) * maxPerLine;
            SetChildCount(childCount - 1, index >= startIndex && index < endIndex);
        }


        /// <summary>
        /// ������ʾ���ڴ�С(����ò�ƿ��Է�����)
        /// </summary>
        /// <param name="port"></param>
        public void SetViewPort(Vector2 port)
        {
            if (port == viewPort) return;
            viewPort = port;
            ReBuild();
        }

        /// <summary>
        /// �����������
        /// </summary>
        /// <param name="max"></param>
        public void SetMaxPerLine(int max)
        {
            maxPerLine = max;
            ReBuild();
        }

        /// <summary>
        /// ���ض���
        /// </summary>
        public void BackTop()
        {
            content.localPosition = Vector3.zero;
            UpdateRectItem(0, true);
        }

        /// <summary>
        /// ���صײ�
        /// </summary>
        public void BackBottom()
        {
            if (arrangement == Arrangement.Vertical)
            {
                content.localPosition = new Vector3(0, -viewPort.y + content.sizeDelta.y, 0);
            }
            else
            {
                content.localPosition = new Vector3(viewPort.x - content.sizeDelta.x, 0);
            }
            UpdateRectItem(Mathf.CeilToInt((float)childCount / (float)maxPerLine) - totalCount + 1, true);
        }

        public void RefreshViewItem()
        {
            UpdateRectItem(scrollLineIndex, true);
        }


        public void SetArrangement(int arr)
        {
            arrangement = (Arrangement)arr;
        }

        public void SetHorizontal(int h)
        {
            horizontalAlign = (HorizontalAlign)h;
        }

        public void SetVerticle(int v)
        {
            verticalAlign = (VerticalAlign)v;
        }
    }
}