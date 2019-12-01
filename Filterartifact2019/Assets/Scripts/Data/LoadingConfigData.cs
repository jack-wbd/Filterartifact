//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
// Using
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
// Class
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	LoadingConfigData.cs
//------------------------------------------------------------------------------
/**
\file	LoadingConfigData.cs

Copyright (c) 2019, BoYue. All rights reserved.

<PRE>

模块名称�?文件所属的模块名称>
文件名称：LoadingConfigData.cs
�?   要：<描述该文件实现的主要功能>

当前版本�?.0
建立日期�?019/11/30
�?   者：lenovo
电子邮件�?username%@BoYue.com
�?   注：<其它说明>

</PRE>
*/
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Filterartifact
{
    //----------------------------------------------------------------------------
    public class TipsInfo
    {
        //----------------------------------------------------------------------------
        public string strTips;
        public int nWeight;
        public string strAltas;
        public int ID;
        //----------------------------------------------------------------------------
        public void Serialize(BinaryWriter bw)
        {
            GameUtil.Write(bw, strTips);
            bw.Write(nWeight);
            GameUtil.Write(bw, strAltas);
            bw.Write(ID);
        }
        //----------------------------------------------------------------------------
        public void Deserialize(BinaryReader br)
        {
            strTips = GameUtil.ReadString(br);
            nWeight = br.ReadInt32();
            strAltas = GameUtil.ReadString(br);
            ID = br.ReadInt32();
        }
        //----------------------------------------------------------------------------
        //----------------------------------------------------------------------------

    }
    //----------------------------------------------------------------------------
    public class LoadingConfigData : DataBase
    {
        //----------------------------------------------------------------------------
        public bool IsInChallenage = false;
        private int MaxCount = 10;
        private List<LoadingInfo> m_loadInfoList;
        private List<string> m_listAllTexture;
        private List<TipsInfo> mTipsArray;
        //----------------------------------------------------------------------------
        public string GetLoadTexture(bool bFirst = false)
        {
            if (bFirst)
            {
                return m_loadInfoList[0].strTextureID;
            }
            else
            {
                float random = UnityEngine.Random.Range(0f, 101f);
                int currPer = 0;
                int totalPer = 0;
                for (int i = 0; i < m_loadInfoList.Count; ++i)
                {
                    totalPer += m_loadInfoList[i].nWeight;
                    if (random >= currPer && random <= totalPer)
                    {
                        return m_loadInfoList[i].strTextureID;
                    }
                }
            }
            return "";
        }
        //----------------------------------------------------------------------------
        public override bool Initialize()
        {
            m_loadInfoList = new List<LoadingInfo>();
            m_listAllTexture = new List<string>(MaxCount);
            LoadTextureFile(XmlFile.LoadingTexture);
            return base.Initialize();
        }
        //----------------------------------------------------------------------------
        public override void Finalized()
        {
            base.Finalized();
        }
        //----------------------------------------------------------------------------
        private void LoadTextureFile(string strFileName)
        {
            string XmlData = FileSystem.Instance().LoadXml(strFileName);
            XmlDocument xmlDouc = new XmlDocument();
            xmlDouc.LoadXml(XmlData);
            XmlNode nodeRoot = xmlDouc.SelectSingleNode("LoadingConfig");
            XmlElement element = nodeRoot.FirstChild as XmlElement;
            LoadingInfo info = null;
            while (element != null)
            {
                info = new LoadingInfo();
                info.strName = element.GetAttribute("name");
                info.strTextureID = element.GetAttribute("TextureAssetID");
                info.nWeight = int.Parse(element.GetAttribute("weight"));
                m_loadInfoList.Add(info);
                if (!m_listAllTexture.Contains(info.strTextureID))
                {
                    m_listAllTexture.Add(info.strTextureID);
                }
                element = element.NextSibling as XmlElement;
            }
        }
    }
    //----------------------------------------------------------------------------
    public class LoadingInfo
    {
        public string strName;
        public string strTextureID;
        public int nWeight;

    }
    //----------------------------------------------------------------------------
    //----------------------------------------------------------------------------
    //----------------------------------------------------------------------------

}
