using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = System.Object;

[Serializable]
[RequireComponent(typeof(Collider))]
public class Tile : MonoBehaviour
{
    // Start is called before the first frame update
    public int positionX;
    public int positionY;
    public GameObject tilePrefab;
    public List<Tile> neighbors;
    public int movementCost;
    public int illumination;
    public int sound;
    public TileMap map;
    private bool highlighted = false;
    private Color basecolor;
    public bool isWalkable = false;
    private MeshRenderer _renderer;
    void Start()
    {
        
        basecolor = tilePrefab.GetComponent<MeshRenderer>().materials[0].color;
        calcNeighbors();
        _renderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void calcNeighbors()
    {
        this.map = GameObject.FindObjectOfType<TileMap>();
        neighbors = new List<Tile>();
        this.positionX = (int)transform.position.x;
        this.positionY = (int)transform.position.y;
        if (this.isWalkable)
        {
            if (positionX < map.map.GetLength(0)-1)
            {
                this.neighbors.Add(map.map[positionX + 1 , positionY]);
            }
            if (positionX > 0)
            {
                this.neighbors.Add(map.map[positionX - 1 , positionY]);
            }
            if (positionY < map.map.GetLength(1)-1)
            {
                this.neighbors.Add(map.map[positionX , positionY +1]);
            }
            if (positionY > 0)
            {
                this.neighbors.Add(map.map[positionX, positionY -1]);
            }
        }
    }

    public void highlight(Color color)
    {
        if (highlighted)
        {
            highlighted = false;
            _renderer.materials[0].color = basecolor;
        }

        else
        {
            highlighted = true;
            _renderer.materials[0].color = color;
        }
    
    }

    public void UnHighlight()
    {
        _renderer.materials[0].color = basecolor;
    }
    
    private void OnMouseDown()
    {
        /*Ray ray;
        RaycastHit hitInfo;
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.Log("Feuer");
        if (GetComponent<Collider>().Raycast(ray, out hitInfo, Mathf.Infinity))
        {
            Debug.Log("treffer");
            hitInfo.collider.GetComponent<MeshRenderer>().material.color = Color.yellow;
            if ()
            {
                
            }
        }*/
        /*map.setSelectedObject(this);
        if (map.getSelectedObject() is Tile)
        {
            map.getSelectedObject()
        }
        this.GetComponent<MeshRenderer>().material.color = Color.yellow;*/
    }

    public enum TileTypes
    {
        TileBasic,
        TileTough,
        TileWall,
    };
}
