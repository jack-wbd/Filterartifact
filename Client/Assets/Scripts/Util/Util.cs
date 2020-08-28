//------------------------------------------------------------------------------
/**
	\file	Util.cs

	Copyright (c) 2019, BoYue. All rights reserved.

	<PRE>

	模块名称：<文件所属的模块名称>
	文件名称：Util.cs
	摘    要：<描述该文件实现的主要功能>

	当前版本：1.0
	建立日期：2019/8/19
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
//	Util.cs
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class Util
{
    // 设置控件是否可见
    //----------------------------------------------------------------------------
    public static void SetActive(GameObject obj_, bool bActive)
    {
        if (null != obj_ && obj_.activeSelf != bActive)
        {
            obj_.SetActive(bActive);
        }
    }
    //----------------------------------------------------------------------------
    public static void SafeDestroy<T>(T com) where T : Component
    {
        if (null != com)
        {
            UnityEngine.Object.Destroy(com);
            com = null;
        }
    }
    //----------------------------------------------------------------------------
    public static void Reset(this Transform value)
    {
        value.localPosition = Vector3.zero;
        value.localRotation = Quaternion.identity;
        value.localScale = Vector3.one;
    }
    //----------------------------------------------------------------------------
    public static void Visible(this Component component, bool visible, bool showError = true)
    {
        if (component == null && showError)
        {
            Debug.LogError("component == null");
            return;
        }

        if (component != null)
        {
            var go = component.gameObject;
            if (go.activeSelf != visible)
            {
                go.SetActive(visible);
            }
        }
    }
    //----------------------------------------------------------------------------
    public static GameObject Clone(this GameObject go, Transform parent, string name = "")
    {
        if (!go)
            return null;

        GameObject cloneGo = UnityEngine.Object.Instantiate(go, parent);
        cloneGo.transform.localPosition = Vector3.zero;
        cloneGo.transform.localScale = Vector3.one;
        cloneGo.transform.localRotation = Quaternion.identity;

        if (!string.IsNullOrEmpty(name))
        {
            cloneGo.name = name;
        }

        return cloneGo;
    }
    //----------------------------------------------------------------------------
    public static void Visible(this GameObject go, bool visible, bool showError = true)
    {
        if (go == null && showError)
        {
            Debug.LogError("go ==null");
            return;
        }
        if (go != null && go.activeSelf != visible)
        {
            go.SetActive(visible);
        }
    }
    //----------------------------------------------------------------------------
    public static GameObject FindChildGO(this GameObject parent, string path, bool showError = true)
    {
        if (!parent)
        {
            if (showError)
                Debug.LogError(path + " parent is null!");

            return null;
        }

        return parent.transform.FindChildGO(path, showError);
    }
    //----------------------------------------------------------------------------
    public static GameObject FindChildGO(this Transform parent, string path, bool showError = true)
    {
        if (!parent)
        {
            if (showError)
                Debug.LogError(path + " parent is null!");

            return null;
        }

        var tf = parent.Find(path);

        if (!tf)
        {
            if (showError)
                Debug.LogError(path + " is null!");
            return null;
        }

        return tf.gameObject;
    }
    //----------------------------------------------------------------------------
    public static T GetUnityComponent<T>(this Transform tran, string path) where T : Component
    {
        var t = tran.FindChildGO(path).GetComponent<T>();
        if (t == null)
        {
            Debug.LogError("this" + path + "is not component");
        }
        return t;
    }
    //----------------------------------------------------------------------------
    /// <summary>
    /// 求数组中n个元素的排列
    /// </summary>
    /// <param name="t">所求数组</param>
    /// <param name="n">元素个数</param>
    /// <returns>数组中n个元素的排列</returns>

    //----------------------------------------------------------------------------
    public static List<List<byte>> GetCombination(List<byte> t, int n)
    {
        if (t.Count < n)
        {
            return null;
        }
        byte[] temp = new byte[n];
        List<List<byte>> list = new List<List<byte>>();
        GetCombination(ref list, t, t.Count, n, temp, n);
        return list;
    }
    //----------------------------------------------------------------------------
    /// <summary>
    /// 递归算法求数组的组合(私有成员)
    /// </summary>
    /// <param name="list">返回的范型</param>
    /// <param name="t">所求数组</param>
    /// <param name="n">辅助变量</param>
    /// <param name="m">辅助变量</param>
    /// <param name="b">辅助数组</param>
    /// <param name="M">辅助变量M</param>
    private static void GetCombination(ref List<List<byte>> list, List<byte> t, int n, int m, byte[] b, int M)
    {
        if (M <= 0)
        {
            return;
        }
        for (int i = n; i >= m; i--)
        {
            b[m - 1] = Convert.ToByte(i - 1);
            if (m > 1)
            {
                GetCombination(ref list, t, i - 1, m - 1, b, M);
            }
            else
            {
                if (list == null)
                {
                    list = new List<List<byte>>();
                }
                List<byte> temp = new List<byte>(M);
                for (int j = 0; j < b.Length; j++)
                {
                    temp.Add(t[Convert.ToInt16(b[j])]);
                }
                list.Add(temp);
            }
        }
    }
    //----------------------------------------------------------------------------
    /// <summary>
    /// 排列循环方法
    /// </summary>
    /// <param name="N"></param>
    /// <param name="R"></param>
    /// <returns></returns>
    private static long P1(int N, int R)
    {
        if (R > N || R <= 0 || N <= 0) throw new ArgumentException("params invalid!");
        long t = 1;
        int i = N;

        while (i != N - R)
        {
            try
            {
                checked
                {
                    t *= i;
                }
            }
            catch
            {
                throw new OverflowException("overflow happens!");
            }
            --i;
        }
        return t;
    }
    //----------------------------------------------------------------------------
    /// <summary>
    /// 排列堆栈方法
    /// </summary>
    /// <param name="N"></param>
    /// <param name="R"></param>
    /// <returns></returns>
    private static long P2(int N, int R)
    {
        if (R > N || R <= 0 || N <= 0) throw new ArgumentException("arguments invalid!");
        Stack<int> s = new Stack<int>();
        long iRlt = 1;
        int t;
        s.Push(N);
        while ((t = s.Peek()) != N - R)
        {
            try
            {
                checked
                {
                    iRlt *= t;
                }
            }
            catch
            {
                throw new OverflowException("overflow happens!");
            }
            s.Pop();
            s.Push(t - 1);
        }
        return iRlt;
    }
    //----------------------------------------------------------------------------
    /// <summary>
    /// 组合
    /// </summary>
    /// <param name="N"></param>
    /// <param name="R"></param>
    /// <returns></returns>
    public static long C(int N, int R)
    {
        return P1(N, R) / P1(R, R);
    }
    //----------------------------------------------------------------------------
    public static List<List<byte>> Combine(List<byte> a, int m)
    {
        int n = a.Count;
        if (m > n)
        {
            throw new Exception("错误！数组a中只有" + n + "个元素。" + m + "大于" + 2 + "!!!");
        }

        List<List<byte>> result = new List<List<byte>>();

        byte[] bs = new byte[n];
        for (int i = 0; i < n; i++)
        {
            bs[i] = 0;
        }
        //初始化
        for (int i = 0; i < m; i++)
        {
            bs[i] = 1;
        }
        bool flag = true;
        bool tempFlag = false;
        int pos = 0;
        int sum = 0;
        //首先找到第一个10组合，然后变成01，同时将左边所有的1移动到数组的最左边
        do
        {
            sum = 0;
            pos = 0;
            tempFlag = true;
            result.Add(Print(bs, a, m));

            for (int i = 0; i < n - 1; i++)
            {
                if (bs[i] == 1 && bs[i + 1] == 0)
                {
                    bs[i] = 0;
                    bs[i + 1] = 1;
                    pos = i;
                    break;
                }
            }
            //将左边的1全部移动到数组的最左边

            for (int i = 0; i < pos; i++)
            {
                if (bs[i] == 1)
                {
                    sum++;
                }
            }
            for (int i = 0; i < pos; i++)
            {
                if (i < sum)
                {
                    bs[i] = 1;
                }
                else
                {
                    bs[i] = 0;
                }
            }

            //检查是否所有的1都移动到了最右边
            for (int i = n - m; i < n; i++)
            {
                if (bs[i] == 0)
                {
                    tempFlag = false;
                    break;
                }
            }
            if (tempFlag == false)
            {
                flag = true;
            }
            else
            {
                flag = false;
            }

        } while (flag);

        result.Add(Print(bs, a, m));

        Debug.Log("result Count: " + result.Count);

        return result;
    }
    //----------------------------------------------------------------------------
    private static List<byte> Print(byte[] bs, List<byte> a, int m)
    {
        List<byte> result = new List<byte>();
        for (int i = 0; i < bs.Length; i++)
        {
            if (bs[i] == 1)
            {
                result.Add(a[i]);
            }
        }
        return result;
    }
    //----------------------------------------------------------------------------
    public static List<List<byte>> GetResult(List<byte> numberDataList, List<List<byte>> redBallSelResult, List<int> selectNumList)
    {
        var result = new List<List<byte>>();
        var curSelectNumList = new List<byte>();
        for (int i = 0; i < numberDataList.Count; i++)
        {
            for (int j = 0; j < redBallSelResult.Count; j++)
            {
                var selList = redBallSelResult[j];
                for (int k = 0; k < selList.Count; k++)
                {
                    if (selList.Contains(numberDataList[i]))
                    {
                        var data = Convert.ToByte(numberDataList[i]);
                        if (!curSelectNumList.Contains(data))
                        {
                            curSelectNumList.Add(data);
                        }

                    }
                }
            }
        }

        if (curSelectNumList.Count == 0)
        {
            return result;
        }

        var dict = GetSelectPopularNumResult(curSelectNumList, redBallSelResult);

        for (int i = 0; i < selectNumList.Count; i++)
        {
            var num = selectNumList[i];
            if (dict.ContainsKey(num))
            {
                var curList = dict[num];
                for (int j = 0; j < curList.Count; j++)
                {
                    result.Add(curList[j]);
                }
            }
        }

        return result;
    }
    //----------------------------------------------------------------------------
    private static Dictionary<int, List<List<byte>>> GetSelectPopularNumResult(List<byte> curSelectNumList, List<List<byte>> redBallResult)
    {
        var dict = new Dictionary<int, List<List<byte>>>();
        var num = 0;
        for (int i = 0; i < redBallResult.Count; i++)
        {
            var list = new List<List<byte>>();
            var result1 = redBallResult[i];
            for (int j = 0; j < result1.Count; j++)
            {
                for (int k = 0; k < curSelectNumList.Count; k++)
                {
                    if (result1[j] == curSelectNumList[k])
                    {
                        num++;
                    }
                }
            }

            list.Add(result1);

            if (dict.ContainsKey(num))
            {
                var curList = dict[num];
                for (int j = 0; j < curList.Count; j++)
                {
                    list.Add(curList[j]);
                }

            }

            dict[num] = list;
            num = 0;
        }
        return dict;
    }
    //----------------------------------------------------------------------------
}

public class FileData
{
    public string name;
    public string md5;
    public long size;
}

public class FileUtils
{
    //----------------------------------------------------------------------------
    public static FileData GetFileInfo(string filePath, string basePath)
    {
        FileData fileData = null;
        filePath = filePath.Replace("\\", "/");
        if (File.Exists(filePath))
        {
            FileStream file = new FileStream(filePath, FileMode.Open);
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] reVal = md5.ComputeHash(file);
            string md5_str = BitConverter.ToString(reVal);
            fileData = new FileData();
            fileData.name = filePath.Substring(basePath.Length);
            fileData.md5 = md5_str;
            fileData.size = file.Length;
            file.Close();
            return fileData;
        }
        return fileData;
    }
    //----------------------------------------------------------------------------
    public static void CleanFloder(string path)
    {
        if (!Directory.Exists(path))
            return;

        DirectoryInfo floderInfo = new DirectoryInfo(path);

        floderInfo.Delete(true);

        Directory.CreateDirectory(path);
    }
    //----------------------------------------------------------------------------
    public static void CopyFile(string src, string dest, bool overwrite = true)
    {
        if (File.Exists(src))
        {
            if (!Path.HasExtension(dest))
                return;
            string destDir = dest.Substring(0, dest.LastIndexOf("/")); ;
            if (!Directory.Exists(destDir))
                Directory.CreateDirectory(destDir);
            File.Copy(src, dest, overwrite);
        }
    }
    //----------------------------------------------------------------------------
    public static void MoveFile(string src, string dest)
    {
        if (File.Exists(src))
        {
            if (!Path.HasExtension(dest))
                return;
            string destDir = dest.Substring(0, dest.LastIndexOf("/")); ;
            if (!Directory.Exists(destDir))
                Directory.CreateDirectory(destDir);
            File.Move(src, dest);
        }
    }
    //----------------------------------------------------------------------------
    public static void CopyFolder(string sourcePath, string destPath, bool overwrite = true, string pattern = null)
    {
        if (Directory.Exists(sourcePath))
        {
            DirectoryInfo srcDirInfo = new DirectoryInfo(sourcePath);
            srcDirInfo.Attributes = FileAttributes.Normal & FileAttributes.Directory;
            if (!Directory.Exists(destPath))
            {
                try
                {
                    Directory.CreateDirectory(destPath);
                }
                catch (Exception e)
                {
                    throw new Exception("创建目录失败:" + e.Message);
                }
            }
            else
            {
                DirectoryInfo destDir = new DirectoryInfo(destPath);
                destDir.Attributes = FileAttributes.Normal & FileAttributes.Directory;
            }
            List<string> files;
            if (pattern != null)
                files = new List<string>(Directory.GetFiles(sourcePath, pattern));
            else
                files = new List<string>(Directory.GetFiles(sourcePath));
            foreach (var file in files)
            {
                File.SetAttributes(file, FileAttributes.Normal);

                string destFile = Path.Combine(destPath, Path.GetFileName(file));
                if (File.Exists(destFile))
                {
                    File.SetAttributes(destFile, FileAttributes.Normal);
                }
                File.Copy(file, destFile, overwrite);
            }

            List<string> folders = new List<string>(Directory.GetDirectories(sourcePath));
            foreach (var folder in folders)
            {
                if (folder.Contains(".svn"))
                    continue;
                string destDir = Path.Combine(destPath, Path.GetFileName(folder));
                DirectoryInfo dirInfo = new DirectoryInfo(folder);
                dirInfo.Attributes = FileAttributes.Normal & FileAttributes.Directory;
                DirectoryInfo destDirInfo = new DirectoryInfo(destDir);
                if (destDirInfo.Exists)
                    destDirInfo.Attributes = FileAttributes.Normal & FileAttributes.Directory;
                CopyFolder(folder, destDir, overwrite, pattern);
            }
        }
    }
    //----------------------------------------------------------------------------
    public static void DeleteDir(string strPath)
    {
        if (Directory.Exists(strPath))
        {
            string[] fileEntries = Directory.GetFiles(strPath, "*.*", SearchOption.AllDirectories);
            foreach (string strFile in fileEntries)
            {
                DeleteFile(strFile);
            }
            Directory.Delete(strPath, true);
        }
    }
    //----------------------------------------------------------------------------
    public static void DeleteFile(string strPath)
    {
        if (File.Exists(strPath))
        {
            if ((File.GetAttributes(strPath) & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                File.SetAttributes(strPath, FileAttributes.Normal);
            File.Delete(strPath);
        }

    }
    //----------------------------------------------------------------------------
}
