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
        public static string Cookie = "UniqueID=j9SnY8tqrOnEmgyb1586787996550; Sites=_21; 21_vq=34; _Jo0OQK=6E3F47F8CB8F460CA24838579B1219C7EC5AC5A31A31A607BC44B89C2257B497CF2FB5D5D8F437DF9834843A9790A90DDA949D06C6FC3774A75021B776A8FCEA7D9F1B3C19C5B2FC5F8E6E66EDA7420CD4BE6E66EDA7420CD4B996CC48E493F4D533328EEBACF9E4A3CGJ1Z1dw==;" +
            " _ga=GA1.3.1280643240.1585452116; HMF_CI=5934fb3eb285f0d34b89cac6221ca462e1580ff03f6b3d8c59822200d116f515c0; _gid=GA1.3.1441132775.1586787997; HBB_HC=1a70f848e31b4e7fd1c69225527dc6a6d755b269c7d72964a0e804a048dac98464d39c0963bb91b4668b23a4ec0b890e9d; _gat_gtag_UA_113065506_1=1; HMY_JC=727e5f4460a2395f065b551e39f25564868978ce7251516551ee2be511e3b7bc52,714; " +
            "HYB_SH=4c817249706fb235b6b804dd5b76eb95c7";
        public static string UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:75.0) Gecko/20100101 Firefox/75.0";
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
