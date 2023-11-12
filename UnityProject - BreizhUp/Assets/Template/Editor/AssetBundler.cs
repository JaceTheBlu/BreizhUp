using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class AssetBundler
{
    /// <summary>
    /// The types of assets to search for in checks.
    /// </summary>
    private static readonly string ASSET_SEARCH_QUERY = "t:prefab,t:textAsset,t:audioclip";

    /// <summary>
    /// Temporary location for building AssetBundles.
    /// </summary>
    private static readonly string TEMP_BUILD_FOLDER = "Temp/AssetBundles";

    /// <summary>
    /// The folders to not search for assets in.
    /// </summary>
    private static readonly string[] EXCLUDED_FOLDERS = new string[] { "Assets/Editor", "Packages" };

    /// <summary>
    /// The build target of the asset bundle. Should either be StandaloneWindows or StandaloneOSX, depending on your platform.
    /// </summary>
    private BuildTarget Target = BuildTarget.StandaloneWindows;

    /// <summary>
    /// Number of warnings encountered.
    /// </summary>
    private int NumWarnings;

    /// <summary>
    /// Name of the output bundle file. This needs to match the bundle that you tag your assets with.
    /// </summary>
    private readonly string _bundleFileName = "mod.assets";

    /// <summary>
    /// The output folder to place the completed bundle in.
    /// </summary>
    private readonly string _outputFolder = "content";

    /// <summary>
    /// Number of warnings encountered.
    /// </summary>
    private string GeneratedAssetBundleTag;

    public AssetBundler(
        string bundleFileName,
        string outputFolder
        )
    {
        _bundleFileName = bundleFileName;
        _outputFolder = outputFolder;
    }

    public void BuildAssetBundle()
    {
        Debug.LogFormat("Creating 1\"{0}\" AssetBundle...", _bundleFileName);

        if (Application.platform == RuntimePlatform.OSXEditor) Target = BuildTarget.StandaloneOSX;

        // Randomly generate the resulting name of the asset bundle
        GenerateRandomAssetBundleTag();

        bool success = false;
        try
        {
            // Check for assets
            WarnIfAssetsAreNotTagged();
            WarnIfZeroAssetsAreTagged();
            WarnIfMeshAssetsAreTagged();
            // bundler.WarnIfMaterialsAreTaggedOrIncluded();

            // Delete the contents of OUTPUT_FOLDER
            CleanBuildFolder();

            // Temporarily move the tagged assets to the temporary tag
            MoveAssetsToTemporaryAssetBundle();

            // Lastly, create the asset bundle itself and copy it to the output folder
            CreateAssetBundle();

            success = true;
        }
        catch (Exception e)
        {
            Debug.LogErrorFormat("Failed to build AssetBundle: {0}\n{1}", e.Message, e.StackTrace);
        }

        // Return assets to the original asset bundle tag
        RestoreAssetBundleTags();
        AssetDatabase.RemoveUnusedAssetBundleNames();

        if (success)
        {
            Debug.LogFormat("[{0}] Build complete with {1} warnings! Output: {2} (temporary ID: {3})", DateTime.Now.ToLocalTime(), NumWarnings, _outputFolder + "/" + _bundleFileName, GeneratedAssetBundleTag);
        }
    }

    /// <summary>
    /// Generate the random asset bundle tag to use when building the asset bundle.
    /// </summary>
    private void GenerateRandomAssetBundleTag()
    {
        System.Random rand = new System.Random();
        GeneratedAssetBundleTag = $"mod-{rand.Next(0, int.MaxValue)}.assets";
    }

    /// <summary>
    /// Move assets tagged with BUNDLE_FILENAME to the temporary asset bundle
    /// </summary>
    private void MoveAssetsToTemporaryAssetBundle()
    {
        SubstituteAssetBundleTags(_bundleFileName, GeneratedAssetBundleTag);
    }

    /// <summary>
    /// Move assets tagged with the temporary asset bundle back to BUNDLE_FILENAME
    /// </summary>
    private void RestoreAssetBundleTags()
    {
        SubstituteAssetBundleTags(GeneratedAssetBundleTag, _bundleFileName);
    }

    /// <summary>
    /// Find all assets tagged with a certain asset bundle tag and replace them with another tag
    /// </summary>
    /// <param name="from">The asset bundle tag to search for</param>
    /// <param name="to">The new asset bundle tag</param>
    private void SubstituteAssetBundleTags(string from, string to)
    {
        string[] assetGUIDs = AssetDatabase.FindAssets($"b:{from}");
        foreach (var assetGUID in assetGUIDs)
        {
            string path = AssetDatabase.GUIDToAssetPath(assetGUID);
            var importer = AssetImporter.GetAtPath(path);
            importer.assetBundleName = to;
        }
    }

    /// <summary>
    /// Delete and recreate the OUTPUT_FOLDER to ensure a clean build.
    /// </summary>
    protected void CleanBuildFolder()
    {
        Debug.LogFormat("Cleaning {0}...", _outputFolder);

        if (Directory.Exists(_outputFolder))
        {
            Directory.Delete(_outputFolder, true);
        }

        Directory.CreateDirectory(_outputFolder);
    }

    /// <summary>
    /// Build the AssetBundle itself and copy it to the OUTPUT_FOLDER.
    /// </summary>
    protected void CreateAssetBundle()
    {
        Debug.Log("Building AssetBundle...");

        // Build all AssetBundles to the TEMP_BUILD_FOLDER
        if (!Directory.Exists(TEMP_BUILD_FOLDER))
        {
            Directory.CreateDirectory(TEMP_BUILD_FOLDER);
        }

#pragma warning disable 618
        // Build the asset bundle with the CollectDependencies flag. This is necessary or else ScriptableObjects will
        // not be accessible within the asset bundle. Unity has deprecated this flag claiming it is now always active,
        // but due to a bug we must still include it (and ignore the warning).
        BuildPipeline.BuildAssetBundles(
            TEMP_BUILD_FOLDER,
            BuildAssetBundleOptions.UncompressedAssetBundle | BuildAssetBundleOptions.CollectDependencies,
            Target);
#pragma warning restore 618

        // We are only interested in the BUNDLE_FILENAME bundle (and not any extra AssetBundle or the manifest files
        // that Unity makes), so just copy that to the final output folder
        string srcPath = Path.Combine(TEMP_BUILD_FOLDER, GeneratedAssetBundleTag);
        string destPath = Path.Combine(_outputFolder, _bundleFileName);
        File.Copy(srcPath, destPath, true);
    }

    /// <summary>
    /// Checks if the given path is a search path.
    /// </summary>
    /// <param name="path">The path to check.</param>
    /// <returns>true if the given path is a search path, otherwise false.</returns>
    protected static bool IsIncludedAssetPath(string path)
    {
        foreach (string excludedPath in EXCLUDED_FOLDERS)
        {
            if (path.StartsWith(excludedPath))
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Log a warning for all potential assets that are not currently tagged to be in this AssetBundle.
    /// </summary>
    protected void WarnIfAssetsAreNotTagged()
    {
        string[] assetGUIDs = AssetDatabase.FindAssets(ASSET_SEARCH_QUERY);
        foreach (var assetGUID in assetGUIDs)
        {
            string path = AssetDatabase.GUIDToAssetPath(assetGUID);
            if (!IsIncludedAssetPath(path))
            {
                continue;
            }

            var importer = AssetImporter.GetAtPath(path);
            if (!importer.assetBundleName.Equals(_bundleFileName))
            {
                Debug.LogWarningFormat("Asset \"{0}\" is not tagged with \"{1}\" and will not be included in the AssetBundle!", path, _bundleFileName);
                ++NumWarnings;
            }
        }
    }

    /// <summary>
    /// Verify that there is at least one asset to be included in the asset bundle.
    /// </summary>
    protected void WarnIfZeroAssetsAreTagged()
    {
        string[] assetsInBundle = AssetDatabase.FindAssets($"{ASSET_SEARCH_QUERY},b:{_bundleFileName}");
        if (assetsInBundle.Length == 0)
        {
            throw new Exception(string.Format("No assets have been tagged for inclusion in the {0} AssetBundle.", _bundleFileName));
        }
    }

    /// <summary>
    /// Warn if there are any mesh assets tagged. If so, the user probably meant to tag a prefab instead.
    /// </summary>
    protected void WarnIfMeshAssetsAreTagged()
    {
        string[] assetGUIDs = AssetDatabase.FindAssets($"t:mesh,b:{_bundleFileName}");
        foreach (var assetGUID in assetGUIDs)
        {
            string path = AssetDatabase.GUIDToAssetPath(assetGUID);
            if (!IsIncludedAssetPath(path))
            {
                continue;
            }

            Debug.LogWarningFormat("Mesh asset \"{0}\" is tagged for inclusion in the {1} AssetBundle! This is likely a mistake. You should include a prefab instead.", path, _bundleFileName);
            ++NumWarnings;
        }
    }

    /// <summary>
    /// Warn if there are any material assets tagged. If so, the user probably meant to tag a prefab instead.
    /// </summary>
    protected void WarnIfMaterialsAreTaggedOrIncluded()
    {
        // Check for directly tagged materials
        string[] assetGUIDs = AssetDatabase.FindAssets($"t:material,b:{_bundleFileName}");
        foreach (var assetGUID in assetGUIDs)
        {
            string path = AssetDatabase.GUIDToAssetPath(assetGUID);
            if (!IsIncludedAssetPath(path))
            {
                continue;
            }

            Debug.LogWarningFormat("Material asset \"{0}\" is tagged for inclusion in the {1} AssetBundle! This is likely a mistake. You should use generate materials using the vanilla shaders instead.", path, _bundleFileName);
            ++NumWarnings;
        }

        // Check for materials assigned to prefabs
        assetGUIDs = AssetDatabase.FindAssets($"t:prefab,b:{_bundleFileName}");
        foreach (var assetGUID in assetGUIDs)
        {
            string path = AssetDatabase.GUIDToAssetPath(assetGUID);
            if (!IsIncludedAssetPath(path))
            {
                continue;
            }

            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            MeshRenderer[] renderers = prefab.GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer renderer in renderers)
            {
                if (renderer.sharedMaterials.Any(m => m != null))
                {
                    Debug.LogWarningFormat("Material found attached to bundle prefab in \"{0}\" at \"<root>/{1}\"! This is likely a mistake. To avoid log spam and texturing issues, you should remove these materials or set them to \"None\".", path, GetGameObjectPath(renderer.transform).Split(new char[] { '/' }, 3)[2]);
                    ++NumWarnings;
                }
            }
        }
    }

    public static string GetGameObjectPath(Transform current)
    {
        if (current.parent == null)
            return "/" + current.name;
        return GetGameObjectPath(current.parent) + "/" + current.name;
    }

    public void RemoveAllPrefabMaterials()
    {
        string[] assetGUIDs = AssetDatabase.FindAssets($"t:prefab,b:{_bundleFileName}");
        foreach (var assetGUID in assetGUIDs)
        {
            string path = AssetDatabase.GUIDToAssetPath(assetGUID);
            if (!IsIncludedAssetPath(path))
            {
                continue;
            }

            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            MeshRenderer[] renderers = prefab.GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer renderer in renderers)
            {
                if (renderer.sharedMaterials.Length > 0)
                {
                    renderer.sharedMaterials = new Material[renderer.sharedMaterials.Length];
                    Debug.LogFormat("Stripped materials from \"{0}\" at \"<root>/{1}\".", path, GetGameObjectPath(renderer.transform).Split(new char[] { '/' }, 3)[2]);
                }
            }
        }

        Debug.LogFormat("[{0}] Done stripping materials.", DateTime.Now.ToLocalTime());
    }

    public void SetAllPrefabMaterialsToDefault()
    {
        string[] assetGUIDs = AssetDatabase.FindAssets($"t:prefab,b:{_bundleFileName}");
        foreach (var assetGUID in assetGUIDs)
        {
            string path = AssetDatabase.GUIDToAssetPath(assetGUID);
            if (!IsIncludedAssetPath(path))
            {
                continue;
            }

            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            MeshRenderer[] renderers = prefab.GetComponentsInChildren<MeshRenderer>();
            Material defaultMaterial = AssetDatabase.GetBuiltinExtraResource<Material>("Default-Material.mat");

            foreach (MeshRenderer renderer in renderers)
            {
                if (renderer.sharedMaterials.Length > 0)
                {
                    var newMaterials = new Material[renderer.sharedMaterials.Length];

                    for (int i = 0; i < newMaterials.Length; i++)
                    {
                        newMaterials[i] = defaultMaterial;
                    }

                    renderer.sharedMaterials = newMaterials;

                    Debug.LogFormat("Set materials from \"{0}\" at \"<root>/{1}\".", path, GetGameObjectPath(renderer.transform).Split(new char[] { '/' }, 3)[2]);
                }
            }
        }

        Debug.LogFormat("[{0}] Done setting materials.", DateTime.Now.ToLocalTime());
    }
}
