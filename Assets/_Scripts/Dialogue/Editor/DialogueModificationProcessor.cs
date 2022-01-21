using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace RPG.Dialogue.Editor
{
    /// <summary>
    /// �ª� Unity �b�� ScriptableObject ����ק�W�ٮɡA�]�P AssetDatabase ���W�٤��P�ӵo�Ͱ��D�A
    /// �ɭP�ק������W�١A�o�O�H���@�Ӥl����Q��W�A�õo�ͤ��l����h�Žմ��C
    /// �s�� Unity �w�ץ������D�A�o�Ӹ}���O�w�蠟�e�����D�Ұ����B�z�A�o�ӱM�׹�ڤW�L�ݨϥΡC
    /// https://issuetracker.unity3d.com/issues/parent-and-child-nested-scriptable-object-assets-switch-places-when-parent-scriptable-object-asset-is-renamed?_ga=2.72742166.489809380.1642587445-1671721701.1639661617
    /// </summary>
    public class DialogueModificationProcessor : UnityEditor.AssetModificationProcessor
    {
        //private static AssetMoveResult OnWillMoveAsset(string sourcePath, string destinationPath)
        //{
        //    Dialogue dialogue = AssetDatabase.LoadMainAssetAtPath(sourcePath) as Dialogue;

        //    // ��L�����ɮ׳Q��W��
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