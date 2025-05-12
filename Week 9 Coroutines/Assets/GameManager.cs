using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    static GameManager instance = null;

    // Public objects
    public Particle particle = null;
    public GameObject pickup = null;
    public GameObject player = null;

    public int level = 0;

    public enum LayerNames
    {
        Enemy = 9,
        PlayerBullet = 10,
        Player = 11
    }

    public void LevelUp()
    {
        level++;
    }

    public int Level()
    {
        return level;
    }

    // sc is scale factor, max is the max value of the curve
    static public float LevelScale(float sc, float max)
    {
        float l = Manager.Level();
        float t = max / (sc*max*l+1);
        return t;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (instance != null)
        {
            Destroy(gameObject);//Singleton
            return;
        }
        instance = this;
        Explosion.particle = particle;
        border = new GameObject();
        border.tag = "Border";

        UIManager.SetCountdown(5.0f);

    }


    public bool IsFree(int x,int y)
    {
        return grid[x, y] == false;
    }
    
    public void Occupy(int x,int y)
    {
        grid[x, y] = true;
    }
    
    public void Free(int x,int y)
    {
        grid[x, y] = false;
    }

    static public GameManager Manager { get => instance; }

    // Number of grids acrss
    private const int x_grid = 28;
    // numer of grids down
    private const int y_grid = 16;

    private const float size = 1; //Size of grid in game units

    bool[,] grid;//= new bool[x_grid, y_grid];

    /// <summary>
    /// get world space size of playfield
    /// </summary>
    /// <returns></returns>
    public (float x, float y) GetExtents()
    {
        return new(x_grid * size, y_grid * size);
    }

    /// <summary>
    /// Get number of cells
    /// </summary>
    /// <returns></returns>
    public (int x, int y) GetCount()
    {
        return new(x_grid, y_grid);
    }
    public float GetSize()
    {
        return size;
    }

    /// <summary>
    /// Get float of closest grid
    /// </summary>
    /// <param name="p"></param>
    /// <returns></returns>
    public Vector3 Quantise(Vector3 p)
    {
        var (x, y) = GetCell(p.x, p.y);
        var(fx,fy)= GetPosition(x, y);
        return new Vector3(fx, fy, p.z);
    }
    /// <summary>
    /// Given a grid cell, return the float position
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public (float x, float y) GetPosition(int x, int y)
    {
        float sz = size / 2.0f;
        (float xs, float ys) = GetExtents();
        return new(x * size - xs / 2.0f + sz, y * size - ys / 2.0f + sz);
    }
    /// <summary>
    /// Get Cell by gameobject
    /// </summary>
    /// <param name="g"></param>
    /// <returns></returns>
    public (int x, int y) GetCell(GameObject g)
    {
        return GetCell(g.transform.position.x, g.transform.position.y);
    }
    public bool IsOnBorder(GameObject g)
    {
        var (cx, cy) = GetCell(g);
        var (x, y) = GetCount();
        if (cx == 0 || cx == x - 1 || cy == 0 || cy == y - 1)
            return true;
        return false;
    }

    ///Given a position, return the cell indexes
    public (int x, int y) GetCell(float x, float y)
    {
       //float sz = size / 2.0f;
        (float xs, float ys) = GetExtents();
        float tx = xs / 2.0f;
        float ty = ys / 2.0f;
        int ix = (int)Mathf.Floor(((x+tx) / size) );
        int iy = (int)Mathf.Floor(((y+ty) / size) );
        return new(ix,iy);
    }

    public (int x, int y) PickRandomCell()
    {
        return new (Random.Range(0, x_grid),Random.Range(0,y_grid));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator GameOverRoutine()
    {
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene("Title");
    }

    static public void GameOver()
    {
        
    }

    static GameObject border = null;
    static public GameObject WhatsAhead(GameObject target, Vector3 offset)
    {
        var sz = GameManager.Manager.GetSize() / 2.0f;
        List<Collider2D> collisions = new List<Collider2D>();
        ContactFilter2D cf = new ContactFilter2D();
        cf.useTriggers = true;
        cf.layerMask = ~0;

        var coll = Physics2D.OverlapBox(target.transform.position + offset, new Vector2(sz, sz), 0f, cf, collisions);

        if (coll != 0)
        {
            //Make sure we don't hit ourselves
            foreach (var c in collisions)
            {
                if (c.gameObject != target)
                {
                    return c.gameObject;
                }
            }
        }

        var (ex, ey) = GameManager.Manager.GetExtents();
        ex /= 2;
        ey /= 2;

        if (
            target.transform.position.x + offset.x < -ex ||
            //target.transform.position.y + offset.y < -ey || no lower border
            target.transform.position.x + offset.x > ex ||
            target.transform.position.y + offset.y > ey
           )
        {
            return border;
        }

        return null;
    }
    
}
