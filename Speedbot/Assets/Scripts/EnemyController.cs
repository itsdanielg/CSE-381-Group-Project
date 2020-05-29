using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    public class EnemyBullet {

        public GameObject enemyBullet;
        public Vector3 playerPos;
        public Vector3 originalPos;

        public float maxDistance;
        public bool dead;

        public EnemyBullet(GameObject enemyBullet, Vector3 playerPos) {
            this.enemyBullet = enemyBullet;
            this.playerPos = playerPos;
            this.dead = false;
            originalPos = enemyBullet.transform.position;
            float maxDistance = Vector3.Distance(originalPos, playerPos);
        }

        public bool reachedDistance() {
            Vector3 currentPos = enemyBullet.transform.position;
            float currentDistance = Vector3.Distance(currentPos, playerPos);
            if (currentDistance <= 0) {
                return true;
            }
            return false;
        }

    }

    public GameObject player;
    public GameObject enemyBullet;
    public AudioSource bulletSound;

    public float maxDistanceDifference;
    public float fireRatePerSecond;
    public float bulletSpeed;
    public float knockbackDistance;         // CURRENTLY UNUSED
    
    private Vector3 originalLook;
    private List<EnemyBullet> bullets;

    private float timeSinceShoot;

    void Start() {
        bullets = new List<EnemyBullet>();
        originalLook = transform.forward;
        enemyBullet.SetActive(false);
    }

    void Update() {
        Vector3 playerPos = player.transform.position;
        playerPos.y -= 1.5f;
        float distanceDiff = Vector3.Distance(transform.position, playerPos);
        // CHECK IF PLAYER IS WITHIN ENEMY RADIUS
        if (distanceDiff < maxDistanceDifference) {
            transform.LookAt(playerPos);
            playerPos.y += 1.5f;
            shootBullet(playerPos);
        }
        else {
            transform.LookAt(originalLook);
            foreach (EnemyBullet bullet in bullets) {
                Destroy(bullet.enemyBullet);
            }
            bullets.Clear();
        }
    }

    void shootBullet(Vector3 playerPos) {
        if (timeSinceShoot >= 1.0/fireRatePerSecond) {
            bulletSound.volume = PlayerPrefs.GetFloat("Sound");
            bulletSound.Play();
            timeSinceShoot = 0;
            GameObject bulletClone = Instantiate(enemyBullet, enemyBullet.transform.position, enemyBullet.transform.rotation);
            bulletClone.SetActive(true);
            EnemyBullet bullet = new EnemyBullet(bulletClone, playerPos);
            bullets.Add(bullet);
        }
        else {
            timeSinceShoot += Time.deltaTime;
        }
        foreach (EnemyBullet bullet in bullets) {
            float step = bulletSpeed * Time.deltaTime;
            Vector3 pos = bullet.enemyBullet.transform.position;
            bullet.enemyBullet.transform.position = Vector3.MoveTowards(pos, bullet.playerPos, step);
            if (bullet.reachedDistance()) {
                bullet.dead = true;
                Destroy(bullet.enemyBullet);
            }
        }
        bullets.RemoveAll(bullet => bullet.dead);
    }

}
