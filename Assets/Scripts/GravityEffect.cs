using UnityEngine;

public class GravityEffect : MonoBehaviour
{
    public bool isBlue = false;

    private Transparent transparent;
    private RotateTransparent rotTransparent;

    private void Awake()
    {
        transparent = GetComponent<Transparent>();
        rotTransparent = GetComponent<RotateTransparent>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (isBlue)
        {
            GPause.GpauseBlueEvent += transparent.CallFade;
            //GPause.GpauseBlueEvent += rotTransparent.StopCallFade;
        }
        else
        {
            GPause.GpausePinkEvent += transparent.CallFade;
            //GPause.GpausePinkEvent += rotTransparent.StopCallFade;
        }

        GameManager.instance.GPauseOffEvent += transparent.CallEmerge;
        //GameManager.instance.GPauseOffEvent += rotTransparent.DoCallFade;
    }

    private void OnDestroy()
    {
        if (isBlue)
        {
            GPause.GpauseBlueEvent -= transparent.CallFade;
            //GPause.GpauseBlueEvent -= rotTransparent.StopCallFade;
        }
        else
        {
            GPause.GpausePinkEvent -= transparent.CallFade;
            //GPause.GpausePinkEvent -= rotTransparent.StopCallFade;
        }

        GameManager.instance.GPauseOffEvent -= transparent.CallEmerge;
        //GameManager.instance.GPauseOffEvent -= rotTransparent.DoCallFade;
    }

}
