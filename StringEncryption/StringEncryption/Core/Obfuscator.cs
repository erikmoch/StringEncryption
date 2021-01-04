using System;

using dnlib.DotNet;

using StringEncryption.Core.Protections;

namespace StringEncryption.Core
{
    public class Obfuscator
    {
        private ModuleDef module;
        private readonly string modulePath;

        public Obfuscator(string path) => modulePath = path;

        public bool Run()
        {
            if (LoadModule())
            {
                if (StartObfuscation()) return true;
                else return false;
            }
            else return false;
        }
        private bool StartObfuscation()
        {
            try
            {
                Encrypter.Protect(module);
                WriteModule();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        private void WriteModule() => module.Write($"StringsEncrypted.exe");
        private bool LoadModule()
        {
            try
            {
                module = ModuleDefMD.Load(modulePath);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
