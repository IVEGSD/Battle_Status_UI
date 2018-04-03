using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Enemy {
    public enum EnemyType { type1, type2, type3 }
    public string Name { get; set; }
    public EnemyType Type { get; set; }
}
