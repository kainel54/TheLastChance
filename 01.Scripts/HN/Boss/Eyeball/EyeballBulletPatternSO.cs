using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/EyeballBoss/Bullet/Pattern")]
public class EyeballBulletPatternSO : ScriptableObject
{
    public List<float> speeds = new List<float>();
    public List<float> angles = new List<float>();
    public float regularAngleAdder; //�Ѿ˵��� ���� ���̰� ������ �� ���
    public float angleOffset; //��ü���� �Ѿ��� ���� �߰�
    public float sizeMultiplier;
    public float fireTerm;
}
