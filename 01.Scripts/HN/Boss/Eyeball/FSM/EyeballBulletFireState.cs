using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeballBulletFireState : BossState
{
    protected int _bulletIndex;
    protected bool _isPatternEnded;
    protected List<EyeballBulletPatternSO> _bulletPatterns = new List<EyeballBulletPatternSO>();
    protected EyeballBoss _eyeballBoss;
    protected EyeBossStageSO _stageData;
    protected float _currentTime;

    public EyeballBulletFireState(Boss boss, BossStateMachine stateMachine, string animBoolName) : base(boss, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        _bulletIndex = 0;
        _isPatternEnded = false;

        _eyeballBoss??= _boss as EyeballBoss;

        _stageData = _eyeballBoss.GetStageData();
        _bulletPatterns = _stageData.bulletPatterns;
    }

    protected virtual void SpawnBulletAndFire()
    {
        _currentTime += Time.deltaTime;

        float fireTerm = _bulletIndex > 0 && !_isPatternEnded ? _bulletPatterns[_bulletIndex].fireTerm : 0.5f;

        if (!_isPatternEnded && _currentTime > fireTerm)
        {
            _eyeballBoss.OnBulletFireEvent?.Invoke();

            float angle = 0;
            float angleAdder = _bulletPatterns[_bulletIndex].regularAngleAdder;
            bool isRegularAngle = !Mathf.Approximately(angleAdder, 0);

            for (int i = 0; i < _bulletPatterns[_bulletIndex].speeds.Count; i++)
            {
                EyeballBullet bullet = PoolManager.Instance.Pop(ObjectPooling.PoolingType.EyeballBullet) as EyeballBullet;

                angle = isRegularAngle ? angle + angleAdder : _bulletPatterns[_bulletIndex].angles[i];
                bullet.transform.eulerAngles = new Vector3(0, 0, angle + _bulletPatterns[_bulletIndex].angleOffset);

                Vector2 firePos = new Vector2(_boss.transform.position.x - 0.14f, _boss.transform.position.y + 0.92f);

                bullet.SetData(_eyeballBoss.GetStageData(), _bulletIndex, i);
                bullet.InitializeAndFire(_boss.gameObject, firePos, bullet.transform.right);

                _currentTime = 0;
            }
            CameraManager.Instance.ShakeCam(1f, 2.5f);

            _bulletIndex++;

            _isPatternEnded = _bulletIndex >= _bulletPatterns.Count;

            if (_isPatternEnded) OnPatternEnded();
        }
    }

    protected virtual void OnPatternEnded()
    {

    }
}
