using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

public class Risovat : MonoBehaviour
{
    private LineRenderer line;
    public bool isMousePressed;
    public List<Vector3> pointsList;
    private Vector3 mousePos;
    public List<Vector3> Rays;
    public List<Vector3> Fig_Rays;
    public List<Vector3> Cur_Rays;
    public List<float> Rays_distace;
    public List<float> Cur_Rays_distace;
    //Figure Renderer
    public List<Figure> Figurs;
    public LineRenderer FiguresRenderer;
   //public List<Vector3> Temp;
   // private Vector3 T;
    public int LVL = 0;
    public RectTransform RT;
    //public List<LineRenderer> FiguresRenderer2;
   // public List<LineRenderer> FiguresRenderer3;
    public bool Arc = false;
    public Vector3 Acroosing;
    public List<float> X;
    public List<float> Y;
    public List<float> Cur_X;
    public List<float> Cur_Y;
    public Vector3 Cur_Acroosing;
    public int CountExeptions;
    public List<bool> Pridels;
    public List<float> TimeGameNominal;
    public List<float> TimeGame;
    public Text TimeText;

    public GameObject TryAgain;
    public Text Schet;
    //public bool MouseWasPressed;

    public bool temp = false;
    void Awake()
    {
        line = gameObject.GetComponent<LineRenderer>();
        line.numPositions = 0;
        line.useWorldSpace = true;
        isMousePressed = false;
        pointsList = new List<Vector3>();

        foreach (float t in TimeGameNominal)
        {
            TimeGame.Add(t);
        }

    }
    void Start()
    {
        
    }

