using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;




public class GameManager : Singleton<GameManager>
{

    int t = 0;

    public FloatingJoystick joystick;
    public GameObject playerPrefabs;
    public GameObject allyPrefabs;
    public GameObject enemyPrefabs;

    public GameObject healthBar;
    [SerializeField] GameObject canvas;

    [Header("space info")]
     [SerializeField] float spaceX = 0.5f;
    [SerializeField] float spaceZ = 1f;
    [SerializeField] float spaceZTemp = 0.3f;
    [SerializeField] float startX = 2f;
    [SerializeField] Transform spawnParentPoint;
    [SerializeField]
    private Cinemachine.CinemachineVirtualCamera virtualCamera;

    GameModel gameModel = new GameModel();
    List<LevelData> allLevels = new List<LevelData>();

    public List<HealthBar> healthBars = new List<HealthBar>();

    public int currentLevelIndex = 0;

    [Header("Countdown")]
    [SerializeField] private TMPro.TextMeshProUGUI countdownText;
    private bool gameStarted = false;
    private bool isGameOver = false;

    [SerializeField] private int currentLevelTemp = 0;
    protected override void Awake()
    {
        base.Awake();
    }




    void Start()
    {

        ObjectPool.Instance.RegisterPrefab(CONST.POOL_ENEMY, enemyPrefabs);
        ObjectPool.Instance.RegisterPrefab(CONST.POOL_ALLY, allyPrefabs);
        ObjectPool.Instance.RegisterPrefab(CONST.POOL_PLAYER, playerPrefabs);
        ObjectPool.Instance.RegisterPrefab(CONST.POOL_HEALTHBAR, healthBar);

        ObjectPool.Instance.PreloadObjects(CONST.POOL_ENEMY, 25);
        ObjectPool.Instance.PreloadObjects(CONST.POOL_ALLY, 24);
        ObjectPool.Instance.PreloadObjects(CONST.POOL_PLAYER, 2);
        ObjectPool.Instance.PreloadObjects(CONST.POOL_HEALTHBAR, 50);


        isGameOver = true;


        string saveFilePath = System.IO.Path.Combine(Application.persistentDataPath, CONST.SAVE_FILE_NAME);
        if (!System.IO.File.Exists(saveFilePath))
        {
            Debug.Log("Save file not found. Creating initial game data for the first launch.");
            gameModel.GenerateAndSaveLevels();
            gameModel.SaveLevels(0);

        }
        else
        {

            Debug.Log("Existing save file found. Skipping initial save.");
        }
        allLevels = gameModel.LoadAllLevels();

    }
    public int GetCurrentLevelIndex()
    {
        return gameModel.LoadLevels()+1;
    }

    private void StartGame()
    {
        Debug.Log("StartGame called");
        ResetLevel();
        //isGameOver = false;
        currentLevelIndex = gameModel.LoadLevels();
        LevelData firstLevel = allLevels[currentLevelIndex];
        SpawnObject(spaceX, spaceZ, firstLevel);
        gameStarted = false;
        StartCoroutine(StartCountdown());
    }

    private void ChallengeMode()
    {
        ResetLevel();
        //isGameOver = false;
        LevelData firstLevel = gameModel.GenerateChallengeLevel();
        currentLevelIndex = 10;
        SpawnObject(spaceX, spaceZ, firstLevel);
        gameStarted = false;
        StartCoroutine(StartCountdown());
    }

    private void OnEnable()
    {
        ListenerManager.Instance.AddListener(EventName.StartNextLevel, StartGame);
        ListenerManager.Instance.AddListener(EventName.HomeGame, StartGame);
        ListenerManager.Instance.AddListener(EventName.GameChallenge, ChallengeMode);
    }
    private void OnDisable()
    {
        if (ListenerManager.Instance != null)
        {
            ListenerManager.Instance.RemoveListener(EventName.StartNextLevel, StartGame);
            ListenerManager.Instance.RemoveListener(EventName.HomeGame, StartGame);
            ListenerManager.Instance.RemoveListener(EventName.GameChallenge, ChallengeMode);

        }
    }

    void Update()
    {
        if (OpponentManager.Instance.EnemyTargets.Count == 0 && !isGameOver)
        {
            isGameOver = true;
            if(currentLevelIndex!=10)
                gameModel.SaveLevels(gameModel.CheckLevel(currentLevelIndex));
            StartCoroutine(TimeToPopup(true));
      
        }
        if(OpponentManager.Instance.PlayerAndAllyTargets.Count == 0 && !isGameOver)
        {
            isGameOver = true;
            StartCoroutine(TimeToPopup(false));
        }
    }
    public void SpawnObject(float spaceX, float spaceZ, LevelData levelData)
    {


        LevelData level = levelData;
        int totalCountAlly = level.allyCount;
        int totalCountEnemy = level.enemyCount;
        int maxSizeCountRow = level.maxSizeCountRow;

        PlayerInfo playerInfo = level.playerInfo;
        EnemyInfo enemyInfo = level.enemyInfo;
        AllyInfo allyInfo = level.allyInfo;




        int arrowCountAlly = Mathf.CeilToInt(totalCountAlly / (float)maxSizeCountRow);
        int arrowCountEnemy = Mathf.CeilToInt(totalCountEnemy / (float)maxSizeCountRow);
         SpawnTeamPlayer(spaceX, spaceZ, totalCountAlly, maxSizeCountRow, playerInfo, allyInfo, arrowCountAlly, level);


        arrowCountEnemy = SpawnEnemy(spaceX, spaceZ, totalCountEnemy, maxSizeCountRow, enemyInfo, arrowCountEnemy);

        isGameOver = false;

    }

