﻿//------------------------------------------------------------------------------
/**
	\file	PackagerUI.cs

	Copyright (c) 2019, BoYue. All rights reserved.

	<PRE>

	模块名称：<文件所属的模块名称>
	文件名称：PackagerUI.cs
	摘    要：<描述该文件实现的主要功能>

	当前版本：1.0
	建立日期：2019/12/12
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
//	PackagerUI.cs
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
class PackagerUI
{
    //----------------------------------------------------------------------------
    class ConfigurationElement
    {
        public string path = string.Empty;
        public string name = string.Empty;
    }
    //----------------------------------------------------------------------------
    public static bool m_bIsProjectBundle = false;
    private static List<AssetBundleBuild> buildList = new List<AssetBundleBuild>();
    static List<string> m_missingFont = new List<string>();
    private static string m_ResourcePath = Application.dataPath + "/Data";
    private static Dictionary<string, List<string>> list_ResourseListXml_UIDeps = new Dictionary<string, List<string>>();
    static Object m_obj = null;
    private static string m_ExtName = ".unity3d";
    private static string m_assetPath = Application.streamingAssetsPath;
    //----------------------------------------------------------------------------
    public static void CreateAssetBundleAll(BuildTarget target)
    {
        if (Directory.Exists(m_assetPath))
        {
            FileUtils.DeleteDir(m_assetPath);
        }

        if (Directory.Exists(ExportPath))
        {
            FileUtils.DeleteDir(ExportPath);
        }

        m_bIsProjectBundle = true;
        PackageAssetBundle(null, target);

    }
    //----------------------------------------------------------------------------
    public static void CreateAssetBundleSelect(BuildTarget target)
    {
        m_bIsProjectBundle = false;
        string[] paths = GetSelectAssetPaths();
        PackageAssetBundle(paths, target);
    }
    //----------------------------------------------------------------------------
    public static void CreateAssetBundleSelectToOne(BuildTarget target)
    {
        CreateGameDataToOne(target);
    }
    //-------------------------------------------------------------------------
    static void CreateGameDataToOne(BuildTarget target)
    {
        string targetPath = GetExportPath("gamedata/");
        string bundleName = "gamedata.unity3d";

        AssetDatabase.Refresh();
        string sourcePath = Application.dataPath + "/Data/data/";
        if (!Directory.Exists(sourcePath))
        {
            Debug.Log("CreateGameDataToOne not exist " + sourcePath);
            return;
        }
        var options = BuildAssetBundleOptions.CompleteAssets | BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.DeterministicAssetBundle | BuildAssetBundleOptions.ChunkBasedCompression;

        targetPath = targetPath.ToLower();
        _CreateMultipleAssetBundleFolder(bundleName, sourcePath, options, target, targetPath);

        AssetDatabase.Refresh();
    }
    //----------------------------------------------------------------------------
    public static void _CreateMultipleAssetBundleFolder(string bundleName, string folder, BuildAssetBundleOptions options, BuildTarget target, string strTargetPath)
    {
        Debug.Log("Selected Folder: " + folder);
        if (folder.Length != 0)
        {
            folder = folder.Replace("\\", "/");
            string[] fileEntries = Directory.GetFiles(folder, "*.*", SearchOption.AllDirectories);
            List<string> sList = new List<string>();
            foreach (string fileName in fileEntries)
            {
                if (!fileName.Contains("meta") && !fileName.Contains("unity"))
                {
                    string fileRelativePath = fileName.Replace("\\", "/");
                    fileRelativePath = fileRelativePath.Substring((Application.dataPath.Replace("Assets", "")).Length); //Asset开头的相对路径
                    sList.Add(fileRelativePath);
                }
            }
            List<AssetBundleBuild> buildList = new List<AssetBundleBuild>();
            AssetBundleBuild bundle = new AssetBundleBuild();
            bundle.assetBundleName = bundleName;
            bundle.assetNames = sList.ToArray();
            buildList.Add(bundle);
            BuildPipeline.BuildAssetBundles(strTargetPath, buildList.ToArray(), options, target);
            Debug.Log("File Count: " + sList.Count);
        }
    }
    //----------------------------------------------------------------------------
    public static string GetExportPath(string strRelativePath = null)
    {
        string exportPath = Application.dataPath + "/StreamingAssets/";
        if (strRelativePath != null)
        {
            Debug.Log("RelativePath: " + strRelativePath);
            exportPath += strRelativePath.ToLower();
        }
        if (!Directory.Exists(exportPath))
        {
            Directory.CreateDirectory(exportPath);
            //exportPath = EditorUtility.SaveFolderPanel("", "", "");
        }
        //Debug.Log(exportPath);
        return exportPath;
    }
    //----------------------------------------------------------------------------
    public static string[] GetSelectAssetPaths()
    {
        List<string> pathList = new List<string>();
        Object[] SelectedObjs = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
        if (SelectedObjs.Length > 0)
        {
            for (int i = 0; i < SelectedObjs.Length; i++)
            {
                string pathTemp = AssetDatabase.GetAssetPath(SelectedObjs[i]);
                pathTemp = Application.dataPath.Replace("Assets", "") + pathTemp;
                pathList.Add(pathTemp);
            }
        }
        return pathList.ToArray();
    }
    //----------------------------------------------------------------------------
    public static void PackageAssetBundle(string[] paths, BuildTarget target)
    {
        Clear();
        if (paths == null) //全部打包
        {
            GetFileSystemInfos(m_ResourcePath, string.Empty);
        }
        else //只打当前选中资源包
        {
            foreach (var path in paths)
            {
                string strTemp = path.Substring((m_ResourcePath + "/").Length);
                if (Directory.Exists(path))
                {
                    GetFileSystemInfos(path, strTemp);
                }
                else
                {
                    if (File.Exists(path))
                    {
                        if (!path.EndsWith(".meta") && !path.EndsWith(".DS_Store") && !path.EndsWith("packager.xml"))
                        {
                            string fileRelativePath = path.Substring((Application.dataPath.Replace("Assets", "")).Length);//Asset开头的相对路径
                            string bundleName = strTemp.Substring(0, strTemp.LastIndexOf("."));

                            BuildGroup(bundleName, new string[] { fileRelativePath });
                            string fileName = strTemp.Substring(strTemp.LastIndexOf("/"));
                            GetUIDependecise(fileName, fileRelativePath);
                        }
                    }
                    else
                    {
                        Debug.LogError("无效路径");
                    }
                }
            }
        }

        CreateAssetBundle(target);

        if (paths == null)
        {
            if (Directory.Exists(m_assetPath))
            {
                FileUtils.DeleteDir(m_assetPath);
            }

            FileUtils.CopyFolder(ExportPath, m_assetPath, true);
        }

        WriteLabelMissingFile();
        if (m_bIsProjectBundle)
        {
            for (int i = 0; i < fileNameList.Count; i++)
            {
                GetUIDependecise(fileNameList[i], fileNameListPath[i]);
            }
            WriteToResourseList();
            list_ResourseListXml_UIDeps.Clear();
        }
        else if (!m_bIsProjectBundle && m_obj != null)
        {
            AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(m_obj));
            m_obj = null;
        }
        if (depsAb != null)
        {
            depsAb.Unload(false);
            depsAb = null;
        }
        if (manifest != null)
        {
            manifest = null;
        }

        Debug.LogError("打包完成");
    }
    //----------------------------------------------------------------------------
    static void WriteLabelMissingFile()
    {
        string fileTXT = ExportPath + "/" + "UILabel_Missing.txt";
        FileStream file = new FileStream(fileTXT, FileMode.Create);
        StreamWriter write = new StreamWriter(file);
        for (int i = 0; i < m_missingFont.Count; i++)
        {
            write.Write(string.Format("UILabelPath:{0}\n", m_missingFont[i]));
        }
        write.Flush();
        write.Close();
        file.Close();
        write.Dispose();
        file.Dispose();
        m_missingFont.Clear();
    }
    //----------------------------------------------------------------------------
    static void WriteToResourseList()
    {
        string xmlPath = m_assetPath + "/" + "Resourselist_2.xml";
        if (File.Exists(xmlPath))
        {
            File.Delete(xmlPath);
        }

        XmlDocument xmlDoc = new XmlDocument();
        XmlNode node = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", "");
        xmlDoc.AppendChild(node);
        XmlElement root = xmlDoc.CreateElement("Resourselist");
        //effect
        XmlElement xe1 = xmlDoc.CreateElement("uideps");

        foreach (var ele in list_ResourseListXml_UIDeps)
        {
            XmlElement xe2 = xmlDoc.CreateElement("Bundle");
            xe2.SetAttribute("id", ele.Key);
            xe1.AppendChild(xe2);
            List<string> list = ele.Value;
            for (int i = 0; i < list.Count; i++)
            {
                XmlElement xe3 = xmlDoc.CreateElement("Bundle");
                xe3.SetAttribute("id", list[i]);
                xe2.AppendChild(xe3);
            }
        }

        root.AppendChild(xe1);
        xmlDoc.AppendChild(root);
        xmlDoc.Save(xmlPath);
        SaveXmlWithUTF8NotBOM(xmlPath, xmlDoc);
        var newPath = Application.dataPath + "/Data/data/Common";
    }
    //----------------------------------------------------------------------------
    public static void SaveXmlWithUTF8NotBOM(string savePath, XmlDocument xml)
    {
        StreamWriter sw = new StreamWriter(savePath, false, new System.Text.UTF8Encoding(false));
        xml.Save(sw);
        sw.WriteLine();
        sw.Close();
        sw.Dispose();
    }
    //----------------------------------------------------------------------------
    public static void CreateAssetBundle(BuildTarget target)
    {
        BuildAssetBundleOptions options = BuildAssetBundleOptions.DeterministicAssetBundle | BuildAssetBundleOptions.ChunkBasedCompression | BuildAssetBundleOptions.DisableWriteTypeTree;
        BuildPipeline.BuildAssetBundles(ExportPath, buildList.ToArray(), options, target);
        AssetDatabase.Refresh();
    }
    static List<string> fileNameList = new List<string>();
    static List<string> fileNameListPath = new List<string>();
    //----------------------------------------------------------------------------
    static void GetFileSystemInfos(string path, string tempConfiguration)
    {
        DirectoryInfo folder = new DirectoryInfo(path);
        FileSystemInfo[] fileInfos = folder.GetFileSystemInfos();
        fileNameList.Clear();
        fileNameListPath.Clear();
        for (int i = 0; i < fileInfos.Length; i++)
        {
            SetAssetBundleGroup(fileInfos[i], tempConfiguration);
        }

    }
    static string atlasStr = "ui/atlas";
    //----------------------------------------------------------------------------
    static void SetAssetBundleGroup(FileSystemInfo systemInfo, string strTempPath)
    {
        string path = systemInfo.FullName.Replace("\\", "/");
        string fileName = systemInfo.Name;
        string relativePath = path.Substring((m_ResourcePath + "/").Length);
        if (path.EndsWith(".meta") || path.EndsWith(".DS_Store") || path.EndsWith(".xml") || path.EndsWith(".txt") || path.EndsWith(".json"))
        {
            return;
        }
        if (string.IsNullOrEmpty(strTempPath))
        {
            strTempPath = fileName;
        }
        else
        {
            strTempPath += string.Format("/{0}", fileName);
        }

        if (systemInfo is FileInfo) //是否为文件
        {
            strTempPath += strTempPath.Substring(0, strTempPath.LastIndexOf("."));
        }

        //不包含(1.文件夹，2.文件)
        if (systemInfo is FileInfo) //是否为文件
        {
            if (!path.EndsWith(".meta") && !path.EndsWith("DS_Store") && !path.EndsWith("packager.xml"))
            {
                string fileRelativePath = path.Substring(Application.dataPath.Replace("Assets", "").Length);//Asset开头的相对路径
                string bundleName = relativePath.Substring(0, relativePath.LastIndexOf("."));
                if (fileRelativePath.Contains(atlasStr))
                {
                    bundleName = bundleName.Substring(0, bundleName.LastIndexOf("/"));
                }
                BuildGroup(bundleName, new string[] { fileRelativePath });
                if (fileRelativePath.Contains("ui/prefab/") && !fileRelativePath.Contains("ui_empty_root"))
                {
                    fileNameList.Add(fileName);
                    char[] cha = { 'u', 'i' };
                    fileRelativePath = fileRelativePath.Substring(fileRelativePath.IndexOfAny(cha));//ui开头的相对路径
                    fileRelativePath = fileRelativePath.Substring(0, fileRelativePath.LastIndexOf(".")) + m_ExtName;
                    fileNameListPath.Add(fileRelativePath);
                }
            }
        }
        else//文件夹
        {
            string tempRelativePath = string.Empty;
            if (strTempPath.Contains("/"))
            {
                string tempPath = strTempPath.Substring(strTempPath.LastIndexOf("/") + 1);
                if (!strTempPath.Contains("{0}"))
                {
                    tempRelativePath = strTempPath.Replace(tempPath, "{0}");
                }
                else if (!strTempPath.Contains("{1}"))
                {
                    tempRelativePath = strTempPath.Replace(tempPath, "{1}");
                }
                else if (!strTempPath.Contains("{2}"))
                {
                    tempRelativePath = strTempPath.Replace(tempPath, "{2}");

                }
            }
            else
            {
                tempRelativePath = strTempPath;
            }
            GetFileSystemInfos(systemInfo.FullName, tempRelativePath);
        }
    }

    //----------------------------------------------------------------------------
    //设置打包参数
    public static void BuildGroup(string groupName, string[] files)
    {
        AssetBundleBuild build = new AssetBundleBuild();
        build.assetBundleName = groupName + m_ExtName;
        build.assetNames = files;
        buildList.Add(build);
    }
    //----------------------------------------------------------------------------
    static string depsPath = Application.streamingAssetsPath + "/uiasset";
    static AssetBundle depsAb = null;
    static AssetBundleManifest manifest = null;
    //----------------------------------------------------------------------------
    static void GetUIDependecise(string fileName, string fileRelativePath)
    {
        if (depsAb == null)
        {
            depsAb = AssetBundle.LoadFromFile(depsPath);
        }
        if (manifest == null)
        {
            manifest = depsAb.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        }
        string[] deps = manifest.GetAllDependencies(fileRelativePath);
        List<string> depsList = new List<string>();
        for (int i = 0; i < deps.Length; i++)
        {
            var str = deps[i].Substring(deps[i].LastIndexOf("/") + 1);
            var str1 = str.Substring(0, str.LastIndexOf("."));
            depsList.Add(str1);
        }
        if (list_ResourseListXml_UIDeps != null)
        {
            if (!list_ResourseListXml_UIDeps.ContainsKey(fileName))
            {
                list_ResourseListXml_UIDeps.Add(fileName, depsList);
            }
        }
    }
    //----------------------------------------------------------------------------
    static void AddXmlUIDeps(Image[] arrSpr, ref List<string> list)
    {
        if (arrSpr == null)
        {
            return;
        }
        if (list == null)
        {
            return;
        }
        int nCount = arrSpr.Length;
        Image temp = null;
        for (int i = 0; i < nCount; i++)
        {
            temp = arrSpr[i];
            if (temp.sprite == null)
            {
                continue;
            }
            string name = (temp.sprite as Object).name;
            if (list.Contains(name))
            {
                continue;
            }
            list.Add(name);
        }
    }
    //----------------------------------------------------------------------------
    public static void Clear()
    {
        ClearExportPath();
        Caching.ClearCache();
        buildList.Clear();
        m_missingFont.Clear();
    }
    //----------------------------------------------------------------------------
    /// <summary>
    /// 清空打包目录下的文件
    /// </summary>
    //----------------------------------------------------------------------------
    public static void ClearExportPath()
    {
        if (Directory.Exists(ExportPath))
        {
            DirectoryInfo folder = new DirectoryInfo(ExportPath);
            FileSystemInfo[] files = folder.GetFileSystemInfos();
            for (int i = 0; i < files.Length; i++)
            {
                if (files[i] is DirectoryInfo)
                {
                    Directory.Delete(files[i].FullName, true);
                }
                else
                {
                    File.Delete(files[i].FullName);
                }
            }
        }
        else
        {
            Directory.CreateDirectory(ExportPath);
        }
    }
    //----------------------------------------------------------------------------
    public static string ExportPath
    {
        get
        {
            string dataPath = Application.dataPath;
            if (dataPath.Contains("Client"))
            {
                dataPath = dataPath.Replace("Assets", "") + "uiasset";
            }
            return dataPath;
        }
    }
    //----------------------------------------------------------------------------
}

