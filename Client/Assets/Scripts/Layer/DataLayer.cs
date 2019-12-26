//------------------------------------------------------------------------------
/**
	\file	DataLayer.cs

	Copyright (c) 2019, BoYue. All rights reserved.

	<PRE>

	模块名称：<文件所属的模块名称>
	文件名称：DataLayer.cs
	摘    要：<描述该文件实现的主要功能>

	当前版本：1.0
	建立日期：2019/12/5
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
//	DataLayer.cs
//------------------------------------------------------------------------------
using LitJson;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filterartifact
{
    //----------------------------------------------------------------------------
    public class DataLayer : BaseLayer
    {
        // variable
        private Dictionary<Type, DataBase> m_dictDataCollection;
        private Queue<DataBase> m_queNeedToParse;
        private XmlDataSerialize m_fileSer;
        //----------------------------------------------------------------------------
        // constructor
        public DataLayer()
        {
            m_dictDataCollection = new Dictionary<Type, DataBase>();
            m_queNeedToParse = new Queue<DataBase>();
        }
        //----------------------------------------------------------------------------
        public override bool Initialize()
        {
            base.Initialize();
            m_fileSer = new XmlDataSerialize();
            m_fileSer.Initialize();
            RigisterCollection();
            return true;
        }
        //----------------------------------------------------------------------------
        public override void Finalized()
        {
            base.Finalized();
            m_fileSer.Finalized();
            m_fileSer = null;
        }
        //----------------------------------------------------------------------------
        public void ReloadTbl()
        {

        }
        //----------------------------------------------------------------------------
        public Queue<DataBase> GetNeedParseDataQue()
        {
            return m_queNeedToParse;
        }
        //----------------------------------------------------------------------------
        public void RigisterCollection()
        {
            AddCollection(typeof(LoadingConfigData), new LoadingConfigData());
        }
        //----------------------------------------------------------------------------
        public BinaryWriter GetStreamWriter()
        {
            if (m_fileSer != null)
            {
                return m_fileSer.GetWriter();
            }
            return null;
        }
        //----------------------------------------------------------------------------
        public BinaryReader GetStreamReader()
        {
            if (m_fileSer!=null)
            {
                return m_fileSer.GetReader();
            }
            return null;
        }
        //----------------------------------------------------------------------------
        public bool HasStreamData()
        {
            if (m_fileSer!=null)
            {
                return m_fileSer.HasData();
            }
            return false;
        }
        //----------------------------------------------------------------------------
        public void FlushStream()
        {
            if (m_fileSer!=null)
            {
                m_fileSer.FlushStream();
            }
        }
        //----------------------------------------------------------------------------
        public void SaveStreamData()
        {
            if (m_fileSer!=null)
            {
                m_fileSer.SaveStream();
            }
        }
        //----------------------------------------------------------------------------
        public void AddCollection(Type type, DataBase data)
        {
            if (!m_dictDataCollection.ContainsKey(type))
            {
                data.SetParent(this);
                m_dictDataCollection.Add(type, data);
                m_queNeedToParse.Enqueue(data);
            }
        }
        //----------------------------------------------------------------------------
        public void AddSingleCollection(Type type, SingleBineryData data)
        {
            if (!m_dictDataCollection.ContainsKey(type))
            {
                data.SetParent(this);
                m_dictDataCollection.Add(type, data);
                data.LoadBinery(false);
                DataBase baseData = new DataBase();
                m_queNeedToParse.Enqueue(baseData);
            }
        }
        //----------------------------------------------------------------------------
        public DataBase GetCollection(Type type)
        {
            DataBase baseData = null;
            m_dictDataCollection.TryGetValue(type, out baseData);
            return baseData;
        }
        //----------------------------------------------------------------------------
        public T GetCollection<T>() where T : DataBase
        {
            return GetCollection(typeof(T)) as T;
        }
        //----------------------------------------------------------------------------
        public void UpdateCollection<T>(JsonData data)
        {
            GetCollection(typeof(T)).UpdateData(data);
        }
        //----------------------------------------------------------------------------
        public void UpdateCollection<T>(object data)
        {
            GetCollection(typeof(T)).UpdateData(data);
        }
        //----------------------------------------------------------------------------
        public void ClearData()
        {
            if (m_dictDataCollection != null)
            {
                var it = m_dictDataCollection.GetEnumerator();
                while (it.MoveNext())
                {
                    var data = it.Current.Value;
                    if (data != null)
                    {
                        data.Clear();
                    }
                    it.Dispose();
                }
            }
        }
        //----------------------------------------------------------------------------
    }
    //----------------------------------------------------------------------------
}
