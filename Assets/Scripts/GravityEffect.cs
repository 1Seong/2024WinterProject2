using UnityEngine;

public class GravityEffect : MonoBehaviour
{
    public bool isBlue = false;

    private Transparent transparent;

    private void Awake()
    {
        transparent = GetComponent<Transparent>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (isBlue)
            GPause.GpauseBlueEvent += transparent.CallFade;
        else
            GPause.GpausePinkEvent += transparent.CallFade;

        GameManager.instance.GPauseOffEvent += transparent.CallEmerge;
    }

    private void OnDestroy()
    {
        if (isBlue)
            GPause.GpauseBlueEvent -= transparent.CallFade;
        else
            GPause.GpausePinkEvent -= transparent.CallFade;

        GameManager.instance.GPauseOffEvent -= transparent.CallEmerge;
    }

    
}
