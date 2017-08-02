using UnityEngine;

[CreateAssetMenu(fileName = "Element", menuName = "RPG/Element")]
public class Element : ScriptableObject
{
    [SerializeField]
    private new string name;
    [SerializeField]
    private Element[] weaknesses;
    [SerializeField]
    private Element[] resistances;
}
