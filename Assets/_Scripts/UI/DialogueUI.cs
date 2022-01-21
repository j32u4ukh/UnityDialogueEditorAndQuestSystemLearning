using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Dialogue;
using TMPro;
using UnityEngine.UI;

namespace RPG.UI
{
    // 透過 PlayerConversant 執行對話(Dialogue)，注意不要在此依賴到 Dialogue
    public class DialogueUI : MonoBehaviour
    {
        PlayerConversant player_conversant;
        [SerializeField] GameObject ai_response;
        [SerializeField] TextMeshProUGUI ai_text;
        [SerializeField] Button next_button;
        [SerializeField] Transform choices;
        [SerializeField] GameObject choice_prefab;

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
            bool is_choosing = player_conversant.isChoosing();
            ai_response.SetActive(!is_choosing);
            choices.gameObject.SetActive(is_choosing);

            if (is_choosing)
            {
                foreach (Transform choice in choices)
                {
                    Destroy(choice.gameObject);
                }

                foreach (DialogueNode choice in player_conversant.getChoices())
                {
                    GameObject obj = Instantiate(choice_prefab, choices);
                    obj.GetComponentInChildren<TextMeshProUGUI>().text = choice.getText();
                }
            }
            else
            {
                ai_text.text = player_conversant.getText();
                next_button.gameObject.SetActive(player_conversant.hasNext());
            }
        }
    }
}
