using UnityEngine;
using System.Collections;

public class BoomBlock : DirectionBlock {

    //set the rotations needed before the block goes Boom. Public so a designer can fiddle.
    public int rotationsToExplode;
   
	// Update is called once per frame
    public BoomBlock()
    {
       
    }

    void Awake()
    {

    }

	void Update () {        

        if(BlockRotations < rotationsToExplode)
        {
            UpdateBlock();
        }
        else
        {
            //Call the Game Manager to destroy this object and the tile it is on. Pivot is set as a child to this object so it gets destroyed along with it. Keep your backyard clean!
            Pivot.transform.parent = transform;
            GM.DestroyTile(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.z));
            GM.DestroyBlock(this.gameObject);            
        }
       
        
	}
}
