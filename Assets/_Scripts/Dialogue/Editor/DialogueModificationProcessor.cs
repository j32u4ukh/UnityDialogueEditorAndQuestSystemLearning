using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace RPG.Dialogue.Editor
{
    /// <summary>
    /// 舊版 Unity 在對 ScriptableObject 物件修改名稱時，因與 AssetDatabase 當中名稱不同而發生問題，
    /// 導致修改父物件名稱，卻是隨機一個子物件被改名，並發生父子物件層級調換。
    /// 新版 Unity 已修正此問題，這個腳本是針對之前的問題所做的處理，這個專案實際上無需使用。
    /// https://issuetracker.unity3d.com/issues/parent-and-child-nested-scriptable-object-assets-switch-places-when-parent-scriptable-object-asset-is-renamed?_ga=2.72742166.489809380.1642587445-1671721701.1639661617
    /// </summary>
    public class DialogueModificationProcessor : UnityEditor.AssetModificationProcessor
    {
        //private static AssetMoveResult OnWillMoveAsset(string sourcePath, string destinationPath)
        //{
        //    Dialogue dialogue = AssetDatabase.LoadMainAssetAtPath(sourcePath) as Dialogue;

        //    // 其他類型檔案被改名時
        //    if(dialogue == null)
        //    {
        //        Debug.Log("Source path: " + sourcePath + ".\nDestination path: " + destinationPath + ".");
        //        return AssetMoveResult.DidNotMove;
        //    }

        //    if (!Path.GetDirectoryName(sourcePath).Equals(Path.GetDirectoryName(destinationPath)))
        //    {
        //        return AssetMoveResult.DidNotMove;
        //    }

        //    dialogue.name = Path.GetFileNameWithoutExtension(destinationPath);
        //    Debug.Log($"[DialogueModificationProcessor] OnWillMoveAsset | dialogue.name: {dialogue.name}");

        //    return AssetMoveResult.DidNotMove;
        //}
    }
}