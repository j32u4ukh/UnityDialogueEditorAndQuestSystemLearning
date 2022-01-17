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
            // �ѦҺ��� 1: https://docs.unity3d.com/ScriptReference/EditorGUILayout.html
            if(selected_dialogue == null)
            {
                EditorGUILayout.LabelField("No Dialogue Selected.");
            }
            else
            {
                EditorGUILayout.LabelField($"Dialogue({selected_dialogue.name})");
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

