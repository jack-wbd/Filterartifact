using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Filterartifact
{
    class UIAdjacentNumFilterInterface : UIBase
    {
        private readonly int MaxCount = 6;
        private List<SumTemp> m_SumTemp = new List<SumTemp>();
        private InputField m_inputField;
        private List<int> selectNumList = new List<int>();
        private Text m_populatLab;
        private LoopScrollerView afterLoopScrollView;
        private Text m_totalAfter;
        private Toggle m_allToggle;
        private bool allTogIsOn = false;
        private Toggle m_clearToggle;
        private bool clearTogIsOn = false;
        private Toggle m_normalToggle;
        private AdjacentNumberData adjacentNumberData;
        public readonly int NormalCount = 2;
        private Tween m_moveTween;
        private List<ResultData> initialFilterResults = new List<ResultData>();
        class SumTemp
        {
            //----------------------------------------------------------------------------
            UIAdjacentNumFilterInterface m_parent;
            GameObject m_ObjUI;
            Transform m_tran;
            public Toggle toggle;
            Text num;
            Text totalNum;
            public int curNum;
            RectTransform m_goRect;
            //----------------------------------------------------------------------------
            public SumTemp(GameObject go, UIAdjacentNumFilterInterface _parent)
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
                m_goRect.sizeDelta += new Vector2(0, count / 10.0f);
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
                BindEvent(m_centerAnchorPath + "next").AddListener(() => OnNext());
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
                afterLoopScrollView = GetUIComponent<LoopScrollerView>(m_centerAnchorPath + "afterFiltering/scrollView");
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
            adjacentNumberData = drawData.adjacentNumData;
            if (arg != null)
            {
                initialFilterResults = arg as List<ResultData>;
            }
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
            foreach (var list in adjacentNumberData.dict)
            {
                for (int i = 0; i < m_SumTemp.Count; i++)
                {
                    if (list.Key == i)
                    {
                        m_SumTemp[i].SetView(list.Key, list.Value);
                    }

                    if (!adjacentNumberData.dict.ContainsKey(i))
                    {
                        m_SumTemp[i].ShowOrHide(false);
                    }
                }
            }
            string str = string.Empty;
            var numberList = adjacentNumberData.numberList[0];
            for (int i = 0; i < numberList.Count; i++)
            {
                if (i == 0)
                {
                    str = numberList[i].ToString();
                }
                else
                    str += "," + numberList[i];
            }

            m_populatLab.text = string.Format(PromptData.GetPrompt("neighborNumThis"), str);
        }
        //----------------------------------------------------------------------------
        protected override void UpdateLoopView(List<ResultData> list, LoopScrollerView scrollView)
        {
            base.UpdateLoopView(list, scrollView);
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
            Messenger.Broadcast<string, object>(DgMsgID.DgUI_ShowNewOneParam, "UIIntervalnumfilterinterfaceCtrl", drawData.resultList);
        }
        //----------------------------------------------------------------------------
        private void OnAllToggleChange(bool isOn)
        {
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
            var numberList = adjacentNumberData.adjacentNumberList;

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
        }
        //----------------------------------------------------------------------------
        private void OnBack()
        {
            drawData.resultList = initialFilterResults;
            Messenger.Broadcast(DgMsgID.DgUI_HideNew, "UIAdjacentNumFilterInterfaceCtrl");
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
                var numberList = adjacentNumberData.numberList[0];

                for (int i = 0; i < numberList.Count; i++)
                {
                    var curNum = numberList[i];
                    if (!drawData.redBallSelNumberList.Contains(curNum))
                    {
                        drawData.redBallSelNumberList.Add(curNum);
                    }
                }
                m_totalAfter.text = string.Format(PromptData.GetPrompt("afterFilterTotal"), drawData.resultList.Count * drawData.blueBallSelNumberList.Count);
                UpdateLoopView(drawData.resultList, afterLoopScrollView);
            }
            else
            {
                drawData.resultList = Util.GetResult(adjacentNumberData.numberList[0], initialFilterResults, selectNumList);
                m_totalAfter.text = string.Format(PromptData.GetPrompt("afterFilterTotal"), drawData.resultList.Count);
                UpdateLoopView(drawData.resultList, afterLoopScrollView);
            }
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
