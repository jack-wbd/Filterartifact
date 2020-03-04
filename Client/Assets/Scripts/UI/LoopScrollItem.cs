using UnityEngine;

public class LoopScrollItem : MonoBehaviour
{
    private LoopScrollerView _scrollerView;
    private int _index;
    //private int _originalIndex;

    void Start()
    {
        //_originalIndex = _index;
    }

    //private void OnButtonAddClick()
    //{
    //    //添加一个新的Item
    //    _scrollerView.AddItem(_index + 1);
    //}

    //private void OnButtonDelClick()
    //{
    //    //删除当前的Item
    //    _scrollerView.DelItem(_index);
    //}

    public int Index
    {
        get { return _index; }
        set
        {
            _index = value;
            transform.localPosition = _scrollerView.GetPosition(_index);
            gameObject.name = "Item_" + _index.ToString("D3");

            //Debug.LogError("older = " + _originalIndex + " ,  data index = " + _index);
            _scrollerView.UpdateItemContent(_index);
        }
    }

    public LoopScrollerView ScrollerView
    {
        set { _scrollerView = value; }
    }
}
