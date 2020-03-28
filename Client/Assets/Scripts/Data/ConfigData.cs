//------------------------------------------------------------------------------
/**
	\file	ConfigData.cs

	Copyright (c) 2019, BoYue. All rights reserved.

	<PRE>

	模块名称：<数据模块>
	文件名称：ConfigData.cs
	摘    要：<描述该文件实现的主要功能>

	当前版本：1.0
	建立日期：2019/11/28
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
//	ConfigData.cs
//------------------------------------------------------------------------------
using System.Xml;
using UnityEngine;

public class ConfigData
{
    //----------------------------------------------------------------------------
    public string m_strDataDir = null;
    public string m_strResDir = null;
    public string m_strLocal = null;
    //----------------------------------------------------------------------------
    public bool InitConfigFile(string strXmlData)
    {
        string XmlData = strXmlData;
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(XmlData);
        XmlNode node = xmlDoc.SelectSingleNode("config");
        if (node != null)
        {

            XmlElement Element = node.SelectSingleNode("datadir") as XmlElement;
#if GMORDER
            m_strDataDir = Application.dataPath + "/Data/data/";
#endif
            Element = node.SelectSingleNode("resdir") as XmlElement;
            m_strResDir = Element.InnerText;
            Element = node.SelectSingleNode("local") as XmlElement;
            m_strLocal = Element.InnerText;
        }
        return true;
    }
    //----------------------------------------------------------------------------


}

