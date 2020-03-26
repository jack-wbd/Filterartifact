using System;
using System.Collections.Generic;
using UnityEngine;
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
        public UnityCompManager unityCompManager;
        public static Transform WorldHudUI;
        public sPVMParamInfo m_NextCityParam = sPVMParamInfo.Clear;
        public bool m_bCanSwitchScene = false;//跳转信息，在下一帧跳场景
        public bool m_bisJumpPoint = false;
        public Vector3 m_vecHeroJumpPoint = Vector3.zero;//临时跳转点
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
        private void CreateWorldNode()
        {
            GameObject temp = new GameObject("WorldHudUI");
            UnityEngine.Object.DontDestroyOnLoad(temp);
            WorldHudUI = temp.transform;
            UISystem.AddToUIRoot(WorldHudUI);
        }
        //----------------------------------------------------------------------------
        public virtual bool CreateWorld()
        {
            CreateMainState();
            screenUnit = new ScreenUnit();
            unityCompManager = UnityCompManager.CreateInstance();
            unityCompManager.Initionlization();
            unityCompManager.InitUIRoot();
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
            UnityCompManager.ReleaseInstance();
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

            if (m_bCanSwitchScene)
            {
                if (!ReferenceEquals(m_NextCityParam, null))
                {
                    if (!m_bisJumpPoint)
                    {
                        m_vecHeroJumpPoint = m_NextCityParam.borthPoint;
                        m_bCanSwitchScene = false;
                        SwitchScene(m_NextCityParam.nGroupTempID, m_NextCityParam.nCopyTempID, m_NextCityParam.nStageTempID, m_NextCityParam.strName, 0);
                    }
                }
            }

            if (!ReferenceEquals(m_gameStateMain, null))
            {
                m_gameStateMain.Update();
            }

            if (FileSystem.Instance() != null)
            {
                FileSystem.Instance().Update();
            }

            UpdateLayer();

        }
        //----------------------------------------------------------------------------
        public bool SwitchScene(int nGroupTempID, int nCopyTempID, int nStageTempID, string strCopy, int nStageIndex)
        {
            StageLayer layer = GetLayer<StageLayer>();
            if (layer != null)
            {
                layer.SwitchScene(nGroupTempID, nCopyTempID, nStageTempID, strCopy, nStageIndex);
            }
            return true;
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
        public void ProMsgToState(int msgId)
        {
            if (ReferenceEquals(m_gameStateMain, null))
            {
                return;
            }
            m_gameStateMain.ProcessMsg(msgId);
        }
        //----------------------------------------------------------------------------
        public void ProMsgToState<T>(int msgId, T arg)
        {
            if (ReferenceEquals(m_gameStateMain, null))
            {
                return;
            }
            m_gameStateMain.ProcessMsg(msgId, arg);
        }
        //---------------------------------------------------------------------------- 
        public void InitGame()
        {
            Messenger.Broadcast(DgMsgID.DgMsg_InitAfterMain);
            CreateWorldNode();
        }
        //----------------------------------------------------------------------------
        public void SetSwitchSceneParam(ref sPVMParamInfo Info, bool bIsJumpPoint = false)
        {
            m_NextCityParam = Info;
            m_bCanSwitchScene = true;
            m_bisJumpPoint = bIsJumpPoint;
        }
        //----------------------------------------------------------------------------
        private void UpdateLayer()
        {
            if (m_dictLayer == null)
            {
                return;
            }
            m_itor = m_dictLayer.GetEnumerator();
            while (m_itor.MoveNext())
            {
                m_itor.Current.Value.Update();
            }
        }
        //----------------------------------------------------------------------------

    }
}

