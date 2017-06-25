using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class Player : MonoBehaviour
{
    // Global player instance
    public static Player Current { get; private set; }

    public AttributeContainer Attributes { get { return attributes; } }
    public PlayerController Controller { get; private set; }

    [SerializeField]
    private AttributeContainer attributes = new AttributeContainer();

    private void OnEnable()
    {
        Current = this;
        Controller = GetComponent<PlayerController>();

        attributes.Initialize();
    }

    private void Update()
    {
        attributes.Update();
        if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            Attributes.Get("Health").Modify(Random.Range(1, 3000));
        }

        if (Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            Attributes.Get("Health").Modify(-Random.Range(1, 3000));
        }
    }
}
