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
    public float Get( float x ) {
        float y = 0;
        for ( int i=0; i<coefs.Count; i++ ) {
            y += Mathf.Pow(x,i) * coefs[i];
        }
        return y;
    }
    
    //retourne le degré du polynome
    public int Deg() {
        return coefs.Count - 1;
    }
    
    //définition de l'addition
    public static Polynomial operator +( Polynomial polynomial, float f ) {
        List<float> lst = new List<float>(polynomial.coefs);
        lst[0] += f;
        return new Polynomial(lst);
    }
    
    //définition de la multiplication
    public static Polynomial operator *( Polynomial polynomial, float f ) {
        List<float> lst = new List<float>(polynomial.coefs);
        for ( int i=0; i<lst.Count; i++ ) {
            lst[i] *= f;
        }
        return new Polynomial(lst);
    }
    
    //calcul de la dérivée, et retourne un autre polynome
    public Polynomial CreateDerivative() {
        List<float> coefsDer = new List<float>();
        for( int i=1; i<coefs.Count; i++ ) {
            coefsDer.Add( i * coefs[i] );
        }
        return new Polynomial(coefsDer);
    }
    
    //calcul de l'intégrale, et retourne un autre polynome. L'intégrale est celle qui passe par (0,0)
    public Polynomial CreateIntegral() {
        List<float> coefsInter = new List<float>();
        coefsInter.Add(0);
        for( int i=0; i<coefs.Count; i++ ) {
            coefsInter.Add( coefs[i] / (i+1) );
        }
        
        return new Polynomial(coefsInter);
    }
    
    //ajoute une constante pour que la courbe du polynome passe par le point pt
    public void changeHeight( Vector2 pt ) {
        float delta = pt.y - Get(pt.x);
        coefs[0] += delta;
    }
    
    
    //magouilles de construction
    public Polynomial( List<float> coefs ) {
        this.coefs = (coefs == null || coefs.Count == 0) ? new List<float>{0} : coefs;
    }
    public Polynomial() : this(null){}
    
    public Polynomial( Vector2 pt1, Vector2 pt2) : this(CreateLine(pt1,pt2)){}
    
    
    
    private static List<float> CreateLine(Vector2 pt1, Vector2 pt2) {
        float a = (pt2.y - pt1.y) / (pt2.x - pt1.x);
        float b = pt1.y - a * pt1.x;
        return new List<float>{b,a};
    }
    
}
