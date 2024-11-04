using UnityEngine;

namespace ModularEventArchitecture
{
    public class Player : UnitEntity
    {
        public override void Use()
        {
            
        }

        public void InteractWithNPC(NPC npc)
        {
            npc.InteractWithPlayer(this);
        }

        public void ReceiveQuest(Quest quest)
        {
            QuestManager.Instance.AddQuest(quest);
            Debug.Log($"Получен новый квест: {quest.Title}");
        }

        public void InteractWithQuestObject(string questTitle, string objectiveDescription, int amount)
        {
            QuestManager.Instance.UpdateQuestProgress(questTitle, objectiveDescription, amount);
        }
    }
}