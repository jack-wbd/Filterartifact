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
    class UIRedeemNumfilterInterface : UIBase
    {
        //----------------------------------------------------------------------------
        private List<ResultData> m_initialFilterResults = new List<ResultData>();
        private LoopScrollerView m_loopScrollView;
        private Text m_totalNum;
        private Text m_curPeriod;
        private GDropDown m_dropdown;
        private Text m_prizeNum;
        private static int ballMaxNumber = 33;
        private static int maxCount = 7;
        private static int maxBlueNum = 16;
        private List<Toggle> ballToggleList = new List<Toggle>();
        private GameObject temp;
        private Transform m_tempParent;
        private List<int> m_selectList = new List<int>();
        private List<Text> m_winningResult = new List<Text>();
        private static int maxRewardLevel = 6;
        private LoopScrollerView m_winResultScroll;
        public List<ResultData> redeemList = new List<ResultData>();
        //----------------------------------------------------------------------------
        protected override bool OnCreate()
        {
            bool bResult = GetUIObject();
            if (bResult)
            {
                BindEvent(m_centerAnchorPath + "back").AddListener(() => OnBack());
                BindEvent(m_centerAnchorPath + "seePanel/clear").AddListener(() => OnClear());
                BindEvent(m_centerAnchorPath + "savePanel/redeem").AddListener(() => OnBeginRedeem());
                BindEvent(m_centerAnchorPath + "backToFir").AddListener(() => OnBackToFir());

            }
            return true;
        }
        //----------------------------------------------------------------------------
        public bool GetUIObject()
        {
            if (m_objUI != null)
            {
                m_loopScrollView = GetUIComponent<LoopScrollerView>(m_centerAnchorPath + "filterResult/scrollView");
                m_winResultScroll = GetUIComponent<LoopScrollerView>(m_centerAnchorPath + "winningResult/scrollView");
                m_totalNum = GetUIComponent<Text>(m_centerAnchorPath + "filterResult/after");
                m_totalNum.text = string.Empty;
                m_curPeriod = GetUIComponent<Text>(m_centerAnchorPath + "curPeriod");
                m_curPeriod.text = string.Empty;
                m_dropdown = GetUIComponent<GDropDown>(m_centerAnchorPath + "seePanel/periodSelection");
                m_dropdown.OnValueChanged = OnDropValueChanged;
                if (drawData.tcbStatiDataList != null && drawData.tcbStatiDataList.Count > 0)
                {
                    List<string> str = new List<string>();
                    for (int i = 0; i < drawData.tcbStatiDataList.Count; i++)
                    {
                        str.Add(drawData.tcbStatiDataList[i].numperiods.ToString());
                    }
                    m_dropdown.AddOptions(str);
                }
                m_dropdown.RefreshShowValue();
                m_prizeNum = GetUIComponent<Text>(m_centerAnchorPath + "seePanel/number");
                m_prizeNum.text = string.Empty;
                m_tempParent = m_uiTrans.Find(m_centerAnchorPath + "seePanel/selectBall");
                temp = m_tempParent.Find("Btn").gameObject;
                for (int i = 0; i < ballMaxNumber; i++)
                {
                    var redToggle = Object.Instantiate(temp, m_tempParent) as GameObject;
                    redToggle.name = (i + 1).ToString();
                    redToggle.transform.localScale = Vector3.one;
                    redToggle.transform.localRotation = Quaternion.identity;
                    redToggle.transform.Find("Label").GetComponent<Text>().text = (i + 1).ToString();
                    var toggle = redToggle.GetComponent<Toggle>();
                    toggle.onValueChanged.AddListener((bool isOn) => OnBallToggleChange(toggle, isOn));
                    ballToggleList.Add(toggle);
                }
                Object.DestroyImmediate(temp);
                for (int i = 1; i <= maxRewardLevel; i++)
                {
                    var text = GetUIComponent<Text>(string.Format(m_centerAnchorPath + "seePanel/result/{0}", i));
                    text.text = string.Empty;
                    m_winningResult.Add(text);
                }
            }
            return true;
        }
        //----------------------------------------------------------------------------
        public override void Show(object arg = null)
        {
            base.Show(arg);

            if (arg != null)
            {
                m_initialFilterResults = arg as List<ResultData>;
            }
            SetSumPanelView();
        }
        //----------------------------------------------------------------------------
        public void SetSumPanelView()
        {
            UpdateLoopView(m_initialFilterResults, m_loopScrollView);
            m_totalNum.text = string.Format(PromptData.GetPrompt("afterFilterTotal"), m_initialFilterResults.Count);
            m_curPeriod.text = string.Format(PromptData.GetPrompt("numperiod"), drawData.curSelectPeriod);
            m_prizeNum.text = string.Format(PromptData.GetPrompt("prizeNum"), drawData.tcbStatiDataList[m_dropdown.Index].prizeNumber);
        }
        //----------------------------------------------------------------------------
        public override void Hide()
        {
            base.Hide();
        }
        //----------------------------------------------------------------------------
        private void OnDropValueChanged(int index)
        {
            drawData.curSelectPeriod = drawData.tcbStatiDataList[index].numperiods;
            m_prizeNum.text = string.Format(PromptData.GetPrompt("prizeNum"), drawData.tcbStatiDataList[index].prizeNumber);
        }
        //----------------------------------------------------------------------------
        protected override void UpdateLoopView(List<ResultData> list, LoopScrollerView scrollView)
        {
            base.UpdateLoopView(list, scrollView);
        }
        //----------------------------------------------------------------------------
        private void OnBallToggleChange(Toggle toggle, bool bstate)
        {

        }
        //----------------------------------------------------------------------------
        private void OnBack()
        {
            Messenger.Broadcast(DgMsgID.DgUI_HideNew, "UIRedeemNumfilterInterfaceCtrl");
        }
        //----------------------------------------------------------------------------
        private void OnBackToFir()
        {
            Messenger.Broadcast(DgMsgID.DgUI_ShowNew, "UIMainInterfaceCtrl");
        }
        //----------------------------------------------------------------------------
        private void OnClear()
        {
            m_prizeNum.text = string.Empty;
        }
        //----------------------------------------------------------------------------
        private void OnBeginRedeem()
        {
            if (redeemList.Count > 0)
            {
                redeemList.Clear();
            }
            var selRedData = drawData.redeemDict[drawData.curSelectPeriod];
            for (int i = 0; i < m_winningResult.Count; i++)
            {
                var curLevel = int.Parse(m_winningResult[i].gameObject.name);
                var result = Util.GetRedemptionDataList(curLevel, selRedData, m_initialFilterResults);
                switch (curLevel)
                {
                    case 1:
                        m_winningResult[curLevel - 1].text = string.Format(PromptData.GetPrompt("firstPrize"), result.Count);
                        break;
                    case 2:
                        m_winningResult[curLevel - 1].text = string.Format(PromptData.GetPrompt("secondPrize"), result.Count);
                        break;
                    case 3:
                        m_winningResult[curLevel - 1].text = string.Format(PromptData.GetPrompt("thirdPrize"), result.Count);
                        break;
                    case 4:
                        m_winningResult[curLevel - 1].text = string.Format(PromptData.GetPrompt("fourthPrize"), result.Count);
                        break;
                    case 5:
                        m_winningResult[curLevel - 1].text = string.Format(PromptData.GetPrompt("fifthPrize"), result.Count);
                        break;
                    case 6:
                        m_winningResult[curLevel - 1].text = string.Format(PromptData.GetPrompt("sixthPrize"), result.Count);
                        break;
                    default:
                        break;
                }

                if (result != null)
                {
                    foreach (var item in result)
                    {
                        redeemList.Add(item);
                    }
                }
            }

            UpdateLoopView(redeemList, m_winResultScroll);
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
