using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DragAndDrop : MonoBehaviour
{
    private bool _mouseState;
    private GameObject target;
    private Vector3 originalScale;
    private bool _scaleUp;
    public Vector3 screenSpace;
    public Vector3 offset;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            RaycastHit hitInfo;
            target = GetClickedObject(out hitInfo);
            if (target != null)
            {
                _mouseState = true;
                _scaleUp = Input.GetMouseButtonDown(0); // True if left click, false if right click
                originalScale = target.transform.localScale;
                screenSpace = Camera.main.WorldToScreenPoint(target.transform.position);
                offset = target.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenSpace.z));
            }
        }

        if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
        {
            if (target != null)
            {
                ChangeScale(_scaleUp);
                target = null;
            }
            _mouseState = false;
        }

        if (_mouseState && target != null)
        {
            // Keep track of the mouse position
            var curScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenSpace.z);

            // Convert the screen mouse position to world point and adjust with offset
            var curPosition = Camera.main.ScreenToWorldPoint(curScreenSpace) + offset;

            // Update the position of the object in the world
            target.transform.position = curPosition;
        }
    }

    public void ChangeScale(bool scaleUp)
    {
        float scaleFactor = scaleUp ? 1.5f : 1 / 1.5f; // Increase or decrease scale
        target.transform.localScale = originalScale * scaleFactor;

        Vector3 pos = target.transform.position;
        pos.z += scaleUp ? 1.80f * (scaleFactor - 1) : -1.80f * (1 - scaleFactor); // Adjust position based on scale change
        target.transform.position = pos;
    }

    GameObject GetClickedObject(out RaycastHit hit)
    {
        GameObject target = null;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray.origin, ray.direction * 10, out hit) && hit.collider.gameObject.tag != "Floor")
        {
            target = hit.collider.gameObject;
        }

        return target;
    }
}


