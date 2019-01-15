using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CMS.Base;
using CMS.FormEngine;
using CMS.Membership;

namespace PageTypeGenerator
{
	public class CodeGenerator
	{
		public static Type CmsDataEngine;


		public static Type LoadExternalAssembly( string rootForProjectAssembily, string type = null )
		{
			Assembly asm = null;
			asm = Assembly.LoadFrom( rootForProjectAssembily );
			if( type == null )
			{
				return null;
			}
			return asm.GetType( type );
		}

		public static void LoadExternalAssemblies( string rootForProjectAssembiles )
		{
			// load external project assemblies
			LoadExternalAssembly( rootForProjectAssembiles + "CMS.DocumentEngine.dll" );
			CmsDataEngine = LoadExternalAssembly( rootForProjectAssembiles + "CMS.DataEngine.dll", "CMS.DataEngine.CMSApplication" );
		}

		public static string GenerateClassCode( string className, string namespaceName, string kenticoDllRoot, string webSiteRoot, string connectionString )
		{
			// https://docs.google.com/document/d/1umNIQG2h3ZuuJo2c-p7LZUpZcXpuoOXYQKza-Y1KA_E/edit

			// http://www.csharp-examples.net/reflection-examples/


			// https://docs.kentico.com/k9/integrating-3rd-party-systems/using-the-kentico-api-externally



			// https://devnet.kentico.com/articles/take-advantage-of-kentico%E2%80%99s-nuget-feed-and-build-your-own-apps
			CMS.DataEngine.ConnectionHelper.ConnectionString = connectionString;
			//CMS.DataEngine.CMSApplication.Init();

			LoadExternalAssemblies( kenticoDllRoot );

			MethodInfo cmsDataEngineInit = CmsDataEngine.GetMethod( "Init" );
			// Create an instance.
			object cmsDataEngineObject = Activator.CreateInstance( CmsDataEngine );
			// Execute the method.
			cmsDataEngineInit.Invoke( cmsDataEngineObject, null );

			CMS.Base.SystemContext.WebApplicationPhysicalPath = webSiteRoot.TrimEnd( '\\' ); // "C:\SourceControl\kentico-rolvs-mvc\CMS"
																							 // Gets an object representing a specific Kentico user
																							 //UserInfo user = UserInfoProvider.GetUserInfo("administrator");


			UserInfo user = UserInfoProvider.GetUserInfo( "administrator" );
			string code = "";

			// Sets the context of the user
			using( new CMSActionContext( user ) ) //{ LogSynchronization = false, LogWebFarmTasks = false } )
			{
				var classInfo = CMS.DataEngine.DataClassInfoProvider.GetDataClassInfo( className ); // "ASM.AboutNode"

				classInfo.ClassCodeGenerationSettingsInfo.NameSpace = namespaceName; // ASM.Core.Models.PageTypes

				if( !SystemContext.IsPrecompiledWebsite )
				{
					// can't save to folder ...
				}

				if( ContentItemCodeGenerator.Internal.CanGenerateItemClass( classInfo ) )
				{

					code = CMS.FormEngine.ContentItemCodeGenerator.Internal.GenerateItemClass( classInfo );
				}

			}

			return code;
		}
	}
}
