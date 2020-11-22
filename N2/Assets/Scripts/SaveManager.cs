using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SaveManager", menuName = "Data/SaveManager")]
public class SaveManager : ScriptableObject
{
    [SerializeField] int _checkpoint;

    public void SetCheckpoint(int checkpoint)
    {
        if(checkpoint > _checkpoint)
            _checkpoint = checkpoint;
    }

    public void ResetCheckpoint()
    {
        _checkpoint = 0;
    }

    public int GetCheckpoint() => _checkpoint;

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
