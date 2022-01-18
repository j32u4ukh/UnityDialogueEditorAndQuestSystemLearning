using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System;

namespace RPG.Dialogue.Editor
{
    public class DialogueEditor : EditorWindow
    {
        Dialogue selected_dialogue = null;
        GUIStyle node_style;

        DialogueNode dragged_node = null;
        Vector2 dragging_offset;

        private void OnEnable()
        {
            //Selection.selectionChanged += OnSelectionChange;
            node_style = new GUIStyle();
            node_style.normal.background = EditorGUIUtility.Load("node0") as Texture2D;
            node_style.normal.textColor = Color.white;
            node_style.padding = new RectOffset(left: 20, right: 20, top: 20, bottom: 20);
            node_style.border = new RectOffset(left: 12, right: 12, top: 12, bottom: 12);
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
                processEvents();

                // 先繪製 Connection 再繪製 Node，可避免 Connection 畫到 Node 之上
                foreach (DialogueNode node in selected_dialogue.getAllNodes())
                {
                    drawConnection(node);
                }

                foreach (DialogueNode node in selected_dialogue.getAllNodes())
                {
                    drawNode(node);
                }
            }
        }

        private void processEvents()
        {
            switch (Event.current.type)
            {
                case EventType.MouseDown:
                    dragged_node = selected_dialogue.getNode(position: Event.current.mousePosition);

                    if(dragged_node != null)
                    {
                        dragging_offset = dragged_node.rect.position - Event.current.mousePosition;
                    }

                    break;

                case EventType.MouseDrag:
                    if (dragged_node != null)
                    {
                        Undo.RecordObject(selected_dialogue, "Move Dialogue Node");
                        dragged_node.rect.position = Event.current.mousePosition + dragging_offset;
                        //Repaint();
                        GUI.changed = true;
                    }                    
                    break;

                case EventType.MouseUp:
                    dragged_node = null;
                    break;                
            }
        }

        private void drawNode(DialogueNode node)
        {
            //GUILayout.BeginArea(new Rect(10f, 10f, 200f, 200f));
            GUILayout.BeginArea(screenRect: node.rect, style: node_style);
            EditorGUI.BeginChangeCheck();

            EditorGUILayout.LabelField("Node:", EditorStyles.whiteLabel);
            string new_id = EditorGUILayout.TextField(node.unique_id);
            string new_text = EditorGUILayout.TextField(node.text);

            if (EditorGUI.EndChangeCheck())
            {
                // 紀錄修改歷程，並取代 EditorUtility.SetDirty(selected_dialogue); 將 selected_dialogue 設為 Dirty，
                // 告訴 Unity 這個檔案已被修改，要更新 selected_dialogue 的數據，而不只有更新 Inspector
                // 在實際修改 selected_dialogue 前呼叫，才能回到最初的狀態
                Undo.RecordObject(selected_dialogue, "Update Dialogue");

                node.unique_id = new_id;

                // 更新 DialogueNode 的 text
                node.text = new_text;
            }

            GUILayout.EndArea();
        }

        private void drawConnection(DialogueNode node)
        {
            Vector3 start_position = new Vector3(node.rect.xMax, node.rect.center.y, 0f);
            Vector3 end_position, start_tangent, end_tangent, offset;

            foreach (DialogueNode child in selected_dialogue.getNodeChildren(root: node))
            {                
                end_position = new Vector3(child.rect.xMin, child.rect.center.y, 0f);
                offset = new Vector2((end_position.x - start_position.x) * 0.8f, 0f);
                start_tangent = start_position + offset;
                end_tangent = end_position - offset;

                Handles.DrawBezier(startPosition: start_position,
                                   endPosition: end_position,
                                   startTangent: start_tangent,
                                   endTangent: end_tangent,
                                   color: Color.white, 
                                   texture: null,
                                   width: 4f);
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

