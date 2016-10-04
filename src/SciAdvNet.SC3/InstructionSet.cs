namespace SciAdvNet.SC3
{
    public sealed class AssignInstruction : Instruction
    {
        public Expression AssignmentStatement { get; internal set; }

        public override void Accept(CodeVisitor visitor)
        {
            visitor.VisitAssignInstruction(this);
        }
    }

    public sealed class CreateThreadInstruction : Instruction
    {
        public Expression ThreadId { get; internal set; }
        public Expression Slot { get; internal set; }

        public override void Accept(CodeVisitor visitor)
        {
            visitor.VisitCreateThreadInstruction(this);
        }
    }

    public sealed class TerminateThreadInstruction : Instruction
    {
        public Expression ThreadId { get; internal set; }

        public override void Accept(CodeVisitor visitor)
        {
            visitor.VisitTerminateThreadInstruction(this);
        }
    }

    public sealed class ExitThreadInstruction : Instruction
    {
        public override void Accept(CodeVisitor visitor)
        {
            visitor.VisitExitThreadInstruction(this);
        }
    }

    public sealed class LoadScriptInstruction : Instruction
    {
        public Expression Slot { get; internal set; }
        public Expression ScriptId { get; internal set; }

        public override void Accept(CodeVisitor visitor)
        {
            visitor.VisitLoadScriptInstruction(this);
        }
    }

    public sealed class HaltInstruction : Instruction
    {
        public override void Accept(CodeVisitor visitor)
        {
            visitor.VisitHaltInstruction(this);
        }
    }

    public sealed class JumpInstruction : Instruction
    {
        public PrimitiveTypeValue TargetBlockId { get; internal set; }

        public override void Accept(CodeVisitor visitor)
        {
            visitor.VisitJumpInstruction(this);
        }
    }

    public sealed class CallFarInstruction : Instruction
    {
        public Expression Slot { get; internal set; }
        public PrimitiveTypeValue TargetBlockId { get; internal set; }
        public PrimitiveTypeValue ReturnAddressId { get; internal set; }

        public override void Accept(CodeVisitor visitor)
        {
            visitor.VisitCallFarInstruction(this);
        }
    }

    public sealed class ReturnInstruction : Instruction
    {
        public override void Accept(CodeVisitor visitor)
        {
            visitor.VisitReturnInstruction(this);
        }
    }

    public sealed class JumpOnFlagInstruction : Instruction
    {
        public PrimitiveTypeValue ExpectedValue { get; internal set; }
        public Expression FlagId { get; internal set; }
        public Expression TargetBlockId { get; internal set; }

        public override void Accept(CodeVisitor visitor)
        {
            visitor.VisitJumpOnFlagInstruction(this);
        }
    }

    public sealed class WaitForFlagInstruction : Instruction
    {
        public PrimitiveTypeValue Value { get; internal set; }
        public Expression FlagId { get; internal set; }

        public override void Accept(CodeVisitor visitor)
        {
            visitor.VisitWaitForFlagInstruction(this);
        }
    }

    public sealed class SetFlagInstruction : Instruction
    {
        public Expression FlagId { get; internal set; }

        public override void Accept(CodeVisitor visitor)
        {
            visitor.VisitSetFlagInstruction(this);
        }
    }

    public sealed class ResetFlagInstruction : Instruction
    {
        public Expression FlagId { get; internal set; }

        public override void Accept(CodeVisitor visitor)
        {
            visitor.VisitResetFlagInstruction(this);
        }
    }

    public sealed class PlayBgmInstruction : Instruction
    {
        public PrimitiveTypeValue Loop { get; internal set; }
        public Expression BgmId { get; internal set; }

        public override void Accept(CodeVisitor visitor)
        {
            visitor.VisitPlayBgmInstruction(this);
        }
    }

    public sealed class StopBgmInstruction : Instruction
    {
        public Expression BgmId { get; internal set; }

        public override void Accept(CodeVisitor visitor)
        {
            visitor.VisitStopBgmInstruction(this);
        }
    }

    public sealed class StopSfxInstruction : Instruction
    {
        public Expression ChannelId { get; internal set; }

        public override void Accept(CodeVisitor visitor)
        {
            visitor.VisitStopSfxInstruction(this);
        }
    }

    public sealed class DisplayChapterName : Instruction
    {
        public override void Accept(CodeVisitor visitor)
        {
            visitor.VisitDisplayChapterNameInstruction(this);
        }
    }

    public sealed class WaitInstruction : Instruction
    {
        public Expression Delay { get; internal set; }

        public override void Accept(CodeVisitor visitor)
        {
            visitor.VisitWaitInstruction(this);
        }
    }

    public sealed class CreateRenderTargetInstruction : Instruction
    {
        public Expression RenderTargetId { get; internal set; }
        public Expression Width { get; internal set; }
        public Expression Height { get; internal set; }

        public override void Accept(CodeVisitor visitor)
        {
            visitor.VisitCreateRenderTargetInstruction(this);
        }
    }

    public sealed class LoadTextureInstruction : Instruction
    {
        public Expression RenderTargetId { get; internal set; }
        public Expression TextureId { get; internal set; }

        public override void Accept(CodeVisitor visitor)
        {
            visitor.VisitLoadTextureInstruction(this);
        }
    }

    public sealed class CheckpointInstruction : Instruction
    {
        public PrimitiveTypeValue CheckpointId { get; internal set; }

        public override void Accept(CodeVisitor visitor)
        {
            visitor.VisitCheckpointInstruction(this);
        }
    }

    public sealed class ClearDialogueWindowInstruction : Instruction
    {
        public override void Accept(CodeVisitor visitor)
        {
            visitor.VisitClearDialogueWindowInstruction(this);
        }
    }

    public sealed class LoadDialogueLineInstruction : Instruction
    {
        public Expression CharacterId { get; internal set; }
        public PrimitiveTypeValue LineId { get; internal set; }

        public override void Accept(CodeVisitor visitor)
        {
            visitor.VisitLoadDialogueLineInstruction(this);
        }
    }

    public sealed class LoadVoiceActedDialogueLineInstruction : Instruction
    {
        public Expression CharacterId { get; internal set; }
        public PrimitiveTypeValue LineId { get; internal set; }
        public Expression VoiceId { get; internal set; }

        public override void Accept(CodeVisitor visitor)
        {
            visitor.VisitLoadVoiceActedDialogueLineInstruction(this);
        }
    }

    public sealed class DisplayDialogueLineInstruction : Instruction
    {
        public override void Accept(CodeVisitor visitor)
        {
            visitor.VisitDisplayDialogueLineInstruction(this);
        }
    }

    public sealed class InstantiateTextStyleInstruction : Instruction
    {
        public Expression InstanceId { get; internal set; }
        public PrimitiveTypeValue StyleDataBlockId { get; internal set; }

        public override void Accept(CodeVisitor visitor)
        {
            visitor.VisitInstantiateTextStyleInstruction(this);
        }
    }

    public sealed class ShowDialogueWindowInstruction : Instruction
    {
        public override void Accept(CodeVisitor visitor)
        {
            visitor.VisitShowDialogueWindowInstruction(this);
        }
    }

    public sealed class AwaitShowDialogueWindowInstruction : Instruction
    {
        public override void Accept(CodeVisitor visitor)
        {
            visitor.VisitAwaitShowDialogueWindowInstruction(this);
        }
    }

    public sealed class AwaitHideDialogueWindowInstruction : Instruction
    {
        public override void Accept(CodeVisitor visitor)
        {
            visitor.VisitAwaitHideDialogueWindowInstruction(this);
        }
    }

    public sealed class HideDialogueWindowInstruction : Instruction
    {
        public override void Accept(CodeVisitor visitor)
        {
            visitor.VisitCloseDialogueWindowInstruction(this);
        }
    }

    public sealed class InstanciateStringArrayInstruction : Instruction
    {
        public Expression InstanceId { get; internal set; }
        public PrimitiveTypeValue DataBlockId { get; internal set; }

        public override void Accept(CodeVisitor visitor)
        {
            visitor.VisitInstanciateStringArrayInstruction(this);
        }
    }

    public sealed class InitSubsystemInstruction : Instruction
    {
        public override void Accept(CodeVisitor visitor)
        {
            visitor.VisitInitSubsystemInstruction(this);
        }
    }

    public sealed class LoadBackgroundInstruction : Instruction
    {
        public override void Accept(CodeVisitor visitor)
        {
            visitor.VisitLoadBackgroundInstruction(this);
        }
    }

    public sealed class LoadCharacterInstruction : Instruction
    {
        public override void Accept(CodeVisitor visitor)
        {
            visitor.VisitLoadCharacterInstruction(this);
        }
    }

    public sealed class QuickSaveInstruction : Instruction
    {
        public override void Accept(CodeVisitor visitor)
        {
            visitor.VisitQuickSaveInstruction(this);
        }
    }

    public sealed class SaveStateInstruction : Instruction
    {
        public override void Accept(CodeVisitor visitor)
        {
            visitor.VisitSaveStateInstruction(this);
        }
    }

    public sealed class LoadStateInstruction : Instruction
    {
        public override void Accept(CodeVisitor visitor)
        {
            visitor.VisitLoadStateInstruction(this);
        }
    }

    public sealed class EndOfScriptInstruction : Instruction
    {
    }
}
