using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneManagement : MonoSingleton<SceneManagement>, IDataPerisistence
{
    [SerializeField] private DataSO _sceneData;
    

    public void LoadScene(bool isNextSceneLoaded)
    {
        if (isNextSceneLoaded)
        {
            _sceneData.sceneCount+=1;
        }
        
        Debug.Log(_sceneData.sceneCount);
        SceneManager.LoadScene(_sceneData.sceneCount);
    }


    public void LoadData(GameData data)
    {
        _sceneData.sceneCount = data.sceneCount;
    }

    public void SaveData(ref GameData data)
    {
        data.sceneCount = _sceneData.sceneCount;
    }

    public void TitleSceneLoad()
    {
        SceneManager.LoadScene(0);
    }
}
