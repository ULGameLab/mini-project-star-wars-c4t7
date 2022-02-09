using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceLevelStatus : MonoBehaviour
{
    private int ForceLvl;
    // Start is called before the first frame update
    void Start()
    {

    }
    int getForceLvl()
    {
        return ForceLvl;
    }
    void setForceLvl(int x)
    {
        ForceLvl = x;
    }
    void AddForceLvl(int x)
    {
        if (ForceLvl < 200)
            ForceLvl += x;
    }
    void SubsForceLvl(int x)
    {
        if (ForceLvl > 0)
            ForceLvl -= x;
    }
}
