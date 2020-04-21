using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Filterartifact
{
        //----------------------------------------------------------------------------
    class UIUnPopularNumFilterInterface : UIBase
    {
        private readonly int MaxCount = 6;
        private List<SumTemp> m_SumTemp = new List<SumTemp>();
        private InputField m_inputField;
        private List<int> selectNumList = new List<int>();
        private Text m_populatLab;
        private LoopScrollerView beforeLoopScrollView;
        private LoopScrollerView afterLoopScrollView;
        private Text m_totalBefore;
        private Text m_totalAfter;
        private Toggle m_allToggle;
        private bool allTogIsOn = false;
        private Toggle m_clearToggle;
        private bool clearTogIsOn = false;
        private Toggle m_normalToggle;
        private bool norTogIsOn = false;
        private UnPopularNumberData unPopularNumberData;
        private List<byte> curStr = new List<byte>();
        private List<byte> str = new List<byte>();
        private List<List<byte>> resultList = new List<List<byte>>();
        public readonly int NormalCount = 2;
        //----------------------------------------------------------------------------
        class SumTemp
        {
            //----------------------------------------------------------------------------
            UIUnPopularNumFilterInterface m_parent;
            GameObject m_ObjUI;
            Transform m_tran;
            public Toggle toggle;
            Text num;
            Text totalNum;
            public int curNum;
            RectTransform m_goRect;
            //----------------------------------------------------------------------------
            public SumTemp(GameObject go, UIUnPopularNumFilterInterface _parent)
            {
                m_ObjUI = go;
                m_tran = m_ObjUI.transform;
                m_parent = _parent;
                toggle = m_tran.GetUnityComponent<Toggle>("toggle");
                toggle.isOn = false;
                toggle.onValueChanged.AddListener((bool isOn) => OnToggleValueChanged(isOn));
                num = m_tran.GetUnityComponent<Text>("num");
                num.text = string.Empty;
                totalNum = m_tran.GetUnityComponent<Text>("go/lab");
                totalNum.text = string.Empty;
                m_goRect = m_tran.GetUnityComponent<RectTransform>("go");
            }
            //----------------------------------------------------------------------------
            public void SetView(int number, int count)
            {
                curNum = number;
                num.text = string.Format(PromptData.GetPrompt("piece"), number);
                totalNum.text = count.ToString();
                m_goRect.anchoredPosition3D = new Vector3(0, 100, 0);
                m_goRect.pivot = new Vector2(0.5f, 0);
                m_goRect.sizeDelta += new Vector2(0, count);
            }
            //----------------------------------------------------------------------------
            public void SetToggleOnOrNot(bool state)
            {
                if (toggle.isOn != state)
                {
                    toggle.isOn = state;
                }
            }
            //----------------------------------------------------------------------------
            public void ShowOrHide(bool bState)
            {
                m_tran.Visible(bState);
            }
            //----------------------------------------------------------------------------
            private void OnToggleValueChanged(bool isOn)
            {
                m_parent.SetInputFieldValue(isOn, curNum);
            }
        }
        //----------------------------------------------------------------------------
        protected override bool OnCreate()
        {
            bool bResult = GetUIObject();
            if (bResult)
            {
                BindEvent(m_centerAnchorPath + "back").AddListener(() => OnBack());
                BindEvent(m_centerAnchorPath + "panel/begin").AddListener(() => OnBegin());
            }
            return true;
        }
        //----------------------------------------------------------------------------
        public bool GetUIObject()
        {
            if (m_objUI != null)
            {

                GameObject temp = m_uiTrans.Find(m_centerAnchorPath + "sumPanel/temp").gameObject;
                Transform parent = m_uiTrans.Find(m_centerAnchorPath + "sumPanel");
                for (int i = 0; i <= MaxCount; i++)
                {
                    GameObject obj = UnityEngine.Object.Instantiate(temp, parent) as GameObject;
                    obj.name = i.ToString();
                    obj.transform.localScale = Vector3.one;
                    obj.transform.localRotation = Quaternion.identity;
                    obj.transform.localPosition = temp.transform.localPosition + new Vector3(i * 100, 0, 0);
                    SumTemp sumTemp = new SumTemp(obj, this);
                    m_SumTemp.Add(sumTemp);
                }

                UnityEngine.Object.DestroyImmediate(temp);
                m_inputField = GetUIComponent<InputField>(m_centerAnchorPath + "panel/inputField");
                m_populatLab = GetUIComponent<Text>(m_centerAnchorPath + "panel/title2");
                m_populatLab.text = string.Empty;
                beforeLoopScrollView = GetUIComponent<LoopScrollerView>(m_centerAnchorPath + "beforeFiltering/scrollView");
                afterLoopScrollView = GetUIComponent<LoopScrollerView>(m_centerAnchorPath + "afterFiltering/scrollView");
                m_totalBefore = GetUIComponent<Text>(m_centerAnchorPath + "beforeFiltering/before");
                m_totalBefore.text = string.Empty;
                m_totalAfter = GetUIComponent<Text>(m_centerAnchorPath + "afterFiltering/after");
                m_totalAfter.text = string.Empty;
                m_allToggle = GetUIComponent<Toggle>(m_centerAnchorPath + "selectGroup/all");
                m_allToggle.onValueChanged.AddListener((bool isOn) => OnAllToggleChange(isOn));
                m_clearToggle = GetUIComponent<Toggle>(m_centerAnchorPath + "selectGroup/clear");
                m_clearToggle.onValueChanged.AddListener((bool isOn) => OnClearToggleChange(isOn));
                m_normalToggle = GetUIComponent<Toggle>(m_centerAnchorPath + "selectGroup/normal");
                m_normalToggle.onValueChanged.AddListener((bool isOn) => OnNormalToggleChange(isOn));
            }
            return true;
        }
        //----------------------------------------------------------------------------
        private bool bFirstShow = true;
        //----------------------------------------------------------------------------
        public override void Show(object arg = null)
        {
            base.Show(arg);
            unPopularNumberData = drawData.unpopularNumData;
            if (bFirstShow)
            {
                bFirstShow = false;
                SetSumPanelView();
            }
           
        }
        //----------------------------------------------------------------------------
        public override void Hide()
        {
            base.Hide();
        }
        //----------------------------------------------------------------------------
        public void SetInputFieldValue(bool bState, int number)
        {
            if (bState)
            {
                if (!selectNumList.Contains(number))
                {
                    selectNumList.Add(number);
                }
            }
            else
            {
                if (selectNumList.Contains(number))
                {
                    selectNumList.Remove(number);
                }
            }

            string str = string.Empty;
            for (int i = 0; i < selectNumList.Count; i++)
            {
                if (i == 0)
                {
                    str = selectNumList[i].ToString();
                }
                else
                    str += "," + selectNumList[i];
            }
            m_inputField.text = str;
        }
        //----------------------------------------------------------------------------
        public void SetSumPanelView()
        {
            foreach (var list in unPopularNumberData.dict)
            {
                for (int i = 0; i < m_SumTemp.Count; i++)
                {
                    if (list.Key == i)
                    {
                        m_SumTemp[i].SetView(list.Key, list.Value);
                    }

                    if (!unPopularNumberData.dict.ContainsKey(i))
                    {
                        m_SumTemp[i].ShowOrHide(false);
                    }
                }
            }
            string str = string.Empty;
            var numberList = unPopularNumberData.numberList;
            for (int i = 0; i < numberList.Count; i++)
            {
                if (i == 0)
                {
                    str = numberList[i].ToString();
                }
                else
                    str += "," + numberList[i];
            }

            m_populatLab.text = string.Format(PromptData.GetPrompt("popularNumThis"), str);

            m_totalBefore.text = string.Format(PromptData.GetPrompt("beforeFilterTotal"), Util.C(drawData.redBallSelNumberList.Count, 6) * drawData.blueBallSelNumberList.Count);

            var curList = drawData.redBallSelResult;
            UpdateLoopView(curList, beforeLoopScrollView);
        }
        //----------------------------------------------------------------------------
        protected override void UpdateLoopView(List<List<byte>> list, LoopScrollerView scrollView)
        {
            base.UpdateLoopView(list, scrollView);
        }
        //----------------------------------------------------------------------------
        private void OnAllToggleChange(bool isOn)
        {
            var numberList = unPopularNumberData.numberList;
            if (isOn)
            {

                for (int i = 0; i < m_SumTemp.Count; i++)
                {
                    m_SumTemp[i].SetToggleOnOrNot(true);
                }

            }
            else
            {
                for (int i = 0; i < m_SumTemp.Count; i++)
                {
                    m_SumTemp[i].SetToggleOnOrNot(false);
                }
            }
            allTogIsOn = isOn;
        }
        //----------------------------------------------------------------------------
        private void OnClearToggleChange(bool isOn)
        {
            if (isOn)
            {
                for (int i = 0; i < m_SumTemp.Count; i++)
                {
                    m_SumTemp[i].SetToggleOnOrNot(false);
                }
            }
            clearTogIsOn = isOn;
        }
        //----------------------------------------------------------------------------
        private void OnNormalToggleChange(bool isOn)
        {
            var numberList = unPopularNumberData.unpopularNumberList;

            if (isOn)
            {
                for (int j = 0; j < m_SumTemp.Count; j++)
                {
                    for (int i = 0; i < NormalCount; i++)
                    {
                        if (m_SumTemp[j].curNum == numberList[i].number)
                        {
                            m_SumTemp[j].toggle.isOn = true;
                            break;
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < m_SumTemp.Count; i++)
                {
                    m_SumTemp[i].SetToggleOnOrNot(false);
                }
            }
            norTogIsOn = isOn;
        }
        //----------------------------------------------------------------------------
        private void OnBack()
        {
            Messenger.Broadcast(DgMsgID.DgUI_HideNew, "UIUnPopularNumFilterInterfaceCtrl");
        }
        //----------------------------------------------------------------------------
        private void OnBegin()
        {
            if (clearTogIsOn || !IsAnyToggleIsOn())
            {
                Messenger.Broadcast<string, object>(DgMsgID.DgMsg_ShowUIOneParam, "UIErrorCtrl", PromptData.GetPrompt("setFilters"));
                return;
            }
            else if (allTogIsOn)
            {
                var numberList = unPopularNumberData.numberList;

                for (int i = 0; i < numberList.Count; i++)
                {
                    var curNum = numberList[i];
                    if (!drawData.redBallSelNumberList.Contains(curNum))
                    {
                        drawData.redBallSelNumberList.Add(curNum);
                    }
                }
                m_totalAfter.text = string.Format(PromptData.GetPrompt("afterFilterTotal"), Util.C(drawData.redBallSelNumberList.Count, 6));
                resultList = Util.GetCombination(GetSelRedBallNumberList(drawData.redBallSelNumberList), 6);
                UpdateLoopView(resultList, afterLoopScrollView);
            }
            else
            {
                if (!ContainsPopularNumber())
                {
                    return;
                }

                resultList = GetResult();
                m_totalAfter.text = string.Format(PromptData.GetPrompt("afterFilterTotal"), resultList.Count);
                UpdateLoopView(resultList, afterLoopScrollView);
            }
        }
        //----------------------------------------------------------------------------
        private List<byte> GetSelRedBallNumberList(List<int> dataList)
        {
            str.Clear();
            for (int i = 0; i < dataList.Count; i++)
            {
                var st = Convert.ToByte(dataList[i]);
                str.Add(st);
            }
            return str;
        }
        //----------------------------------------------------------------------------
        private List<byte> GetRemovePopularNumRedBallNumberList()
        {
            var list = unPopularNumberData.numberList;
            List<int> dataList = new List<int>();
            for (int i = 0; i < drawData.redBallSelNumberList.Count; i++)
            {
                dataList.Add(drawData.redBallSelNumberList[i]);
            }
            for (int i = 0; i < list.Count; i++)
            {
                if (dataList.Contains(list[i]))
                {
                    dataList.Remove(list[i]);
                }
            }

            return GetSelRedBallNumberList(dataList);
        }

        //----------------------------------------------------------------------------
        private List<List<byte>> GetResult()
        {
            var result = new List<List<byte>>();
            var curSelectNumList = new List<byte>();
            var numberDataList = unPopularNumberData.numberList;
            for (int i = 0; i < numberDataList.Count; i++)
            {
                if (drawData.redBallSelNumberList.Contains(numberDataList[i]))
                {
                    curSelectNumList.Add(Convert.ToByte(numberDataList[i]));
                }
            }

            for (int i = 0; i < selectNumList.Count; i++)
            {
                var selNum = selectNumList[i];
                var list = Util.GetCombination(curSelectNumList, selNum);
                var selectRedBallList = Util.GetCombination(GetRemovePopularNumRedBallNumberList(), 6 - selNum);
                for (int j = 0; j < list.Count; j++)
                {
                    for (int k = 0; k < selectRedBallList.Count; k++)
                    {
                        var res = list[j].Union(selectRedBallList[k]).ToList();
                        result.Add(res);
                    }
                }
            }
            return result;
        }
        //----------------------------------------------------------------------------
        private bool ContainsPopularNumber()
        {
            bool bstate = false;
            for (int i = 0; i < selectNumList.Count; i++)
            {
                if (drawData.redBallSelNumberList.Contains(selectNumList[i]))
                {
                    return true;
                }
            }
            return bstate;
        }
        //----------------------------------------------------------------------------
        private bool IsAnyToggleIsOn()
        {
            bool bState = false;
            for (int i = 0; i < m_SumTemp.Count; i++)
            {
                if (m_SumTemp[i].toggle.isOn)
                {
                    bState = true;
                    break;
                }
            }
            return bState;
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
