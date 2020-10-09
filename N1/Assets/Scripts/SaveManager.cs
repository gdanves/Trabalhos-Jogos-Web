using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SaveManager", menuName = "Data/SaveManager")]
public class SaveManager : ScriptableObject
{
    [SerializeField] int _checkpoint;

    public int GetCheckpoint() => _checkpoint;

    public int AddCheckpoint() => ++_checkpoint;

    public void LoadData()
    {
        if (PlayerPrefs.HasKey("Checkpoint"))
            _checkpoint = PlayerPrefs.GetInt("Checkpoint");
    }

    public void SaveData()
    {
        PlayerPrefs.SetInt("Checkpoint", _checkpoint);        
    }

    private void OnEnable()
    {
        LoadData();
    }

    private void OnDisable()
    {
        SaveData();
    }
}
