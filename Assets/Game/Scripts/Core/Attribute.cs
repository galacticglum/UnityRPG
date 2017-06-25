using System;
using UnityEngine;

public delegate void AttributeValueChangedEventHandler(object sender, AttributeEventArgs args);
public class AttributeEventArgs : EventArgs
{
    public readonly Attribute Attribute;
    public AttributeEventArgs(Attribute attribute)
    {
        Attribute = attribute;
    }
}

[CreateAssetMenu(fileName = "Attribute", menuName = "RPG/Attribute")]
public class Attribute : ScriptableObject
{
    public string Name { get { return name; } }
    public int MinimumValue { get { return minimumValue; } }
    public int MaximumValue { get { return maximumValue; } }
    public int Value { get; private set; }

    public event AttributeValueChangedEventHandler ValueChanged;
    private void OnValueChanged()
    {
        if (ValueChanged == null) return;
        ValueChanged(this, new AttributeEventArgs(this));
    }

    [SerializeField]
    private string name;
    [SerializeField]
    private int minimumValue;
    [SerializeField]
    private int maximumValue;
    [SerializeField]
    private bool defaultToMaximumValue = true;

    public void Initialize()
    {
        if (defaultToMaximumValue == false) return;
        Value = maximumValue;
    }

    public void Modify(int byValue)
    {
        Value += byValue;
        Value = Mathf.Clamp(Value, minimumValue, maximumValue);
        OnValueChanged();
    }
}
