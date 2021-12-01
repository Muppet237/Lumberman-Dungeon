using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack:MonoBehaviour
{
	PlayerMovement playerMovementScript;
    [SerializeField] GameObject axePrefab, axeOffset;
	[SerializeField] GameObject axeAttackPrefab;
    float axeThrowForce = 20;
    Rigidbody2D rgbd2D;
	
	void Start()
	{
		playerMovementScript = GameObject.FindObjectOfType(typeof(PlayerMovement)) as PlayerMovement;
	}

    void Update()
    {

        if(Input.GetMouseButtonDown(0) && playerMovementScript.attacking < 1)
        {
            GetAngle(Camera.main.ScreenToWorldPoint(Input.mousePosition), 0);

        } else if(Input.GetMouseButtonDown(1) && playerMovementScript.attacking < 1) {
            GetAngle(Camera.main.ScreenToWorldPoint(Input.mousePosition), 1);
        }
    }

    void GetAngle(Vector2 mousePos, int mouseInput)
    {
        Vector2 lookDirection = mousePos - (Vector2)transform.position;
        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
        axeOffset.transform.rotation = Quaternion.Euler(0, 0, angle);
        switch(mouseInput)
        {
            case 0:
                MeeleAxe(angle);
                break;
            case 1:
                ThrowingAxe(angle);
                break;
        }
    }

    void ThrowingAxe(float angle)
    {
		playerMovementScript.attacking = 1;
        GameObject axe = Instantiate(axePrefab, axeOffset.transform.position, axeOffset.transform.rotation);
        rgbd2D = axe.GetComponent<Rigidbody2D>();
        rgbd2D.AddForce(axe.transform.right * axeThrowForce, ForceMode2D.Impulse);
    }

	void MeeleAxe(float angle)
	{
		playerMovementScript.attacking = 1;
        GameObject axeAttack = Instantiate(axeAttackPrefab, axeOffset.transform.position + new Vector3(1, 0, 0), Quaternion.identity);
        rgbd2D = axeAttack.GetComponent<Rigidbody2D>();
        axeAttack.transform.RotateAround(axeOffset.transform.position, Vector3.forward, angle);
    }
}
