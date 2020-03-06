using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Curve
{

    protected List<Polynomial> polyLst = new List<Polynomial>();
    protected List<Vector2> keys = new List<Vector2>();

    public float Get(float x)
    {

        int id = GetRangeId(x);
        return (id < 0) ? 0 : polyLst[id].Get(x);
    }


    public void AddKey(float x, float y)
    {
        AddKey(new Vector2(x, y));
    }

    public void AddKey(Vector2 key)
    {
        int insertId = GetRangeId(key.x);
        if (insertId == -1)
        {
            keys.Add(key);
            insertId = keys.Count - 1;
        }
        else
        {
            if (insertId == -2)
            {
                insertId = 0;
            }
            keys.Insert(insertId, key);
        }

        if (insertId > 0)
        {
            polyLst.Insert(insertId - 1, CreatePoly(insertId - 1));
        }
        if (insertId + 1 < keys.Count)
        {
            polyLst.Insert(insertId, CreatePoly(insertId));
        }
    }


    public abstract Polynomial CreatePoly(int id);


    private int GetRangeId(float x)
    {
        if (keys.Count == 0 || x > keys[keys.Count-1].x)
        {
            return -1;
        }
        else if (x < keys[0].x)
        {
            return -2;
        }

        int i = 0;
        while (keys[i + 1].x < x)
        {
            i++;
        }
        return i;
    }
    
    
    
    public Curve() : this(null) {}
    
    
    public Curve(List<Vector2> keys)
    {
        this.keys = new List<Vector2>();
        if ( keys != null ) {
            foreach ( Vector2 key in keys ) {
                AddKey(key);
            }
        }
    }

}