using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Dialogue;
using TMPro;
using UnityEngine.UI;

namespace RPG.UI
{
    // �z�L PlayerConversant ������(Dialogue)�A�`�N���n�b���̿�� Dialogue
    public class DialogueUI : MonoBehaviour
    {
        PlayerConversant player_conversant;
        [SerializeField] TextMeshProUGUI ai_text;
        [SerializeField] Button next_button;

        // Start is called before the first frame update
        void Start()
        {
            player_conversant = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerConversant>();
            next_button.onClick.AddListener(next);
            updateUI();
        }

        void next()
        {
            player_conversant.next();
            updateUI();
        }

        // Update is called once per frame
        void updateUI()
        {
            ai_text.text = player_conversant.getText();

            if (!player_conversant.hasNext())
            {
                next_button.gameObject.SetActive(false);
            }
        }
    }
}
