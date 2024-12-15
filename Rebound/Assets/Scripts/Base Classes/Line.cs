using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using System.Linq;
using System.Diagnostics;
using System;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;


public class Line
{   
    public IPlayer LinePlayer { get; private set; }
    public GameObject Instance { get; private set; }
    public LineRenderer RendererInstance { get; set; }
    public Dot StartDot { get; private set; }
    public Dot EndDot { get; set; }
    public static List<Line> LineHistory = new();

    public Line(IPlayer player, Dot startDot, Dot endDot = null, bool isGameLine = false)
    {
        Game g =Game.Instance;
        Instance = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Prefabs/Line"), startDot.Instance.transform.position, Quaternion.identity);
        RendererInstance = Instance.GetComponent<LineRenderer>();

        if (isGameLine)
        {
            LineHistory.Add(this);
        }
        if (player != null)
        {
            SetColor(player.Color);
        }
        else
        {
            SetColor(Color.white);
        }

        LinePlayer = player;
        RendererInstance.loop = false;
        RendererInstance.positionCount = 2;
        SetStartDot(startDot);
        SetEndDot(endDot);
        if (GetEndDot() == null) RendererInstance.SetPosition(1, GetStartDot().Instance.transform.position);
    }
    public void SetColor(Color color)
    {
        RendererInstance.startColor = color;
        RendererInstance.endColor = color;
    }
    public Dot GetStartDot()
    {
        return StartDot;
    }
    public Dot GetEndDot()
    {
        return EndDot;
    }
    public void SetEndDot(Dot dot, bool noAttachment = false)
    {
        EndDot = dot;
        if (dot == null) return;
        if (!noAttachment) dot.AttachedLines.Add(this);

        Instance.GetComponent<LineRenderer>().SetPosition(1, dot.Instance.transform.position);
    }
    public void SetStartDot(Dot dot)
    {
        StartDot = dot;
        dot.AttachedLines.Add(this);
        Instance.GetComponent<LineRenderer>().SetPosition(0, dot.Instance.transform.position);
    }
    public GameObject GetInstance()
    {
        return Instance;
    }
    public LineRenderer GetRendererInstance()
    {
        return RendererInstance;
    }

}
