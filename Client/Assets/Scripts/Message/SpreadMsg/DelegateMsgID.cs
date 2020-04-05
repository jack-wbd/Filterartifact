//------------------------------------------------------------------------------
/**
	\file	DelegateMsgID.cs

	Copyright (c) 2019, BoYue. All rights reserved.

	<PRE>

	模块名称：<文件所属的模块名称>
	文件名称：DelegateMsgID.cs
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
//	DelegateMsgID.cs
//------------------------------------------------------------------------------
namespace Filterartifact
{
    public enum DgMsgID
    {
        DgMsg_Invaild = 0,
        DgMsg_InitStatChange = 0x10000001,
        DgMsg_FileSystemOK = 0x10000002,
        DgMsg_UpdateGameProgress = 0x10000003,
        DgMsg_NtyUILoading_Finish = 0x10000004,
        DgMsg_ActiveLoadUI = 0x10000005,
        DgUI_ShowUI = 0x10000006,
        DgUI_HideLastUI = 0x10000007,
        DgMsg_PreLoadUI = 0x10000008,
        DgMsg_RegisterAllUI = 0x10000009,
        DgMsg_InitAfterMain = 0x10000010,
        DgMsg_HideLateToShow = 0x10000011,
        DgUI_HideUI = 0x10000012,
        DgUI_ShowNew = 0x10000013,
        DgMsg_GUIDE_NewbieShowNew = 0x10000014,
        DgMsg_GoToCity = 0x10000015,
        DgMsg_ShowLoadingUIByType = 0x10000016,
        DgMsg_ShowUIOneParam = 0x10000017,
        DgMsg_NtyLoadProsser_Finish = 0x10000018,
        DgMsg_LSNtyStageLoadingProcess = 0x10000019,
        DgMsg_NtyStageLoadingProcess = 0x10000020,
        DgMsg_UnloadAssetFalse = 0x10000021,
        DgMsg_HideLoadingUIByType = 0x10000022,
        DgUI_ShowOrHide = 0x10000023,
        DgUI_HideNew = 0x10000024,
    }
}

