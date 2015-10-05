using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//grid starts at the bottom left corner of model

public class Grid : MonoBehaviour 
{
    public int 
		row,
		col;
    public Transform 
		laserGrid;
	
    private float 
		squareArea, 
		width, 
		depth, 
		height = 0f;
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
        width = size.x / row;
        depth = size.z / col;
        center = new Vector3(gameObject.transform.position.x, height, gameObject.transform.position.z);
        start = new Vector3(center.x - (size.x / 2), height, center.z - (size.z / 2));
        gridLasers = new List<GameObject>();
        canPlaceArray = new bool[row, col];
        objectsInGrid = new GameObject[row, col];

        //sets up canPlace array
        for (int i = 0; i < row; i++)
            for (int j = 0; j < col; j++)
                canPlaceArray[i, j] = true;

        //Lines along x axis
        for (int i = 0; i <= row; i++)
        {
            Vector3 rowPos = start;
            rowPos.x += width * i;
            //creates grid line
            gridLine(rowPos, new Vector3(rowPos.x, height, center.z + size.z/2));
        }

        //Lines along z axis
        for (int j = 0; j <= col; j++)
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
        for (int i = 0; i <= row + 1; i++)
        {
            if (position.x > start.x + (width * i) && position.x <  start.x + (width * (i + 1)))
            {
                gridPos.x = i;
            }
        }

        //gets cols
        for (int j = 0; j <= col + 1; j++)
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
        Vector3 gridPos = new Vector3(-1, 0, -1);

        //gets row
        for (int i = 0; i <= row + 1; i++)
        {
            if (position.x > start.x + (width * i) && position.x < start.x + (width * (i + 1)))
            {
                gridPos.x = i;
            }
        }

        //gets cols
        for (int j = 0; j <= col; j++)
        {
            if (position.z > start.z + (depth * j) && position.z < start.z + (depth * (j + 1)))
            {
                gridPos.y = j;
            }
        }

        gridPos.x = (start.x + (gridPos.x * width)) + width / 2;
        gridPos.z = (start.z + (gridPos.y * depth)) + depth / 2;

        return gridPos;
    }

    public List<Vector3> getAdjacentWallLocations(Vector3 position)
    {

        Vector2 gridPos = new Vector2() ;
        //gets row
        for (int i = 0; i <= row + 1; i++)
        {
            if (position.x > start.x + (width * i) && position.x < start.x + (width * (i + 1)))
            {
                gridPos.x = i;
            }
        }

        //gets cols
        for (int j = 0; j <= col; j++)
        {
            if (position.z > start.z + (depth * j) && position.z < start.z + (depth * (j + 1)))
            {
                gridPos.y = j;
            }
        }

        List<Vector3> vectors = new List<Vector3>();

        int up = 0, down = 0, left = 0, right = 0;
        for (int i = 1; i < 5; i++)
        {
            if (gridPos.y + i < row)
            {
                if (up == 0 && objectsInGrid[(int)gridPos.x, (int)gridPos.y + i] != null)//checks for walls above up to 4 spaces away
                {
                    if (objectsInGrid[(int)gridPos.x, (int)gridPos.y + i].tag == "Wall")
                        up = i;
                }
            }
            if (gridPos.y - i >= 0)
                if (down == 0 && objectsInGrid[(int)gridPos.x, (int)gridPos.y - i] != null)//checks for walls below up to 4 spaces away
                {
                    if (objectsInGrid[(int)gridPos.x, (int)gridPos.y - i].tag == "Wall")
                        down = i;
                }
            if (gridPos.x + i < col)
                if (right == 0 && objectsInGrid[(int)gridPos.x + i, (int)gridPos.y] != null)//checks for walls to the right up to 4 spaces away
                {
                    if (objectsInGrid[(int)gridPos.x + i, (int)gridPos.y].tag == "Wall")
                        right = i;
                }
            if (gridPos.x - i >= 0)
                if (left == 0 && objectsInGrid[(int)gridPos.x - i, (int)gridPos.y] != null)//checks for walls to the left up to 4 spaces away
                {
                    if (objectsInGrid[(int)gridPos.x - i, (int)gridPos.y].tag == "Wall")
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
    
    public void editGrid(GameObject obj, bool create)
    //places the reference object in objectsInGrid and updates canPlaceArray
    {
        Vector2 gridPos = new Vector3(-1, -1);
        Vector3 position = obj.transform.position;
        //gets row
        for (int i = 0; i <= row + 1; i++)
        {
            if (position.x > start.x + (width * i) && position.x < start.x + (width * (i + 1)))
            {
                gridPos.x = i;
            }
        }

        //gets cols
        for (int j = 0; j <= col; j++)
        {
            if (position.z > start.z + (depth * j) && position.z < start.z + (depth * (j + 1)))
            {
                gridPos.y = j;
            }
        }
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
        Vector2 gridPos = new Vector2(-1, -1);

        //gets row
        for (int i = 0; i <= row + 1; i++)
        {
            if (position.x > start.x + (width * i) && position.x < start.x + (width * (i + 1)))
            {
                gridPos.x = i;
            }
        }

        //gets cols
        for (int j = 0; j <= col; j++)
        {
            if (position.z > start.z + (depth * j) && position.z < start.z + (depth * (j + 1)))
            {
                gridPos.y = j;
            }
        }

        if (gridPos.x == -1 || gridPos.y == -1)
            return false;
        else
            return canPlaceArray[(int)gridPos.x, (int)gridPos.y];

    }
}
