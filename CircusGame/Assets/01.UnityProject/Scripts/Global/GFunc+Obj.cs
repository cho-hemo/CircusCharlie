using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public static partial class GFunc
{
    //! ??? ????????? ??? ????????? ?????? ?????? ???
    public static GameObject FindChildObj(
        this GameObject targetObj_, string objName_)
    {
        GameObject searchResult = default;
        GameObject searchTarget = default;
        for (int i=0; i< targetObj_.transform.childCount; i++)
        {
            searchTarget = targetObj_.transform.GetChild(i).gameObject;
            if (searchTarget.name.Equals(objName_))
            {
                searchResult = searchTarget;
                return searchResult;
            }
            else
            {
                searchResult = FindChildObj(searchTarget, objName_);

                if (searchResult == null || searchResult == default) { /* Pass */ }
                else
                {
                    return searchResult;
                }
            }
        }       // loop

        // // ??????
        // if(searchResult == null || searchResult == default) { /* Pass */ }
        // else 
        // {
        // GFunc.Log("찾은 오브젝트 이름 : "+searchResult.name);
        // return searchResult;
        // }

        return searchResult;
    }       // FindChildObj()


    //! ???? ??? ????????? ?????? ?????? ???
    public static GameObject GetRootObj(string objName_)
    {
        Scene activeScene_ = GetActiveScene();
        GameObject[] rootObjs_ = activeScene_.GetRootGameObjects();

        GameObject targetObj_ = default;
        foreach(GameObject rootObj in rootObjs_)
        {
            if(rootObj.name.Equals(objName_))
            {
                targetObj_ = rootObj;
                return targetObj_;
            }
            else { continue; }
        }       // loop

        return targetObj_;
    }       // GetRootObj()

    //! RectTransform ???? sizeDelta?? ???? ??????? ???
    public static Vector2 GetRectSizeDelta(this GameObject obj_)
    {
        return obj_.GetComponentMust<RectTransform>().sizeDelta;
    }       // GetRectSizeDelta()

    //! ???? ???? ??? ??? ???? ?????? ???
    public static Scene GetActiveScene()
    {
        Scene activeScene_ = SceneManager.GetActiveScene();
        return activeScene_;
    }       // GetActiveScene()

    //! ????????? ???? ???????? ??????? ???
    public static void SetLocalPos(this GameObject obj_, 
        float x, float y, float z)
    {
        obj_.transform.localPosition = new Vector3(x, y, z);
    }       // SetLocalPos()

    //! ????????? ???? ???????? ??????? ???
    public static void AddLocalPos(this GameObject obj_, 
        float x, float y, float z)
    {
        obj_.transform.localPosition = 
            obj_.transform.localPosition + new Vector3(x, y, z);
    }       // AddLocalPos()

    //! ????????? ?????? ????????? ??????? ???
    public static void Translate(this Transform transform_, Vector2 moveVector)
    {
        transform_.Translate(moveVector.x, moveVector.y, 0f);
    }       // Translate()

    //! ??????? ???????? ???
    public static T GetComponentMust<T>(this GameObject obj)
    {
        T component_ = obj.GetComponent<T>();

        GFunc.Assert(component_.IsValid<T>() != false, 
            string.Format("{0}???? {1}??(??) ??? ?? ???????.",
            obj.name, component_.GetType().Name));

        return component_;
    }       // GetComponentMust()

    //! RectTranstform?? ???? ??????? ???
    public static RectTransform GetRect(this GameObject obj_)
    {
        return obj_.GetComponentMust<RectTransform>();
    }       // GetRect()

    //! ????????? ??�� ???????? ??????? ???
    public static void AddAnchoredPos(this GameObject obj_,Vector2 position2D)
    {
        obj_.GetRect().anchoredPosition += position2D;
    }           // AddAnchoredPos

    //! ???��? ????????? ????? ????????? ??????? ???
    public static T CreateObj<T>(string objName) where T : Component
    {
        GameObject newObj = new GameObject(objName);
        return newObj.AddComponent<T>();
    }
}
