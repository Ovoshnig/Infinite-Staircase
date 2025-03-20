using UnityEditor.Build.Reporting;
using UnityEditor.Build;

public class PreBuildMusicListGenerator : IPreprocessBuildWithReport
{
    public int callbackOrder => 0;

    public void OnPreprocessBuild(BuildReport report) => ResourcesPathsListCreator.CreateList();
}
