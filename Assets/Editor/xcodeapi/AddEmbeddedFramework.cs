using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.Collections;
using Pubnative.UnityEditor.iOS.Xcode;
using System.IO;
using System.Text.RegularExpressions;

public static class AddEmbeddedFramework
{
	[PostProcessBuild]
	    public static void OnPostProcessBuild(BuildTarget buildTarget, string path)
	    {
	        if (buildTarget == BuildTarget.iOS)
	        {
	            string projectPath = path + "/Unity-iPhone.xcodeproj/project.pbxproj";
	                 PBXProject pbxProject = new PBXProject();
	            pbxProject.ReadFromFile(projectPath);
	                 string target = pbxProject.TargetGuidByName("Unity-iPhone");
	            // pbxProject.SetBuildProperty(target, "ALWAYS_EMBED_SWIFT_STANDARD_LIBRARIES", "YES");
	            pbxProject.SetBuildProperty(target, "LD_RUNPATH_SEARCH_PATHS", "$(inherited) @executable_path/Frameworks");

	            AddDynamicFrameworks(ref pbxProject, target);

	            pbxProject.WriteToFile(projectPath);

	            string contents = File.ReadAllText(projectPath);

	            // Enable CodeSignOnCopy for the framework
	            contents = Regex.Replace(contents,
	                "(?<=Embed Frameworks)(?:.*)(\\/\\* Pubnative\\.framework \\*\\/)(?=; };)",
	                m => m.Value.Replace("/* Pubnative.framework */",
	                    "/* Pubnative.framework */; settings = {ATTRIBUTES = (CodeSignOnCopy, ); }"));

	            File.WriteAllText(projectPath, contents);
	        }
	    }

	static void AddDynamicFrameworks(ref PBXProject project, string target)
	    {
	        const string defaultLocationInProj = "Frameworks/Plugins/iOS";
	        const string coreFrameworkName = "Pubnative.framework";

	        string relativeCoreFrameworkPath = Path.Combine(defaultLocationInProj, coreFrameworkName);
	        project.AddDynamicFrameworkToProject(target, relativeCoreFrameworkPath);

	        Debug.Log("Dynamic Frameworks added to Embedded binaries.");
	    }

}
