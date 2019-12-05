//------------------------------------------------------------------------------
/**
	\file	SingleDataSerialize.cs

	Copyright (c) 2019, BoYue. All rights reserved.

	<PRE>

	模块名称：<文件所属的模块名称>
	文件名称：SingleDataSerialize.cs
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
//	SingleDataSerialize.cs
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Filterartifact
{
    //----------------------------------------------------------------------------
    public class SingleDataSerialize : DataSerialize
    {
        //----------------------------------------------------------------------------
        private string m_binaryFileName;
        //----------------------------------------------------------------------------
        public SingleDataSerialize(string binaryFileName)
        {
            m_binaryFileName = binaryFileName;
        }
        //----------------------------------------------------------------------------
        public override bool Initialize()
        {
            string binary = m_binaryFileName.Substring(0, m_binaryFileName.LastIndexOf("."));
            string strBineryFilePath = GameUtil.GetDataPath(string.Format("gamedata/{0}.unity3d", binary));
            if (!Application.isMobilePlatform)
            {
                if (File.Exists(strBineryFilePath))
                {
                    m_bHasSerializeData = false;
                    return true;
                }
            }
            AssetBundle bundle = AssetBundle.LoadFromFile(strBineryFilePath);
            if (bundle == null)
            {
                m_bHasSerializeData = false;
                return true;
            }
            string binaryName = Path.GetFileName(m_binaryFileName);
            TextAsset asset = bundle.LoadAsset<TextAsset>(binaryName);
            bundle.Unload(false);
            byte[] byteTemp = null;
            if (asset != null)
            {
                byteTemp = asset.bytes;
            }
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

            UnityEngine.Object.DestroyImmediate(asset, true);
            return true;
        }
        //----------------------------------------------------------------------------
        public virtual void CallByEditor(string bineryFileName)
        {

            string strBineryFilePath = Application.dataPath + "/ToExport/gamedata/" + bineryFileName;

            string strBineryFileDir = Path.GetDirectoryName(strBineryFilePath);

            if (!Directory.Exists(strBineryFileDir))
            {
                Directory.CreateDirectory(strBineryFileDir);
            }

            if (File.Exists(strBineryFilePath))
            {
                File.Delete(strBineryFilePath);
            }
            m_bHasSerializeData = false;
            m_fsFile = new FileStream(strBineryFilePath, FileMode.CreateNew, FileAccess.ReadWrite);
            m_bwData = new BinaryWriter(m_fsFile);

        }
        //----------------------------------------------------------------------------
        //----------------------------------------------------------------------------
        //----------------------------------------------------------------------------
        //----------------------------------------------------------------------------
    }
}
