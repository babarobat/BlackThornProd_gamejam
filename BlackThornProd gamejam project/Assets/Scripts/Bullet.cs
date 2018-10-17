﻿using UnityEngine;
/// <summary>
/// Содержит логику и поля пули
/// </summary>
class Bullet : MonoBehaviour
{
    /// <summary>
    /// Скорость пули
    /// </summary>
    [HideInInspector]
    public float Speed;
    /// <summary>
    /// Время, через которое снаряд будет уничтожен, если ни во что не врежется
    /// </summary>

    
    private PlayerController _playerController;

    private const float _destroyTime = 1.5f;
    

    private void Start()
    {
        DestroyBullet(_destroyTime);
        _playerController = FindObjectOfType<PlayerController>();
    }
    private void Update()
    {
        transform.Translate(Vector3.right * Speed * Time.deltaTime);
    }
    /// <summary>
    /// Логика уничтожения пули
    /// </summary>
    private void DestroyBullet()
    {
        //анимация взрыва
        Destroy(gameObject);
        return;
    }
    /// <summary>
    /// логика уничтожения пули через промежуток времени
    /// </summary>
    /// <param name="time">Время до уничтожения</param>
    private void DestroyBullet(float time)
    {
        //анимация взрыва
        Destroy(gameObject, time);
        return;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        
        if (collision.tag == "PickUp" || collision.tag == "Destractable")
        {
            var collisionScript = collision.GetComponent<ScrollingGameObject>();
            switch (collisionScript.TypeOf)
            {
                case TypeOfObject.none:
                    break;
                case TypeOfObject.buff:
                    Debug.Log("bullet destroy buff");
                    collisionScript.SwitchVisibility();
                    DestroyBullet();
                    break;
                case TypeOfObject.debuff:
                    Debug.Log("bullet destroy debuff");
                    collisionScript.SwitchVisibility();
                    DestroyBullet();
                    break;
                case TypeOfObject.life:
                    collisionScript.SwitchVisibility();
                    DestroyBullet();
                    break;
                case TypeOfObject.weapon:
                    collisionScript.SwitchVisibility();
                    DestroyBullet();
                    break;
                case TypeOfObject.bullet:
                    collisionScript.SwitchVisibility();
                    DestroyBullet();
                    break;
                case TypeOfObject.bug:
                    Debug.Log("bullet destroy bug");
                    collisionScript.SwitchVisibility();
                    DestroyBullet();
                    break;
                case TypeOfObject.destructable:
                    Debug.Log("bullet destroy Wall");
                    collisionScript.SwitchVisibility();
                    DestroyBullet();
                    break;

            }
        }
        
    }
}

