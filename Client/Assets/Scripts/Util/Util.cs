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
using Filterartifact;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
    public static List<List<byte>> GetResult(List<byte> numberList, List<List<byte>> redBallSelResult, List<int> selectNumList)
    {
        var result = new List<List<byte>>();
        for (int i = 0; i < redBallSelResult.Count; i++)
        {
            var list = redBallSelResult[i];
            var num = 0;
            for (int j = 0; j < list.Count; j++)
            {
                for (int k = 0; k < numberList.Count; k++)
                {
                    if (list[j] == numberList[k])
                    {
                        num++;
                    }
                }
            }

            for (int k = 0; k < selectNumList.Count; k++)
            {
                if (num == selectNumList[k])
                {
                    result.Add(list);
                }
            }
        }
        return result;
    }
    /// <summary>
    /// 区间号码过滤结果
    /// </summary>
    /// <returns></returns>
    public static List<List<byte>> GetResult(List<List<byte>> redBallSelResult, List<string> selectType)
    {
        var result = new List<List<byte>>();
        for (int i = 0; i < redBallSelResult.Count; i++)
        {
            var list = redBallSelResult[i];
            var numOne = 0;
            var numTwo = 0;
            var numThree = 0;
            for (int j = 0; j < list.Count; j++)
            {
                if (list[j] >= 1 && list[j] <= 11)
                {
                    numOne++;
                }
                else if (list[j] >= 12 && list[j] <= 22)
                {
                    numTwo++;
                }
                else
                    numThree++;

            }
            var str = string.Format("{0}{1}{2}", numOne, numTwo, numThree);
            for (int k = 0; k < selectType.Count; k++)
            {
                if (str.Equals(selectType[k]))
                {
                    result.Add(list);
                }
            }
        }
        return result;
    }
    //--------------------------------------------------------------------------------
    /// <summary>
    /// 最大间隔号码过滤结果
    /// </summary>
    /// <returns></returns>
    public static List<List<byte>> GetMaxIntervalResult(List<List<byte>> redBallSelResult, List<int> selectType)
    {
        var result = new List<List<byte>>();
        var intervalList = new List<int>();
        for (int i = 0; i < redBallSelResult.Count; i++)
        {
            var list = redBallSelResult[i];

            intervalList.Clear();

            for (int j = 0; j < list.Count; j++)
            {
                if (j < list.Count - 1)
                {
                    intervalList.Add(list[j + 1] - list[j]);
                }
            }

            intervalList.Sort(SortBySize);

            if (intervalList.Count > 0)
            {
                for (int j = 0; j < selectType.Count; j++)
                {
                    if (selectType[j] == intervalList[0])
                    {
                        result.Add(list);
                    }
                }
            }
        }
        return result;
    }
    //----------------------------------------------------------------------------
    /// <summary>
    /// AC值过滤
    /// </summary>
    /// <returns></returns>
    public static List<List<byte>> GetAcFilterResult(List<List<byte>> redBallSelResult, List<int> selectType)
    {
        var result = new List<List<byte>>();
        for (int i = 0; i < redBallSelResult.Count; i++)
        {
            var curNumberData = redBallSelResult[i];
            var acNumber = GetAcNumber(curNumberData);
            for (int j = 0; j < selectType.Count; j++)
            {
                if (acNumber == selectType[j])
                {
                    result.Add(curNumberData);
                }
            }
        }
        return result;
    }
    //----------------------------------------------------------------------------
    /// <summary>
    /// 奇偶比过滤
    /// </summary>
    /// <param name="curNumberData"></param>
    /// <returns></returns>
    public static List<List<byte>> GetParityFilterResult(List<List<byte>> redBallSelResult, List<string> selectType)
    {
        var result = new List<List<byte>>();
        for (int i = 0; i < redBallSelResult.Count; i++)
        {
            var curNumberData = redBallSelResult[i];
            var curParityNumber = GetParityNumber(curNumberData);
            for (int j = 0; j < selectType.Count; j++)
            {
                if (selectType[j].Equals(curParityNumber))
                {
                    result.Add(curNumberData);
                }
            }
        }
        return result;
    }
    //----------------------------------------------------------------------------
    private static string GetParityNumber(List<byte> curNumberData)
    {
        var oldNum = 0;
        var even = 0;
        for (int i = 0; i < curNumberData.Count; i++)
        {
            if (curNumberData[i] % 2 == 0)
            {
                oldNum += 1;
            }
            else
            {
                even += 1;
            }
        }
        return even.ToString() + oldNum.ToString();
    }
    /// <summary>
    /// 尾数过滤
    /// </summary>
    /// <param name="redBallSelResult"></param>
    /// <param name="selectType"></param>
    /// <returns></returns>
    //----------------------------------------------------------------------------
    public static List<List<byte>> GetMantissaFilterResult(List<List<byte>> redBallSelResult, List<int> selectType)
    {
        var result = new List<List<byte>>();
        for (int i = 0; i < redBallSelResult.Count; i++)
        {
            var curNumberData = redBallSelResult[i];
            var curMantissaNumber = GetMantissaNumber(curNumberData);
            for (int j = 0; j < selectType.Count; j++)
            {
                if (selectType[j] == curMantissaNumber)
                {
                    result.Add(curNumberData);
                }
            }
        }
        return result;
    }
    //----------------------------------------------------------------------------
    /// <summary>
    /// 和值过滤
    /// </summary>
    /// <param name="currentNumList"></param>
    /// <returns></returns>
    public static List<List<byte>> GetSumvalueFilterResult(List<List<byte>> redBallSelResult, List<int> selectType)
    {
        var result = new List<List<byte>>();
        for (int i = 0; i < redBallSelResult.Count; i++)
        {
            var curNumberData = redBallSelResult[i];
            var curMantissaNumber = GetSumvalueNumber(curNumberData);
            for (int j = 0; j < selectType.Count; j++)
            {
                if (selectType[j] == curMantissaNumber)
                {
                    result.Add(curNumberData);
                }
            }
        }
        return result;
    }
    //----------------------------------------------------------------------------
    /// <summary>
    /// 连号过滤
    /// </summary>
    /// <param name="curNumData"></param>
    /// <returns></returns>
    public static List<List<byte>> GetSerialFilterResult(List<List<byte>> redBallSelResult, List<int> selectType)
    {
        var result = new List<List<byte>>();
        for (int i = 0; i < redBallSelResult.Count; i++)
        {
            var curNumberData = redBallSelResult[i];
            var curSerialNumber = GetSerialNumber(curNumberData);
            for (int j = 0; j < selectType.Count; j++)
            {
                if (selectType[j] == curSerialNumber)
                {
                    result.Add(curNumberData);
                }
            }
        }
        return result;
    }
    //----------------------------------------------------------------------------
    /// <summary>
    /// 重号过滤
    /// </summary>
    /// <param name="curNumData"></param>
    /// <returns></returns>
    public static List<List<byte>> GetDoubleFilterResult(List<List<byte>> redBallSelResult, List<int> selectType, List<byte> lastIssueNumber)
    {
        var result = new List<List<byte>>();
        for (int i = 0; i < redBallSelResult.Count; i++)
        {
            var curNumberData = redBallSelResult[i];
            var curSerialNumber = GetDoubleNumber(curNumberData, lastIssueNumber);
            for (int j = 0; j < selectType.Count; j++)
            {
                if (selectType[j] == curSerialNumber)
                {
                    result.Add(curNumberData);
                }
            }
        }
        return result;
    }
    /// <summary>
    /// 大小比顾虑
    /// </summary>
    /// <param name="curNumberData"></param>
    /// <param name="lastIssueNumber"></param>
    /// <returns></returns>
    public static List<List<byte>> GetSizeRatioFilterResult(List<List<byte>> redBallSelResult, List<string> selectType)
    {
        var result = new List<List<byte>>();
        for (int i = 0; i < redBallSelResult.Count; i++)
        {
            var curNumberData = redBallSelResult[i];
            var curSerialNumber = GetSizeRatioNumber(curNumberData);
            for (int j = 0; j < selectType.Count; j++)
            {
                if (selectType[j].Equals(curSerialNumber))
                {
                    result.Add(curNumberData);
                }
            }
        }
        return result;
    }
    //----------------------------------------------------------------------------
    private static string GetSizeRatioNumber(List<byte> curNumberData)
    {
        var bigNum = 0;
        var smallNum = 0;
        for (int i = 0; i < curNumberData.Count; i++)
        {
            if (curNumberData[i] >= 17)
            {
                bigNum += 1;
            }
            else
            {
                smallNum += 1;
            }
        }
        return bigNum.ToString() + smallNum.ToString();
    }
    //----------------------------------------------------------------------------
    private static int GetDoubleNumber(List<byte> curNumberData, List<byte> lastIssueNumber)
    {
        int value = 0;
        for (int i = 0; i < curNumberData.Count; i++)
        {
            for (int j = 0; j < lastIssueNumber.Count; j++)
            {
                if (curNumberData[i] == lastIssueNumber[j])
                {
                    value++;
                }
            }
        }
        return value;
    }
    //----------------------------------------------------------------------------
    private static int GetSerialNumber(List<byte> curNumData)
    {
        int num = 0;
        for (int i = 0; i < curNumData.Count; i++)
        {
            if (i < curNumData.Count - 1)
            {
                if (curNumData[i + 1] - curNumData[i] == 1)
                {
                    num += 1;
                }
            }
        }
        return num;
    }
    //----------------------------------------------------------------------------
    private static int GetSumvalueNumber(List<byte> curNumData)
    {
        int value = 0;
        for (int i = 0; i < curNumData.Count; i++)
        {
            value += curNumData[i];
        }
        return value / 10;
    }
    //----------------------------------------------------------------------------
    private static int GetMantissaNumber(List<byte> currentNumList)
    {
        var mantissaNumList = new List<int>();
        for (int i = 0; i < currentNumList.Count; i++)
        {
            if (currentNumList[i] < 10)
            {
                mantissaNumList.Add(currentNumList[i]);
            }
            else
            {
                mantissaNumList.Add(currentNumList[i] % 10);
            }

        }
        var list = new List<int>();
        foreach (var s in mantissaNumList.GroupBy(c => c))
        {
            list.Add(s.Count());
        }
        int num = 0;
        bool hasValue = false;
        foreach (int x in list)
        {
            if (hasValue)
            {
                if (x > num)
                    num = x;
            }
            else
            {
                num = x;
                hasValue = true;
            }
        }
        if (hasValue)

            return num;

        return 0;
    }
    //----------------------------------------------------------------------------
    private static int GetAcNumber(List<byte> curNumberData)
    {
        var absovallist = new List<int>();
        for (int j = 0; j < curNumberData.Count; j++)
        {
            if (j != 0)
                absovallist.Add(Mathf.Abs(curNumberData[0] - curNumberData[j]));
            if (j != 1)
                absovallist.Add(Mathf.Abs(curNumberData[1] - curNumberData[j]));
            if (j != 2)
                absovallist.Add(Mathf.Abs(curNumberData[2] - curNumberData[j]));
            if (j != 3)
                absovallist.Add(Mathf.Abs(curNumberData[3] - curNumberData[j]));
            if (j != 4)
                absovallist.Add(Mathf.Abs(curNumberData[4] - curNumberData[j]));
            if (j != 5)
                absovallist.Add(Mathf.Abs(curNumberData[5] - curNumberData[j]));
        }
        absovallist = absovallist.Distinct().ToList();//除去列表中重复元素Linq
        return absovallist.Count - 5;
    }
    //----------------------------------------------------------------------------
    static int SortBySize(int numberOne, int numberTwo)
    {
        if (numberOne > numberTwo)
        {
            return -1;
        }
        else
            return 1;
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
