//------------------------------------------------------------------------------
/**
	\file	PVMStateDefine.cs

	Copyright (c) 2020, BoYue. All rights reserved.

	<PRE>

	模块名称�?文件所属的模块名称>
	文件名称：PVMStateDefine.cs
	�?   要：<描述该文件实现的主要功能>

	当前版本�?.0
	建立日期�?020/3/14
	�?   者：SYSTEM
	电子邮件�?username%@BoYue.com
	�?   注：<其它说明>

	</PRE>
*/
//------------------------------------------------------------------------------
// Using
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
// Class
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	PVMStateDefine.cs
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Filterartifact
{
    public struct sPVMParamInfo
    {
        public int nGroupTempID;
        public int nCopyTempID;
        public int nStageTempID;
        public string strName;
        public int nStageIndex;
        public Vector3 borthPoint;
        public static sPVMParamInfo Clear
        {
            get
            {
                sPVMParamInfo temp;
                temp.nGroupTempID = 0;
                temp.nCopyTempID = 0;
                temp.nStageTempID = 0;
                temp.strName = "";
                temp.nStageIndex = 0;
                temp.borthPoint = Vector3.zero;
                return temp;
            }
        }

    }
}
