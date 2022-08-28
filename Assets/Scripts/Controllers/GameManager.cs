using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{

    public int coins;
    public int gems;
    public bool isGameStartedFirstTime;
    public bool[] levels;

    private GameData data;

    public int CurrentLevel { get; set; }

    protected override void Awake()
    {
        base.Awake();
        InitializeGameDate();
    }
    void Start()
    {
        CurrentLevel = 0;
    }


    public void LoadLevel(int index)
    {
        CurrentLevel = index;
        SceneManager.LoadScene("Game");
    }

    public void Save()
    {
        FileStream file = null;

        try
        {
            BinaryFormatter bf = new BinaryFormatter();
            file = File.Create(Application.persistentDataPath + "/GamaData.dat");
            if (data != null)
            {
                data.Coins = coins;
                data.Gems = gems;
                data.IsGameStartedFirstTime = isGameStartedFirstTime;
                data.Levels = levels;
                bf.Serialize(file, data);
            }
        }
#pragma warning disable CS0168 // Variable is declared but never used
        catch (Exception e)
#pragma warning restore CS0168 // Variable is declared but never used
        {

        }
        finally
        {
            if (file != null)
            {
                file.Close();
            }
        }
    }

    private void Load()
    {
        FileStream file = null;
        try
        {
            BinaryFormatter bf = new BinaryFormatter();
            file = File.Open(Application.persistentDataPath + "/GameData.dat", FileMode.Open);
            data = (GameData)bf.Deserialize(file);
        }
        catch (Exception e)
        {

        }
        finally
        {
            if (file != null)
            {
                file.Close();
            }
        }
    }

    void InitializeGameDate()
    {
        Load();
        if (data != null)
        {
            isGameStartedFirstTime = data.IsGameStartedFirstTime;
        }
        else
        {
            isGameStartedFirstTime = true;
        }

        if (isGameStartedFirstTime)
        {
            coins = 100;
            gems = 0;
            levels = new bool[5];


            isGameStartedFirstTime = false;

            levels[0] = true;

            for (int i = 1; i < levels.Length; i++)
            {
                levels[i] = false;
            }
            data = new GameData();

            data.Coins = coins;
            data.IsGameStartedFirstTime = isGameStartedFirstTime;
            data.Levels = levels;

            Save();

            Load();


        }
    }

}

[Serializable]
class GameData
{
    public int Coins { get; set; }
    public int Gems { get; set; }
    public bool IsGameStartedFirstTime { get; set; }
    public bool[] Levels { get; set; }
}
