using UnityEngine;
using System.Collections;

public class TileBehavior : MonoBehaviour {

    private int _xPos;
    private int _zPos;
    private bool _isAccessible;

    public TileData data;

    private float _startPosY;
    private float _lowPosY;
    private float _wallPosY;
    private Vector3 _startPosition;
    private Vector3 _lowPosition;
    private Vector3 _wallPosition;

    private bool _isOccupied;
    private bool _blockLeft;
    private bool _isWall;

    private GameManager _gameManager;

	// Use this for initialization
	void Start () {

        _xPos = Mathf.RoundToInt(transform.position.x);
        _zPos = Mathf.RoundToInt(transform.position.z);
        _startPosY = 0;
        _lowPosY = -0.2f;
        _wallPosY = 1.0f;
        _isAccessible = true;
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _startPosition = new Vector3(_xPos, _startPosY, _zPos);
        transform.position = _startPosition;
        _lowPosition = new Vector3(_xPos, _lowPosY, _zPos);
        _wallPosition = new Vector3(_xPos, _wallPosY, _zPos);

        data = new TileData();
        data.tilePosition = new Vector3(_xPos, _zPos);
        data.tileGameObject = this.gameObject;
        data.IsAccessible = _isAccessible;

        _isOccupied = false;
        _blockLeft = false;
        _isWall = false;
        
	}
	
	// Update is called once per frame
	void Update () {
	
        if(_isOccupied)
        {
            MoveTile(_lowPosition, Vector3.down);
        }

        else if(_blockLeft)
        {
            MoveTile(_startPosition, Vector3.up);            
        }

       

	}

    void OnTriggerEnter(Collider other)
    {
        _blockLeft = false;
        _isOccupied = true;
    }

    void OnTriggerExit(Collider other)
    {

        _isAccessible = false;        
        _isOccupied = false;
        _blockLeft = true;

    }

    private void MoveTile(Vector3 target, Vector3 direction)
    {
        transform.position += direction * Time.deltaTime;

        if(direction == Vector3.up)
        {
           if(transform.position.y >= target.y)
           {
               transform.position = target;
               
           }
        }
        else
        {
            if(transform.position.y <= target.y)
            {
                transform.position = target;
                
            }
        }
    }

    public Vector2 TilePosition
    {
        get { return new Vector2(_xPos, _zPos); }
    }

    public bool IsAccessible
    {
        get { return _isAccessible; }
        set { _isAccessible = value; }
    }

    public bool IsWall
    {
        set { _isWall = value; }
    }

    public Vector3 WallPosition
    {
        get { return _wallPosition; }
    }
}
