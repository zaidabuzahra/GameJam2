using System.Collections;
using TMPro;
using UnityEngine;
using DG.Tweening;
using System.Xml.Schema;
using NUnit.Framework;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private int[] riggedDice;
    [SerializeField] private float stepTime;
    [SerializeField] private float stepDelay;
    [SerializeField] private float stepDistance;
    [SerializeField] private GameObject pressText;
    [SerializeField] private Animator dieAnimator;

    private readonly Vector3[] directions = new Vector3[] { Vector2.left, Vector2.up, Vector2.right, Vector2.down};

    public int _dicdCounter = 0;
    public int _directionCounter = 0;
    private bool _canThrow = true;

    void Update()
    {
        if (_canThrow && _dicdCounter < riggedDice.Length)
        {
            TakePlayerInput();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Corner"))
        {
            _directionCounter++;
            if (_directionCounter > 3)
            {
                _directionCounter = 0;
            }
        }
    }
    private void TakePlayerInput()
    { 
        pressText.SetActive(true);
        Debug.Log("can throw");
        if (Input.GetButtonDown("Jump"))
        {
            ThrowDice();
        }
    }
    private void ThrowDice()
    {
        //play die animation
        dieAnimator.SetInteger("die", riggedDice[_dicdCounter]);
        dieAnimator.SetTrigger("shoot");
        Debug.Log("Dice thrown");
        _canThrow = false;
        pressText.SetActive(false);
        StartCoroutine(Step());
    }
    IEnumerator Step()
    {
        yield return new WaitForSeconds(1.5f);
        for (int i = 0; i < riggedDice[_dicdCounter]; i++)
        { 
            transform.DOMove(gameObject.transform.position + (directions[_directionCounter] * stepDistance), stepTime);
            yield return new WaitForSeconds(stepDelay + stepTime);
        }
        _dicdCounter++;
        //play sad animation and update ui
        _canThrow = true;
    }
}
