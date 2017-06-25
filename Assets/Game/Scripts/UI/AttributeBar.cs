using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class AttributeBar : MonoBehaviour
{
    [SerializeField]
    private string attributeName;
    [SerializeField]
    private Text text;
    private Slider slider;

	// Use this for initialization
	private void Start ()
	{
	    slider = GetComponent<Slider>();
	    Attribute attribute = Player.Current.Attributes.Get(attributeName);
	    if (attribute == null) return;
	    attribute.ValueChanged += OnValueChanged;
        // Force trigger our ValueChanged event so it can update the UI (for the first time).
        OnValueChanged(this, new AttributeEventArgs(attribute));
	}

    private void OnValueChanged(object sender, AttributeEventArgs args)
    {
        if (text == null) return;
        text.text = string.Format("{0} / {1}", args.Attribute.Value, args.Attribute.MaximumValue);
        slider.value = args.Attribute.Value / (float) args.Attribute.MaximumValue;
    }
}
