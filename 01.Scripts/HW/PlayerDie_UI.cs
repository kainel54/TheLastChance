using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerDie_UI : MonoBehaviour, IPlayerCompo
{
    private UIDocument _uiDocument;

    private VisualElement _fade;
    private VisualElement _diePanel;

    private Player _player;
    private PlayerHealth _playerHealth;

    private void Awake()
    {
        _uiDocument = GetComponent<UIDocument>();
    }
    public void Init(Player player)
    {
        _player = player;
    }


    
    private void OnEnable()
    {
        Time.timeScale = 1;

        var root = _uiDocument.rootVisualElement;

        _fade = root.Q("fade");
        _diePanel = root.Q("die-panel");

        var buttons = root.Query(className: "button").ToList();

        foreach (var button in buttons)
        {
            button.Q("button").pickingMode = PickingMode.Ignore;

            button.RegisterCallback<ClickEvent>(HandleSettingButtonClickEvent);

            button.RegisterCallback<PointerDownEvent>(HandleSettingButtonDownEvent);
            button.RegisterCallback<PointerUpEvent>(HandleSettingButtonUpvent);
            button.RegisterCallback<MouseLeaveEvent>(HandleSettingButtonUpvent);

        }

    }
    private void SettingButtonUp(VisualElement target)
    {
        if (target == null) return;

        var child = target.Q("button");

        child.RemoveFromClassList("button-clicked");
    }
    private void HandleSettingButtonUpvent(MouseLeaveEvent evt)
    {
        SettingButtonUp(evt.target as VisualElement);
    }

    private void HandleSettingButtonUpvent(PointerUpEvent evt)
    {
        SettingButtonUp(evt.target as VisualElement);
    }

    private void HandleSettingButtonDownEvent(PointerDownEvent evt)
    {
        var target = evt.target as VisualElement;

        var child = target.Q("button");

        child.AddToClassList("button-clicked");
    }


    private void HandleSettingButtonClickEvent(ClickEvent evt)
    {
        var target = evt.target as VisualElement;

        if (target.name == "retry-button")
        {
            SceneManagement.Instance.LoadScene(false);
        }

        if (target.name == "to-title-button")
        {
            SceneManagement.Instance.TitleSceneLoad();
        }
    }

    
    public void OnDeadEvent()
    {
        Debug.Log("»ç¸Á");
        DataPerisistenceManager.Instance.SaveGame();
        _diePanel.pickingMode = PickingMode.Position;
        _diePanel.AddToClassList("appear");
        _fade.AddToClassList("fade");
    }


}
