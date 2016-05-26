//Interfaz para objetos 3D que pueden recibir eventos de Touch
//El objeto debe implementar la interfaz
public interface ITouchObject
{
    void onTouchBegan(UnityEngine.Touch touch);
    void onTouchEnded(UnityEngine.Touch touch);
    void onTouchMoved(UnityEngine.Vector2 deltaPosition);
    void onTouchStayed(UnityEngine.Touch touch, float deltaTime);
}
