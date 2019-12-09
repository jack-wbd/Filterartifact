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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        //----------------------------------------------------------------------------
        //----------------------------------------------------------------------------
        public struct sUI
        {
            public int nAssetID;
            public UIBase uI;
        }
        //----------------------------------------------------------------------------
        public bool Initialize()
        {
            m_sys = this;
            return true;
        }
        //----------------------------------------------------------------------------
        public void Finalized()
        {

        }
        //----------------------------------------------------------------------------
        public GameObject GetUIRoot()
        {
            return m_objUIRoot;
        }
        //----------------------------------------------------------------------------
        public void AcitivePreLoad()
        {

        }
        //----------------------------------------------------------------------------
        public void AddPreLoadAsset(string strAssetID)
        {

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
        public bool HasUIController(string key)
        {
            return m_dictAllUICtrl.ContainsKey(key);
        }
        //----------------------------------------------------------------------------
        public void AddUIController(string key,sUIUseInfo uiCtrl)
        {
            m_dictAllUICtrl.Add(key, uiCtrl);
        }
        //----------------------------------------------------------------------------
        //----------------------------------------------------------------------------
    }
    //----------------------------------------------------------------------------
}
