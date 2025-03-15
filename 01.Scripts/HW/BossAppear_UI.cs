using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class BossAppear_UI : MonoBehaviour
{
    private UIDocument _uiDocument;

    private List<TimeValue> _beforeTimeV = new List<TimeValue> { new TimeValue(0.75f) };
    private List<TimeValue> _afterTimeV = new List<TimeValue> { new TimeValue(1.7f) };

    private VisualElement _window;
    private VisualElement _topPanel;
    private VisualElement _bottomPanel;
    private VisualElement _background;

    private VisualElement _bossVisual;
    private Label _bossLabel;

    [SerializeField] private Sprite _bossSprite;
    [SerializeField] private string _bossText;

    private void Awake()
    {
        _uiDocument = GetComponent<UIDocument>();
    }

    private void OnEnable()
    {
        var root = _uiDocument.rootVisualElement;

        _bossVisual = root.Q("boss-visual");
        _bossLabel = root.Q<Label>("boss-label");

        float width = _bossSprite.bounds.size.x;
        float height = _bossSprite.bounds.size.y;   

        _bossVisual.style.backgroundImage = new StyleBackground(_bossSprite);
        _bossVisual.style.width = width * 800;
        _bossVisual.style.height = height * 800;
        _bossLabel.text = _bossText;

        _window = root.Q("window");
        _topPanel = root.Q("top");
        _bottomPanel = root.Q("bottom");
        _background = root.Q("back");

    }

    public void OnBossAppearUI()
    {
        StartCoroutine(BossAppear());
    }

    IEnumerator BossAppear()
    {
        _window.style.transitionDuration =
        _topPanel.style.transitionDuration =
        _bottomPanel.style.transitionDuration =
        _background.style.transitionDuration = _beforeTimeV;

        _window.AddToClassList("window-appear");
        _topPanel.AddToClassList("panel-appear");
        _bottomPanel.AddToClassList("panel-appear");
        _background.AddToClassList("background-appear");

        yield return new WaitForSeconds(2.5f);

        _window.style.transitionDuration =
        _topPanel.style.transitionDuration =
        _bottomPanel.style.transitionDuration =
        _background.style.transitionDuration = _afterTimeV;

        _window.RemoveFromClassList("window-appear");
        _topPanel.RemoveFromClassList("panel-appear");
        _bottomPanel.RemoveFromClassList("panel-appear");
        _background.RemoveFromClassList("background-appear");
    }
}
