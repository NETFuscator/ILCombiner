using System;
using System.Collections.Generic;
using System.Text;
using dnlib.DotNet;

namespace ILCombiner.Combiners {
    internal class AssemblyMerger : ICombiner {
        public ModuleDef Combine(ModuleDef module, ILCombinerDependency[] dependencies) {
            this._importer = new Importer(module);

            foreach (var dependency in dependencies)
                if (!MergeDependency(module, dependency.Module))
                    throw new ILCombinerException($"Failed to merge: {dependency.Module.FullName}");

            return module;
        }

        private bool MergeDependency(ModuleDef module, ModuleDef dependency) {
            if (!MergeSkeleton(module, dependency))
                return false;
            
            foreach (var pair in this._typeMap) {
                foreach (var field in pair.Key.Fields) {
                    var sigType = field.FieldSig.Type.ToTypeDefOrRef();

                    FieldSig sig = null;

                    if (sigType.IsTypeDef)
                        sig = new FieldSig(this._importer.Import(sigType as TypeDef).ToTypeSig());
                    else
                        sig = this._importer.Import(field.FieldSig);

                    pair.Value.Fields.Add(new FieldDefUser(field.Name, sig, field.Attributes));
                }
            }

            foreach (var pair in this._typeMap) {
                foreach (var method in pair.Key.Methods) {
                    var skeletonMethod = new MethodDefUser(method.Name, this._importer.Import(method.MethodSig));

                    skeletonMethod.Body = method.Body;

                    this._methodMap[method] = skeletonMethod;

                    pair.Value.Methods.Add(skeletonMethod);
                }
            }

            return true;
        }

        private bool MergeSkeleton(ModuleDef module, ModuleDef dependency) {
            foreach (var type in dependency.Types) {
                var skeletonType = new TypeDefUser(type.Namespace, type.Name) {
                    Attributes = type.Attributes
                };
                
                MergeSkeletonNestedTypes(module, type, skeletonType);

                module.Types.Add(skeletonType);

                this._typeMap[type] = skeletonType;
            }

            return true;
        }

        private void MergeSkeletonNestedTypes(ModuleDef module, TypeDef type, TypeDef skeleton) {
            if (!type.HasNestedTypes)
                return;

            foreach (var nestedType in type.NestedTypes) {
                var newSkeleton = new TypeDefUser(nestedType.Namespace, type.Name) {
                    Attributes = nestedType.Attributes
                };

                MergeSkeletonNestedTypes(module, nestedType, newSkeleton);

                skeleton.NestedTypes.Add(newSkeleton);
            }

            this._typeMap[type] = skeleton;
        }

        private Importer _importer;

        private Dictionary<TypeDef, TypeDef> _typeMap = new Dictionary<TypeDef, TypeDef>();
        private Dictionary<FieldDef, FieldDef> _fieldMap = new Dictionary<FieldDef, FieldDef>();
        private Dictionary<MethodDef, MethodDef> _methodMap = new Dictionary<MethodDef, MethodDef>();
    }
}