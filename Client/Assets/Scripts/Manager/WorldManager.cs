using System;
using System.Collections.Generic;
//------------------------------------------------------------------------------
/**
	\file	WorldManager.cs

	Copyright (c) 2019, BoYue. All rights reserved.

	<PRE>

	模块名称：<文件所属的模块名称>
	文件名称：WorldManager.cs
	摘    要：<描述该文件实现的主要功能>

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
//	WorldManager.cs
//------------------------------------------------------------------------------

namespace Filterartifact
{
    public class WorldManager
    {
        //----------------------------------------------------------------------------
        public static WorldManager m_tSingleton = null;
        public GameState_Main m_gameStateMain = null;
        private Dictionary<Type, BaseLayer> m_dictLayer = null;
        private Dictionary<Type, BaseLayer>.Enumerator m_itor;
        private DataLayer m_layerData = null;
        private int nLayerNum = 0;
        public ScreenUnit screenUnit;
        private StageLayer m_layerStage = null;
        //----------------------------------------------------------------------------
        public static WorldManager CreateInstance()
        {
            if (m_tSingleton != null)
            {
                m_tSingleton = null;
            }
            m_tSingleton = new WorldManager();

            return m_tSingleton;
        }
        //----------------------------------------------------------------------------
        public static WorldManager Instance()
        {
            return m_tSingleton;
        }
        //----------------------------------------------------------------------------
        public static void ReleaseInstance()
        {
            if (m_tSingleton != null)
            {
                m_tSingleton = null;
            }
        }
        //----------------------------------------------------------------------------
        public void IniDataLayer()
        {
            DoInitDataLayer();
        }
        //----------------------------------------------------------------------------
        public void CreateLayer()
        {
            DoCreateLayer();
        }
        //----------------------------------------------------------------------------
        private void DoCreateLayer()
        {
            m_layerData.SaveStreamData();
            m_layerStage = AddLayerToDict<StageLayer>();
            AssetLayer t = new AssetLayer();
            t.Initialize();
            m_dictLayer.Add(typeof(AssetLayer), t);
            nLayerNum = m_dictLayer.Count;
        }

        //----------------------------------------------------------------------------
        public virtual bool CreateWorld()
        {
            CreateMainState();
            screenUnit = new ScreenUnit();
            return true;
        }
        //----------------------------------------------------------------------------
        private void CreateMainState()
        {
            m_gameStateMain = new GameState_Main((int)GameStateType.GST_Main, null);
            if (!ReferenceEquals(m_gameStateMain, null))
            {
                m_gameStateMain.Init();
            }
        }
        //----------------------------------------------------------------------------
        public virtual void Destroy()
        {
            DestroyLayer();
            ReleaseInstance();
        }
        //----------------------------------------------------------------------------
        private void DestroyLayer()
        {
            if (m_dictLayer == null)
            {
                return;
            }
            foreach (KeyValuePair<Type, BaseLayer> temp in m_dictLayer)
            {
                temp.Value.Finalized();
            }
        }
        //----------------------------------------------------------------------------
        public virtual void Update()
        {
            if (!ReferenceEquals(m_gameStateMain, null))
            {
                m_gameStateMain.Update();
            }

            if (FileSystem.Instance() != null)
            {
                FileSystem.Instance().Update();
            }
        }
        //----------------------------------------------------------------------------
        public T GetLayer<T>() where T : BaseLayer
        {
            if (m_dictLayer.ContainsKey(typeof(T)))
            {
                return m_dictLayer[typeof(T)] as T;
            }
            return null;
        }
        //----------------------------------------------------------------------------
        public T GetDataCollection<T>() where T : DataBase
        {
            if (m_layerData != null)
            {
                return m_layerData.GetCollection(typeof(T)) as T;
            }
            return null;
        }
        //----------------------------------------------------------------------------
        private bool DoInitDataLayer()
        {
            m_dictLayer = new Dictionary<Type, BaseLayer>();
            m_layerData = AddLayerToDict<DataLayer>();
            return true;
        }
        //----------------------------------------------------------------------------
        private T AddLayerToDict<T>() where T : BaseLayer, new()
        {
            T t = new T();
            m_dictLayer.Add(t.GetType(), t);
            t.Initialize();
            return t;
        }
        //----------------------------------------------------------------------------
        //----------------------------------------------------------------------------
    }
}

