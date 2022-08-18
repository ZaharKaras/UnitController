using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public static InputHandler instance;

    private RaycastHit hit; //what we hit with out ray

    public List<Transform> selectedUnits = new List<Transform>();

    private bool isDragging = false;

    private Vector3 mousePosition;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        instance = this;
    }

    private void OnGUI()
    {
        if(isDragging)
        {
            Rect rect = MultiSelect.GetScreenRect(mousePosition, Input.mousePosition);
            MultiSelect.DrawScreenRect(rect, new Color(0f, 0f, 0f, 0.25f));
            MultiSelect.DrawScreenRectBorder(rect, 3, Color.green);
        }
    }
    public void HandleUnitMovement()
    {
        if(Input.GetMouseButtonDown(0))
        {
            mousePosition = Input.mousePosition;
            //create a ray
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //check if we hit something
            if (Physics.Raycast(ray, out hit))
            {
                //if we do, then do somehting with that dat
                LayerMask layerHit = hit.transform.gameObject.layer;

                switch(layerHit.value)
                {
                    case 8: // Units layer
                        //do something
                        SelectUnit(hit.transform, Input.GetKey(KeyCode.LeftShift));
                        break;
                    default: // if nothing happens
                        //do something
                        isDragging = true;
                        DeselectUnits();
                        break;
                }
            }
        }

        if(Input.GetMouseButtonUp(0))
        {
            
            foreach(Transform child in PlayerManager.instance.playerUnits)
            {
                foreach(Transform unit in child)
                {
                    if(isWithingSelectionBounds(unit))
                    {
                        SelectUnit(unit, true);
                    }
                }
            }
            isDragging = false;
        }

        if(Input.GetMouseButtonDown(1) && HaveSelectedUnits())
        {
            //create a ray
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //check if we hit something
            if (Physics.Raycast(ray, out hit))
            {
                //if we do, then do somehting with that dat
                LayerMask layerHit = hit.transform.gameObject.layer;

                switch (layerHit.value)
                {
                    case 8: // Units layer
                        //do something
                        break;
                    case 9://enemy unit
                        //attack
                        break;
                    default: // if nothing happens
                        //do something
                        foreach(Transform unit in selectedUnits)
                        {
                            PlayerUnit playerUnit = unit.gameObject.GetComponent<PlayerUnit>();
                            playerUnit.MoveUnit(hit.point);
                        }
                        break;
                }
            }
        }
    }

    private void SelectUnit(Transform unit, bool canMultiselect)
    {
        if(!canMultiselect)
        {
            DeselectUnits();
        }
        
        selectedUnits.Add(unit);
        //set an obj on the unit clled Highlight
        unit.Find("Highlight").gameObject.SetActive(true);
    }

    private void DeselectUnits()
    {
        for(int i = 0; i < selectedUnits.Count; i++)
        {
            selectedUnits[i].Find("Highlight").gameObject.SetActive(false);
        }

        selectedUnits.Clear();
    }

    private bool isWithingSelectionBounds(Transform tf)
    {
        if(!isDragging)
        {
            return false;
        }

        Camera camera = Camera.main;
        Bounds vpBounds = MultiSelect.GetViewPointBounds(camera, mousePosition, Input.mousePosition);
        return vpBounds.Contains(camera.WorldToViewportPoint(tf.position));
    }

    private bool HaveSelectedUnits()
    {
        if (selectedUnits.Count > 0)
            return true;

        return false;
    }
}
