using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.SpaceInvadersGame.Source.Core.Enemy
{
	public class EnemyController : MonoBehaviour
	{
		private static int rows = 5;
		private static int cols = 10;

		private static int colOffset = -5;
		private static int rowOffset = 5;

		private int row = 0;
		private int col = 0;



		public static int poolSize = 105;

		private List<GameObject> pool = new List<GameObject>();
		private List<GameObject> spawns = new List<GameObject>();
		[SerializeField] private GameObject spawnPrefab = null;
		private bool canMove = false;


		[SerializeField] public enum EMoveDirection
		{
			NONE = 0,
			DOWN = 1,
			LEFT = 2,
			RIGHT = 3
		}

		private EMoveDirection moving = EMoveDirection.NONE;
		private EMoveDirection lastMove = EMoveDirection.NONE;

		private float elapsed = 0f;
		private static float span = 2f;

		public UnityAction<EMoveDirection> moveDirectionUpdated;

		private GameManager gameManager = null;
		private void OnDestroy()
		{
			DestroyPooledObjects();
			pool = null;
			spawns = null;
			spawnPrefab = null;
			gameManager = null;
		}

		public void StartGame()
		{
			// spawn enemies, start move logic
			for (row = 0; row < rows; row++)
			{
				for (col = 0; col < cols; col++)
				{
					Vector3 pos = new Vector3(col + colOffset, row + rowOffset, 0);
					GameObject obj = InstantiatePooledObject(pos);
				}
			}
			canMove = true;
			elapsed = 0f;
			lastMove = EMoveDirection.NONE;
			moving = EMoveDirection.LEFT;
			ElapsedSpan();
		}
		public void EndGame()
		{
			// nuke all enemies, pause move logic
			ReturnAllToPool();
			canMove = false;
		}

		private void Start()
		{
			CreatePool();
			gameManager = GameObjects.Instance.gameManager;
		}
		private void Update()
		{
			if (canMove == false) return;
			if (elapsed + Time.deltaTime > span)
			{
				elapsed = 0f;
				ElapsedSpan();
				return;
			}
			elapsed += Time.deltaTime;
		}
		private void ElapsedSpan()
		{
			EMoveDirection nextMove = EMoveDirection.DOWN;
			span = 1f;
			if (lastMove == EMoveDirection.LEFT)
			{
				nextMove = EMoveDirection.RIGHT;
				span = 2f;
			}
			else if (lastMove == EMoveDirection.RIGHT)
			{
				nextMove = EMoveDirection.LEFT;
				span = 2f;
			}
			lastMove = moving;
			moving = nextMove;
			moveDirectionUpdated?.Invoke(moving);
		}


		private void DestroyPooledObjects()
		{
			foreach (GameObject obj in pool)
			{
				Destroy(obj);
			}

			foreach (GameObject obj in spawns)
			{
				Destroy(obj);
			}
		}

		public void ReturnAllToPool()
		{
			HashSet<GameObject> rm = new HashSet<GameObject>();
			foreach(GameObject obj in spawns) rm.Add(obj);
			foreach(GameObject obj in rm) AddToPool(obj);
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

				spawn.GetComponent<Enemy>().controller = this;

				pool.Add(spawn);
				spawn.SetActive(false);

				i++;
			}
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

			if (spawns.Count <= 0)
			{
				if (gameManager.State == GameManager.EState.PLAY) gameManager.WonGame();
			}
		}

		[ContextMenu("TestSpawn")]
		private void TestSpawn()
		{
			StartGame();
		}
	}
}