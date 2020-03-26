//------------------------------------------------------------------------------
/**
	\file	DownLoadManager.cs

	Copyright (c) 2020, BoYue. All rights reserved.

	<PRE>

	模块名称：<文件所属的模块名称>
	文件名称：DownLoadManager.cs
	摘    要：<描述该文件实现的主要功能>

	当前版本：1.0
	建立日期：2020/3/25
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
//	DownLoadManager.cs
//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
/**
	\file	DownLoadManager.cs

	Copyright (c) 2020, BoYue. All rights reserved.

	<PRE>

	模块名称：<文件所属的模块名称>
	文件名称：DownLoadManager.cs
	摘    要：<描述该文件实现的主要功能>

	当前版本：1.0
	建立日期：2020/3/25
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
//	DownLoadManager.cs
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filterartifact
{
    class DownLoadManager : Manager
    {
        //----------------------------------------------------------------------------
        public static string SourceAddress = "http://www.cwl.gov.cn/";
        public static string OneHundredDoAddress = "http://www.cwl.gov.cn/cwl_admin/kjxx/findDrawNotice?name=ssq&issueCount=100";
        public static string RedKillDoAddress = "https://zst.cjcp.com.cn/shdd/ssq-hq-20.html";
        public static string BlueKillDoAddress = "https://zst.cjcp.com.cn/shdd/ssq-lq.html";
        public static string MethodType = "Get";
        public static int TimeOunt = 100000;
        public static int ReadWriteTimeout = 30000; 
        public static bool IsToLower = false;
        public static string Cookie = ""; 
        public static string UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:18.0) Gecko/20100101 Firefox/18.0";
        public static string Accept = "text/html, application/xhtml+xml, */*";
        public static string ContentType = "text/html";
        public static bool Allowautoredirect = false; 
        public static bool AutoRedirectCookie = false;
        public static string Postdata = ""; 
        public static DownLoadManager m_tSingleton;
        //----------------------------------------------------------------------------
        public static DownLoadManager Instance()
        {
            if (m_tSingleton == null)
            {
                m_tSingleton = new DownLoadManager();
                m_tSingleton.Initialized();
            }
            return m_tSingleton;
        }
        //----------------------------------------------------------------------------
        public override bool Initialized()
        {
            return true;
        }
        //----------------------------------------------------------------------------
        public override void Finalized()
        {
            base.Finalized();
        }
        //----------------------------------------------------------------------------
        //----------------------------------------------------------------------------
        //----------------------------------------------------------------------------

    }
}
