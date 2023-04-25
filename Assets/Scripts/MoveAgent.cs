using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.AI;

public class MoveAgent : MonoBehaviour
{

    [SerializeField] Transform _endGoal;
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
            agent.SetDestination(_endGoal.position);
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

