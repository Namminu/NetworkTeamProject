using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataManager
{
    ///// <summary>
    ///// 클래스 저장
    ///// </summary>
    ///// <typeparam name="T">저장할 클래스 형식</typeparam>
    ///// <param name="fileName">저장될 파일 이름</param>
    ///// <param name="saveData">저장할 데이터</param>
    //public void SaveFile<T>(string userName, T saveData) where T : JsonForms, new()
    //{

    //}


    ///// <summary>
    ///// 클래스 불러오기
    ///// </summary>
    ///// <typeparam name="T">불러올 클래스 형식</typeparam>
    ///// <param name="userName">불러올 파일 이름</param>
    ///// <returns>불러온 데이터</returns>
    //public T LoadFile<T>(string userName, bool isJsonFile = true) where T : JsonForms, new()
    //{
    //    string path;

    //    T saveData = null;
    //    if (isJsonFile)
    //    {
    //        path = $"{Application.dataPath}/Resources/Data/{userName}.txt";

    //        StreamReader sr = null;

    //        try
    //        {
    //            sr = new StreamReader(path);
    //        }
    //        catch
    //        {
    //            SaveFile<T>(userName, new T());
    //            sr = new StreamReader(path);
    //        }

    //        string jsonData = sr.ReadToEnd();
    //        saveData = JsonUtility.FromJson<T>(jsonData);

    //        sr.Close();
    //    }
    //    else
    //    {
    //        // TODO : 서버에서 불러오기
    //        saveData = null;
    //    }

    //    return saveData;
    //}
}