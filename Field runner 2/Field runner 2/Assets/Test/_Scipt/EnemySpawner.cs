using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoSingleton<EnemySpawner>
{
    [SerializeField] private _FieldSlot startSlot;
    [SerializeField] private _FieldSlot endSlot;
    [SerializeField] private List<WaveConfig> waveConfigs;

    private int currentWaveIndex = 0;
    private float spawnTimer = 0;
    private Wave currentWave;

    public _FieldSlot StartSlot => startSlot;
    public _FieldSlot EndSlot => endSlot;

    private void Start()
    {
        currentWaveIndex = 0;
        currentWave = new Wave(waveConfigs[currentWaveIndex]);
    }

    private void Update()
    {
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= currentWave.Config.SpawnDelay)
        {
            if (currentWave.SpawnEnemy())
            {
                spawnTimer -= currentWave.Config.SpawnDelay;
                var enemy = Instantiate(currentWave.Config.EnemyData.EnemyPrefab);
                enemy.transform.position = StartSlot.transform.position;
                enemy.SetupData(currentWave.Config.EnemyData, StartSlot, EndSlot);
            }
            else
            {
                currentWaveIndex++;
                if (currentWaveIndex < waveConfigs.Count)
                {
                    currentWave = new Wave(waveConfigs[currentWaveIndex]);
                }
            }
        }
    }
}
