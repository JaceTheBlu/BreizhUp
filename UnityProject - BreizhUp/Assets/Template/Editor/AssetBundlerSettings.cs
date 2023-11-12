using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[FilePath(ASSET_PATH, FilePathAttribute.Location.ProjectFolder)]
public class AssetBundlerSettings : ScriptableSingleton<AssetBundlerSettings>
{
    /// <summary>
    /// Where the settings asset is saved.
    /// </summary>
    private const string ASSET_PATH = "ProjectSettings/AssetBundlerSettings.asset";

    [SerializeField] private string _bundleFileName = "mod.assets";
    [SerializeField] private string _outputFolder = "content";

    public void Save()
    {
        Save(saveAsText: true);
    }

    public void BuildAssetBundle()
    {
        AssetBundler asset_bundler = new AssetBundler(_bundleFileName, _outputFolder);
        asset_bundler.BuildAssetBundle();
    }

    public void RemoveAllPrefabMaterials()
    {
        if (!AskRemoveAllPrefabMaterials())
        {
            return;
        }

        AssetBundler asset_bundler = new AssetBundler(_bundleFileName, _outputFolder);
        asset_bundler.RemoveAllPrefabMaterials();
    }

    public void SetAllPrefabMaterialsToDefault()
    {
        if (!AskSetAllPrefabMaterialsToDefault())
        {
            return;
        }

        AssetBundler asset_bundler = new AssetBundler(_bundleFileName, _outputFolder);
        asset_bundler.SetAllPrefabMaterialsToDefault();
    }

    private bool AskRemoveAllPrefabMaterials()
    {
        return InternalEditorUtility.inBatchMode
            || EditorUtility.DisplayDialog("Confirm", "Stripping materials from prefabs is an irreversible process. Perform at your own risk.", "Proceed", "Cancel");
    }

    private bool AskSetAllPrefabMaterialsToDefault()
    {
        return InternalEditorUtility.inBatchMode
            || EditorUtility.DisplayDialog("Confirm", "Changing the materials of prefabs is an irreversible process. Perform at your own risk.", "Proceed", "Cancel");
    }

    private void OnEnable()
    {
        hideFlags &= ~HideFlags.NotEditable;
    }
}