    private void SpawnTeamPlayer(float spaceX, float spaceZ, int totalCountAlly, int maxSizeCountRow, PlayerInfo playerInfo, AllyInfo allyInfo, int arrowCountAlly, LevelData level)
    {
        int allyRowCount = 0;
        allyRowCount = maxSizeCountRow;
        if (totalCountAlly < maxSizeCountRow)
        {
            allyRowCount = totalCountAlly;
            arrowCountAlly = 1;
        }


        int indexXPlayer = Mathf.FloorToInt(allyRowCount / 2);
        float statrtXPlayer = -(allyRowCount - 1) / 2 + indexXPlayer * spaceX + startX;

        Player player = ObjectPool.Instance.GetObject<Player>(CONST.POOL_PLAYER,spawnParentPoint);
        player.ResetStatEntity();
        player.transform.position = new Vector3(statrtXPlayer, playerPrefabs.transform.position.y, -spaceZ);
        player.transform.rotation = playerPrefabs.transform.rotation;

        //GameObject player = Instantiate(playerPrefabs, new Vector3(statrtXPlayer, playerPrefabs.transform.position.y, -spaceZ), Quaternion.identity);
        //player.transform.SetParent(spawnParentPoint);
        
        //Player player1 = player.GetComponent<Player>();
        player.GetComponent<Player>().SetUpInfo(playerInfo.player_health_bonus, playerInfo.player_damage_bonus, playerInfo.player_speed_bonus, playerInfo.cooldown_attack_bonus);

        string levelName = "Level " + (currentLevelIndex + 1).ToString();
        if(currentLevelIndex == 10)
        {
            levelName = "50-Man Brawl";
        }
        GameView.Instance.UpdateGameInfo(player.maxHealth, player.currentHealth,  levelName, level.mode );
        virtualCamera.Follow = player.transform;
        virtualCamera.LookAt = player.transform;
        int tempRowCount = totalCountAlly;
        for (int i = 0; i < arrowCountAlly; i++)
        {
            for (int j = 0; j < allyRowCount; j++)
            {
                float statrtX = startX + -(allyRowCount - 1) / 2;
                float posX = statrtX + j * spaceX;
                float posZ = -spaceZTemp * (i + 1) - spaceZ;

                Ally ally = ObjectPool.Instance.GetObject<Ally>(CONST.POOL_ALLY, spawnParentPoint);
                ally.ResetStatEntity();
                ally.transform.position = new Vector3(posX, allyPrefabs.transform.position.y, posZ);
                ally.transform.rotation = allyPrefabs.transform.rotation;
                ally.GetComponent<Ally>().
                    SetUpInfo(allyInfo.ally_health_bonus, allyInfo.ally_damage_bonus, allyInfo.ally_speed_bonus, allyInfo.cooldown_attack_bonus);


                HealthBar healthBar = ObjectPool.Instance.GetObject<HealthBar>(CONST.POOL_HEALTHBAR, canvas.transform);
                
                healthBars.Add(healthBar);

                healthBar.SetColorBar(false);
                healthBar.SetupHealthBar(ally.transform, 1, 1);

                ally.GetComponent<Entity>().SetHealthBar(healthBar);
                healthBar.gameObject.SetActive(false);

            }
            tempRowCount = tempRowCount - allyRowCount;
            if (tempRowCount <= 0)
            {
                break;
            }
            if(allyRowCount > tempRowCount)
            {
                allyRowCount = tempRowCount;
            }

        }

    }

