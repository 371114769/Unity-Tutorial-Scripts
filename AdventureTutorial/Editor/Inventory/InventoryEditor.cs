using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//这个开关好像没有什么影响
[CustomEditor(typeof(Inventory))]
public class InventoryEditor : Editor {

    //类似于class的serializable，可以把公共元素都显示到inspector中
    //SerializedProperty 也就是可以显示到inspector的property
    private SerializedProperty itemImagesProperty;
    private SerializedProperty itemsProperty;

    //主要就是用来搞折叠Foldout的展开与折叠
    private bool[] showItemSlots = new bool[Inventory.numItemSlots];

    //serializedObject.FindProperty只能通过变量名来寻找，所以这里设置好准确的变量
    private const string inventoryPropItemImagesName = "itemImages";
    private const string inventoryPropItemsName = "items";

    private void OnEnable()
    {
        //serializedObject 应该是直接这样子写了代码就好了
        itemImagesProperty = serializedObject.FindProperty(inventoryPropItemImagesName);
        itemsProperty = serializedObject.FindProperty(inventoryPropItemsName);
    }

    //所以这个func负责主界面
    public override void OnInspectorGUI()
    {
        //serializedObject开头
        serializedObject.Update();

        for (int i = 0; i < Inventory.numItemSlots; i++)
        {
            ItemSlotGUI(i);
        }

        //serializedObject结尾
        serializedObject.ApplyModifiedProperties();
    }

    //这个func则设置每个property排版
    private void ItemSlotGUI(int index)
    {
        //GUI.skin.box是把外观设置成盒子状
        //EditorGUILayout开头
        EditorGUILayout.BeginVertical(GUI.skin.box);
        //如果两个indent的设置都关掉的话，三角折叠箭头位置很差
        //如果只把++关了，所有元素都会往左边递增
        EditorGUI.indentLevel++;

        //这个禁用的话在slot那里就只有小框框，别的什么都没有
        //设置每个slot的外观文字、以及三角折叠箭头
        //Foldout就是折叠用的结构
        //按三角折叠箭头则设置true和false，用来切换展开与折叠
        showItemSlots[index] = EditorGUILayout.Foldout(showItemSlots[index], "Item slot" + index);

        //如果不加这个，在展开后就没有内容可以显示
        //上面的三角折叠为true(展开)的时候，设置显示内容
        if(showItemSlots[index])
        {
            //PropertyField创造一个区域给SerializedProperty
            //GetArrayElementAtIndex通过index来从list中获取相应的property
            EditorGUILayout.PropertyField(itemImagesProperty.GetArrayElementAtIndex(index));
            EditorGUILayout.PropertyField(itemsProperty.GetArrayElementAtIndex(index));
        }

        //如果只把--关了，所有元素会往右边缩紧
        EditorGUI.indentLevel--;
        //EditorGUILayout结尾
        EditorGUILayout.EndVertical();
    }

}
