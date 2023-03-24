using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

<<<<<<< HEAD
namespace BossArena.game
{


    class AutoAttack : AbilityBase
    {

        // Need to have reference to Camera.
        [SerializeField]
        private Camera mainCamera;

        // Need to have reference to Parent Player Prefab
        [SerializeField]
        private GameObject PlayerPrefab;

        // Parent Player Prefab MUST have AutoAttackCollider Prefab\
        private GameObject AUTOATTACK_COLLIDER;


        Vector3 currentMousePosition;

        // Start is called before the first frame update
        void Start()
        {
            // Get the Prefab holding the BoxCollider2D
            AUTOATTACK_COLLIDER = PlayerPrefab.transform.GetChild(0).gameObject;
            AUTOATTACK_COLLIDER.SetActive(false);

        }

        // Update is called once per frame
        protected override void Update()
        {
            //// Get and Convert Mouse Position into World Coordinates
            //currentMousePosition = new Vector3(mainCamera.ScreenToWorldPoint(Input.mousePosition).x, mainCamera.ScreenToWorldPoint(Input.mousePosition).y, 0f);
            //// Calculate Focus Cursor
            //Vector2 focusCursor = calculateFocusCursor();

            //AUTOATTACK_COLLIDER.transform.position = focusCursor;
            //UnityEngine.Debug.Log("COLLIDER: " + AUTOATTACK_COLLIDER.transform.position);
            this.DrawAbilityIndicator(Input.mousePosition);
            if (Input.GetMouseButtonDown(0))
            {
                this.ActivateAbility(Input.mousePosition);

            }
            //else
            //{
            //    // Enable Collider for short amount of time.
            //    AUTOATTACK_COLLIDER.SetActive(false);
            //}

            /*transform.position = currentMousePosition;*/

        }

        public override void ActivateAbility(Vector3? mosPos = null)
        {
            //currentMousePosition = new Vector3(mainCamera.ScreenToWorldPoint(mosPos).x, mainCamera.ScreenToWorldPoint(mosPos).y, 0f);
            //// Calculate Focus Cursor
            //Vector2 focusCursor = calculateFocusCursor();

            //AUTOATTACK_COLLIDER.transform.position = focusCursor;
            //UnityEngine.Debug.Log("COLLIDER: " + AUTOATTACK_COLLIDER.transform.position);

            AUTOATTACK_COLLIDER.SetActive(true);


            AUTOATTACK_COLLIDER.SetActive(false);

        }

        public override void DrawAbilityIndicator(Vector3 targetLocation)
        {
            // Get and Convert Mouse Position into World Coordinates
            currentMousePosition = new Vector3(mainCamera.ScreenToWorldPoint(Input.mousePosition).x, mainCamera.ScreenToWorldPoint(Input.mousePosition).y, 0f);
            // Calculate Focus Cursor
            Vector2 focusCursor = calculateFocusCursor();

            AUTOATTACK_COLLIDER.transform.position = focusCursor;
            UnityEngine.Debug.Log("COLLIDER: " + AUTOATTACK_COLLIDER.transform.position);
        }

        private void OnDrawGizmos()
        {
            Vector3 focusCursor = calculateFocusCursor();
            Gizmos.color = new Color(1, 1, 1, 0.25f);
            drawFocusCursor();

            /*        if (Input.GetMouseButtonDown(0))
                    {
                        drawFocusCursor();
                    }*/

            /*        Gizmos.DrawCube(new Vector3(3, 4, currentMousePosition.z), new Vector3(1, 1, 1)); */


            /*        Vector2 mousePosition = Input.mousePosition;
                    UnityEngine.Debug.Log(mousePosition);

                    Gizmos.color = new Color(1, 0, 0, 0.5f);
                    Gizmos.DrawCube(new Vector2(mousePosition.x, mousePosition.y), new Vector3(1, 1, 1));*/
        }

