using SeraphUtil.ObjectPool;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Pool Options", menuName = "Seraph Utility/Pool/Enemy Pool Options", order = 1)]
public class EnemyPoolOptions : MultiPoolOptions<EnemyUnit>{}
