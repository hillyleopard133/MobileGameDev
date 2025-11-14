using TMPro;
using UnityEngine;


public class ButtonCounter: MonoBehaviour
{
    [SerializeField] TextMeshProUGUI counterText;
    private int counter;
    
    public void IncreaseCounter()
    {
        counter++;
        counterText.text = counter.ToString();
    }
}
