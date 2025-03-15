using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/EyeballBoss/StageData")]
public class EyeBossStageSO : ScriptableObject
{
    [Header("LaserSetting")]
    public float laserSpeed;

    [Header("BulletSetting")]
    public float fireTerm = 0.1f;
    public float bulletSpeed = 5;
    public Vector2 bulletSize = new Vector2(1.3f, 1.3f);
    public List<EyeballBulletPatternSO> bulletPatterns = new List<EyeballBulletPatternSO>();

    [Header("BlindSetting")]
    public float blindTime;
    public float blindLaserTerm;
    public float laserCastSpeed;
    public int blindLaserCount;
}
