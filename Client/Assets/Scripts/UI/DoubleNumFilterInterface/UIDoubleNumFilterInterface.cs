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
    class UIDoubleNumFilterInterface : UIBase
    {
        private List<SumTemp> m_SumTemp = new List<SumTemp>();
        private InputField m_inputField;
        private List<int> selectTypeList = new List<int>();
        private LoopScrollerView afterLoopScrollView;
        private Text m_totalAfter;
        private Toggle m_allToggle;
        private bool allTogIsOn = false;
        private Toggle m_clearToggle;
        private bool clearTogIsOn = false;
        private Toggle m_normalToggle;
        private DoubleNumberData doubleNumberData;
        public readonly int NormalCount = 3;
        private Tween m_moveTween;
        private List<List<byte>> initialFilterResults = new List<List<byte>>();
        private IOrderedEnumerable<KeyValuePair<int, int>> dictor;
        private bool bFirstShow = true;
        class SumTemp
        {
            //----------------------------------------------------------------------------
            UIDoubleNumFilterInterface m_parent;
            public GameObject m_ObjUI;
            Transform m_tran;
            public Toggle toggle;
            Text num;
            Text totalNum;
            public int type;
            RectTransform m_goRect;
            //----------------------------------------------------------------------------
            public SumTemp(GameObject go, UIDoubleNumFilterInterface _parent, int numType)
            {
                m_ObjUI = go;
                m_tran = m_ObjUI.transform;
                m_parent = _parent;
                type = numType;
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
            public void SetView(int count)
            {
                num.text = type.ToString();
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
                m_parent.SetInputFieldValue(isOn, type);
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
                m_inputField = GetUIComponent<InputField>(m_centerAnchorPath + "panel/inputField");
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
        public override void Show(object arg = null)
        {
            base.Show(arg);
            doubleNumberData = drawData.doubleNumberData;

            if (arg != null)
            {
                initialFilterResults = arg as List<List<byte>>;
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
        public void SetInputFieldValue(bool bState, int selectType)
        {
            if (bState)
            {
                if (!selectTypeList.Contains(selectType))
                {
                    selectTypeList.Add(selectType);
                }
            }
            else
            {
                if (selectTypeList.Contains(selectType))
                {
                    selectTypeList.Remove(selectType);
                }
            }

            string str = string.Empty;
            for (int i = 0; i < selectTypeList.Count; i++)
            {
                if (i == 0)
                {
                    str = selectTypeList[i].ToString();
                }
                else
                    str += "," + selectTypeList[i];
            }
            m_inputField.text = str;
        }
        //----------------------------------------------------------------------------
        public void SetSumPanelView()
        {
            GameObject temp = m_uiTrans.Find(m_centerAnchorPath + "sumPanel/temp").gameObject;
            Transform parent = m_uiTrans.Find(m_centerAnchorPath + "sumPanel");
            dictor = from objDic in doubleNumberData.doubleNumberDict orderby objDic.Key select objDic;
            int i = 0;
            foreach (var item in dictor)
            {
                GameObject obj = Object.Instantiate(temp, parent) as GameObject;
                obj.name = item.Key.ToString();
                obj.transform.localScale = Vector3.one;
                obj.transform.localRotation = Quaternion.identity;
                i++;
                obj.transform.localPosition = temp.transform.localPosition + new Vector3(i * 100, 0, 0);
                SumTemp sumTemp = new SumTemp(obj, this, item.Key);
                m_SumTemp.Add(sumTemp);
            }
            Object.DestroyImmediate(temp);

            foreach (var item in dictor)
            {
                for (int j = 0; j < m_SumTemp.Count; j++)
                {
                    if (m_SumTemp[j].type == item.Key)
                    {
                        m_SumTemp[j].SetView(item.Value);
                    }
                }
            }
        }
        //----------------------------------------------------------------------------
        protected override void UpdateLoopView(List<List<byte>> list, LoopScrollerView scrollView)
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
            Messenger.Broadcast<string, object>(DgMsgID.DgUI_ShowNewOneParam, "UISizeRatioNumFilterInterfaceCtrl", drawData.resultList);
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
            if (isOn)
            {
                int i = 0;
                dictor = from objDic in doubleNumberData.doubleNumberDict orderby objDic.Value descending select objDic;
                foreach (var item in dictor)
                {
                    i++;
                    for (int j = 0; j < m_SumTemp.Count; j++)
                    {
                        if (i <= NormalCount)
                        {
                            if (item.Key == m_SumTemp[j].type)
                            {
                                m_SumTemp[j].SetToggleOnOrNot(true);
                            }
                        }
                    }
                }
            }
            else
            {
                for (int j = 0; j < m_SumTemp.Count; j++)
                {
                    m_SumTemp[j].SetToggleOnOrNot(false);
                }
            }
        }
        //----------------------------------------------------------------------------
        private void OnBack()
        {
            drawData.resultList = initialFilterResults;      
            Messenger.Broadcast(DgMsgID.DgUI_HideNew, "UIDoubleNumFilterInterfaceCtrl");
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
                m_totalAfter.text = string.Format(PromptData.GetPrompt("afterFilterTotal"), drawData.resultList.Count);
                UpdateLoopView(drawData.resultList, afterLoopScrollView);
            }
            else
            {
                string curPeriod = "2020085";
                drawData.resultList = Util.GetDoubleFilterResult(initialFilterResults, selectTypeList, drawData.GetLastPeriodNumber(curPeriod));
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
    }
}
