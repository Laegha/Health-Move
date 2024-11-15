using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BochasCutsceneOnScored : PointScoreReciever
{
    [SerializeField] GameObject _distanceLinePrefab;
    [SerializeField] float lineDrawingSpeed = 1.0f;
    [SerializeField] float timeToEndRound = 1.0f;
    Bocha[] _scoringBochas;
    Transform _bochin;

    public override void OnScored(MinigameManager minigameManager)
    {
        base.OnScored(minigameManager);
        BochasMinigameManager bochasMinigameManager = minigameManager as BochasMinigameManager;

        _scoringBochas = bochasMinigameManager.ThrownBochas.Where(bocha => bocha.scoring).ToArray();
        _bochin = bochasMinigameManager.ThrownBochin.transform;
        StartCoroutine(_bochin.GetComponent<BochaCamera>().FocusBocha(ShowBochasDistances));
    }

    void ShowBochasDistances()
    {
        StartCoroutine(ShowBochasDistancesRoutine());
    }

    IEnumerator ShowBochasDistancesRoutine()
    {
        List<GameObject> lines = new List<GameObject>();
        foreach(var bocha in _scoringBochas)
        {
            LineRenderer lineRenderer = Instantiate(_distanceLinePrefab).GetComponent<LineRenderer>();
            lineRenderer.SetPosition(0, bocha.transform.position);
            lineRenderer.SetPosition(1, bocha.transform.position);
            lines.Add(lineRenderer.gameObject);
        
            bool drawingLine = true;
            float drewLine = 0;
            float distance = (_bochin.position - bocha.transform.position).magnitude;
            Vector3 drawDirection =  (_bochin.position - bocha.transform.position).normalized;
           
            while (drawingLine)
            {
                drewLine += Time.deltaTime * lineDrawingSpeed;
                lineRenderer.SetPosition(1, bocha.transform.position + drawDirection * drewLine);
                if(drewLine >= distance)
                    drawingLine = false;
                yield return null;
            }
        }
        yield return new WaitForSeconds(timeToEndRound);

        (GameManager.gm.CurrMinigameManager as BochasMinigameManager).RoundEnding = false;

        yield return new WaitForEndOfFrame();
        FindObjectOfType<CinemachineBrain>().ActiveVirtualCamera.Priority = 0;
        (GameManager.gm.CurrMinigameManager as BochasMinigameManager).PlayerCam.Priority = 1;
        lines.ForEach(line => Destroy(line));
    }
}
