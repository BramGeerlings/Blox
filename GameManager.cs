using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    //Gather all the tiles in the level
    GameObject[] Tiles;
    //Create a dictionary where a tile position is the key. Using a Struct called TileData to sneak in more than one value. I need a boolean and a Game Object.  
    Dictionary<Vector2, TileData> TileLookUp;
    
    //Get the tile script so we can play with it when we find them.
    TileBehavior tileBehavior;

    //Just to make sure the tiles are actually updated when the function is called.
    private bool _tilesUpdated;

    void Awake()
    {
        _tilesUpdated = false;
        
    }

	// Use this for initialization
	void Start () {

        UpdateTileLookUp();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void LateUpdate()
    {
        //Tiles get updated after everything else is, just to make sure nothing gets missed.
       if(_tilesUpdated)
       {
           UpdateTileLookUp();           
           _tilesUpdated = false;
       }
    }

    public void UpdateTileLookUp()
    {
        //Find all the tiles by using a tag.
        Tiles = GameObject.FindGameObjectsWithTag("Tile");
        
        //Instantiate a new Dictionary when the function is called. 
        TileLookUp = new Dictionary<Vector2, TileData>();
        
        foreach(GameObject tile in Tiles)
        {
            tileBehavior = tile.GetComponent<TileBehavior>();         
            // add all the tiles in the Tile list to the dictionary
            TileLookUp.Add(tileBehavior.TilePosition, tileBehavior.data);
        }
        
    }

    //Check if the tile we're moving to is accessible. We need a current position and the direction we're travelling in.
    public bool CanMoveInDirection(Vector3 position, Vector3 direction)
    {
        //Making sure the current position is 100% rounded to an int to prevent missing a key because of a floating point somewhere.
        Vector2 _position = new Vector2(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.z));
        Vector2 _direction = new Vector2(direction.z, direction.x);

        //The tile we're moving to is equal to our current position and our direction.
        Vector2 _targetPosition = _position + _direction;
        
        //If there's no key, the tile is either outside the level boundaries or was destroyed by a BoomBlock. Or maybe the tile exists but is inaccessible. 
        if (!TileLookUp.ContainsKey(_targetPosition) || !TileLookUp[_targetPosition].IsAccessible)
        {            
            return false;            
        }
        else
        {           
            //if the tile exists and is accessible, we tell out program it is!
            return true;
        }        
    }

    public void DestroyBlock(GameObject block)
    {
        GameObject.Destroy(block);
    }

    public void DestroyTile(float xPos, float zPos)
    {       
        //This is where using a Struct for the value pays off. If I hadn't used one, I would've only been able to find the Accessible boolean, OR the gameobject. Now I can have both.
        //Look up the tile by its position, get the the GameObject associated with it, destroy it and then make sure the Dictionary is updated.
        Destroy(TileLookUp[new Vector2(xPos,zPos)].tileGameObject);
        _tilesUpdated = true;
    }
}
