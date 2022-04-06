function Update () {
	renderer.material.color = 
		GetComponent.<CharacterController>().isGrounded ? Color.green : Color.red;
}