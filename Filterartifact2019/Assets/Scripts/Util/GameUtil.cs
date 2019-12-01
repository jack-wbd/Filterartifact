//------------------------------------------------------------------------------
/**
	\file	GameUtil.cs

	Copyright (c) 2019, BoYue. All rights reserved.

	<PRE>

	模块名称：<文件所属的模块名称>
	文件名称：GameUtil.cs
	摘    要：<描述该文件实现的主要功能>

	当前版本：1.0
	建立日期：2019/11/29
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
//	GameUtil.cs
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
    public class GameUtil
    {
        //----------------------------------------------------------------------------
        public static string GetAssetName(AssetBundle ab, string path)
        {
            if (ab != null)
            {
                string[] names = ab.GetAllAssetNames();
                foreach (var v in names)
                {
                    if (v.Contains(path.ToLower()))
                    {
                        return v;
                    }
                }
            }
            return null;
        }
        //----------------------------------------------------------------------------
        //Write a value to the stream
        static public void Write(BinaryWriter writer,string v)
        {
            if (string.IsNullOrEmpty(v))
            {
                writer.Write("n");
            }
            else
            {
                writer.Write(v);
            }
        }
        //----------------------------------------------------------------------------
        //Read a value from the stream
        static public string ReadString(BinaryReader reader)
        {
            string strValue = reader.ReadString();
            if (strValue.Equals("n"))
            {
                return null;
            }
            return strValue;
        }
        //----------------------------------------------------------------------------
        //----------------------------------------------------------------------------
        //----------------------------------------------------------------------------
        //----------------------------------------------------------------------------
    }
}
