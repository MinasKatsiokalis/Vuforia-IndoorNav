using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.AI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;

public class MoveAgent : MonoBehaviour
{
    [SerializeField] TMP_Dropdown _dropdownList;

    [SerializeField] Transform _endGoal1;
    [SerializeField] Transform _endGoal2;
    [SerializeField] Transform _endGoal3;

    Transform endGoal;


    [SerializeField] LineRenderer _line;
    [SerializeField] GameObject _pointPrefab;

    private NavMeshAgent agent;

    private bool navigation;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.isStopped = true;
        navigation = false;
        endGoal = _endGoal1;

        _dropdownList.onValueChanged.AddListener(delegate {
            DropdownValueChanged(_dropdownList);
        });
    }

    // Update is called once per frame
    void Update()
    {
        agent.transform.position = new Vector3(Camera.main.transform.position.x, agent.transform.position.y, Camera.main.transform.position.z);

        /*if (Input.GetMouseButtonDown(0))
        {
            
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(ray, out hit))
            {
                agent.SetDestination(hit.point);
                Invoke("DrawPoints", 0.1f);
            }
        }*/
    }
    void DropdownValueChanged(TMP_Dropdown change)
    {
        if (change.value == 0)
            endGoal = _endGoal1;
        else if(change.value == 1)
            endGoal = _endGoal2;
        else
            endGoal = _endGoal3;

    }


    public void StartNavigation()
    {   
        if(navigation == false)
        {
            navigation = true;
            StartCoroutine(SetNavigationPath());
        }
        else
        {
            navigation = false;
            StopCoroutine(SetNavigationPath());

            _line.positionCount = 0;
            foreach (GameObject go in points)
            {
                Destroy(go);
            }
            points.Clear();
        }
    }

    IEnumerator SetNavigationPath()
    {   
        while(navigation == true)
        {
            agent.SetDestination(endGoal.position);
            yield return new WaitForEndOfFrame();

            DrawPoints();
            DisplayLineDestination();

            yield return new WaitForSeconds(1f);
        }
    }


    void DisplayLineDestination()
    {
        _line.positionCount = agent.path.corners.Length;
        _line.SetPositions(agent.path.corners);
    }

    private List<GameObject> points = new List<GameObject>();
    void DrawPoints()
    {   
        foreach(GameObject go in points)
        {
            Destroy(go);
        }
        points.Clear();

        for (int i = 0; i < agent.path.corners.Length; i++)
        {
            points.Add(Instantiate(_pointPrefab, agent.path.corners[i], new Quaternion(0, 0, 0, 0)));
        }
    }
}

