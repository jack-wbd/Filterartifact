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
            Object.Destroy(com);
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
    /// <summary>
    /// 查找子对象
    /// </summary>
    public static GameObject FindChildObject(Transform go)
    {
        string objName = go.name;
        GameObject tagetObj = null;
        if (objName.Contains("uiroot_matchex"))
        {
            tagetObj = go.transform.Find("MatchUI/Canvas/CneterPageNode").gameObject;
        }
        else
        {
            tagetObj = go.transform.Find("Canvas/CenterPageNode").gameObject;
        }
        return tagetObj;
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

        GameObject cloneGo = Object.Instantiate(go, parent);
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
    public static GameObject FindChildGO(this GameObject parent,string path,bool showError =true)
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

    //----------------------------------------------------------------------------
}