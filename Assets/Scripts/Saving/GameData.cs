using System;

[Serializable]
public class Data
{
    public bool[] isUnlock = new bool[25];
    public Data()
    {
        for (int i = 0; i < 25; i++)
                isUnlock[i] = false;
        isUnlock[0] = true;
    }
}