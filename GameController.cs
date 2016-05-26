using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Vuforia;

public class GameController : MonoBehaviour, ITrackableEventHandler
{
    private ArrayList enemies;
    private float lastTime = 0;
    private TrackableBehaviour mTrackableBehaviour;
    public bool imageTarget = false;
	public Text t_score, t_vida;
	public TextMesh t_mesh, t_mvida;

    public float timeToSpawn = 3.0f;
    public GameObject targetObject;
    public SpawnerCustom []spawners = new SpawnerCustom[5];

    public int numEnemies= 0;
    public int score = 0;
    public float live = 50;

	public Animation playerAnim;

    // Use this for initialization
    void Start()
    {
        lastTime = Time.time;
        enemies = new ArrayList();

        mTrackableBehaviour = GetComponent<TrackableBehaviour>();

        if (mTrackableBehaviour)
        {
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
        }
		t_mvida.text = "Vida : " + live;
    }

    // Update is called once per frame
    void Update()
    {
        if (imageTarget && (Time.time - lastTime > timeToSpawn) )
        {
            callSpawners();
            lastTime = Time.time;
        }
		t_mesh.text = "Puntos:"  + score;
    }

    void OnGUI()
    {
        GUILayout.Label("-");
		if(t_score)
			t_score.text = "Puntos:" + score;
    }

    void callSpawners()
    {
        foreach (SpawnerCustom spawn in spawners)
        {
            GameObject spawned = spawn.spawnObject();
            if (spawned != null)
            {
                ThiefBehavior th = spawned.GetComponent("ThiefBehavior") as ThiefBehavior;
				if (th != null)
					th.setTargetObject (targetObject);
				else {
					CiclopBehavior cic = spawned.GetComponent("CiclopBehavior") as CiclopBehavior;
					if (cic != null)
						cic.setTargetObject(targetObject);
				}
					
                enemies.Add(spawned);
                numEnemies++;
            }
        }
    }

    void incrementDificulty()
    { }

	public void Damage(float dam)
	{
		//if (dam > 1)
		if (live <= 15) {
			t_mvida.color = new Color (1, 0.2f, 0);
		}
		if(live <= 0)
			gameOver();
		else {
			live -= dam * Time.deltaTime;
			t_mvida.text = "Vida : " + (int)live;
			if (t_vida)
				t_vida.text = "Vida : " + (int)live;
		}
		if (playerAnim)
			//playerAnim.Play ("Damage");
			playerAnim.Play ("attack");
	}
	
    public void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus)
    {
        //throw new System.NotImplementedException();
        if (newStatus == TrackableBehaviour.Status.DETECTED || newStatus == TrackableBehaviour.Status.TRACKED)        
            OnTrackingFound();        
        else if (newStatus == TrackableBehaviour.Status.NOT_FOUND || newStatus == TrackableBehaviour.Status.UNKNOWN)
            OnTrackingLost();
            
    }

	public void playerAttack(){
		if (playerAnim)
			//playerAnim.Play ("Attack");
			playerAnim.Play ("attack");
	}

	public void deleteEnemy(object enemy)
	{
		enemies.Remove (enemy);
		if (enemies.Count < 3)
			resetSpawners ();
	}

    private void OnTrackingFound()
    {
        imageTarget = true;
    }

    private void OnTrackingLost()
    {
        imageTarget = false;
        resetEnemies();
        resetSpawners();
    }

    private void resetSpawners()
    {
        foreach (SpawnerCustom spawner in spawners)
        {
            spawner.reset();
        }
    }

    private void resetEnemies()
    {               
        foreach (GameObject obj in enemies)
        {
            if (obj != null)
            {
                //enemies.Remove(obj);
                GameObject.Destroy(obj);
                numEnemies--;
            }
        }
        enemies.Clear();
    }

	private void gameOver()
	{
		if (playerAnim)
			playerAnim.Play ("dead");
		Application.LoadLevel ("GameOver");
	}
}
