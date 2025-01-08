using System.IO;
using UnityEditor;
using UnityEditor.Android;
using UnityEngine;

public class AndroidPostBuildProcessor : IPostGenerateGradleAndroidProject
{

    public int callbackOrder { get { return 999; } }

    void IPostGenerateGradleAndroidProject.OnPostGenerateGradleAndroidProject(string path)
    {
       /* Debug.Log("Bulid path : " + path);
        //string gradlePropertiesFile = path + "/gradle/wrapper/gradle-wrapper.properties";
        string gradlePropertiesFile = path + "gradle.properties";

        if (File.Exists(gradlePropertiesFile))
        {
            File.Delete(gradlePropertiesFile);
        }
        StreamWriter writer = File.CreateText(gradlePropertiesFile);
        writer.WriteLine("org.gradle.jvmargs=-Xmx2048m");
        writer.WriteLine("org.gradle.parallel=true");
        writer.WriteLine("android.useAndroidX=true");
        writer.WriteLine("android.enableJetifier=true");
        //writer.WriteLine("distributionBase = GRADLE_USER_HOME");
        //writer.WriteLine("distributionPath=wrapper/dists");
        writer.WriteLine("distributionUrl=http\\://services.gradle.org/distributions/gradle-5.6.4-all.zip");
        writer.Flush();
        writer.Close();
       */
    }
}