using UnityEngine;
using System.Collections;
using System;

public class DirectionBlock : BaseBlock {

    //Have it public so it shows up in the inspector.
    public StartDirections StartDirection;

    //While this is public, I don't want designers to mess around with this. It's just a string I need for a switch, and for other classes to inherit.
    [System.NonSerialized]
    public string _startDirection;    

    //Pass the selected direction to the block itself. Note again, up is right, down is left etc. etc. I really need to fix that. 
    public DirectionBlock()
    {
        _startDirection = StartDirection.ToString();

        switch (_startDirection)
        {
            case "Up":
                Direction = Vector3.right;
                break;

            case "Down":
                Direction = Vector3.left;
                break;

            case "Left":
                Direction = Vector3.back;
                break;

            case "Right":
                Direction = Vector3.forward;
                break;
        }
    }
   
	// Update is called once per frame
	public void Update () {
        UpdateBlock();
	}
    
    //Override the function from BaseBlock. Instead of stopping the block from moving in this direction we're sending it in the opposite direction instead.
    public override void HandleInaccessibleBlock()
    {
        Direction *= -1;
    }
}

//Using an enum to get a dropdown menu in the inspector. Designers get to use this.
public enum StartDirections
{
    Up,
    Down,
    Left,
    Right
}