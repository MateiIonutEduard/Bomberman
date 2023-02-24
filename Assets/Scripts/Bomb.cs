﻿using UnityEngine;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Collections.Generic;

public class Bomb : MonoBehaviour
{
    public AudioClip explosionSound;
    public GameObject explosionPrefab;

    public LayerMask levelMask;
    private bool exploded = false;
    public int PlayerId;

    public int explode_size = 2;
    public PlayerUnit player;
    private AudioSource audioSource;

    public void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = MyPlayerPrefs.GetVolume();
    }

    public void Start()
    {
        Invoke("Explode", 3f);
    }

    private void Explode()
    {
        float volume = MyPlayerPrefs.GetVolume();
        AudioSource.PlayClipAtPoint(explosionSound, transform.position, volume);
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        StartCoroutine(CreateExplosions(Vector3.forward));
        StartCoroutine(CreateExplosions(Vector3.right));

        StartCoroutine(CreateExplosions(Vector3.back));
        StartCoroutine(CreateExplosions(Vector3.left));

        exploded = true;
        GetComponent<MeshRenderer>().enabled = false;

        transform.Find("Collider").gameObject.SetActive(false);
        Destroy(gameObject, .3f);
        player.bombs++;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (!exploded && other.CompareTag("Explosion"))
        {
            CancelInvoke("Explode");
            Explode();
        }
    }

    private IEnumerator CreateExplosions(Vector3 direction)
    {
        var list = new List<Vector3>();

        for (int i = 1; i < explode_size; i++)
        {
            RaycastHit hit;
            Physics.Raycast(transform.position + new Vector3(0, .5f, 0), direction, out hit, i, levelMask);

            if (!hit.collider) list.Add(transform.position + (i * direction));
            else
            {
                if (hit.collider.CompareTag("Breakable"))
                {
                    hit.collider.GetComponent<Brick>().Collide = true;
                    hit.collider.gameObject.SetActive(false);
                }
                else if (hit.collider.CompareTag("Player") || hit.collider.CompareTag("PowerUp") || hit.collider.CompareTag("Bomb"))
                {
                    list.Add(transform.position + (i * direction));
                    continue;
                }

                break;
            }
        }

        foreach (Vector3 position in list)
        {
            var obj = Instantiate(explosionPrefab, position, explosionPrefab.transform.rotation);
            var script = obj.GetComponent<DestroySelf>();
            script.EnemyId = PlayerId;
            yield return new WaitForSeconds(.05f);
        }
    }
}
