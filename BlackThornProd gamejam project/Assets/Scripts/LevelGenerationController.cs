﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerationController : MonoBehaviour {

    [SerializeField]
    [Tooltip("Расстояние в юнитах от центра геймобжекта до левого или правого краёв, за пределами которых происходит генерация карты")]
    private int _distanceToABorder;

    [SerializeField]
    [Tooltip("Задержка между проверками создания/уничтожения объектов")]
    private float _delayToCheckBuildAndRemoveObjects = 1;

    //вероятно, лучше эти числа получить из отношения размеров экрана к размерам тайлов, но пока вобьём руками
    [Header("Платформы")]

    [SerializeField]
    private int _sizeOfThePlatformPartsObjectPool;
    [SerializeField]
    [Range(2,10)]
    private int _minPlatformLength = 3;
    [SerializeField]
    [Range(3, 20)]
    private int _maxPlatformLength = 10;
    [SerializeField]
    private int _firstPlatformLength = 5;

    [SerializeField]
    [Range(1.5f, 2f)]
    private float _minXDistanceBetweenPlatforms = 1.5f;
    [SerializeField]
    [Range(2f, 4f)]
    private float _maxXDistanceBetweenPlatforms = 3f;

    [SerializeField]
    [Range(0.1f, 1f)]
    private float _minYDistanceBetweenPlatforms = 0.2f;
    [SerializeField]
    [Range(1f, 3f)]
    private float _maxYDistanceBetweenPlatforms = 2f;




    [SerializeField]
    private Sprite _leftPlatformEdgeSprite;
    [SerializeField]
    private Sprite _rightPlatformEdgeSprite;
    [SerializeField]
    private Sprite[] _platformMiddlePartsSprites;

    [SerializeField]
    private GameObject _platformPartPrefab;
    [SerializeField]
    private GameObject _testPickupPrefab;



    [Header("Баффы на платформах")]

    [SerializeField]
    private int _sizeOfPickupBuffsOnPlatformsObjectPool = 5;
    [SerializeField]
    [Range(10f, 30f)]
    private float _minXDistanceBetweenBuffsOnPlatforms = 20f;
    [SerializeField]
    [Range(30f, 100f)]
    private float _maxXDistanceBetweenBuffsOnPlatforms = 50f;
    [SerializeField]
    [Range(3f, 10f)]
    private float _yOfRaycastTesterForBuffsOnPlatforms = 5f;
    [SerializeField]
    [Range(0.2f, 2f)]
    private float _buffOnPlatformYDistanceToPlatform = 1f;
    [SerializeField]
    private float _delayBeforeFirstBuffOnPlatforms = 10f;
    [SerializeField]
    private float _minDelayBetweenBuffOnPlatforms = 5f;
    [SerializeField]
    private float _maxDelayBetweenBuffOnPlatforms = 20f;


    [SerializeField]
    private GameObject _pickupBuffOnPlatformsPrefab;
    


    //private float _platformPartPrefabWidth;

    private PlatformGenerator _platformGenerator;
    private PropsGenerator _backgroundGenerator;
    private PropsGenerator _someShitGenerator;
    private ObjectsLyingOnPlatformsGenerator _buffsOnPlatformsGenerator;


    private GameObject _tempPlatformPart;

    private PixelGridSnapper _gridSnapper;



    void Start()
    {
        _gridSnapper = FindObjectOfType<GameManager>().PixelGridSnapper;

        _platformGenerator = new PlatformGenerator(_sizeOfThePlatformPartsObjectPool, transform, _platformPartPrefab, _firstPlatformLength, _leftPlatformEdgeSprite, _platformMiddlePartsSprites, _rightPlatformEdgeSprite, _gridSnapper);
        _someShitGenerator = new PropsGenerator(30, transform, _testPickupPrefab, new Vector3(5, 1, 0), _gridSnapper);//тестовый

        _buffsOnPlatformsGenerator = new ObjectsLyingOnPlatformsGenerator(_sizeOfPickupBuffsOnPlatformsObjectPool, transform, _pickupBuffOnPlatformsPrefab, _delayBeforeFirstBuffOnPlatforms, _minDelayBetweenBuffOnPlatforms, _maxDelayBetweenBuffOnPlatforms);
                

        StartCoroutine(LevelGenerationLoop());
    }
     

    private IEnumerator LevelGenerationLoop()
    {
        WaitForSeconds delay = new WaitForSeconds(_delayToCheckBuildAndRemoveObjects);        

        while (true)
        {
            int leftBorderX = (int)(transform.position.x - _distanceToABorder);
            int rightBorderX = (int)(transform.position.x + _distanceToABorder);


            _platformGenerator.CheckAndTryCreatePlatform(rightBorderX, _minPlatformLength, _maxPlatformLength, _minXDistanceBetweenPlatforms, _maxXDistanceBetweenPlatforms, _minYDistanceBetweenPlatforms, _maxYDistanceBetweenPlatforms, _leftPlatformEdgeSprite, _platformMiddlePartsSprites, _rightPlatformEdgeSprite, _gridSnapper);

            _buffsOnPlatformsGenerator.CheckAndTryCreateObjectOnPlatform(_delayToCheckBuildAndRemoveObjects, rightBorderX, _buffOnPlatformYDistanceToPlatform, _gridSnapper);

            _someShitGenerator.CheckAndTryCreateObject(rightBorderX, _minXDistanceBetweenPlatforms, _maxXDistanceBetweenPlatforms, -5, 5, _gridSnapper);//тестовый



            _platformGenerator.CheckAndTryRemoveObjects(leftBorderX);

            _buffsOnPlatformsGenerator.CheckAndTryRemoveObjects(leftBorderX);

            _someShitGenerator.CheckAndTryRemoveObjects(leftBorderX);//тестовый


            yield return delay;
        }
    }
    

    //а это так, посмотреть, как рандом работает в Юнити и как можно организовать точное воспроизведение уровня через "сид", точнее через стейт.
    public void TestRandom()
    {

        Random.InitState(12345);//задаём стейт числом

        Debug.Log("Первое число - " + Random.Range(0, 1001));
        Debug.Log("Второе число - " + Random.Range(0, 1001));
        Debug.Log("Третье число - " + Random.Range(0, 1001));

        Random.InitState(12345);//задаём стейт числом

        Debug.Log("Первое число ещё раз - " + Random.Range(0, 1001));
        Debug.Log("Второе число ещё раз - " + Random.Range(0, 1001));
        Debug.Log("Третье число другая формула - " + Random.value);

        Random.State state = Random.state;//сохраняем стейт

        Debug.Log("Новое первое число - " + Random.Range(0, 1001));
        Debug.Log("Новое торое число - " + Random.Range(0, 1001));
        Debug.Log("Новое третье число другая формула - " + Random.value);

        Random.state = state;//передаём сохранённый стейт

        Debug.Log("Новое первое число ещё раз - " + Random.Range(0, 1001));
        Debug.Log("Новое торое число ещё раз - " + Random.Range(0, 1001));
        Debug.Log("Новое третье число другая формула ещё раз - " + Random.value);

        //делаем рандомный стейт
        string timeTemp = System.DateTime.Now.Ticks.ToString();
        Random.InitState(System.Convert.ToInt16(timeTemp.Substring(timeTemp.Length - 4)));

        Debug.Log("Новое первое число ещё раз - " + Random.Range(0, 1001));
        Debug.Log("Новое торое число ещё раз - " + Random.Range(0, 1001));
        Debug.Log("Новое третье число другая формула ещё раз - " + Random.value);
    }


}