using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGolemIState
{
    public void OnEnterState();
    public void OnExiState();
    public void UpdateState();
}
