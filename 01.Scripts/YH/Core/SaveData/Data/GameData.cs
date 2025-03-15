using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int sceneCount;
    public int deathCount;

    public GameData()
    {
        this.sceneCount = 0;
        this.deathCount = 0;
    }
}
