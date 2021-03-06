﻿//------------------------------------------------------------------------------
/**
	\file	ExportAssetBundle.cs

	Copyright (c) 2019, BoYue. All rights reserved.

	<PRE>

	模块名称：<文件所属的模块名称>
	文件名称：ExportAssetBundle.cs
	摘    要：<描述该文件实现的主要功能>

	当前版本：1.0
	建立日期：2019/12/11
	作    者：SYSTEM
	电子邮件：SYSTEM@BoYue.com
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
//	ExportAssetBundle.cs
//------------------------------------------------------------------------------
using UnityEditor;

public class ExportAssetBundle
{
    public static bool m_bIsProjectBundle = false;
    //----------------------------------------------------------------------------
    public static void CreateAssetBundleAll_Win64()
    {
        PackagerUI.CreateAssetBundleAll(BuildTarget.StandaloneWindows64);
    }
    //----------------------------------------------------------------------------
    public static void CreateAssetBundleAll_Android()
    {
        PackagerUI.CreateAssetBundleAll(BuildTarget.Android);
    }
    //----------------------------------------------------------------------------
    public static void CreateAssetBundleSelect_Win64()
    {
        PackagerUI.CreateAssetBundleSelect(BuildTarget.StandaloneWindows64);
    }
    //----------------------------------------------------------------------------
    public static void CreateAssetBundleSelectToOne_Android()
    {
        PackagerUI.CreateAssetBundleSelectToOne(BuildTarget.Android);
    }
    //----------------------------------------------------------------------------
    public static void CreateAssetBundleSelectToOne_Windows64()
    {
        PackagerUI.CreateAssetBundleSelectToOne(BuildTarget.StandaloneWindows64);
    }
    //----------------------------------------------------------------------------
    public static void CreateAssetBundleAll_Win32()
    {
        PackagerUI.CreateAssetBundleAll(BuildTarget.StandaloneWindows);
    }
    //----------------------------------------------------------------------------
    public static void CreateAssetBundleSelect_Win32()
    {
        PackagerUI.CreateAssetBundleSelect(BuildTarget.StandaloneWindows);
    }
    //----------------------------------------------------------------------------
}

