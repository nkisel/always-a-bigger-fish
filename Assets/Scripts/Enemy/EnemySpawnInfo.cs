using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemySpawnInfo
{
	#region Editor Variables
	[SerializeField]
	[Tooltip("Prefab of the enemy to spawn")]
	private GameObject m_EnemyObj;
	public GameObject EnemyObj
	{
		get
		{
			return m_EnemyObj;
		}
	}

	[SerializeField]
	[Tooltip("Top number in ratio that this enemy will be spawned")]
	private int m_EnemyChance;
	public int EnemyChance
	{
		get
		{
			return m_EnemyChance;
		}
	}
	#endregion
}
