using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static class Util
{
    public static float GetTimeMillis()
    {
        return Mathf.Round(Time.time*1000);
    }
}
