using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
public class BlockPlacer : MonoBehaviour
{
    public CubeManager CM;
    public Transform RaycastPoint;
    public Transform preBlock;
    public GameObject cursor;
    public InputActionProperty rightTrigger;
    public InputActionProperty leftTrigger;
    private bool hasClickedRight = false;
    private bool hasClickedLeft = false;

    private void Update()
    {
        float rightTriggerValue = rightTrigger.action.ReadValue<float>();
        float leftTriggerValue = leftTrigger.action.ReadValue<float>();

        Ray preRay = new Ray(RaycastPoint.position, RaycastPoint.forward);
        RaycastHit preHit;

        if (Physics.Raycast(preRay, out preHit))
        {
            preBlock.gameObject.SetActive(true);
            cursor.gameObject.SetActive(true);
            preBlock.transform.position = preHit.transform.position;

            // Calculate the offset direction away from the block
            Vector3 offsetDirection = -preHit.normal;
            float offsetDistance =- 0.001f; // Adjust this value as needed
            Vector3 cursorPosition = preHit.point + offsetDirection * offsetDistance;
            cursor.transform.position = cursorPosition;

            // Calculate the rotation based on the hit normal
            Quaternion rotation = Quaternion.LookRotation(-preHit.normal);
            cursor.transform.rotation = rotation;
        }
        else
        {
            preBlock.gameObject.SetActive(false);
            cursor.gameObject.SetActive(false);
        }

        if (rightTriggerValue == 1 && hasClickedRight == false)
        {
            hasClickedRight = true;

            Ray ray = new Ray(RaycastPoint.position, RaycastPoint.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                GameObject hitBlock = hit.collider.gameObject;
                Vector3 blockPosition = hitBlock.transform.position + hit.normal;
                print((Mathf.FloorToInt(blockPosition.x), Mathf.FloorToInt(blockPosition.y), Mathf.FloorToInt(blockPosition.z)));
                CM.SpawnCube(Mathf.FloorToInt(blockPosition.x), Mathf.FloorToInt(blockPosition.y), Mathf.FloorToInt(blockPosition.z), false);
            }
        }else if (rightTriggerValue == 0 && hasClickedRight == true){
            hasClickedRight = false;
        }

        if (leftTriggerValue == 1 && hasClickedLeft == false)
        {
            hasClickedLeft = true;

            Ray ray = new Ray(RaycastPoint.position, RaycastPoint.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform != null && hit.transform.gameObject != null)
                {
                    // Check if the object is a cube before destroying
                    if (hit.transform.gameObject.CompareTag("Cube"))
                    {
                        Destroy(hit.transform.gameObject);
                    }
                }
            }
        }else if (leftTriggerValue == 0 && hasClickedLeft == true){
            hasClickedLeft = false;
        }
    }
}