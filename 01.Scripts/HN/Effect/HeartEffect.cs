using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartEffect : Effect
{
    [SerializeField] private float _delay;

    private List<Color> _colors = new List<Color>();
    private int _index;

    public void SetColors(List<Color> colors)
    {
        if (colors.Count == 1) return;

        _colors.Clear();

        for(int i = 0; i < colors.Count; i++)
        {
            _colors.Add(colors[i]);
        }

        SetIndexAndChangeColor();
    }

    private void SetIndexAndChangeColor()
    {
        ChangeColor(_colors[_index], _delay, () =>
        {
            _index++;

            if (_index == _colors.Count)
                _index = 0;

            SetIndexAndChangeColor();
        });
    }
}
