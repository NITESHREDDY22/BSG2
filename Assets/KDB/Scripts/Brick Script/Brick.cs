using UnityEngine;
using System.Collections;

public class Brick : MonoBehaviour
{

    public float health = 70f;
    void OnCollisionEnter2D(Collision2D target)
    {
        if (target.gameObject.GetComponent<Rigidbody2D>() == null)
            return;
        float damage = target.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude * 10f;
        if (damage > 10)
        {
            int soundProp = PlayerPrefs.GetInt("SOUND");
            if (soundProp == 1)
            {
                //SoundManager.PlaySFX("WoodTap", false, 0);
            }
        }
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
