//------------------------------------------------------------------------------
/**
	\file	PromptData.cs

	Copyright (c) 2020, BoYue. All rights reserved.

	<PRE>

	模块名称：<文件所属的模块名称>
	文件名称：PromptData.cs
	摘    要：<描述该文件实现的主要功能>

	当前版本：1.0
	建立日期：2020/4/7
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
//	PromptData.cs
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Filterartifact
{
    //----------------------------------------------------------------------------
    public struct stDescParam
    {
        public int nIndex;
        public int nBegin;
        public int nEnd;
        //----------------------------------------------------------------------------
        public void Deserialize(BinaryReader bw)
        {
            nIndex = bw.ReadInt32();
            nBegin = bw.ReadInt32();
            nEnd = bw.ReadInt32();
        }
        //----------------------------------------------------------------------------
        public void Serialize(BinaryWriter bw)
        {
            bw.Write(nIndex);
            bw.Write(nBegin);
            bw.Write(nEnd);
        }
        //----------------------------------------------------------------------------
    }
    //----------------------------------------------------------------------------
    public struct stPromptData
    {
        public string strDesc;
        public List<stDescParam> paramList;
        //----------------------------------------------------------------------------
        public void Serialize(BinaryWriter bw)
        {
            GameUtil.Write(bw, strDesc);
            if (paramList != null)
            {
                bw.Write(paramList.Count);
                foreach (stDescParam obj in paramList)
                {
                    obj.Serialize(bw);
                }
            }
            else
            {
                bw.Write(-1);
            }

        }
        //----------------------------------------------------------------------------
        public void Deserialize(BinaryReader bw)
        {
            strDesc = GameUtil.ReadString(bw);
            int count = bw.ReadInt32();
            if (count == -1)
            {
                return;
            }
            stDescParam obj = new stDescParam();
            for (int i = 0; i < count; i++)
            {
                obj.Deserialize(bw);
                paramList.Add(obj);
            }
        }
    }
    //----------------------------------------------------------------------------
    public class PromptData : DataBase
    {
        protected Dictionary<string, stPromptData> m_DescDict;
        //----------------------------------------------------------------------------
        public override void Deserialize()
        {
            BinaryReader bw = m_layerData.GetStreamReader();
            if (bw == null)
            {
                return;
            }
            int count = bw.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                string key = GameUtil.ReadString(bw);
                stPromptData obj = new stPromptData();
                obj.Deserialize(bw);
                m_DescDict.Add(key, obj);
            }

        }
        //----------------------------------------------------------------------------
        public override void Serialize()
        {
            BinaryWriter bw = m_layerData.GetStreamWriter();
            if (bw == null)
            {
                return;
            }
            bw.Write(m_DescDict.Count);
            foreach (var item in m_DescDict)
            {
                GameUtil.Write(bw, item.Key);
                item.Value.Serialize(bw);
            }
        }
        //----------------------------------------------------------------------------
        public override bool Initialize()
        {
            m_DescDict = new Dictionary<string, stPromptData>();
            if (m_layerData.HasStreamData())
            {
                Deserialize();
            }
            else
            {
                LoadXml(XmlFile.PromptLang);
                Serialize();
            }
            return true;
        }
        //----------------------------------------------------------------------------
        private void LoadXml(string strFileName)
        {
            string XmlData = FileSystem.Instance().LoadXml(strFileName);
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(XmlData);
            XmlNode rootNode = xmlDoc.SelectSingleNode("PromptData");
            if (rootNode == null)
            {
                return;
            }
            XmlElement indexElm = rootNode.FirstChild as XmlElement;
            if (indexElm == null)
            {
                return;
            }
            XmlElement elm = indexElm.FirstChild as XmlElement;
            while (elm != null)
            {
                stPromptData data = new stPromptData();
                string id = elm.GetAttribute("ID");
                string desc = elm.GetAttribute("Desc");
                data.strDesc = desc;
                m_DescDict.Add(id, data);
                elm = elm.NextSibling as XmlElement;
            }
        }
        //----------------------------------------------------------------------------
        public static string GetPrompt(string strID)
        {
            string strDesc;
            PromptData data = WorldManager.Instance().GetDataCollection<PromptData>();
            data.MakePromptDesc(strID, out strDesc);
            return strDesc;
        }
        //----------------------------------------------------------------------------
        public void MakePromptDesc(string strID, out string strDesc)
        {
            if (m_DescDict.ContainsKey(strID))
            {

                strDesc = m_DescDict[strID].strDesc;
            }
            else
            {
                strDesc = strID;
            }
        }
        //----------------------------------------------------------------------------
    }
}
