using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PageTypeGenerator;

namespace PageTypeApp
{
	class Program
	{
		static void Main( string[] args )
		{
			string className = "ASM.AboutNode";
			string namespaceName = "ASM.Core.Models.PageTypes";
			string kenticoDllRoot = @"C:\Users\itegrootenhuis\Documents\BZS\asm-web\CMS\bin";
			string webSiteRoot = @"C:\Users\itegrootenhuis\Documents\BZS\asm-web";

			Console.WriteLine( CodeGenerator.GenerateClassCode( className, namespaceName, kenticoDllRoot, webSiteRoot ) );
			Console.ReadLine( );
		}
	}
}
