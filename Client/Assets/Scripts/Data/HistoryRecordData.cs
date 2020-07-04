using UnityEngine;
using UnityEditor;
using Filterartifact;
using System.Collections.Generic;
using System;
using System.Text;
using System.IO;
using Pathfinding.Serialization.JsonFx;

namespace Filterartifact
{
    public class HistoryTplData
    {
        public int m_nIssue;
        public string m_strDrawDate;
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
        private List<HistoryTplData> historyTplDataList;
        public override bool Initialize()
        {
            m_dictHistoryData = new Dictionary<int, HistoryTplData>();
            historyTplDataList = new List<HistoryTplData>();
            ParseHistoryRecordData();
            //SaveHistoryRecordData();
            return true;
        }
        //---------------------------------------------------------------------------
        public void SaveHistoryRecordData()
        {
            List<TCBData> tcbDataList = new List<TCBData>();
            for (int i = 0; i < historyTplDataList.Count; i++)
            {
                TCBData tcbData = new TCBData();
                tcbData.code = historyTplDataList[i].m_nIssue.ToString();
                tcbData.date = historyTplDataList[i].m_strDrawDate;
                StringBuilder builder = new StringBuilder(17);
                builder.Append(historyTplDataList[i].m_nRedOne);
                builder.Append(",");
                builder.Append(historyTplDataList[i].m_nRedTwo);
                builder.Append(",");
                builder.Append(historyTplDataList[i].m_nRedThree);
                builder.Append(",");
                builder.Append(historyTplDataList[i].m_nRedFour);
                builder.Append(",");
                builder.Append(historyTplDataList[i].m_nRedFive);
                builder.Append(",");
                builder.Append(historyTplDataList[i].m_nRedSix);
                tcbData.red = builder.ToString();
                tcbData.blue = historyTplDataList[i].m_nBlue.ToString();
                tcbData.sales = historyTplDataList[i].m_nSales.ToString();
                tcbData.poolmoney = historyTplDataList[i].m_nPrizePool.ToString();
                PrizeGrades prizeGradesOne = new PrizeGrades();
                prizeGradesOne.typenum = historyTplDataList[i].m_nFirstPrize.ToString();
                prizeGradesOne.typemoney = historyTplDataList[i].m_nFristPrizeBonus.ToString();
                tcbData.prizegrades.Add(prizeGradesOne);
                PrizeGrades prizeGradesTwo = new PrizeGrades();
                prizeGradesTwo.typenum = historyTplDataList[i].m_nSecondPrize.ToString();
                prizeGradesTwo.typemoney = historyTplDataList[i].m_nSecondPrizeBonus.ToString();
                tcbData.prizegrades.Add(prizeGradesTwo);
                tcbDataList.Add(tcbData);
            }
            SerializeAndSaveHistoricalData(tcbDataList);
        }
        //---------------------------------------------------------------------------
        private List<TCBData> ProcessDownLoadData()
        {
            var downLoadDataPath = Application.persistentDataPath + "/download.json";
            if (!File.Exists(@downLoadDataPath))
            {
                return null;
            }

            var streamReader = new StreamReader(downLoadDataPath);
            var savedata = streamReader.ReadToEnd();
            var savejsondata = JsonReader.Deserialize<DownLoadData>(savedata);
            savejsondata.result.Sort(drawData.SortTcbDataList);
            return savejsondata.result;

        }
        //---------------------------------------------------------------------------
        private void SerializeAndSaveHistoricalData(List<TCBData> dataList)
        {
            if (dataList == null || dataList.Count <= 0)
            {
                return;
            }
            var tcbstajsonpath = Application.persistentDataPath + "/tcbhistory.json";
            var downloadData = ProcessDownLoadData();
            var dataNewestCode = int.Parse(dataList[0].code);
            bool canWrite = false;
            for (int i = 0; i < downloadData.Count; i++)
            {
                var downloadCode = int.Parse(downloadData[i].code);
                if (downloadCode > dataNewestCode)
                {
                    dataList.Add(downloadData[i]);
                    canWrite = true;
                }
            }

            if (canWrite)
            {
                drawData.tcbdatalist = dataList;
                dataList.Sort(drawData.SortTcbDataList);
                drawData.tcbNumberDataList.Clear();
                drawData.tcbNumberDataList = drawData.ParseTCBNumberData(drawData.tcbdatalist);
                drawData.tcbStatiDataList.Clear();
                drawData.tcbStatiDataList = drawData.CalculateData(drawData.tcbdatalist);
                drawData.tcbStatiDataList.Sort(drawData.SortTcbStatisticsDataList);
                drawData.SerializeAndSaveStatiData(drawData.tcbStatiDataList);
            }

            if (!File.Exists(tcbstajsonpath) || canWrite)
            {
                var data = JsonWriter.Serialize(dataList);
                var streamWriter = new StreamWriter(tcbstajsonpath);
                Debug.Log("path: " + Application.persistentDataPath);
                streamWriter.Write(data);
                streamWriter.Close();
            }
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
                TplData.m_strDrawDate = data.m_strDrawDate;
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
                historyTplDataList.Add(TplData);
                m_dictHistoryData.Add(ID.Key, TplData);
            }
        }
        //---------------------------------------------------------------------------
        DrawData drawData
        {
            get { return WorldManager.Instance().GetDataCollection<DrawData>(); }
        }
        //---------------------------------------------------------------------------
        //---------------------------------------------------------------------------
    }
}