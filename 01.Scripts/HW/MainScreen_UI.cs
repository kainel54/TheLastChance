using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;


public class MainScreen_UI : MonoBehaviour
{
    private UIDocument _uiDocument;
    private VisualElement _settingContainer;
    private VisualElement _loadContainer;

    private Label _title;
    private Label _start;
    private Label _setting;
    private Label _exit;

    private bool _isAddClass = true;

    private bool _anyPanelActived = false;

    private void Awake()
    {
        _uiDocument = GetComponent<UIDocument>();
        if (_uiDocument == null)
        {
            Debug.LogError("UIDocument 컴포넌트를 찾을 수 없습니다.");
        }
    }


    private void OnEnable()
    {
        Time.timeScale = 1;

        var root = _uiDocument.rootVisualElement;
        _settingContainer = root.Q("setting-container");
        _loadContainer = root.Q("load-container");
        _title = root.Q<Label>("title");
        _start = root.Q<Label>("start");
        _setting = root.Q<Label>("setting");
        _exit = root.Q<Label>("exit");

        var buttons = root.Query(className: "button").ToList();
        var exitButtons = root.Query("exit").ToList();

        StartCoroutine( InitializeClass());


        _start.RegisterCallback<ClickEvent>(HandleMainScreenClickEvent);
        _setting.RegisterCallback<ClickEvent>(HandleMainScreenClickEvent);
        _exit.RegisterCallback<ClickEvent>(HandleMainScreenClickEvent);

        _start.RegisterCallback<ClickEvent>(evt =>
        {
            //_fade.style.display = DisplayStyle.Flex;
            //_fade.style.backgroundColor = new StyleColor(new Color(0, 0, 0, 1));
            _loadContainer.AddToClassList("appear");
        });

        _setting.RegisterCallback<ClickEvent>(evt =>
        {
            //개별적으로 할 부분
            _settingContainer.AddToClassList("appear");
        });

        _exit.RegisterCallback<ClickEvent>(evt =>
        {
            Application.Quit(0);
        });

        foreach (var button in buttons)
        {
            button.Q("button").pickingMode = PickingMode.Ignore;

            button.RegisterCallback<ClickEvent>(HandleSettingButtonClickEvent);

            button.RegisterCallback<PointerDownEvent>(HandleSettingButtonDownEvent);
            button.RegisterCallback<PointerUpEvent>(HandleSettingButtonUpEvent);
            button.RegisterCallback<MouseLeaveEvent>(HandleSettingButtonUpEvent);
            button.RegisterCallback<MouseEnterEvent>(HandleSettingButtonEnterEvent);

        }

        foreach (var exitBtn in exitButtons)
        {
            exitBtn.RegisterCallback<ClickEvent>(HandleSettingExitButtonClickEvent);
        }


    }

    private void HandleSettingButtonEnterEvent(MouseEnterEvent evt)
    {
        var target = evt.target as VisualElement;

        var label = target.Q<Label>();

        label.AddToClassList("hover");
    }

    private void HandleSettingExitButtonClickEvent(ClickEvent evt)
    {
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
    private void HandleSettingButtonUpEvent(MouseLeaveEvent evt)
    {

        var target = evt.target as VisualElement;
        SettingButtonUp(target);

        var label = target.Q<Label>();
        label.RemoveFromClassList("hover");


    }

    private void HandleSettingButtonUpEvent(PointerUpEvent evt)
    {
        SettingButtonUp(evt.target as VisualElement);
    }
    private void HandleSettingButtonClickEvent(ClickEvent evt)
    {
        var target = evt.target as VisualElement;

        if (target.name == "load-button")
        {
            DataPerisistenceManager.Instance.LoadGame();
            SceneManagement.Instance.LoadScene(false);
            DataPerisistenceManager.Instance.SaveGame();
        }

        if (target.name == "new-game-button")
        {
            DataPerisistenceManager.Instance.NewGame();
            SceneManagement.Instance.LoadScene(true);
            DataPerisistenceManager.Instance.SaveGame();
        }

        if (target.name == "game-exit-button")
        {
            Application.Quit(0);
            Debug.Log("나가기");
        }

    }


    private void HandleSettingButtonDownEvent(PointerDownEvent evt)
    {
        var target = evt.target as VisualElement;

        var child = target.Q("button");

        child.AddToClassList("button-clicked");
    }

    private void HandleMainScreenClickEvent(ClickEvent evt)
    {
        Label label = evt.target as Label;

        //RotateLabel(label);
    }

    private void RotateLabel(Label label)
    {
        label.ToggleInClassList("clicked");

        _isAddClass = !_isAddClass;
    }

    private IEnumerator InitializeClass()
    {
        Debug.Log("시작화면 클래스 제거하고");
        _title.RemoveFromClassList("appear");
        _start.RemoveFromClassList("appear");
        _setting.RemoveFromClassList("appear");
        _exit.RemoveFromClassList("appear");

        Debug.Log("곧 0.1초뒤에 나타날 예정");
        float elapsedTime = 0f;
        while (elapsedTime < 0.1f)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Debug.Log("0.1초뒤에 추가");

        _title.AddToClassList("appear");
        _start.AddToClassList("appear");
        _setting.AddToClassList("appear");
        _exit.AddToClassList("appear");
    }

    private void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            _settingContainer.ToggleInClassList("appear");
        }
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
}
