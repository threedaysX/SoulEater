using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//右上梯形
/*             A
 *         --------
 *        / \     /
 *   D   /   \   /  
 *      /     \ /    
 *     ---------   B  
 *      \     /      
 *   C    \  /      
 *          \      
 *                 
 */
//  上     右上      下
//  左     上右     左右
public class c_URTrapezoid : chip
{
    //有哪些邊，不同邊被觸發後又會發生甚麼事

    //A邊被觸發
    bool touchE_A = false;
    bool touchE_B = false;
    bool touchE_C = false;
    bool touchE_D = false;

   
}
