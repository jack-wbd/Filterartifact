//------------------------------------------------------------------------------
/**
	\file	LoadingUIManager.cs

	Copyright (c) 2019, BoYue. All rights reserved.

	<PRE>

	模块名称�?文件所属的模块名称>
	文件名称：LoadingUIManager.cs
	�?   要：<描述该文件实现的主要功能>

	当前版本�?.0
	建立日期�?019/11/30
	�?   者：lenovo
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
//	LoadingUIManager.cs
//------------------------------------------------------------------------------

namespace Filterartifact
{
    public class LoadingUIManager : Manager
    {
        //----------------------------------------------------------------------------
        private static LoadingUIManager m_tSingleton = null;
        private LoadingConfigData m_loadCongData = null;
        private string m_curTexture = string.Empty;
        private AssetsManager m_assetmaneger = null;
        //----------------------------------------------------------------------------
        public static LoadingUIManager Instance()
        {
            if (m_tSingleton == null)
            {
                m_tSingleton = new LoadingUIManager();
                m_tSingleton.Initialized();
            }
            return m_tSingleton;
        }
        //----------------------------------------------------------------------------
        public override bool Initialized()
        {
            m_assetmaneger = WorldManager.Instance().GetLayer<AssetLayer>().GetManager();
            return base.Initialized();

        }
        //----------------------------------------------------------------------------
        public override void Update()
        {
            base.Update();
        }
        //----------------------------------------------------------------------------
        public override void LateUpdate()
        {
            base.LateUpdate();
        }
        //----------------------------------------------------------------------------
        public override void Finalized()
        {
            base.Finalized();
        }
        //----------------------------------------------------------------------------
        //预加载Loading图片
        public void PreLoadLoadingTexture(Callback<string, UnityEngine.Object> call, bool bFirst = false)
        {
            string nextLoadTexture = m_loadCongData.GetLoadTexture(bFirst);
            if (m_curTexture != nextLoadTexture)
            {
                m_assetmaneger.LoadAssetRes(nextLoadTexture, delegate (string strAssetID, UnityEngine.Object obj)
                    {
                        m_assetmaneger.UnLoadAsset(m_curTexture);
                        m_curTexture = strAssetID;
                        if (call != null)
                        {
                            call.Invoke(strAssetID,obj);
                        }
                    });
            }

        }
        //----------------------------------------------------------------------------

        //----------------------------------------------------------------------------
        //----------------------------------------------------------------------------
        //----------------------------------------------------------------------------
        //----------------------------------------------------------------------------
        //----------------------------------------------------------------------------
    }
}
