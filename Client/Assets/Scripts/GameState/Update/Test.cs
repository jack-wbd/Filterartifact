using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Linq;
using Filterartifact;
using System.Diagnostics;

public class Test : MonoBehaviour
{
    #region Create (从零创建)
    public static string CreateFromZero()
    {
        StringBuilder jsonText = new StringBuilder();
        StringWriter sw = new StringWriter(jsonText);

        using (JsonWriter writer = new JsonTextWriter(sw))
        {
            writer.Formatting = Formatting.None;
            writer.WriteStartObject();//{
            #region children
            writer.WritePropertyName("children");
            writer.WriteStartArray();//[

            writer.WriteStartObject();//{
            writer.WritePropertyName("children");
            writer.WriteStartArray();//[

            writer.WriteStartObject();//{
            writer.WritePropertyName("organizationCode");
            writer.WriteValue("grandson");
            writer.WritePropertyName("organizationName");
            writer.WriteValue("孙子节点");
            writer.WriteEndObject();//} 

            writer.WriteEndArray();//]

            writer.WritePropertyName("organizationCode");
            writer.WriteValue("son");
            writer.WritePropertyName("organizationName");
            writer.WriteValue("子节点");
            writer.WriteEndObject();//} 

            writer.WriteEndArray();//]
            #endregion
            writer.WritePropertyName("organizationCode");
            writer.WriteValue("root");
            writer.WritePropertyName("organizationName");
            writer.WriteValue("根节点");

            writer.WriteEndObject();//} 

        }
        return jsonText.ToString();
    }

    #endregion

    #region Delete ,Update (删除指定节点 + 在指定节点前后添加 + 修改节点)
    public static string OperateNode()
    {
        string jsonText = CreateFromZero();
        JToken jobject = JObject.Parse(jsonText);
        #region 新节点
        StringBuilder jsonTextNew = new StringBuilder();
        StringWriter sw = new StringWriter(jsonTextNew);

        using (JsonWriter writer = new JsonTextWriter(sw))
        {
            writer.Formatting = Formatting.None;
            writer.WriteStartObject();//{
            writer.WritePropertyName("organizationCode");
            writer.WriteValue("newson");
            writer.WritePropertyName("organizationName");
            writer.WriteValue("新节点");
            writer.WriteEndObject();//} 
        }
        #endregion
        UnityEngine.Debug.Log(jsonTextNew.ToString());
        JToken jobjectNew = JObject.Parse(jsonTextNew.ToString());

        string[] field = new string[] { "孙子节点" };
        List<JToken> removeNodes = new List<JToken>();
        GetRemoveNodes(jobject, field, ref removeNodes);
        foreach (JToken node in removeNodes)
        {
            JToken nodeObj = node.Parent;
            //nodeObj.Remove();  //删除此节点

            //nodeObj.AddAfterSelf(jobjectNew); //在此节点后添加 json 对象
            //nodeObj.AddBeforeSelf(jobjectNew);//在此节点前添加 json 对象
            nodeObj.Replace(jobjectNew);  //修改此节点, 通过替换实现

        }


        return jobject.ToString();
    }
    private static void GetRemoveNodes(JToken token, string[] fields, ref List<JToken> removeNodes)
    {
        JContainer container = token as JContainer;
        if (container == null) return;

        foreach (JToken el in container.Children())
        {
            JProperty p = el as JProperty;
            if (p != null && fields.Contains(p.Value.ToString()))
            {
                removeNodes.Add(el);
            }
            GetRemoveNodes(el, fields, ref removeNodes);
        }

    }
    #endregion

    private void Start()
    {

        //string jsonpath = Application.persistentDataPath + "/test.json";
        //writeJsonFile(jsonpath, CreateFromZero());
        //writeJsonFile(jsonpath, OperateNode());
        Stopwatch sw = new Stopwatch();
        sw.Start();
        Test1();
        sw.Stop();
        UnityEngine.Debug.LogError(string.Format("total: {0} ms", sw.ElapsedMilliseconds));
    }
    //将序列化的json字符串内容写入Json文件，并且保存
    void writeJsonFile(string path, string jsonConents)
    {
        using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
        {
            using (StreamWriter sw = new StreamWriter(fs, Encoding.UTF8))
            {
                sw.WriteLine(jsonConents);
                sw.Close();
            }
        }
    }
    void Test1()
    {
        string jsonpath = Application.persistentDataPath + "/tcbhistory.json";
        using (StreamReader file = File.OpenText(jsonpath))
        {
            using (JsonTextReader reader = new JsonTextReader(file))
            {
                JContainer jContainer = JToken.ReadFrom(reader) as JContainer;
                if (jContainer == null) return;
                file.Close();
                var first = jContainer.First;
                StringBuilder jsonText = new StringBuilder();
                StringWriter sw = new StringWriter(jsonText);
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    writer.Formatting = Formatting.None;
                    writer.WriteStartObject();//{
                    #region children
                    writer.WritePropertyName("children");
                    writer.WriteStartArray();//[

                    writer.WriteStartObject();//{
                    writer.WritePropertyName("children");
                    writer.WriteStartArray();//[

                    writer.WriteStartObject();//{
                    writer.WritePropertyName("organizationCode");
                    writer.WriteValue("grandson");
                    writer.WritePropertyName("organizationName");
                    writer.WriteValue("孙子节点");
                    writer.WriteEndObject();//} 

                    writer.WriteEndArray();//]

                    writer.WritePropertyName("organizationCode");
                    writer.WriteValue("son");
                    writer.WritePropertyName("organizationName");
                    writer.WriteValue("子节点");
                    writer.WriteEndObject();//} 

                    writer.WriteEndArray();//]
                    #endregion
                    writer.WritePropertyName("organizationCode");
                    writer.WriteValue("root");
                    writer.WritePropertyName("organizationName");
                    writer.WriteValue("根节点");
                    writer.WriteEndObject();//} 
                }
                JToken jobject = JObject.Parse(jsonText.ToString());
                first.AddBeforeSelf(jobject);
                writeJsonFile(jsonpath, jContainer.First.ToString());
            }
        }

    }



    #region Retrieve (查询)
    //官网例子比较好:
    //http://www.newtonsoft.com/json/help/html/QueryingLINQtoJSON.htm
    #endregion
}
