using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

namespace RPG.Dialogue.Editor
{
    public class DialogueEditor : EditorWindow
    {
        Dialogue selected_dialogue = null;

        private void OnEnable()
        {
            //Selection.selectionChanged += OnSelectionChange;            
        }

        private void OnSelectionChange()
        {
            Dialogue dialogue = Selection.activeObject as Dialogue;

            if (dialogue != null)
            {
                selected_dialogue = dialogue;
                Repaint();
            }
        }

        private void OnGUI()
        {
            // 參考網站 1: https://docs.unity3d.com/ScriptReference/EditorGUILayout.html
            if(selected_dialogue == null)
            {
                EditorGUILayout.LabelField("No Dialogue Selected.");
            }
            else
            {
                EditorGUILayout.LabelField($"Dialogue({selected_dialogue.name})");
            }
        }

        // 開啟 Editor 視窗
        [MenuItem("Window/Dialogue Editor")]
        public static void showEditorWindow()
        {
            // 參考網站 1: https://docs.unity3d.com/ScriptReference/EditorWindow.GetWindow.html
            //Debug.Log("showEditorWindow()");
            // utility: true 該視窗不會形成頁籤，無法添加到原本的視窗當中，而是獨立一個視窗。通常用於該功能為一次性，不會反覆使用到
            // utility: false 會形成頁籤，可以添加到原本的視窗當中。通常用於該功能會反覆使用。
            GetWindow(t: typeof(DialogueEditor), utility: false, title: "Dialogue Editor");
        }

        // OnOpenAssetAttribute -> using UnityEditor.Callbacks;
        // 1: OnOpenAssetAttribute 呼叫順序
        // 所有 asset 點擊後都會觸發此函式，因此需要利用 instance_id 分辨點擊的對象，是否是我們要處理的 asset，並返回是否可以處理
        // 參考網站 1: https://docs.unity3d.com/ScriptReference/Callbacks.OnOpenAssetAttribute.html
        // 參考網站 2: https://docs.unity3d.com/ScriptReference/EditorUtility.InstanceIDToObject.html
        [OnOpenAsset(1)]
        public static bool onOpenAsset(int instance_id, int line)
        {
            // 若利用 instance_id 所取得的 Object 實際上不是 Dialogue，則將為 null
            Dialogue dialogue = EditorUtility.InstanceIDToObject(instance_id) as Dialogue;

            if (dialogue != null)
            {
                //Debug.Log("onOpenDialogue");
                showEditorWindow();
                return true;
            }

            return false;
        }
    }
}

