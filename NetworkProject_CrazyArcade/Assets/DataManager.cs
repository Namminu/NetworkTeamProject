using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataManager
{
    ///// <summary>
    ///// Ŭ���� ����
    ///// </summary>
    ///// <typeparam name="T">������ Ŭ���� ����</typeparam>
    ///// <param name="fileName">����� ���� �̸�</param>
    ///// <param name="saveData">������ ������</param>
    //public void SaveFile<T>(string userName, T saveData) where T : JsonForms, new()
    //{

    //}


    ///// <summary>
    ///// Ŭ���� �ҷ�����
    ///// </summary>
    ///// <typeparam name="T">�ҷ��� Ŭ���� ����</typeparam>
    ///// <param name="userName">�ҷ��� ���� �̸�</param>
    ///// <returns>�ҷ��� ������</returns>
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
    //        // TODO : �������� �ҷ�����
    //        saveData = null;
    //    }

    //    return saveData;
    //}
}