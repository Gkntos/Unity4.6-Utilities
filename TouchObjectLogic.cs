using UnityEngine;
using System.Collections;

public class TouchObjectLogic : MonoBehaviour
{
    private Ray ray;
    private RaycastHit rayHit;
    private ITouchObject touchedObject;

    void Update()
    {
        //Si hay touches.
        if (Input.touchCount > 0)
        {
            //Analizar cada touch en Input.touches
            foreach (Touch t in Input.touches)
            {
                //Se genera Ray de la posisión del touch
                ray = Camera.main.ScreenPointToRay(t.position);
                if (Physics.Raycast(ray, out rayHit))
                {
                    touchedObject = rayHit.transform.GetComponent(typeof(ITouchObject)) as ITouchObject;
                    //Si se toco un objeto(touchable), se ejecuta la accion del objeto.
                    if (touchedObject != null)
                    {
                        switch (t.phase)
                        {
                            case TouchPhase.Began:
                                touchedObject.onTouchBegan(t);
                                break;
                            case TouchPhase.Ended:
                                touchedObject.onTouchEnded(t);
                                break;
                            case TouchPhase.Moved:
                                touchedObject.onTouchMoved(t.deltaPosition);
                                break;
                            case TouchPhase.Stationary:
                                touchedObject.onTouchStayed(t, t.deltaTime);
                                break;
                        }
                    }
                }
                else
                {
                    //Logica si touch no toco algun objeto.
                }
            }
        }
    }
}
