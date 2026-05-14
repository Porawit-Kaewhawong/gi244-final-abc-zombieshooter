using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public int bestWave;
    public int bestKillCount;
    public float bestSurviveTime;
}

[System.Serializable]
public class Wave
{
    public int totalEnemies;
    public float delayStart;
    public float spawnInterval;
    public int numberOfPowerUp;
}

public class GameManager : MonoBehaviour
{
    public List<Wave> waves;
    public GameObject enemyPrefab;
    public GameObject[] powerUps;
    public Transform[] spawnPoints;
    public Transform powerUpSpawnArea;

    [Header("Current Run UI")]
    public TextMeshProUGUI waveText;
    public TextMeshProUGUI surviveTimeText;
    public TextMeshProUGUI countdownText;
    public TextMeshProUGUI currentKillsText;

    [Header("High Score UI")]
    public TextMeshProUGUI bestWaveText;
    public TextMeshProUGUI bestKillsText;
    public TextMeshProUGUI bestTimeText;

    private int currentWaveIndex;
    private int enemyKillCount;
    private float surviveTime;
    private string savePath;

    private int bestWave;
    private int bestKillCount;
    private float bestSurviveTime;

    public static GameManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        savePath = Application.persistentDataPath + "/highscores.json";
        LoadHighScores();
        UpdateHighScoreUI();

        StartCoroutine(WaveControl());
    }

    private void Update()
    {
        surviveTime += Time.deltaTime;

        if (surviveTimeText != null)
        {
            surviveTimeText.text = "Survive Time: " + surviveTime.ToString("F1") + "s";
        }
    }

    private IEnumerator WaveControl()
    {
        for (int i = 0; i < waves.Count; i++)
        {
            currentWaveIndex = i + 1;
            yield return StartCoroutine(WaveSpawn(i));
        }
    }

    private IEnumerator WaveSpawn(int waveId)
    {
        Wave currentWave = waves[waveId];
        float time = currentWave.delayStart;

        while (time > 0)
        {
            countdownText.text = time.ToString("F1") + "s before next wave";
            yield return new WaitForSeconds(0.1f);
            time -= 0.1f;
        }

        countdownText.text = "";
        waveText.text = "Wave " + (waveId + 1);

        for (int i = 0; i < currentWave.numberOfPowerUp; i++)
        {
            float spawnRange = powerUpSpawnArea.localScale.x * 5f;
            Vector3 randomPosition = new Vector3(
                Random.Range(-spawnRange, spawnRange),
                0.5f,
                Random.Range(-spawnRange, spawnRange));

            Instantiate(powerUps[Random.Range(0, powerUps.Length)], powerUpSpawnArea.position + randomPosition, Quaternion.identity);
        }

        for (int i = 0; i < currentWave.totalEnemies; i++)
        {
            int random = Random.Range(0, spawnPoints.Length);
            Instantiate(enemyPrefab, spawnPoints[random].position + Vector3.up * 0.5f, Quaternion.identity);
            yield return new WaitForSeconds(currentWave.spawnInterval);
        }
    }

    public void AddKill()
    {
        enemyKillCount++;
        currentKillsText.text = "Kills: " + enemyKillCount;
    }

    public void SaveHighScores()
    {
        if (currentWaveIndex > bestWave) bestWave = currentWaveIndex;
        if (enemyKillCount > bestKillCount) bestKillCount = enemyKillCount;
        if (surviveTime > bestSurviveTime) bestSurviveTime = surviveTime;

        SaveData data = new SaveData
        {
            bestWave = bestWave,
            bestKillCount = bestKillCount,
            bestSurviveTime = bestSurviveTime
        };

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);

        UpdateHighScoreUI();
        Debug.Log("High Scores Updated");
    }

    public void LoadHighScores()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            bestWave = data.bestWave;
            bestKillCount = data.bestKillCount;
            bestSurviveTime = data.bestSurviveTime;
        }
    }

    private void UpdateHighScoreUI()
    {
        if (bestWaveText) bestWaveText.text = "Highest Wave: " + bestWave;
        if (bestKillsText) bestKillsText.text = "Highest Kills: " + bestKillCount;
        if (bestTimeText) bestTimeText.text = "Longest Survive Time: " + bestSurviveTime.ToString("F1") + "s";
    }

    private void OnApplicationQuit()
    {
        SaveHighScores();
    }
}