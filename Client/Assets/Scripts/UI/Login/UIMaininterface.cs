//------------------------------------------------------------------------------
/**
	\file	UIMaininterface.cs

	Copyright (c) 2020, BoYue. All rights reserved.

	<PRE>

	模块名称：<文件所属的模块名称>
	文件名称：UIMaininterface.cs
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
//	UIMaininterface.cs
//------------------------------------------------------------------------------
using CsharpHttpHelper;
using CsharpHttpHelper.Enum;
using DG.Tweening;
using HtmlAgilityPack;
using Pathfinding.Serialization.JsonFx;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using UnityEngine;
using UnityEngine.UI;
namespace Filterartifact
{
    class UIMaininterface : UIBase
    {
        //----------------------------------------------------------------------------
        private HttpHelper http;
        private HttpItem item;
        private string html;
        private LoopScrollerView loopScrollView;
        private List<TCBData> tcbdatalist = new List<TCBData>();
        private List<TCBNumberData> tcbNumberDataList = new List<TCBNumberData>();
        private List<TCBStatisticsData> tcbStatiDataList = new List<TCBStatisticsData>();
        private AnalysisRedData m_analysisRedData = new AnalysisRedData();
        private AnalysisBlueData m_analysisBlueData = new AnalysisBlueData();
        private List<PrizeNumberData> prizeNumberDataList = new List<PrizeNumberData>();
        private Text m_redRecommendedData;
        private Text m_blueRecommendedData;
        private Text m_periodHite;
        private ForecastDataHitRate forecastDataHitRate = new ForecastDataHitRate();
        private List<ForecastDataHitRate> forecastDataHitRateList = new List<ForecastDataHitRate>();
        Tween m_moveTween;
        //----------------------------------------------------------------------------
        protected override bool OnCreate()
        {
            bool bResult = GetUIObject();
            if (bResult)
            {
                BindEvent(m_downAnchorPath + "onhdown").AddListener(() => OnOneHundredClick());
                BindEvent(m_downAnchorPath + "redsmarego").AddListener(() => OnRedBallKillClick());
                BindEvent(m_downAnchorPath + "bluesmarego").AddListener(() => OnBlueBallKillClick());
                BindEvent(m_downAnchorPath + "saverecommend").AddListener(() => SaveRecommendClick());
                BindEvent(m_downAnchorPath + "statistics").AddListener(() => OnStatisticsClick());
                BindEvent(m_downAnchorPath + "begin").AddListener(() => TweenMove());
            }
            base.OnCreate();
            return true;
        }
        //----------------------------------------------------------------------------
        public bool GetUIObject()
        {
            if (m_objUI != null)
            {
                m_redRecommendedData = m_uiTrans.Find(m_downAnchorPath + "redsmarego/title").GetComponent<Text>();
                m_redRecommendedData.text = string.Empty;
                m_blueRecommendedData = m_uiTrans.Find(m_downAnchorPath + "bluesmarego/title").GetComponent<Text>();
                m_blueRecommendedData.text = string.Empty;
                m_periodHite = m_uiTrans.Find(m_downAnchorPath + "hite").GetComponent<Text>();
                m_periodHite.text = string.Empty;
                loopScrollView = m_uiTrans.Find(m_centerAnchorPath + "pan_scroll").GetComponent<LoopScrollerView>();
                InitData();
            }
            return true;
        }
        //----------------------------------------------------------------------------
        public override void Show(object arg = null)
        {
            base.Show(arg);
            ShowView();
        }
        //----------------------------------------------------------------------------
        public void ShowView()
        {

        }
        //----------------------------------------------------------------------------
        private void TweenMove()
        {
            var moveSize = ScreenUnit.fWidth;
            m_moveTween = m_uiTrans.GetComponent<RectTransform>().DOLocalMove(new Vector2(-moveSize, 0), 0.1f);
        }
        //----------------------------------------------------------------------------
        public override void Hide()
        {
            base.Hide();
        }
        //----------------------------------------------------------------------------
        protected override void OnUpdate()
        {
            base.OnUpdate();
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
        private void OnOneHundredClick()
        {
            http = new HttpHelper();
            item = new HttpItem
            {
                URL = DownLoadManager.OneHundredDoAddress,              //URL     必需项  
                Method = DownLoadManager.MethodType,                    //URL     可选项 默认为Get  
                Timeout = DownLoadManager.TimeOunt,                     //连接超时时间     可选项默认为100000  
                ReadWriteTimeout = DownLoadManager.ReadWriteTimeout,    //写入Post数据超时时间     可选项默认为30000  
                IsToLower = DownLoadManager.IsToLower,                  //得到的HTML代码是否转成小写     可选项默认转小写  
                Cookie = DownLoadManager.Cookie,                        //字符串Cookie     可选项  
                UserAgent = DownLoadManager.UserAgent,                  //用户的浏览器类型，版本，操作系统     可选项有默认值  
                Accept = DownLoadManager.Accept,                        //可选项 有默认值  
                ContentType = DownLoadManager.ContentType,              //返回类型    可选项有默认值  
                Referer = DownLoadManager.SourceAddress,                //来源URL     可选项  
                Allowautoredirect = DownLoadManager.Allowautoredirect,  //是否根据３０１跳转     可选项  
                AutoRedirectCookie = DownLoadManager.AutoRedirectCookie,//是否自动处理Cookie     可选项  
                Postdata = DownLoadManager.Postdata,                    //Post数据     可选项GET时不需要写  
                ResultType = ResultType.String,                         //返回数据类型，是Byte还是String  
            };

            var httpResult = http.GetHtml(item);
            if (httpResult.StatusCode == HttpStatusCode.OK)
            {
                html = httpResult.Html;
                Debug.Log("html: " + html);
                DownLoadData downdata = Deserialize(html);
                tcbdatalist = downdata.result;
                tcbdatalist.Sort(SortTcbDataList);
                tcbNumberDataList = ParseTCBNumberData(tcbdatalist);
                SerializeAndSaveDownData(downdata);
                tcbStatiDataList = CalculateData(tcbdatalist);
                tcbStatiDataList.Sort(SortTcbStatisticsDataList);
                SerializeAndSaveStatiData(tcbStatiDataList);
            }
            else
            {
                Debug.Log("result.StatusCode: " + httpResult.StatusCode);
            }

            DownLoadRedBallKillData();
            DownLoadBuleBallKillData();
        }
        //----------------------------------------------------------------------------
        private void OnRedBallKillClick()
        {
            if (!string.IsNullOrEmpty(m_redRecommendedData.text))
            {
                return;
            }
            if (m_analysisRedData != null)
            {
                m_analysisRedData.nextforeRedBallList.Sort(SortRedRecommendList);
            }
            List<string> nlist = new List<string>();
            for (int i = 0; i < m_analysisRedData.nextforeRedBallList.Count; i++)
            {
                var str1 = m_analysisRedData.nextforeRedBallList[i].accuracy;
                var st2 = str1.Substring(0, str1.Length - 1);
                var accuracy = int.Parse(st2) / 100f;
                if (accuracy > 0.8f)
                {
                    nlist.Add(m_analysisRedData.nextforeRedBallList[i].redballNum);

                }
            }
            forecastDataHitRate.redballNumList.Clear();
            var disList = nlist.Distinct().ToList();
            for (int i = 0; i < disList.Count; i++)
            {
                forecastDataHitRate.redballNumList.Add(int.Parse(disList[i]));
            }

            for (int i = 0; i < disList.Count; i++)
            {
                m_redRecommendedData.text += disList[i] + " ";
            }

        }
        //----------------------------------------------------------------------------
        private void OnBlueBallKillClick()
        {
            if (!string.IsNullOrEmpty(m_blueRecommendedData.text))
            {
                return;
            }
            if (m_analysisBlueData != null)
            {
                m_analysisBlueData.nextforeBlueBallDataList.Sort(SortBlueRecommendList);
            }
            List<string> nlist = new List<string>();
            for (int i = 0; i < m_analysisBlueData.nextforeBlueBallDataList.Count; i++)
            {
                var str1 = m_analysisBlueData.nextforeBlueBallDataList[i].accuracy;
                var st2 = str1.Substring(0, str1.Length - 1);
                var accuracy = int.Parse(st2) / 100f;
                if (accuracy >= 0.9f)
                {
                    nlist.Add(m_analysisBlueData.nextforeBlueBallDataList[i].blueballNum);
                }
            }
            forecastDataHitRate.blueballNumList.Clear();
            var disList = nlist.Distinct().ToList();
            for (int i = 0; i < disList.Count; i++)
            {
                forecastDataHitRate.blueballNumList.Add(int.Parse(disList[i]));
            }
            for (int i = 0; i < disList.Count; i++)
            {
                m_blueRecommendedData.text += disList[i] + " ";
            }
        }
        //----------------------------------------------------------------------------
        private void SaveRecommendClick()
        {
            if (string.IsNullOrEmpty(m_redRecommendedData.text) || string.IsNullOrEmpty(m_blueRecommendedData.text))
            {
                Debug.Log("请先点击智能分析红球和篮球");
                return;
            }
            if (forecastDataHitRateList.Count != 0 && forecastDataHitRateList != null)
            {

                if (forecastDataHitRateList[0].numperiods != int.Parse(m_analysisRedData.nextforenumperiods))
                {
                    forecastDataHitRate.numperiods = int.Parse(m_analysisRedData.nextforenumperiods);
                    forecastDataHitRateList.Add(forecastDataHitRate);
                    forecastDataHitRateList.Sort(ForecastDataSort);//排序，期数由前向后
                }
            }
            else
            {
                forecastDataHitRate.numperiods = int.Parse(m_analysisRedData.nextforenumperiods);
                forecastDataHitRateList.Add(forecastDataHitRate);
                forecastDataHitRateList.Sort(ForecastDataSort);//排序，期数由前向后
            }
            CalculatePredictedHitRate();
            SaveForecastData();
        }
        //----------------------------------------------------------------------------
        private void OnStatisticsClick()
        {
            if (tcbStatiDataList == null || tcbStatiDataList.Count == 0)
            {
                Debug.Log("请先下载数据");
                return;
            }

            loopScrollView.Init(tcbStatiDataList.Count, (idx, item) =>
            {
                var tran = item.transform;
                var task = tcbStatiDataList[idx];

                tran.Find("periods").GetComponent<Text>().text = task.numperiods;

                tran.Find("number").GetComponent<Text>().text = task.prizeNumber;

                tran.Find("sales").GetComponent<Text>().text = task.salesNumber;

                tran.Find("firstprize").GetComponent<Text>().text = task.firstprizeNumber;

                tran.Find("values").GetComponent<Text>().text = task.valuesNumber;//和值

                tran.Find("distance").GetComponent<Text>().text = task.distanceNumber;//最大间隔


                tran.Find("adjacent").GetComponent<Text>().text = task.adjacentNumber;//相邻号码

                tran.Find("popular").GetComponent<Text>().text = task.popularNumber;//热门号码

                tran.Find("unpopular").GetComponent<Text>().text = task.unpopularNumber;//冷门号码

                tran.Find("neutral").GetComponent<Text>().text = task.neutralNumber;//中性号码

                tran.Find("interval").GetComponent<Text>().text = task.intervalNumber;//号码区间

                tran.Find("ac").GetComponent<Text>().text = task.acNumber;//AC值

                tran.Find("parity").GetComponent<Text>().text = task.parityNumber;//奇偶

                tran.Find("mantissa").GetComponent<Text>().text = task.mantissaNumber;//尾数相同个数

                tran.Find("serial").GetComponent<Text>().text = task.serialNumber;//连号个数

                tran.Find("heavy").GetComponent<Text>().text = task.heavyNumber;//重号个数

                tran.Find("sizeratio").GetComponent<Text>().text = task.sizeratioNumber; //大小比       
            });
        }
        //----------------------------------------------------------------------------
        private void CalculatePredictedHitRate()//计算命中率
        {

            for (int i = 0; i < prizeNumberDataList.Count; i++)
            {
                for (int j = 0; j < forecastDataHitRateList.Count; j++)
                {
                    if (prizeNumberDataList[i].numperiods == forecastDataHitRateList[j].numperiods)
                    {
                        for (int z = 0; z < prizeNumberDataList[i].redNumberdata.Count; z++)
                        {
                            for (int g = 0; g < forecastDataHitRateList[j].redballNumList.Count; g++)
                            {
                                if (prizeNumberDataList[i].redNumberdata[z] == forecastDataHitRateList[j].redballNumList[g])
                                {
                                    forecastDataHitRateList[j].errorredballNumList.Add(prizeNumberDataList[i].redNumberdata[z]);
                                }
                            }
                            for (int g = 0; g < forecastDataHitRateList[j].blueballNumList.Count; g++)
                            {
                                if (prizeNumberDataList[i].blueBallNum == forecastDataHitRateList[j].blueballNumList[g])
                                {
                                    forecastDataHitRateList[j].errorblueballNum = prizeNumberDataList[i].blueBallNum;
                                }
                            }
                        }
                    }
                    if (forecastDataHitRateList[j].errorredballNumList.Count > 0 && forecastDataHitRateList[j].errorredballNumList != null)
                    {
                        var disList = forecastDataHitRateList[j].errorredballNumList.Distinct().ToList();
                        forecastDataHitRateList[j].errorredballNumList = disList;
                    }

                }
            }
            if (forecastDataHitRateList.Count > 1)
            {
                for (int i = 1; i < forecastDataHitRateList.Count; i++)
                {
                    var numOne = forecastDataHitRateList[i].errorredballNumList.Count;
                    var numTwo = forecastDataHitRateList[i].redballNumList.Count;
                    var hite = Convert.ToDouble(numTwo - numOne) / Convert.ToDouble(numTwo);
                    forecastDataHitRateList[i].redballHitRate = string.Format("{0:0.00%}", hite);
                    forecastDataHitRateList[i].blueballHitRate = forecastDataHitRateList[i].errorblueballNum == 0 ? "100%" : "0%";
                }
            }
            m_periodHite.text = forecastDataHitRateList[0].numperiods + "期 红篮球命中率:" + forecastDataHitRateList[0].redballHitRate + " "
                + forecastDataHitRateList[0].blueballHitRate;
        }
        //----------------------------------------------------------------------------
        private void SaveForecastData()
        {
            var jsonpath = Application.persistentDataPath + "/forecastdata.json";
            if (!File.Exists(@jsonpath))
            {
                var data = JsonWriter.Serialize(forecastDataHitRateList);
                var streamWriter = new StreamWriter(jsonpath);
                Debug.Log("path: " + Application.persistentDataPath);
                streamWriter.Write(data);
                streamWriter.Close();
            }
            else
            {
                var streamReader = new StreamReader(jsonpath);
                var savedata = streamReader.ReadToEnd();
                var savejsondata = JsonReader.Deserialize<List<ForecastDataHitRate>>(savedata);
                if (forecastDataHitRateList[0].numperiods == savejsondata[0].numperiods)
                {
                    Debug.Log("数据已经是最新，不需要重新写入");
                }
                else
                {
                    streamReader.Close();
                    var data = JsonWriter.Serialize(forecastDataHitRateList);
                    var streamWriter = new StreamWriter(jsonpath);
                    streamWriter.Write(data);
                    streamWriter.Close();
                    Debug.Log("已经写入最新数据");
                }
            }
        }
        //----------------------------------------------------------------------------
        int ForecastDataSort(ForecastDataHitRate t1, ForecastDataHitRate t2)
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
        private int SortBlueRecommendList(NextBlueBallData data1, NextBlueBallData data2)
        {
            if (int.Parse(data1.blueballNum) < int.Parse(data2.blueballNum))
            {
                return -1;
            }
            else
            {
                return 1;
            }
        }
        //----------------------------------------------------------------------------
        private int SortRedRecommendList(NextRedBallData data1, NextRedBallData data2)
        {
            if (int.Parse(data1.redballNum) < int.Parse(data2.redballNum))
            {
                return -1;
            }
            else
            {
                return 1;
            }
        }
        //----------------------------------------------------------------------------
        private void DownLoadRedBallKillData()
        {
            http = new HttpHelper();
            item = new HttpItem
            {
                URL = DownLoadManager.RedKillDoAddress,
                Method = DownLoadManager.MethodType,
                Timeout = DownLoadManager.TimeOunt,
                ReadWriteTimeout = DownLoadManager.ReadWriteTimeout,
                IsToLower = DownLoadManager.IsToLower,
                Cookie = DownLoadManager.Cookie,
                UserAgent = DownLoadManager.UserAgent,
                Accept = DownLoadManager.Accept,
                ContentType = DownLoadManager.ContentType,
                Referer = string.Empty,
                Allowautoredirect = DownLoadManager.Allowautoredirect,
                AutoRedirectCookie = DownLoadManager.AutoRedirectCookie,
                Postdata = DownLoadManager.Postdata,
                ResultType = ResultType.String,
            };

            var httpresult = http.GetHtml(item);
            if (httpresult.StatusCode == HttpStatusCode.OK)
            {
                Debug.LogError("httpresult.StatusCode: " + httpresult.StatusCode);
                var html = httpresult.Html;
                var cookie = httpresult.Cookie;
                Debug.Log(html);
                ParseAnalysisRedData(html);
                SaveAnalysisRedData(m_analysisRedData);
                SavePrizeNumberData(prizeNumberDataList);
            }
            else
            {
                Debug.Log("result.StatusCode: " + httpresult.StatusCode);
            }
        }
        //----------------------------------------------------------------------------
        private void SavePrizeNumberData(List<PrizeNumberData> prizenumberdata)
        {
            var analysisPath = Application.persistentDataPath + "/prizenumberdata.json";

            if (!File.Exists(@analysisPath))
            {
                var data = JsonWriter.Serialize(prizeNumberDataList);
                var streamWriter = new StreamWriter(analysisPath);
                Debug.Log("path: " + Application.persistentDataPath);
                streamWriter.Write(data);
                streamWriter.Close();
            }
            else
            {
                var streamReader = new StreamReader(analysisPath);
                var savedata = streamReader.ReadToEnd();
                var savejsondata = JsonReader.Deserialize<List<PrizeNumberData>>(savedata);
                if (prizeNumberDataList[0].numperiods == savejsondata[0].numperiods)
                {
                    Debug.Log("数据已经是最新，不需要重新下载");
                }
                else
                {
                    streamReader.Close();
                    var data = JsonWriter.Serialize(prizenumberdata);
                    var streamWriter = new StreamWriter(analysisPath, false);
                    streamWriter.Write(data);
                    streamWriter.Close();
                    Debug.Log("已经下载最新数据");
                }

            }
        }
        //----------------------------------------------------------------------------
        private void SaveAnalysisRedData(AnalysisRedData analysisRedData)
        {
            var analysisPath = Application.persistentDataPath + "/analysisreddata.json";

            if (!File.Exists(@analysisPath))
            {
                var data = JsonWriter.Serialize(analysisRedData);
                var streamWriter = new StreamWriter(analysisPath);
                Debug.Log("path: " + Application.persistentDataPath);
                streamWriter.Write(data);
                streamWriter.Close();
            }
            else
            {
                var streamReader = new StreamReader(analysisPath);
                var savedata = streamReader.ReadToEnd();
                var savejsondata = JsonReader.Deserialize<AnalysisRedData>(savedata);
                if (analysisRedData.nextforenumperiods == savejsondata.nextforenumperiods)
                {
                    Debug.Log("数据已经是最新，不需要重新下载");
                }
                else
                {
                    streamReader.Close();
                    var data = JsonWriter.Serialize(analysisRedData);
                    var streamWriter = new StreamWriter(analysisPath, false);
                    streamWriter.Write(data);
                    streamWriter.Close();
                    Debug.Log("已经下载最新数据");
                }

            }
        }
        //----------------------------------------------------------------------------
        private void ParseAnalysisRedData(string html)
        {

            HtmlDocument m_html = new HtmlDocument();
            m_html.LoadHtml(html);
            HtmlNode htmlNode = m_html.DocumentNode;
            //取出近20期红球杀号数据总html
            HtmlNode OneHundreDataTable = htmlNode.SelectSingleNode("//*[@id='showTable']/table");
            Debug.Log("OneHundreDataNode: " + OneHundreDataTable.OuterHtml);
            var tr = OneHundreDataTable.SelectNodes("./tr");
            m_analysisRedData.curforeRedBallList.Clear();
            m_analysisRedData.nextforeRedBallList.Clear();
            prizeNumberDataList.Clear();
            for (int i = 0; i <= 19; i++)
            {
                var forecastTable = tr[i];
                var td = forecastTable.SelectNodes("./td");
                var data = new PrizeNumberData();
                data.numperiods = int.Parse(td[0].InnerText);
                ParsePrizeInnerText(td[1].InnerText, data);
                prizeNumberDataList.Add(data);
            }
            prizeNumberDataList.Sort(PrizeNumberDataSort);
            for (int i = 19; i <= 20; i++)
            {
                var forecastTable = tr[i];
                var redNumList = new List<string>();
                var curredNumList = new List<string>();
                var td = forecastTable.SelectNodes("./td");
                switch (i)
                {
                    case 19:
                        m_analysisRedData.curforenumperiods = td[0].InnerText;
                        m_analysisRedData.curforeHitRate = td[33].InnerText;
                        Debug.Log("当前红球命中个数: " + m_analysisRedData.curforeHitRate);
                        break;
                    case 20:
                        m_analysisRedData.nextforenumperiods = td[0].InnerText;
                        break;
                    default:
                        break;
                }
                for (int j = 3; j <= 30; j += 3)
                {
                    redNumList.Add(td[j].InnerText);
                    if (i == 19)
                        curredNumList.Add(td[j + 1].InnerHtml.Split('\"')[3]);
                }
                for (int j = 0; j < redNumList.Count; j++)
                {
                    var redballdata = new NextRedBallData();
                    var currentredballdata = new CurrentRedBallData();
                    switch (i)
                    {
                        case 19:
                            currentredballdata.redballNum = redNumList[j];
                            currentredballdata.isRightOrNot = curredNumList[j];
                            m_analysisRedData.curforeRedBallList.Add(currentredballdata);
                            Debug.Log("当期红球杀号 : " + m_analysisRedData.curforeRedBallList[j].redballNum);
                            break;
                        case 20:
                            redballdata.redballNum = redNumList[j];
                            m_analysisRedData.nextforeRedBallList.Add(redballdata);
                            Debug.Log("下期红球杀号 : " + m_analysisRedData.nextforeRedBallList[j].redballNum);
                            break;
                        default:
                            break;
                    }
                }
            }
            for (int i = 21; i <= 26; i++)
            {
                var dataTd = tr[i].SelectNodes("./td");
                var dataList = new List<string>();
                for (var j = 2; j <= 20; j += 2)
                {
                    dataList.Add(dataTd[j].InnerText);
                }
                for (int j = 0; j < dataList.Count; j++)
                {
                    switch (i)
                    {
                        case 21:
                            m_analysisRedData.nextforeRedBallList[j].consecutiveErrorsNum = dataList[j];
                            Debug.Log("consecutiveErrorsNum : " + m_analysisRedData.nextforeRedBallList[j].consecutiveErrorsNum);
                            break;
                        case 22:
                            m_analysisRedData.nextforeRedBallList[j].numberOfPairs = dataList[j];
                            Debug.Log("numberOfPairs : " + m_analysisRedData.nextforeRedBallList[j].numberOfPairs);
                            break;
                        case 23:
                            m_analysisRedData.nextforeRedBallList[j].totalCorrectNum = dataList[j];
                            Debug.Log("totalCorrectNum : " + m_analysisRedData.nextforeRedBallList[j].totalCorrectNum);
                            break;
                        case 24:
                            m_analysisRedData.nextforeRedBallList[j].accuracy = dataList[j];
                            Debug.Log("accuracy : " + m_analysisRedData.nextforeRedBallList[j].accuracy);
                            break;
                        case 25:
                            m_analysisRedData.nextforeRedBallList[j].consecutiveErrorsMaxNum = dataList[j];
                            Debug.Log("consecutiveErrorsMaxNum : " + m_analysisRedData.nextforeRedBallList[j].consecutiveErrorsMaxNum);
                            break;
                        case 26:
                            m_analysisRedData.nextforeRedBallList[j].maximumNumOfPairs = dataList[j];
                            Debug.Log("maximumNumOfPairs : " + m_analysisRedData.nextforeRedBallList[j].maximumNumOfPairs);
                            break;
                        default:
                            break;
                    }
                }
            }
        }
        //----------------------------------------------------------------------------
        private int PrizeNumberDataSort(PrizeNumberData t1, PrizeNumberData t2)
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
        private void ParsePrizeInnerText(string td, PrizeNumberData data)
        {
            var t1 = td.Split('+')[0];
            for (int i = 0; i <= 5; i++)
            {
                data.redNumberdata.Add(int.Parse(t1.Split(' ')[i]));
            }
            data.blueBallNum = int.Parse(td.Split('+')[1]);
        }
        //----------------------------------------------------------------------------
        private void DownLoadBuleBallKillData()
        {
            http = new HttpHelper();
            item = new HttpItem
            {
                URL = DownLoadManager.BlueKillDoAddress,
                Method = DownLoadManager.MethodType,
                Timeout = DownLoadManager.TimeOunt,
                ReadWriteTimeout = DownLoadManager.ReadWriteTimeout,
                IsToLower = DownLoadManager.IsToLower,
                Cookie = DownLoadManager.Cookie,
                UserAgent = DownLoadManager.UserAgent,
                Accept = DownLoadManager.Accept,
                ContentType = DownLoadManager.ContentType,
                Referer = string.Empty,
                Allowautoredirect = DownLoadManager.Allowautoredirect,
                AutoRedirectCookie = DownLoadManager.AutoRedirectCookie,
                Postdata = DownLoadManager.Postdata,
                ResultType = ResultType.String,
            };

            var httpresult = http.GetHtml(item);

            if (httpresult.StatusCode == HttpStatusCode.OK)
            {

                Debug.LogError("httpresult.StatusCode: " + httpresult.StatusCode);
                var html = httpresult.Html;
                var cookie = httpresult.Cookie;
                Debug.Log(html);
                ParseAnalysisBlueData(html);
                SaveAnalysisBlueData(m_analysisBlueData);
            }
            else
            {
                Debug.Log("result.StatusCode: " + httpresult.StatusCode);
            }
        }
        //----------------------------------------------------------------------------
        private void SaveAnalysisBlueData(AnalysisBlueData analysisBlueData)
        {
            var analysisPath = Application.persistentDataPath + "/analysisbluedata.json";

            if (!File.Exists(@analysisPath))
            {
                var data = JsonWriter.Serialize(analysisBlueData);
                var streamWriter = new StreamWriter(analysisPath);
                Debug.Log("path: " + Application.persistentDataPath);
                streamWriter.Write(data);
                streamWriter.Close();
            }
            else
            {
                var streamReader = new StreamReader(analysisPath);
                var savedata = streamReader.ReadToEnd();
                var savejsondata = JsonReader.Deserialize<AnalysisBlueData>(savedata);
                if (analysisBlueData.nextforenumperiods == savejsondata.nextforenumperiods)
                {
                    Debug.Log("数据已经是最新，不需要重新下载");
                }
                else
                {
                    streamReader.Close();
                    var data = JsonWriter.Serialize(analysisBlueData);
                    var streamWriter = new StreamWriter(analysisPath, false);
                    streamWriter.Write(data);
                    streamWriter.Close();
                    Debug.Log("已经下载最新数据");
                }
            }
        }
        //----------------------------------------------------------------------------
        private void ParseAnalysisBlueData(string html)
        {
            HtmlDocument m_html = new HtmlDocument();
            m_html.LoadHtml(html);
            HtmlNode htmlNode = m_html.DocumentNode;
            //取出近20期蓝球杀号数据总html
            HtmlNode OneHundreDataTable = htmlNode.SelectSingleNode("//*[@id='showTable']/table");
            Debug.Log("OneHundreDataNode: " + OneHundreDataTable.OuterHtml);
            var tr = OneHundreDataTable.SelectNodes("./tr");
            m_analysisBlueData.curforeBlueBallDataList = new List<CurrentBlueBallData>();
            m_analysisBlueData.nextforeBlueBallDataList = new List<NextBlueBallData>();
            for (int i = 19; i <= 20; i++)
            {
                var forecastTable = tr[i];
                var blueNumList = new List<string>();
                var curredNumList = new List<string>();
                var td = forecastTable.SelectNodes("./td");
                switch (i)
                {
                    case 19:
                        m_analysisBlueData.curforenumperiods = td[0].InnerText;
                        m_analysisBlueData.curforeHitRate = td[33].InnerText;
                        Debug.Log("当前篮球命中个数: " + m_analysisBlueData.curforeHitRate);
                        break;
                    case 20:
                        m_analysisBlueData.nextforenumperiods = td[0].InnerText;
                        break;
                    default:
                        break;
                }
                for (int j = 3; j <= 30; j += 3)
                {
                    blueNumList.Add(td[j].InnerText);
                    if (i == 19)
                        curredNumList.Add(td[j + 1].InnerHtml.Split('\"')[3]);
                }
                for (int j = 0; j < blueNumList.Count; j++)
                {
                    var blueballdata = new NextBlueBallData();
                    var currentblueballdata = new CurrentBlueBallData();
                    switch (i)
                    {
                        case 19:
                            currentblueballdata.blueballNum = blueNumList[j];
                            currentblueballdata.isRightOrNot = curredNumList[j];
                            m_analysisBlueData.curforeBlueBallDataList.Add(currentblueballdata);
                            Debug.Log("blueballNum currentforeBlueBallList : " + m_analysisBlueData.curforeBlueBallDataList[j].blueballNum);
                            break;
                        case 20:
                            blueballdata.blueballNum = blueNumList[j];
                            m_analysisBlueData.nextforeBlueBallDataList.Add(blueballdata);
                            Debug.Log("blueballNum nextforeBlueBallList : " + m_analysisBlueData.nextforeBlueBallDataList[j].blueballNum);
                            break;
                        default:
                            break;
                    }
                }
            }
            for (int i = 21; i <= 26; i++)
            {
                var dataTd = tr[i].SelectNodes("./td");
                var dataList = new List<string>();
                for (var j = 2; j <= 20; j += 2)
                {
                    dataList.Add(dataTd[j].InnerText);
                }
                for (int j = 0; j < dataList.Count; j++)
                {
                    switch (i)
                    {
                        case 21:
                            m_analysisBlueData.nextforeBlueBallDataList[j].consecutiveErrorsNum = dataList[j];
                            Debug.Log("consecutiveErrorsNum : " + m_analysisBlueData.nextforeBlueBallDataList[j].consecutiveErrorsNum);
                            break;
                        case 22:
                            m_analysisBlueData.nextforeBlueBallDataList[j].numberOfPairs = dataList[j];
                            Debug.Log("numberOfPairs : " + m_analysisBlueData.nextforeBlueBallDataList[j].numberOfPairs);
                            break;
                        case 23:
                            m_analysisBlueData.nextforeBlueBallDataList[j].totalCorrectNum = dataList[j];
                            Debug.Log("totalCorrectNum : " + m_analysisBlueData.nextforeBlueBallDataList[j].totalCorrectNum);
                            break;
                        case 24:
                            m_analysisBlueData.nextforeBlueBallDataList[j].accuracy = dataList[j];
                            Debug.Log("accuracy : " + m_analysisBlueData.nextforeBlueBallDataList[j].accuracy);
                            break;
                        case 25:
                            m_analysisBlueData.nextforeBlueBallDataList[j].consecutiveErrorsMaxNum = dataList[j];
                            Debug.Log("consecutiveErrorsMaxNum : " + m_analysisBlueData.nextforeBlueBallDataList[j].consecutiveErrorsMaxNum);
                            break;
                        case 26:
                            m_analysisBlueData.nextforeBlueBallDataList[j].maximumNumOfPairs = dataList[j];
                            Debug.Log("maximumNumOfPairs : " + m_analysisBlueData.nextforeBlueBallDataList[j].maximumNumOfPairs);
                            break;
                        default:
                            break;
                    }
                }
            }
        }
        //----------------------------------------------------------------------------
        private void SerializeAndSaveStatiData(List<TCBStatisticsData> tcbStatiDataList)
        {
            var tcbstajsonpath = Application.persistentDataPath + "/tcbstatistics.json";
            if (!File.Exists(tcbstajsonpath))
            {
                var data = JsonWriter.Serialize(tcbStatiDataList);
                var streamWriter = new StreamWriter(tcbstajsonpath);
                Debug.Log("path: " + Application.persistentDataPath);
                streamWriter.Write(data);
                streamWriter.Close();
            }
            else
            {
                var streamReader = new StreamReader(tcbstajsonpath);
                var savedata = streamReader.ReadToEnd();
                var savejsondata = JsonReader.Deserialize<List<TCBStatisticsData>>(savedata);
                var index = savejsondata.Count - 1;
                if (tcbStatiDataList[0].numperiods == savejsondata[0].numperiods &&
                    !string.IsNullOrEmpty(savejsondata[index].firstprizeNumber))
                {
                    Debug.Log("数据分析已经是最新，不需要重新分析");
                }
                else
                {
                    streamReader.Close();
                    var data = JsonWriter.Serialize(tcbStatiDataList);
                    var streamWriter = new StreamWriter(tcbstajsonpath, false);
                    streamWriter.Write(data);
                    streamWriter.Close();
                    Debug.Log("已经分析为最新数据");
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
        private DownLoadData Deserialize(string json)
        {
            DownLoadData data = JsonReader.Deserialize<DownLoadData>(json);
            Debug.Log("开奖日期: " + data.result[0].date);
            Debug.Log("期数: " + data.result[0].code);
            Debug.Log("红球: " + data.result[0].red);
            Debug.Log("篮球: " + data.result[0].blue);
            Debug.Log("本期销售额:" + data.result[0].sales);
            Debug.Log("奖池滚存: " + data.result[0].poolmoney);
            Debug.Log("一等奖注数: " + data.result[0].prizegrades[0].typenum);
            Debug.Log("一等奖中奖金额: " + data.result[0].prizegrades[0].typemoney);
            Debug.Log("一等奖中奖地区: " + data.result[0].content);
            Debug.Log("二等奖中奖注数:" + data.result[0].prizegrades[1].typenum);
            Debug.Log("二等奖中奖金额:" + data.result[0].prizegrades[1].typemoney);
            return data;
        }
        //----------------------------------------------------------------------------
        private void SerializeAndSaveDownData(DownLoadData downdata)
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
        private List<TCBNumberData> ParseTCBNumberData(List<TCBData> tcbData)
        {

            var TCBNumberDataList = new List<TCBNumberData>();
            for (int i = 0; i < tcbData.Count; i++)
            {
                var numberData = new TCBNumberData();
                string[] redArray = tcbData[i].red.Split(',');
                numberData.numberOne = int.Parse(redArray[0]);
                numberData.numberTwo = int.Parse(redArray[1]);
                numberData.numberThree = int.Parse(redArray[2]);
                numberData.numberFour = int.Parse(redArray[3]);
                numberData.numberFive = int.Parse(redArray[4]);
                numberData.numberSix = int.Parse(redArray[5]);
                numberData.numberBlue = int.Parse(tcbData[i].blue);
                TCBNumberDataList.Add(numberData);
            }
            return TCBNumberDataList;
        }
        //----------------------------------------------------------------------------
        private int SortTcbDataList(TCBData t1, TCBData t2)
        {
            int date1 = int.Parse(t1.code);
            int date2 = int.Parse(t2.code);
            if (date1 < date2)
            {
                return -1;
            }
            else
            {
                return 1;
            }

        }
        //----------------------------------------------------------------------------
        //----------------------------------------------------------------------------
        private List<TCBStatisticsData> CalculateData(List<TCBData> tcbdatalist)
        {

            var tcbstatisticsdatalist = new List<TCBStatisticsData>();

            for (int i = 0; i < tcbdatalist.Count; i++)
            {
                var tcbstatisticsdata = new TCBStatisticsData();
                var task = tcbdatalist[i];
                var tcbcurNumberData = tcbNumberDataList[i];
                var tcbbeforeNumberData = new TCBNumberData();
                if (i > 0)
                {
                    tcbbeforeNumberData = tcbNumberDataList[i - 1];
                }
                tcbstatisticsdata.numperiods = task.code;
                tcbstatisticsdata.prizeNumber = task.red + " " + task.blue;
                tcbstatisticsdata.salesNumber = task.sales;
                tcbstatisticsdata.firstprizeNumber = task.prizegrades[0].typemoney;
                tcbstatisticsdata.valuesNumber = Values(tcbcurNumberData);
                tcbstatisticsdata.distanceNumber = MaxDistance(tcbcurNumberData);
                if (i > 0)
                {
                    tcbstatisticsdata.adjacentNumber = AdjacentNumber(tcbcurNumberData, tcbbeforeNumberData);
                }
                else
                {
                    tcbstatisticsdata.adjacentNumber = 0.ToString();
                }
                tcbstatisticsdata.popularNumber = PopularNum(tcbNumberDataList, tcbcurNumberData, DownType.POPULAR);
                tcbstatisticsdata.unpopularNumber = PopularNum(tcbNumberDataList, tcbcurNumberData, DownType.UNPOPULAR);
                tcbstatisticsdata.neutralNumber = PopularNum(tcbNumberDataList, tcbcurNumberData, DownType.NEUTRAL);
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
        //----------------------------------------------------------------------------
        private string Values(TCBNumberData numberdata)//和值
        {
            int values = 0;
            values = numberdata.numberOne + numberdata.numberTwo + numberdata.numberThree +
                numberdata.numberFour + numberdata.numberFive + numberdata.numberSix;
            return values.ToString();
        }

        //----------------------------------------------------------------------------
        private string MaxDistance(TCBNumberData numberdata)//最大间隔
        {
            List<int> distanceList = new List<int>();
            distanceList.Add(numberdata.numberTwo - numberdata.numberOne);
            distanceList.Add(numberdata.numberThree - numberdata.numberTwo);
            distanceList.Add(numberdata.numberFour - numberdata.numberThree);
            distanceList.Add(numberdata.numberFive - numberdata.numberFour);
            distanceList.Add(numberdata.numberSix - numberdata.numberFive);
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
        private string AdjacentNumber(TCBNumberData curNumberdata, TCBNumberData beforeNumberdata)//相邻号码
        {

            var numone = 0;
            var numtwo = 0;
            var numthree = 0;
            var numfour = 0;
            var numfive = 0;
            var numsix = 0;
            var currentNumList = GetCurNumDataList(curNumberdata);
            var beforeNumList = GetCurNumDataList(beforeNumberdata);

            for (int j = 0; j < beforeNumList.Count; j++)
            {
                if (Mathf.Abs(currentNumList[0] - beforeNumList[j]) == 1)
                {
                    numone = 1;
                }
                if (Mathf.Abs(currentNumList[1] - beforeNumList[j]) == 1)
                {
                    numtwo = 1;
                }
                if (Mathf.Abs(currentNumList[2] - beforeNumList[j]) == 1)
                {
                    numthree = 1;
                }
                if (Mathf.Abs(currentNumList[3] - beforeNumList[j]) == 1)
                {
                    numfour = 1;
                }
                if (Mathf.Abs(currentNumList[4] - beforeNumList[j]) == 1)
                {
                    numfive = 1;
                }
                if (Mathf.Abs(currentNumList[5] - beforeNumList[j]) == 1)
                {
                    numsix = 1;
                }
            }
            return (numone + numtwo + numthree + numfour + numfive + numsix).ToString();
        }
        //----------------------------------------------------------------------------
        private List<int> GetCurNumDataList(TCBNumberData curNumData)
        {
            var currentNum = new List<int>();
            currentNum.Add(curNumData.numberOne);
            currentNum.Add(curNumData.numberTwo);
            currentNum.Add(curNumData.numberThree);
            currentNum.Add(curNumData.numberFour);
            currentNum.Add(curNumData.numberFive);
            currentNum.Add(curNumData.numberSix);
            return currentNum;
        }
        //----------------------------------------------------------------------------
        private string PopularNum(List<TCBNumberData> tcbnumberdata, TCBNumberData curentNumberdata, DownType type)
        {
            var popularOneNum = 0;
            var popularTwoNum = 0;
            var popularThreeNum = 0;
            var popularFourNum = 0;
            var popularFiveNum = 0;
            var popularSixNum = 0;

            var unpopularOneNum = 0;
            var unpopularTwoNum = 0;
            var unpopularThreeNum = 0;
            var unpopularFourNum = 0;
            var unpopularFiveNum = 0;
            var unpopularSixNum = 0;

            var neutralOneNum = 0;
            var neutralTwoNum = 0;
            var neutralThreeNum = 0;
            var neutralFourNum = 0;
            var neutralFiveNum = 0;
            var neutralSixNum = 0;

            List<int> allTcbNum = new List<int>();
            for (int i = 0; i < tcbnumberdata.Count; i++)
            {
                allTcbNum.Add(tcbnumberdata[i].numberOne);
                allTcbNum.Add(tcbnumberdata[i].numberTwo);
                allTcbNum.Add(tcbnumberdata[i].numberThree);
                allTcbNum.Add(tcbnumberdata[i].numberFour);
                allTcbNum.Add(tcbnumberdata[i].numberFive);
                allTcbNum.Add(tcbnumberdata[i].numberSix);
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
            if (dic.ContainsKey(curentNumberdata.numberOne))
            {

                if (dic[curentNumberdata.numberOne].RepeatNum > 21)//平均数18.18
                {
                    popularOneNum = 1;

                }
                else if (dic[curentNumberdata.numberOne].RepeatNum < 15)
                {
                    unpopularOneNum = 1;
                }
                else
                {
                    neutralOneNum = 1;
                }

            }
            if (dic.ContainsKey(curentNumberdata.numberTwo))
            {

                if (dic[curentNumberdata.numberTwo].RepeatNum > 21)//平均数18.18
                {
                    popularTwoNum = 1;

                }
                else if (dic[curentNumberdata.numberTwo].RepeatNum < 15)
                {
                    unpopularTwoNum = 1;
                }
                else
                {
                    neutralTwoNum = 1;
                }

            }
            if (dic.ContainsKey(curentNumberdata.numberThree))
            {

                if (dic[curentNumberdata.numberThree].RepeatNum > 21)//平均数18.18
                {
                    popularThreeNum = 1;

                }
                else if (dic[curentNumberdata.numberThree].RepeatNum < 15)
                {
                    unpopularThreeNum = 1;
                }
                else
                {
                    neutralThreeNum = 1;
                }

            }
            if (dic.ContainsKey(curentNumberdata.numberFour))
            {

                if (dic[curentNumberdata.numberFour].RepeatNum > 21)//平均数18.18
                {
                    popularFourNum = 1;

                }
                else if (dic[curentNumberdata.numberFour].RepeatNum < 15)
                {
                    unpopularFourNum = 1;
                }
                else
                {
                    neutralFourNum = 1;
                }

            }

            if (dic.ContainsKey(curentNumberdata.numberFive))
            {

                if (dic[curentNumberdata.numberFive].RepeatNum > 21)//平均数18.18
                {
                    popularFiveNum = 1;

                }
                else if (dic[curentNumberdata.numberFive].RepeatNum < 15)
                {
                    unpopularFiveNum = 1;
                }
                else
                {
                    neutralFiveNum = 1;
                }
            }
            if (dic.ContainsKey(curentNumberdata.numberSix))
            {

                if (dic[curentNumberdata.numberSix].RepeatNum > 21)//平均数18.18
                {
                    popularSixNum = 1;

                }
                else if (dic[curentNumberdata.numberSix].RepeatNum < 15)
                {
                    unpopularSixNum = 1;
                }
                else
                {
                    neutralSixNum = 1;
                }


            }
            if (type == DownType.POPULAR)
            {
                return (popularOneNum + popularTwoNum + popularThreeNum + popularFourNum + popularFiveNum + popularSixNum).ToString();
            }
            else if (type == DownType.UNPOPULAR)
            {
                return (unpopularOneNum + unpopularTwoNum + unpopularThreeNum + unpopularFourNum + unpopularFiveNum
                    + unpopularSixNum).ToString();
            }
            else
            {
                return (neutralOneNum + neutralTwoNum + neutralThreeNum + neutralFourNum + neutralFiveNum +
                    neutralSixNum).ToString();
            }
        }

        //----------------------------------------------------------------------------
        private string IntervalType(TCBNumberData currentNumberData)//号码区间
        {
            var firstInterval = 0;
            var secondInterval = 0;
            var thirdInterval = 0;
            var curNumberdataList = GetCurNumDataList(currentNumberData);
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
        private string ACNumber(TCBNumberData tcbNumberData) //AC值
        {

            var currentNum = GetCurNumDataList(tcbNumberData);

            List<int> Absovallist = new List<int>();

            for (int i = 0; i < currentNum.Count; i++)
            {
                if (i != 0)
                    Absovallist.Add(Mathf.Abs(tcbNumberData.numberOne - currentNum[i]));
                if (i != 1)
                    Absovallist.Add(Mathf.Abs(tcbNumberData.numberTwo - currentNum[i]));
                if (i != 2)
                    Absovallist.Add(Mathf.Abs(tcbNumberData.numberThree - currentNum[i]));
                if (i != 3)
                    Absovallist.Add(Mathf.Abs(tcbNumberData.numberFour - currentNum[i]));
                if (i != 4)
                    Absovallist.Add(Mathf.Abs(tcbNumberData.numberFive - currentNum[i]));
                if (i != 5)
                    Absovallist.Add(Mathf.Abs(tcbNumberData.numberSix - currentNum[i]));
            }

            Absovallist = Absovallist.Distinct().ToList();//除去列表中重复元素Linq

            return (Absovallist.Count - 5).ToString();

        }
        //----------------------------------------------------------------------------
        private string ParityNumber(TCBNumberData currentData)//奇偶
        {

            var oldNum = 0;
            var even = 0;
            var currentNum = GetCurNumDataList(currentData);

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

            var currentNumList = GetCurNumDataList(currentNumData);

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
        private string SerialNumber(TCBNumberData crrentNumberData)//连号
        {

            var list = GetCurNumDataList(crrentNumberData);

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
            var curNumList = GetCurNumDataList(currentNumberData);
            var befNumList = GetCurNumDataList(curBeforeNumberData);
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
            var list = GetCurNumDataList(currentNumberData);
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
    }
    //----------------------------------------------------------------------------
    public enum DownType
    {
        POPULAR = 0,
        UNPOPULAR = 1,
        NEUTRAL = 2,
    }
    //----------------------------------------------------------------------------
}
