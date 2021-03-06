using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementT:MonoBehaviour
{
    BossAttackManagerT activateBossScript;

    public bool dashCooldown, dashing, bossCollide, rootSnared;

	float dashTime = 0.4f, dashCooldownTime = 1f, dashSpeed = 10, movementSpeed = 7.5f, rotateArrow ,arrowAngle, relativeAngle;
    public float angle;
    public Vector2 playerPosition;
	public enum Attack {Idle, Throw, AxeReturning, Melee};
	public Attack axeAttack;
    Rigidbody2D rgbd2D;
	Timer timerScript;
	public GameObject arrow, boss;
	Vector3 arrowDirection;



    void Start()
    {
        Time.timeScale = 1;
        rgbd2D = GetComponent<Rigidbody2D>();
		axeAttack = Attack.Idle;
        activateBossScript = GameObject.FindObjectOfType(typeof(BossAttackManagerT)) as BossAttackManagerT;
        timerScript = GameObject.FindObjectOfType(typeof(Timer)) as Timer;
		arrow.SetActive(false);
    }

    void Update()
    {
		if (timerScript.timeLeft <= 0f && (transform.position - boss.transform.position).magnitude > 17f)
		{
			arrow.SetActive(true);
			DirectionArrow();
		} else {
			arrow.SetActive(false);
		}
		
		
        if(Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            LookAtMouse(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }

        if(Input.GetAxisRaw("Melee") > 0f || Input.GetAxisRaw("Throw") > 0f)
        {
			LookAtMouse(transform.position + new Vector3(Input.GetAxisRaw("HorizontalAim"), Input.GetAxisRaw("VerticalAim"), 0));
        }

        if(axeAttack != Attack.Melee && axeAttack != Attack.Throw && axeAttack != Attack.AxeReturning)
        {
            playerPosition.x = Input.GetAxisRaw("Horizontal");
            playerPosition.y = Input.GetAxisRaw("Vertical");

            if((Input.GetButtonDown("Dash") || Input.GetKeyDown(KeyCode.LeftShift)) && !dashCooldown && playerPosition.magnitude != 0 && !rootSnared)
            {
                dashCooldown = true;
                dashing = true;
                rgbd2D.AddForce(playerPosition.normalized * dashSpeed, ForceMode2D.Impulse);
                Invoke(nameof(DashLength), dashTime);
            }

            if(!dashing && !rootSnared) rgbd2D.velocity = playerPosition.normalized * movementSpeed;
        }

		if (bossCollide == true)
		{
			rgbd2D.velocity = Vector2.zero;
            rgbd2D.AddForce(new Vector2(0f, -6f), ForceMode2D.Impulse);
			dashCooldown = false;
			dashing = false;
		}
	}

    void LookAtMouse(Vector2 mousePos)
    {
        rgbd2D.velocity = Vector2.zero;
        Vector2 lookDirection = mousePos - (Vector2)transform.position;
        rgbd2D.AddForce(lookDirection.normalized * 2, ForceMode2D.Impulse);
        angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
        
    }

	void DashLength()
	{
        dashing = false;
		axeAttack = Attack.Idle;
		rgbd2D.AddForce(Vector2.zero, ForceMode2D.Impulse);
        Invoke(nameof(DashCooldown), dashCooldownTime);
	}

    void DashCooldown()
    {
        dashCooldown = false;
    }

	 private void OnTriggerStay2D(Collider2D collision)
	{
	    if(collision.CompareTag("Axe") && axeAttack == Attack.AxeReturning)
	    {
			axeAttack = Attack.Idle;
			Destroy(collision.gameObject);
	    }
	}

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Boss") && !bossCollide)
        {
            bossCollide = true;
            Invoke(nameof(PlayerBossCollisionCooldown), 0.2f);
        }

        if(collision.CompareTag("BossRoom"))
        {
            activateBossScript.bossAwake = true;
			arrow.SetActive(false);
        }

        if(collision.CompareTag("Snare"))
        {
            rootSnared = true;
            rgbd2D.velocity = Vector3.zero;
            BossHP healBoss = GameObject.FindObjectOfType(typeof(BossHP)) as BossHP;
            healBoss.healing = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("BossRoom"))
        {
            activateBossScript.bossAwake = false;
        }

        if(collision.CompareTag("Snare"))
        {
            rootSnared = false;
            BossHP healBoss = GameObject.FindObjectOfType(typeof(BossHP)) as BossHP;
            healBoss.healing = false;
        }
    }

    void PlayerBossCollisionCooldown()
	{
		bossCollide = false;
		CancelInvoke();
	}
	
	void DirectionArrow()
	{
		arrowDirection = transform.position - boss.transform.position;
		arrowAngle = Mathf.Atan2(arrowDirection.y, arrowDirection.x) * Mathf.Rad2Deg;
		relativeAngle = arrow.transform.eulerAngles.z;
		
		if (relativeAngle - 180f > arrowAngle)
		{
			rotateArrow = -0.01f;
			
		} else {
			rotateArrow = 0.01f;
		}
		
		float arrowAngleDeadZone = Mathf.Abs(relativeAngle - 180f - arrowAngle);
		if (arrowAngleDeadZone < 1f)
		{
			rotateArrow = 0f;
		}
		
		arrow.transform.RotateAround (transform.position, new Vector3(0f, 0f, 1f), rotateArrow * Time.deltaTime * 5000f);	
	}
}
