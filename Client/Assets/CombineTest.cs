using System;
using System.Collections.Generic;
using UnityEngine;

public class CombineTest : MonoBehaviour
{
    //------------------------------------------------------------------------------
    void Start()
    {
        System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
        sw.Start();
        Combination(set, 33, 6);
        Debug.LogError("组合总数：" + charList.Count);
        sw.Stop();
        Debug.LogError(string.Format("total: {0} ms", sw.ElapsedMilliseconds));
    }
    //定义包含4个元素的集合 
    List<byte> set = new List<byte>() { 1, 2, 3, 4, 5, 6, 7 ,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27
    ,28,29,30,31,32,33};
    List<List<byte>> charList = new List<List<byte>>();
    //------------------------------------------------------------------------------
    //根据C的二进制表示输出一个组合 
    void print(List<byte> set, long C)
    {
        long l = 1;
        int i = 0;
        long k;
        List<byte> text = new List<byte>();
        while ((k = l << i) <= C)
        {//循环测试每个bit是否为1 
            if ((C & k) != 0)
            {
                text.Add(set[i]);
            }
            i++;
        }
        charList.Add(text);
    }
    //------------------------------------------------------------------------------
    //这个NextN跟之前我们讨论的是一样的，只不过省去了临时变量 
    long NextN(long N)
    {
        return (N + (N & (-N))) | ((N ^ (N + (N & (-N)))) / (N & (-N))) >> 2;
    }
    //------------------------------------------------------------------------------
    //求从set中前N个元素 中选择m个的组合 
    void Combination(List<byte> set, int N, int m)
    {
        long l = 1;
        long C = (l << m) - 1;
        while (C <= ((l << N) - (l << (N - m))))
        {
            print(set, C);
            C = NextN(C);
        }
    }
    //------------------------------------------------------------------------------
}
