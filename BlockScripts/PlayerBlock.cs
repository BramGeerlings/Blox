using UnityEngine;
using System.Collections;

public class PlayerBlock : BaseBlock {

    public Color blockColor;

    void Awake()
    {
        
    }
	
	void Update () {
        //Call the update of the block
        UpdateBlock();
        //Direction is based on user input. Up and down is left and right, and the other way around. 
        Direction = new Vector3(Input.GetAxisRaw("Vertical"), 0, Input.GetAxisRaw("Horizontal"));
	}

   
}
