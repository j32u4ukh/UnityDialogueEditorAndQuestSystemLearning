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
        [NonSerialized] GUIStyle node_style;

        [NonSerialized] DialogueNode dragged_node = null;
        [NonSerialized] Vector2 dragging_offset;

        [NonSerialized] DialogueNode parent_node = null;
        [NonSerialized] DialogueNode removed_node = null;
        [NonSerialized] DialogueNode linking_parent_node = null;

        Vector2 scroll_position = Vector2.zero;
        float width = 0f;
        float height = 0f;

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
            // �ѦҺ��� 1: https://docs.unity3d.com/ScriptReference/EditorGUILayout.html
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

                // TODO: �ثe�L�k�R���a���l�`�I���`�I
                if(removed_node != null)
                {
                    Undo.RecordObject(selected_dialogue, "Remove Dialogue Node.");
                    selected_dialogue.removeNode(removed_node);
                    removed_node = null;
                }

                scroll_position = EditorGUILayout.BeginScrollView(scroll_position);
                processEvents();
                width = 0f;
                height = 0f;

                // ��ø�s Connection �Aø�s Node�A�i�קK Connection �e�� Node ���W
                foreach (DialogueNode node in selected_dialogue.getAllNodes())
                {
                    drawConnection(node);
                }

                foreach (DialogueNode node in selected_dialogue.getAllNodes())
                {
                    drawNode(node);
                }

                GUILayoutUtility.GetRect(width + 10f, height + 10f);
                Debug.Log($"(width, height) = ({width + 10f}, {height + 10f})");
                EditorGUILayout.EndScrollView();
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
            width = Mathf.Max(width, node.rect.xMax);
            height = Mathf.Max(height, node.rect.yMax);
            EditorGUI.BeginChangeCheck();

            string new_text = EditorGUILayout.TextField(node.text);

            if (EditorGUI.EndChangeCheck())
            {
                // �����ק���{�A�è��N EditorUtility.SetDirty(selected_dialogue); �N selected_dialogue �]�� Dirty�A
                // �i�D Unity �o���ɮפw�Q�ק�A�n��s selected_dialogue ���ƾڡA�Ӥ��u����s Inspector
                // �b��ڭק� selected_dialogue �e�I�s�A�~��^��̪쪺���A
                Undo.RecordObject(selected_dialogue, "Update Dialogue");

                // ��s DialogueNode �� text
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

            else if (linking_parent_node.children.Contains(node.unique_id))
            {
                if (GUILayout.Button("unlink"))
                {
                    Undo.RecordObject(selected_dialogue, "Remove Dialogue Link");
                    linking_parent_node.children.Remove(node.unique_id);
                    linking_parent_node = null;
                }
            }

            else if (!node.unique_id.Equals(linking_parent_node.unique_id))
            {
                if (GUILayout.Button("child"))
                {
                    Undo.RecordObject(selected_dialogue, "Add Dialogue Link");
                    linking_parent_node.children.Add(node.unique_id);
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

