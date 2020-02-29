using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

/// <summary>
/// This class is used to store the GameObject and the amount of said GameObject as the value in the
/// key-value pair.
/// NOTE: In a different program the value would be as simple as the amount. I choose to store the GameObject as well
/// as a short way to tell the program what GameObject to spawn when the respawn button is pressed.
/// </summary>
public class inventoryItem {

    public inventoryItem(GameObject itm, int am)
    {
        item = itm;
        amount = am;
    }

    public void increaseAmount()
    {
        amount += 1;
    }

    public void decreaseAmount()
    {
        amount--;
    }

    public GameObject item;
    public int amount;
}

public class HashInventory : MonoBehaviour
{
    public int numbOfInventorySlots;
    public HashMap<string, inventoryItem> inventory;

    public GameObject inventoryMenu;
    GameObject inventoryMenuPanel;
    public GameObject inventoryTextPrefab;
    static public bool showInv = false;

    public GameObject environmentParent;

    void Awake()
    {
        inventory = new HashMap<string, inventoryItem>(numbOfInventorySlots);
        inventoryMenuPanel = inventoryMenu.GetComponent<InvetoryMenuPanel>().panel;
    }

    void Update()
    {
        ///This portion of code handles showing the inventory UI panel. It also changes the TimeScale to change the player's ability to move.
        if (Input.GetKeyDown(KeyCode.E))
        {
            showInv = !showInv;
  
            if (showInv)
            {
                Time.timeScale = 0;
                inventoryMenu.gameObject.SetActive(true);
                displayTable();
            }
            else
            {
                Time.timeScale = 1;
                eraseTable();
                inventoryMenu.gameObject.SetActive(false);
            }
        }
        /// This next chunk of code allows for the GameObjects in the player's inventory to be
        /// respawned at random once the 'Q' button is pressed.
        else if (Input.GetKeyDown(KeyCode.Q)) 
        {
            if (!showInv && inventory.getKeys().Count > 0)
            {
                string[] keys = inventory.getKeys().ToArray();
                int rand = Random.Range(0, keys.Length);

                GameObject tempGO = Instantiate(inventory.get(keys[rand]).item, environmentParent.transform);

                tempGO.transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z) + transform.forward * 2f;
                tempGO.name = inventory.get(keys[rand]).item.name;

                tempGO.SetActive(true);

                inventory.get(keys[rand]).decreaseAmount();

                if (inventory.get(keys[rand]).amount == 0)
                {
                    Destroy(inventory.get(keys[rand]).item);
                    inventory.remove(keys[rand]);
                }
            }
        }
        ///This portion of code resets the current scene.
        else if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    /// <summary>
    /// Removes all text children of the inventory panel when the inventory is not being shown.
    /// </summary>
    void eraseTable()
    {
        int childCount = inventoryMenuPanel.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Destroy(inventoryMenuPanel.transform.GetChild(i).gameObject);
        }
    }

    /// <summary>
    /// Creates and displays the text information for all of the items currently in the players inventory.
    /// </summary>
    void displayTable()
    {
        int count = 1;
        int textPos = -50;
        foreach (string key in inventory.getKeys().ToArray())
        {
            if (inventory.getKeys().ToArray().Length != 0)
            {
                GameObject textComp = Instantiate(inventoryTextPrefab, inventoryMenuPanel.transform);
                string currentItem = "Invetory slot " + count + ": " + inventory.get(key).item.name + " x" + inventory.get(key).amount;
                Debug.Log("Invetory slot " + count + ": " + inventory.get(key).item.name + " x" + inventory.get(key).amount);

                textComp.GetComponent<TMPro.TextMeshProUGUI>().text = currentItem;
                textComp.transform.position += new Vector3(100, textPos, 0);

                count++;
                textPos -= 50;
            }
        }
    }
}
