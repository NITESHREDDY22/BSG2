#include "pch-cpp.hpp"

#ifndef _MSC_VER
# include <alloca.h>
#else
# include <malloc.h>
#endif


#include <limits>


template <typename T1>
struct InterfaceActionInvoker1
{
	typedef void (*Action)(void*, T1, const RuntimeMethod*);

	static inline void Invoke (Il2CppMethodSlot slot, RuntimeClass* declaringInterface, RuntimeObject* obj, T1 p1)
	{
		const VirtualInvokeData& invokeData = il2cpp_codegen_get_interface_invoke_data(slot, obj, declaringInterface);
		((Action)invokeData.methodPtr)(obj, p1, invokeData.method);
	}
};

struct IIronSourceClient_t4B2D2FE506AA67640FB4BB4B3CE528AAA37BD0ED;
struct String_t;

IL2CPP_EXTERN_C RuntimeClass* IIronSourceClient_t4B2D2FE506AA67640FB4BB4B3CE528AAA37BD0ED_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* IronSource_t5B6826DEF70CE75E97EFC7E9DAB48567FA1D2DDE_il2cpp_TypeInfo_var;


IL2CPP_EXTERN_C_BEGIN
IL2CPP_EXTERN_C_END

#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
struct U3CModuleU3E_t8ED2151BB19719223CF843C883B5CF4D38CC6870 
{
};
struct IronSource_t5B6826DEF70CE75E97EFC7E9DAB48567FA1D2DDE  : public RuntimeObject
{
};
struct ValueType_t6D9B272BD21782F0A9A14F2E41F85A50E97A986F  : public RuntimeObject
{
};
struct ValueType_t6D9B272BD21782F0A9A14F2E41F85A50E97A986F_marshaled_pinvoke
{
};
struct ValueType_t6D9B272BD21782F0A9A14F2E41F85A50E97A986F_marshaled_com
{
};
struct Boolean_t09A6377A54BE2F9E6985A8149F19234FD7DDFE22 
{
	bool ___m_value;
};
struct Void_t4861ACF8F4594C3437BB48B6E56783494B843915 
{
	union
	{
		struct
		{
		};
		uint8_t Void_t4861ACF8F4594C3437BB48B6E56783494B843915__padding[1];
	};
};
struct IronSource_t5B6826DEF70CE75E97EFC7E9DAB48567FA1D2DDE_StaticFields
{
	RuntimeObject* ___client;
};
struct Boolean_t09A6377A54BE2F9E6985A8149F19234FD7DDFE22_StaticFields
{
	String_t* ___TrueString;
	String_t* ___FalseString;
};
#ifdef __clang__
#pragma clang diagnostic pop
#endif



IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR RuntimeObject* IronSourceClientFactory_CreateIronSourceClient_m83CB258857F66B4F8F3E788A387F454487BA0BDE (const RuntimeMethod* method) ;
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
#ifdef __clang__
#pragma clang diagnostic pop
#endif
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
// Method Definition Index: 53061
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void IronSource_SetConsent_m63F119CC67A53D171D9D571E09ADBCDC0D5C6F70 (bool ___0_consent, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&IIronSourceClient_t4B2D2FE506AA67640FB4BB4B3CE528AAA37BD0ED_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&IronSource_t5B6826DEF70CE75E97EFC7E9DAB48567FA1D2DDE_il2cpp_TypeInfo_var);
		s_Il2CppMethodInitialized = true;
	}
	{
		//<source_info:E:/Projects/Sigma/BSG2/Assets/GoogleMobileAds/Mediation/IronSource/Api/IronSource.cs:26>
		il2cpp_codegen_runtime_class_init_inline(IronSource_t5B6826DEF70CE75E97EFC7E9DAB48567FA1D2DDE_il2cpp_TypeInfo_var);
		RuntimeObject* L_0 = ((IronSource_t5B6826DEF70CE75E97EFC7E9DAB48567FA1D2DDE_StaticFields*)il2cpp_codegen_static_fields_for(IronSource_t5B6826DEF70CE75E97EFC7E9DAB48567FA1D2DDE_il2cpp_TypeInfo_var))->___client;
		bool L_1 = ___0_consent;
		NullCheck(L_0);
		InterfaceActionInvoker1< bool >::Invoke(0, IIronSourceClient_t4B2D2FE506AA67640FB4BB4B3CE528AAA37BD0ED_il2cpp_TypeInfo_var, L_0, L_1);
		//<source_info:E:/Projects/Sigma/BSG2/Assets/GoogleMobileAds/Mediation/IronSource/Api/IronSource.cs:27>
		return;
	}
}
// Method Definition Index: 53062
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void IronSource__cctor_m2F691335CE5C3A5D174EEEA108D8A2AA14200499 (const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&IronSource_t5B6826DEF70CE75E97EFC7E9DAB48567FA1D2DDE_il2cpp_TypeInfo_var);
		s_Il2CppMethodInitialized = true;
	}
	{
		//<source_info:E:/Projects/Sigma/BSG2/Assets/GoogleMobileAds/Mediation/IronSource/Api/IronSource.cs:21>
		//<source_info:E:/Projects/Sigma/BSG2/Assets/GoogleMobileAds/Mediation/IronSource/Api/IronSource.cs:22>
		RuntimeObject* L_0;
		L_0 = IronSourceClientFactory_CreateIronSourceClient_m83CB258857F66B4F8F3E788A387F454487BA0BDE(NULL);
		((IronSource_t5B6826DEF70CE75E97EFC7E9DAB48567FA1D2DDE_StaticFields*)il2cpp_codegen_static_fields_for(IronSource_t5B6826DEF70CE75E97EFC7E9DAB48567FA1D2DDE_il2cpp_TypeInfo_var))->___client = L_0;
		Il2CppCodeGenWriteBarrier((void**)(&((IronSource_t5B6826DEF70CE75E97EFC7E9DAB48567FA1D2DDE_StaticFields*)il2cpp_codegen_static_fields_for(IronSource_t5B6826DEF70CE75E97EFC7E9DAB48567FA1D2DDE_il2cpp_TypeInfo_var))->___client), (void*)L_0);
		return;
	}
}
#ifdef __clang__
#pragma clang diagnostic pop
#endif
