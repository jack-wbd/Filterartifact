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
    public void GetAssetBundleInfo(string strID,ref sAssetInfo info)
    {
        m_dicResourseList.TryGetValue(strID, out info);
    }
    //----------------------------------------------------------------------------





}
