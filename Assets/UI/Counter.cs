using System.Reflection;
using TMPro;
using UnityEngine;

public class CounterDisplay : MonoBehaviour
{
    public MonoBehaviour sourceScript;   
    public string variableName;           
    public TextMeshProUGUI valueText;     

    FieldInfo field;
    PropertyInfo property;

    void Start()
    {
        var t = sourceScript.GetType();
        var f = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
        field = t.GetField(variableName, f);
        if (field == null) property = t.GetProperty(variableName, f);
    }

    void Update()
    {
        object v = field != null ? field.GetValue(sourceScript)
            : property != null ? property.GetValue(sourceScript) : null;
        if (v != null) valueText.text = v.ToString();
    }
}