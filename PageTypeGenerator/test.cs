using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CMS.Base;
using CMS.DataEngine;
using CMS.FormEngine;
using CMS.Membership;

namespace KenticoCodeGenerator
{
	public static class CodeGenerator
	{

		public static string GenerateClassCode( string className, string namespaceName, string kenticoDllRoot, string webSiteRoot )
		{
			// https://docs.google.com/document/d/1umNIQG2h3ZuuJo2c-p7LZUpZcXpuoOXYQKza-Y1KA_E/edit

			// http://www.csharp-examples.net/reflection-examples/


			// https://docs.kentico.com/k9/integrating-3rd-party-systems/using-the-kentico-api-externally



			// https://devnet.kentico.com/articles/take-advantage-of-kentico%E2%80%99s-nuget-feed-and-build-your-own-apps
			//CMS.DataEngine.ConnectionHelper.ConnectionString = "Data Source=CLIENT-SQL-03;Initial Catalog=Kentico-ROLVS-MVC;Integrated Security=False;Persist Security Info=False;User ID=sa;Password=sys_440/,.;Connect Timeout=60;Encrypt=False;Current Language=English;";
			CMS.DataEngine.CMSApplication.Init();
			CMS.Base.SystemContext.WebApplicationPhysicalPath = webSiteRoot.TrimEnd( '\\' ); // "C:\SourceControl\kentico-rolvs-mvc\CMS"
																							 // Gets an object representing a specific Kentico user
																							 //UserInfo user = UserInfoProvider.GetUserInfo("administrator");

			string code = "";
			// Sets the context of the user
			using( new CMSActionContext { LogSynchronization = false, LogWebFarmTasks = false } )
			{
				var classInfo = DataClassInfoProvider.GetDataClassInfo( className ); // "RVS.ArticleNode"
				classInfo.ClassCodeGenerationSettingsInfo.NameSpace = namespaceName; // RolandCloud.Core.Models.PageTypes
				code = ContentItemCodeGenerator.Internal.GenerateItemClass( classInfo );
			}
			return code;


			/*
            // Set base path (I assume for web config)
            string ddlPathForBase = Path.Combine(kenticoDllRoot, "CMS.Base.dll");
            string ddlPathForDataEngine = Path.Combine(kenticoDllRoot, "CMS.DataEngine.dll");
            string ddlPathForFormEngine = Path.Combine(kenticoDllRoot, "CMS.FormEngine.dll");
            Assembly baseAssembly = Assembly.LoadFrom(ddlPathForBase);
            Assembly dataEngineAssembly = Assembly.LoadFrom(ddlPathForDataEngine);
            Assembly formngineAssembly = Assembly.LoadFrom(ddlPathForFormEngine);
            Assembly.


            AppDomain.CurrentDomain.Load(baseAssembly.GetName());
            AppDomain.CurrentDomain.Load(dataEngineAssembly.GetName());
            AppDomain.CurrentDomain.Load(formngineAssembly.GetName());



            
            Type cmsBaseSystemContextType = baseAssembly.GetType("CMS.Base.SystemContext");
            object baseInstance = Activator.CreateInstance(cmsBaseSystemContextType);
            PropertyInfo pathPropertyInfo = cmsBaseSystemContextType.GetProperty("WebApplicationPhysicalPath");

            pathPropertyInfo.SetValue(baseInstance, webSiteRoot);


            // Data Engine Time
            Type dataClassinfoProviderType = dataEngineAssembly.GetType("CMS.DataEngine.DataClassInfoProvider");
            object dataClassProviderInstance = Activator.CreateInstance(dataClassinfoProviderType);
            MethodInfo getDataClassInfoMethodInfo = dataClassinfoProviderType.GetMethod("GetDataClassInfo");

            getDataClassInfoMethodInfo.Invoke()

            */

			//MethodInfo Method = myType.GetMethod("CMS.Base.SystemContext.WebApplicationPhysicalPath");
			//object myInstance = Activator.CreateInstance(myType);
			//Method.Invoke(myInstance, null);

			//AppDomainSetup domaininfo = new AppDomainSetup();
			//domaininfo.ApplicationBase = kenticoDllRoot;
			//Evidence adevidence = AppDomain.CurrentDomain.Evidence;
			//AppDomain domain = AppDomain.CreateDomain("MyDomain", adevidence, domaininfo);

			//LoadAndRun(kenticoDllRoot, new string[] { "CMS.Base.dll" , "CMS.DataEngine.dll" });

			/*
            CMS.Base.SystemContext.WebApplicationPhysicalPath = webSiteRoot; // "C:\SourceControl\kentico-rolvs-mvc\CMS\"
            var classInfo = CMS.DataEngine.DataClassInfoProvider.GetDataClassInfo(className); // "RVS.ArticleNode"
            classInfo.ClassCodeGenerationSettingsInfo.NameSpace = namespaceName; // RolandCloud.Core.Models.PageTypes
            CMS.DataEngine.ContentItemCodeGenerator.Internal.GenerateItemClass(classInfo);

            /*
            var DLL = Assembly.LoadFile(Path.Combine(kenticoDllRoot, "CMS.Base.dll"));

            foreach (Type type in DLL.GetExportedTypes())
            {
                var c = Activator.CreateInstance(type);
                type.InvokeMember("Output", BindingFlags.InvokeMethod, null, c, new object[] { @"Hello" });
            }
            */

			return "/* TEST */";
		}


		private static void LoadAndRun( string binPath, string[] dllsToLoad )
		{
			DirectoryInfo oDirectoryInfo = new DirectoryInfo( binPath );

			//Check the directory exists
			if( oDirectoryInfo.Exists )
			{
				//Foreach Assembly with dll as the extension
				foreach( var file in dllsToLoad )
				{

					Assembly tempAssembly = null;

					//Before loading the assembly, check all current loaded assemblies in case talready loaded
					//has already been loaded as a reference to another assembly
					//Loading the assembly twice can cause major issues
					foreach( Assembly loadedAssembly in AppDomain.CurrentDomain.GetAssemblies() )
					{
						//Check the assembly is not dynamically generated as we are not interested in these
						if( loadedAssembly.ManifestModule.GetType().Namespace != "System.Reflection.Emit" )
						{
							//Get the loaded assembly filename
							string sLoadedFilename =
								loadedAssembly.CodeBase.Substring( loadedAssembly.CodeBase.LastIndexOf( '/' ) + 1 );

							//If the filenames match, set the assembly to the one that is already loaded
							if( sLoadedFilename.ToUpper() == file.ToUpper() )
							{
								tempAssembly = loadedAssembly;
								break;
							}
						}
					}

					//If the assembly is not aleady loaded, load it manually
					if( tempAssembly == null )
					{
						tempAssembly = Assembly.LoadFrom( Path.Combine( binPath, file ) );
					}

					Assembly a = tempAssembly;
					//var names = a.GetReferencedAssemblies();
				}

			}
		}

	}
}
