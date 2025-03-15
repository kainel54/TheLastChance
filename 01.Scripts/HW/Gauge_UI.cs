using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Gauge_UI : MonoBehaviour
{
    private UIDocument _uiDocument;

    [SerializeField] private Player _player;

    private float yOffset = -100;

    private Vector2 _position;

    private VisualElement _gaugeBar;

    private VisualElement _gauge;

    private void Awake()
    {
        _uiDocument = GetComponent<UIDocument>();
    }
    private void Update()
    {
        _position = _player.transform.position;

        Vector2 screenPosition = Camera.main.WorldToScreenPoint(_position);

        _gaugeBar.style.left = screenPosition.x - 960;
        _gaugeBar.style.bottom = screenPosition.y - 540 + yOffset;
    }

    private void OnEnable()
    {
        var root = _uiDocument.rootVisualElement.Q("gauge-container");

        _gaugeBar = root.Q("gauge");
    }
}
