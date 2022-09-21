using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Управляет сущностями
/// </summary>
public class EdificeManager : MonoBehaviour
{
    [SerializeField] private List<Entity> allEntity;
    [SerializeField] private List<Edifice> allEdifices;
    [SerializeField] private List<EntityUnit> allUnits;
    [SerializeField] private List<Wall> allWalls;
    [SerializeField] private List<Turret> allTurrets;
    [SerializeField] private List<Generator> allGenerators;

    public List<Entity> AllEntity { get => allEntity; set => allEntity = value; }
    public List<Edifice> AllEdifice { get => allEdifices; set => allEdifices = value; }
    public List<EntityUnit> AllUnits { get => allUnits; set => allUnits = value; }
    public List<Wall> AllWalls { get => allWalls; set => allWalls = value; }
    public List<Turret> AllTurrets { get => allTurrets; set => allTurrets = value; }
    public List<Generator> AllGenerators { get => allGenerators; set => allGenerators = value; }


    private void OnEnable()
    {
        StartCoroutine(TurretsTick());
        StartCoroutine(GeneratorTick());
    }

    [SerializeField] private float turretFrameDelay; // Сколько фреймов ждать для продолжения
    [Range(1f, 100f)]
    [SerializeField] private float turretToFramePersent;
    [SerializeField] private int turretToFrame; // Количество индексов которые будут проходится в 1 фрейм
    [SerializeField] private int turretLast = 0;
    [SerializeField] private int turretCurrent = 0;


    [SerializeField] private float genFrameDelay;
    [Range(1f, 100f)]
    [SerializeField] private float genToFramePersent;
    [SerializeField] private int genToFrame;
    [SerializeField] private int genLast = 0;
    [SerializeField] private int genCurrent = 0;



    private IEnumerator TurretsTick()
    {
        turretToFrame = allTurrets.Count == 0 ? 0 : Mathf.Clamp(Mathf.RoundToInt(allTurrets.Count / 100f * turretToFramePersent), 1, 100);

        int t = turretLast;
        int r = Mathf.Clamp(t + turretToFrame, 0, allTurrets.Count);
        for (int i = t; i < r; i++)
        {
            allTurrets[i].Tick();
            t++;
        }
        turretLast = t;
        if (turretLast >= allTurrets.Count)
        {
            turretLast = 0;
        }
        
        yield return new WaitForSeconds(turretFrameDelay / LevelManager.Instance.GameSpeed);
        StartCoroutine(TurretsTick());
        yield break;
    }
    private IEnumerator GeneratorTick()
    {
        genToFrame = allGenerators.Count == 0 ? 0 : Mathf.Clamp(Mathf.RoundToInt(allGenerators.Count / 100f * genToFramePersent), 1, 100);

        int t = genLast;
        int r = Mathf.Clamp(t + genToFrame, 0, allGenerators.Count);
        for (int i = t; i < r; i++)
        {
            allGenerators[i].Tick();
            t++;
        }
        genLast = t;
        if (genLast >= allGenerators.Count)
        {
            genLast = 0;
        }
        yield return new WaitForSeconds(genFrameDelay / LevelManager.Instance.GameSpeed);
        StartCoroutine(GeneratorTick());
        yield break;
    }
}
