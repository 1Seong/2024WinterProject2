using UnityEngine;
using UnityEngine.UI;

public class TitelPanel : MonoBehaviour
{
    public Sprite u, d, l, r;
    public Sprite uPressed, dPressed, lPressed, rPressed;
    void Start()
    {
        var imgs = GetComponentInChildren<Image>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
