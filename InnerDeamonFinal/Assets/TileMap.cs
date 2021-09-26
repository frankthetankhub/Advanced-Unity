using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Object = UnityEngine.Object;
public class TileMap : MonoBehaviour
{
    public Unit[] playerTeam;
    public Unit[] enemyTeam;
    public Unit selectedUnit;
    public GameObject selectedObject ;
    public GameObject prevSelectedObject;
    public Tile.TileTypes[,] testMapData = new Tile.TileTypes[20,20];

    public Tile[,] map;
    void Start()
    {
        
        FillTestMap();
        GenerateMap(testMapData);
        selectedObject = null;

    }
    void Update()
    {
        
    }
    
    
    public void highlightPath(Tile[] path)
    {
        if (path == null)
        {
            return;
        }
        if (path.Length == 0)
        {
            return;
        }
        
        path[path.Length-1].highlight(Color.yellow);
        for (int i = 0; i < path.Length-1 ; i++)
        {
            path[i].highlight(Color.cyan);
        }
    }

    public List<Tile> FindPath(Tile startPoint, Tile target)
    {
        //check if it makes sense to calculate a path
        if (target)
        {
            //initialize all needed lists and dictionaries
            List<Tile> path = new List<Tile>();
            Dictionary<Tile, int> dist = new Dictionary<Tile, int>();
            Dictionary<Tile, Tile> prev = new Dictionary<Tile, Tile>();
            //declare start and end tile
            Tile start = startPoint;
            foreach (var tile in map)
            {
                dist[tile] = 10000;
                prev[tile] = null;
            }
            if( target.isWalkable == false ) {
                // We clicked on a tile !walkable
                Debug.Log("Cannot go there, mate");
                return path;
            }
            dist[start] = 0;
            path.Add(start);
            while (path.Count > 0)
            {
                Tile t = null;
                foreach(Tile possibleTile in path) {
                    if(!t || dist[possibleTile] < dist[t]) {
                        t = possibleTile;
                    }
                }
                if (t == target)
                {
                    path.Clear();
                    break;
                }
                path.Remove(t);
                // Now we see if we can update the distance values for neighbors of our new node
                // if so we also update the previous node of our new best node
                foreach(Tile v in t.neighbors) {
                    int alt = dist[t] + v.movementCost;
                    if( alt < dist[v] ) {
                        dist[v] = alt;
                        prev[v] = t;
                        path.Add(v);
                    }
                }
            }
            if(!prev[target]) {
                // No route between our target and the source
                Debug.Log("There is no path to that Tile");
                path.Clear();
                return path;
            }
            path.Add(target);
            while (prev[target])
            {
                path.Add(prev[target]);
                target = prev[target];
            }
            path.Remove(target);
            path.Reverse();
            return path;
        }

        return new List<Tile>();

    }
    public void GenerateMap(Tile.TileTypes[,] Tiletypes)
    {
        map = new Tile[Tiletypes.GetLength(0),Tiletypes.GetLength(1)];
        
        for (int i = 0; i < Tiletypes.GetLength(0); i++)
        {
            for (int j = 0; j < Tiletypes.GetLength(1); j++)
            {
                // Potentiell könnte man eine Liste mit den prefabs anfertigen und dann nur das element aus der Liste Instantiaten
                if (Tiletypes[i,j] == Tile.TileTypes.TileBasic)
                {
                    GameObject go =  (GameObject)Tile.Instantiate(Resources.Load("TileBasic"), new Vector3(i,j,0), Quaternion.identity);
                    map[i, j] = go.GetComponent<Tile>();
                }
                else
                {
                    if (Tiletypes[i,j] == Tile.TileTypes.TileTough)
                    {
                        GameObject go = (GameObject) Tile.Instantiate(Resources.Load("TileTough"), new Vector3(i,j,0), Quaternion.identity);
                        map[i, j] = go.GetComponent<Tile>();
                    }
                    else
                    {
                        if (Tiletypes[i,j] == Tile.TileTypes.TileWall)
                        {
                            GameObject go = (GameObject) Tile.Instantiate(Resources.Load("TileWall"), new Vector3(i,j,0), Quaternion.identity);
                            map[i, j] = go.GetComponent<Tile>();
                        }
                    }
                }
            }
        }
    }
    public void FillTestMap()
    {
        
        for(int x=0; x < 10; x++) {
            for(int y=0; y < 10; y++) {
                testMapData[x,y] = (Tile.TileTypes) 0;
            }
        }
        // Make a tough area
        for(int x=3; x <= 5; x++) {
            for(int y=0; y < 4; y++) {
                testMapData[x,y] = (Tile.TileTypes) 1;
            }
        }
		
        // u-shaped wall range
        testMapData[4, 4] = (Tile.TileTypes) 2;
        testMapData[5, 4] = (Tile.TileTypes) 2;
        testMapData[6, 4] = (Tile.TileTypes) 2;
        testMapData[7, 4] = (Tile.TileTypes) 2;
        testMapData[8, 4] = (Tile.TileTypes) 2;

        testMapData[4, 5] = (Tile.TileTypes) 2;
        testMapData[4, 6] = (Tile.TileTypes) 2;
        testMapData[8, 5] = (Tile.TileTypes) 2;
        testMapData[8, 6] = (Tile.TileTypes) 2;

        //closed are = not reachable
        testMapData[0, 8] = (Tile.TileTypes) 2;
        testMapData[1, 8] = (Tile.TileTypes) 2;
        testMapData[1, 9] = (Tile.TileTypes) 2;
        testMapData[0,10] = (Tile.TileTypes) 2;
    }


    public Object getSelectedObject()
    {
        return this.selectedObject;
    }
    public void setSelectedObject(GameObject obj)
    {
        this.selectedObject = obj;
    }

    public Tile GetTileAtPosition(Vector3 position)
    {
        int x = (int)position.x;
        int y = (int)position.y;
        if (x>=0 && x<= map.GetLength(0) && y>= 0 && y < map.GetLength(1) )
        {
            return map[(int)position.x,(int) position.y];
        }

        return null;
    }
}
