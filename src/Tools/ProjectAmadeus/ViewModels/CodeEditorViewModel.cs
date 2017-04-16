using Caliburn.Micro;
using ProjectAmadeus.Models;
using SciAdvNet.SC3;
using System.IO;
using System.Linq;

namespace ProjectAmadeus.ViewModels
{
    public sealed class CodeEditorViewModel : Screen, IEditor
    {
        private readonly Workspace _workspace;

        private Module _module;

        public CodeEditorViewModel(Workspace workspace)
        {
            _workspace = workspace;
        }

        public CodeEditorViewModel()
        {
        }

        public string Code { get; set; }
        public bool AnyUnsavedChanges => false;

        protected override void OnInitialize()
        {
            base.OnInitialize();

            _module = _workspace.CurrentModule;
            Decompile();
        }

        private void Decompile()
        {
            //using (var decompiler = SC3Disassember.Open(_module.Script, leaveOpen: true))
            //{
            //    var sw = new StringWriter();
            //    var codeWriter = new DefaultCodeWriter(sw);
            //    foreach (var blockDefinition in _module.Script.Blocks.Select(x => x.AsCode()))
            //    {
            //        try
            //        {
            //            var decompilationResult = decompiler.DecompileCodeBlock(blockDefinition);
            //            codeWriter.WriteCodeBlock(decompilationResult);
            //        }
            //        catch
            //        {
            //        }
            //    }

            //    Code = sw.ToString();
            //}
        }

        public void SaveChanges()
        {

        }
    }
}
