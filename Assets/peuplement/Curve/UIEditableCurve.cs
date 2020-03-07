using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;


/**
script a attacher a un élément de l'interface graphique qui contient un UILineRenderer, et qui s'occupe de rajouter les points d'ancrage sur l'interface.
La courbe édité est accesible depuis l'extérieur
*/
[RequireComponent(typeof(UILineRenderer))]
public class UIEditableCurve : MonoBehaviour
{
    private UILineRenderer rend;

    private Curve _currentCurve;
    public Curve CurrentCurve
    {
        get { return _currentCurve; }
        set
        {
            _currentCurve = value;
            UpdateLineRender();
        }
    }


    [SerializeField]
    private int _subdivision = 10;
    public int Subdivision
    {
        get { return _subdivision; }
        set
        {
            _subdivision = (value > 2) ? value : 2;
            rend.Points = new Vector2[_subdivision];
            for (int i = 0; i < _subdivision; i++)
            {
                rend.Points[i] = new Vector2();
                rend.Points[i].x = ((float)i) / (_subdivision - 1);
            }
            UpdateLineRender();
        }
    }

    private void UpdateLineRender()
    {
        for (int i = 0; i < rend.Points.Length; i++)
        {
            rend.Points[i].y = CurrentCurve.Get(rend.Points[i].x);
        }
    }


    private void OnValidate()
    {
        Subdivision = _subdivision;
    }

    private void Start()
    {
        rend = GetComponent<UILineRenderer>();

        //List<Vector2> keyLst = new List<Vector2> { new Vector2(0, 0), new Vector2(1, 1) };
        List<Vector2> keyLst = new List<Vector2> { new Vector2(0, 0), new Vector2(0.5f, 1), new Vector2(1,0.75f) };
        CurrentCurve = new LinearCurve(keyLst);

        Subdivision = 10;
    }

}
