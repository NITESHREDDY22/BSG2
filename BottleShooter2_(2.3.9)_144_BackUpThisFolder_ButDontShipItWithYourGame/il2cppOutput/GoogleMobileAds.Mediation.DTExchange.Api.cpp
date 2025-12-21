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

struct IDTExchangeClient_t40C7673524A173A275C316A9F4F1BCF9D317FFF7;
struct String_t;

IL2CPP_EXTERN_C RuntimeClass* DTExchange_t06C0CD645E8D4124F227F280A5B422A7F3B155B0_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* IDTExchangeClient_t40C7673524A173A275C316A9F4F1BCF9D317FFF7_il2cpp_TypeInfo_var;


IL2CPP_EXTERN_C_BEGIN
IL2CPP_EXTERN_C_END

#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
struct U3CModuleU3E_t48530CB633BA5085F3ADA53342FD3E53CCCAFC62 
{
};
struct DTExchange_t06C0CD645E8D4124F227F280A5B422A7F3B155B0  : public RuntimeObject
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
struct DTExchange_t06C0CD645E8D4124F227F280A5B422A7F3B155B0_StaticFields
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



IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR RuntimeObject* DTExchangeClientFactory_CreateDTExchangeClient_m6BEA4C4CAE394CE458FA4906449C08181FAA9382 (const RuntimeMethod* method) ;
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
// Method Definition Index: 53055
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void DTExchange_SetGDPRConsent_m032BC3A631951F4F1D690A283FF4F48EC8230BAA (bool ___0_consent, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&DTExchange_t06C0CD645E8D4124F227F280A5B422A7F3B155B0_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&IDTExchangeClient_t40C7673524A173A275C316A9F4F1BCF9D317FFF7_il2cpp_TypeInfo_var);
		s_Il2CppMethodInitialized = true;
	}
	{
		//<source_info:E:/Projects/Sigma/BSG2/Assets/GoogleMobileAds/Mediation/DTExchange/Api/DTExchange.cs:28>
		il2cpp_codegen_runtime_class_init_inline(DTExchange_t06C0CD645E8D4124F227F280A5B422A7F3B155B0_il2cpp_TypeInfo_var);
		RuntimeObject* L_0 = ((DTExchange_t06C0CD645E8D4124F227F280A5B422A7F3B155B0_StaticFields*)il2cpp_codegen_static_fields_for(DTExchange_t06C0CD645E8D4124F227F280A5B422A7F3B155B0_il2cpp_TypeInfo_var))->___client;
		bool L_1 = ___0_consent;
		NullCheck(L_0);
		InterfaceActionInvoker1< bool >::Invoke(0, IDTExchangeClient_t40C7673524A173A275C316A9F4F1BCF9D317FFF7_il2cpp_TypeInfo_var, L_0, L_1);
		//<source_info:E:/Projects/Sigma/BSG2/Assets/GoogleMobileAds/Mediation/DTExchange/Api/DTExchange.cs:29>
		return;
	}
}
// Method Definition Index: 53056
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void DTExchange__cctor_mBBA9DB8C28EAAB0E573F98AC4AB27AB2487E36EE (const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&DTExchange_t06C0CD645E8D4124F227F280A5B422A7F3B155B0_il2cpp_TypeInfo_var);
		s_Il2CppMethodInitialized = true;
	}
	{
		//<source_info:E:/Projects/Sigma/BSG2/Assets/GoogleMobileAds/Mediation/DTExchange/Api/DTExchange.cs:23>
		//<source_info:E:/Projects/Sigma/BSG2/Assets/GoogleMobileAds/Mediation/DTExchange/Api/DTExchange.cs:24>
		RuntimeObject* L_0;
		L_0 = DTExchangeClientFactory_CreateDTExchangeClient_m6BEA4C4CAE394CE458FA4906449C08181FAA9382(NULL);
		((DTExchange_t06C0CD645E8D4124F227F280A5B422A7F3B155B0_StaticFields*)il2cpp_codegen_static_fields_for(DTExchange_t06C0CD645E8D4124F227F280A5B422A7F3B155B0_il2cpp_TypeInfo_var))->___client = L_0;
		Il2CppCodeGenWriteBarrier((void**)(&((DTExchange_t06C0CD645E8D4124F227F280A5B422A7F3B155B0_StaticFields*)il2cpp_codegen_static_fields_for(DTExchange_t06C0CD645E8D4124F227F280A5B422A7F3B155B0_il2cpp_TypeInfo_var))->___client), (void*)L_0);
		return;
	}
}
#ifdef __clang__
#pragma clang diagnostic pop
#endif
