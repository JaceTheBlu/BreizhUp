using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AssetBundlerSettingsProvider
{
    public const string SETTINGS_PATH = "PlateUp!/Asset bundler";

    private static bool _preparationExpanded = false;

    private static void Draw(
        string search_context
        )
    {
        SerializedObject serialized_object = new SerializedObject(AssetBundlerSettings.instance);
        SerializedProperty bundle_file_name_property = serialized_object.FindProperty("_bundleFileName");
        SerializedProperty output_folder_property = serialized_object.FindProperty("_outputFolder");

        EditorGUILayout.PropertyField(bundle_file_name_property);
        EditorGUILayout.PropertyField(output_folder_property);

        if (GUILayout.Button("Build asset bundle"))
        {
            AssetBundlerSettings.instance.BuildAssetBundle();
        }

        _preparationExpanded = EditorGUILayout.Foldout(_preparationExpanded, "Preparation", toggleOnLabelClick: true);

        if (_preparationExpanded)
        {
            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Remove all prefab materials"))
            {
                AssetBundlerSettings.instance.RemoveAllPrefabMaterials();
            }

            if (GUILayout.Button("Set all prefab materials to default"))
            {
                AssetBundlerSettings.instance.SetAllPrefabMaterialsToDefault();
            }

            EditorGUILayout.EndHorizontal();
        }

        serialized_object.ApplyModifiedPropertiesWithoutUndo();
    }

    [SettingsProvider]
    public static SettingsProvider CreateSettingsProvider()
    {
        var settings_provider = new SettingsProvider(SETTINGS_PATH, SettingsScope.Project)
        {
            label = "Asset bundler",
            guiHandler = Draw,
            keywords = new HashSet<string>(new[] { "Asset", "Bundler", "PlateUp" })
        };

        return settings_provider;
    }
}
