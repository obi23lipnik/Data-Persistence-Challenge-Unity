using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public new string name;
    public int highScore = 0;

    public Text ScoreText;
    public Text Highscore;
    public GameObject GameOverText;
    
    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;

    
    // Start is called before the first frame update
    void Start()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
        DisplayHighscore();
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            UpdateHighscore();
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
    }

    [System.Serializable]
    class PlayerHighscore
    {
        public string name;
        public int score;
    }
    private PlayerHighscore LoadHighscore()
    {
        PlayerHighscore data = new PlayerHighscore
        {
            name = "None",
            score = 0
        };
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            PlayerHighscore saved_data = JsonUtility.FromJson<PlayerHighscore>(json);

            highScore = saved_data.score;
            data = saved_data;
        }
        return data;
    }
    public void SaveHighscore()
    {
        PlayerHighscore data = new PlayerHighscore();
        if (highScore < m_Points)
        {
            data.name = NameManager.Instance.playerName;
            data.score = m_Points;

            string json = JsonUtility.ToJson(data);

            File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
        }
    }
    public void UpdateHighscore()
    {
        LoadHighscore();
        SaveHighscore();
    }
    public void DisplayHighscore()
    {
        PlayerHighscore data = LoadHighscore();
        Highscore.text = "Best Score : " + data.name + " - " + data.score;
    }
}