        protected Vector3 calculateFocusCursor()
        {
            /*Vector2 mousePosition = Input.mousePosition;
            UnityEngine.Debug.Log("MousePosition - X:" + mousePosition.x + "Y:" + mousePosition.y);*/


            float angle = Mathf.Atan2(this.currentMousePosition.y - transform.position.y, currentMousePosition.x - transform.position.x);

            UnityEngine.Debug.Log("Transform Position: " + transform.position);

            float focusX = transform.position.x + Mathf.Cos(angle);
            float focusY = transform.position.y + Mathf.Sin(angle);

            Vector3 focusCursorPosition = new Vector3(focusX, focusY, 0f);
            UnityEngine.Debug.Log(focusCursorPosition);

            return focusCursorPosition;


            /* Gizmos.color = new Color(1, 0, 0, 0.5f);
             Gizmos.DrawCube(new Vector2(mousePosition.x, mousePosition.y), new Vector3(1, 1, 1));*/
        }

        protected void drawFocusCursor()
        {
            UnityEngine.Debug.Log("Auto Attacking");
            Gizmos.color = new Color(1, 0, 0, 0.5f);
            Gizmos.DrawCube(calculateFocusCursor(), new Vector3(1, 1, 1));
        }

    }
}
=======
public class AutoAttack : MonoBehaviour
{

    // Need to have reference to Camera.
    [SerializeField]
    private Camera mainCamera;

    // Need to have reference to Parent Player Prefab
    [SerializeField]
    private GameObject PlayerPrefab;

    // Parent Player Prefab MUST have AutoAttackCollider Prefab\
    private GameObject AUTOATTACK_COLLIDER;
    

    Vector3 currentMousePosition;

    // Start is called before the first frame update
    void Start()
    {
        // Get the Prefab holding the BoxCollider2D
        AUTOATTACK_COLLIDER = PlayerPrefab.transform.GetChild(0).gameObject;
        AUTOATTACK_COLLIDER.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        // Get and Convert Mouse Position into World Coordinates
        currentMousePosition = new Vector3(mainCamera.ScreenToWorldPoint(Input.mousePosition).x, mainCamera.ScreenToWorldPoint(Input.mousePosition).y, 0f);
        // Calculate Focus Cursor
        Vector2 focusCursor = calculateFocusCursor();

        AUTOATTACK_COLLIDER.transform.position = focusCursor;
        UnityEngine.Debug.Log("COLLIDER: " + AUTOATTACK_COLLIDER.transform.position);

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("peepeepoopoo");
            // Enable Collider for short amount of time.
            AUTOATTACK_COLLIDER.SetActive(true);

        } else
        {
            // Enable Collider for short amount of time.
            AUTOATTACK_COLLIDER.SetActive(false);
        }

        /*transform.position = currentMousePosition;*/

    }

    private void OnDrawGizmos()
    {
        Vector3 focusCursor = calculateFocusCursor();
        Gizmos.color = new Color(1, 1, 1, 0.25f);
        drawFocusCursor();

/*        if (Input.GetMouseButtonDown(0))
        {
            drawFocusCursor();
        }*/

/*        Gizmos.DrawCube(new Vector3(3, 4, currentMousePosition.z), new Vector3(1, 1, 1)); */


/*        Vector2 mousePosition = Input.mousePosition;
        UnityEngine.Debug.Log(mousePosition);

        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(new Vector2(mousePosition.x, mousePosition.y), new Vector3(1, 1, 1));*/
    }

    protected Vector3 calculateFocusCursor()
    {
        /*Vector2 mousePosition = Input.mousePosition;
        UnityEngine.Debug.Log("MousePosition - X:" + mousePosition.x + "Y:" + mousePosition.y);*/


        float angle = Mathf.Atan2(  this.currentMousePosition.y - transform.position.y, currentMousePosition.x - transform.position.x);

        UnityEngine.Debug.Log("Transform Position: " + transform.position);

        float focusX = transform.position.x + Mathf.Cos(angle);
        float focusY = transform.position.y + Mathf.Sin(angle);

        Vector3 focusCursorPosition = new Vector3(focusX, focusY, 0f);
        UnityEngine.Debug.Log(focusCursorPosition);

        return focusCursorPosition;


       /* Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(new Vector2(mousePosition.x, mousePosition.y), new Vector3(1, 1, 1));*/
    }

    protected void drawFocusCursor()
    {
        UnityEngine.Debug.Log("Auto Attacking");
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(calculateFocusCursor(), new Vector3(1, 1, 1));
    }

}
>>>>>>> 785de7ee86b3265043adaa638a2b31764b5b0d38
