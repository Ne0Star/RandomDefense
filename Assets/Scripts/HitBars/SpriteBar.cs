using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct AnimationState
{
    public Sprite[] sprites;
    public SpriteRenderer render;
}

public class SpriteBar : HitBar
{
    [Header("В порядке возрастания")]

    public List<AnimationState> states;

    [SerializeField] private float persent;
    [SerializeField] private float tt;
    [SerializeField] private int currentIndex;
    public override void UpdateData()
    {
        float totalDamage = totalHealthDamage + totalStrengthDamage; // 300
        float maxDamage = maxHealth + maxHealth; // 500
        for (int i = 0; i < states.Count; i++)
        {
            AnimationState state = states[i];
            for (int j = 0; j < state.sprites.Length; j++)
            {

                persent = Mathf.Round(((totalDamage * 100) / maxDamage));
                tt = ((state.sprites.Length) / 100f * persent);
                currentIndex = Mathf.RoundToInt(tt);
                state.render.sprite = state.sprites[Mathf.Clamp(currentIndex, 0, state.sprites.Length - 1)];
            }
        }



    }
}
