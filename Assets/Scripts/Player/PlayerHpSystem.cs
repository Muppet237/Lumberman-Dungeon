using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHpSystem:MonoBehaviour
{
    [SerializeField] Slider sliderHealth;
    bool invincible, noPoison, lifeSteal, darkness;
    public bool isDead, bonfire, knockback;
    public float health;

    [SerializeField] GameObject[] armorSprite;
	Timer timerScript;

	public GameObject miniMap;
    public int armor;

    static float minionDamage = 5, bossDamage = 15, leafDamage = 8, branchDamage = 12;

	void Start()
	{
		timerScript = GameObject.FindObjectOfType(typeof(Timer)) as Timer;
		Invoke(nameof(Vincible), 1f);
	}

	void Update()
	{
        sliderHealth.value = health / 100;
        if(health <= 0)
        {
            isDead = true;
            timerScript.timeOut = false;
        }

        if(!bonfire && !darkness && !GetComponent<InventorySystem>().torchUsing)
		{
			miniMap.SetActive(false);
			Invoke(nameof(Poison), 1f);
		} else {
			miniMap.SetActive(true);
		}
        if(bonfire) Invoke(nameof(Heal), 0.2f);
        if(lifeSteal) Invoke(nameof(LifeSteal), 1f);
    }

    void Heal()
    {
        if(health < 100)
        {
            health += 2;
        }
        CancelInvoke(nameof(Heal));

    }

    void LifeSteal()
    {
        health -= 1.6f;
        CancelInvoke();
    }

    void Poison()
    {
        health -= Random.Range(2, 8);
        CancelInvoke(nameof(Poison));
    }

    public void TakeDamage(float damage)
    {
        invincible = true;
        knockback = true;
        if(armor > 0)
        {
            armor--;
            UpdateArmor();

        } else {
            health -= damage;
        }
        Invoke(nameof(Vincible), 0.1f);
    }

    public void UpdateArmor()
    {
        for(int i = 0; i < armorSprite.Length; i++)
        {
            if(i < armor)
            {
                armorSprite[i].SetActive(true);
            } else {
                armorSprite[i].SetActive(false);

            }
        }
    }

    void Vincible()
    {
        invincible = false;
        CancelInvoke();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(!invincible)
        {
            switch(collision.gameObject.tag)
            {
                case "Minion":
                    TakeDamage(minionDamage);
                    break;
                case "Boss":
                    TakeDamage(bossDamage);
                    break;
                case "Leaf":
                    TakeDamage(leafDamage);
                    break;
                case "Branch":
                    TakeDamage(branchDamage);
                    break;
                case "Light":
                    darkness = true;
                    break;
                case "Heal":
                    bonfire = true;
                    break;
                case "Snare":
                    lifeSteal = true;
                    break;
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        switch(collision.gameObject.tag)
        {
            case "Light":
                if(timerScript.timeOut)
                darkness = false;
                break;
            case "Heal":
                bonfire = false;
                break;
            case "Snare":
                lifeSteal = false;
                break;
        }
    }
}
