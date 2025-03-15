using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/EyeballBoss/Bullet/Pattern")]
public class EyeballBulletPatternSO : ScriptableObject
{
    public List<float> speeds = new List<float>();
    public List<float> angles = new List<float>();
    public float regularAngleAdder; //총알들의 각도 차이가 일정할 때 사용
    public float angleOffset; //전체적인 총알의 각도 추가
    public float sizeMultiplier;
    public float fireTerm;
}
