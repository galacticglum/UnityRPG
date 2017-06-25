using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class AttributeContainer : IEnumerable<Attribute>
{
    [SerializeField]
    private List<Attribute> attributes = new List<Attribute>();

    public void Initialize()
    {
        foreach (Attribute attribute in attributes)
        {
            attribute.Initialize();
        }
    }

    public bool Contains(Attribute attribute)
    {
        return attributes.Contains(attribute);
    }

    public Attribute Get(string name)
    {
        return attributes.FirstOrDefault(attribute => attribute.Name == name);
    }

    public IEnumerator<Attribute> GetEnumerator()
    {
        return attributes.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
