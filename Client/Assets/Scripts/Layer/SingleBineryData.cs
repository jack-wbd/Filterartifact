//------------------------------------------------------------------------------
/**
	\file	SingleBineryData.cs

	Copyright (c) 2019, BoYue. All rights reserved.

	<PRE>

	模块名称：<文件所属的模块名称>
	文件名称：SingleBineryData.cs
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
//	SingleBineryData.cs
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filterartifact
{
    public class SingleBineryData : DataBase
    {
        //----------------------------------------------------------------------------
        protected string m_streamName = "template.byte";
        protected SingleDataSerialize m_fileSer;
        //----------------------------------------------------------------------------
        public void CreateBinery()
        {
            if (m_fileSer == null)
            {
                m_fileSer = new SingleDataSerialize(m_streamName);
                m_fileSer.CallByEditor(m_streamName);
            }
        }
        //----------------------------------------------------------------------------
        public bool LoadBinery(bool doInit = true)
        {
            m_fileSer = new SingleDataSerialize(m_streamName);
            m_fileSer.Initialize();
            if (doInit)
            {
                Initialize();
            }
            return HasStreamData();
        }
        //----------------------------------------------------------------------------
        public bool HasStreamData()
        {
            if (m_fileSer != null)
            {
                return m_fileSer.HasData();
            }
            return false;
        }
        //----------------------------------------------------------------------------

        //----------------------------------------------------------------------------
    }
}
