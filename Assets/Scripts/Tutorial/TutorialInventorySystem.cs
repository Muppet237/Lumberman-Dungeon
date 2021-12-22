using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialInventorySystem:MonoBehaviour
{
	[SerializeField] GameObject bonFirePrefab, torchPrefab, treePrefab;
    [SerializeField] Text seedText, vineText, torchText;
    public bool seedBool, vineBool, torchBool, logBool;
	float bonFireTimer = 10, torchTimer = 5;
    public bool maxCapacity = false;
    public int seedInt, vineInt, torchInt;
    PlayerHpSystemT playerHpScript;
    GameObject holdResource;
	Debugger debuggerScript;

    EarthMoundT earthMoundScript;
    VineT vineScript;
    LogT logScript;

    void Start()
    {
        playerHpScript = GetComponent<PlayerHpSystemT>();
		debuggerScript = GameObject.FindObjectOfType(typeof(Debugger)) as Debugger;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) || Input.GetButtonDown("Pickup"))
        {
            if(seedBool && !earthMoundScript.taken)
            {
                AddSeed();
            } else if(vineBool && !vineScript.taken) {
                AddVine();
            } else if(torchBool) {
                AddTorch();
            } else if(logBool) {
                AddArmor();
            }
        }
		
		if (Input.GetKeyDown(KeyCode.Alpha1))// || Input.GetButtonDown("UseTorch"))
		{
            PlaceTorch();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))// || Input.GetButtonDown("Placetree"))
		{
            PlaceTree();
		}
		
		if (Input.GetKeyDown(KeyCode.Alpha3))// || Input.GetButtonDown("PlaceBonFire"))
		{
            PlaceBonfire();
		}
		
		// Debug code
		if (debuggerScript.addInventorySeed)
		{
			AddSeed();
			debuggerScript.addInventorySeed = !debuggerScript.addInventorySeed;
		}
		if (debuggerScript.addInventoryVine)
		{
			AddVine();
			debuggerScript.addInventoryVine = !debuggerScript.addInventoryVine;
		}
		if (debuggerScript.addInventoryTorch)
		{
			AddTorch();
			debuggerScript.addInventoryTorch = !debuggerScript.addInventoryTorch;
		}
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        holdResource = collision.gameObject;
        switch(collision.gameObject.tag)
        {
            case "Seed":
                seedBool = true;
                earthMoundScript = collision.GetComponent<EarthMoundT>();
                break;
            case "Vine":
                vineBool = true;
                vineScript = collision.GetComponent<VineT>();
                break;
            case "Tourch":
                torchBool = true;
                break;
            case "Log":
                logBool = true;
                logScript = collision.GetComponent<LogT>();
                break;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        holdResource = null;
        seedBool = false;
        vineBool = false;
        torchBool = false;
        logBool = false;
    }

    void AddSeed()
    {
        if(Random.Range(0, 100) > 50) seedInt++;
        earthMoundScript.taken = true;
        earthMoundScript.ChangeSprite();
        seedText.text = string.Format("{0:0}", seedInt);
    }

    void AddVine()
    {
        vineInt++;
        vineScript.taken = true;
        vineScript.ChangeSprite();
        vineText.text = string.Format("{0:0}", vineInt);
    }

    void AddTorch()
    {
        torchInt++;
        torchText.text = string.Format("{0:0}", torchInt);
        Destroy(holdResource);
    }

    void AddArmor()
    {
        if(playerHpScript.armor < 3 && !logScript.taken)
        {
            playerHpScript.armor++;
            logScript.taken = true;
            logScript.ChangeSprite();
            playerHpScript.UpdateArmor();
        }
    }
	
	void PlaceBonfire()
	{
		if (vineInt >= 4)
		{
			vineInt -= 4;
			vineText.text = vineInt.ToString();
			GameObject bonFireinstance = Instantiate(bonFirePrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z),  Quaternion.identity);
			Destroy(bonFireinstance, bonFireTimer);
            Invoke(nameof(DestroyBon), bonFireTimer);
		}
	}
	
    void DestroyBon()
    {
        playerHpScript.healing = false;
        CancelInvoke(nameof(DestroyBon));
    }

	void PlaceTorch()
	{
		if (torchInt > 0)
		{
			torchInt--;
			torchText.text = torchInt.ToString();
			GameObject torchinstance = Instantiate(torchPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z),  Quaternion.identity);
			Destroy(torchinstance, torchTimer);
		}
	}


    void PlaceTree()
	{
		if (seedInt > 0)
		{
			seedInt--;
			seedText.text = seedInt.ToString();
			GameObject treeinstance = Instantiate(treePrefab, new Vector3(transform.position.x, transform.position.y + 1, transform.position.z),  Quaternion.identity);
		}
	}
}
