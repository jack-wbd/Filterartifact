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

using System.IO;
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
        static public void Write(BinaryWriter writer, string v)
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
        public static string GetDataPath(string childPath)
        {
            string strStreamAssetsDir = "";
            string strPerisistentDir = "";
            switch (Application.platform)
            {
                case RuntimePlatform.IPhonePlayer:
                    strStreamAssetsDir = Application.streamingAssetsPath + "/";
                    strPerisistentDir = Application.persistentDataPath + "/StreamingAssets/";
                    break;
                case RuntimePlatform.Android:
                    strStreamAssetsDir = Application.streamingAssetsPath + "/";
                    strPerisistentDir = Application.persistentDataPath + "/";
                    break;
                case RuntimePlatform.WebGLPlayer:
                    strStreamAssetsDir = "StreamingAssets/";
                    break;
                default:
                    strStreamAssetsDir = Application.streamingAssetsPath + "/";
                    strPerisistentDir = Application.persistentDataPath + "/StreamingAssets/";
                    break;
            }

            string perisistentPath = strPerisistentDir + childPath;
            if (File.Exists(perisistentPath))
            {
                return perisistentPath;
            }
            else
            {
                string streamPath = strStreamAssetsDir + childPath;
                return streamPath;
            }

        }
    }
}
