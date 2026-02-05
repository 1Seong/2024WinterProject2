using UnityEngine;

public class CreditBGMStart : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AudioManager.instance.PlayCreditBGM();
    }
}
