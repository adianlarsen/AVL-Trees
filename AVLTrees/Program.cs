/*
Part 1:
1. A balanced tree is a tree data structure where the heights of the left and right subtrees are kept close to each other, typically within a height of 1. This makes searching more efficient and keeps chains as short as possible.

2. 
10
  \
   20
     \
      30
        \
         40
           \
            50
Height: 5
Number of Nodes: 5
Height if balanced: 3

3. Height of the left subtree minus the height of the right subtree in left minus right. Every node in an AVL has to have balance factor -1, 0, 1

4. With an AVL tree, we are able to visit one node per level of the tree, effectively making it much more efficient and O(logn). WIth the BST, you have to visit each node, so it it O(n) tine.
Node    Height    Balance Factor
10      0         0
20      1         1
30      2         1
40      0         0
50      3         0
70      2         -2
80      1         -1  
90      0         0

The tree fails at node 70, because we have -1-1 = -2, which is not -1, 0, 1.


Part 2: 
6. 
33: 33(0) - none
18: 33(+1) -> 18(0) - none
7:  33(+2) -> 18(+1) -> 7(0) - LL at 33
25: 18(-1) -> 33(+1) -> 25(0) - none
29: 18(-2) -> 33(+2) -> 25(-1) -> 29(0) - LR at 33

    33
    /
   18
   /
  7
  LL at 33
    18
    /\
   7  33
   
   18
   /\
  7  33
     /
    25
     \
      29
  Lr at 33

  Final Tree:
    18
    /\
   7  29
      /\
     25 33


7.
14: 14(0) - none
26: 14(-1) -> 26(0) - none
38: 14(-2) -> 26(-1) -> 38(0) - RR at 14
44: 26(-1) -> 38(-1) -> 44(0) - none
41: 26(-2) -> 38(-2) -> 44(+1) -> 41(0) - RL at 38 

    14
      \
       26
        \
         38
    RR at 14
    26
    /\
   14 38

    26
    /\
   14 38
       \
        44
        /
        41

    RL at 38

    Final Tree:
    26
    /\
   14 41
      / \
     38  44

8. 
42: 71(+2) -> 48(+2) -> 39(-1) -> 42(0) - LR at 48
       71
       / \
      48  90
     /
    39
    \
     42

LR at 48

Final Tree: 
       71
       / \
      42  90
      / \
     39 48
    

9. No rotations til this:
          52
          / \
         37  68
         / \
        29 44
           /
          41
          
LR at 52

        44
        / \
       37  52
      / \    \
     29 41    68
                \
                 75

RR at 52

        44
        / \
       37 68
      / \   / \
     29 41 52 75
         \  \
         61  80
             /
            78

RL at 75
Final Tree:
          44
         /   \
        37    68
       / \    / \
      29  41  52  78
              \   / \
              61 75 80

*/

using System;

public class Program
{
  static void Main()
  {
    AvlNode root = null;
    int[] numbers = { 52, 37, 68, 29, 44, 41, 75, 80, 61, 78 };

    foreach (int number in numbers)
    {
      Console.WriteLine("Insert " + number);
      root = Insert(root, number);
    }

    Console.Write("In order: ");
    PrintInOrder(root);
    Console.WriteLine();

    Console.WriteLine("Root value: " + root.Value);
    Console.WriteLine("Tree height: " + GetHeight(root));
    Console.WriteLine("Is balanced: " + IsBalanced(root));

    AvlNode secondRoot = null;

    for (int i = 1; i <= 15; i++)
    {
      Console.WriteLine("Insert " + i);
      secondRoot = Insert(secondRoot, i);
    }

    Console.WriteLine("Height after inserting 1 to 15: " + GetHeight(secondRoot));
  }
  static int GetHeight(AvlNode node)
  {
    if (node == null)
    {
      return 0;
    }

    return node.Height;
  }

  static int GetBalance(AvlNode node)
  {
    if (node == null)
    {
      return 0;
    }

    return GetHeight(node.Left) - GetHeight(node.Right);
  }

  static void UpdateHeight(AvlNode node)
  {
    if (node == null)
    {
      return;
    }

    int leftHeight = GetHeight(node.Left);
    int rightHeight = GetHeight(node.Right);

    if (leftHeight > rightHeight)
    {
      node.Height = leftHeight + 1;
    }
    else
    {
      node.Height = rightHeight + 1;
    }
  }

  static AvlNode RotateRight(AvlNode node)
  {
    AvlNode newTop = node.Left;
    node.Left = newTop.Right;
    newTop.Right = node;

    UpdateHeight(node);
    UpdateHeight(newTop);

    return newTop;
  }

  static AvlNode RotateLeft(AvlNode node)
  {
    AvlNode newTop = node.Right;
    node.Right = newTop.Left;
    newTop.Left = node;

    UpdateHeight(node);
    UpdateHeight(newTop);

    return newTop;
  }

  static AvlNode Insert(AvlNode node, int value)
  {
    if (node == null)
    {
      return new AvlNode(value);
    }

    if (value < node.Value)
    {
      node.Left = Insert(node.Left, value);
    }
    else if (value > node.Value)
    {
      node.Right = Insert(node.Right, value);
    }
    else
    {
      return node;
    }

    UpdateHeight(node);
    int balance = GetBalance(node);

    if (balance > 1)
    {
      if (value < node.Left.Value)
      {
        Console.WriteLine($"LL rotation at {node.Value}");
        return RotateRight(node);
      }

      Console.WriteLine($"LR rotation at {node.Value}");
      node.Left = RotateLeft(node.Left);
      return RotateRight(node);
    }

    if (balance < -1)
    {
      if (value > node.Right.Value)
      {
        Console.WriteLine($"RR rotation at {node.Value}");
        return RotateLeft(node);
      }

      Console.WriteLine($"RL rotation at {node.Value}");
      node.Right = RotateRight(node.Right);
      return RotateLeft(node);
    }

    return node;
  }

  static void PrintInOrder(AvlNode node)
  {
    if (node == null)
    {
      return;
    }

    PrintInOrder(node.Left);
    Console.Write(node.Value + " ");
    PrintInOrder(node.Right);
  }

  static bool IsBalanced(AvlNode root)
  {
    if (root == null)
    {
      return true;
    }

    if (!IsBalanced(root.Left) || !IsBalanced(root.Right))
    {
      return false;
    }

    UpdateHeight(root);
    int balance = GetBalance(root);

    if (balance < -1 || balance > 1)
    {
      return false;
    }

    return true;
  }
}

public class AvlNode
{
  public int Value;
  public AvlNode Left;
  public AvlNode Right;
  public int Height = 1;

  public AvlNode(int value)
  {
    Value = value;
  }
}