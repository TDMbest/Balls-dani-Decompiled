using System.Collections.Generic;
using UnityEngine;

namespace Game
{
	public class SpawnEnemies : MonoBehaviour
	{
		public GameObject normalEnemy;

		public GameObject spikeEnemy;

		public GameObject directionEnemy;

		public GameObject goldenEnemy;

		public GameObject healerEnemy;

		public GameObject blackHoleEnemy;

		public GameObject cashEnemy;

		public GameObject superSpikeEnemy;

		public GameObject missileEnemy;

		public GameObject superDuperSpikeEnemy;

		public GameObject gravityEnemy;

		public GameObject detEnemy;

		public GameObject succEnemy;

		private float distance;

		private float distanceTravelled;

		private float min;

		private float max;

		private float spawnDistance = 30f;

		private float goldMultiplier = 1f;

		public List<GameObject> entities;

		private bool gravity;

		public static SpawnEnemies Instance { get; set; }

		private void Awake()
		{
			Instance = this;
			entities = new List<GameObject>();
			distance = 0f;
			distanceTravelled = 0f;
			min = 0f;
			max = 0f;
			goldMultiplier = 1f;
		}

		private void Update()
		{
			distance = base.transform.position.x;
			if (distance - max > 1f)
			{
				distanceTravelled += 1f;
				SpawnEnemy(spawnDistance);
				max = distance;
			}
			else if (min - distance > 1f)
			{
				distanceTravelled += 1f;
				SpawnEnemy(0f - spawnDistance);
				min = distance;
			}
			if (distance - min > spawnDistance)
			{
				min = distance - spawnDistance;
			}
			if (max - distance > spawnDistance)
			{
				max = distance + spawnDistance;
			}
		}

		private void SpawnEnemy(float offset)
		{
			if ((!(distance > 1130f) || !(distance < 1550f)) && !(distance < -920f))
			{
				GameObject original = SelectEnemyToSpawn();
				float y = Mathf.Floor(Mathf.Abs(Random.Range(0f, 1f) - Random.Range(0f, 1f)) * 203f + -2f);
				Vector2 vector = new Vector2(distance + offset, y);
				entities.Add(Object.Instantiate(original, vector, Quaternion.identity));
			}
		}

		private GameObject SelectEnemyToSpawn()
		{
			float num = distanceTravelled / 2000f;
			if (num > 1f)
			{
				num = 1f;
			}
			float num2 = distanceTravelled / 3000f * 0.35f;
			if (num2 > 2f)
			{
				num2 = 2f;
			}
			if (distanceTravelled < 400f)
			{
				num2 = 0f;
			}
			float num3 = distanceTravelled / 3000f * 0.3f;
			if (distanceTravelled < 1000f)
			{
				num3 = 0f;
			}
			float num4 = distanceTravelled / 4000f * 0.4f;
			float num5 = (0.01f + distanceTravelled / 5000f * 0.1f) * goldMultiplier;
			float num6 = 0.005f + distanceTravelled / 10000f * 0.005f;
			float num7 = (0.001f + distanceTravelled / 5000f * 0.05f) * goldMultiplier;
			float num8 = 0.009f + distanceTravelled / 5000f * 0.1f;
			float num9 = distanceTravelled / 4000f * 0.4f;
			if (num9 > 1f)
			{
				num9 = 1f;
			}
			float num10 = distanceTravelled / 3000f * 0.2f;
			float num11 = 0.01f + distanceTravelled / 5000f * 0.1f;
			if (!gravity)
			{
				num11 = 0f;
			}
			if (distanceTravelled < 50f)
			{
				num = 0f;
				num6 = 0f;
				num8 = 0f;
			}
			float num12 = Random.Range(0f, 1f + num + num4 + num5 + num6 + num7 + num2 + num8 + num3 + num11 + num9 + num10);
			if ((double)num12 < 0.05)
			{
				return healerEnemy;
			}
			if (num12 < 1f)
			{
				if (distance > 670f && distance < 1550f)
				{
					return spikeEnemy;
				}
				if (distance < -470f)
				{
					return superSpikeEnemy;
				}
				return normalEnemy;
			}
			if (num12 < 1f + num)
			{
				return spikeEnemy;
			}
			if (num12 < 1f + num + num4)
			{
				return directionEnemy;
			}
			if (num12 < 1f + num + num4 + num5)
			{
				return goldenEnemy;
			}
			if (num12 < 1f + num + num4 + num5 + num6)
			{
				return blackHoleEnemy;
			}
			if (num12 < 1f + num + num4 + num5 + num6 + num7)
			{
				return cashEnemy;
			}
			if (num12 < 1f + num + num4 + num5 + num6 + num7 + num2)
			{
				return superSpikeEnemy;
			}
			if (num12 < 1f + num + num4 + num5 + num6 + num7 + num2 + num8)
			{
				return missileEnemy;
			}
			if (num12 < 1f + num + num4 + num5 + num6 + num7 + num2 + num8 + num3)
			{
				return superDuperSpikeEnemy;
			}
			if (num12 < 1f + num + num4 + num5 + num6 + num7 + num2 + num8 + num3 + num11)
			{
				return gravityEnemy;
			}
			if (num12 < 1f + num + num4 + num5 + num6 + num7 + num2 + num8 + num3 + num11 + num9)
			{
				return detEnemy;
			}
			if (num12 < 1f + num + num4 + num5 + num6 + num7 + num2 + num8 + num3 + num11 + num9 + num10)
			{
				return succEnemy;
			}
			return normalEnemy;
		}

		public void ResetGame()
		{
			foreach (GameObject entity in entities)
			{
				Object.Destroy(entity);
			}
			entities = new List<GameObject>();
			distance = 0f;
			distanceTravelled = 0f;
			min = 0f;
			max = 0f;
			for (int i = 20; i < 60; i++)
			{
				SpawnEnemy(5f);
				distance += 1f;
			}
			distance = 0f;
			gravity = SaveManager.Instance.state.gravity;
			goldMultiplier = SaveManager.Instance.state.GetGoldMultiplier();
		}

		public int GetDistTravelled()
		{
			return (int)distanceTravelled;
		}

		public void SetDistTravelled(int d)
		{
			distanceTravelled = d;
		}

		public float GetCurrentDistance()
		{
			return distance;
		}

		public bool CubeZone()
		{
			return distance > 1550f;
		}
	}
}