    private int SpawnEnemy(float spaceX, float spaceZ, int totalCountEnemy, int maxSizeCountRow, EnemyInfo enemyInfo, int arrowCountEnemy)
    {
        int enemyRowCount = 0;
        enemyRowCount = maxSizeCountRow;
        if (totalCountEnemy < maxSizeCountRow)
        {
            enemyRowCount = totalCountEnemy;
            arrowCountEnemy = 1;
        }
        int tempRowCount = totalCountEnemy;

        for (int i = 0; i < arrowCountEnemy; i++)
        {
            for (int j = 0; j < enemyRowCount; j++)
            {

                float statrtX = startX + -(enemyRowCount - 1) / 2;
                float posX = statrtX + j * spaceX;
                float posZ = -spaceZTemp * (i + 1) - spaceZ;


                Enemy enemy = ObjectPool.Instance.GetObject<Enemy>(CONST.POOL_ENEMY, spawnParentPoint);
                enemy.ResetStatEntity();
                enemy.transform.position = new Vector3(posX, enemyPrefabs.transform.position.y, -posZ);
                enemy.transform.rotation = enemyPrefabs.transform.rotation;
                enemy.GetComponent<Enemy>().SetUpInfo(enemyInfo.enemy_health_bonus, enemyInfo.enemy_damage_bonus, enemyInfo.enemy_speed_bonus, enemyInfo.cooldown_attack_bonus);


                HealthBar healthBar = ObjectPool.Instance.GetObject<HealthBar>(CONST.POOL_HEALTHBAR, canvas.transform);
          

                healthBars.Add(healthBar);

                healthBar.SetColorBar(true);
                healthBar.SetupHealthBar(enemy.transform, 1, 1);

                enemy.GetComponent<Entity>().SetHealthBar(healthBar);
                healthBar.gameObject.SetActive(false);

                //SetHealthBar(enemy.GetComponent<Enemy>(), true);
            }
            tempRowCount = tempRowCount - enemyRowCount;
            if (tempRowCount <= 0)
            {
                break;
            }
            if (enemyRowCount > tempRowCount)
            {
                enemyRowCount = tempRowCount;
            }
        }

        return arrowCountEnemy;
    }

    private void SetHealthBar(Entity enemy, bool isEnemy)
    {
        t++;
        Debug.Log("SetHealthBar called for " + enemy.name + " with ID: " + t);
        //GameObject healthBar = Instantiate(this.healthBar);
        //HealthBar healthBar1 = healthBar.GetComponent<HealthBar>();
        //healthBar.transform.SetParent(canvas.transform);
        HealthBar healthBar = ObjectPool.Instance.GetObject<HealthBar>(CONST.POOL_HEALTHBAR, canvas.transform);

        if(healthBar  == null)
        {
            Debug.LogError("Failed to get HealthBar from ObjectPool. Check if the prefab is registered correctly.");
            return;
        }

        healthBars.Add(healthBar);

        healthBar.SetColorBar(isEnemy);
        healthBar.SetupHealthBar(enemy.transform, 1, 1);

        enemy.GetComponent<Entity>().SetHealthBar(healthBar);
        healthBar.gameObject.SetActive(false);

    }




    private IEnumerator StartCountdown()
    {
        gameStarted = false;


        for (int i = 3; i > 0; i--)
        {
            countdownText.text = i.ToString();

            countdownText.transform.localScale = Vector3.one * 2;
            countdownText.DOFade(1, 0);

            countdownText.transform.DOScale(1, 0.5f).SetEase(Ease.OutBack);
            countdownText.DOFade(0, 0.5f).SetDelay(0.5f);

            yield return new WaitForSeconds(1f);
        }

        countdownText.text = "GO!";
        countdownText.DOFade(1, 0);
        countdownText.transform.localScale = Vector3.one * 2;
        countdownText.transform.DOScale(1, 0.5f).SetEase(Ease.OutBack);
        countdownText.DOFade(0, 0.5f).SetDelay(0.5f);

        yield return new WaitForSeconds(1f);


        gameStarted = true;
    }






    public bool IsGameplayActive()
    {
        return gameStarted;
    }


    IEnumerator TimeToPopup(bool isWin)
    {
        yield return new WaitForSeconds(1.5f);
        if (isWin)
            ListenerManager.Instance.TriggerEvent<bool>(EventName.GameOver, isWin);
        else
            ListenerManager.Instance.TriggerEvent<bool>(EventName.GameOver, isWin);
        yield return new WaitForSeconds(0.5f);
    }

    public  void ResetLevel()
    {
        foreach (Transform teamPlayer in OpponentManager.Instance.PlayerAndAllyTargets)
        {
            if (teamPlayer.GetComponent<Player>()!=null)
            {
                ObjectPool.Instance.ReturnObject(CONST.POOL_PLAYER, teamPlayer.gameObject);
            }
            else
            {
                ObjectPool.Instance.ReturnObject(CONST.POOL_ALLY, teamPlayer.gameObject);

            }
        }
        foreach (Transform enemy in OpponentManager.Instance.EnemyTargets)
        {
            ObjectPool.Instance.ReturnObject(CONST.POOL_ENEMY, enemy.gameObject);
        }
        foreach (HealthBar healthBar in healthBars)
        {
            ObjectPool.Instance.ReturnObject(CONST.POOL_HEALTHBAR, healthBar.gameObject);
        }
        healthBars.Clear();
        OpponentManager.Instance.playerAndAllyTargets.Clear();
        OpponentManager.Instance.enemyTargets.Clear();
    }
}

