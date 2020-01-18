﻿using System.Linq;

using UnityEngine;
using UnityEngine.SceneManagement;

public class Group : MonoBehaviour
{
    private bool isLeftPressed = false;
    private bool isRightPressed = false;
    private bool isDownPressed = false;

    private int horizontalCounter = 0;

    private float fallSpeed = Constants.InitialFallSpeed;
    private float lastFallSpeed = Constants.InitialFallSpeed;
    private float lastFall = 0;
    
    public void Start()
    {
        if (!this.gameObject.IsValidGridPos())
        {
            Debug.Log("GAME OVER");
            Common.IsRunning = false;
            Common.IsGameStarted = false;
            SceneManager.LoadScene(3);
        }
    }

    // Update is called once per frame
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Common.IsRunning = false;
            FindObjectsOfType<GameObject>().ToList().ForEach(go => go.SetActive(false));
            SceneManager.LoadScene(0, LoadSceneMode.Additive);
        }

        if (!Common.IsRunning || !this.gameObject.IsValidGridPos())
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            this.isLeftPressed = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            this.isLeftPressed = false;
            this.horizontalCounter = 0;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            this.isRightPressed = true;
        }
        else if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            this.isRightPressed = false;
            this.horizontalCounter = 0;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            this.PressUp();
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            this.isDownPressed = true;
        }
        else if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            this.isDownPressed = false;
        }

        this.fallSpeed = this.lastFallSpeed = Constants.InitialFallSpeed + (Constants.IncreasePerLevel * Results.Level);
        if (Time.time - this.lastFall >= 1 / this.fallSpeed)
        {
            this.PressDown();
        }
    }

    public void FixedUpdate()
    {
        if (!Common.IsRunning)
        {
            return;
        }

        if (this.isLeftPressed && !this.isRightPressed)
        {
            this.horizontalCounter++;
            if (this.horizontalCounter % Constants.MaxHorizontalCount == 0)
            {
                this.PressLeft();
                this.horizontalCounter = 0;
            }
        }
        
        if (this.isRightPressed && !this.isLeftPressed)
        {
            this.horizontalCounter++;
            if (this.horizontalCounter % Constants.MaxHorizontalCount == 0)
            {
                this.PressRight();
                this.horizontalCounter = 0;
            }
        }
        
        if (this.isDownPressed)
        {
            this.lastFallSpeed = this.fallSpeed;
            this.fallSpeed = Constants.MaxFallSpeed;
            this.PressDown();
        }
        else
        {
            this.fallSpeed = this.lastFallSpeed;
        }
    }

    private void PressDown()
    {
        // Modify position
        this.transform.position += new Vector3(0, -1, 0);

        // See if valid
        if (this.gameObject.IsValidGridPos())
        {
            // It's valid. Update grid.
            this.gameObject.UpdateGrid();
        }
        else
        {
            var lastY = this.transform.localPosition.y;
            if (lastY > 0)
            {
                lastY = 0;
            }

            // It's not valid. revert.
            this.transform.position += new Vector3(0, 1, 0);
            Common.HitSound.Play();
            Results.Score += (int)(Constants.MaxAdditionalPoints - lastY);

            // Clear filled horizontal lines
            Playfield.DeleteFullRows();

            // Spawn next Group
            GameObject.FindObjectOfType<Spawner>().SpawnNext();

            // Disable script
            this.enabled = false;
        }

        this.lastFall = Time.time;
    }

    private void PressUp() => this.gameObject.RotateFigure();

    private void PressRight()
    {
        // Modify position
        this.transform.position += new Vector3(1, 0, 0);

        // See if valid
        if (this.gameObject.IsValidGridPos())
        {
            // It's valid. Update grid.
            this.gameObject.UpdateGrid();
        }
        else
        {
            // It's not valid. revert.
            this.transform.position += new Vector3(-1, 0, 0);
        }
    }

    private void PressLeft()
    {
        // Modify position
        this.transform.position += new Vector3(-1, 0, 0);

        // See if valid
        if (this.gameObject.IsValidGridPos())
        {
            // It's valid. Update grid.
            this.gameObject.UpdateGrid();
        }
        else
        {
            // It's not valid. revert.
            this.transform.position += new Vector3(1, 0, 0);
        }
    }
}