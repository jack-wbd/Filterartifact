﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace HotFix_Project
{
    public class TestCoroutine
    {
        public static void RunTest()
        {
            CoroutineDemo.Instance.DoCoroutine(Coroutine());
        }

        static System.Collections.IEnumerator Coroutine()
        {
            Debug.Log("开始协程,t=" + Time.time);
            yield return new WaitForSeconds(10);
            Debug.Log("等待了10秒,t=" + Time.time);
        }
    }
}
