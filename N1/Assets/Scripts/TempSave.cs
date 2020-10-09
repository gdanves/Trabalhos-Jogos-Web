using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TempSave : MonoBehaviour
{
    [SerializeField] SaveManager _saveManager;

    void Start()
    {
        if(_saveManager)
            _saveManager.LoadData();
    }

    private void OnDestroy()
    {
        if(_saveManager)
            _saveManager.SaveData();
    }
}
