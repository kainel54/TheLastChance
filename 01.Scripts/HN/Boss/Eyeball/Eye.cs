using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eye : MonoBehaviour
{
    [SerializeField] private Sprite _damagedSprite;
    [SerializeField] private FeedbackPlayer _damagedFeedback;

    public bool IsDamaged { get; private set; }

    private EyeballBoss _boss;
    private Animator _anim;
    private SpriteRenderer _renderer;
    private bool _isOpen;
    private int _openEyeHash = Animator.StringToHash("Open");
    private int _damagedHash = Animator.StringToHash("Damaged");

    public void Initialize(EyeballBoss boss)
    {
        _anim = GetComponent<Animator>();
        _renderer = GetComponent<SpriteRenderer>();
        _boss = boss;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(_isOpen && !IsDamaged && collision.TryGetComponent(out FireBall bullet))
        {
            _damagedFeedback.PlayFeedback();
            
            IsDamaged = true;

            if (_boss.AllEyesDamaged())
                _boss.ChanceTrigger();

            _boss.SetEyes(false);
            _anim.SetBool(_damagedHash, true);

            CameraManager.Instance.StopShake();
            CameraManager.Instance.ShakeCam(1.5f, 13f);

            if (_boss.CheckClosedEyes() >= 4) _boss.OnChanceCast();
        }
        //나중에 구현
    }

    public void SetEyeValue(bool open)
    {
        _isOpen = open;
        if (IsDamaged) _renderer.sprite = _damagedSprite;
        else _anim.SetBool(_openEyeHash, open);
    }
}
