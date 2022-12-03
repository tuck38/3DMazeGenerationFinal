using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeCell : MonoBehaviour
{
    int set; //which set the cell belongs to

    [SerializeField] GameObject northWall, eastWall, southWall, westWall;


    public void SetNorth(bool value) { northWall.SetActive(value); }
    public void SetSouth(bool value) { southWall.SetActive(value); }
    public void SetEast(bool value) { eastWall.SetActive(value); }
    public void SetWest(bool value) { westWall.SetActive(value); }

    public bool GetNorth() { return northWall.activeSelf; }
    public bool GetSouth() { return southWall.activeSelf; }
    public bool GetEast() { return eastWall.activeSelf; }
    public bool GetWest() { return westWall.activeSelf; }






}