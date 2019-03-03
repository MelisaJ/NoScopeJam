﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[] LevelsGOS;
    public GameObject[] CheckPointsGOS;
    public GameObject PlayerPrefabGO;
    public GameObject PlayerCurrentGO;
    private GameObject CurrentLevelGO;
    public int StartLevelNumerator;
    private int CurrentLevelNumerator;
    public bool ShowAllLevels = false;
    public bool WorkingOnLevel = false;
    public bool StartFromMain = false;
    public static GameManager Instance;
    private void Awake()
    {
        if(Instance == null) {
            Instance = this;
        }else{
            Destroy(this.gameObject);
            return;
        }
        // Set Current Level
        CurrentLevelNumerator = StartLevelNumerator;
        // Load PlayerPref

        if ( !StartFromMain ) {
            NewGame();
        }
    }
    private void Start()
    {
        
    }
    private void Update()
    {
        
    }
    public void NewGame()
    {
        // SpawnLevels
        if ( !WorkingOnLevel ) {
            if ( ShowAllLevels ) {
                for ( int i = LevelsGOS.Length - 1; i > -1; i-- ) {
                    LoadLevel( i );
                }
            } else {
                LoadLevel( StartLevelNumerator );
            }
        }

        if (PlayerPrefabGO)
        {
            // SpawnPlayer
            PlayerCurrentGO = Instantiate(PlayerPrefabGO, CheckPointsGOS[StartLevelNumerator].transform.position,
                Quaternion.identity);
            
            // Deactivate Camera
            GameObject Camera = GameObject.FindGameObjectWithTag("MainCamera");
            Camera.SetActive(false);
        }
    }
    private void LoadLevel( int slot )
    {
        CurrentLevelNumerator = slot;
        Instantiate(LevelsGOS[ slot ]);
        AudioManager.Instance.SoundsEventTrigger(SoundEvents.BackGroundMusic, true);
    }
    private void DestoryLevel( float LifeTime, GameObject KillMe )
    {
        Destroy(KillMe, LifeTime);
    }
    public void SavePlayerStats(string slot)
    {
        // GameObject TempPlayer = GameObject.FindGameObjectWithTag("Player");
        string json = JsonUtility.ToJson(PlayerStats.Instance);
        
        PlayerPrefs.SetString(slot, json);
        // Check if string of slots is in there
        if( PlayerPrefs.HasKey("SlotsNames")) {
            Slots TempSlots = JsonUtility.FromJson<Slots>(PlayerPrefs.GetString("SlotsNames"));
            TempSlots.slots.Add(slot);
            json = JsonUtility.ToJson(TempSlots);
            PlayerPrefs.SetString("SlotsName", json);
        } else {
            Slots NSlots =  new Slots();
            NSlots.slots.Add(slot);
            json = JsonUtility.ToJson(NSlots);
            PlayerPrefs.SetString("SlotsName", json);
        }
    }
    public void LoadPlayerStats(string slot)
    {
        string json = PlayerPrefs.GetString(slot);
        PlayerStats.Instance = JsonUtility.FromJson<PlayerStats>(json);
    }
    public List<string> PlayerStatsKeys()
    {
        if( PlayerPrefs.HasKey("SlotsNames")) {
            Slots TempSlots = JsonUtility.FromJson<Slots>(PlayerPrefs.GetString("SlotsNames"));
            return TempSlots.slots;
        }
        return new List<string>{"null"};
    }

}
[System.Serializable]
public class Slots
{
    public List<string> slots;
}