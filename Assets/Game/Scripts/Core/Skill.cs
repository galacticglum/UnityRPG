using UnityEngine;

[CreateAssetMenu(fileName = "Skill", menuName = "RPG/Skill")]
public class Skill : ScriptableObject
{
    [Header("General")]
    [SerializeField]
    private new string name;
    [SerializeField]
    private string description;

    [Header("Skill")]
    [SerializeField]
    private Element element;
    [SerializeField]
    private SkillType skillType;
    [SerializeField]
    private DamageType damageType;

    [Header("Stats")]
    [SerializeField]
    private float damage;
    [SerializeField]
    private float manaCost;
    [Range(0, 1)]
    [SerializeField]
    private float criticalChance;
    [SerializeField]
    private int accuracy;
    [SerializeField]
    private float cooldown;
    [SerializeField]
    private float castingTime;
    [SerializeField]
    private float damagePerTick;
    [SerializeField]
    private float tickFrequency;
    [SerializeField]
    private float duration;
    [SerializeField]

    [Header("Skill Type Specific")]
    private float range; // Projectile, Selected Enemy, or Selected Area.
    [SerializeField]
    private float projectileSpeed; // Projectile
    [SerializeField]
    private float areaRadius; // Selected Area or Area Around Self.
}
