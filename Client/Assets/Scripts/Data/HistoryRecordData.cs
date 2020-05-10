using UnityEngine;
using UnityEditor;
using Filterartifact;
using System.Collections.Generic;
using System;

namespace Filterartifact
{
    public class HistoryTplData
    {
        public int m_nIssue;
        public string m_strDrawData;
        public int m_nRedOne;
        public int m_nRedTwo;
        public int m_nRedThree;
        public int m_nRedFour;
        public int m_nRedFive;
        public int m_nRedSix;
        public int m_nBlue;
        public int m_nPrizePool;
        public int m_nFirstPrize;
        public int m_nFristPrizeBonus;
        public int m_nSecondPrize;
        public int m_nSecondPrizeBonus;
        public int m_nSales;
    }
    public class HistoryRecordData : DataBase
    {
        private Dictionary<int, HistoryTplData> m_dictHistoryData;
        public override bool Initialize()
        {
            m_dictHistoryData = new Dictionary<int, HistoryTplData>();
            ParseHistoryRecordData();
            return true;
        }
        //---------------------------------------------------------------------------
        public void ParseHistoryRecordData()
        {
            Dictionary<int, StrucCTblHistoryRecord> history = tablemgr.Instance.g_oTblHistoryRecord.m_hTblHistoryRecord;
            foreach (var ID in history)
            {
                if (m_dictHistoryData.ContainsKey(ID.Key))
                {
                    continue;
                }
                StrucCTblHistoryRecord data = ID.Value;
                HistoryTplData TplData = new HistoryTplData();
                TplData.m_nIssue = data.m_nIssue;
                TplData.m_strDrawData = data.m_strDrawDate;
                TplData.m_nRedOne = data.m_nRedOne;
                TplData.m_nRedTwo = data.m_nRedTwo;
                TplData.m_nRedThree = data.m_nRedThree;
                TplData.m_nRedFour = data.m_nRedFour;
                TplData.m_nRedFive = data.m_nFirstPrize;
                TplData.m_nRedSix = data.m_nRedSix;
                TplData.m_nBlue = data.m_nBlue;
                TplData.m_nPrizePool = data.m_nPrizePool;
                TplData.m_nFirstPrize = data.m_nFirstPrize;
                TplData.m_nFristPrizeBonus = data.m_nFristPrizeBonus;
                TplData.m_nSecondPrize = data.m_nSecondPrize;
                TplData.m_nSecondPrizeBonus = data.m_nSecondPrizeBonus;
                TplData.m_nSales = data.m_nSales;
                m_dictHistoryData.Add(ID.Key, TplData);
            }
        }
        //---------------------------------------------------------------------------
        //---------------------------------------------------------------------------
        //---------------------------------------------------------------------------
    }
}