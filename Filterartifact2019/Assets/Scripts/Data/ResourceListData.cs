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
namespace Filterartifact
{
    public class ResourceListData : DataBase
    {
        //----------------------------------------------------------------------------
        private List<string> m_gameInitPreLoadRes = null;//游戏初始化预加载资源列表
        public Dictionary<string, sAssetInfo> m_dicResourseList = null;
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
            string strResourceList = FileSystem.Instance().LoadXml("/Common/Resourselist");
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
            }
            return true;
        }
        //----------------------------------------------------------------------------
        public sAssetInfo InsertResourseList(ref XmlElement element, EAssetType type = EAssetType.eGameObject)
        {

            sAssetInfo info = sAssetInfo.zero;
            info.strID = element.GetAttribute("id");
            info.strFile = element.GetAttribute("file").ToLower();
            info.strName = ParseName(info.strFile);
            info.bPreLoad = InitToBool(element.GetAttribute("preload"));
            info.eAssetType = type;
            if (element.HasAttribute("EnvirUse"))
            {
                info.ParseEnvir(element.GetAttribute("EnvirUse"));
            }
            else
                info.ParseEnvir("");
            return info;

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
            strTemp = strTemp.Substring(nFlage + 1);
            return strTemp;
        }
        //----------------------------------------------------------------------------
    }
}
