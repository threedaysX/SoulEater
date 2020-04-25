using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Endowment : MonoBehaviour
{
    //isOccupy[y][x]
    //public List<List<bool>> isOccupy = new List<List<bool>>();
    public bool[][] isOccupy = new bool[16][];
    void Start() {
        for (int i = 0; i < 8; i++) isOccupy[i] = new bool[17 + 2 * i];
        for (int i = 8; i < 16; i++) isOccupy[i] = new bool[31 + 16 - 2 * i];

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; i < 17+2*i; j++) isOccupy[i][j] = false;
        }
        for (int i = 8; i < 16; i++)
        {
            for (int j = 0; i < 31 + 16 - 2 * i; j++) isOccupy[i][j] = false;
        }
        /*for (int i = 0; i <= 16; i++) isOccupy.Add(new List<bool>());
        for (int i = 1; i <= 8; i++)
        {
            for (int j = 0; j <= 17-2+2*i; j++) isOccupy[i].Add(false);
        }
        for (int i = 9; i <= 16; i++)
        {
            for (int j = 0; j <= 31 + 18 - 2 * i; j++) isOccupy[i].Add(false);
        }
        isOccupy[0][1] = true;*/
    }
}
