//------------------------------------------------------------------------------
/**
	\file	DataSerialize.cs

	Copyright (c) 2019, BoYue. All rights reserved.

	<PRE>

	模块名称：<文件所属的模块名称>
	文件名称：DataSerialize.cs
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
//	DataSerialize.cs
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filterartifact
{
    public class DataSerialize
    {
        //----------------------------------------------------------------------------
        protected Stream m_fsFile;
        protected BinaryWriter m_bwData;
        protected BinaryReader m_brData;
        protected bool m_bHasSerializeData;
        //----------------------------------------------------------------------------
        public virtual bool Initialize()
        {
            byte[] byteTemp = FileSystem.Instance().GetSerializeByte();
            if (byteTemp != null)
            {
                m_fsFile = new MemoryStream(byteTemp);
                m_brData = new BinaryReader(m_fsFile);
                byteTemp = null;
                m_bHasSerializeData = true;
            }
            else
            {
                m_bHasSerializeData = false;
            }
            FileSystem.Instance().ClearSerializeByte();
            return true;
        }
        //----------------------------------------------------------------------------
        public virtual void Finalized()
        {

        }
        //----------------------------------------------------------------------------
        public virtual void SaveStream()
        {
            if (m_bwData != null)
            {
                m_bwData.Flush();
                m_bwData.Close();
                m_bwData = null;
            }
            if (m_brData != null)
            {
                m_brData.Close();
                m_brData = null;
            }
            if (m_fsFile != null)
            {
                m_fsFile.Close();
                m_fsFile = null;
            }
        }
        //----------------------------------------------------------------------------
        public bool HasData()
        {
            return m_bHasSerializeData;
        }
        //----------------------------------------------------------------------------
        public virtual BinaryWriter GetWriter()
        {
            return m_bwData;
        }
        //----------------------------------------------------------------------------
        public virtual BinaryReader GetReader()
        {
            return m_brData;
        }
        //----------------------------------------------------------------------------
        public virtual void FlushStream()
        {
            if (m_fsFile != null)
            {
                if (m_bwData!=null)
                {
                    m_bwData.Flush();
                }
                if (m_fsFile!=null)
                {
                    m_fsFile.Flush();
                }
            }
        }
        //----------------------------------------------------------------------------



    }
}
