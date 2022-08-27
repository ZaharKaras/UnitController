using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public static InputHandler instance;

    private RaycastHit hit; //what we hit with out ray

    public List<Transform> selectedUnits = new List<Transform>();

    public Transform selectedBuilding;

    private bool isDragging = false;

    private Vector3 mousePosition;

    private void Awake()
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

    private void Update()
    {
        
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
                    case 10: // Minerals
                        selectedBuilding = hit.transform;
                        selectedBuilding.gameObject.SetActive(true);
                        break;
                    case 11: // Storage Building
                        selectedBuilding = hit.transform;
                        selectedBuilding.gameObject.SetActive(true);
                        break;
                    default: // if nothing happens
                        //do something
                        isDragging = true;
                        DeselectUnits();
                        if(selectedBuilding != null)
                        {
                            selectedBuilding.gameObject.SetActive(false);
                        }
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
                        hit.transform.Find("Highlight").gameObject.SetActive(true);

                        foreach (var unit in selectedUnits)
                        {
                            PlayerUnit pU = unit.gameObject.GetComponent<PlayerUnit>();
                            pU.MoveToAttack(hit.transform);
                            pU.isControlled = true;
                        }
                        break;
                    case 10: //minerals

                        hit.transform.Find("Highlight").gameObject.SetActive(true);

                        for (int i = 0; i < selectedUnits.Count; i++)
                        {
                            PlayerUnit pU = selectedUnits[i].gameObject.GetComponent<PlayerUnit>();
                            pU.isGathered = true;
                            pU.mine = hit.transform;
                        }
                        
                        break;
                    default: // if nothing happens
                        var position = GetPointPosition(selectedUnits.Count);
                        
                        for(int i = 0; i < selectedUnits.Count; i++)
                        {
                            PlayerUnit pU = selectedUnits[i].gameObject.GetComponent<PlayerUnit>();
                            pU.MoveUnit(hit.point + (position[i] * ((selectedUnits.Count)/(Mathf.PI))));
                        }
                        foreach (Transform unit in selectedUnits)
                        {
                            PlayerUnit pU = unit.gameObject.GetComponent<PlayerUnit>();
                            //pU.MoveUnit(hit.point);
                            pU.isControlled = false;
                            if(pU.aggroUnit != null)
                            {
                                pU.aggroTarget.Find("Highlight").gameObject.SetActive(false);
                            }
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
        if(selectedUnits.Count < 10)
        {
            selectedUnits.Add(unit);
            //set an obj on the unit clled Highlight
            unit.Find("Highlight").gameObject.SetActive(true);
        }
        
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

    public Vector3[] GetPointPosition(int count)
    {
        float step = Mathf.PI * 2 / count;
        List<Vector3> points = new List<Vector3>();

        for (var i = 0; i < count; i++)
        {
            points.Add(new Vector3(Mathf.Sin(i * step), 0f, Mathf.Cos(i * step)));
        }

        return points.ToArray();
    }
}
