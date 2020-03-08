//------------------------------------------------------------------------------
/**
	\file	UIBase.cs

	Copyright (c) 2019, BoYue. All rights reserved.

	<PRE>

	模块名称：<文件所属的模块名称>
	文件名称：UIBase.cs
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
//	UIBase.cs
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Filterartifact
{
    //----------------------------------------------------------------------------
    public enum eUIImpower
    {
        Default = 0,//默认，可以穿梭于任意层
        MainUI = 1,
        Window = 2, //窗口一级
        Panel = 3, //窗口二级
        Dialog = 4,//提示对话框
        Guid = 5, //引导
        Promt = 6,//提示层，反馈，战力等
        FightUI = 7, //战斗UI
    }
    //----------------------------------------------------------------------------
    public class UIBase : IMsgPipe
    {
        //----------------------------------------------------------------------------
        protected UIBase parent = null;
        public GameObject m_objUI;
        protected Transform m_uiTrans;
        public Image panel;
        protected UISystem m_system;
        static private bool useFullScreenBg = true;
        public string strAssetID = "";
        public string strName = "";
        public string strCtrl = "";
        protected int m_nUIIndex;
        public bool IsShow = false;
        public bool bEffect = true;
        public UIController m_ctrl;
        public int m_nMaxNeed;
        private bool bInitLayout = false;
        private int m_nCurCount;
        public bool m_bLoadResOK = false;
        protected Dictionary<string, Transform> m_newParent = new Dictionary<string, Transform>();
        protected int m_recordTime = 0;
        private List<Transform> m_ChangedChild = new List<Transform>();
        private List<Transform> m_OrigParent = new List<Transform>();
        private List<Vector3> m_OrigPos = new List<Vector3>();
        public bool m_bNoNeedLoadAtlasOrFont = false;
        public object dataObj;
        private float f_Invoke;
        private bool b_Invoke = false;
        private RectTransform m_rect;
        //----------------------------------------------------------------------------
        public bool Create()
        {
            InitLayout();
            if (!OnCreate())
            {
                return false;
            }
            return true;
        }
        //----------------------------------------------------------------------------
        public void Update()
        {
            OnUpdate();
        }
        //----------------------------------------------------------------------------
        public virtual void OnDestroy()
        {

        }
        //----------------------------------------------------------------------------
        public virtual void initData(object arg = null)
        {

        }
        //----------------------------------------------------------------------------
        protected virtual void OnUpdate()
        {

        }
        //----------------------------------------------------------------------------
        public virtual void Show(object arg = null)
        {
            if (_impower == eUIImpower.Dialog)
            {

            }
            dataObj = arg;

            if (m_objUI == null)
            {
                return;
            }

            CheckDelayHide();

            IsShow = true;
            if (m_objUI.activeSelf != true)
            {
                m_objUI.SetActive(true);
            }

            InitDepth();
            m_objUI.SendMessage("DoOpenTween", SendMessageOptions.DontRequireReceiver);
            //隐藏小tips界面
            f_Invoke = 0.5f;
            b_Invoke = true;
            Messenger.Broadcast(DgMsgID.DgUI_HideUI, "UIItemSmallTipCtrl");
            InitRecTTransform();
        }
        //----------------------------------------------------------------------------
        private void CheckDelayHide()
        {
            if (m_ctrl != null && m_ctrl.NeedDelayHide() && strCtrl != "" && strCtrl != "UICircleCtrl")
            {
                Messenger.Broadcast(DgMsgID.DgMsg_HideLateToShow, strCtrl);
                m_ctrl.SetDelayHide(false);
            }
        }
        //----------------------------------------------------------------------------
        protected virtual bool OnCreate()
        {
            return true;
        }
        //----------------------------------------------------------------------------
        private eUIImpower _impower = eUIImpower.Default;
        public eUIImpower impower
        {
            set
            {
                _impower = value;
                InitDepth();
            }
            get { return _impower; }
        }
        //----------------------------------------------------------------------------
        public void SetParent(UIBase _parent)
        {
            parent = _parent;
        }
        //----------------------------------------------------------------------------
        public GameObject GetUIObject()
        {
            return m_objUI;
        }
        //----------------------------------------------------------------------------
        public void SetSystem(UISystem system)
        {
            m_system = system;
        }
        //----------------------------------------------------------------------------
        public UIBase GetParent()
        {
            return parent;
        }
        //----------------------------------------------------------------------------
        public virtual void SetObjUI(GameObject objUI)
        {
            if (objUI == null)
            {
                Debug.LogError("SetObjUI objUI ==null");
                return;
            }
            m_objUI = objUI;
            m_uiTrans = m_objUI.transform;
            if (m_uiTrans == null)
            {
                Debug.LogError("SetObjUI m_uiTrans == null");
                return;
            }

            panel = m_uiTrans.GetComponent<Image>();
            m_rect = m_uiTrans.GetComponent<RectTransform>();
            InitLayout();
            InitAdaption();
            InitDepth();
        }
        //----------------------------------------------------------------------------
        public virtual void Hide()
        {
            IsShow = false;
            if (m_objUI.activeSelf != false)
            {
                m_objUI.SetActive(false);
            }
        }
        //----------------------------------------------------------------------------
        protected virtual void InitLayout()
        {
            if (m_system == null)
            {
                m_system = UISystem.m_sys;
            }

        }
        //----------------------------------------------------------------------------
        public void LoadNeedRes(UIController ctrl)
        {
            m_ctrl = ctrl;
            var depsDict = FileSystem.Instance().GetResData().GetResDepsDict();
            List<string> listRes1 = null;
            depsDict.TryGetValue(m_ctrl.viewer.strName, out listRes1);
            m_nMaxNeed = listRes1.Count;
            m_bNoNeedLoadAtlasOrFont = m_nMaxNeed > 0 ? false : true;
            if (m_bNoNeedLoadAtlasOrFont && m_ctrl != null)
            {
                m_ctrl.SetLoadedOK(true);
                return;
            }
            m_nCurCount = 0;
            if (listRes1 != null)
            {
                for (int i = 0; i < listRes1.Count; i++)
                {
                    if (!m_system.LoadUIAsset(listRes1[i], OnLoadNeedOkInLoading))
                    {
                        Debug.LogWarning("注意： 不是预期的调用：" + listRes1[i]);
                        ++m_nCurCount;
                        CheckFinished();
                    }
                }
            }
        }
        //----------------------------------------------------------------------------
        private void OnLoadNeedOkInLoading(string strAssetID, UnityEngine.Object obj)
        {
            this.strAssetID = strAssetID;
            ++m_nCurCount;
            CheckFinished();
        }
        //----------------------------------------------------------------------------
        protected virtual void InitDepth()
        {
          
        }
        //----------------------------------------------------------------------------
        public virtual void InitRecTTransform()
        {
            if (m_rect)
            {
                m_rect.anchorMin = new Vector2(0, 0);
                m_rect.anchorMax = new Vector2(1, 1);
                m_rect.pivot = new Vector2(0.5f, 0.5f);
                m_rect.offsetMin = new Vector2(0, 0);
                m_rect.offsetMax = new Vector2(0, 0);
                m_rect.localRotation = Quaternion.identity;
                m_rect.localScale = Vector3.one;
                m_rect.anchoredPosition3D = new Vector3(0, 0, 0);
            }
        }
        //----------------------------------------------------------------------------
        private void CheckFinished()
        {
            if (m_nCurCount >= m_nMaxNeed)
            {
                try
                {

                }
                catch (Exception ex)
                {
                    Debug.LogError(ex.Message);
                    Debug.LogError(ex.StackTrace);
                }
                finally
                {
                    m_bLoadResOK = true;
                    if (m_ctrl != null)
                    {
                        m_ctrl.SetLoadedOK(true);
                    }
                    if (m_newParent.Count > 0)
                    {
                        Dictionary<string, Transform>.Enumerator it = m_newParent.GetEnumerator();
                        while (it.MoveNext())
                        {
                            DoChangeChildParent(it.Current.Key, it.Current.Value);
                        }
                    }
                }
            }
        }
        //----------------------------------------------------------------------------
        private float waitToShowTime;
        protected Dictionary<string, Transform> m_dicParent = new Dictionary<string, Transform>();
        //----------------------------------------------------------------------------
        protected virtual void DoChangeChildParent(string childPath, Transform _newParent)
        {
            //没加载完毕
            if (!m_bLoadResOK)
            {

                if (!m_newParent.ContainsKey(childPath))
                {
                    m_newParent[childPath] = _newParent;
                    return;
                }

                if (m_recordTime > 0)
                {
                    if (!m_newParent.ContainsKey(childPath))
                    {
                        m_newParent[childPath] = _newParent;
                    }
                    return;
                }

                if (!IsShow)
                {
                    Debug.LogError("waitForShow");
                    waitToShowTime = Time.time;
                    if (!m_dicParent.ContainsKey(childPath))
                    {
                        m_dicParent[childPath] = _newParent;
                    }
                    return;
                }

                GameObject _childobj = GetChild(childPath);

                if (_childobj == null)
                {

                }


            }
        }
        //----------------------------------------------------------------------------
        private void CheckRestoreChildPosition(string childPath, Transform _newParent)
        {
            if (m_ChangedChild.Count > 0)
            {
                int _count = m_ChangedChild.Count;
                for (int i = 0; i < _count; i++)
                {
                    if (childPath.Contains(m_ChangedChild[i].name))
                    {
                        m_ChangedChild[i].parent = m_OrigParent[i];
                        m_ChangedChild[i].localPosition = m_OrigPos[i];
                        m_ChangedChild[i].parent = _newParent;
                        break;
                    }
                }
            }
        }
        //----------------------------------------------------------------------------
        protected virtual GameObject GetChild(string strPath, bool logError = true)
        {
            Transform _child = m_uiTrans.Find(strPath);
            if (_child == null)
            {
                if (logError)
                {
                    Debug.LogError(string.Format("Can not Find ChildPath:{0},UIAssetID:{1}", strPath, strAssetID));
                }
                return null;
            }

            return _child.gameObject;

        }
        //----------------------------------------------------------------------------
        public virtual void SetCtrl(UIController ctrl)
        {
            m_ctrl = ctrl;
        }
        //----------------------------------------------------------------------------
        public void SetUIIndex(int nUIIndex)
        {
            m_nUIIndex = nUIIndex;
        }
        //----------------------------------------------------------------------------
        protected virtual void InitAdaption()
        {
            Transform bgTF = m_uiTrans.Find("bgSpr");
            if (bgTF != null)
            {
                Vector3 vec3;
                if (!useFullScreenBg)
                {
                    vec3 = WorldManager.Instance().screenUnit.GetAstrictScreenVec3();
                }
                else
                {
                    vec3 = WorldManager.Instance().screenUnit.GetFullVec3();
                }
                RectTransform bgSpr = bgTF.GetComponent<RectTransform>();
                if (bgSpr != null)
                {
                    bgSpr.sizeDelta = new Vector2((int)vec3.x, (int)vec3.y);
                }
                BoxCollider boxCollider = bgTF.GetComponent<BoxCollider>();
                if (boxCollider != null)
                {
                    boxCollider.size = vec3;
                }
            }
        }
        //----------------------------------------------------------------------------
        public void RegisterViewMsg()
        {
            RegisterMsg<string, Transform>((int)DgMsgID.DgMsg_InitStatChange, OnChangeChildParent);
            RegisterMsg((int)MsgID.Msg_NewBie_RestoreUIParent, OnRestoreChildParent);
            RegisterMsg<string, int>((int)MsgID.Msg_NewBie_DyUIScrollViewEvent, OnDyUIScrollViewEvent);
        }
        //----------------------------------------------------------------------------
        public void OnChangeChildParent(string childpath, Transform _newparent)
        {

        }
        //----------------------------------------------------------------------------
        public void OnRestoreChildParent()
        {

        }
        //----------------------------------------------------------------------------
        public void OnDyUIScrollViewEvent(string childPath, int index)
        {

        }
        //----------------------------------------------------------------------------

    }
}
