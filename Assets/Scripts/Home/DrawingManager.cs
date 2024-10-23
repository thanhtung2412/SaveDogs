using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DrawingManager : MonoBehaviour
{
    private LineRenderer pathLineRenderer; 
    private PolygonCollider2D pathPolygonCollider;
    private Rigidbody2D pathRigidbody; 
    private int posCount;
    private float colliderAngle;
    public List<Vector2> newVerticies = new List<Vector2>();
    private List<Vector2> newVerticies2 = new List<Vector2>();
    private List<Vector2> newVerticies_ = new List<Vector2>();
    [SerializeField]private  GameObject path;   
    public GameObject mousePointer;  
    private GameObject clone;
    [HideInInspector]
    public Vector2 centerOfMass = Vector2.zero;
    private RaycastHit2D mouseRay;
    [SerializeField] private LayerMask layerMask;  
    private bool canDraw = true;
    [Header("Line Settings")]
    [SerializeField]private Color colorStart;
    [SerializeField]private Color colorEnd;
    public PhysicsMaterial2D material;
    [Range(0f, 10f)]
    [SerializeField] private float widthStart;  
    [Range(0f, 5f)]
    [SerializeField] float verticesDistance;
    public bool fixedPosition;
    public bool isPermanent;
    public float lifeTime;  
    [Range(0f, 10f)]
    public float massScale;
    [Range(-10f, 10f)]    
    private bool prepareDrawFinish;
    private Vector2 touchDownPosition;
    private bool isBeginDrawOnTouch;

    private void Start()
    {      
        verticesDistance = 0.1f;
        lifeTime = 2f;
        isPermanent = true;                   
        pathLineRenderer = path.GetComponent<LineRenderer>();
        pathLineRenderer.useWorldSpace = false;       
        posCount = 0;
    }
    private void Update()
    {
        EventSystem current = EventSystem.current;
        if (current != null && current.IsPointerOverGameObject())
        {
            return;
        }     
        mouseRay = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(GetTouchPosition()), Vector2.zero, float.PositiveInfinity, layerMask);
        if (FixGetMouseButtonDown())
        {
            OnTouchDown();
        }
        if (FixGetMouseButton())
        {
            OnTouchMove();
        }
        if (FixGetMouseButtonUp())
        {
            OnTouchUp();
        } 
    }
    private void FixedUpdate()
    {
        mousePointer.GetComponent<TargetJoint2D>().target = Camera.main.ScreenToWorldPoint(GetTouchPosition());
    }
    private void InitCanDraw()
    {
        if (mouseRay.collider != null)
        {
            canDraw = false;
        }
    }
    private void PrepareDraw()
    {
        mousePointer.transform.position = Camera.main.ScreenToWorldPoint(GetTouchPosition());
        mousePointer.GetComponent<CircleCollider2D>().isTrigger = true;
        mousePointer.transform.localScale = new Vector3(widthStart, widthStart, widthStart);
        StarPrefab();      
        centerOfMass = Vector2.zero;
        prepareDrawFinish = true;
    } 
    #region TouchDown
    private void InitDrawWhenTouch()
    {
        newVerticies.Clear();
        newVerticies.Add(Vector3.zero);
        mousePointer.transform.position = Camera.main.ScreenToWorldPoint(this.GetTouchPosition());
        touchDownPosition = mousePointer.transform.position;
    }

    private void OnTouchDown()
    {      
        prepareDrawFinish = false;
        newVerticies.Clear();
        canDraw = true;
        isBeginDrawOnTouch = false;
        InitCanDraw();
        if (canDraw)
        {
            InitDrawWhenTouch();
        }
    }
    #endregion
    private void OnTouchMove()
    {
        if (!prepareDrawFinish)
        {
            canDraw = true;
            InitCanDraw();
        }
        if (canDraw)
        {
            if (isBeginDrawOnTouch)
            {
                DrawVisibleLine();
            }
            else
            {
                Vector2 a = Camera.main.ScreenToWorldPoint(GetTouchPosition());
                if (Vector2.Distance(a, touchDownPosition) > 0.1f && !prepareDrawFinish)
                {
                    isBeginDrawOnTouch = true;
                    PrepareDraw();
                }
            }
        }
    }
    private void OnTouchUp()
    {     
        if (canDraw)
        {
            mousePointer.GetComponent<CircleCollider2D>().isTrigger = true;
            MakeLinePhysics();
        }
      
        gameObject.SetActive(false);
    }
    private void MakeLinePhysics()
    {
        pathPolygonCollider.isTrigger = false;
        newVerticies2.Clear();
        newVerticies.Add(mousePointer.transform.position);
        for (int i = 0; i < newVerticies.Count - 1; i++)
        {
            if (i < newVerticies.Count - 2)
            {
                colliderAngle = Mathf.Atan2(newVerticies[i].y - newVerticies[i + 1].y, newVerticies[i].x - newVerticies[i + 1].x);
                colliderAngle += 1.57079637f;
            }
            float num = Mathf.Cos(colliderAngle);
            float num2 = Mathf.Sin(colliderAngle);
            newVerticies2.Add(new Vector2(newVerticies[i].x + widthStart / 2f * num, newVerticies[i].y + widthStart / 2f * num2));
            newVerticies2.Insert(0,new Vector2(newVerticies[i].x - widthStart / 2f * num, newVerticies[i].y - widthStart / 2f * num2));
        }
        pathPolygonCollider.SetPath(0, newVerticies2.ToArray());
        pathPolygonCollider.sharedMaterial = material;
        CalculatesPrevCenterOfMassAndMass((float)(newVerticies.Count - 1));    
        pathRigidbody.mass *= massScale;         
        pathRigidbody.bodyType = RigidbodyType2D.Dynamic;
    }
    private void StarPrefab()
    {
        clone = Instantiate(path, Vector3.zero, Quaternion.identity);
        clone.name = "Drawing";       
        pathLineRenderer = clone.GetComponent<LineRenderer>();
        pathRigidbody = clone.GetComponent<Rigidbody2D>();
        pathRigidbody.bodyType = RigidbodyType2D.Kinematic;
        pathLineRenderer.startColor = colorStart;
        pathLineRenderer.endColor = colorEnd;
        pathLineRenderer.startWidth = widthStart;
        pathLineRenderer.endWidth = widthStart;
        newVerticies.Clear();
        newVerticies_.Clear();
        newVerticies2.Clear();
        pathLineRenderer.positionCount = 1;
        pathLineRenderer.SetPosition(0, mousePointer.transform.position - new Vector3(0f, 0f, Camera.main.transform.position.z));
        newVerticies.Add(mousePointer.transform.position);
        newVerticies_.Add(mousePointer.transform.position);
        pathPolygonCollider = clone.GetComponent<PolygonCollider2D>();
        pathPolygonCollider.isTrigger = true;
    }
    private void DrawVisibleLine()
    {
        if (Vector2.Distance(mousePointer.transform.position, newVerticies_[posCount]) > verticesDistance)
        {
            if (CanPassWhenDraw(newVerticies[newVerticies.Count - 1], mousePointer.transform.position))
            {             
                posCount++;
                pathLineRenderer.positionCount = posCount + 1;
                pathLineRenderer.SetPosition(posCount, mousePointer.transform.position - new Vector3(0f, 0f, Camera.main.transform.position.z));
                newVerticies_.Add(mousePointer.transform.position);
                newVerticies.Add(mousePointer.transform.position);
                newVerticies2.Clear();
                newVerticies.Add(mousePointer.transform.position);
                for (int i = 0; i < newVerticies.Count - 1; i++)
                {
                    colliderAngle = Mathf.Atan2(newVerticies[i].y - newVerticies[i + 1].y, newVerticies[i].x - newVerticies[i + 1].x);
                    colliderAngle += 1.57079637f;
                    float num = Mathf.Cos(colliderAngle);
                    float num2 = Mathf.Sin(colliderAngle);
                    float num3 = Mathf.Lerp(widthStart, widthStart, (float)i / (float)(newVerticies.Count - 2));
                    newVerticies2.Add(new Vector2(newVerticies[i].x + num3 / 2f * num, newVerticies[i].y + num3 / 2f * num2));
                    newVerticies2.Insert(0, new Vector2(newVerticies[i].x - num3 / 2f * num, newVerticies[i].y - num3 / 2f * num2));
                }
                newVerticies.RemoveAt(newVerticies.Count - 1);
                pathPolygonCollider.SetPath(0, newVerticies2.ToArray());
                pathPolygonCollider.sharedMaterial = material;
            }      
        }
    }
    private void CalculatesPrevCenterOfMassAndMass(float width)
    {       
        if (newVerticies.Count > 2)
        {
            pathRigidbody.mass += width;      
        }
    }
    #region Input
    private bool FixGetMouseButtonDown()
    {
        return Input.GetMouseButtonDown(0);
    }
    private bool FixGetMouseButton()
    {
        return Input.GetMouseButton(0);
    }
    private bool FixGetMouseButtonUp()
    {
        return Input.GetMouseButtonUp(0);
    }
    private Vector3 GetTouchPosition()
    {
        return Input.mousePosition;
    }
    #endregion
    public bool CanPassWhenDraw(Vector2 begin, Vector2 end)
    {     
        RaycastHit2D[] array = Physics2D.RaycastAll(begin, end - begin, Vector2.Distance(begin, end));
        if (array.Length > 0)
        {
            RaycastHit2D[] array2 = array;
            for (int i = 0; i < array2.Length; i++)
            {
                RaycastHit2D raycastHit2D = array2[i];
                if (!(raycastHit2D.collider.gameObject == mousePointer))
                {
                    if (!raycastHit2D.collider.gameObject.name.Contains("Drawing"))
                    {
                        if (!(raycastHit2D.collider.gameObject.GetComponent<Target>() != null))
                        {                          
                            return false;
                        }
                    }
                }
            }
        }
        return true;
    }
}
