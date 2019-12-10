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
    //----------------------------------------------------------------------------
    public static string LoadingTexture = "Common/LoadingConfig.xml";

    //----------------------------------------------------------------------------
}
//----------------------------------------------------------------------------

