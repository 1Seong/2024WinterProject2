using UnityEngine;

public class HintButtonLinebreak : HintButton
{
    public float[] FontSteps = { 55f, 50f, 45f };

    protected override void Start()
    {
        base.Start();
        LangSwitchCallback();
    }

    public override void LangSwitchCallback()
    {
        for (int i = 0; i < FontSteps.Length; i++)
        {
            tmp.fontSize = FontSteps[i];
            tmp.ForceMeshUpdate(true, true);

            if (tmp.textInfo.lineCount <= 1)
                return;
        }

        tmp.fontSize = FontSteps[^1];
    }
}
