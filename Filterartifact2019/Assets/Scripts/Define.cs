using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public enum eUseEnvir
{
    none,
    city,
    battle,
    login,
    role,
    noenv
}
public enum EAssetType
{
    eObject,
    eAtlasm,
    eGameObject,
    eAudio,
    eTexture,
}
public struct sAssetInfo
{
    public string strID;
    public string strName;
    public bool bPreLoad;
    public eUseEnvir eEnvirUse;
    public string strFile;
    public EAssetType eAssetType;
    public List<string> childListAssetID;
    public bool bAlwaysCache;
    public bool bDeps;
    public string strDepsname;

    public static sAssetInfo zero
    {
        get
        {
            sAssetInfo temp;
            temp.strID = "";
            temp.strName = "";
            temp.strFile = "";
            temp.bPreLoad = true;
            temp.eAssetType = EAssetType.eGameObject;
            temp.eEnvirUse = eUseEnvir.none;
            temp.childListAssetID = new List<string>();
            temp.bAlwaysCache = false;
            temp.bDeps = false;
            temp.strDepsname = "";
            return temp;
        }
    }

}

