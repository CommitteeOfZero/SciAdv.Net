namespace SciAdvNet.SC3Script
{
    public abstract class CodeVisitor
    {
        public virtual void Visit(IVisitable visitable)
        {
            visitable?.Accept(this);
        }

        public virtual void VisitCodeBlock(DisassemblyResult codeBlock) { }
        public virtual void DefaultVisitInstruction(Instruction instruction) { }

        public virtual void VisitAssignInstruction(AssignInstruction instruction)
        {
            DefaultVisitInstruction(instruction);
        }

        public virtual void VisitCreateThreadInstruction(CreateThreadInstruction instruction)
        {
            DefaultVisitInstruction(instruction);
        }

        public virtual void VisitTerminateThreadInstruction(TerminateThreadInstruction instruction)
        {
            DefaultVisitInstruction(instruction);
        }

        public virtual void VisitExitThreadInstruction(ExitThreadInstruction instruction)
        {
            DefaultVisitInstruction(instruction);
        }

        public virtual void VisitLoadScriptInstruction(LoadScriptInstruction instruction)
        {
            DefaultVisitInstruction(instruction);
        }

        public virtual void VisitHaltInstruction(HaltInstruction instruction)
        {
            DefaultVisitInstruction(instruction);
        }

        public virtual void VisitJumpInstruction(JumpInstruction instruction)
        {
            DefaultVisitInstruction(instruction);
        }

        public virtual void VisitCallFarInstruction(CallFarInstruction instruction)
        {
            DefaultVisitInstruction(instruction);
        }

        public virtual void VisitReturnInstruction(ReturnInstruction instruction)
        {
            DefaultVisitInstruction(instruction);
        }

        public virtual void VisitJumpOnFlagInstruction(JumpOnFlagInstruction instruction)
        {
            DefaultVisitInstruction(instruction);
        }

        public virtual void VisitWaitForFlagInstruction(WaitForFlagInstruction instruction)
        {
            DefaultVisitInstruction(instruction);
        }

        public virtual void VisitSetFlagInstruction(SetFlagInstruction instruction)
        {
            DefaultVisitInstruction(instruction);
        }

        public virtual void VisitResetFlagInstruction(ResetFlagInstruction instruction)
        {
            DefaultVisitInstruction(instruction);
        }

        public virtual void VisitPlayBgmInstruction(PlayBgmInstruction instruction)
        {
            DefaultVisitInstruction(instruction);
        }

        public virtual void VisitStopBgmInstruction(StopBgmInstruction instruction)
        {
            DefaultVisitInstruction(instruction);
        }

        public virtual void VisitStopSfxInstruction(StopSfxInstruction instruction)
        {
            DefaultVisitInstruction(instruction);
        }

        public virtual void VisitDisplayChapterNameInstruction(DisplayChapterName instruction)
        {
            DefaultVisitInstruction(instruction);
        }

        public virtual void VisitWaitInstruction(WaitInstruction instruction)
        {
            DefaultVisitInstruction(instruction);
        }

        public virtual void VisitCreateRenderTargetInstruction(CreateRenderTargetInstruction instruction)
        {
            DefaultVisitInstruction(instruction);
        }

        public virtual void VisitLoadTextureInstruction(LoadTextureInstruction instruction)
        {
            DefaultVisitInstruction(instruction);
        }

        public virtual void VisitCheckpointInstruction(CheckpointInstruction instruction)
        {
            DefaultVisitInstruction(instruction);
        }

        public virtual void VisitClearDialogueWindowInstruction(ClearDialogueWindowInstruction instruction)
        {
            DefaultVisitInstruction(instruction);
        }

        public virtual void VisitLoadDialogueLineInstruction(LoadDialogueLineInstruction instruction)
        {
            DefaultVisitInstruction(instruction);
        }

        public virtual void VisitLoadVoiceActedDialogueLineInstruction(LoadVoiceActedDialogueLineInstruction instruction)
        {
            DefaultVisitInstruction(instruction);
        }

        public virtual void VisitDisplayDialogueLineInstruction(DisplayDialogueLineInstruction instruction)
        {
            DefaultVisitInstruction(instruction);
        }

        public virtual void VisitInstantiateTextStyleInstruction(InstantiateTextStyleInstruction instruction)
        {
            DefaultVisitInstruction(instruction);
        }

        public virtual void VisitShowDialogueWindowInstruction(ShowDialogueWindowInstruction instruction)
        {
            DefaultVisitInstruction(instruction);
        }

        public virtual void VisitAwaitShowDialogueWindowInstruction(AwaitShowDialogueWindowInstruction instruction)
        {
            DefaultVisitInstruction(instruction);
        }

        public virtual void VisitAwaitHideDialogueWindowInstruction(AwaitHideDialogueWindowInstruction instruction)
        {
            DefaultVisitInstruction(instruction);
        }

        public virtual void VisitCloseDialogueWindowInstruction(HideDialogueWindowInstruction instruction)
        {
            DefaultVisitInstruction(instruction);
        }

        public virtual void VisitInstanciateStringArrayInstruction(InstanciateStringArrayInstruction instruction)
        {
            DefaultVisitInstruction(instruction);
        }

        public virtual void VisitInitSubsystemInstruction(InitSubsystemInstruction instruction)
        {
            DefaultVisitInstruction(instruction);
        }

        public virtual void VisitLoadBackgroundInstruction(LoadBackgroundInstruction instruction)
        {
            DefaultVisitInstruction(instruction);
        }

        public virtual void VisitLoadCharacterInstruction(LoadCharacterInstruction instruction)
        {
            DefaultVisitInstruction(instruction);
        }

        public virtual void VisitQuickSaveInstruction(QuickSaveInstruction instruction)
        {
            DefaultVisitInstruction(instruction);
        }

        public virtual void VisitSaveStateInstruction(SaveStateInstruction instruction)
        {
            DefaultVisitInstruction(instruction);
        }

        public virtual void VisitLoadStateInstruction(LoadStateInstruction instruction)
        {
            DefaultVisitInstruction(instruction);
        }

        public virtual void VisitPrimitiveTypeValue(PrimitiveTypeValue value) { }
        public virtual void VisitConstantExpression(ConstantExpression expression) { }
        public virtual void VisitVariableExpression(VariableExpression expression) { }
        public virtual void VisitPrefixUnaryExpression(PrefixUnaryExpression expression) { }

        public virtual void VisitPostfixUnaryExpression(PostfixUnaryExpression expression) { }
        public virtual void VisitBinaryExpression(BinaryExpression expression) { }
        public virtual void VisitAssignmentExpression(AssignmentExpression expression) { }
        public virtual void VisitDataBlockReferenceExpression(DataBlockReferenceExpression expression) { }
        public virtual void VisitDataBlockAccessExpression(DataBlockAccessExpression expression) { }
        public virtual void VisitRandomNumberExpression(RandomNumberExpression expression) { }
    }
}
