//------------------------------------------------------------------------------
/**
	\file	ResourceListData.cs

	Copyright (c) 2019, BoYue. All rights reserved.

	<PRE>

	模块名称：<数据模块>
	文件名称：ResourceListData.cs
	摘    要：<游戏资源总数据>

	当前版本：1.0
	建立日期：2019/11/14
	作    者：wangbodong
	电子邮件：wangbodong@BoYue.com
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
//	ResourceListData.cs
//------------------------------------------------------------------------------
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

namespace Filterartifact
{
    public class ResourceListData : DataBase
    {
        //----------------------------------------------------------------------------
        private List<string> m_gameInitPreLoadRes = null;//游戏初始化预加载资源列表
        public Dictionary<string, sAssetInfo> m_dicResourseList = null;
        private Dictionary<string, List<string>> m_dictResourseList_1 = null;//临时保存每个UI里面用到了那些图集和字体
        //----------------------------------------------------------------------------
        public override bool Initialize()
        {
            base.Initialize();
            m_gameInitPreLoadRes = new List<string>();
            m_dicResourseList = new Dictionary<string, sAssetInfo>();
            return true;
        }
        //----------------------------------------------------------------------------
        public override void Finalized()
        {
            base.Finalized();
        }
        //----------------------------------------------------------------------------
        public bool InitResourceListFile(string strXmlData)
        {
            string XmlData = strXmlData;
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(XmlData);
            XmlNode node = xmlDoc.SelectSingleNode("Resourselist");
            if (node != null)
            {
                XmlNodeList nodeList = node.SelectSingleNode("ui").ChildNodes;
                int nNodeCount = nodeList.Count;
                for (int i = 0; i < nNodeCount; i++)
                {
                    XmlElement element = nodeList[i] as XmlElement;
                    m_gameInitPreLoadRes.Add(element.GetAttribute("id"));
                }
            }
            return true;
        }
        //----------------------------------------------------------------------------
        public List<string> GetDepsResourceList()
        {
            return m_gameInitPreLoadRes;
        }
        //----------------------------------------------------------------------------
        public void GetAssetBundleInfo(string strID, ref sAssetInfo info)
        {
            m_dicResourseList.TryGetValue(strID, out info);
        }
        //----------------------------------------------------------------------------
        public void LoadResourceListFileDev()
        {
            string strResourceList = FileSystem.Instance().LoadXml_Resourselist_other(Application.streamingAssetsPath + "/Resourselist_2");
            InitResourseList_2File(strResourceList);
            strResourceList = FileSystem.Instance().LoadXml("Common/Resourselist");
            InitResourseListFile(strResourceList);
        }
        //----------------------------------------------------------------------------
        private bool InitResourseListFile(string xmlData)
        {
            string XmlData = xmlData;
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(XmlData);
            XmlNode node = xmlDoc.SelectSingleNode("Resourselist");
            if (node != null)
            {

                XmlNodeList nodeList = node.SelectSingleNode("deps").ChildNodes;
                int nNodeCount = nodeList.Count;
                for (int i = 0; i < nNodeCount; ++i)
                {
                    XmlElement element = nodeList[i] as XmlElement;
                    sAssetInfo info = InsertResourseList(ref element);
                    info.bAlwaysCache = true;
                    m_dicResourseList[info.strID] = info;
                    m_gameInitPreLoadRes.Add(element.GetAttribute("id"));
                }

                nodeList = node.SelectSingleNode("uitexture").ChildNodes;
                nNodeCount = nodeList.Count;
                for (int i = 0; i < nNodeCount; i++)
                {
                    XmlElement element = nodeList[i] as XmlElement;
                    InsertResourseList(ref element, EAssetType.eTexture);
                }



            }
            return true;
        }
        //----------------------------------------------------------------------------
        public sAssetInfo InsertResourseList(ref XmlElement element, EAssetType type = EAssetType.eGameObject)
        {
            sAssetInfo info = ParseResourseList(ref element, type);
            AddElement(ref m_dicResourseList, info.strID, info);
            return info;
        }
        //----------------------------------------------------------------------------
        public sAssetInfo ParseResourseList(ref XmlElement element, EAssetType type = EAssetType.eGameObject)
        {
            sAssetInfo info = sAssetInfo.zero;
            info.strID = element.GetAttribute("id");
            info.assetName = element.GetAttribute("name");
            if (!string.IsNullOrEmpty(info.assetName))
            {
                info.strName = ParseName(info.assetName);
            }
            info.strFile = element.GetAttribute("file").ToLower();
            info.strRealFile = element.GetAttribute("file").ToLower();

            if (string.IsNullOrEmpty(info.strName))
            {
                info.strName = ParseName(info.strFile);
            }

            if (string.IsNullOrEmpty(info.assetName))
            {
                info.assetName = info.strName;
            }
            info.bundleName = ParseName(info.strFile);
            info.bPreLoad = InitToBool(element.GetAttribute("preload"));
            info.eAssetType = type;
            if (element.HasAttribute("EnvirUse"))
            {
                info.ParseEnvir(element.GetAttribute("EnvirUse"));
            }
            else
            {
                info.ParseEnvir("");
            }
            return info;
        }
        //----------------------------------------------------------------------------
        public void AddElement<T1, T2>(ref Dictionary<T1, T2> dict, T1 nID, T2 strName)
        {

            if (dict.ContainsKey(nID))
            {
                string LogStr = string.Format("ResourseList 表配置重复: BundleId={0}", nID);
                Debug.Log(LogStr);
                return;
            }

            dict.Add(nID, strName);

        }
        //----------------------------------------------------------------------------
        protected bool InitToBool(string strOne)
        {
            if (string.IsNullOrEmpty(strOne))
            {
                return false;
            }
            return strOne.Equals("1") ? true : false;
        }
        //----------------------------------------------------------------------------
        public string ParseName(string strFile)
        {
            int nFlage = strFile.LastIndexOf(".");
            string strTemp = strFile.Substring(0, nFlage);
            nFlage = strTemp.LastIndexOf("/");
            if (nFlage > 0)
            {
                strTemp = strTemp.Substring(nFlage + 1);
                return strTemp;
            }
            else
                return strTemp;
        }
        //----------------------------------------------------------------------------
        public void LoadResourceListFileFromBundle()
        {
#if DEBUG
            string strResourceList = FileSystem.Instance().LoadXml_Resourselist_other(Application.streamingAssetsPath + "/Resourselist_2");
#else
            string strResourceList = FileSystem.Instance().LoadXml("Common/Resourselist_2");//从bundle里读取
#endif

            InitResourseList_2File(strResourceList);
            strResourceList = FileSystem.Instance().LoadXml("Common/Resourselist");
            InitResourceListFile(strResourceList);
        }
        //----------------------------------------------------------------------------
        public bool InitResourseList_2File(string strXmlData)
        {
            if (strXmlData == null)
            {
                Debug.LogError("read Resourselist_2.xml error");
                return false;
            }
            string XmlData = strXmlData;
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(XmlData);
            XmlNode node = xmlDoc.SelectSingleNode("Resourselist");
            if (node != null)
            {
                XmlNodeList nodeList = node.SelectSingleNode("uideps").ChildNodes;
                int nNodeCount = nodeList.Count;
                for (int i = 0; i < nNodeCount; i++)
                {
                    XmlElement element = nodeList[i] as XmlElement;
                    if (element != null)
                    {
                        InsertResourseList_1(ref element);
                    }
                }
            }
            return true;
        }
        //----------------------------------------------------------------------------
        public void InsertResourseList_1(ref XmlElement element)
        {
            string strID = element.GetAttribute("id");
            strID = strID.ToLower();
            XmlNodeList nodeList = element.ChildNodes;
            if (nodeList != null)
            {
                int nCount = nodeList.Count;
                List<string> list = new List<string>();
                for (int i = 0; i < nCount; i++)
                {
                    XmlElement ele = nodeList[i] as XmlElement;
                    if (ele.HasAttribute("id"))
                    {
                        list.Add(ele.GetAttribute("id"));
                    }

                }

                if (!string.IsNullOrEmpty(strID))
                {
                    int indexEnd = strID.LastIndexOf(".");
                    if (indexEnd != -1)
                    {
                        string listStr = strID.Substring(0, indexEnd);
                        if (m_dictResourseList_1 != null && !m_dictResourseList_1.ContainsKey(listStr))
                        {
                            m_dictResourseList_1.Add(listStr, list);
                        }
                        else
                        {
                            Debug.LogError("ResourseList_1 has same key id: " + strID);
                        }
                    }
                }
            }
        }
        //----------------------------------------------------------------------------
        //----------------------------------------------------------------------------
        //----------------------------------------------------------------------------
        //----------------------------------------------------------------------------

    }
}
