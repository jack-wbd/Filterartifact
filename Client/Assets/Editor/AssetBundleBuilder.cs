using System.IO;
using UnityEditor;
using UnityEngine;

public class AssetBundleBuilder : EditorWindow
{
    /// <summary>
    /// LZ4重新压缩。使用资源的时候不需要整体解压。在下载的时候可以使用LZMA算法，一旦它被下载了之后，它会使用LZ4算法保存到本地上
    /// 使用LZ4压缩，压缩率没有LZMA高，但是我们可以加载指定资源而不用解压全部 
    /// 注意使用LZ4压缩，可以获得可以跟不压缩想媲美的加载速度，而且比不压缩文件要小.
    /// </summary>
    //----------------------------------------------------------------------------
    [MenuItem("Assets/Export/全部打包/Windows64", false, 1001)]
    [MenuItem("AssetBundle/全部打包/Windows64", false, 1000)]
    public static void BuildAllAssetBundleForWindows64()
    {
        BuildPipeline.BuildAssetBundles(Application.streamingAssetsPath, BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.StandaloneWindows64);
        Debug.LogError("打包完成.");
    }
}
