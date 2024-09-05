using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class LineScript : MonoBehaviour
{
    [SerializeField] private List<GameObject> particles;
    private Line thisLine;
    private bool isSnapped = false;
    private Dot snappedDot;
    public bool currentLineValid;

    public void SetOrigin(Line line)
    {
        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        thisLine = line;
        lineRenderer.startWidth = 0.075f;
        lineRenderer.positionCount = 2;
        thisLine.SetStartDot(line.GetStartDot());
        thisLine.SetEndDot(line.GetEndDot());
    }
    private void Update()
    {
        LineBehavior lineBehavior = GameObject.Find("Controller").GetComponent<LineBehavior>();
        if (thisLine != null)
        {
            if (thisLine.GetStartDot() == lineBehavior.currentDot && !isSnapped && lineBehavior.gameInProgress)
            {
                MoveLine();
            }
        }
    }
    private void MoveLine()
    {
        LineBehavior lineBehavior = GameObject.Find("Controller").GetComponent<LineBehavior>();
        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        if (Input.touchCount > 0)
        {



            Touch touch = Input.GetTouch(0);
            if (touch.phase != TouchPhase.Ended && lineBehavior.currentLineValid)
            {

                Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                touchPosition.z = 0;
                bool isAdjacent = lineBehavior.CheckAdjecent(thisLine.GetStartDot()).Item2;
                snappedDot = lineBehavior.CheckAdjecent(thisLine.GetStartDot()).Item1;

                if (isAdjacent)
                {
                    lineBehavior.currentDot = snappedDot;
                    lineBehavior.UpdateUndoButton();
                    isSnapped = true;
                    thisLine.SetEndDot(snappedDot);
                    thisLine.GetEndDot().AttachedLines.Add(thisLine);
                    thisLine.GetStartDot().AttachedLines.Add(thisLine);
                    if (lineBehavior.P2Goal.Contains(thisLine.GetEndDot()))
                    {
                        Winner("Player 1");
                    }
                    if (lineBehavior.P1Goal.Contains(thisLine.GetEndDot()))
                    {
                        Winner("Player 2");
                    }
                    lineBehavior.AllGameLines.Add(thisLine);
                    lineBehavior.ResetDotColors();
                    
                    if (thisLine.GetEndDot().AttachedLines.Count < 2)
                    {
                        lineBehavior.startOfTurnDot = lineBehavior.endTurnDot;
                        lineBehavior.endTurnDot = thisLine.GetEndDot();
                        lineBehavior.ChangePlayer();
                    }
                    else{
                        GameObject.Find("Controller").GetComponent<LineBehavior>().CreateLine(lineBehavior.currentDot);
                    }
                    

                }

                else
                {
                    lineRenderer.SetPosition(1, touchPosition);
                    isSnapped = false;
                }
                lineBehavior.UpdateUndoButton();
            }
            else
            {
                if (isSnapped)
                {
                    lineBehavior.currentDot = snappedDot;
                }



                else
                {
                    Destroy(this.gameObject);
                }


            }
        }
    }

    void Winner(string winner)
    {
        particles = new(GameObject.FindGameObjectsWithTag("WinnerCoriandoli"));
        GameObject.Find("Controller").GetComponent<LineBehavior>().gameInProgress = false;
        GameObject.Find("Controller").GetComponent<LineBehavior>().WinnerText.SetActive(true);
        GameObject.Find("Controller").GetComponent<LineBehavior>().WinnerText.GetComponent<TMP_Text>().text = winner + " Wins!";
        if (winner == "Player 1")
        {
            GameObject.Find("Controller").GetComponent<LineBehavior>().WinnerText.GetComponent<TMP_Text>().color = PlayerPrefs.GetColor("P1");
        }
        else
        {
            GameObject.Find("Controller").GetComponent<LineBehavior>().WinnerText.GetComponent<TMP_Text>().color = PlayerPrefs.GetColor("P2");
        }

        foreach (GameObject particle in particles)
        {
            particle.GetComponent<ParticleSystem>().Play();
        }

    }


}
