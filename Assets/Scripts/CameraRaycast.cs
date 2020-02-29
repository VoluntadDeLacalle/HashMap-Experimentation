using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CameraRaycast : MonoBehaviour
{
    Camera cam;
    HashInventory HS;
    Ray ray;

    bool showInteractiveText;
    public TextMeshProUGUI interactiveText;

    void Awake()
    {
        cam = GetComponent<Camera>();
        HS = GetComponent<HashInventory>();
        showInteractiveText = false;
    }

    /// <summary>
    /// In this Update function a raycast is sent out from the camera to allow the player to pick up objects to add to their inventory.
    /// </summary>
    void Update()
    {
        ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit outHit;
        
        if (showInteractiveText)
        {
            interactiveText.gameObject.SetActive(false);
            showInteractiveText = false;
        }

        if (Physics.Raycast(ray, out outHit, 3))
        {

            if (outHit.transform.gameObject.GetComponent<PickUp>() != null)
            {

                if (!showInteractiveText)
                {
                    interactiveText.gameObject.SetActive(true);
                    showInteractiveText = true;
                }

                ///This section checks the player's input and attempts two different things.
                ///1. If the object is not found in the current HashMap, it will be put their using the put function.
                ///2. If the object is found in the current HashMap, it will increase the amount of said GameObject.
                if (Input.GetMouseButtonDown(0) && !HashInventory.showInv)
                {
                    if (!HS.inventory.find(outHit.transform.gameObject.tag))
                    {
                        HS.inventory.put(outHit.transform.gameObject.tag, new inventoryItem(outHit.transform.gameObject, 1));
                        outHit.transform.gameObject.SetActive(false);
                    }
                    else
                    {
                        HS.inventory.get(outHit.transform.gameObject.tag).increaseAmount();
                        Destroy(outHit.transform.gameObject);
                    }

                }

            }
        }
    }
}
