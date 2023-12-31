﻿using dnlib.DotNet;
using dnlib.DotNet.Emit;
using Astro_Renewed.Protections.Anti.Runtime;
using Astro_Renewed.Services;
using System.Linq;

namespace Astro_Renewed.Protections.Anti
{
    internal class Antimanything
    {
        public static void Execute(ModuleDef module)
        {
            var typeModule = ModuleDefMD.Load(typeof(SelfDeleteClass).Module);
            var cctor = module.GlobalType.FindOrCreateStaticConstructor();
            var typeDef = typeModule.ResolveTypeDef(MDToken.ToRID(typeof(SelfDeleteClass).MetadataToken));
            var members = Inject_Helper.InjectHelper.Inject(typeDef, module.GlobalType, module);
            var init = (MethodDef)members.Single(method => method.Name == "Init");
            cctor.Body.Instructions.Insert(0, Instruction.Create(OpCodes.Call, init));
            foreach (var md in module.GlobalType.Methods)
            {
                if (md.Name != ".ctor") continue;
                module.GlobalType.Remove(md);
                break;
            }
        }
    }
}