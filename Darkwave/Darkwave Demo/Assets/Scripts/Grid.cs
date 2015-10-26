using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//grid starts at the bottom left corner of model

public class Grid : MonoBehaviour 
{
    public int 
		rows,
		columns;
    public Transform 
		laserGrid;
	
    private float 
		squareArea, 
		width, 
		depth, 
		height = .25f;
    private Vector3 
		start, 
		placementPos, 
		center, 
		size;
    private List<GameObject> gridLasers;
    private Vector2 lastGridPos;
    private bool[,] canPlaceArray;

    private GameObject[,] objectsInGrid;

	// Use this for initialization
	void Start () 
    {
        size = gameObject.GetComponent<Renderer>().bounds.size;
        width = size.x / rows;
        depth = size.z / columns;
        center = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + height, gameObject.transform.position.z);
		start = new Vector3(center.x - (size.x / 2), gameObject.transform.position.y + height, center.z - (size.z / 2));
        gridLasers = new List<GameObject>();
        canPlaceArray = new bool[rows, columns];
        objectsInGrid = new GameObject[rows, columns];

        //sets up canPlace array
        for (int i = 0; i < rows; i++)
            for (int j = 0; j < columns; j++)
                canPlaceArray[i, j] = true;

        //Lines along x axis
        for (int i = 0; i <= rows; i++)
        {
            Vector3 rowPos = start;
            rowPos.x += width * i;
            //creates grid line
            gridLine(rowPos, new Vector3(rowPos.x, height, center.z + size.z/2));
        }

        //Lines along z axis
        for (int j = 0; j <= columns; j++)
        {
            Vector3 colPos = start;
            colPos.z += depth * j;
            //creates grid line
            gridLine(colPos, new Vector3(center.x + size.x/2, height, colPos.z));
        }
	}

    //creates and stores the grid lines in an list
    void gridLine(Vector3 start, Vector3 end)
    {
		GameObject laserLine;
        LineRenderer line;
        line = laserGrid.transform.GetComponent<LineRenderer>();
        line.SetPosition(0, start);
        line.SetPosition(1, end);

		laserLine = GameObject.Instantiate(laserGrid.gameObject, start, Quaternion.identity) as GameObject;
		laserLine.layer = 10;//GridLines layer
		laserLine.transform.parent = GameObject.Find("Ground").transform;
		gridLasers.Add (laserLine);

        //gridLasers.Add(GameObject.Instantiate(laserGrid.gameObject, start, Quaternion.identity) as GameObject);
    }

    //returns the row and col number as a vector2
    public Vector2 GetGridPosition(Vector3 position)
    {
        Vector2 gridPos = new Vector2(-1, -1);

        //gets row
        for (int i = 0; i <= rows + 1; i++)
        {
            if (position.x > start.x + (width * i) && position.x <  start.x + (width * (i + 1)))
            {
                gridPos.x = i;
            }
        }

        //gets cols
        for (int j = 0; j <= columns + 1; j++)
        {
            if (position.z > start.z + (depth * j) && position.z <  start.z + (depth * (j + 1)))
            {
                gridPos.y = j;
            }
        }

        return gridPos;
    }

    //returns the correct vector3(middle of the square) for the grid
    public Vector3 getVector3(Vector3 position)
    {
		Vector2 gridPosition = GetGridPosition(position);
        Vector3 placementPosition = new Vector3(gridPosition.x, 0, gridPosition.y);

        placementPosition.x = (start.x + (placementPosition.x * width)) + width / 2;
        placementPosition.z = (start.z + (placementPosition.z * depth)) + depth / 2;

        return placementPosition;
    }

    public List<Vector3> getAdjacentWallLocations(Vector3 position, int range)
    {
		Vector2 gridPos = GetGridPosition(position);
       
        List<Vector3> vectors = new List<Vector3>();

        int up = 0, down = 0, left = 0, right = 0;
        for (int i = 1; i < range; i++)
        {
			if (gridPos.y + i < rows)//checks for walls above in range
            {
                if (up == 0 && 
				    objectsInGrid[(int)gridPos.x, (int)gridPos.y + i] != null &&
				    objectsInGrid[(int)gridPos.x, (int)gridPos.y + i].GetComponent<BuildableObject>().isPillar)
                {
                        up = i;
                }
            }
			if (gridPos.y - i >= 0)//checks for walls below in range
                if (down == 0 && 
				    objectsInGrid[(int)gridPos.x, (int)gridPos.y - i] != null &&
				    objectsInGrid[(int)gridPos.x, (int)gridPos.y - i].GetComponent<BuildableObject>().isPillar)
                {
                        down = i;
                }
			if (gridPos.x + i < columns)//checks for walls to the right in range
                if (right == 0 && 
				    objectsInGrid[(int)gridPos.x + i, (int)gridPos.y] != null &&
				    objectsInGrid[(int)gridPos.x + i, (int)gridPos.y].GetComponent<BuildableObject>().isPillar)

                {
                        right = i;
                }
			if (gridPos.x - i >= 0)//checks for walls to the left in range
                if (left == 0 && 
				    objectsInGrid[(int)gridPos.x - i, (int)gridPos.y] != null &&
				    objectsInGrid[(int)gridPos.x - i, (int)gridPos.y].GetComponent<BuildableObject>().isPillar)

                {
                        left = i;
                }
        }

        //this segments calculates the position all connecting walls should have
        for (int i = 1; i < up; i++)
        {
            Vector2 gridTemp = gridPos;
            gridTemp.y += i;
            vectors.Add(gridToVector(gridTemp));
        }
        for (int i = 1; i < down; i++)
        {
            Vector2 gridTemp = gridPos;
            gridTemp.y -= i;
            vectors.Add(gridToVector(gridTemp));
        }
        for (int i = 1; i < right; i++)
        {
            Vector2 gridTemp = gridPos;
            gridTemp.x += i;
            vectors.Add(gridToVector(gridTemp));
        }
        for (int i = 1; i < left; i++)
        {
            Vector2 gridTemp = gridPos;
            gridTemp.x -= i;
            vectors.Add(gridToVector(gridTemp));
        }
        return vectors;
    }

	//places the reference object in objectsInGrid and updates canPlaceArray
    public void editGrid(GameObject obj, bool create)
    {
        Vector3 position = obj.transform.position;
		Vector2 gridPos = GetGridPosition(position);
        
		if(create)
		{
	        canPlaceArray[(int)gridPos.x, (int)gridPos.y] = false;//updates array so that objects cannot be placed on others
	        objectsInGrid[(int)gridPos.x, (int)gridPos.y] = obj;//puts gameobject in reference array
		}
		else
		{
			canPlaceArray[(int)gridPos.x, (int)gridPos.y] = true;//updates array so that objects cannot be placed on others
			objectsInGrid[(int)gridPos.x, (int)gridPos.y] = null;//puts gameobject in reference array
		}
    }

    private Vector3 gridToVector(Vector2 grid)
    {
        Vector3 vec = new Vector3();
        vec.x = (start.x + (grid.x * width)) + width / 2;
        vec.z = (start.z + (grid.y * depth)) + depth / 2;
        return vec;
    }

    public bool canPlace(Vector3 position)
    {
        Vector2 gridPos = GetGridPosition(position);

        if (gridPos.x == -1 || gridPos.y == -1)
            return false;
        else
            return canPlaceArray[(int)gridPos.x, (int)gridPos.y];

    }
}
