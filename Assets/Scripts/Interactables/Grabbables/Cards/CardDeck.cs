﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardDeck : MonoBehaviour
{
    #region globals
    public static CardDeck Instance = null;

    public Texture[] cardTextures;
    public List<Transform> cardSlots;

    private AudioSource cardDeckAudioEmitter;
    #endregion

    void Awake()
    {
        Shuffle();

        cardDeckAudioEmitter = this.GetComponent<AudioSource>();

        if (Instance == null)
            Instance = this;

        else if (Instance != this)
            Destroy(gameObject);
    }

    private IEnumerator DealCards()
    {
        //go through 10 cards and deal them
        for (int x = 0; x < 10; x++)
        {
            //get top card from deck
            Transform card = transform.GetChild(transform.childCount - 1);

            //remove card from deck
            card.parent = null;

            //orient card correctly
            card.eulerAngles = new Vector3(-90, 0, 0);

            //set holding orientation
            card.GetComponent<Card>().data.holdingOrientation = new Vector3(-90, cardSlots[x].rotation.eulerAngles.y, 0);

            //play audio effect
            cardDeckAudioEmitter.Play();

            //lerp cards to pos
            StartCoroutine(card.GetComponent<Card>().MoveCardTo(cardSlots[x].position, .4f));
            StartCoroutine(card.GetComponent<Card>().RotateCard(
                Quaternion.Euler(new Vector3(-90, cardSlots[x].rotation.eulerAngles.y, 0))
                , .25f));

            yield return new WaitForSeconds(.5f);
        }

    }

    private void Shuffle()
    {
        //go through all children of deck
        foreach (Transform card in transform)
        {
            //get renderer of card
            Renderer renderer = card.GetComponent<Renderer>();

            int i = 0;
            int[] chosen = new int[40];
            //populate chosen
            for (int j = 0; j < 40; j++)
            {
                chosen[j] = 60;
            }

            bool assignedTex = false;
            //while unique card tex has not been assigned
            while (!assignedTex)
            {
                //chooose randomly between 0 and 39
                int rand = Random.Range(0, 40);
                //if chosen doesnt contain randomly chosen number
                if (!chosen.Contains(rand))
                {
                    //set texture to a card not previously drawn
                    assignedTex = true;
                    renderer.material.SetTexture("_MainTex", cardTextures[rand]);
                    //add already chosen textures to chosen array to keep track
                    chosen[i] = rand;
                    i++;
                }
            }
        }
    }

    private void OnMouseDown()
    {
        StartCoroutine(DealCards());
    }
}

