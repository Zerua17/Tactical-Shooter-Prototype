using UnityEngine;
using UnityEditor;

public static class UpgradeModelMetadata
{
    [MenuItem("Tools/Upgrade Robot Soldier Metadata")]
    private static void UpgradeMetadata()
    {
        string folderPath = "Assets/Prefabs/Robot_Soldier";

        string[] guids = AssetDatabase.FindAssets(
            "t:Model",
            new[] { folderPath }
        );

        string[] assetPaths = new string[guids.Length];

        for (int i = 0; i < guids.Length; i++)
        {
            assetPaths[i] = AssetDatabase.GUIDToAssetPath(guids[i]);
        }

        if (assetPaths.Length == 0)
        {
            Debug.LogWarning(
                "No se encontraron modelos en " + folderPath
            );
            return;
        }

        AssetDatabase.ForceReserializeAssets(
            assetPaths,
            ForceReserializeAssetsOptions.ReserializeMetadata
        );

        AssetDatabase.Refresh();

        Debug.Log(
            "Actualización solicitada para " +
            assetPaths.Length + " modelos."
        );
    }
}
