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
using System;
using System.Collections.Generic;

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
        public string numperiods;
        public string prizeNumber;
        public string popularNumber;
        public string neutralNumber;
        public string unpopularNumber;
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


}