    void Update()
    {
        
        TimeGame[LVL] -= Time.deltaTime;
        Schet.text = (LVL).ToString();
        if (TimeGame[LVL] >= 0)
        {
            TimeText.text = TimeGame[LVL].ToString("0.0");
            if (Input.GetMouseButtonDown(0))
            {
                isMousePressed = true;
                line.numPositions = 0;

            }
            if (Input.GetMouseButtonUp(0))
            {
                isMousePressed = false;
            }

            if (isMousePressed)
            {
                mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePos.z = 0;
                if (!pointsList.Contains(mousePos))
                {
                    pointsList.Add(mousePos);
                    line.numPositions = pointsList.Count;
                    line.SetPosition(pointsList.Count - 1, pointsList[pointsList.Count - 1]);
                    if (Across())
                    {
                        isMousePressed = false;
                        Arc = true;
                        temp = false;
                    }
                }
            }



            FiguresRenderer.numPositions = Figurs[LVL].FiguresPosition.Count;

            for (int i = 0; i < Figurs[LVL].FiguresPosition.Count; i++)
            {
                FiguresRenderer.SetPosition(i, Figurs[LVL].FiguresPosition[i]);
            }


            if (X.Count < Figurs[LVL].FiguresPosition.Count)
            {
                for (int i = 0; i < Figurs[LVL].FiguresPosition.Count; i++)
                {
                    X.Add(Figurs[LVL].FiguresPosition[i].x);
                    Y.Add(Figurs[LVL].FiguresPosition[i].y);

                }
                Acroosing = new Vector3((Mathf.Max(X.ToArray()) + Mathf.Min(X.ToArray())) / 2, (Mathf.Max(Y.ToArray()) + Mathf.Min(Y.ToArray())) / 2, 0f);

            }

            if (Arc)
            {
                Cur_X.Clear();
                Cur_Y.Clear();
                Cur_Rays.Clear();
                Fig_Rays.Clear();
                Rays_distace.Clear();
                Pridels.Clear();

                if (Cur_X.Count < pointsList.Count)
                {
                    for (int i = 0; i < pointsList.Count; i++)
                    {
                        Cur_X.Add(pointsList[i].x);
                        Cur_Y.Add(pointsList[i].y);

                    }
                    Cur_Acroosing = new Vector3((Mathf.Max(Cur_X.ToArray()) + Mathf.Min(Cur_X.ToArray())) / 2, (Mathf.Max(Cur_Y.ToArray()) + Mathf.Min(Cur_Y.ToArray())) / 2, 0f);
                    //Rays[0] = new Vector3(Acroosing.x, Acroosing.y, 0f);

                }
                if (Fig_Rays.Count < Rays.Count)
                {
                    for (int i = 0; i < Rays.Count; i++)
                    {
                        Fig_Rays.Add(new Vector3(Acroosing.x + 3 + Rays[i].x, Acroosing.y + Rays[i].y, 0));
                    }
                }
                if (Cur_Rays.Count < Rays.Count)
                {
                    for (int i = 0; i < Rays.Count; i++)
                    {

                        Cur_Rays.Add(new Vector3(Cur_Acroosing.x + 3 + Rays[i].x, Cur_Acroosing.y + Rays[i].y, 0));
                    }
                }
                /*
                for (int i = 0; i < FiguresRenderer2.Count; i++)
                {
                    FiguresRenderer2[i].numPositions = 2;
                    FiguresRenderer2[i].SetPosition(0, Acroosing);
                    FiguresRenderer2[i].SetPosition(1, Fig_Rays[i]);

                    FiguresRenderer3[i].numPositions = 2;
                    FiguresRenderer3[i].SetPosition(0, Cur_Acroosing);
                    FiguresRenderer3[i].SetPosition(1, Cur_Rays[i]);


                }*/

                if (Rays_distace.Count < Fig_Rays.Count)
                {

                    for (int i = 0; i < Fig_Rays.Count; i++)
                    {
                        Rays_distace.Add(new float());
                        for (int j = 0; j < Figurs[LVL].FiguresPosition.Count - 1; j++)
                        {
                            if (Point_Acroos(Acroosing, Fig_Rays[i], Figurs[LVL].FiguresPosition[j], Figurs[LVL].FiguresPosition[j + 1]) != new Vector3(0.000000322f, 0.000000322f, 0f))
                            {
                                Vector3 tempV = Point_Acroos(Acroosing, Fig_Rays[i], Figurs[LVL].FiguresPosition[j], Figurs[LVL].FiguresPosition[j + 1]);

                                if (PeresikanieProv(Acroosing, Fig_Rays[i], Figurs[LVL].FiguresPosition[j], Figurs[LVL].FiguresPosition[j + 1]))
                                {

                                    if ((((Mathf.Min(Acroosing.x, Fig_Rays[i].x) <= tempV.x) && (Mathf.Max(Acroosing.x, Fig_Rays[i].x) >= tempV.x) && (Mathf.Min(Figurs[LVL].FiguresPosition[j].x, Figurs[LVL].FiguresPosition[j + 1].x) <= tempV.x) && (Mathf.Max(Figurs[LVL].FiguresPosition[j].x, Figurs[LVL].FiguresPosition[j + 1].x) >= tempV.x)) &&
                                    (((Mathf.Min(Acroosing.y, Fig_Rays[i].y) <= tempV.y) && (Mathf.Max(Acroosing.y, Fig_Rays[i].y) >= tempV.y) && (Mathf.Min(Figurs[LVL].FiguresPosition[j].y, Figurs[LVL].FiguresPosition[j + 1].y) <= tempV.y) && (Mathf.Max(Figurs[LVL].FiguresPosition[j].y, Figurs[LVL].FiguresPosition[j + 1].y) >= tempV.y)))))
                                    {
                                        // Debug.Log("tempV_"+i+" = " + tempV);
                                        Rays_distace[i] = (Vector3.Distance(Acroosing, Point_Acroos(Acroosing, Fig_Rays[i], Figurs[LVL].FiguresPosition[j], Figurs[LVL].FiguresPosition[j + 1])));
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }

                if (Cur_Rays_distace.Count < Cur_Rays.Count)
                {

                    for (int i = 0; i < Cur_Rays.Count; i++)
                    {
                        Cur_Rays_distace.Add(new float());

                        for (int j = 0; j < pointsList.Count - 1; j++)
                        {

                            if (Point_Acroos(Cur_Acroosing, Cur_Rays[i], pointsList[j], pointsList[j + 1]) != new Vector3(0.000000322f, 0.000000322f, 0f))
                            {
                                Vector3 tempV = new Vector3(0f, 0f, 0f);
                                tempV = Point_Acroos(Cur_Acroosing, Cur_Rays[i], pointsList[j], pointsList[j + 1]);

                                if (PeresikanieProv(Cur_Acroosing, Cur_Rays[i], pointsList[j], pointsList[j + 1]))
                                {

                                    if ((((Mathf.Min(Cur_Acroosing.x, Cur_Rays[i].x) <= tempV.x) && (Mathf.Max(Cur_Acroosing.x, Cur_Rays[i].x) >= tempV.x) && (Mathf.Min(pointsList[j].x, pointsList[j + 1].x) <= tempV.x) && (Mathf.Max(pointsList[j].x, pointsList[j + 1].x) >= tempV.x)) &&
                                    (((Mathf.Min(Cur_Acroosing.y, Cur_Rays[i].y) <= tempV.y) && (Mathf.Max(Cur_Acroosing.y, Cur_Rays[i].y) >= tempV.y) && (Mathf.Min(pointsList[j].y, pointsList[j + 1].y) <= tempV.y) && (Mathf.Max(pointsList[j].y, pointsList[j + 1].y) >= tempV.y)))))
                                    {
                                        //  Debug.Log("tempV_" + i + " = " + tempV);
                                        Cur_Rays_distace[i] = (Vector3.Distance(Cur_Acroosing, Point_Acroos(Cur_Acroosing, Cur_Rays[i], pointsList[j], pointsList[j + 1])));

                                        break;
                                    }


                                }

                            }

                        }


                    }
                }

                if (temp == false)
                {
                    temp = true;
                    CountExeptions = 0;
                    foreach (float t in Cur_Rays_distace)
                    {
                        if (t == 0)
                        {
                            CountExeptions++;
                        }
                    }




                    if (CountExeptions <= 5)
                    {
                        for (int i = 0; i < Rays_distace.Count; i++)
                        {

                            if (Cur_Rays_distace[i] == 0 || Rays_distace[i] == 0)
                            {
                                Pridels.Add(true);
                            }
                            else
                            {
                                int scale = 0;
                                for (int k = 0; k < Rays_distace.Count; k++)
                                {
                                    if (Cur_Rays_distace[k] != 0 && Rays_distace[k] != 0)
                                    {
                                        scale = k;
                                        break;
                                    }
                                } 
                                //  if ((Cur_Rays_distace[i] <= (Rays_distace[i] + 0.25f)) && (Cur_Rays_distace[i] >= (Rays_distace[i] - 0.25f)))
                                // {
                                if ((Cur_Rays_distace[i]*(Rays_distace[scale] / Cur_Rays_distace[scale]) <= (Rays_distace[i] + 0.25f)) && (Cur_Rays_distace[i] * (Rays_distace[scale] / Cur_Rays_distace[scale]) >= (Rays_distace[i] - 0.25f)))
                                {

                                    Pridels.Add(true);
                                }
                                else
                                {
                                    Pridels.Add(false);
                                }
                            }
                        }

                    }
                    else
                    {
                        //Новая попытка
                        Arc = false;
                        pointsList.Clear();
                        Cur_Rays_distace.Clear();
                        //Cur_Rays.Clear();
                        //   Pridels.Clear();
                        Cur_Acroosing = new Vector3(0f, 0f, 0f);
                        line.numPositions = 0;
                        Cur_X.Clear();
                        Cur_Y.Clear();
                        /*
                        for (int i = 0; i < FiguresRenderer2.Count; i++)
                        {
                            FiguresRenderer3[i].numPositions = 0;
                        }*/
                    }

                    int Next_LVL = 0;

                    foreach (bool p in Pridels)
                    {

                        if (p)
                        {
                            Next_LVL++;
                        }
                    }
                     Debug.Log(Next_LVL);


                    if (Next_LVL == Rays_distace.Count || Next_LVL == Rays_distace.Count - 1 || Next_LVL == Rays_distace.Count - 2)
                    {
                        LVL++;
                        line.numPositions = 0;

                    }
                    else
                    {
                        //Новая попытка
                        Arc = false;
                        pointsList.Clear();
                        Cur_Rays_distace.Clear();
                        Cur_Rays.Clear();
                        //Pridels.Clear();
                        Cur_Acroosing = new Vector3(0f, 0f, 0f);
                        line.numPositions = 0;
                        Cur_X.Clear();
                        Cur_Y.Clear();
                        X.Clear();
                        Y.Clear();
                        //for (int i = 0; i < FiguresRenderer2.Count; i++)
                        //{
                        //    FiguresRenderer2[i].numPositions = 0;
                        //}
                        /*
                        for (int i = 0; i < FiguresRenderer3.Count; i++)
                        {
                            FiguresRenderer3[i].numPositions = 0;
                        }*/
                    }




                }


            }
            else if (Arc == false && isMousePressed == false)
            {
                //Новая попытка
                Arc = false;
                pointsList.Clear();
                Cur_Rays_distace.Clear();
                Cur_Rays.Clear();
                //Pridels.Clear();
                Cur_Acroosing = new Vector3(0f, 0f, 0f);
                line.numPositions = 0;
                Cur_X.Clear();
                Cur_Y.Clear();
                X.Clear();
                Y.Clear();

                //for (int i = 0; i < FiguresRenderer2.Count; i++)
                //{
                //    FiguresRenderer2[i].numPositions = 0;
                //}
                /*
                for (int i = 0; i < FiguresRenderer3.Count; i++)
                {
                    FiguresRenderer3[i].numPositions = 0;
                }*/
            }
        }
        else
        {
            TryAgain.SetActive(true);
        }
    }

    public void FuncTryAgain()
    {
        LVL = 0;
        TimeGame.Clear();
        foreach (float t in TimeGameNominal)
        {
            TimeGame.Add(t);
        }
        TryAgain.SetActive(false);
    }

    public Vector3 Point_Acroos(Vector3 ray_start, Vector3 ray_end, Vector3 din_ray_start, Vector3 din_ray_end)
    {
        float X,Y = 0; 
        if (PeresikanieProv(din_ray_start, din_ray_end, ray_start, ray_end))
        {
            X = -(((ray_start.x * ray_end.y - ray_end.x * ray_start.y) * (din_ray_end.x - din_ray_start.x) - (din_ray_start.x * din_ray_end.y - din_ray_end.x * din_ray_start.y) * (ray_end.x - ray_start.x))
                / ((ray_start.y - ray_end.y) * (din_ray_end.x - din_ray_start.x) - (din_ray_start.y - din_ray_end.y) * (ray_end.x - ray_start.x)));
            if (din_ray_end.x != din_ray_start.x)
            {
                Y = ((din_ray_start.y - din_ray_end.y) * (-X) - (din_ray_start.x * din_ray_end.y - din_ray_end.x * din_ray_start.y)) / (din_ray_end.x - din_ray_start.x);
            }
            else
            {
                Y = ((din_ray_start.y - din_ray_end.y) * (-X) - (din_ray_start.x * din_ray_end.y - din_ray_end.x * din_ray_start.y));
            }

            return new Vector3(X, Y,0f);
        }

        return new Vector3(0.000000322f,0.000000322f, 0f);
    }

    public bool IfPointOnLine(Vector3 Point, Vector3 Line_A, Vector3 Line_B)
    {
        if ((Point.x - Line_A.x) / (Line_B.x - Line_A.x) == (Point.y - Line_A.y) / (Line_B.y - Line_A.y))
        {
            return true;
        }
        return false;
    }

    private bool Across()
    {
        if (pointsList.Count < 4)
        {
            return false;
        }

        for (int i = 0; i < (pointsList.Count - 2); i++)
        {
            if (PeresikanieProv(pointsList[i], pointsList[i + 1], pointsList[pointsList.Count - 1], pointsList[pointsList.Count - 2]))
            {
                return true;
                
            }

        }
        return false;
        
       

    }

    private bool PeresikanieProv( Vector3 DinamicPosFirst, Vector3 DinamicPosLast, Vector3 LastPosFirst, Vector3 LastPosLast)
    {
        if (checkPoints(DinamicPosFirst, LastPosFirst) ||
            checkPoints(DinamicPosFirst, LastPosLast) ||
            checkPoints(DinamicPosLast, LastPosFirst) ||
            checkPoints(DinamicPosLast, LastPosLast))
        {
            return false;
        }

        return ((Mathf.Max(DinamicPosFirst.x, DinamicPosLast.x) >= Mathf.Min(LastPosFirst.x, LastPosLast.x)) &&
            (Mathf.Max(LastPosFirst.x, LastPosLast.x) >= Mathf.Min(DinamicPosFirst.x, DinamicPosLast.x)) &&
            (Mathf.Max(DinamicPosFirst.y, DinamicPosLast.y) >= Mathf.Min(LastPosFirst.y, LastPosLast.y)) &&
            (Mathf.Max(LastPosFirst.y, LastPosLast.y) >= Mathf.Min(DinamicPosFirst.y, DinamicPosLast.y)));

      
    }
    private bool PeresikanieProv2(Vector3 DinamicPosFirst, Vector3 DinamicPosLast, Vector3 LastPosFirst, Vector3 LastPosLast)
    {
        return ((Mathf.Max(DinamicPosFirst.x, DinamicPosLast.x) >= Mathf.Min(LastPosFirst.x, LastPosLast.x)) &&
            (Mathf.Max(LastPosFirst.x, LastPosLast.x) >= Mathf.Min(DinamicPosFirst.x, DinamicPosLast.x)) &&
            (Mathf.Max(DinamicPosFirst.y, DinamicPosLast.y) >= Mathf.Min(LastPosFirst.y, LastPosLast.y)) &&
            (Mathf.Max(LastPosFirst.y, LastPosLast.y) >= Mathf.Min(DinamicPosFirst.y, DinamicPosLast.y)));
        
    }

    private bool checkPoints(Vector3 pointA, Vector3 pointB)
    {
        return (pointA.x == pointB.x && pointA.y == pointB.y);
    }

}
[Serializable]
public class Figure
{
    public List<Vector3> FiguresPosition;
}
