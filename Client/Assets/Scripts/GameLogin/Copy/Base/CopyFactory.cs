//------------------------------------------------------------------------------
/**
	\file	CopyFactory.cs

	Copyright (c) 2020, BoYue. All rights reserved.

	<PRE>

	模块名称�?文件所属的模块名称>
	文件名称：CopyFactory.cs
	�?   要：<描述该文件实现的主要功能>

	当前版本�?.0
	建立日期�?020/3/15
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
//	CopyFactory.cs
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Filterartifact
{
    public class CopyFactory
    {
        //----------------------------------------------------------------------------
        public Copy Constract_Copy(string strCopyName,StageLayer layer)
        {
            Copy newCopy = null;
            string strRootPath = "Map/";
            strRootPath += strCopyName + "/";
            string strXmlFileName = strRootPath;
            strXmlFileName += strCopyName;
            XmlDocument xmlDouc = new XmlDocument();
            string XmlData = FileSystem.Instance().LoadXml(strXmlFileName + ".xml");
            xmlDouc.LoadXml(XmlData);
            XmlElement copyElement = xmlDouc.SelectSingleNode("Copy") as XmlElement;
            if (copyElement==null)
            {
                return null;
            }
            int nType = int.Parse(copyElement.GetAttribute("TypeID"));
            eSceneType type = eSceneType.BATTLE_SCENE;
            type = GetCopyType(nType);
            newCopy = CreateCopy(type,layer);
            if (!newCopy.Create(copyElement,ref strCopyName,type))
            {
                newCopy = null;
                return null;
            }
            xmlDouc = null;
            return newCopy;
        }
        //----------------------------------------------------------------------------
        private Copy CreateCopy(eSceneType Type, StageLayer layer)
        {
            Copy copy = null;
            switch (Type)
            {
                case eSceneType.CITY_SCENE:
                    break;
                case eSceneType.BATTLE_SCENE:
                    break;
                case eSceneType.CREATEROLE_SCENE:
                    break;
                case eSceneType.NEWBIE_SCENE_OLD:
                    break;
                case eSceneType.PVR_SCENE:
                    break;
                case eSceneType.PVP_SCENE:
                    break;
                case eSceneType.LOBBY_SCENE:
                    copy = new LobbyStage(layer);
                    break;
                case eSceneType.ACTIVE_SCENE:
                    break;
                case eSceneType.Elimit_SCENE:
                    break;
                case eSceneType.NEWBIE_SCENE:
                    break;
                case eSceneType.QUICKRUNGAME_SCENE:
                    break;
                case eSceneType.COMMON_SCENE:
                    break;
                case eSceneType.OFFLINEPK_SCENE:
                    break;
                case eSceneType.KOF_SCENE:
                    break;
                case eSceneType.PROMOTION_SCENE:
                    break;
                case eSceneType.PRELIM_SCENE:
                    break;
                case eSceneType.MOBA:
                    break;
                case eSceneType.TSPORTS:
                    break;
                case eSceneType.VIDEO_SCENE:
                    break;
                case eSceneType.UNKNOWN_REGION:
                    break;
                default:
                    break;
            }
            return copy;
        }
        //----------------------------------------------------------------------------
        private eSceneType GetCopyType(int nType)
        {
            eSceneType sceneType = eSceneType.BATTLE_SCENE;
            switch (nType)
            {
                case 0:
                    sceneType = eSceneType.CITY_SCENE;
                    break;
                case 1:
                    sceneType = eSceneType.BATTLE_SCENE;
                    break;
                case 2:
                    sceneType = eSceneType.CREATEROLE_SCENE;
                    break;
                case 3:
                    sceneType = eSceneType.NEWBIE_SCENE_OLD;
                    break;
                case 4:
                    sceneType = eSceneType.PVR_SCENE;
                    break;
                case 5:
                    sceneType = eSceneType.PVP_SCENE;
                    break;
                case 6:
                    sceneType = eSceneType.LOBBY_SCENE;
                    break;
                case 7:
                    sceneType = eSceneType.ACTIVE_SCENE;
                    break;
                case 8:
                    sceneType = eSceneType.Elimit_SCENE;
                    break;
                case 9:
                    sceneType = eSceneType.NEWBIE_SCENE;
                    break;
                case 10:
                    sceneType = eSceneType.QUICKRUNGAME_SCENE;
                    break;
                case 12:
                    sceneType = eSceneType.OFFLINEPK_SCENE;
                    break;
                case 13:
                    sceneType = eSceneType.KOF_SCENE;
                    break;
                case 14:
                    sceneType = eSceneType.PROMOTION_SCENE;
                    break;
                case 15:
                    sceneType = eSceneType.PRELIM_SCENE;
                    break;
            }
            return sceneType;
        }
        //----------------------------------------------------------------------------
        public static Scene CreateScene(eSceneType Type,Copy copy)
        {
            Scene scene = null;
            switch (Type)
            {
                case eSceneType.CITY_SCENE:
                    break;
                case eSceneType.BATTLE_SCENE:
                    break;
                case eSceneType.CREATEROLE_SCENE:
                    break;
                case eSceneType.NEWBIE_SCENE_OLD:
                    break;
                case eSceneType.PVR_SCENE:
                    break;
                case eSceneType.PVP_SCENE:
                    break;
                case eSceneType.LOBBY_SCENE:
                    scene = new LobbyScene(copy);
                    break;
                case eSceneType.ACTIVE_SCENE:
                    break;
                case eSceneType.Elimit_SCENE:
                    break;
                case eSceneType.NEWBIE_SCENE:
                    break;
                case eSceneType.QUICKRUNGAME_SCENE:
                    break;
                case eSceneType.COMMON_SCENE:
                    break;
                case eSceneType.OFFLINEPK_SCENE:
                    break;
                case eSceneType.KOF_SCENE:
                    break;
                case eSceneType.PROMOTION_SCENE:
                    break;
                case eSceneType.PRELIM_SCENE:
                    break;
                case eSceneType.MOBA:
                    break;
                case eSceneType.TSPORTS:
                    break;
                case eSceneType.VIDEO_SCENE:
                    break;
                case eSceneType.UNKNOWN_REGION:
                    break;
                default:
                    break;
            }
            return scene;
        }
        //----------------------------------------------------------------------------
    }
}
