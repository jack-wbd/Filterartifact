//------------------------------------------------------------------------------
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
using UnityEngine;

namespace Filterartifact
{
    [Serializable]
    public class TCBData
    {
        public string name;
        public string code;
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
        public string numperiods;
        public string prizeNumber;
        public List<string> popularNumber = new List<string>();
        public List<string> neutralNumber = new List<string>();
        public List<string> unpopularNumber = new List<string>();
        public string adjacentNumber;
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
        public List<int> redNumberdata = new List<int>();
        public int blueBallNum;
    }
    [Serializable]
    //----------------------------------------------------------------------------
    public class TCBNumberData
    {
        public int numberOne;
        public int numberTwo;
        public int numberThree;
        public int numberFour;
        public int numberFive;
        public int numberSix;
        public int numberBlue;
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

    //----------------------------------------------------------------------------
    public class DrawData : DataBase
    {
        public List<TCBStatisticsData> tcbStatiDataList = new List<TCBStatisticsData>();
        public List<TCBData> tcbdatalist = new List<TCBData>();
        public List<TCBNumberData> tcbNumberDataList = new List<TCBNumberData>();
        public AnalysisRedData m_analysisRedData = new AnalysisRedData();
        public AnalysisBlueData m_analysisBlueData = new AnalysisBlueData();
        public List<PrizeNumberData> prizeNumberDataList = new List<PrizeNumberData>();
        public ForecastDataHitRate forecastDataHitRate = new ForecastDataHitRate();
        public List<ForecastDataHitRate> forecastDataHitRateList = new List<ForecastDataHitRate>();
        public List<int> redBallSelNumberList = new List<int>();
        public List<int> blueBallSelNumberList = new List<int>();
        public PopularNumberData popularNumData = new PopularNumberData();
        public UnPopularNumberData unpopularNumData = new UnPopularNumberData();
        public List<List<byte>> redBallSelResult = new List<List<byte>>();
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
            return base.Initialize();
        }

        //----------------------------------------------------------------------------
        private void InitData()
        {
            var tcbstajsonpath = Application.persistentDataPath + "/tcbstatistics.json";
            if (File.Exists(tcbstajsonpath))
            {
                var streamReader = new StreamReader(tcbstajsonpath);
                var savedata = streamReader.ReadToEnd();
                var savejsondata = JsonReader.Deserialize<List<TCBStatisticsData>>(savedata);
                if (savejsondata != null)
                {
                    tcbStatiDataList = savejsondata;
                    tcbStatiDataList.Sort(SortTcbStatisticsDataList);
                    popularNumData = ParsePopularData(tcbStatiDataList);
                    unpopularNumData = ParseUnPopularData(tcbStatiDataList);
                }

            }
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
        //----------------------------------------------------------------------------
        private int SortTcbStatisticsDataList(TCBStatisticsData t1, TCBStatisticsData t2)//TCBStatisticsData
        {
            int date1 = int.Parse(t1.numperiods);
            int date2 = int.Parse(t2.numperiods);
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
        public PopularNumberData ParsePopularData(List<TCBStatisticsData> dataList)
        {
            if (dataList == null)
            {
                return null;
            }
            PopularNumberData data = new PopularNumberData();
            for (int i = 0; i < dataList.Count; i++)
            {
                if (!data.dict.ContainsKey(dataList[i].popularNumber.Count))
                {
                    data.dict.Add(dataList[i].popularNumber.Count, 1);
                }
                else
                {
                    data.dict[dataList[i].popularNumber.Count] += 1;
                }

                var list = dataList[i].popularNumber;
                for (int j = 0; j < list.Count; j++)
                {
                    var st = Convert.ToByte(list[j]);
                    if (!data.numberList.Contains(st))
                    {
                        data.numberList.Add(st);
                    }
                }
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
        public UnPopularNumberData ParseUnPopularData(List<TCBStatisticsData> dataList)
        {
            if (dataList == null)
            {
                return null;
            }
            UnPopularNumberData data = new UnPopularNumberData();
            for (int i = 0; i < dataList.Count; i++)
            {
                if (!data.dict.ContainsKey(dataList[i].unpopularNumber.Count))
                {
                    data.dict.Add(dataList[i].unpopularNumber.Count, 1);
                }
                else
                {
                    data.dict[dataList[i].unpopularNumber.Count] += 1;
                }

                var list = dataList[i].unpopularNumber;
                for (int j = 0; j < list.Count; j++)
                {
                    var st = Convert.ToByte(list[j]);
                    if (!data.numberList.Contains(st))
                    {
                        data.numberList.Add(st);
                    }
                }
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
    }

}
