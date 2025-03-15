using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Clear_UI : MonoBehaviour
{
    private UIDocument _uiDocument;

    private VisualElement _clearPanel;

    private void Awake()
    {
        _uiDocument = GetComponent<UIDocument>();
    }

    private void OnEnable()
    {
        Time.timeScale = 1;

        var root = _uiDocument.rootVisualElement;

        _clearPanel = root.Q("clear-panel");
        _clearPanel.pickingMode = PickingMode.Ignore;

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

        if (target.name == "game-exit-button")
        {
            Application.Quit(0);
        }
    }



    public void Clear()
    {
        _clearPanel.pickingMode = PickingMode.Position;
        DataPerisistenceManager.Instance.SaveGame();
        _clearPanel.AddToClassList("appear");
    }
}
