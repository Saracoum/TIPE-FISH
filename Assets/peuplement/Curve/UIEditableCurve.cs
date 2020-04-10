using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
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
    //le rectTransform pour chopper la position de la souris
    private RectTransform rect;

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
    private GameObject AddKeyMarker = null;

    [SerializeField]
    private GameObject sliderPref = null;
    private List<BoxSlider> sliders = new List<BoxSlider>();


    //la distance a la courbe de la souris pour placer un point
    [Range(0.0f, 0.5f)]
    [SerializeField]
    private float distToAddKey = 0.1f;

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

    [SerializeField]
    private bool _editCurve = true;
    public bool EditCurve
    {
        get { return _editCurve; }
        set{
            _editCurve = value;
            foreach( BoxSlider slider in sliders ) {
                slider.gameObject.SetActive(_editCurve);
            }
            if (rend != null) {
                rend.raycastTarget = _editCurve;
            }
            if ( !_editCurve ) {
                AddKeyMarker.SetActive(false);
            }
        }
    }
    
    
    //intervalle de temps de la courbe
    [SerializeField]
    private float _timeSpan = 1.0f;
    public float TimeSpan {
        get { return _timeSpan; }
        set {
            _timeSpan = value;
            UpdateScale( _xScale, 0, _timeSpan );
        }
    }
    
    //valeur min et max de la courbe
    [SerializeField]
    private float _minRange = 0.0f;
    public float MinRange {
        get { return _minRange; }
        set { 
            _minRange = Mathf.Min(value, _maxRange); 
            UpdateScale( _yScale, _minRange, _maxRange );
        }
    }
    [SerializeField]
    private float _maxRange = 1.0f;
    public float MaxRange {
        get { return _maxRange; }
        set {
            _maxRange = Mathf.Max(value, MinRange); 
            UpdateScale( _yScale, _minRange, _maxRange );
        }
    }
    
    [SerializeField]
    private List<Text> _yScale;
    public List<Text> YScale {
        get { return _yScale; }
        set{ 
            _yScale = value; 
            UpdateScale( _yScale, _minRange, _maxRange );
        }
    }
    [SerializeField]
    private List<Text> _xScale;
    public List<Text> XScale {
        get { return _xScale; }
        set{ 
            _xScale = value; 
            UpdateScale( _xScale, 0, _timeSpan );
        }
    }
    
    
    //conversion entre le repère de la courbe et celui de l'écran
    private Vector2 ScreenToCurve( Vector2 v ) {
        return new Vector2( 
            v.x * TimeSpan,
            ScreenToCurveY(v.y)
        );
    }
    
    private float ScreenToCurveY( float y ) {
        return (y * (MaxRange-MinRange)) + MinRange;
    }
    
    private Vector2 CurveToScreen( Vector2 v ) {
        return new Vector2( 
            v.x / TimeSpan, 
            CurveToScreenY(v.y)
        );
    }
    
    private float CurveToScreenY( float y ) {
        return (y - MinRange) / (MaxRange-MinRange);
    }
    
    
    private void UpdateScale ( List<Text> scale, float min, float max ) {
        float countF = scale.Count-1.0f;
        
        for ( int i=0; i<scale.Count; i++ ) {
            scale[i].text = (i/countF * (max-min) + min ).ToString();
        }
    }
    
    
    
    
    private void Update()
    {
        if ( EditCurve ) {
            ProcessMouseInput();
        }
    }
    
    //récupère la position de la souris pour déterminer si il faut afficher la nouvelle clé, et la créer
    private void ProcessMouseInput () {
        
        Vector2 mouseScreenPos = Input.mousePosition;
        Vector2 mouseLocalPos = Rect.PointToNormalized(rect.rect, rect.InverseTransformPoint(mouseScreenPos));
        
        //la position du marker, dans le repère local
        Vector2 keyMarkerPos = new Vector2(
            mouseLocalPos.x, 
            CurveToScreenY( CurrentCurve.Get( mouseLocalPos.x * TimeSpan ) )
        );

        bool canAddKey = Mathf.Abs(mouseLocalPos.y - keyMarkerPos.y) < distToAddKey;
        foreach (Vector2 key in CurrentCurve.GetKeys())
        {
            // canAddKey = canAddKey && (mouseLocalPos - CurveToScreen(key) ).sqrMagnitude > distToAddKey * distToAddKey;
            canAddKey = canAddKey && Mathf.Abs(mouseLocalPos.x - CurveToScreen(key).x) > distToAddKey; //???
        }
        AddKeyMarker.SetActive(canAddKey);
        if (canAddKey)
        {
            AddKeyMarker.transform.localPosition = Rect.NormalizedToPoint(rect.rect, keyMarkerPos);
            if (Input.GetMouseButtonDown(0))
            {
                AddKey(  ScreenToCurve(keyMarkerPos) );
            }


        }
    }
    
    
    //ajoute une clé a la courbe et met a jour l'affichage
    public void AddKey(Vector2 key)
    {
        CurrentCurve.AddKey(key);
        UpdateSliders();
        UpdateLineRender();
    }


    //met à jour les positions des sliders sur l'interface
    private void UpdateSliders()
    {
        //Nettoyage des sliders
        foreach (BoxSlider slider in sliders)
        {
            Destroy(slider.gameObject);
        }
        sliders = new List<BoxSlider>();

        List<Vector2> keys = CurrentCurve.GetKeys();
        foreach (Vector2 key in keys)
        {
            GameObject sliderObj = Instantiate(sliderPref, transform);
            BoxSliderKey boxSlider = sliderObj.GetComponent<BoxSliderKey>();
            sliderObj.SetActive(EditCurve);
            
            Vector2 keyScreen = CurveToScreen(key);
            boxSlider.ValueX = keyScreen.x;
            boxSlider.ValueY = keyScreen.y;
            boxSlider.editableCurve = this;

            sliders.Add(boxSlider);
        }
    }

    //applique les modifs depuis le slider vers la courbe, et met a jour l'affichage
    public void UpdateCurveKey(BoxSlider changedSlider)
    {
        //tri de la liste des sliders, et conservation des indice avant et après le tri
        int oldId = sliders.IndexOf(changedSlider);
        sliders.Sort(CompSliders);
        int newId = sliders.IndexOf(changedSlider);

        if (oldId > newId)
        {
            int tmp = oldId;
            oldId = newId;
            newId = tmp;
        }
        for (int id = oldId; id <= newId; id++)
        {
            BoxSliderKey currentSlider = sliders[id].GetComponent<BoxSliderKey>();
            Vector2 keyPos = ScreenToCurve( new Vector2(currentSlider.ValueX, currentSlider.ValueY) );
            CurrentCurve.ChangeKeyById(id, keyPos);
        }
        UpdateLineRender();
    }


    //supprime un slider
    public void RemoveSlider(BoxSlider slider)
    {
        int id = sliders.IndexOf(slider);
        sliders.Remove(slider);
        Destroy(slider.gameObject);

        CurrentCurve.RemoveKeyAt(id);
        UpdateLineRender();
    }


    //tri la liste des sliders
    private int CompSliders(BoxSlider slider1, BoxSlider slider2)
    {
        float delta = slider1.ValueX - slider2.ValueX;
        if (delta == 0)
        {
            return 0;
        }
        return delta > 0 ? 1 : -1;
    }


    //met à jour l'affichage de la courbe
    private void UpdateLineRender()
    {
        if (rend == null) { return; }

        for (int i = 0; i < rend.Points.Length; i++)
        {
            rend.Points[i].y = CurveToScreenY( CurrentCurve.Get(rend.Points[i].x * TimeSpan) );
        }
        rend.SetAllDirty();
    }


    //permet d'actualiser l'affichage lors du changement de certains paramètres
    private void OnValidate()
    {
        Subdivision = _subdivision;
        EditCurve = _editCurve;
        
        TimeSpan = _timeSpan;
        MaxRange = _maxRange;
        MinRange = _minRange;
        
        XScale = _xScale;
        YScale = _yScale;
    }

    private void Awake()
    {
        
        
        //récupération des composants
        rend = GetComponent<UILineRenderer>();
        rect = GetComponent<RectTransform>();

        //création de la courbe
        //List<Vector2> keyLst = new List<Vector2> { new Vector2(0, 0), new Vector2(1, 1) };
        List<Vector2> keyLst = new List<Vector2> { new Vector2(0, 0), new Vector2(0.5f, 1), new Vector2(1, 0.75f) };
        CurrentCurve = new LinearCurve(keyLst);

        OnValidate();

    }

}
