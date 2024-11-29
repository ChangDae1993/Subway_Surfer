using UnityEngine;

public class playermotor : MonoBehaviour
{
    public CharacterController controller;

    private float speed = 5f;

    private void Awake()
    {
        if (controller == null)
            controller = GetComponent<CharacterController>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        controller.Move(Vector3.forward * Time.deltaTime * speed);
    }
}
