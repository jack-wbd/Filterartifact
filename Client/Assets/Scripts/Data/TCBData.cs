﻿//------------------------------------------------------------------------------
/**
	\file	TCBData.cs

	Copyright (c) 2020, BoYue. All rights reserved.

	<PRE>

	模块名称：<文件所属的模块名称>
	文件名称：TCBData.cs
	摘    要：<描述该文件实现的主要功能>

	当前版本：1.0
	建立日期：2020/3/25
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
//	TCBData.cs
//------------------------------------------------------------------------------
using Pathfinding.Serialization.JsonFx;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Filterartifact
{
    [Serializable]
    public class TCBData
    {
        public string name;
        public int code;
        public string detailsLink;
        public string videoLink;
        public string date;
        public string week;
        public string red;
        public string blue;
        public string blue2;
        public string sales;
        public string poolmoney;
        public string content;
        public string addmoney;
        public string addmoney2;
        public string msg;
        public string z2add;
        public string m2add;
        public List<PrizeGrades> prizegrades = new List<PrizeGrades>();
    }
    //----------------------------------------------------------------------------
    [Serializable]
    public class TCBStatisticsData
    {
        public string date;
        public int numperiods;
        public string prizeNumber;
        public List<string> popularNumber = new List<string>();
        public List<string> unpopularNumber = new List<string>();
        public List<string> adjacentNumber = new List<string>();
        public string intervalNumber;
        public string distanceNumber;
        public string acNumber;
        public string parityNumber;
        public string mantissaNumber;
        public string valuesNumber;
        public string serialNumber;
        public string heavyNumber;
        public string sizeratioNumber;
        public string salesNumber;
        public string firstprizeNumber;
    }
    //----------------------------------------------------------------------------
    [Serializable]
    public class AnalysisRedData
    {
        public string curforenumperiods;
        public List<CurrentRedBallData> curforeRedBallList = new List<CurrentRedBallData>();
        public string curforeHitRate;
        public string nextforenumperiods;
        public List<NextRedBallData> nextforeRedBallList = new List<NextRedBallData>();
        public void Clear()
        {
            curforenumperiods = string.Empty;
            curforeRedBallList.Clear();
            curforeHitRate = string.Empty;
            nextforenumperiods = string.Empty;
            nextforeRedBallList.Clear();

        }

    }
    //----------------------------------------------------------------------------
    [Serializable]
    public class NextRedBallData
    {
        public string redballNum;//红球号码
        public string consecutiveErrorsNum;//连错次数
        public string numberOfPairs;//连对次数
        public string totalCorrectNum;//总正确数
        public string accuracy;//准确率
        public string consecutiveErrorsMaxNum;//最大连错次数
        public string maximumNumOfPairs;//最大连对次数
    }
    //----------------------------------------------------------------------------
    [Serializable]
    public class CurrentRedBallData
    {
        public string redballNum;//红球号码
        public string isRightOrNot;//是否正确
    }
    //----------------------------------------------------------------------------
    [Serializable]
    public class AnalysisBlueData
    {

        public string curforenumperiods;
        public List<CurrentBlueBallData> curforeBlueBallDataList = new List<CurrentBlueBallData>();
        public string curforeHitRate;
        public string nextforenumperiods;
        public List<NextBlueBallData> nextforeBlueBallDataList = new List<NextBlueBallData>();

        public void Clear()
        {
            curforenumperiods = string.Empty;
            curforeBlueBallDataList.Clear();
            curforeHitRate = string.Empty;
            nextforenumperiods = string.Empty;
            nextforeBlueBallDataList.Clear();
        }
    }
    //----------------------------------------------------------------------------
    [Serializable]
    public class NextBlueBallData
    {
        public string blueballNum;//篮球号码
        public string consecutiveErrorsNum;//连错次数
        public string numberOfPairs;//连对次数
        public string totalCorrectNum;//总正确数
        public string accuracy;//准确率
        public string consecutiveErrorsMaxNum;//最大连错次数
        public string maximumNumOfPairs;//最大连对次数
    }
    //----------------------------------------------------------------------------
    [Serializable]
    public class CurrentBlueBallData
    {
        public string blueballNum;//蓝球号码
        public string isRightOrNot;//是否正确
    }
    //----------------------------------------------------------------------------
    [Serializable]
    public class ForecastDataHitRate
    {
        public int numperiods;//推荐期数
        public List<int> redballNumList = new List<int>();//红球号码
        public List<int> errorredballNumList = new List<int>();//选错红球号码
        public string redballHitRate = string.Empty;//智能推荐过滤后红球命中率
        public List<int> blueballNumList = new List<int>();//篮球号码
        public int errorblueballNum;//选错篮球号码
        public string blueballHitRate = string.Empty;//智能推荐过滤后篮球命中率

        public void Clear()
        {
            numperiods = 0;
            redballNumList.Clear();
            errorredballNumList.Clear();
            redballHitRate = string.Empty;
            blueballNumList.Clear();
            errorblueballNum = 0;
            blueballHitRate = string.Empty;
        }
    }
    //----------------------------------------------------------------------------
    [Serializable]
    public class PrizeNumberData
    {
        public int numperiods;
        public List<byte> redNumberdataList = new List<byte>();
        public int blueBallNum;
    }
    [Serializable]
    //----------------------------------------------------------------------------
    public class TCBNumberData
    {
        public List<byte> tcbNumberList = new List<byte>();
        public byte numberBlue;
    }
    //----------------------------------------------------------------------------
    [Serializable]
    public class PrizeGrades
    {
        public int type = 0;
        public string typenum = string.Empty;
        public string typemoney = string.Empty;
    }
    //----------------------------------------------------------------------------
    [Serializable]
    public class PopularNumberData
    {
        public List<PopularNumber> popularNumberList = new List<PopularNumber>();
        public Dictionary<int, int> dict = new Dictionary<int, int>();
        public List<byte> numberList = new List<byte>();
    }
    //----------------------------------------------------------------------------
    [Serializable]
    public class PopularNumber
    {
        public int number;
        public int count;
    }
    //----------------------------------------------------------------------------
    [Serializable]
    public class AdjacentNumberData
    {
        public List<AdjacentNumber> adjacentNumberList = new List<AdjacentNumber>();
        public Dictionary<int, int> dict = new Dictionary<int, int>();
        public List<List<byte>> numberList = new List<List<byte>>();
    }
    //----------------------------------------------------------------------------
    [Serializable]
    public class AdjacentNumber
    {
        public int number;
        public int count;
    }
    //----------------------------------------------------------------------------
    [Serializable]
    public class UnPopularNumberData
    {
        public List<UnPopularNumber> unpopularNumberList = new List<UnPopularNumber>();
        public Dictionary<int, int> dict = new Dictionary<int, int>();
        public List<byte> numberList = new List<byte>();
    }
    //----------------------------------------------------------------------------
    [Serializable]
    public class UnPopularNumber
    {
        public int number;
        public int count;
    }
    [Serializable]
    //----------------------------------------------------------------------------
    public class IntervalNumberData
    {
        public Dictionary<string, int> intervalNumberDict = new Dictionary<string, int>();
    }
    //----------------------------------------------------------------------------
    [Serializable]
    public class MaxIntervalNumberData
    {
        public Dictionary<int, int> maxIntervalNumberDict = new Dictionary<int, int>();
    }
    //----------------------------------------------------------------------------
    [Serializable]
    public class ACNumberData
    {
        public Dictionary<int, int> acNumberDict = new Dictionary<int, int>();
    }
    //----------------------------------------------------------------------------
    [Serializable]
    public class ParityNumberData
    {
        public Dictionary<string, int> parityNumberDict = new Dictionary<string, int>();
    }
    //----------------------------------------------------------------------------
    [Serializable]
    public class MantissaNumberData
    {
        public Dictionary<int, int> mantissaNumberDict = new Dictionary<int, int>();
    }
    //----------------------------------------------------------------------------
    [Serializable]
    public class SumvalueNumberData
    {
        public Dictionary<int, int> sumvalueNumberDict = new Dictionary<int, int>();
    }
    //----------------------------------------------------------------------------
    [Serializable]
    public class SerialNumberData
    {
        public Dictionary<int, int> serialNumberDict = new Dictionary<int, int>();
    }
    //----------------------------------------------------------------------------
    [Serializable]
    public class DoubleNumberData
    {
        public Dictionary<int, int> doubleNumberDict = new Dictionary<int, int>();
    }
    //----------------------------------------------------------------------------
    [Serializable]
    public class SizeRatioNumberData
    {
        public Dictionary<string, int> sizeRatioNumberDict = new Dictionary<string, int>();
    }
    //----------------------------------------------------------------------------
    public class ResultData
    {
        public List<byte> redBallList = new List<byte>();
        public byte blueBall;
        public int level = 0;
    }
    //----------------------------------------------------------------------------
    public enum NumberCount
    {
        One = 1,
        Two = 2,
        Three = 3,
        Four = 4,
        Five = 5,
        Six = 6,
    }
    //----------------------------------------------------------------------------
    public enum RewardLevel
    {
        First = 1,
        Second = 2,
        Third = 3,
        Fourth = 4,
        Fifty = 5,
        Sisty = 6,
    }
    //----------------------------------------------------------------------------
    public class DrawData : DataBase
    {
        public List<TCBStatisticsData> tcbStatiDataList = new List<TCBStatisticsData>();
        public List<TCBData> tcbHistorydatalist = new List<TCBData>();
        public List<TCBNumberData> tcbNumberDataList = new List<TCBNumberData>();
        public AnalysisRedData m_analysisRedData = new AnalysisRedData();
        public AnalysisBlueData m_analysisBlueData = new AnalysisBlueData();
        public List<PrizeNumberData> prizeNumberDataList = new List<PrizeNumberData>();
        public ForecastDataHitRate forecastDataHitRate = new ForecastDataHitRate();
        public List<ForecastDataHitRate> forecastDataHitRateList = new List<ForecastDataHitRate>();
        public List<byte> redBallSelNumberList = new List<byte>();
        public List<byte> blueBallSelNumberList = new List<byte>();
        public PopularNumberData popularNumData = new PopularNumberData();
        public UnPopularNumberData unpopularNumData = new UnPopularNumberData();
        public AdjacentNumberData adjacentNumData = new AdjacentNumberData();
        public List<ResultData> resultList = new List<ResultData>();//各过滤条件过滤后结果
        public IntervalNumberData intervalNumberData = new IntervalNumberData();
        public MaxIntervalNumberData maxIntervalNumberData = new MaxIntervalNumberData();
        public ACNumberData acNumberData = new ACNumberData();
        public ParityNumberData parityNumberData = new ParityNumberData();
        public MantissaNumberData mantissaNumberData = new MantissaNumberData();
        public SumvalueNumberData sumvalueNumberData = new SumvalueNumberData();
        public SerialNumberData serialNumberData = new SerialNumberData();
        public DoubleNumberData doubleNumberData = new DoubleNumberData();
        public SizeRatioNumberData sizeRatioNumberData = new SizeRatioNumberData();
        public bool canWrite = false;
        public int curSelectPeriod = 2003001;
        public Dictionary<int, ResultData> redeemDict = new Dictionary<int, ResultData>();
        //----------------------------------------------------------------------------
        public override void Deserialize()
        {
            base.Deserialize();
        }
        //----------------------------------------------------------------------------
        public override void Serialize()
        {
            base.Serialize();
        }
        //----------------------------------------------------------------------------
        public override bool Initialize()
        {
            InitData();
            return true;
        }
        //----------------------------------------------------------------------------
        private void Test()
        {
            tcbNumberDataList.Clear();
            tcbNumberDataList = ParseTCBNumberData();

            List<int> allTcbNum = new List<int>();
            for (int i = 0; i < tcbNumberDataList.Count; i++)
            {
                var list = tcbNumberDataList[i].tcbNumberList;
                for (int j = 0; j < list[j]; j++)
                {
                    allTcbNum.Add(list[j]);
                }
            }

            Dictionary<int, ItemInfo> dic = new Dictionary<int, ItemInfo>();
            foreach (var item in allTcbNum)
            {
                if (dic.ContainsKey(item))
                {
                    dic[item].RepeatNum += 1;
                }
                else
                {
                    dic.Add(item, new ItemInfo(item));
                }
            }
            var dicGetEnumertor = dic.GetEnumerator();
            List<ItemInfo> testList = new List<ItemInfo>();
            while (dicGetEnumertor.MoveNext())
            {
                testList.Add(dicGetEnumertor.Current.Value);
            }
            testList.Sort(SortByRepeatNum);

            for (int i = 0; i < 8; i++)
            {
                Debug.Log(testList[i].Value);
            }
        }
        //----------------------------------------------------------------------------
        int SortByRepeatNum(ItemInfo info1, ItemInfo info2)
        {
            if (info1.RepeatNum > info2.RepeatNum)
            {
                return -1;

            }
            else
                return 1;
        }
        //----------------------------------------------------------------------------
        private void InitData()
        {
            var historyDataPath = Application.persistentDataPath + "/tcbhistory.json";
            if (!File.Exists(historyDataPath))
            {
                var savedata = FileSystem.Instance().LoadGameData("tcbhistory.json");
                var savejsondata = JsonReader.Deserialize<List<TCBData>>(savedata);
                savejsondata.Sort(SortTcbDataList);
                tcbHistorydatalist.Clear();
                tcbHistorydatalist = savejsondata;
                var data = JsonWriter.Serialize(savejsondata);
                var streamWriter = new StreamWriter(historyDataPath);
                Debug.Log("path: " + Application.persistentDataPath);
                streamWriter.Write(data);
                streamWriter.Close();
            }
            else
            {
                var streamReader = new StreamReader(historyDataPath);
                var savedata = streamReader.ReadToEnd();
                var savejsondata = JsonReader.Deserialize<List<TCBData>>(savedata);
                if (savejsondata != null)
                {
                    tcbHistorydatalist.Clear();
                    tcbHistorydatalist = savejsondata;
                }
            }

            CalculateTcbStaticsDataAndSave();

            var analysisredjsonpath = Application.persistentDataPath + "/analysisreddata.json";
            if (File.Exists(analysisredjsonpath))
            {
                var streamReader = new StreamReader(analysisredjsonpath);
                var savedata = streamReader.ReadToEnd();
                var savejsondata = JsonReader.Deserialize<AnalysisRedData>(savedata);
                if (savejsondata != null)
                {
                    m_analysisRedData.Clear();
                    m_analysisRedData = savejsondata;
                }
            }

            var analysisbluejsonpath = Application.persistentDataPath + "/analysisbluedata.json";
            if (File.Exists(analysisbluejsonpath))
            {
                var streamReader = new StreamReader(analysisbluejsonpath);
                var savedata = streamReader.ReadToEnd();
                var savejsondata = JsonReader.Deserialize<AnalysisBlueData>(savedata);
                if (savejsondata != null)
                {
                    m_analysisBlueData.Clear();
                    m_analysisBlueData = savejsondata;
                }
            }

            var forecastjsonpath = Application.persistentDataPath + "/forecastdata.json";
            if (File.Exists(forecastjsonpath))
            {
                var streamReader = new StreamReader(forecastjsonpath);
                var savedata = streamReader.ReadToEnd();
                var savejsondata = JsonReader.Deserialize<List<ForecastDataHitRate>>(savedata);
                if (savejsondata != null)
                {
                    forecastDataHitRateList.Clear();
                    forecastDataHitRateList = savejsondata;
                }
            }

            var prizenumberjsonpath = Application.persistentDataPath + "/prizenumberdata.json";
            if (File.Exists(prizenumberjsonpath))
            {
                var streamReader = new StreamReader(prizenumberjsonpath);
                var savedata = streamReader.ReadToEnd();
                var savejsondata = JsonReader.Deserialize<List<PrizeNumberData>>(savedata);
                if (savejsondata != null)
                {
                    prizeNumberDataList.Clear();
                    prizeNumberDataList = savejsondata;
                }
            }
        }
        //---------------------------------------------------------------------------
        private void ParseHistoryTplData()
        {
            var dict = tablemgr.Instance.g_oTblHistoryRecord.m_hTblHistoryRecord.GetEnumerator();
            List<TCBData> tcbDataList = new List<TCBData>();
            while (dict.MoveNext())
            {
                var value = dict.Current.Value;
                TCBData tcbdata = new TCBData();
                StringBuilder builder = new StringBuilder();
                if (value.m_nRedOne < 10)
                {
                    builder.Append(0);
                    builder.Append(value.m_nRedOne);
                    builder.Append(",");
                }
                else
                {
                    builder.Append(value.m_nRedOne);
                    builder.Append(",");
                }
                if (value.m_nRedTwo < 10)
                {
                    builder.Append(0);
                    builder.Append(value.m_nRedTwo);
                    builder.Append(",");
                }
                else
                {
                    builder.Append(value.m_nRedTwo);
                    builder.Append(",");
                }

                if (value.m_nRedThree < 10)
                {
                    builder.Append(0);
                    builder.Append(value.m_nRedThree);
                    builder.Append(",");
                }
                else
                {
                    builder.Append(value.m_nRedThree);
                    builder.Append(",");
                }

                if (value.m_nRedFour < 10)
                {
                    builder.Append(0);
                    builder.Append(value.m_nRedFour);
                    builder.Append(",");
                }
                else
                {
                    builder.Append(value.m_nRedFour);
                    builder.Append(",");
                }

                if (value.m_nRedFive < 10)
                {
                    builder.Append(0);
                    builder.Append(value.m_nRedFive);
                    builder.Append(",");
                }
                else
                {
                    builder.Append(value.m_nRedFive);
                    builder.Append(",");
                }
                if (value.m_nRedSix < 10)
                {
                    builder.Append(0);
                    builder.Append(value.m_nRedSix);
                }
                else
                {
                    builder.Append(value.m_nRedSix);
                }
                tcbdata.red = builder.ToString();
                tcbdata.code = value.m_nIssue;
                tcbdata.date = value.m_strDrawDate;
                if (value.m_nBlue < 10)
                {
                    tcbdata.blue = "0" + value.m_nBlue;
                }
                else
                {
                    tcbdata.blue = value.m_nBlue.ToString();
                }
                tcbdata.sales = value.m_nSales.ToString();
                tcbdata.poolmoney = value.m_nPrizePool.ToString();

                tcbdata.prizegrades.Clear();
                PrizeGrades prizeOne = new PrizeGrades();
                prizeOne.typenum = value.m_nFirstPrize.ToString();
                prizeOne.typemoney = value.m_nFristPrizeBonus.ToString();
                tcbdata.prizegrades.Add(prizeOne);
                PrizeGrades prizeTwo = new PrizeGrades();
                prizeTwo.typenum = value.m_nSecondPrize.ToString();
                prizeTwo.typemoney = value.m_nSecondPrizeBonus.ToString();
                tcbdata.prizegrades.Add(prizeTwo);
                tcbDataList.Add(tcbdata);
            }
            tcbDataList.Sort(SortTcbDataList);
            var historyRedDataPath = Application.persistentDataPath + "/historyred.json";
            if (!File.Exists(historyRedDataPath))
            {
                var data = JsonWriter.Serialize(tcbDataList);
                var streamWriter = new StreamWriter(historyRedDataPath);
                Debug.Log("path: " + Application.persistentDataPath);
                streamWriter.Write(data);
                streamWriter.Close();
            }
        }
        //---------------------------------------------------------------------------
        private void CalculateTcbStaticsDataAndSave()
        {
            if (tcbStatiDataList.Count > 0 && tcbStatiDataList != null)
            {
                tcbStatiDataList.Clear();
            }
            tcbNumberDataList.Clear();
            tcbNumberDataList = ParseTCBNumberData();
            tcbStatiDataList = CalculateData();
            redeemDict = ParseRedeemDict();
            popularNumData = ParsePopularData();
            intervalNumberData = ParseIntervalNumberData();
            maxIntervalNumberData = ParseMaxIntervalNumberData();
            acNumberData = ParseAcNumberData();
            parityNumberData = ParseParityNumberData();
            mantissaNumberData = ParseMantissaNumberData();
            sumvalueNumberData = ParseSumvalueNumberData();
            serialNumberData = ParseSerialNumberData();
            doubleNumberData = ParseDoubleNumberData();
            sizeRatioNumberData = ParseSizeRatioNumberData();
            unpopularNumData = ParseUnPopularData();
            adjacentNumData = ParseAdjacentData();
            SerializeAndSaveStatiData();
        }
        //---------------------------------------------------------------------------
        private IntervalNumberData ParseIntervalNumberData()
        {
            IntervalNumberData data = new IntervalNumberData();
            for (int i = 0; i < tcbStatiDataList.Count; i++)
            {
                var key = tcbStatiDataList[i].intervalNumber;
                if (data.intervalNumberDict.ContainsKey(key))
                {
                    data.intervalNumberDict[key]++;
                }
                else
                    data.intervalNumberDict[key] = 1;
            }
            return data;
        }
        //---------------------------------------------------------------------------
        private MaxIntervalNumberData ParseMaxIntervalNumberData()
        {
            MaxIntervalNumberData data = new MaxIntervalNumberData();
            for (int i = 0; i < tcbStatiDataList.Count; i++)
            {
                var key = int.Parse(tcbStatiDataList[i].distanceNumber);
                if (data.maxIntervalNumberDict.ContainsKey(key))
                {
                    data.maxIntervalNumberDict[key]++;
                }
                else
                    data.maxIntervalNumberDict[key] = 1;
            }
            return data;
        }
        //---------------------------------------------------------------------------
        private ACNumberData ParseAcNumberData()
        {
            ACNumberData data = new ACNumberData();
            for (int i = 0; i < tcbStatiDataList.Count; i++)
            {
                var key = int.Parse(tcbStatiDataList[i].acNumber);
                if (data.acNumberDict.ContainsKey(key))
                {
                    data.acNumberDict[key]++;
                }
                else
                    data.acNumberDict[key] = 1;
            }
            return data;
        }
        //---------------------------------------------------------------------------
        private ParityNumberData ParseParityNumberData()
        {
            ParityNumberData data = new ParityNumberData();
            for (int i = 0; i < tcbStatiDataList.Count; i++)
            {
                var key = tcbStatiDataList[i].parityNumber;
                if (data.parityNumberDict.ContainsKey(key))
                {
                    data.parityNumberDict[key]++;
                }
                else
                    data.parityNumberDict[key] = 1;
            }
            return data;
        }
        //---------------------------------------------------------------------------
        private MantissaNumberData ParseMantissaNumberData()
        {
            MantissaNumberData data = new MantissaNumberData();
            for (int i = 0; i < tcbStatiDataList.Count; i++)
            {
                var key = int.Parse(tcbStatiDataList[i].mantissaNumber);
                if (data.mantissaNumberDict.ContainsKey(key))
                {
                    data.mantissaNumberDict[key]++;
                }
                else
                    data.mantissaNumberDict[key] = 1;
            }
            return data;
        }
        //---------------------------------------------------------------------------
        private SumvalueNumberData ParseSumvalueNumberData()
        {
            SumvalueNumberData data = new SumvalueNumberData();
            for (int i = 0; i < tcbStatiDataList.Count; i++)
            {
                var key = int.Parse(tcbStatiDataList[i].valuesNumber) / 10;
                if (data.sumvalueNumberDict.ContainsKey(key))
                {
                    data.sumvalueNumberDict[key]++;
                }
                else
                    data.sumvalueNumberDict[key] = 1;
            }
            return data;
        }
        //---------------------------------------------------------------------------
        private SerialNumberData ParseSerialNumberData()
        {
            SerialNumberData data = new SerialNumberData();
            for (int i = 0; i < tcbStatiDataList.Count; i++)
            {
                var key = int.Parse(tcbStatiDataList[i].serialNumber);
                if (data.serialNumberDict.ContainsKey(key))
                {
                    data.serialNumberDict[key]++;
                }
                else
                    data.serialNumberDict[key] = 1;
            }
            return data;
        }
        //---------------------------------------------------------------------------
        private DoubleNumberData ParseDoubleNumberData()
        {
            DoubleNumberData data = new DoubleNumberData();
            for (int i = 0; i < tcbStatiDataList.Count; i++)
            {
                var key = int.Parse(tcbStatiDataList[i].heavyNumber);
                if (data.doubleNumberDict.ContainsKey(key))
                {
                    data.doubleNumberDict[key]++;
                }
                else
                    data.doubleNumberDict[key] = 1;
            }
            return data;
        }
        //---------------------------------------------------------------------------
        private SizeRatioNumberData ParseSizeRatioNumberData()
        {
            SizeRatioNumberData data = new SizeRatioNumberData();
            for (int i = 0; i < tcbStatiDataList.Count; i++)
            {
                var key = tcbStatiDataList[i].sizeratioNumber;
                if (data.sizeRatioNumberDict.ContainsKey(key))
                {
                    data.sizeRatioNumberDict[key]++;
                }
                else
                    data.sizeRatioNumberDict[key] = 1;
            }
            return data;
        }
        //---------------------------------------------------------------------------
        public void SerializeAndSaveHistoricalData()
        {
            var tcbstajsonpath = Application.persistentDataPath + "/tcbhistory.json";
            var downloadData = ProcessDownLoadData();
            var historyData = ProcessHistoryData(tcbstajsonpath);
            var dataNewestCode = historyData[0].code;
            canWrite = false;
            for (int i = 0; i < downloadData.Count; i++)
            {
                var downloadCode = downloadData[i].code;
                if (downloadCode > dataNewestCode)
                {
                    historyData.Add(downloadData[i]);
                    canWrite = true;
                }
            }
            if (canWrite)
            {
                tcbHistorydatalist.Clear();
                tcbHistorydatalist = historyData;
                historyData.Sort(SortTcbDataList);
                CalculateTcbStaticsDataAndSave();
            }

            if (!File.Exists(tcbstajsonpath) || canWrite)
            {
                var data = JsonWriter.Serialize(historyData);
                var streamWriter = new StreamWriter(tcbstajsonpath);
                Debug.Log("path: " + Application.persistentDataPath);
                streamWriter.Write(data);
                streamWriter.Close();
            }
        }
        //---------------------------------------------------------------------------
        private List<TCBData> ProcessHistoryData(string path = null)
        {
            if (string.IsNullOrEmpty(path))
            {
                path = Application.persistentDataPath + "/tcbhistory.json";
            }
            if (!File.Exists(path))
            {
                return null;
            }
            var streamReader = new StreamReader(path);
            var savedata = streamReader.ReadToEnd();
            var savejsondata = JsonReader.Deserialize<List<TCBData>>(savedata);
            savejsondata.Sort(SortTcbDataList);
            return savejsondata;
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
            savejsondata.result.Sort(SortTcbDataList);
            return savejsondata.result;

        }

        //----------------------------------------------------------------------------
        public void SerializeAndSaveStatiData()
        {
            var tcbstajsonpath = Application.persistentDataPath + "/tcbstatistics.json";
            if (!File.Exists(tcbstajsonpath) || canWrite)
            {
                var data = JsonWriter.Serialize(tcbStatiDataList);
                var streamWriter = new StreamWriter(tcbstajsonpath);
                Debug.Log("path: " + Application.persistentDataPath);
                streamWriter.Write(data);
                streamWriter.Close();
                Debug.Log("已经写入最新分析数据");
            }
        }
        //----------------------------------------------------------------------------
        public void SerializeAndSaveDownData(DownLoadData downdata)
        {
            var jsonpath = Application.persistentDataPath + "/download.json";

            if (!File.Exists(@jsonpath))
            {
                var data = JsonWriter.Serialize(downdata);
                var streamWriter = new StreamWriter(jsonpath);
                Debug.Log("path: " + Application.persistentDataPath);
                streamWriter.Write(data);
                streamWriter.Close();
            }
            else
            {
                var streamReader = new StreamReader(jsonpath);
                var savedata = streamReader.ReadToEnd();
                var savejsondata = JsonReader.Deserialize<DownLoadData>(savedata);
                var index = savejsondata.result.Count - 1;
                if (downdata.result[0].code == savejsondata.result[0].code &&
                    !string.IsNullOrEmpty(savejsondata.result[index].prizegrades[0].typemoney))
                {
                    Debug.Log("数据已经是最新，不需要重新下载");
                }
                else
                {
                    streamReader.Close();
                    var data = JsonWriter.Serialize(downdata);
                    var streamWriter = new StreamWriter(jsonpath, false);
                    streamWriter.Write(data);
                    streamWriter.Close();
                    Debug.Log("已经下载最新数据");
                }
            }
        }
        //----------------------------------------------------------------------------
        public List<TCBNumberData> ParseTCBNumberData()
        {
            var TCBNumberDataList = new List<TCBNumberData>();
            for (int i = 0; i < tcbHistorydatalist.Count; i++)
            {
                var numberData = new TCBNumberData();
                string[] redArray = tcbHistorydatalist[i].red.Split(',');
                for (int j = 0; j < redArray.Length; j++)
                {
                    numberData.tcbNumberList.Add(byte.Parse(redArray[j]));
                }
                numberData.numberBlue = byte.Parse(tcbHistorydatalist[i].blue);
                TCBNumberDataList.Add(numberData);
            }
            return TCBNumberDataList;
        }
        //----------------------------------------------------------------------------
        public int SortTcbStatisticsDataList(TCBStatisticsData t1, TCBStatisticsData t2)//TCBStatisticsData
        {
            int date1 = t1.numperiods;
            int date2 = t2.numperiods;
            if (date1 < date2)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }
        //----------------------------------------------------------------------------
        public int SortTcbDataList(TCBData t1, TCBData t2)
        {
            int date1 = t1.code;
            int date2 = t2.code;
            if (date1 < date2)
            {
                return 1;
            }
            else
            {
                return -1;
            }

        }
        //----------------------------------------------------------------------------
        public PopularNumberData ParsePopularData()
        {
            if (tcbStatiDataList == null)
            {
                return null;
            }
            PopularNumberData data = new PopularNumberData();
            for (int i = 0; i < tcbStatiDataList.Count; i++)
            {
                if (!data.dict.ContainsKey(tcbStatiDataList[i].popularNumber.Count))
                {
                    data.dict.Add(tcbStatiDataList[i].popularNumber.Count, 1);
                }
                else
                {
                    data.dict[tcbStatiDataList[i].popularNumber.Count] += 1;
                }

                var list = tcbStatiDataList[i].popularNumber;
                for (int j = 0; j < list.Count; j++)
                {
                    var st = Convert.ToByte(list[j]);
                    if (!data.numberList.Contains(st))
                    {
                        data.numberList.Add(st);
                    }
                }
                data.numberList.Sort(OnSortBySize);
            }

            foreach (var item in data.dict)
            {
                PopularNumber popularNum = new PopularNumber();
                popularNum.number = item.Key;
                popularNum.count = item.Value;
                data.popularNumberList.Add(popularNum);
            }
            data.popularNumberList.Sort(OnSortPopularNumber);
            return data;
        }
        //----------------------------------------------------------------------------
        int OnSortBySize(byte byte1, byte byte2)
        {
            if (byte1 > byte2)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }
        //----------------------------------------------------------------------------
        public UnPopularNumberData ParseUnPopularData()
        {
            if (tcbStatiDataList == null)
            {
                return null;
            }
            UnPopularNumberData data = new UnPopularNumberData();
            for (int i = 0; i < tcbStatiDataList.Count; i++)
            {
                if (!data.dict.ContainsKey(tcbStatiDataList[i].unpopularNumber.Count))
                {
                    data.dict.Add(tcbStatiDataList[i].unpopularNumber.Count, 1);
                }
                else
                {
                    data.dict[tcbStatiDataList[i].unpopularNumber.Count] += 1;
                }

                var list = tcbStatiDataList[i].unpopularNumber;
                for (int j = 0; j < list.Count; j++)
                {
                    var st = Convert.ToByte(list[j]);
                    if (!data.numberList.Contains(st))
                    {
                        data.numberList.Add(st);
                    }
                }
                data.numberList.Sort(OnSortBySize);
            }
            foreach (var item in data.dict)
            {
                UnPopularNumber popularNum = new UnPopularNumber();
                popularNum.number = item.Key;
                popularNum.count = item.Value;
                data.unpopularNumberList.Add(popularNum);
            }
            data.unpopularNumberList.Sort(OnSortUnPopularNumber);
            return data;

        }
        //----------------------------------------------------------------------------
        public AdjacentNumberData ParseAdjacentData()
        {
            if (tcbStatiDataList == null)
            {
                return null;
            }
            AdjacentNumberData data = new AdjacentNumberData();
            for (int i = 0; i < tcbStatiDataList.Count; i++)
            {
                if (!data.dict.ContainsKey(tcbStatiDataList[i].adjacentNumber.Count))
                {
                    data.dict.Add(tcbStatiDataList[i].adjacentNumber.Count, 1);
                }
                else
                {
                    data.dict[tcbStatiDataList[i].adjacentNumber.Count] += 1;
                }

                var list = tcbStatiDataList[i].adjacentNumber;
                var strList = new List<byte>();
                for (int j = 0; j < list.Count; j++)
                {
                    var st = Convert.ToByte(list[j]);
                    strList.Add(st);
                }
                data.numberList.Add(strList);
            }
            foreach (var item in data.dict)
            {
                AdjacentNumber numData = new AdjacentNumber();
                numData.number = item.Key;
                numData.count = item.Value;
                data.adjacentNumberList.Add(numData);
            }
            data.adjacentNumberList.Sort(OnSortAdjacentNumber);
            return data;
        }
        //----------------------------------------------------------------------------
        int OnSortAdjacentNumber(AdjacentNumber data1, AdjacentNumber data2)
        {
            if (data1.count > data2.count)
            {
                return -1;
            }
            else
            {
                return 1;
            }
        }
        //----------------------------------------------------------------------------
        int OnSortUnPopularNumber(UnPopularNumber data1, UnPopularNumber data2)
        {
            if (data1.count > data2.count)
            {
                return -1;
            }
            else
            {
                return 1;
            }
        }
        //----------------------------------------------------------------------------
        int OnSortPopularNumber(PopularNumber data1, PopularNumber data2)
        {
            if (data1.count > data2.count)
            {
                return -1;
            }
            else
            {
                return 1;
            }
        }
        //----------------------------------------------------------------------------
        public override void Clear()
        {
            base.Clear();
        }
        //----------------------------------------------------------------------------
        public List<TCBStatisticsData> CalculateData()
        {
            var tcbstatisticsdatalist = new List<TCBStatisticsData>();
            for (int i = 0; i < tcbHistorydatalist.Count; i++)
            {
                var tcbstatisticsdata = new TCBStatisticsData();
                var task = tcbHistorydatalist[i];
                var tcbcurNumberData = tcbNumberDataList[i];
                var tcbbeforeNumberData = new TCBNumberData();
                if (i < tcbHistorydatalist.Count - 1)
                {
                    tcbbeforeNumberData = tcbNumberDataList[i + 1];
                }
                tcbstatisticsdata.date = task.date;
                tcbstatisticsdata.numperiods = task.code;
                tcbstatisticsdata.prizeNumber = task.red + " " + task.blue;
                tcbstatisticsdata.salesNumber = task.sales;
                tcbstatisticsdata.firstprizeNumber = task.prizegrades[0].typemoney;
                tcbstatisticsdata.valuesNumber = Values(tcbcurNumberData);
                tcbstatisticsdata.distanceNumber = MaxDistance(tcbcurNumberData);
                if (i < tcbHistorydatalist.Count - 1)
                {
                    tcbstatisticsdata.adjacentNumber = AdjacentNumber(tcbcurNumberData, tcbbeforeNumberData);
                }
                tcbstatisticsdata.popularNumber = PopularNum(tcbcurNumberData, DownType.POPULAR);
                tcbstatisticsdata.unpopularNumber = PopularNum(tcbcurNumberData, DownType.UNPOPULAR);
                tcbstatisticsdata.intervalNumber = IntervalType(tcbcurNumberData);
                tcbstatisticsdata.acNumber = ACNumber(tcbcurNumberData);
                tcbstatisticsdata.parityNumber = ParityNumber(tcbcurNumberData);
                tcbstatisticsdata.mantissaNumber = MantissaNum(tcbcurNumberData);
                tcbstatisticsdata.serialNumber = SerialNumber(tcbcurNumberData);
                tcbstatisticsdata.heavyNumber = HeavyNumber(tcbcurNumberData, tcbbeforeNumberData);
                tcbstatisticsdata.sizeratioNumber = SizeratioNumber(tcbcurNumberData);
                tcbstatisticsdatalist.Add(tcbstatisticsdata);
            }
            return tcbstatisticsdatalist;
        }
        public List<byte> GetLastPeriodNumber(string curPeroid)
        {
            var lastPrize = new List<byte>();
            for (int i = 0; i < tcbHistorydatalist.Count; i++)
            {
                if (tcbHistorydatalist[i].code.Equals(curPeroid))
                {
                    if (i < tcbHistorydatalist.Count - 1)
                    {
                        var lastPrizeStr = tcbHistorydatalist[i + 1].red.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < lastPrizeStr.Length; j++)
                        {
                            lastPrize.Add(byte.Parse(lastPrizeStr[j]));
                        }
                    }
                    break;
                }
            }
            return lastPrize;
        }
        //----------------------------------------------------------------------------
        private Dictionary<int, ResultData> ParseRedeemDict()
        {
            var dict = new Dictionary<int, ResultData>();
            for (int i = 0; i < tcbHistorydatalist.Count; i++)
            {
                var key = tcbHistorydatalist[i].code;
                var data = new ResultData();
                var redStr = tcbHistorydatalist[i].red.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                for (int j = 0; j < redStr.Length; j++)
                {
                    data.redBallList.Add(byte.Parse(redStr[j]));
                }
                data.blueBall = byte.Parse(tcbHistorydatalist[i].blue);
                dict[key] = data;
            }
            return dict;
        }
        //----------------------------------------------------------------------------
        private string Values(TCBNumberData numberdata)//和值
        {
            int values = 0;
            for (int i = 0; i < numberdata.tcbNumberList.Count; i++)
            {
                values += numberdata.tcbNumberList[i];
            }
            return values.ToString();
        }

        //----------------------------------------------------------------------------
        private string MaxDistance(TCBNumberData numberdata)//最大间隔
        {
            List<int> distanceList = new List<int>();
            var list = numberdata.tcbNumberList;
            for (int i = 0; i < list.Count; i++)
            {
                if (i < list.Count - 1)
                {
                    distanceList.Add(list[i + 1] - list[i]);
                }
            }
            //排序算法
            int a = 0;
            for (int i = 0; i < distanceList.Count - 1; i++)//升序
            {
                for (int j = 0; j < distanceList.Count - 1 - i; j++)
                {
                    if (distanceList[j] > distanceList[j + 1])
                    {
                        a = distanceList[j];
                        distanceList[j] = distanceList[j + 1];
                        distanceList[j + 1] = a;
                    }
                }
            }
            return distanceList[distanceList.Count - 1].ToString();
        }
        //----------------------------------------------------------------------------
        private List<string> AdjacentNumber(TCBNumberData curNumberdata, TCBNumberData beforeNumberdata)//相邻号码
        {
            var currentNumList = curNumberdata.tcbNumberList;
            var beforeNumList = beforeNumberdata.tcbNumberList;
            var list = new List<string>();
            for (int i = 0; i < beforeNumList.Count; i++)
            {
                for (int j = 0; j < currentNumList.Count; j++)
                {
                    if (Mathf.Abs(currentNumList[j] - beforeNumList[i]) == 1)
                    {
                        var str = beforeNumList[i].ToString();
                        if (!list.Contains(str))
                        {
                            list.Add(str);
                        }
                    }
                }
            }
            return list;
        }
        //----------------------------------------------------------------------------
        private List<string> PopularNum(TCBNumberData curentNumberdata, DownType type)
        {
            List<string> popularDataList = new List<string>();
            List<string> unPopularDataList = new List<string>();
            List<int> allTcbNum = new List<int>();
            for (int i = 0; i < tcbNumberDataList.Count; i++)
            {
                var list = tcbNumberDataList[i].tcbNumberList;
                for (int j = 0; j < list.Count; j++)
                {
                    allTcbNum.Add(list[j]);
                }
            }

            Dictionary<int, ItemInfo> dic = new Dictionary<int, ItemInfo>();
            foreach (var item in allTcbNum)
            {
                if (dic.ContainsKey(item))
                {
                    dic[item].RepeatNum += 1;
                }
                else
                {
                    dic.Add(item, new ItemInfo(item));
                }
            }

            var enumerator = dic.GetEnumerator();
            var itemList = new List<ItemInfo>();
            while (enumerator.MoveNext())
            {
                itemList.Add(enumerator.Current.Value);
            }
            itemList.Sort(SortByRepeatNum);
            var curData = curentNumberdata.tcbNumberList;
            switch (type)//前八个出现次数最多，后八个出现次数最少
            {
                case DownType.POPULAR:
                    {
                        for (int i = 0; i < 8; i++)
                        {
                            for (int j = 0; j < curData.Count; j++)
                            {
                                if (curData[j] == itemList[i].Value)
                                {
                                    popularDataList.Add(curData[j].ToString());
                                }
                            }
                        }
                        return popularDataList;
                    }
                case DownType.UNPOPULAR:
                    {
                        for (int i = 0; i < 8; i++)
                        {
                            for (int j = 0; j < curData.Count; j++)
                            {
                                if (curData[j] == itemList[itemList.Count - 1 - i].Value)
                                {
                                    unPopularDataList.Add(curData[j].ToString());
                                }
                            }
                        }
                        return unPopularDataList;
                    }

                default:
                    break;
            }
            return null;
        }

        //----------------------------------------------------------------------------
        private string IntervalType(TCBNumberData currentNumberData)//号码区间
        {
            var firstInterval = 0;
            var secondInterval = 0;
            var thirdInterval = 0;
            var curNumberdataList = currentNumberData.tcbNumberList;
            for (int i = 0; i < curNumberdataList.Count; i++)
            {
                if (curNumberdataList[i] <= 11)
                {
                    firstInterval += 1;
                }
                else if (curNumberdataList[i] > 11 && curNumberdataList[i] <= 22)
                {
                    secondInterval += 1;
                }
                else
                {
                    thirdInterval += 1;
                }
            }

            return firstInterval.ToString() + secondInterval.ToString() + thirdInterval.ToString();

        }
        //----------------------------------------------------------------------------
        public string ACNumber(TCBNumberData tcbNumberData) //AC值
        {

            var currentNum = tcbNumberData.tcbNumberList;

            List<int> Absovallist = new List<int>();

            for (int i = 0; i < currentNum.Count; i++)
            {
                if (i != 0)
                    Absovallist.Add(Mathf.Abs(currentNum[0] - currentNum[i]));
                if (i != 1)
                    Absovallist.Add(Mathf.Abs(currentNum[1] - currentNum[i]));
                if (i != 2)
                    Absovallist.Add(Mathf.Abs(currentNum[2] - currentNum[i]));
                if (i != 3)
                    Absovallist.Add(Mathf.Abs(currentNum[3] - currentNum[i]));
                if (i != 4)
                    Absovallist.Add(Mathf.Abs(currentNum[4] - currentNum[i]));
                if (i != 5)
                    Absovallist.Add(Mathf.Abs(currentNum[5] - currentNum[i]));
            }

            Absovallist = Absovallist.Distinct().ToList();//除去列表中重复元素Linq

            return (Absovallist.Count - 5).ToString();

        }
        //----------------------------------------------------------------------------
        private string ParityNumber(TCBNumberData currentData)//奇偶
        {

            var oldNum = 0;
            var even = 0;
            var currentNum = currentData.tcbNumberList;

            for (int i = 0; i < currentNum.Count; i++)
            {
                if (currentNum[i] % 2 == 0)
                {
                    oldNum += 1;
                }
                else
                {
                    even += 1;
                }
            }
            return even.ToString() + oldNum.ToString();
        }
        //----------------------------------------------------------------------------
        private string MantissaNum(TCBNumberData currentNumData)//尾数
        {
            var mantissaNumList = new List<int>();

            var currentNumList = currentNumData.tcbNumberList;

            for (int i = 0; i < currentNumList.Count; i++)
            {
                if (currentNumList[i] < 10)
                {
                    mantissaNumList.Add(currentNumList[i]);
                }
                else
                {
                    mantissaNumList.Add(currentNumList[i] % 10);
                }

            }


            var list = new List<int>();

            foreach (var s in mantissaNumList.GroupBy(c => c))
            {
                list.Add(s.Count());
            }

            int num = 0;
            bool hasValue = false;

            foreach (int x in list)
            {
                if (hasValue)
                {
                    if (x > num)
                        num = x;
                }
                else
                {
                    num = x;
                    hasValue = true;
                }
            }

            if (hasValue)
                return num.ToString();

            return "";


        }
        //----------------------------------------------------------------------------
        private string SerialNumber(TCBNumberData currenttNumberData)//连号
        {

            var list = currenttNumberData.tcbNumberList;

            int num = 0;

            for (int i = 0; i < list.Count; i++)
            {
                if (i < list.Count - 1)
                {
                    if (list[i + 1] - list[i] == 1)
                    {
                        num += 1;
                    }
                }
            }

            return num.ToString();

        }
        //----------------------------------------------------------------------------
        private string HeavyNumber(TCBNumberData currentNumberData, TCBNumberData curBeforeNumberData) //重号
        {
            var curNumList = currentNumberData.tcbNumberList;
            var befNumList = curBeforeNumberData.tcbNumberList;
            var num = 0;
            for (int i = 0; i < befNumList.Count; i++)
            {
                for (int j = 0; j < curNumList.Count; j++)
                {
                    if (befNumList[i] == curNumList[j])
                    {
                        num += 1;
                    }
                }
            }
            return num.ToString();
        }
        //----------------------------------------------------------------------------
        private string SizeratioNumber(TCBNumberData currentNumberData)//大小比
        {
            var list = currentNumberData.tcbNumberList;
            var bigNum = 0;
            var smallNum = 0;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] >= 17)
                {
                    bigNum += 1;
                }
                else
                {
                    smallNum += 1;
                }
            }

            return bigNum.ToString() + smallNum.ToString();

        }
        //--------------------------------------------------------------------------
    }

}
