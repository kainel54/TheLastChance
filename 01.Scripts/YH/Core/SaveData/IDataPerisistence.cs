using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDataPerisistence
{
    public void LoadData(GameData data);
    public void SaveData(ref GameData data);
}
