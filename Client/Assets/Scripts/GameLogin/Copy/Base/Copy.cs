//------------------------------------------------------------------------------
/**
	\file	Copy.cs

	Copyright (c) 2020, BoYue. All rights reserved.

	<PRE>

	模块名称�?文件所属的模块名称>
	文件名称：Copy.cs
	�?   要：<描述该文件实现的主要功能>

	当前版本�?.0
	建立日期�?020/3/14
	�?   者：lenovo
	电子邮件�?username%@BoYue.com
	�?   注：<其它说明>

	</PRE>
*/
//------------------------------------------------------------------------------
// Using
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
// Class
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	Copy.cs
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Filterartifact
{
    //----------------------------------------------------------------------------
    public class Copy : IBase
    {
        //----------------------------------------------------------------------------
        protected Dictionary<int, Scene> m_sceneDict = null;
        protected Scene m_CurScene;
        protected StageLoad m_stageLoad = null;
        protected Scene m_lastScene;
        protected Scene m_nextScene;
        protected string m_strSceneName;
        protected string m_strUIName = null;//用于UI资源
        protected int m_nBGM;
        protected int m_nCopyTempID;
        protected int m_nGroupTempID;
        private StageLayer m_stageLayer = null;
        protected CopyStateCtrl m_stateCtrl = null;
        //----------------------------------------------------------------------------
        public bool ActiveScene(int nSceneTempID)
        {
            if (m_sceneDict == null)
            {


            }
            if (!m_sceneDict.ContainsKey(nSceneTempID))
            {
                return false;
            }
            Scene scene = m_sceneDict[nSceneTempID];
            if (scene == null)
            {
                return false;
            }

            if (m_CurScene != null)
            {
                PullOutMsgPipe(m_CurScene);
                m_stageLoad.UnLoad();
                m_lastScene = m_CurScene;
                m_CurScene = null;
            }

            m_nextScene = scene;
            m_nextScene.Init();

            m_CurScene = m_nextScene;
            if (m_stageLoad != null)
            {
                m_stageLoad.StartSceneLoad(m_nextScene);
            }
            else
            {

            }


            return true;


        }
        //----------------------------------------------------------------------------
        public virtual void CreateFinished()
        {

            //资源加载完毕
            WorldManager.Instance().GetLayer<StageLayer>().ProcessMsg((int)MsgID.MSG_LOADING_ALLREADY);

        }
        //----------------------------------------------------------------------------
        public override void Update()
        {
            base.Update();
            if (m_CurScene != null)
            {
                m_CurScene.Update();
            }
            if (m_stageLoad != null)
            {
                m_stageLoad.Update();
                if (m_stageLoad.IsLoadOK())
                {
                    m_stageLoad.SetLoadOk(false);
                    AssetBundleAllLoaded();
                }
            }

        }
        //----------------------------------------------------------------------------
        public void AssetBundleAllLoaded()
        {
            m_CurScene = m_nextScene;
            m_CurScene.LoadAllFinished();
            PlugInMsgPipe(m_CurScene);
        }
        //----------------------------------------------------------------------------
        public void SetLayer(StageLayer layer)
        {
            m_stageLayer = layer;
        }
        //----------------------------------------------------------------------------
        public virtual bool Init()
        {
            return true;
        }
        //----------------------------------------------------------------------------
        public void ActiveCopy()
        {
            foreach (KeyValuePair<int, Scene> element in m_sceneDict)
            {
                element.Value.ParseSceneData();
            }
        }
        //----------------------------------------------------------------------------
        public string GetCopyName()
        {
            return m_strSceneName;
        }
        //----------------------------------------------------------------------------
        public Copy(StageLayer layer)
        {
            m_stageLoad = layer.m_stageLoad;
            m_stageLoad.SetCopy(this);
            InitComponent();
        }
        //----------------------------------------------------------------------------
        public bool Create(XmlNode node, ref string strCopyName, eSceneType type)
        {
            XmlElement mainElm = node as XmlElement;
            if (mainElm == null)
            {
                return false;
            }
            if (m_sceneDict == null)
            {
                m_sceneDict = new Dictionary<int, Scene>();
            }
            m_strSceneName = mainElm.GetAttribute("Name");
            if (mainElm.HasAttribute("UIName"))
            {
                m_strUIName = mainElm.GetAttribute("UIName");
            }
            string strBGM = mainElm.GetAttribute("BGM");
            if (!string.IsNullOrEmpty(strBGM))
            {
                m_nBGM = int.Parse(strBGM);
            }
            const string strStage = "Stage";
            const string strPart = "Part";
            const string strName = "Name";
            int sceneIndex = 0;
            XmlElement startElm = mainElm.SelectSingleNode(strStage) as XmlElement;
            while (startElm != null)
            {
                int nStageID = int.Parse(startElm.GetAttribute("ID"));
                if (startElm.HasAttribute("TypeID"))
                {
                    int nType = int.Parse(startElm.GetAttribute("TypeID"));
                    type = (eSceneType)nType;
                }
                float nBackSpeed = 0f;
                float nMedSpeed = 0f;
                if (startElm.HasAttribute("BackSpeed"))
                {
                    nBackSpeed = float.Parse(startElm.GetAttribute("BackSpeed"));
                }
                if (startElm.HasAttribute("MedSpeed"))
                {
                    nMedSpeed = float.Parse(startElm.GetAttribute("MedSpeed"));
                }
                Scene newScene = null;
                newScene = CopyFactory.CreateScene(type, this);
                newScene.SetType(type);
                newScene.SetStageID(nStageID);
                newScene.SetStageBackSpeed(nBackSpeed);
                newScene.SetStageMedSpeed(nMedSpeed);
                newScene.SetFirstSceneFlag(sceneIndex == 0 ? true : false);
                XmlElement parElm = startElm.SelectSingleNode(strPart) as XmlElement;
                while (parElm != null)
                {
                    newScene.AddToDict(int.Parse(parElm.GetAttribute("ID")), parElm.GetAttribute(strName));
                    parElm = parElm.NextSibling as XmlElement;
                }
                m_sceneDict.Add(nStageID, newScene);
                startElm = startElm.NextSibling as XmlElement;
                ++sceneIndex;
            }
            PlugInMsgPipe(m_CurScene);
            return true;
        }
        //----------------------------------------------------------------------------
        public virtual void Destroy()
        {

        }
        //----------------------------------------------------------------------------
        public int CopyTempID
        {
            get
            {
                return m_nCopyTempID;
            }
            set
            {
                m_nCopyTempID = value;
            }
        }
        //----------------------------------------------------------------------------
        public int GroupTempID
        {
            get
            {
                return m_nGroupTempID;
            }
            set
            {
                m_nGroupTempID = value;
            }
        }
        //----------------------------------------------------------------------------
        public virtual void InitComponent()
        {

        }
        //----------------------------------------------------------------------------
    }
}
