using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Reflection;
using System.Text;
using System.IO;
using WowTools.Core;

namespace WoWPacketViewer
{
    class ParserCompiler
    {
        public static Assembly CompileParser(string source, OpCodes opcode)
        {
            source = AddImpliedCode(source, opcode);
            using (CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp"))
            {
                CompilerParameters cp = new CompilerParameters();

                cp.GenerateInMemory = true;
                cp.TreatWarningsAsErrors = false;
                cp.GenerateExecutable = false;
                cp.ReferencedAssemblies.Add("System.dll");
                cp.ReferencedAssemblies.Add("System.Core.dll");
                cp.ReferencedAssemblies.Add("WowTools.Core.dll");
                cp.ReferencedAssemblies.Add("WoWPacketViewer.exe");

                CompilerResults cr = provider.CompileAssemblyFromSource(cp, source);

                if (cr.Errors.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (CompilerError ce in cr.Errors)
                        sb.AppendFormat("{0}", ce).AppendLine();
                    throw new Exception(sb.ToString());
                }

                return cr.CompiledAssembly;
            }
        }

        public static string AddImpliedCode(string source, OpCodes opcode)
        {
            bool knownOpcode = Enum.IsDefined(typeof (OpCodes), opcode);
            StringBuilder s = new StringBuilder(source.Length + 300);
            if (!source.Contains("using WowTools.Core"))
            {
                s.Append(
@"using System;
using System.Collections.Generic;
using WowTools.Core;").AppendLine().AppendLine();
            }
            int tab = 0;
            if (!source.Contains("namespace WoWPacketViewer"))
            {
                s.Append("namespace WoWPacketViewer").AppendLine();
                s.Append("{").AppendLine();
                tab++;
            }
            if (!source.Contains(" class "))
            {
                s.AppendFormat("    public class {0} : Parser", GetClassName(opcode)).AppendLine();
                s.Append("    {").AppendLine();
                tab++;
            }
            if(!source.Contains(" void "))  // hacky: assumes parser functions return void
            {
                if(!knownOpcode)
                {
                    s.AppendFormat("        [Parser((OpCodes){0})]", opcode).AppendLine();
                }
                string funcName = knownOpcode ? opcode.ToString() : "Handle" + (uint) opcode;
                s.AppendFormat("        public void {0}()", funcName).AppendLine();
                s.Append("        {").AppendLine();
                tab++;
            }
            var lines = source.Split(new[] {Environment.NewLine}, StringSplitOptions.None);
            foreach(string line in lines)
            {
                s.Append(new string(' ', 4*tab)); // add tabulation
                s.Append(line).AppendLine();
            }

            for (int i = tab - 1; i >= 0; i--)
            {
                s.Append(new string(' ', 4 * i)).Append("}").AppendLine();
            }

            using (var debug = new StreamWriter("LastParser.cs"))   // output complete code for possible debugging
            {
                debug.WriteLine(s.ToString());
            }

            return s.ToString();
        }

        public static string GetClassName(OpCodes opcode)
        {
            return ToCamel(opcode.ToString()) + "Parser";
        }

        /// <summary>
        /// Converts opcode name to camel case to be used in code.
        /// </summary>
        public static string ToCamel(string str)
        {
            var parts = str.Split('_');
            if (parts.Length < 2)
                return "Op" + str; // probably just a number

            for (int i = 1; i < parts.Length; i++)
            {
                parts[i] = parts[i][0] + parts[i].Substring(1).ToLower();
            }
            return String.Join("", parts, 1, parts.Length - 1);
        }
    }
}
