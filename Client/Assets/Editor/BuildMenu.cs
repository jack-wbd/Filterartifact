﻿//------------------------------------------------------------------------------
/**
	\file	BuildMenu.cs

	Copyright (c) 2019, BoYue. All rights reserved.

	<PRE>

	模块名称：<文件所属的模块名称>
	文件名称：BuildMenu.cs
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
//	BuildMenu.cs
//------------------------------------------------------------------------------
using UnityEditor;
public class BuildMenu : EditorWindow
{
    /// <summary>
    /// LZ4重新压缩。使用资源的时候不需要整体解压。在下载的时候可以使用LZMA算法，一旦它被下载了之后，它会使用LZ4算法保存到本地上
    /// 使用LZ4压缩，压缩率没有LZMA高，但是我们可以加载指定资源而不用解压全部 
    /// 注意使用LZ4压缩，可以获得可以跟不压缩想媲美的加载速度，而且比不压缩文件要小.
    /// </summary>
    //----------------------------------------------------------------------------
    [MenuItem("Assets/Export/全部打包/Windows64", false, 1000)]
    [MenuItem("AssetBundle/全部打包/Windows64", false, 1000)]
    public static void BuildAllAssetBundleForWindows64()
    {
        ExportAssetBundle.CreateAssetBundleAll_Win64();
    }
    //----------------------------------------------------------------------------
    [MenuItem("Assets/Export/单独打包/Windows64", false, 1000)]
    [MenuItem("AssetBundle/单独打包/Windows64", false, 1000)]
    public static void BuildSelectAssetBundleForWindows64()
    {
        ExportAssetBundle.CreateAssetBundleSelect_Win64();
    }
    //----------------------------------------------------------------------------
    [MenuItem("Assets/Export/全部打包/Windows32", false, 1001)]
    [MenuItem("AssetBundle/全部打包/Windows32", false, 1000)]
    public static void BuildAllAssetBundleForWindows32()
    {
        ExportAssetBundle.CreateAssetBundleAll_Win32();
    }
    //----------------------------------------------------------------------------
    [MenuItem("Assets/Export/AssetBundle/选中（多合一）/Android", false, 1000)]
    [MenuItem("Export/AssetBundle/选中（多合一）/Android", false, 1000)]
    public static void Build_AssetBundle_Multiple_Android()
    {
        ExportAssetBundle.CreateAssetBundleSelectToOne_Android();
    }
    //----------------------------------------------------------------------------
    [MenuItem("Assets/Export/AssetBundle/选中（多合一）/Windows64", false, 1000)]
    [MenuItem("Export/AssetBundle/选中（多合一）/Windows64", false, 1000)]
    public static void Build_AssetBundle_Multiple_Windows64()
    {
        ExportAssetBundle.CreateAssetBundleSelectToOne_Windows64();
    }
    //----------------------------------------------------------------------------
    [MenuItem("Assets/Export/单独打包/Windows32", false, 1000)]
    [MenuItem("AssetBundle/单独打包/Windows32", false, 1000)]
    public static void BuildSelectAssetBundleForWindows32()
    {
        ExportAssetBundle.CreateAssetBundleSelect_Win32();
    }
    //----------------------------------------------------------------------------
    [MenuItem("Assets/Export/全部打包/Android", false, 1000)]
    [MenuItem("AssetBundle/全部打包/Android", false, 1000)]
    public static void BuildAllAssetBundleForAndroid()
    {
        ExportAssetBundle.CreateAssetBundleAll_Android();
    }
    //----------------------------------------------------------------------------
}
