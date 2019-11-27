//------------------------------------------------------------------------------
/**
	\file	AssetsManager.cs

	Copyright (c) 2019, BoYue. All rights reserved.

	<PRE>

	模块名称：<文件所属的模块名称>
	文件名称：AssetsManager.cs
	摘    要：<描述该文件实现的主要功能>

	当前版本：1.0
	建立日期：2019/11/22
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
//	AssetsManager.cs
//------------------------------------------------------------------------------
using System.Collections.Generic;
using UnityEngine;

public class AssetsManager : Manager
{
    //----------------------------------------------------------------------------
    private static AssetsManager m_Singlton;
    //----------------------------------------------------------------------------
    private static AssetsManager CreateInstance()
    {
        if (m_Singlton != null)
        {
            m_Singlton = null;
        }
        m_Singlton = new AssetsManager();
        return m_Singlton;
    }
    //----------------------------------------------------------------------------
    public static AssetsManager Instance()
    {
        return m_Singlton;
    }
    //----------------------------------------------------------------------------
    private static void Release()
    {
        if (m_Singlton != null)
        {
            m_Singlton = null;
        }
    }
    //----------------------------------------------------------------------------
    private struct AssetCube
    {
        public object objAsset;

        public static AssetCube Clear
        {
            get
            {
                AssetCube cube;
                cube.objAsset = null;
                return cube;
            }
        }
    }
    //----------------------------------------------------------------------------
    private Dictionary<string, AssetCube> m_dictAsset = new Dictionary<string, AssetCube>();//当前所有的资源

    public object GetAssetObjByID(string strAssetID)
    {
        if (string.IsNullOrEmpty(strAssetID))
        {
            Debug.LogWarning("the assetID is null");
            return null;
        }
        AssetCube cube;

        if (m_dictAsset.TryGetValue(strAssetID, out cube))
        {
            return cube.objAsset;
        }
        return null;    
    }
    //----------------------------------------------------------------------------
    public bool LoadAssetRes<T, U>(string strAssetID, Callback<T, U> call)
    {

        if (call == null)
            return false;

        if (string.IsNullOrEmpty(strAssetID))
            return false;

        AssetCube cube;
        if (m_dictAsset.TryGetValue(strAssetID, out cube))
        {
            Callback<string, object> callBack = call as Callback<string, object>;
            callBack(strAssetID, cube.objAsset);
            return true;
        }
        return FileSystem.Instance().StartLoad(strAssetID, call);

    }

}

