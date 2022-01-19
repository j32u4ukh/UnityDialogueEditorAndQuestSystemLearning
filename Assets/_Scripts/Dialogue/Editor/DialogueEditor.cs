using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System;

namespace RPG.Dialogue.Editor
{
    // EditorWindow 中的屬性變數預設為 SerializeField，因此當重新開啟頁籤，變數中可能還存著上一次操作時的數值，導致非預期結果
    // 因此使用 NonSerialized 確保變數在下次載入時，會是空的。但 selected_dialogue 希望繼續記得，因此不添加。
    public class DialogueEditor : EditorWindow
    {
        Dialogue selected_dialogue = null;
        [NonSerialized] GUIStyle node_style;

        [NonSerialized] DialogueNode dragged_node = null;
        [NonSerialized] Vector2 dragging_offset;

        [NonSerialized] DialogueNode parent_node = null;
        [NonSerialized] DialogueNode removed_node = null;
        [NonSerialized] DialogueNode linking_parent_node = null;

        [NonSerialized] bool is_canvas_dragged = false;
        [NonSerialized] Vector2 dragged_start_point;
        Vector2 scroll_position = Vector2.zero;
        const float CANVAS_SIZE = 4000f;
        const float BACKGROUND_SIZE = 50f;

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
                if(parent_node != null)
                {
                    Undo.RecordObject(selected_dialogue, "Create New Dialogue Node.");
                    selected_dialogue.addChildNode(parent_node);
                    parent_node = null;
                }

                // TODO: 目前無法刪除帶有子節點的節點
                if(removed_node != null)
                {
                    Undo.RecordObject(selected_dialogue, "Remove Dialogue Node.");
                    selected_dialogue.removeNode(removed_node);
                    removed_node = null;
                }

                processEvents();
                scroll_position = EditorGUILayout.BeginScrollView(scroll_position);
                Rect canvas = GUILayoutUtility.GetRect(CANVAS_SIZE, CANVAS_SIZE);
                Texture2D texture = Resources.Load<Texture2D>("background");
                Rect coords = new Rect(0f, 0f, CANVAS_SIZE / BACKGROUND_SIZE, CANVAS_SIZE / BACKGROUND_SIZE);
                GUI.DrawTextureWithTexCoords(canvas, texture, coords);

                // 先繪製 Connection 再繪製 Node，可避免 Connection 畫到 Node 之上
                foreach (DialogueNode node in selected_dialogue.getAllNodes())
                {
                    drawConnection(node);
                }

                foreach (DialogueNode node in selected_dialogue.getAllNodes())
                {
                    drawNode(node);
                }
                
                EditorGUILayout.EndScrollView();
            }
        }

        private void processEvents()
        {
            switch (Event.current.type)
            {
                case EventType.MouseDown:
                    dragged_node = selected_dialogue.getNode(position: scroll_position + Event.current.mousePosition);
                    
                    if(dragged_node != null)
                    {
                        dragging_offset = dragged_node.rect.position - Event.current.mousePosition;
                        Selection.activeObject = dragged_node;
                    }
                    else
                    {
                        is_canvas_dragged = true;
                        dragged_start_point = Event.current.mousePosition + scroll_position;
                        Selection.activeObject = selected_dialogue;
                    }

                    break;

                case EventType.MouseDrag:
                    if (dragged_node != null)
                    {
                        Undo.RecordObject(selected_dialogue, "Move Dialogue Node");
                        dragged_node.rect.position = Event.current.mousePosition + dragging_offset;
                    }
                    else if(is_canvas_dragged)
                    {
                        scroll_position = dragged_start_point - Event.current.mousePosition;
                    }

                    GUI.changed = true;
                    break;

                case EventType.MouseUp:                        
                    dragged_node = null;
                    is_canvas_dragged = false;
                    break;                
            }
        }

        private void drawNode(DialogueNode node)
        {
            GUILayout.BeginArea(screenRect: node.rect, style: node_style);
            EditorGUI.BeginChangeCheck();

            string new_text = EditorGUILayout.TextField(node.text);

            if (EditorGUI.EndChangeCheck())
            {
                // 紀錄修改歷程，並取代 EditorUtility.SetDirty(selected_dialogue); 將 selected_dialogue 設為 Dirty，
                // 告訴 Unity 這個檔案已被修改，要更新 selected_dialogue 的數據，而不只有更新 Inspector
                // 在實際修改 selected_dialogue 前呼叫，才能回到最初的狀態
                Undo.RecordObject(selected_dialogue, "Update Dialogue");

                // 更新 DialogueNode 的 text
                node.text = new_text;
            }

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("+"))
            {
                parent_node = node;
            }

            operateLinking(node);

            if (GUILayout.Button("-"))
            {
                removed_node = node;
            }

            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }

        private void operateLinking(DialogueNode node)
        {
            if (linking_parent_node == null)
            {
                if (GUILayout.Button("link"))
                {
                    linking_parent_node = node;
                }
            }

            else if (linking_parent_node.children.Contains(node.name))
            {
                if (GUILayout.Button("unlink"))
                {
                    Undo.RecordObject(selected_dialogue, "Remove Dialogue Link");
                    linking_parent_node.children.Remove(node.name);
                    linking_parent_node = null;
                }
            }

            else if (!node.name.Equals(linking_parent_node.name))
            {
                if (GUILayout.Button("child"))
                {
                    Undo.RecordObject(selected_dialogue, "Add Dialogue Link");
                    linking_parent_node.children.Add(node.name);
                    linking_parent_node = null;
                }
            }

            // 當前 node 即為 linking_parent_node
            else
            {
                if (GUILayout.Button("cancel"))
                {
                    linking_parent_node = null;
                }
            }
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

