//------------------------------------------------------------------------------
/**
	\file	Define.cs

	Copyright (c) 2019, BoYue. All rights reserved.

	<PRE>

	模块名称：<文件所属的模块名称>
	文件名称：Define.cs
	摘    要：<描述该文件实现的主要功能>

	当前版本：1.0
	建立日期：2019/11/28
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
//	Define.cs
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

//------------------------------------------------------------------------------
/**
	\file	Define.cs

	Copyright (c) 2019, BoYue. All rights reserved.

	<PRE>

	模块名称：<文件所属的模块名称>
	文件名称：Define.cs
	摘    要：<描述该文件实现的主要功能>

	当前版本：1.0
	建立日期：2019/11/28
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
//	Define.cs
//------------------------------------------------------------------------------
public enum eUseEnvir
{
    none,
    city,
    battle,
    login,
    role,
    noenv
}
//----------------------------------------------------------------------------
public enum EAssetType
{
    eObject,
    eAtlas,
    eGameObject,
    eAudio,
    eTexture,
}
//----------------------------------------------------------------------------
public struct sAssetInfo
{
    public string strID;
    public string strName;
    public bool bPreLoad;
    public eUseEnvir eEnvirUse;
    public string strFile;
    public string assetName;//游戏里用的文件路径
    public EAssetType eAssetType;
    public List<string> childListAssetID;
    public bool bAlwaysCache;
    public bool bDeps;
    public string strDepsname;
    public string strRealFile;
    public string bundleName;
    //----------------------------------------------------------------------------
    public static sAssetInfo zero
    {
        get
        {
            sAssetInfo temp;
            temp.strID = "";
            temp.strName = "";
            temp.strFile = "";
            temp.assetName = "";
            temp.bPreLoad = true;
            temp.eAssetType = EAssetType.eGameObject;
            temp.eEnvirUse = eUseEnvir.none;
            temp.childListAssetID = new List<string>();
            temp.bAlwaysCache = false;
            temp.bDeps = false;
            temp.strDepsname = "";
            temp.strRealFile = "";
            temp.bundleName = "";
            return temp;
        }
    }
    //----------------------------------------------------------------------------
    public void ParseEnvir(string strEnvir)
    {

        if (strEnvir.Equals("none"))
            eEnvirUse = eUseEnvir.none;
        else if (strEnvir.Equals("city"))
            eEnvirUse = eUseEnvir.city;
        else if (strEnvir.Equals("battle"))
            eEnvirUse = eUseEnvir.battle;
        else if (strEnvir.Equals("login"))
            eEnvirUse = eUseEnvir.login;
        else if (strEnvir.Equals("role"))
            eEnvirUse = eUseEnvir.role;
        else
            eEnvirUse = eUseEnvir.role;

    }
}
//----------------------------------------------------------------------------
public enum UpdateState
{
    Update_Load_VersionUrl,
    Load_Gamedata,
    Parse_GameData,
    Catch_GameRes,
    Init_Game,
}
//----------------------------------------------------------------------------
public struct XmlFile
{
    public static string LoadingTexture = "LoadingConfig.xml";
    public static string PromptLang = "PromptData.xml";
}
//----------------------------------------------------------------------------
public struct UseColor
{
    public static readonly Color normal = new Color(200 / 255f, 200 / 255f, 180 / 255f);         //游戏新增正常颜色
    public static readonly Color gray = new Color(120 / 255f, 130 / 255f, 150 / 255f);           //灰色
    public static readonly Color black = Color.black;                                            //黑色
    public static readonly Color white = Color.white;                                            //白色
    public static readonly Color green = new Color(6 / 255f, 254 / 255f, 0f);                   //绿色
    public static readonly Color blue = new Color(75 / 255f, 175 / 255f, 252 / 255f);            //蓝色
    public static readonly Color purple = new Color(250 / 255f, 0 / 255f, 171 / 255f);           //紫色
    public static readonly Color orange = new Color(1f, 134 / 255f, 61 / 255f);                  //橘色（橙色）
    public static readonly Color yellow = new Color(255 / 255f, 234 / 255f, 0f);                 //黄色（金色）
    public static readonly Color acidBlue = new Color(0 / 255f, 253 / 255f, 196 / 255f);         //湖蓝
    public static readonly Color acidBlue2 = new Color(90 / 255f, 266 / 255f, 246 / 255f);       //新湖蓝(星星颜色)
    public static readonly Color pink = new Color(242 / 255f, 110 / 255f, 250 / 255f);           //粉色
    public static readonly Color brilliantYellow = new Color(254 / 255f, 245 / 255f, 143 / 255f);//亮黄
    public static readonly Color red = new Color(1f, 0f, 0f);                                    //红色
    public static readonly Color brightYellow = new Color(255 / 255f, 209 / 255f, 55 / 255f);      //亮黄
    public static readonly Color redOrange = new Color(255 / 255f, 115 / 255f, 55 / 255f);          //红橙
    public static readonly Color brightPurple = new Color(250 / 255f, 54 / 255f, 127 / 255f);      //亮紫
    public static readonly Color allBlue = new Color(49 / 255f, 255 / 255f, 224 / 255f);         //蔚蓝色

    public static readonly Color restraintColorD = new Color(116 / 255f, 150 / 255f, 170 / 255f, 255 / 255f);//战斗飘字“抵抗”的颜色
    public static readonly Color restraintColorK = new Color(255 / 255f, 174 / 255f, 71 / 255f, 255 / 255f);//战斗飘字“克制”的颜色

    public static readonly Color NpcClickWhite = new Color(128 / 255f, 128 / 255f, 128 / 255f);
    public static readonly Color grey150 = new Color(150 / 255f, 150 / 255f, 150 / 255f);

    public static readonly Color color1 = new Color(255f, 255f, 255f, 255f);
    public static readonly Color color2 = new Color(0f, 0f, 0f, 255f);

    public static Color[] IconColors = { gray, white, green, blue, purple, orange, yellow, acidBlue, pink, brilliantYellow, red, brightYellow, redOrange, brightPurple, allBlue };
    public static string[] ColorArrayStr = { "[788296]", "[ffffff]", "[06fe00]", "[4baffc]", "[fa00ab]", "[ff863d]", "[ffea00]", "[00fdc4]", "[f26efa]", "[fef58f]", "[ff0000]", "[ffd137]", "[ff7337]", "[fa367f]", "[31ffe0]" };
    public static string[] ColorStrings = { "White", "Green", "Blue", "Purple", "Orange", "Red" };
    public static Color[] IconColorsForStar = { gray, white, green, blue, purple, orange, yellow, acidBlue2, pink, brilliantYellow, red, brightYellow, redOrange, brightPurple, allBlue };   //星星颜色使用
}
//----------------------------------------------------------------------------

