using UnityEngine;

public class Player_Anim_Cotrol : MonoBehaviour
{
    public CapsuleCollider thisCol;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //void Start()
    //{

    //}

    // Update is called once per frame
    //void Update()
    //{

    //}

    public void RollOn()
    {
        thisCol.center = new Vector3(0, 0.5f, 0);
        thisCol.height = 1.1f;
    }

    public void RollOff()
    {
        thisCol.center = new Vector3(0, 0.89f, 0);
        thisCol.height = 1.86f;
    }

    public void FootStepSound()
    {
        AudioManager.AM.PlaySfx(AudioManager.Sfx.footstep);
    }

}
