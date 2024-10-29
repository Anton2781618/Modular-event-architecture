using UnityEngine;

[CreateAssetMenu(fileName = "New NPC Data", menuName = "Game Data/NPC Data")]
public class NPCData : ScriptableObject
{
    public string npcName;
    public float health = 100f;
    public float speed = 3f;
    public float interactionRange = 2f;
    public float detectionRange = 10f;

    [TextArea(3, 10)]
    public string description;

    // Дополнительные параметры для настройки поведения NPC
    public bool isAggressive = false;
    public float attackDamage = 10f;
    public float attackCooldown = 1f;

    // Можно добавить другие параметры, специфичные для вашей игры
    // Например:
    // public int level;
    // public List<Item> inventory;
    // public DialogueTree dialogueTree;
}
