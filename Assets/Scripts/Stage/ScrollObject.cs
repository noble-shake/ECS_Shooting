using UnityEngine;

public class ScrollObject : MonoBehaviour
{
    public Vector3 scrollInit;
    public Vector3 scrollEnd;
    public float scrollSpeed;
    public float ScrollSpeed { get { return scrollSpeed; } set { scrollSpeed = value; } }

    private void Update()
    {

        transform.localPosition = transform.localPosition + new Vector3(0f, 0f, -Time.deltaTime * scrollSpeed);

        if (transform.localPosition.z < scrollEnd.z)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, scrollInit.z);
        }


    }
}