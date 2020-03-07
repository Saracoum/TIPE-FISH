using System;
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

    //le renderer de la courbe
    private UILineRenderer rend;

    //la courbe a afficher
    private Curve _currentCurve;
    public Curve CurrentCurve
    {
        get { return _currentCurve; }
        set
        {
            _currentCurve = value;
            UpdateLineRender();
            UpdateSliders();
        }
    }

    [SerializeField]
    private GameObject sliderPref = null;
    private List<GameObject> sliders;


    // le nombre de subdivision pour afficher la courbe
    [SerializeField]
    private int _subdivision = 10;
    public int Subdivision
    {
        get { return _subdivision; }
        set
        {
            _subdivision = (value > 2) ? value : 2;

            if (rend != null)
            {
                rend.Points = new Vector2[_subdivision];
                for (int i = 0; i < _subdivision; i++)
                {
                    rend.Points[i] = new Vector2();
                    rend.Points[i].x = ((float)i) / (_subdivision - 1);
                }
                UpdateLineRender();
            }
        }
    }

    //met à jour les positions des sliders sur l'interface
    private void UpdateSliders()
    {
        //Nettoyage des sliders
        while (transform.childCount > 0)
        {
            Destroy(transform.GetChild(0).gameObject);
        }
        sliders = new List<GameObject>();

        List<Vector2> keys = CurrentCurve.GetKeys();
        foreach (Vector2 key in keys)
        {
            GameObject sliderObj = Instantiate(sliderPref, transform);
            BoxSliderKey boxSlider = sliderObj.GetComponent<BoxSliderKey>();

            boxSlider.ValueX = key.x;
            boxSlider.ValueY = key.y;
            boxSlider.editableCurve = this;

            sliders.Add(sliderObj);
        }
    }

    //applique les modifs depuis le slider vers la courbe, et met a jour l'affichage
    public void UpdateCurveKey(BoxSlider changedSlider)
    {
        //TODO sort sliders
        
        int sliderId = sliders.IndexOf( changedSlider.gameObject );
        CurrentCurve.ChangeKeyById(sliderId, changedSlider.ValueX, changedSlider.ValueY);
        
        UpdateLineRender();
    }

    //met à jour l'affichage de la courbe
    private void UpdateLineRender()
    {
        if (rend == null) { return; }

        for (int i = 0; i < rend.Points.Length; i++)
        {
            rend.Points[i].y = CurrentCurve.Get(rend.Points[i].x);
        }
        rend.SetAllDirty();
    }


    //permet d'actualiser l'affichage lors du changement de certains paramètres
    private void OnValidate()
    {
        Subdivision = _subdivision;
    }

    private void Start()
    {
        //récupération du UILineRenderer
        rend = GetComponent<UILineRenderer>();

        //création de la courbe
        //List<Vector2> keyLst = new List<Vector2> { new Vector2(0, 0), new Vector2(1, 1) };
        List<Vector2> keyLst = new List<Vector2> { new Vector2(0, 0), new Vector2(0.5f, 1), new Vector2(1, 0.75f) };
        CurrentCurve = new LinearCurve(keyLst);


    }

}
