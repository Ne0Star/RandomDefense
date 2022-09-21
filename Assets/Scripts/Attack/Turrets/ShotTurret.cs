using System.Collections;
using UnityEngine;


// Если weapon mode, турель не ищет цели, цель устанавливает носитель оружия
public class ShotTurret : RotateTurret
{
    [SerializeField] private Transform[] shotSFX;
    [SerializeField] private float attackDamage;
    [SerializeField] private int attackCount;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float reloadSpeed;

    protected override void OnAttack()
    {
        StartCoroutine(Attack_Animation());
    }

    private void OnDisable()
    {
        attack = false;
        rotated = false;
        target = null;
        foreach (Transform shot in shotSFX)
        {
            shot.gameObject.SetActive(false);
        }
    }

    private IEnumerator Attack_Animation()
    {
        for (int i = 0; i < attackCount; i++)
        {
            if (!target || !target.gameObject.activeInHierarchy) break;
            if (mainTrigger && !mainTrigger.CheckRadius(target)) break;
            GameUtils.LookAt2DSmooth(transform, target.transform.position, rotateOffset, rotateSpeed, 0.1f);
            foreach (Transform shot in shotSFX)
            {
                shot.gameObject.SetActive(true);
                if (target)
                    target.TakeDamage(attackDamage, 0f, transform.position);
                yield return new WaitForSeconds((attackSpeed / LevelManager.Instance.GameSpeed) / 2f);
                shot.gameObject.SetActive(false);
                yield return new WaitForSeconds((attackSpeed / LevelManager.Instance.GameSpeed) / 2f);
            }
        }
        foreach (Transform shot in shotSFX)
        {
            shot.gameObject.SetActive(false);
        }
        yield return new WaitForSeconds(reloadSpeed / LevelManager.Instance.GameSpeed);
        attack = false;
        rotated = false;
        yield break;
    }
}
