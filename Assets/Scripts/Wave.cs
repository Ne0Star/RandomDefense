using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wave", menuName = "Enemu Wave")]
public class Wave : ScriptableObject
{
    [Header("Шанс что волна появится в данный момент времени")]
    [SerializeField] private float chance;

    [Header("Диапазон волн, когда эта волна испытает удачю спавна")]
    [SerializeField] private int minWave, maxWave;

    [Header("Сущности волны")]
    [SerializeField] private List<EnemuWave> waveUnits;

    public float Chance { get => chance; }
    public int MinWave { get => minWave; }
    public int MaxWave { get => maxWave; }
    public List<EnemuWave> WaveUnits { get => waveUnits; set => waveUnits = value; }

}
[System.Serializable]
public class EnemuWave
{
    [Header("Сущность")]
    [SerializeField] private Enemu enemu;

    [SerializeField] private int minCount, maxCount;
    [SerializeField] private float currentChance;
    [SerializeField] private float maxChance;


    [Header("Задержка между спавном сущностей")]
    [SerializeField] private float spawnDelay;

    public Enemu Enemu { get => enemu; }
    public float SpawnDelay { get => spawnDelay; }
    public int MinCount { get => minCount; set => minCount = value; }
    public int MaxCount { get => maxCount; set => maxCount = value; }
    public float MaxChance { get => maxChance; set => maxChance = value; }
    public float CurrentChance { get => currentChance; set => currentChance = value; }
}