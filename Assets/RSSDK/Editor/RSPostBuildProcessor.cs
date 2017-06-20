using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.Collections;
using System.IO;
using System.Linq;
using System;
using Microsoft.Win32;

public class RSPostBuildProcessor
{
	public static string SpecifiedRuntimePath = null;

	/// <summary>
	/// Clears the console.
	/// </summary>
	static void ClearEditorConsole () {
		var logEntries = System.Type.GetType("UnityEditorInternal.LogEntries,UnityEditor.dll");
		var clear = logEntries.GetMethod("Clear", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
		clear.Invoke(null,null);
	}

	/// <summary>
	/// Raises the postprocess build event.
	/// </summary>
	/// <param name="target">Target.</param>
	/// <param name="pathToBuiltProject">Path to built project.</param>
	[PostProcessBuildAttribute(1)]
	public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject) 
	{
	
		ClearEditorConsole ();

		// Retrieve all the native dlls present in current project
		string [] nativePlugins = Directory.GetFiles (Application.dataPath, "*_c.dll", SearchOption.AllDirectories);
		if (nativePlugins.Length == 0) {
			Debug.LogWarning ("Failed to copy runtime directories. No RSSDK dlls found in current project.");
			return;
		}

		// Retrieve the runtime path to copy from
		string _runtimePath = GetRuntimePath(SpecifiedRuntimePath);
		if (_runtimePath == null)
			return;

		// Destination directory path to place runtime contents
		string destPath = Path.Combine(Directory.GetParent (pathToBuiltProject).FullName, Path.Combine (Path.GetFileNameWithoutExtension (pathToBuiltProject) + "_Data", "Plugins"));
		destPath = Path.Combine (destPath, "runtime");

		int componentCopyCount = 0; // counter for actually contents copied.
		for (int i=0; i<nativePlugins.Length; i++) {
			// Extract the native dll name
			nativePlugins [i] = nativePlugins [i].Split (Path.DirectorySeparatorChar).Last ();

			// Retrieve the directories containing the same dll from SpecifiedRuntimePath
			string[] paths = Directory.GetFiles (_runtimePath, nativePlugins [i], SearchOption.AllDirectories);
			foreach (string path in paths) {

				// Exclude paths that don't belong to current build target
				if (target.ToString().Contains ("64") && !path.Contains ("64"))
					continue;
				if (!target.ToString().Contains ("64") && path.Contains ("64"))
					continue;

				// Source component directory
				DirectoryInfo srcInfo = Directory.GetParent (path);

                // Skip if dest directory exists (i.e. if the same *_c.dll file is found again in list e.g. x86, x86_64)
                if (Directory.Exists(Path.Combine(Path.Combine(destPath, path.Split(Path.DirectorySeparatorChar)[path.Split(Path.DirectorySeparatorChar).Length - 3]), srcInfo.Name))) continue;

                // Destination component directory 
                DirectoryInfo dstInfo = Directory.CreateDirectory (Path.Combine (Path.Combine (destPath, path.Split (Path.DirectorySeparatorChar) [path.Split (Path.DirectorySeparatorChar).Length - 3]), srcInfo.Name));

				RecursiveDirectoryCopy (srcInfo, dstInfo);
				
				// If exists, Copy data directory for component
				String dataDirPath = Path.Combine(srcInfo.Parent.FullName, "data");
				if(Directory.Exists(dataDirPath))
				{
					DirectoryInfo dstDataInfo = Directory.CreateDirectory (Path.Combine (Path.Combine (destPath, path.Split (Path.DirectorySeparatorChar) [path.Split (Path.DirectorySeparatorChar).Length - 3]), "data"));
					DirectoryInfo srcDataInfo = new DirectoryInfo(dataDirPath);
					RecursiveDirectoryCopy (srcDataInfo, dstDataInfo);
				}

				componentCopyCount++;
			}
		}

		int componentInProjectCount = 0; //counter for components found in current project
		// Retrieve managed dlls loaded in current project
		var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies ();
		foreach (var loadedAssembly in loadedAssemblies)
			if (loadedAssembly.GetName ().Name.Contains ("Intel.RealSense")) 
				componentInProjectCount++;

		// final check: managed components (dlls) count = actual runtime component directories count 
		if (componentCopyCount != componentInProjectCount) 
			Debug.LogWarning ("Found " + componentInProjectCount + " managed SDK Plugin components in project, copied " + componentCopyCount + " runtime components in " + destPath);
		else
			Debug.Log ("SDK Plugin Runtime components placed successfully in " + destPath);
	}

	/// <summary>
	/// Recursively copies contents.
	/// </summary>
	/// <param name="src">Source Directory Info.</param>
	/// <param name="dst">Destination Directory Info.</param>
	public static void RecursiveDirectoryCopy (DirectoryInfo src, DirectoryInfo dst)
	{
		// Create Directories
		foreach (DirectoryInfo srcDir in src.GetDirectories())
			RecursiveDirectoryCopy (srcDir, dst.CreateSubdirectory (srcDir.Name));

		// Copy Files
		foreach (FileInfo srcFile in src.GetFiles()) {
			try {
				srcFile.CopyTo (Path.Combine (dst.FullName, srcFile.Name), true);
			} catch (Exception e) {
				Debug.Log (e.ToString ());
			}
		}
	}

	private static String GetLocalRuntime()
	{
		RegistryKey baseReg = Registry.LocalMachine;            
		var rKey = baseReg.OpenSubKey("Software\\Intel\\RSSDK\\Dispatch");
		if (rKey != null)
			return rKey.GetValue("LocalRuntime").ToString();
		else
			return null;
	}
	
	private static Boolean IsSDKInstalled()
	{
		RegistryKey baseReg = Registry.LocalMachine;
		var rKey = baseReg.OpenSubKey("Software\\Intel\\RSSDK_DEV");
		return (rKey != null);
	}
	
	private static String GetRuntimePath(string _specifiedRuntimePath = null)
	{
		string fullRuntimePath = null;
		
		// specifiedRuntimePath
		if (_specifiedRuntimePath != null) {
			return _specifiedRuntimePath;
		}
		
		// LocalRuntime
		fullRuntimePath = GetLocalRuntime ();
		if (fullRuntimePath != null && fullRuntimePath.Length != 0) return fullRuntimePath;
		
		// RSSDK_DIR_Runtime
		if (IsSDKInstalled()) {
			fullRuntimePath = Environment.GetEnvironmentVariable("RSSDK_DIR");
			if (fullRuntimePath == null) return null;
			
			fullRuntimePath = Path.Combine(fullRuntimePath, "runtime");
			return fullRuntimePath;
		}
		
		UnityEngine.Debug.Log ("SDK not installed.");
		
		// SDK not Installed
		fullRuntimePath = null;
		return fullRuntimePath;
	}

}
