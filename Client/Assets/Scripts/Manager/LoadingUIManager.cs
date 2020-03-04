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
        public static bool isInLoading = false;
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
            m_loadCongData = WorldManager.Instance().GetDataCollection<LoadingConfigData>();
            Messenger.AddListener<eSceneType>(DgMsgID.DgMsg_ShowLoadingUIByType, OnShowLoadingUIByType);
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
            Messenger.RemoveListener<eSceneType>(DgMsgID.DgMsg_ShowLoadingUIByType, OnShowLoadingUIByType);
        }
        //----------------------------------------------------------------------------
        private void OnShowLoadingUIByType(eSceneType type)
        {
            switch (type)
            {
                case eSceneType.CREATEROLE_SCENE:
                    break;
                case eSceneType.NEWBIE_SCENE_OLD:
                    break;
                case eSceneType.LOBBY_SCENE:
                    Messenger.Broadcast(DgMsgID.DgMsg_ShowUIOneParam, "UILoadingCtrl", (object)m_curTexture);
                    break;
                case eSceneType.COMMON_SCENE:
                    break;
                case eSceneType.BATTLE_SCENE:
                    break;
                case eSceneType.UNKNOWN_REGION:
                    break;
                default:
                    break;
            }
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
                            call.Invoke(strAssetID, obj);
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
