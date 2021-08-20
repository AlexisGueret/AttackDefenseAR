using UnityEngine;
using System.Collections;
using UnityEngine.AI;

//<summary>
//Game object, that creates maze and instantiates it in scene
//</summary>
public class MazeSpawner : MonoBehaviour {
	public enum MazeGenerationAlgorithm{
		PureRecursive,
		RecursiveTree,
		RandomTree,
		OldestTree,
		RecursiveDivision,
	}
	private bool goalAlreadySpawned = false;
	public MazeGenerationAlgorithm Algorithm = MazeGenerationAlgorithm.PureRecursive;
	public bool FullRandom = false;
	public int RandomSeed = 12345;
	public GameObject Floor = null;
	public GameObject Wall = null;
	public GameObject Pillar = null;
	public int Rows = 5;
	public int Columns = 5;
	public float CellWidth = 5;
	public float CellHeight = 5;
	public bool AddGaps = true;
	public GameObject GoalPrefab = null;

	private BasicMazeGenerator mMazeGenerator = null;
	private GameController gameController;

	[SerializeField]
	GameObject field;

    private void Awake()
    {
		gameController = FindObjectOfType<GameController>();
    }
    public void GenerateMaze()
    {
		if (!FullRandom)
		{
			Random.seed = RandomSeed;
		}
		switch (Algorithm)
		{
			case MazeGenerationAlgorithm.PureRecursive:
				mMazeGenerator = new RecursiveMazeGenerator(Rows, Columns);
				break;
			case MazeGenerationAlgorithm.RecursiveTree:
				mMazeGenerator = new RecursiveTreeMazeGenerator(Rows, Columns);
				break;
			case MazeGenerationAlgorithm.RandomTree:
				mMazeGenerator = new RandomTreeMazeGenerator(Rows, Columns);
				break;
			case MazeGenerationAlgorithm.OldestTree:
				mMazeGenerator = new OldestTreeMazeGenerator(Rows, Columns);
				break;
			case MazeGenerationAlgorithm.RecursiveDivision:
				mMazeGenerator = new DivisionMazeGenerator(Rows, Columns);
				break;
		}
		mMazeGenerator.GenerateMaze();
		for (int row = 0; row < Rows; row++)
		{
			for (int column = 0; column < Columns; column++)
			{
				float x = column * (CellWidth + (AddGaps ? .2f : 0));
				float z = row * (CellHeight + (AddGaps ? .2f : 0));
				MazeCell cell = mMazeGenerator.GetMazeCell(row, column);
				GameObject tmp;
				tmp = Instantiate(Floor, new Vector3(x, 0, z), Quaternion.Euler(0, 0, 0)) as GameObject;
				tmp.transform.parent = transform;
				if (cell.WallRight)
				{
					tmp = Instantiate(Wall, new Vector3(x + CellWidth / 2, 0, z) + Wall.transform.position, Quaternion.Euler(0, 90, 0)) as GameObject;// right
					tmp.transform.parent = transform;
				//	tmp.GetComponent<NavMeshSurface>().BuildNavMesh();
				}
				if (cell.WallFront)
				{
					tmp = Instantiate(Wall, new Vector3(x, 0, z + CellHeight / 2) + Wall.transform.position, Quaternion.Euler(0, 0, 0)) as GameObject;// front
					tmp.transform.parent = transform;
					//tmp.GetComponent<NavMeshSurface>().BuildNavMesh();
				}
				if (cell.WallLeft)
				{
					tmp = Instantiate(Wall, new Vector3(x - CellWidth / 2, 0, z) + Wall.transform.position, Quaternion.Euler(0, 270, 0)) as GameObject;// left
					tmp.transform.parent = transform;
					//tmp.GetComponent<NavMeshSurface>().BuildNavMesh();
				}
				if (cell.WallBack)
				{
					tmp = Instantiate(Wall, new Vector3(x, 0, z - CellHeight / 2) + Wall.transform.position, Quaternion.Euler(0, 180, 0)) as GameObject;// back
					tmp.transform.parent = transform;
				//	tmp.GetComponent<NavMeshSurface>().BuildNavMesh();
				}
				if (cell.IsGoal && GoalPrefab != null && !goalAlreadySpawned)
				{
					if(!gameController.GetIsARMode())
						tmp = Instantiate(GoalPrefab, new Vector3(x, 0.7f, z), Quaternion.Euler(0, 0, 0)) as GameObject;
					else
						tmp = Instantiate(GoalPrefab, new Vector3(x, 0.07f, z), Quaternion.Euler(0, 0, 0)) as GameObject;
					goalAlreadySpawned = true;
					tmp.transform.parent = transform;
				}
			}
		}
		if (Pillar != null)
		{
			for (int row = 0; row < Rows + 1; row++)
			{
				for (int column = 0; column < Columns + 1; column++)
				{
					float x = column * (CellWidth + (AddGaps ? .2f : 0));
					float z = row * (CellHeight + (AddGaps ? .2f : 0));
					GameObject tmp = Instantiate(Pillar, new Vector3(x - CellWidth / 2, 0, z - CellHeight / 2), Quaternion.identity) as GameObject;
					tmp.transform.parent = transform;
				}
			}
		}
		if (!gameController.GetIsARMode())
			this.transform.position += new Vector3(-7, 1.5f, -5);
		else
        {
			
			this.transform.position = field.transform.position;
			this.transform.rotation = Quaternion.Euler(this.transform.rotation.eulerAngles.x, field.transform.rotation.eulerAngles.y, this.transform.rotation.eulerAngles.z);
			this.transform.position += new Vector3(-0.7f, 0, -0.7f);
			//this.transform.localScale = Vector3.one * 0.1f;
		}
			
	}
}
