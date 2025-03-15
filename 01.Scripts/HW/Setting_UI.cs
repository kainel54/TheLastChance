using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class Setting_UI : MonoBehaviour
{
    private UIDocument _uiDocument;

    private VisualElement _settingContainer;

    private void Awake()
    {
        _uiDocument = GetComponent<UIDocument>();
    }

    private void OnEnable()
    {
        Time.timeScale = 1;
        var root = _uiDocument.rootVisualElement;

        _settingContainer = root.Q("setting-container");
        _settingContainer.pickingMode = PickingMode.Ignore;

        var buttons = root.Query(className: "button").ToList();
        var exitButtons = root.Query("exit").ToList();


        foreach (var button in buttons)
        {
            button.Q("button").pickingMode = PickingMode.Ignore;

            button.RegisterCallback<ClickEvent>(HandleSettingButtonClickEvent);

            button.RegisterCallback<PointerDownEvent>(HandleSettingButtonDownEvent);
            button.RegisterCallback<PointerUpEvent>(HandleSettingButtonUpvent);
            button.RegisterCallback<MouseLeaveEvent>(HandleSettingButtonUpvent);

        }

        foreach (var exitBtn in exitButtons)
        {
            exitBtn.RegisterCallback<ClickEvent>(HandleSettingExitButtonClickEvent);
        }

    }

    private void Update()
    {
        SettingUI();
    }

    private VisualElement FindParentByContainedName(VisualElement child, string containedName)
    {
        var currentParent = child.parent;

        while (currentParent != null)
        {
            if (currentParent.name.Contains(containedName))
                return currentParent;
            currentParent = currentParent.parent;
        }
        return null;
    }

    private void HandleSettingExitButtonClickEvent(ClickEvent evt)
    {
        Time.timeScale = 1;
        var target = evt.target as VisualElement;

        var parent = FindParentByContainedName(target, "container");

        parent.RemoveFromClassList("appear");
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

        if (target.name == "next-button")
        {
            SceneManagement.Instance.LoadScene(true);
            DataPerisistenceManager.Instance.SaveGame();
        }

        if (target.name == "to-title-button")
        {
            SceneManagement.Instance.TitleSceneLoad();
            DataPerisistenceManager.Instance.SaveGame();
        }
    }

    private void SettingUI()
    {
        if (_settingContainer != null)
        {
            if (Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                if (_settingContainer.ClassListContains("appear"))
                    Time.timeScale = 1;
                else
                    Time.timeScale = 0;

                _settingContainer.ToggleInClassList("appear");
            }
        }
    }
}
