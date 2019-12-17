using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] _allPlatforms;
    [SerializeField] private GameObject[] _selectedPlatforms = new GameObject[4];
    [SerializeField] private GameObject _winPrefab;
    
    private GameObject _normalPlatforms, _winPlatform;
    public int _level = 1, _platformAddition = 7;
    [SerializeField] private float _rotationSpeed = 10f;
    private float i = 0;
    public Material plateMaterial, baseMaterial;
    public Image currentLevelImage, nextLevelImage, progressBarImage;
    public MeshRenderer playerMesh;

    void Awake()
    {
        LevelManagement();
    }

    private void LevelManagement()
    {
        plateMaterial.color = Random.ColorHSV(0, 1, .5f, 1, 1, 1);
        baseMaterial.color = plateMaterial.color + Color.gray;
        playerMesh.material.color = plateMaterial.color;
        currentLevelImage.color = plateMaterial.color;
        nextLevelImage.color = plateMaterial.color;
        progressBarImage.color = plateMaterial.color;

        _level = PlayerPrefs.GetInt("Level", 1);
        if (_level > 9)
            _platformAddition = 0;

        PlatformSelection();

        //Instead of increasing the i, I decreased it so I can instantiate them below. Platform addition is just the amount of platforms, you can change it to your liking. 0.5f is also an approach that I picked so you can change that to your liking as well.
        for (i = 0; i > -_level - _platformAddition; i -= 0.5f)
        {
            //Select platforms related to difficulty
            if (_level <= 40)
                _normalPlatforms = Instantiate(_selectedPlatforms[Random.Range(0, 2)]);
            if (_level > 40 && _level <= 80)
                _normalPlatforms = Instantiate(_selectedPlatforms[Random.Range(1, 3)]);
            if (_level > 80 && _level <= 140)
                _normalPlatforms = Instantiate(_selectedPlatforms[Random.Range(2, 4)]);
            if (_level > 140)
                _normalPlatforms = Instantiate(_selectedPlatforms[Random.Range(3, 4)]);

            _normalPlatforms.transform.position = new Vector3(0, i, 0);
            _normalPlatforms.transform.eulerAngles = new Vector3(0, i * _rotationSpeed, 0);

            if (Mathf.Abs(i) >= _level * .3f && Mathf.Abs(i) <= _level * .6f) //putting some randomness to interrupt the level being so monoton, you can change .3f and .6f to your liking. 
            {
                _normalPlatforms.transform.eulerAngles += Vector3.up * 180;
            }

            _normalPlatforms.transform.parent = FindObjectOfType<Platforms>().transform;
        }

        _winPlatform = Instantiate(_winPrefab);
        _winPlatform.transform.position = new Vector3(0, i, 0);
    }

    void PlatformSelection()
    {
        int randomModel = Random.Range(0, 5);
        switch (randomModel)
        {
            case 0:
                for (int i = 0; i < 4; i++)
                    _selectedPlatforms[i] = _allPlatforms[i];
                break;
            case 1:
                for (int i = 0; i < 4; i++)
                    _selectedPlatforms[i] = _allPlatforms[i + 4];
                break;
            case 2:
                for (int i = 0; i < 4; i++)
                    _selectedPlatforms[i] = _allPlatforms[i + 8];
                break;
            case 3:
                for (int i = 0; i < 4; i++)
                    _selectedPlatforms[i] = _allPlatforms[i + 12];
                break;
            case 4:
                for (int i = 0; i < 4; i++)
                    _selectedPlatforms[i] = _allPlatforms[i + 16];
                break;
        }
    }

    public void IncreaseTheLevel()
    {
        PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level") + 1);
        SceneManager.LoadScene(0);
    }
}
