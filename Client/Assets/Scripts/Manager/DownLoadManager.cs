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
        public static string Cookie = "UniqueID=KW7J1i4IFpk5Rbrm1596471257738; Sites=_21; _ga=GA1.3.1737314477.1596471253; _gid=GA1.3.647681559.1596471253; _Jo0OQK=6725B101606D6AB40902187798CF04AD545E38960767F0166F7AA785B0398B3EC0F2C0D7AD5AB240F62FD3032332C117BFA15F5713ACC780484BB5B52A26237419D14D60443865B47C66E86BF559106348BF1FDB3AE9ADADF3E1EDEE5F4E46F792B191548DFD201DCF1GJ1Z1dw==; 21_vq=2";
        public static string UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/84.0.4147.89 Safari/537.36";
        public static string Accept = "application/json, text/javascript, */*; q=0.01";
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
