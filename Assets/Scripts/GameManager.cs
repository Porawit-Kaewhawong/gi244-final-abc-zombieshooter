using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private Coroutine waveCoroutine;

    void Start()
    {
        StartCoroutine(WaveControl());
    }

    private IEnumerator WaveControl()
    {
        for (int i = 0; i < waves.Count; i++)
        {
            int currentWave = i + 1;

            if (waveCoroutine != null)
            {
                StopCoroutine(waveCoroutine);
            }

            yield return StartCoroutine(WaveSpawn(i));

            Debug.Log("Wave " + currentWave + " is completed.");
        }
    }

    private IEnumerator WaveSpawn(int waveId)
    {
        Wave currentWave = waves[waveId];

        yield return new WaitForSeconds(currentWave.delayStart);

        for (int i = 0; i < currentWave.totalEnemies ; i++)
        {
            int random = Random.Range(0, spawnPoints.Length);
            Instantiate(enemyPrefab, spawnPoints[random]);

            yield return new WaitForSeconds(currentWave.spawnInterval);
        }

        for (int i = 0; i < currentWave.numberOfPowerUp; i++)
        {
            int random = Random.Range(0, powerUps.Length);
            float spawnRange = powerUpSpawnArea.localScale.x * 5f;

            Vector3 randomPosition = new Vector3(
                Random.Range(-spawnRange, spawnRange),
                transform.position.y + 0.5f,
                Random.Range(-spawnRange, spawnRange));

            Instantiate(
                powerUps[random], 
                powerUpSpawnArea.position + randomPosition, 
                Quaternion.identity);
        }
    }
}
