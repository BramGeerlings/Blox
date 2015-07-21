using UnityEngine;
using System.Collections;

public class WallerBlock : DirectionBlock {

    private Quaternion _rollTo;
  
    private Quaternion rotateTo;
    private Vector3 _pivotFallPos;
    
    private float _dropSpeed;

    private GameObject _tile;
    private TileBehavior _tileBehavior;

    // Use this for initialization
    public void Awake()
    {        
        _dropSpeed = Speed;       
        _tile = Resources.Load("Prefabs/WallPrefab") as GameObject;
    }
	
	// Update is called once per frame
    void Update()
    {
        UpdateBlock();
        
        if (!IsRolling && ! IsFalling)
        {
            //GameObject instantiatedTile = Instantiate(_tile,transform.position,Quaternion.identity) as GameObject;            
        }

        //When this block reaches the edge of a tile (the next tile in this direction is empty), it needs to fall. Hence the boolean.
        if (IsFalling)
        {
            FallOffEdge();
        }
    }



    public override void HandleInaccessibleBlock()
    {
       
        if(!IsRolling)
        {
            
            if (!IsFalling)
            {
                //Because we're not using the UpdateBlock function, I need to set the pivot point manually.
                SetNewPivot();
            }            
        }      
    }

    private void SetNewPivot()
    {
        
        _pivotFallPos = Pivot.transform.position;
        _pivotFallPos.x += Direction.z;
        _pivotFallPos.z += Direction.x;
        Pivot.transform.position = _pivotFallPos;
        if (Direction.z != 0)
        {
            rotateTo = Quaternion.Euler(Pivot.transform.rotation.eulerAngles + 180 * Direction);
        }
        else
        {
            rotateTo = Quaternion.Euler(Pivot.transform.rotation.eulerAngles - 180 * Direction);
            
        }        
        transform.parent = Pivot.transform;
        
        IsFalling = true;
    }

    //Function that sends the block over the edge, then destroys it. Could add something to make it shrink as it falls if using an Orthographic camera. (transform.localScale multiplied by 0.Something over time)
    private void FallOffEdge()
    {
        
        Pivot.transform.rotation = Quaternion.RotateTowards(Pivot.transform.rotation,
                                           rotateTo,
                                           Speed);
       
        if(Pivot.transform.rotation == rotateTo)
        {
            _dropSpeed++;
            transform.position -= Vector3.up * _dropSpeed * Time.deltaTime;
        }

        if(transform.position.y < -20)
        {
            Destroy(this.transform.parent.gameObject);
        }
    }

  
}
