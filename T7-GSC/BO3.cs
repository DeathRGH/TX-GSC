﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T7_GSC.BO3
{
	namespace _01_00
	{
		class Addresses
		{
			public static ulong s_ScriptParseTreePool = 0x00000000087A8100;
			public static ulong DB_FindXAssetHeader = 0x0000000000BEEE50; //int DB_FindXAssetHeader(int type, const char *name, bool errorIfMissing, int waitTime)
		}
	}

	namespace _01_33
	{
		class Addresses
		{
			public static ulong s_ScriptParseTreePool = 0x000000000547EBC0;
			public static ulong DB_FindXAssetHeader = 0x0000000000C591B0; //int DB_FindXAssetHeader(int type, const char *name, bool errorIfMissing, int waitTime)
		}
	}

	enum XAssetType : int
	{
		ASSET_TYPE_MATERIAL = 0x06,
		ASSET_TYPE_IMAGE = 0x09,
		ASSET_TYPE_FX = 0x26,
		ASSET_TYPE_STRINGTABLE = 0x30,
		ASSET_TYPE_SCRIPTPARSETREE = 0x36,
		ASSET_TYPE_TTF = 0x50
	}

	public enum ScriptOpCode : byte
	{
		VM_OP_End_Handler = 0x0,
		VM_OP_Return_Handler = 0x1,
		VM_OP_GetUndefined_Handler = 0x2,
		VM_OP_GetZero_Handler = 0x3,
		VM_OP_GetByte_Handler = 0x4,
		VM_OP_GetNegByte_Handler = 0x5,
		VM_OP_GetUnsignedShort_Handler = 0x6,
		VM_OP_GetNegUnsignedShort_Handler = 0x7,
		VM_OP_GetInteger_Handler = 0x8,
		VM_OP_GetFloat_Handler = 0x9,
		VM_OP_GetString_Handler = 0xA,
		VM_OP_GetIString_Handler = 0xB,
		VM_OP_GetVector_Handler = 0xC,
		VM_OP_GetLevelObject_Handler = 0xD,
		VM_OP_GetAnimObject_Handler = 0xE,
		VM_OP_GetSelf_Handler = 0xF,
		VM_OP_GetLevel_Handler = 0x10,
		VM_OP_GetGame_Handler = 0x11,
		VM_OP_GetAnim_Handler = 0x12,
		VM_OP_GetAnimation_Handler = 0x13,
		VM_OP_GetGameRef_Handler = 0x14,
		VM_OP_GetFunction_Handler = 0x15,
		//CreateLocalVariable = 0x16,
		VM_OP_SafeCreateLocalVariables_Handler = 0x17,
		//RemoveLocalVariables = 0x18,
		VM_OP_EvalLocalVariableCached_Handler = 0x19,
		VM_OP_EvalArray_Handler = 0x1A,
		//EvalLocalArrayRefCached = 0x1B,
		VM_OP_EvalArrayRef_Handler = 0x1C,
		VM_OP_ClearArray_Handler = 0x1D,
		VM_OP_EmptyArray_Handler = 0x1E,
		VM_OP_GetSelfObject_Handler = 0x1F,
		VM_OP_EvalFieldVariable_Handler = 0x20,
		VM_OP_EvalFieldVariableRef_Handler = 0x21,
		//ClearFieldVariable = 0x22,
		//SafeSetVariableFieldCached = 0x23,
		VM_OP_SetWaittillVariableFieldCached_Handler = 0x24,
		VM_OP_ClearParams_Handler = 0x25,
		VM_OP_CheckClearParams_Handler = 0x26,
		VM_OP_EvalLocalVariableRefCached_Handler = 0x27,
		VM_OP_SetVariableField_Handler = 0x28,
		VM_OP_CallBuiltin_Handler = 0x29,
		VM_OP_CallBuiltinMethod_Handler = 0x2A,
		VM_OP_Wait_Handler = 0x2B,
		VM_OP_WaitTillFrameEnd_Handler = 0x2C,
		VM_OP_PreScriptCall_Handler = 0x2D,
		VM_OP_ScriptFunctionCall_Handler = 0x2E,
		VM_OP_ScriptFunctionCallPointer_Handler = 0x2F,
		VM_OP_ScriptMethodCall_Handler = 0x30,
		VM_OP_ScriptMethodCallPointer_Handler = 0x31,
		VM_OP_ScriptThreadCall_Handler = 0x32,
		VM_OP_ScriptThreadCallPointer_Handler = 0x33,
		VM_OP_ScriptMethodThreadCall_Handler = 0x34,
		VM_OP_ScriptMethodThreadCallPointer_Handler = 0x35,
		VM_OP_DecTop_Handler = 0x36,
		VM_OP_CastFieldObject_Handler = 0x37,
		VM_OP_CastBool_Handler = 0x38,
		VM_OP_BoolNot_Handler = 0x39,
		VM_OP_BoolComplement_Handler = 0x3A,
		VM_OP_JumpOnFalse_Handler = 0x3B,
		VM_OP_JumpOnTrue_Handler = 0x3C,
		VM_OP_JumpOnFalseExpr_Handler = 0x3D,
		VM_OP_JumpOnTrueExpr_Handler = 0x3E,
		VM_OP_Jump_Handler = 0x3F,
		//JumpBack = 0x40,
		VM_OP_Inc_Handler = 0x41,
		VM_OP_Dec_Handler = 0x42,
		VM_OP_Bit_Or_Handler = 0x43,
		VM_OP_Bit_Xor_Handler = 0x44,
		VM_OP_Bit_And_Handler = 0x45,
		VM_OP_Equal_Handler = 0x46,
		VM_OP_NotEqual_Handler = 0x47,
		VM_OP_LessThan_Handler = 0x48,
		VM_OP_GreaterThan_Handler = 0x49,
		VM_OP_LessThanOrEqualTo_Handler = 0x4A,
		VM_OP_GreaterThanOrEqualTo_Handler = 0x4B,
		VM_OP_ShiftLeft_Handler = 0x4C,
		VM_OP_ShiftRight_Handler = 0x4D,
		VM_OP_Plus_Handler = 0x4E,
		VM_OP_Minus_Handler = 0x4F,
		VM_OP_Multiply_Handler = 0x50,
		VM_OP_Divide_Handler = 0x51,
		VM_OP_Modulus_Handler = 0x52,
		VM_OP_SizeOf_Handler = 0x53,
		VM_OP_WaitTillMatch_Handler = 0x54,
		VM_OP_WaitTill_Handler = 0x55,
		VM_OP_Notify_Handler = 0x56,
		VM_OP_EndOnCallback_Handler = 0x57,
		//VoidCodePos = 0x58,
		VM_OP_Switch_Handler = 0x59,
		VM_OP_EndSwitch_Handler = 0x5A,
		VM_OP_Vector_Handler = 0x5B,
		VM_OP_GetHash_Handler = 0x5C,
		VM_OP_RealWait_Handler = 0x5D,
		VM_OP_VectorConstant_Handler = 0x5E,
		VM_OP_IsDefined_Handler = 0x5F,
		VM_OP_VectorScale_Handler = 0x60,
		//AnglesToUp = 0x61,
		//AnglesToRight = 0x62,
		//AnglesToForward = 0x63,
		//AngleClamp180 = 0x64,
		//VectorToAngles = 0x65,
		//Abs = 0x66,
		VM_OP_GetTime_Handler = 0x67,
		//GetDvar = 0x68,
		//GetDvarInt = 0x69,
		//GetDvarFloat = 0x6A,
		//GetDvarVector = 0x6B,
		//GetDvarColorRed = 0x6C,
		//GetDvarColorGreen = 0x6D,
		//GetDvarColorBlue = 0x6E,
		//GetDvarColorAlpha = 0x6F,
		VM_OP_FirstArrayKey_Handler = 0x70,
		VM_OP_NextArrayKey_Handler = 0x71,
		VM_OP_ProfileStart_Handler = 0x72,
		VM_OP_ProfileStop_Handler = 0x73,
		VM_OP_SafeDecTop_Handler = 0x74,
		VM_OP_Nop_Handler = 0x75,
		VM_OP_Abort_Handler = 0x76,
		//Obj = 0x77,
		//ThreadObject = 0x78,
		//EvalLocalVariable = 0x79,
		//EvalLocalVariableRef = 0x7A,
		VM_OP_DevblockBegin_Handler = 0x7B,
		//DevblockEnd = 0x7C,
		VM_OP_Breakpoint_Handler = 0x7D,
		VM_OP_AutoBreakpoint_Handler = 0x7E,
		VM_OP_ErrorBreakpoint_Handler = 0x7F,
		VM_OP_WatchBreakpoint_Handler = 0x80,
		VM_OP_NotifyBreakpoint_Handler = 0x81,
		//GetObjectType,
		//WaitRealTime,
		VM_OP_GetWorldObject_Handler = 0x84,
		VM_OP_GetClassesObject_Handler = 0x85,
		VM_OP_ScriptFunctionCallClass_Handler = 0x86,
		//Bit_Not,
		VM_OP_GetWorld_Handler = 0x88,
		VM_OP_EvalLevelFieldVariable_Handler = 0x89,
		VM_OP_EvalLevelFieldVariableRef_Handler = 0x8A,
		VM_OP_EvalSelfFieldVariable_Handler = 0x8B,
		VM_OP_EvalSelfFieldVariableRef_Handler = 0x8C,
		VM_OP_SuperEqual_Handler = 0x8D,
		VM_OP_SuperNotEqual_Handler = 0x8E,
		//Count,

		VM_OP_InvalidOpcode_Handler = 0xFF,
	}
}
