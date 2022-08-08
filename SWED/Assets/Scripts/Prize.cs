using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

[CreateAssetMenu(menuName = "Prize")]
public class Prize : ScriptableObject
{
    public string id;
    public string displayName;
    public float winningChance;

    [SerializeField] private SpriteAtlas atlas;

    public Sprite GetPrizeSprite()
    {
        return atlas.GetSprite(id);
    }
}


