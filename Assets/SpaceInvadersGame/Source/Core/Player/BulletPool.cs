using System.Collections.Generic;
using UnityEngine;

namespace Assets.SpaceInvadersGame.Source.Core.Player
{
	public class BulletPool : MonoBehaviour
	{
		public static int poolSize = 20;

		private List<GameObject> pool = new List<GameObject>();
		private List<GameObject> spawns = new List<GameObject>();
		[SerializeField] private GameObject spawnPrefab = null;
		private void Start()
		{
			CreatePool();
		}

		public void CreatePool()
		{
			if (spawnPrefab == null)
			{
				Debug.LogError("EnemyController.CreatePool: spawnPrefab is null", gameObject);
				return;
			}
			int i = 0;
			while (i < poolSize)
			{
				GameObject spawn = Instantiate(spawnPrefab, transform);
				spawn.transform.position = transform.position;

				spawn.GetComponent<Bullet>().poolController = this;

				pool.Add(spawn);
				spawn.SetActive(false);

				i++;
			}
		}
		public void ReturnAllToPool()
		{
			HashSet<GameObject> rm = new HashSet<GameObject>();
			foreach(GameObject obj in spawns) rm.Add(obj);
			foreach(GameObject obj in rm) AddToPool(obj);
		}


		public GameObject InstantiatePooledObject(Vector3 pos)
		{
			if (pool.Count <= 0) return null;
			GameObject spawn = pool[0];
			spawn.transform.position = pos;
			spawn.SetActive(true);
			RemoveFromPool(spawn);
			return spawn;
		}

		private void RemoveFromPool(GameObject obj)
		{
			if (spawns.Contains(obj)) return;
			spawns.Add(obj);
			pool.Remove(obj);
		}

		public void AddToPool(GameObject obj)
		{
			if (!spawns.Contains(obj)) return;
			spawns.Remove(obj);
			pool.Add(obj);
			obj.SetActive(false);
		}

	}
}