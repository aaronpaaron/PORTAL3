using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Notes : MonoBehaviour
{
    [SerializeField] private Image _noteImage;

    public GameObject MessagePanel;
    public bool Action = false;

    public void Start()
    {
        MessagePanel.SetActive(false);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (Action == true)
            {
                MessagePanel.SetActive(false);
                Action = false;
                _noteImage.enabled = true;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            MessagePanel.SetActive(true);
            Action = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            MessagePanel.SetActive(false);
            Action = false;
            _noteImage.enabled = false;
        }
    }
}
