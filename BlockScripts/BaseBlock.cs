using UnityEngine;
using System.Collections;

public class BaseBlock : MonoBehaviour
{

    private Vector3 _direction;
    private Vector3 _position;
    private int _startPosY;
    private Color _blockColor;
    private Vector3 _scale;
    private bool _isRolling;
    private Vector3 _pivotPointPosition;
    private Quaternion _currentRotation;
    private Quaternion _targetRotation;
    private Vector3 _currentPosition;
    private Vector3 _storedDirection;
    private float _speed;
    private GameObject PivotPoint;
    private bool _isFalling;
    private GameManager gameManager;
    private int _rotations;

    public BaseBlock()
    {
        _direction = Vector3.zero;
        _startPosY = 1;
        _speed = 4.0f;
        _isFalling = false;
        _rotations = 0;
    }
 

	// Use this for initialization
	void Start () {   
        
        SetPosition();
        SetUpPivotPoint();
        _blockColor = transform.renderer.material.color;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
       
	}
	
	// Update is called once per frame
    public void UpdateBlock()
    {
        //Store position for future use
        _position = transform.position;
        if (!_isFalling) //only move if the block isn't already falling 
        {
            if (gameManager.CanMoveInDirection(_position, _direction)) //check if the tile the block wants to move to is accessible
            {
                if (!_isRolling) //Check if the block isn't already rolling
                {
                    if (_direction != Vector3.zero) //See if the block actually has a direction to travel in
                    {
                        //store the current rotation of the pivot point
                        _currentRotation = PivotPoint.transform.rotation;
                        //store the current position of the block
                        _currentPosition = transform.position;
                        //Set the pivot point to the rolling edge of the cube (Centerpoint of the block + half the scale on X/Z axis (multiplied by direction to figure out the axis, 
                        //half the scale down on y-axis to put it on the floor.
                        _pivotPointPosition = new Vector3(_position.x + _scale.x * _direction.z,
                                                       _position.y - _scale.y,
                                                    _position.z + _scale.z * _direction.x);
                        
                        //Set the new position for the pivot point
                        PivotPoint.transform.position = _pivotPointPosition;
                        //Make the cube a child of the pivot point so that when the pivot point rotates, the cube follows.
                        transform.parent = PivotPoint.transform;

                        //Store the movement direction for good measure
                        _storedDirection = _direction;

                        //X and Z axis rotation are handled differently. If not, the block will climb vertically. Weird. 
                        if (_storedDirection.x != 0)
                        {

                            _storedDirection.z = 0;
                            //Set the target rotation. Current rotation + 90 degrees over an angle determined by the direction.
                            _targetRotation = Quaternion.Euler(_currentRotation.eulerAngles + 90 * _storedDirection);
                        }

                        if (_storedDirection.z != 0)
                        {
                            _storedDirection.x = 0;
                            //Use negative direction to keep the cube from spazzing out. Might have been better to use Y and X axis and make everything vertical?
                            _targetRotation = Quaternion.Euler(_currentRotation.eulerAngles + 90 * _storedDirection * -1);
                        }

                        //Rolling is true so new input won't interrupt the current roll half-way through.
                        _isRolling = true;

                    }

                }
            }
            else
            {
                //If there is an inaccessible tile, do something about it. 
                HandleInaccessibleBlock();
            }

            if (_isRolling)
            {
                //While rolling is true, perform this function, pass the target rotation.
                Roll(_targetRotation);
            }
        }
    }

    public void Roll( Quaternion targetRotation)
    {
        //Since a cube always rolls over its leading edge, RotateTowards sounded great. Sadly, this function works on the pivot point of the object, which is always the dead-center.
        //To compenstate, I work with a pivot point game object that becomes a temporary parent for the Block, positions on the rolling edge and then rotates. Seems to work!
        PivotPoint.transform.rotation = Quaternion.RotateTowards(PivotPoint.transform.rotation,
                                           targetRotation,
                                           _speed);

        //When the rotation is complete.
        if(PivotPoint.transform.rotation == targetRotation)
        {
            //Store number of rotations made. This is used in the BoomBlock so we can tell it when to explode.
            _rotations++;
            //Make sure all the positions are set clearly.
            SetPosition();
            //No longer rolling!
            _isRolling = false; 
            //Detach block from pivot point. 
            transform.parent = null;
            //Reset pivot point rotation, NOT the block rotation. Otherwise the block just 'plops back' once it's moved to the next tile. Ugly. 
            PivotPoint.transform.eulerAngles = Vector3.zero;
            //Set the new position for the Block.
            Vector3 newPosition = new Vector3(transform.position.z + _storedDirection.z, _startPosY, transform.position.x + _storedDirection.x);
        }
                                                               
    }


       

    //Position of the Block on X an Z axis is rounded off, Y position is given.
    private void SetPosition()
    {       
        _position = new Vector3(Mathf.Round(transform.position.x), _startPosY, Mathf.Round(transform.position.z));
        transform.position = _position;        
        _scale = transform.localScale / 2;
    }

    //Initial settings for the Pivot point
    private void SetUpPivotPoint()
    {
        PivotPoint = new GameObject();
        PivotPoint.name = transform.name + " Pivot Point";
        PivotPoint.transform.position = _position;
    }
    // Virtual method so I can change it in classes that inherit from this one. As it is, it just stops the block from moving. Convenient for the player.
    public virtual void HandleInaccessibleBlock()
    {
        return;
    }
    
    // Bunch of getters and setters
    public Vector3 Direction
    {
        get { return _direction; }
        set { _direction = value; }
    }

    public Color BlockColor
    {
        get { return _blockColor; }       
    }    
   
    public GameObject Pivot
    {
        get { return PivotPoint; }
    }

    public bool IsRolling
    {
        get { return _isRolling; }
        set { _isRolling = value; }
    }

    public float Speed
    {
        get { return _speed; }
    }

    public bool IsFalling
    {
        get { return _isFalling; }
        set { _isFalling = value; }
    }

    public int BlockRotations
    {
        get { return _rotations; }
    }

    public GameManager GM
    {
        get { return gameManager; }
    }
}
