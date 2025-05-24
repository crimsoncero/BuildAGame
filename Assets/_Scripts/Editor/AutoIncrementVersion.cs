using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class AutoIncrementVersion : IPostprocessBuildWithReport
{
    
    public int callbackOrder { get; } = 0;
    
    
    public void OnPostprocessBuild(BuildReport report)
    {

        if (report.summary.result == BuildResult.Failed)
        {
            
        }
        else
        {
            string version = PlayerSettings.bundleVersion;

            if (!Regex.IsMatch(version, @"^\d+\.\d+\.\d+$"))
            {
                Debug.LogWarning("Build Version is not in a #.#.# format, can't update automatically");
                return;
            }
        
            var versionParts = version.Split('.');
            int patch = int.Parse(versionParts[2]);
            patch++;

            PlayerSettings.bundleVersion = $"{versionParts[0]}.{versionParts[1]}.{patch}";
        
            Debug.Log("Build version updated to " + PlayerSettings.bundleVersion);
        }
      

    }
}
