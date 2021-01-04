using System;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Collections.Generic;

using dnlib.DotNet;
using dnlib.DotNet.Emit;

using StringEncryption.Core.Utils;

namespace StringEncryption.Core.Protections
{
    public class Encrypter
    {
        private static MethodDef xorMethod;

        public static void Protect(ModuleDef module) => Start(module);

        private static void Start(ModuleDef module)
        {
            InjectDecryptMethod(module);

            TypeDef[] moduleTypes = module.GetTypes().Where(t => t.IsGlobalModuleType == false).ToArray();
            foreach (TypeDef moduleType in moduleTypes)
            {
                MethodDef[] moduleMethods = moduleType.Methods.Where(m => m.HasBody && m.Body.HasInstructions).ToArray();
                for (int i = 0; i < moduleMethods.Length; i++)
                    EncryptStrings(moduleMethods[i]);
            }
        }
        private static Random random = new Random();
        private static void EncryptStrings(MethodDef moduleMethod)
        {
            for (int i = 0; i < moduleMethod.Body.Instructions.Count; i++)
            {
                if (moduleMethod.Body.Instructions[i].OpCode == OpCodes.Ldstr)
                {
                    int bytesCount = Encoding.UTF8.GetByteCount(moduleMethod.Name);
                    int encryptionKey = random.Next(111, 999);
                    string originalString = moduleMethod.Body.Instructions[i].Operand as string;
                    string encryptedString = EncryptString(originalString, encryptionKey * bytesCount);

                    moduleMethod.Body.Instructions[i].Operand = encryptedString;
                    moduleMethod.Body.Instructions.Insert(i += 1, new Instruction(OpCodes.Ldc_I4, encryptionKey));
                    moduleMethod.Body.Instructions.Insert(i += 1, new Instruction(OpCodes.Call, xorMethod));
                }
            }
        }
        private static string EncryptString(string originalString, int encryptionKey)
        {
            StringBuilder sbInputString = new StringBuilder(originalString);
            StringBuilder sbOutString = new StringBuilder(originalString.Length);
            char Textch;
            for (int iCount = 0; iCount < originalString.Length; iCount++)
            {
                Textch = sbInputString[iCount];
                Textch = (char)(Textch ^ encryptionKey);
                sbOutString.Append(Textch);
            }
            return sbOutString.ToString();
        }

        private static void InjectDecryptMethod(ModuleDef module)
        {
            ModuleDefMD typeModule = ModuleDefMD.Load(typeof(Xor).Module);
            TypeDef typeDef = typeModule.ResolveTypeDef(MDToken.ToRID(typeof(Xor).MetadataToken));
            IEnumerable<IDnlibDef> members = InjectHelper.Inject(typeDef, module.GlobalType,
                module);
            xorMethod = (MethodDef)members.Single(method => method.Name == "DecryptString");
        }
    }

    public static class Xor
    {
        public static string DecryptString(string originalString, int encryptionKey)
        {
            var callingMethod = new StackTrace().GetFrame(1).GetMethod();
            int bytesCount = Encoding.UTF8.GetByteCount(callingMethod.Name);

            encryptionKey *= bytesCount;

            StringBuilder sbInputString = new StringBuilder(originalString);
            StringBuilder sbOutString = new StringBuilder(originalString.Length);
            char Textch;
            for (int iCount = 0; iCount < originalString.Length; iCount++)
            {
                Textch = sbInputString[iCount];
                Textch = (char)(Textch ^ encryptionKey);
                sbOutString.Append(Textch);
            }
            return sbOutString.ToString();
        }
    }
}