using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class WoolProgressBar : MonoBehaviour
{
    [Header("Source")]
    public MonoBehaviour sourceScript;
    public string variableName = "timer";
    public float maxValue = 20f;
    public bool reverse = true;           

    [Header("Test")]
    public bool useTestTimer = true;       
    public float testDuration = 10f;

    [Header("UI (mêmes parents)")]
    public Image fill;                     
    public RectTransform ball;
    Vector3[] corners = new Vector3[4];

    [Header("Rotation")]
    public float spinSpeed = 40f;
    public float ballOffsetY = 20f;

    FieldInfo field;
    PropertyInfo property;
    float angle;

    void Start()
    {
        fill.type = Image.Type.Filled;
        fill.fillMethod = Image.FillMethod.Horizontal;
        fill.fillOrigin = 0;
        ball.SetAsLastSibling();

        if (!useTestTimer)
        {
            if (sourceScript == null) { Debug.LogError("sourceScript non assigné"); return; }
            var t = sourceScript.GetType();
            var f = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            field = t.GetField(variableName, f);
            if (field == null) property = t.GetProperty(variableName, f);
            if (field == null && property == null)
                Debug.LogError("Variable '" + variableName + "' introuvable sur " + t.Name);
        }
    }

    float GetValue()
    {
        if (useTestTimer) return Mathf.PingPong(Time.time, testDuration) / testDuration * maxValue;
        object v = field != null ? field.GetValue(sourceScript)
                 : property != null ? property.GetValue(sourceScript) : null;
        return v == null ? 0f : System.Convert.ToSingle(v);
    }

    void Update()
    {
        float p = Mathf.Clamp01(GetValue() / maxValue);
        float amount = reverse ? 1f - p : p;

        fill.fillAmount = amount;

        fill.rectTransform.GetWorldCorners(corners);
        Vector3 leftMid  = (corners[0] + corners[1]) * 0.5f;   
        Vector3 rightMid = (corners[2] + corners[3]) * 0.5f;   

        Vector3 pos = Vector3.Lerp(leftMid, rightMid, amount);
        pos += ball.parent.up * (ballOffsetY * ball.lossyScale.y);
        ball.position = pos;

        angle += spinSpeed * Time.unscaledDeltaTime;
        ball.localRotation = Quaternion.Euler(0f, 0f, -angle);
    }
}