//------------------------------------------------------------------------------
/**
	\file	UILoadingCtrl.cs

	Copyright (c) 2020, BoYue. All rights reserved.

	<PRE>

	模块名称�?文件所属的模块名称>
	文件名称：UILoadingCtrl.cs
	�?   要：<描述该文件实现的主要功能>

	当前版本�?.0
	建立日期�?020/3/3
	�?   者：SYSTEM
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
//	UILoadingCtrl.cs
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filterartifact
{
    class UILoadingCtrl : UIController
    {
        //----------------------------------------------------------------------------
        public bool m_bLoad = false;
        public int m_nCurFramFlag = 0;
        public int m_nCurLoadedCount = 0;
        public int m_nNeedLoadedCount = 0;
        //----------------------------------------------------------------------------
        public override void InitCtrl()
        {
            base.InitCtrl();
            Messenger.AddListener<bool>(DgMsgID.DgMsg_LSNtyStageLoadingProcess, OnLoadProcess);
            Messenger.AddListener<bool>(DgMsgID.DgMsg_NtyStageLoadingProcess, OnLoadProcess);
        }
        //----------------------------------------------------------------------------
        public override void Finalized()
        {
            base.Finalized();
            Messenger.RemoveListener<bool>(DgMsgID.DgMsg_LSNtyStageLoadingProcess, OnLoadProcess);
            Messenger.RemoveListener<bool>(DgMsgID.DgMsg_NtyStageLoadingProcess, OnLoadProcess);
        }
        //----------------------------------------------------------------------------
        private void OnLoadProcess(bool bLoad)
        {
            if (bLoad)
            {
                m_bLoad = bLoad;
                m_nCurFramFlag = 0;
                m_nNeedLoadedCount = 0;

            }
            else
            {
                m_nCurLoadedCount = m_nNeedLoadedCount;
            }
        }
        //----------------------------------------------------------------------------
        public override void Update()
        {
            base.Update();

        }
        //----------------------------------------------------------------------------
        public void OnShowOrHide(object obj)
        {
            bool bShowOrHide = (bool)obj;
            if (bShowOrHide)
            {
            
            }
            else
            {
                Hide();
            }
            SetDoEventReturnFlag(false);
        }
        //----------------------------------------------------------------------------
    }
}
