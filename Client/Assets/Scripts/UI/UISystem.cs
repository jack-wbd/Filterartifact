//------------------------------------------------------------------------------
/**
	\file	UISystem.cs

	Copyright (c) 2019, BoYue. All rights reserved.

	<PRE>

	模块名称：<文件所属的模块名称>
	文件名称：UISystem.cs
	摘    要：<描述该文件实现的主要功能>

	当前版本：1.0
	建立日期：2019/12/6
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
//	UISystem.cs
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Filterartifact
{
    //----------------------------------------------------------------------------
    public struct sUIUseInfo
    {
        public string strAsset;
        public UIController uiCtrl;
        public eUseEnvir eEnvirUse;
        public bool bReload;
    }
    //----------------------------------------------------------------------------
    public class UISystem : IPreLoad
    {
        //----------------------------------------------------------------------------
        public static UISystem m_sys;
        private GameObject m_objUIRoot;
        private Dictionary<string, sUI> m_dictUIClass;
        private ResourceListData m_data;
        private Dictionary<Type, eUseEnvir> m_dictUIType_Env = new Dictionary<Type, eUseEnvir>();
        public Dictionary<string, sUIUseInfo> m_dictAllUICtrl = new Dictionary<string, sUIUseInfo>();
        private int m_nTotalCount = 0;
        private int m_nAssetCount = 0;
        private List<string> m_listUIRes = null;
        private List<UIController> m_listPreLoadEnvir = new List<UIController>();
        public static int CurAssetCount = 0;
        public static int MaxAssetCount = 0;
        private Dictionary<string, int> m_dictUseCount;
        private List<string> m_listOtherUIRes = null;
        private List<string> m_listUnload;
        private int m_nUIClassIndex;
        private Transform m_rootAttachTrans;
        private UnityCompManager unityCompManager;
        //----------------------------------------------------------------------------
        public struct sUI
        {
            public int nAssetID;
            public UIBase ui;
        }
        //----------------------------------------------------------------------------
        public bool Initialize()
        {
            Messenger.AddListener(DgMsgID.DgMsg_ActiveLoadUI, OnActiveLoadUI);
            Messenger.AddListener<eUseEnvir>(DgMsgID.DgMsg_PreLoadUI, OnPreLoadUI);
            m_listUIRes = new List<string>();
            m_listOtherUIRes = new List<string>();
            m_listUnload = new List<string>();
            m_dictUIClass = new Dictionary<string, sUI>();
            m_dictUseCount = new Dictionary<string, int>();
            m_sys = this;
            return true;
        }
        //----------------------------------------------------------------------------
        private void OnActiveLoadUI()
        {
            AcitvePreLoad();
        }
        //----------------------------------------------------------------------------
        public virtual void AddPreLoadAsset(string strAssetID)
        {

            if (string.IsNullOrEmpty(strAssetID))
            {
                return;
            }
            if (m_listUIRes.Contains(strAssetID))
            {
                return;
            }

            m_listUIRes.Add(strAssetID);

        }
        //----------------------------------------------------------------------------
        private void OnPreLoadUI(eUseEnvir eUse)
        {
            PreLoadUIByUseType(eUse);
        }
        //----------------------------------------------------------------------------
        public void PreLoadUIByUseType(eUseEnvir eUse)
        {
            m_listUIRes.Clear();
            UIController ctrlTemp = null;
            foreach (var item in m_dictAllUICtrl)
            {
                if (item.Value.eEnvirUse == eUse && item.Value.bReload)
                {
                    ctrlTemp = item.Value.uiCtrl;
                    m_listPreLoadEnvir.Add(ctrlTemp);
                    ctrlTemp.StageLoadViewer();
                }
            }
            for (int i = 0; i < m_listOtherUIRes.Count; i++)
            {
                if (m_dictAllUICtrl.ContainsKey(m_listOtherUIRes[i]))
                {
                    ctrlTemp = m_dictAllUICtrl[m_listOtherUIRes[i]].uiCtrl;
                    m_listPreLoadEnvir.Add(ctrlTemp);
                    ctrlTemp.StageLoadViewer();
                }
            }
            m_listOtherUIRes.Clear();
            m_nTotalCount = m_listUIRes.Count;
            MaxAssetCount = m_nTotalCount;
            CurAssetCount = 0;


        }
        //----------------------------------------------------------------------------
        public UIController GetUIControllerById(string key)
        {
            if (m_dictAllUICtrl.ContainsKey(key))
            {
                return m_dictAllUICtrl[key].uiCtrl;
            }
            return null;
        }
        //----------------------------------------------------------------------------
        public bool LoadUIAsset(string strAssetID, Callback<string, UnityEngine.Object> call)
        {
            return assetManager.LoadAssetRes(strAssetID, call);
        }
        //----------------------------------------------------------------------------
        public UnityEngine.Object GetAssetBundle(string strAssetID)
        {
            return assetManager.GetAssetObjByID(strAssetID);
        }
        //----------------------------------------------------------------------------
        public void Update()
        {
            foreach (var element in m_dictUIClass)
            {
                if (element.Value.ui.IsShow)
                {
                    element.Value.ui.Update();
                }
            }
            foreach (var element in m_dictAllUICtrl)
            {
                element.Value.uiCtrl.Update();
            }
        }
        //----------------------------------------------------------------------------
        private void CheckNoUseAtlas()
        {
            m_listUnload.Clear();

            foreach (var item in m_dictUseCount)
            {
                if (item.Value <= 0)
                {
                    m_listUnload.Add(item.Key);
                }
            }

            foreach (var strAsset in m_listUnload)
            {
                m_dictUseCount.Remove(strAsset);
                UnLoadUIAsset(strAsset);
            }

        }
        //----------------------------------------------------------------------------
        private void UnLoadUIAsset(string strAssetID)
        {
            if (assetManager != null)
            {
                assetManager.TobeDelAsset(strAssetID);
            }
        }
        //----------------------------------------------------------------------------
        public void Finalized()
        {
            Messenger.RemoveListener(DgMsgID.DgMsg_ActiveLoadUI, OnActiveLoadUI);
            Messenger.RemoveListener<eUseEnvir>(DgMsgID.DgMsg_PreLoadUI, OnPreLoadUI);
        }
        //----------------------------------------------------------------------------
        public GameObject GetUIRoot()
        {
            return m_objUIRoot;
        }
        //----------------------------------------------------------------------------
        public void AddPreLoadAudio(string strAudioID)
        {

        }
        //----------------------------------------------------------------------------
        public void AddPreLoadCommander(int nCommanderID)
        {

        }
        //----------------------------------------------------------------------------
        public void AddPreLoadEffect(string strEffectID)
        {

        }
        //----------------------------------------------------------------------------
        public bool HasUIClass(string strAssetID)
        {
            if (m_dictUIClass.ContainsKey(strAssetID))
            {
                return true;
            }
            return false;
        }
        //----------------------------------------------------------------------------
        public void GetAssetInfo(string strID, ref sAssetInfo info)
        {
            m_data.GetAssetBundleInfo(strID, ref info);
        }
        //----------------------------------------------------------------------------
        public void AddUITypeEnvToDict(Type type, eUseEnvir env)
        {

            if (!m_dictUIType_Env.ContainsKey(type))
            {
                m_dictUIType_Env.Add(type, env);
            }
            else
            {
                eUseEnvir envout;
                m_dictUIType_Env.TryGetValue(type, out envout);
                if (envout != env)
                {
                    Debug.LogWarning("AddUITypeEnvToDict value env not equal origin value!");
                }
            }
        }
        //----------------------------------------------------------------------------
        public static bool LoadUIOK()
        {
            return CurAssetCount >= MaxAssetCount;
        }
        //----------------------------------------------------------------------------
        public bool HasUIController(string key)
        {
            return m_dictAllUICtrl.ContainsKey(key);
        }
        //----------------------------------------------------------------------------
        public void AddUIController(string key, sUIUseInfo uiCtrl)
        {
            m_dictAllUICtrl.Add(key, uiCtrl);
        }
        //----------------------------------------------------------------------------
        public virtual void AcitvePreLoad()
        {
            if (m_nTotalCount == 0)
            {
                DoFinish();
                return;
            }
            for (int i = 0; i < m_nTotalCount; i++)
            {
                if (!assetManager.LoadAssetRes<string, UnityEngine.Object>(m_listUIRes[i], OnLoadCallBack))
                {
                    CheckFinish();
                }
            }
        }
        //----------------------------------------------------------------------------
        private void OnLoadCallBack(string strAssetID, UnityEngine.Object obj)
        {
            CheckFinish();
        }
        //----------------------------------------------------------------------------
        private void CheckFinish()
        {
            ++m_nAssetCount;
            CurAssetCount = m_nAssetCount;
            if (m_nAssetCount >= m_nTotalCount)
            {
                DoFinish();
            }
        }
        //----------------------------------------------------------------------------
        private void DoFinish()
        {
            m_nAssetCount = 0;
            m_nTotalCount = 0;
            PreInitUIView();
            Messenger.Broadcast(DgMsgID.DgMsg_NtyUILoading_Finish);
        }
        //----------------------------------------------------------------------------
        private void PreInitUIView()
        {
            foreach (UIController ctrl in m_listPreLoadEnvir)
            {
                ctrl.InitViewer(null);
            }
            m_listPreLoadEnvir.Clear();
        }
        //----------------------------------------------------------------------------
        public void AddUseAtlas(string strAtlas)
        {
            if (string.IsNullOrEmpty(strAtlas))
            {
                return;
            }
            if (m_dictUseCount.ContainsKey(strAtlas))
            {
                m_dictUseCount[strAtlas]++;
            }
            else
            {
                m_dictUseCount.Add(strAtlas, 1);
            }

        }
        //----------------------------------------------------------------------------
        public bool InitializeAferMain()
        {
            m_data = FileSystem.Instance().GetResData();
            InitUIRoot();
            Messenger.Broadcast(DgMsgID.DgMsg_RegisterAllUI);
            return true;
        }
        //----------------------------------------------------------------------------
        private void InitUIRoot()
        {
            unityCompManager = WorldManager.Instance().unityCompManager;
            m_rootAttachTrans = unityCompManager.m_rootAttachTrans;
        }
        //----------------------------------------------------------------------------
        public void RemoveUseAtlas(string strAtlas)
        {
            if (m_dictUseCount.ContainsKey(strAtlas))
            {
                m_dictUseCount[strAtlas]--;
            }
        }
        //----------------------------------------------------------------------------
        public int AddUIClass(string strAssetID, UIBase ui)
        {
            if (m_dictUIClass.ContainsKey(strAssetID))
            {
                Debug.LogError(string.Format("ui strAssetID:{0} is already exist!", strAssetID));
                return m_nUIClassIndex;
            }

            if (ui == null)
            {
                Debug.LogError(string.Format("ui strAssetID:{0} UIBase is null!", strAssetID));
            }
            ++m_nUIClassIndex;
            ui.SetUIIndex(m_nUIClassIndex);
            sUI uiTemp;
            uiTemp.nAssetID = 0;
            uiTemp.ui = ui;
            m_dictUIClass.Add(strAssetID, uiTemp);
            AddChildUI(ui.GetUIObj().transform);
            return m_nUIClassIndex;
        }
        //----------------------------------------------------------------------------
        public void AddChildUI(Transform transUI)
        {
            transUI.SetParent(m_rootAttachTrans);
            transUI.localRotation = Quaternion.identity;
            transUI.localPosition = Vector3.zero;
            transUI.localScale = Vector3.one;
        }
        //----------------------------------------------------------------------------
        public static void AddToUIRoot(Transform trans)
        {
            m_sys.AddChildUI(trans);
        }
        //----------------------------------------------------------------------------
        AssetsManager assetManager
        {
            get
            {
                return WorldManager.Instance().GetLayer<AssetLayer>().GetManager();
            }
        }
        //----------------------------------------------------------------------------
    }
    //----------------------------------------------------------------------------
}
