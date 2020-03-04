//------------------------------------------------------------------------------
/**
	\file	LoopScrollerView.cs

	Copyright (c) 2020, BoYue. All rights reserved.

	<PRE>

	模块名称�?文件所属的模块名称>
	文件名称：LoopScrollerView.cs
	�?   要：<描述该文件实现的主要功能>

	当前版本�?.0
	建立日期�?020/3/1
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
//	LoopScrollerView.cs
//------------------------------------------------------------------------------
//无限循环滚动ScrollerView
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//todo: index_ 实际的数据索引， item_ 是数据索引对应的Item
public delegate void UpdateHandle(int index_, GameObject item_);

public class LoopScrollerView : MonoBehaviour
{
    public enum Arrangement { Horizontal, Vertical, }
    public Arrangement _movement = Arrangement.Horizontal;
    //单行或单列的Item数量
    [Range(1, 20)]
    public int maxPerLine = 5;
    public int cellPadiding = 5;
    public int gridPadiding = 0;

    //Item的宽高
    public int cellWidth = 500;
    public int cellHeight = 100;

    public float cellWidthScale = 1.0f;
    public float cellHeightScale = 1.0f;

    //初始加载数，一般比实际行数多2行
    public int viewCount = 6;
    public GameObject itemPrefab;
    private RectTransform _content;
    private ScrollRect _scrollRect;

    //ScrollRect设置
    public ScrollRect.MovementType movementType = ScrollRect.MovementType.Clamped;

    private int _index = -1;
    private List<LoopScrollItem> _itemList = new List<LoopScrollItem>();
    private int _dataCount;

    private Queue<LoopScrollItem> _unUsedQueue = new Queue<LoopScrollItem>();

    private UpdateHandle _updateItem;
    private Action _valueChange;

    public RectTransform Content
    {
        get { return _content; }
    }

    void Awake()
    {
        cellWidth = (int)(cellWidth * cellWidthScale);
        cellHeight = (int)(cellHeight * cellHeightScale);

        GameObject obj = new GameObject("Content");

        _content = obj.AddComponent<RectTransform>();
        _content.anchorMin = new Vector2(0, 1);
        _content.anchorMax = new Vector2(0, 1);
        _content.pivot = new Vector2(0, 1);
        _content.offsetMin = new Vector2(0, -1);
        _content.offsetMax = new Vector2(1, 0);
        _content.localScale = Vector3.one;
        _content.SetParent(this.transform, false);

        //this.gameObject.AddComponent<Mask>().showMaskGraphic = false;
        var mask = this.gameObject.GetComponent<Mask>();
        if (null != mask)
        {
            GameObject.Destroy(mask);
        }
        var mask2D = this.gameObject.GetComponent<RectMask2D>();
        if (null == mask2D)
        {
            this.gameObject.AddComponent<RectMask2D>();
        }

        this.gameObject.AddComponent<Image>().color = new Color(0, 0, 0, 0);
        _scrollRect = this.gameObject.GetComponent<ScrollRect>();
        _scrollRect.content = _content;
        _scrollRect.movementType = this.movementType;
        _scrollRect.horizontal = false;
        _scrollRect.vertical = false;

        if (_movement == Arrangement.Horizontal)
            _scrollRect.horizontal = true;

        if (_movement == Arrangement.Vertical)
            _scrollRect.vertical = true;

        Util.SetActive(itemPrefab, false);
    }

    void Start()
    {

    }

    private void OnDestroy()
    {
        // 销毁组件
        Util.SafeDestroy(this.gameObject.GetComponent<RectMask2D>());
        Util.SafeDestroy(this.gameObject.GetComponent<ScrollRect>());
        Util.SafeDestroy(this.gameObject.GetComponent<Image>());

        // 销毁容器
        GameObject.Destroy(_content.gameObject);
    }

    public void Init(int itemCount_, UpdateHandle update_ = null, bool bResetPos = true)
    {
        _updateItem = update_;
        DataCount = itemCount_;
        if (bResetPos)
        {
            ClearData();
            OnValueChange(Vector2.zero);
        }
        else
        {
            for (int i = 0; i < _itemList.Count; ++i)
            {
                _unUsedQueue.Enqueue(_itemList[i]);
                _itemList[i].gameObject.SetActive(false);
            }
            _itemList.Clear();
            UpdateItem();
            _scrollRect.normalizedPosition = new Vector2(Mathf.Clamp01(_scrollRect.normalizedPosition.x), Mathf.Clamp01(_scrollRect.normalizedPosition.y));
        }
    }

    private void ClearData()
    {
        for (int i = 0; i < _itemList.Count; ++i)
        {
            GameObject.Destroy(_itemList[i].gameObject);
        }

        _itemList.Clear();

        while (_unUsedQueue.Count > 0)
        {
            GameObject.Destroy(_unUsedQueue.Dequeue().gameObject);
        }

        _index = -1;
        _scrollRect.StopMovement();
        _content.anchoredPosition = new Vector2(0.000f, 0.000f);
    }

    public void UpdateScrollerVeiw()
    {
        for (int i = 0; i < _itemList.Count; ++i)
        {
            UpdateItemContent(_itemList[i].Index);
        }
    }

    public void UpdateItemContent(int index_)
    {
        if (_updateItem != null)
        {
            LoopScrollItem item_ = _itemList.Find(e => e.name == "Item_" + index_.ToString("D3"));
            if (item_ == null)
            {
                Debug.LogError("scrollview error index_ = " + index_);
                return;
            }

            _updateItem(index_, item_.gameObject);
        }
    }

    //获取真实数据对应的Item
    public GameObject GetItem(int dataIndex_)
    {
        LoopScrollItem item_ = _itemList.Find(e => e.name == "Item_" + dataIndex_.ToString("D3"));
        if (item_ == null)
        {
            //Debug.LogError("scrollview error index_ = " + dataIndex_);
            return null;
        }

        return item_.gameObject;
    }

    private void UpdateItem()
    {
        int index = GetPosIndex();
        if (index <= -1)
        {
            return;
        }

        _index = index;
        for (int i = _itemList.Count; i > 0; i--)
        {
            LoopScrollItem item = _itemList[i - 1];
            if (item.Index < index * maxPerLine || (item.Index >= (index + viewCount) * maxPerLine))
            {
                _itemList.Remove(item);
                _unUsedQueue.Enqueue(item);
            }
        }

        for (int i = _index * maxPerLine; i < (_index + viewCount) * maxPerLine; i++)
        {
            if (i < 0 || i > _dataCount - 1)
                continue;

            bool isOk = false;
            for (int j = 0; j < _itemList.Count; ++j)
            {
                if (_itemList[j].Index == i)
                {
                    isOk = true;
                    break;
                }
            }

            if (isOk)
                continue;

            CreateItem(i);
        }
    }

    public void OnValueChange(Vector2 pos)
    {
        int index = GetPosIndex();
        if (_index != index && index > -1)
        {
            UpdateItem();
        }
    }

    public void TraverseAllItems(Action<LoopScrollItem> action)
    {
        foreach (LoopScrollItem item_ in _itemList)
        {
            action(item_);
        }
    }


    /// <summary>
    /// 提供给外部的方法，添加指定位置的Item
    /// </summary>
    public void AddItem(int index)
    {
        if (index > _dataCount)
        {
            Debug.LogError("添加错误:" + index);
            return;
        }
        AddItemIntoPanel(index);
        DataCount += 1;
    }

    /// <summary>
    /// 提供给外部的方法，删除指定位置的Item
    /// </summary>
    public void DelItem(int index)
    {
        if (index < 0 || index > _dataCount - 1)
        {
            Debug.LogError("删除错误:" + index);
            return;
        }
        DelItemFromPanel(index);
        DataCount -= 1;
    }

    private void AddItemIntoPanel(int index)
    {
        for (int i = 0; i < _itemList.Count; i++)
        {
            LoopScrollItem item = _itemList[i];
            if (item.Index >= index) item.Index += 1;
        }

        CreateItem(index);
    }

    private void DelItemFromPanel(int index)
    {
        int maxIndex = -1;
        int minIndex = int.MaxValue;
        for (int i = _itemList.Count; i > 0; i--)
        {
            LoopScrollItem item = _itemList[i - 1];
            if (item.Index == index)
            {
                GameObject.Destroy(item.gameObject);
                _itemList.Remove(item);
            }
            if (item.Index > maxIndex)
            {
                maxIndex = item.Index;
            }

            if (item.Index < minIndex)
            {
                minIndex = item.Index;
            }

            if (item.Index > index)
            {
                item.Index -= 1;
            }
        }

        if (maxIndex < DataCount - 1)
        {
            CreateItem(maxIndex);
        }
    }

    private void CreateItem(int index)
    {
        //Debug.Log("CreateItem index = " + index);
        LoopScrollItem itemBase;
        if (_unUsedQueue.Count > 0)
        {
            itemBase = _unUsedQueue.Dequeue();
        }
        else
        {
            //Debug.LogError("CreateItem index = " + index);
            itemBase = AddChild(_content, itemPrefab).GetComponent<LoopScrollItem>();
        }

        itemBase.gameObject.SetActive(true);
        if (!_itemList.Contains(itemBase))
        {
            _itemList.Add(itemBase);
        }
        itemBase.ScrollerView = this;
        itemBase.Index = index;

    }

    private GameObject AddChild(Transform parent, GameObject prefab)
    {
        GameObject go = GameObject.Instantiate(prefab) as GameObject;

        if (go != null && parent != null)
        {
            Transform t = go.transform;
            t.SetParent(parent, false);
            //go.layer = parent.gameObject.layer;
            go.SetActive(true);
        }
        return go;
    }

    private int GetPosIndex()
    {
        switch (_movement)
        {
            case Arrangement.Horizontal:
                {
                    if (Mathf.Abs(_content.anchoredPosition.x) < 0.01f)
                        return 0;

                    return Mathf.FloorToInt(_content.anchoredPosition.x / -(cellWidth + cellPadiding));
                }

            case Arrangement.Vertical:
                {
                    if (Mathf.Abs(_content.anchoredPosition.y) < 0.01f)
                        return 0;

                    return Mathf.FloorToInt(_content.anchoredPosition.y / (cellHeight + cellPadiding));
                }

        }
        return 0;
    }

    public Vector3 GetPosition(int i)
    {
        switch (_movement)
        {
            case Arrangement.Horizontal:
                return new Vector3(cellWidth * (i / maxPerLine), -(cellHeight + cellPadiding + gridPadiding) * (i % maxPerLine), 0f);
            case Arrangement.Vertical:
                return new Vector3((cellWidth + gridPadiding) * (i % maxPerLine), -(cellHeight + cellPadiding) * (i / maxPerLine), 0f);
        }
        return Vector3.zero;
    }

    public int DataCount
    {
        get { return _dataCount; }
        set
        {
            _dataCount = value;
            UpdateTotalWidth();
        }
    }

    private void UpdateTotalWidth()
    {
        int lineCount = Mathf.CeilToInt((float)_dataCount / maxPerLine);
        switch (_movement)
        {
            case Arrangement.Horizontal:
                _content.sizeDelta = new Vector2(cellWidth * lineCount + cellPadiding * (lineCount - 1), _content.sizeDelta.y);
                break;
            case Arrangement.Vertical:
                _content.sizeDelta = new Vector2(_content.sizeDelta.x, cellHeight * lineCount + cellPadiding * (lineCount - 1));
                break;
        }
    }

    public List<LoopScrollItem> GetItems()
    {
        return _itemList;
    }

    public bool IsEnable
    {
        get { return _scrollRect.enabled; }
        set { _scrollRect.enabled = value; }
    }

    private void UpdateContentPos(float xOffset, float yOffset)
    {
        _scrollRect.content.anchoredPosition = new Vector2(_scrollRect.content.anchoredPosition.x + xOffset, _scrollRect.content.anchoredPosition.y + yOffset);
        OnValueChange(Vector2.zero);
    }

    public void ItemToCenter(GameObject item)
    {
        RectTransform itemRect = item.transform as RectTransform;
        RectTransform thisRect = this.transform as RectTransform;
        float yOffset = _scrollRect.content.anchoredPosition.y - Mathf.Abs(itemRect.anchoredPosition.y);
        if (yOffset > 0f)
        {
            UpdateContentPos(0, -yOffset);
        }
        yOffset = (_scrollRect.content.anchoredPosition.y + thisRect.rect.size.y) - (Mathf.Abs(itemRect.anchoredPosition.y) + cellHeight);
        if (yOffset < 0f)
        {
            UpdateContentPos(0, -yOffset);
        }
    }
}
