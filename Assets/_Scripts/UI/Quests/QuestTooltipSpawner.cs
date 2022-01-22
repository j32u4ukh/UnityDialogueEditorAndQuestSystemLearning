using GameDevTV.Core.UI.Tooltips;
using RPG.Quests;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.UI.Quests
{
    public class QuestTooltipSpawner : TooltipSpawner
    {
        public override bool CanCreateTooltip()
        {
            return true;
        }

        public override void UpdateTooltip(GameObject tooltip)
        {
            QuestStatus status = GetComponent<QuestItemUI>().getQuestStatus();
            tooltip.GetComponent<QuestTooltipUI>().setUp(status: status);
        }
    }
}
