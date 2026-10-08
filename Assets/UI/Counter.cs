using TMPro;
using UnityEngine;

public class CounterDisplay : MonoBehaviour
{
    public TextMeshProUGUI valueText;   // le texte à changer
    public Recorder source;            // remplace TonScript par le vrai nom de ta classe

    void Update()
    {
        valueText.text = source.clonesRemain.ToString();   // remplace tavariable par ta variable
    }
}