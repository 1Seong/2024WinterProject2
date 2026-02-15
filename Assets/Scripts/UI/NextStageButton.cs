using UnityEngine;
using UnityEngine.UI;

public class NextStageButton : MonoBehaviour
{
    public Button button;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.N))
            button.onClick.Invoke();
    }
}
