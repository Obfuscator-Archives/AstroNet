﻿using dnlib.DotNet;
using dnlib.DotNet.Emit;
using Astro_Renewed.Protections.Anti.Runtime;
using Astro_Renewed.Services;
using System.Linq;

namespace Astro_Renewed.Protections.Anti
{
    internal class AntiDump
    {
        public static void Execute(ModuleDef mod)
        {
            var typeModule = ModuleDefMD.Load(typeof(AntiDumpRun).Module);
            var cctor = mod.GlobalType.FindOrCreateStaticConstructor();
            var typeDef = typeModule.ResolveTypeDef(MDToken.ToRID(typeof(AntiDumpRun).MetadataToken));
            var members = Inject_Helper.InjectHelper.Inject(typeDef, mod.GlobalType, mod);
            var init = (MethodDef)members.Single(method => method.Name == "Initialize");
            cctor.Body.Instructions.Insert(0, Instruction.Create(OpCodes.Call, init));
            foreach (var md in mod.GlobalType.Methods)
            {
                if (md.Name != ".ctor") continue;
                mod.GlobalType.Remove(md);
                break;
            }
        }
    }
}