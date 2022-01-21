using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Dialogue;
using TMPro;

namespace RPG.UI
{
    // �z�L PlayerConversant ������(Dialogue)�A�`�N���n�b���̿�� Dialogue
    public class DialogueUI : MonoBehaviour
    {
        PlayerConversant player_conversant;
        [SerializeField] TextMeshProUGUI ai_text;

        // Start is called before the first frame update
        void Start()
        {
            player_conversant = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerConversant>();
            ai_text.text = player_conversant.getText();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
