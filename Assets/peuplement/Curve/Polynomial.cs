using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
Une classe pour manipuler des polynomes. On peux faire des opérations de base avec,
 et surtout calculer l'intégrale et la dérivée
*/
public class Polynomial
{

    private List<float> coefs;

    //évaluer le polynome en x
    public float Get(float x)
    {
        float y = 0;
        for (int i = 0; i < coefs.Count; i++)
        {
            y += Mathf.Pow(x, i) * coefs[i];
        }
        return y;
    }

    //retourne le degré du polynome
    public int Deg()
    {
        return coefs.Count - 1;
    }

    //////////////////////////////////////////////////////////////////////
    //définition des opérations de math de base
    //////////////////////////////////////////////////////////////////////

    //définition de l'addition
    public static Polynomial operator +(Polynomial polynomial, float f)
    {
        Polynomial clone = polynomial.Clone();
        clone.Add(f);
        return clone;
    }
    public void Add(float f)
    {
        if (coefs.Count == 0)
        {
            coefs.Add(f);
        }
        else
        {
            coefs[0] += f;
        }
    }

    //définition de la multiplication
    public static Polynomial operator *(Polynomial polynomial, float f)
    {
        Polynomial clone = polynomial.Clone();
        clone.Mul(f);
        return clone;
    }
    public void Mul(float f)
    {
        for (int i = 0; i < coefs.Count; i++)
        {
            coefs[i] *= f;
        }
    }
    
    //créer un nouveau polynome qui est le polynome actuel au carré
    public Polynomial Squared()
    {
        int n = coefs.Count;
        List<float> sqrCoefs = new List<float>();
        for( int i=0; i<n*2; i++ ) {
            sqrCoefs.Add(0);
        }
        
        for ( int i=0; i<n; i++ ) {
            for ( int j=0; j<n; j++ ) {
                sqrCoefs[i+j] += coefs[i] * coefs[j];
            }
        }
        
        return new Polynomial(sqrCoefs);
    }


    //ajoute une constante pour que la courbe du polynome passe par le point pt
    public void ChangeHeight(Vector2 pt)
    {
        float delta = pt.y - Get(pt.x);
        coefs[0] += delta;
    }


    //////////////////////////////////////////////////////////////////////
    //calcul de la dérivée et de la primitive
    //////////////////////////////////////////////////////////////////////

    //calcul de la dérivée, et retourne un autre polynome
    public Polynomial GetDerivative()
    {
        List<float> coefsDer = new List<float>();
        for (int i = 1; i < coefs.Count; i++)
        {
            coefsDer.Add(i * coefs[i]);
        }
        return new Polynomial(coefsDer);
    }

    //calcul d'une primitive, et la retourne en tant qu'un autre polynome. La primitive est celle qui passe par (0,0)
    public Polynomial GetPrimitive()
    {
        List<float> coefsInter = new List<float>();
        coefsInter.Add(0);
        for (int i = 0; i < coefs.Count; i++)
        {
            coefsInter.Add(coefs[i] / (i + 1));
        }

        return new Polynomial(coefsInter);
    }




    //clone
    public Polynomial Clone()
    {
        return new Polynomial(new List<float>(coefs));
    }


    //magouilles de construction
    public Polynomial(List<float> coefs)
    {
        this.coefs = (coefs == null || coefs.Count == 0) ? new List<float> { 0 } : coefs;
    }
    public Polynomial() : this(null) { }

    public Polynomial(Vector2 pt1, Vector2 pt2) : this(CreateLine(pt1, pt2)) { }



    private static List<float> CreateLine(Vector2 pt1, Vector2 pt2)
    {
        float a = (pt2.y - pt1.y) / (pt2.x - pt1.x);
        float b = pt1.y - a * pt1.x;
        return new List<float> { b, a };
    }

}
