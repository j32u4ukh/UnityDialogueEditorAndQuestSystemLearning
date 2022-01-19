using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System;

namespace RPG.Dialogue.Editor
{
    // EditorWindow �����ݩ��ܼƹw�]�� SerializeField�A�]�����s�}�ҭ��ҡA�ܼƤ��i���٦s�ۤW�@���ާ@�ɪ��ƭȡA�ɭP�D�w�����G
    // �]���ϥ� NonSerialized �T�O�ܼƦb�U�����J�ɡA�|�O�Ū��C�� selected_dialogue �Ʊ��~��O�o�A�]�����K�[�C
    public class DialogueEditor : EditorWindow
    {
        Dialogue selected_dialogue = null;
        [NonSerialized] GUIStyle npc_style;
        [NonSerialized] GUIStyle player_style;

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
            npc_style = new GUIStyle();
            npc_style.normal.background = EditorGUIUtility.Load("node0") as Texture2D;
            npc_style.normal.textColor = Color.white;
            npc_style.padding = new RectOffset(left: 20, right: 20, top: 20, bottom: 20);
            npc_style.border = new RectOffset(left: 12, right: 12, top: 12, bottom: 12);

            player_style = new GUIStyle();
            player_style.normal.background = EditorGUIUtility.Load("node1") as Texture2D;
            player_style.normal.textColor = Color.white;
            player_style.padding = new RectOffset(left: 20, right: 20, top: 20, bottom: 20);
            player_style.border = new RectOffset(left: 12, right: 12, top: 12, bottom: 12);
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
            // �ѦҺ��� 1: https://docs.unity3d.com/ScriptReference/EditorGUILayout.html
            if(selected_dialogue == null)
            {
                EditorGUILayout.LabelField("No Dialogue Selected.");
            }
            else
            {
                if (parent_node != null)
                {
                    selected_dialogue.addNode(parent_node);
                    parent_node = null;
                }

                if (removed_node != null)
                {
                    selected_dialogue.removeNode(removed_node);
                    removed_node = null;
                }

                processEvents();
                scroll_position = EditorGUILayout.BeginScrollView(scroll_position);

                Rect canvas = GUILayoutUtility.GetRect(CANVAS_SIZE, CANVAS_SIZE);
                Texture2D texture = Resources.Load<Texture2D>("background");
                Rect coords = new Rect(0f, 0f, CANVAS_SIZE / BACKGROUND_SIZE, CANVAS_SIZE / BACKGROUND_SIZE);
                GUI.DrawTextureWithTexCoords(canvas, texture, coords);

                // ��ø�s Connection �Aø�s Node�A�i�קK Connection �e�� Node ���W
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
                        dragging_offset = dragged_node.getRect().position - Event.current.mousePosition;
                        Selection.activeObject = dragged_node;
                    }
                    else
                    {
                        is_canvas_dragged = true;
                        dragged_start_point = scroll_position + Event.current.mousePosition;
                        Selection.activeObject = selected_dialogue;
                    }

                    break;

                case EventType.MouseDrag:
                    if (dragged_node != null)
                    {
                        dragged_node.setPosition(position: Event.current.mousePosition + dragging_offset);
                        GUI.changed = true;
                    }
                    else if(is_canvas_dragged)
                    {
                        scroll_position = dragged_start_point - Event.current.mousePosition;
                        GUI.changed = true;
                    }

                    break;

                case EventType.MouseUp:                        
                    dragged_node = null;
                    is_canvas_dragged = false;
                    break;                
            }
        }

        private void drawNode(DialogueNode node)
        {
            if (node.isPlayerSpeaking())
            {
                GUILayout.BeginArea(screenRect: node.getRect(), style: player_style);
            }
            else
            {
                GUILayout.BeginArea(screenRect: node.getRect(), style: npc_style);
            }
            
            //EditorGUI.BeginChangeCheck();

            // ��s DialogueNode �� text
            node.setText(text: EditorGUILayout.TextField(node.getText()));

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

            else if (linking_parent_node.containChild(node.name))
            {
                if (GUILayout.Button("unlink"))
                {
                    linking_parent_node.removeChild(node.name);
                    linking_parent_node = null;
                }
            }

            else if (!node.name.Equals(linking_parent_node.name))
            {
                if (GUILayout.Button("child"))
                {
                    linking_parent_node.addChild(node.name);
                    linking_parent_node = null;
                }
            }

            // ��e node �Y�� linking_parent_node
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
            Vector3 start_position = new Vector3(node.getRect().xMax, node.getRect().center.y, 0f);
            Vector3 end_position, offset;

            foreach (DialogueNode child in selected_dialogue.getNodeChildren(root: node))
            {                
                end_position = new Vector3(child.getRect().xMin, child.getRect().center.y, 0f);
                offset = new Vector2((end_position.x - start_position.x) * 0.8f, 0f);

                Handles.DrawBezier(startPosition: start_position,
                                   endPosition: end_position,
                                   startTangent: start_position + offset,
                                   endTangent: end_position - offset,
                                   color: Color.white, 
                                   texture: null,
                                   width: 4f);
            }
        }

        // �}�� Editor ����
        [MenuItem("Window/Dialogue Editor")]
        public static void showEditorWindow()
        {
            // �ѦҺ��� 1: https://docs.unity3d.com/ScriptReference/EditorWindow.GetWindow.html
            //Debug.Log("showEditorWindow()");
            // utility: true �ӵ������|�Φ����ҡA�L�k�K�[��쥻���������A�ӬO�W�ߤ@�ӵ����C�q�`�Ω�ӥ\�ର�@���ʡA���|���ШϥΨ�
            // utility: false �|�Φ����ҡA�i�H�K�[��쥻���������C�q�`�Ω�ӥ\��|���ШϥΡC
            GetWindow(t: typeof(DialogueEditor), utility: false, title: "Dialogue Editor");
        }

        // OnOpenAssetAttribute -> using UnityEditor.Callbacks;
        // 1: OnOpenAssetAttribute �I�s����
        // �Ҧ� asset �I���᳣�|Ĳ�o���禡�A�]���ݭn�Q�� instance_id �����I������H�A�O�_�O�ڭ̭n�B�z�� asset�A�ê�^�O�_�i�H�B�z
        // �ѦҺ��� 1: https://docs.unity3d.com/ScriptReference/Callbacks.OnOpenAssetAttribute.html
        // �ѦҺ��� 2: https://docs.unity3d.com/ScriptReference/EditorUtility.InstanceIDToObject.html
        [OnOpenAsset(1)]
        public static bool onOpenAsset(int instance_id, int line)
        {
            // �Y�Q�� instance_id �Ҩ��o�� Object ��ڤW���O Dialogue�A�h�N�� null
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

