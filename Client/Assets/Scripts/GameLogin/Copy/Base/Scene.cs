//------------------------------------------------------------------------------
/**
	\file	Scene.cs

	Copyright (c) 2020, BoYue. All rights reserved.

	<PRE>

	模块名称�?文件所属的模块名称>
	文件名称：Scene.cs
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
//	Scene.cs
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Filterartifact
{
    public class Scene : IBase
    {
        //function
        protected eSceneType m_type;
        protected int m_nSceneID;
        protected float m_BackGroundSpeed = 0f;
        protected float m_MdeGroundSpeed = 0f;
        protected bool firstSceneFlag = false;
        protected Dictionary<int, string> m_PartDict = null;
        protected Copy m_CopyStage = null;
        //----------------------------------------------------------------------------
        public virtual bool Init()
        {
            return true;
        }
        //----------------------------------------------------------------------------
        public eSceneType GetSceneType()
        {
            return m_type;
        }
        //----------------------------------------------------------------------------
        public Scene(Copy scene)
        {

        }
        //----------------------------------------------------------------------------
        public void SetType(eSceneType type)
        {
            m_type = type;
        }
        //----------------------------------------------------------------------------
        public void SetStageID(int nStageID)
        {
            m_nSceneID = nStageID;
        }
        //----------------------------------------------------------------------------
        public void SetStageBackSpeed(float speed)
        {
            m_BackGroundSpeed = speed;
        }
        //----------------------------------------------------------------------------
        public void SetStageMedSpeed(float speed)
        {
            m_MdeGroundSpeed = speed;
        }
        //----------------------------------------------------------------------------
        public void SetFirstSceneFlag(bool firstSceneFlag)
        {
            this.firstSceneFlag = firstSceneFlag;
        }
        //----------------------------------------------------------------------------
        public void AddToDict(int nPartID,string strPartName)
        {
            if (m_PartDict.ContainsKey(nPartID))
            {
                Debug.LogError("stage: AddToDict error:same key");
                return;
            }
            m_PartDict.Add(nPartID, strPartName);
        }
        //----------------------------------------------------------------------------
        public void ParseSceneData()
        {
            string strStageFileName = "Map/";
            strStageFileName += m_CopyStage.GetCopyName();
            strStageFileName += "/";  
        }
        //----------------------------------------------------------------------------

        //----------------------------------------------------------------------------
        //----------------------------------------------------------------------------
    }
}
