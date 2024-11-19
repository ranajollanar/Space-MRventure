using UnityEngine;
using UnityEngine.UI;

public class ScreenToggle : MonoBehaviour
{
    public GameObject screen1;
    public GameObject screen2;

    public void SwitchToScreen1()
    {
        screen1.SetActive(true);
        screen2.SetActive(false);
    }

    public void SwitchToScreen2()
    {
        screen1.SetActive(false);
        screen2.SetActive(true);
    }
}
