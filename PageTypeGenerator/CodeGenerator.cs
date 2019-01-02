using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Base;
using CMS.DataEngine;
using CMS.FormEngine;

namespace PageTypeGenerator
{
    public class CodeGenerator
    {
		public static string GenerateClassCode( string className, string namespaceName, string kenticoDllRoot, string webSiteRoot )
		{
			// https://docs.google.com/document/d/1umNIQG2h3ZuuJo2c-p7LZUpZcXpuoOXYQKza-Y1KA_E/edit

			// http://www.csharp-examples.net/reflection-examples/


			// https://docs.kentico.com/k9/integrating-3rd-party-systems/using-the-kentico-api-externally



			// https://devnet.kentico.com/articles/take-advantage-of-kentico%E2%80%99s-nuget-feed-and-build-your-own-apps
			//CMS.DataEngine.ConnectionHelper.ConnectionString = "Data Source=client-sql-16;Initial Catalog=ASM-Web;Integrated Security=False;Persist Security Info=False;User ID=ASM-Web;Password=asm_/,.;Connect Timeout=60;Encrypt=False;Current Language=English;";
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
		}
	}
}
