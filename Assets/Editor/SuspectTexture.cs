using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SuspectTexture : MonoBehaviour
{
    [MenuItem("Tools/Find Suspect Textures")]
    public static void FindSuspectTextures()
    {
        foreach (var guid in AssetDatabase.FindAssets("t:Texture2D"))
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            var tex = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
            if (tex.width > 2048 || tex.height > 2048 || tex.isReadable)
            {
                Debug.LogWarning($"Suspect Texture: {path} - {tex.width}x{tex.height} Readable: {tex.isReadable}");
            }
        }
    }

    [MenuItem("Tools/Audit Scene Textures")]
    public static void AuditTextures()
    {
        foreach (var renderer in GameObject.FindObjectsOfType<Renderer>())
        {
            foreach (var mat in renderer.sharedMaterials)
            {
                if (mat == null) continue;
                foreach (var name in mat.GetTexturePropertyNames())
                {
                    var tex = mat.GetTexture(name) as Texture2D;
                    if (tex != null)
                    {
                        Debug.Log($"{tex.name} - {tex.width}x{tex.height}, readable: {tex.isReadable}, mipmaps: {tex.mipmapCount > 1}");
                    }
                }
            }
        }
    }

    [MenuItem("Tools/Check All Texture2D Assets")]
    public static void ValidateTextures()
    {
        var guids = AssetDatabase.FindAssets("t:Texture2D");
        foreach (var guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            var tex = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
            if (tex == null) continue;

            string issues = "";
            if (tex.width > 2048 || tex.height > 2048)
                issues += $"[BIG: {tex.width}x{tex.height}] ";
            if (tex.isReadable)
                issues += "[Read/Write Enabled] ";
            if (tex.mipmapCount > 1)
                issues += "[MipMaps] ";

            if (!string.IsNullOrEmpty(issues))
                Debug.LogWarning($"{path} --> {issues}");
        }
    }

    [MenuItem("Tools/Scan Textures for Crashes")]
    static void CheckTextures()
    {
        var guids = AssetDatabase.FindAssets("t:Texture2D");
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            Texture2D tex = AssetDatabase.LoadAssetAtPath<Texture2D>(path);

            if (tex == null) continue;

            if (tex.width > 2048 || tex.height > 2048)
                Debug.LogWarning($"LARGE: {path} — {tex.width}x{tex.height}");

            TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
            if (importer != null)
            {
                if (importer.isReadable)
                    Debug.LogWarning($"READABLE: {path}");
                if (importer.mipmapEnabled)
                    Debug.LogWarning($"MIPMAP: {path}");
                if (importer.textureCompression == TextureImporterCompression.Uncompressed)
                    Debug.LogWarning($"UNCOMPRESSED: {path}");
            }
        }
    }
}
