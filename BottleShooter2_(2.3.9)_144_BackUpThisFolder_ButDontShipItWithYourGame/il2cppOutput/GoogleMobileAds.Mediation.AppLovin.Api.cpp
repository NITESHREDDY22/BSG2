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

struct AppLovinClient_t992E9401D2473450B09ED70E1119FC56E3577638;
struct IAppLovinClient_tF8DED5C8119A256747DC923E16780B61E07C24C2;
struct String_t;

IL2CPP_EXTERN_C RuntimeClass* AppLovinClient_t992E9401D2473450B09ED70E1119FC56E3577638_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* AppLovin_tA0CCF5779C045B9E8636E6967DE80A1575F4C1C8_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* IAppLovinClient_tF8DED5C8119A256747DC923E16780B61E07C24C2_il2cpp_TypeInfo_var;


IL2CPP_EXTERN_C_BEGIN
IL2CPP_EXTERN_C_END

#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
struct U3CModuleU3E_t467AC9319138C9F325E910545FBD0B87DD740407 
{
};
struct AppLovin_tA0CCF5779C045B9E8636E6967DE80A1575F4C1C8  : public RuntimeObject
{
};
struct AppLovinClient_t992E9401D2473450B09ED70E1119FC56E3577638  : public RuntimeObject
{
};
struct AppLovinClientFactory_t667766DB299752298BB40AB985E25DBD08E48F1E  : public RuntimeObject
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
struct AppLovin_tA0CCF5779C045B9E8636E6967DE80A1575F4C1C8_StaticFields
{
	RuntimeObject* ___client;
};
struct AppLovinClient_t992E9401D2473450B09ED70E1119FC56E3577638_StaticFields
{
	AppLovinClient_t992E9401D2473450B09ED70E1119FC56E3577638* ___instance;
};
struct Boolean_t09A6377A54BE2F9E6985A8149F19234FD7DDFE22_StaticFields
{
	String_t* ___TrueString;
	String_t* ___FalseString;
};
#ifdef __clang__
#pragma clang diagnostic pop
#endif



IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR AppLovinClient_t992E9401D2473450B09ED70E1119FC56E3577638* AppLovinClient_get_Instance_m64347CAD91B05FDEA244D5F71C012A7F59578434_inline (const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR RuntimeObject* AppLovinClientFactory_AppLovinInstance_mC3E85BCBDAE836274139F5B1961244749193A00F (const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR RuntimeObject* AppLovin_GetAppLovinClient_m1A62B600BE996A7BFD82213F6230AE3D3B968365 (const RuntimeMethod* method) ;
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
// Method Definition Index: 53047
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR RuntimeObject* AppLovinClientFactory_AppLovinInstance_mC3E85BCBDAE836274139F5B1961244749193A00F (const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&AppLovinClient_t992E9401D2473450B09ED70E1119FC56E3577638_il2cpp_TypeInfo_var);
		s_Il2CppMethodInitialized = true;
	}
	{
		//<source_info:E:/Projects/Sigma/BSG2/Assets/GoogleMobileAds/Mediation/AppLovin/Api/AppLovinClientFactory.cs:28>
		il2cpp_codegen_runtime_class_init_inline(AppLovinClient_t992E9401D2473450B09ED70E1119FC56E3577638_il2cpp_TypeInfo_var);
		AppLovinClient_t992E9401D2473450B09ED70E1119FC56E3577638* L_0;
		L_0 = AppLovinClient_get_Instance_m64347CAD91B05FDEA244D5F71C012A7F59578434_inline(NULL);
		return L_0;
	}
}
#ifdef __clang__
#pragma clang diagnostic pop
#endif
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
// Method Definition Index: 53048
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void AppLovin_SetHasUserConsent_m3BC657B602A7F9DA094024BB5E21CCF5ECD6D6DF (bool ___0_hasUserConsent, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&AppLovin_tA0CCF5779C045B9E8636E6967DE80A1575F4C1C8_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&IAppLovinClient_tF8DED5C8119A256747DC923E16780B61E07C24C2_il2cpp_TypeInfo_var);
		s_Il2CppMethodInitialized = true;
	}
	{
		//<source_info:E:/Projects/Sigma/BSG2/Assets/GoogleMobileAds/Mediation/AppLovin/Api/AppLovin.cs:25>
		il2cpp_codegen_runtime_class_init_inline(AppLovin_tA0CCF5779C045B9E8636E6967DE80A1575F4C1C8_il2cpp_TypeInfo_var);
		RuntimeObject* L_0 = ((AppLovin_tA0CCF5779C045B9E8636E6967DE80A1575F4C1C8_StaticFields*)il2cpp_codegen_static_fields_for(AppLovin_tA0CCF5779C045B9E8636E6967DE80A1575F4C1C8_il2cpp_TypeInfo_var))->___client;
		bool L_1 = ___0_hasUserConsent;
		NullCheck(L_0);
		InterfaceActionInvoker1< bool >::Invoke(0, IAppLovinClient_tF8DED5C8119A256747DC923E16780B61E07C24C2_il2cpp_TypeInfo_var, L_0, L_1);
		//<source_info:E:/Projects/Sigma/BSG2/Assets/GoogleMobileAds/Mediation/AppLovin/Api/AppLovin.cs:26>
		return;
	}
}
// Method Definition Index: 53049
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR RuntimeObject* AppLovin_GetAppLovinClient_m1A62B600BE996A7BFD82213F6230AE3D3B968365 (const RuntimeMethod* method) 
{
	{
		//<source_info:E:/Projects/Sigma/BSG2/Assets/GoogleMobileAds/Mediation/AppLovin/Api/AppLovin.cs:35>
		RuntimeObject* L_0;
		L_0 = AppLovinClientFactory_AppLovinInstance_mC3E85BCBDAE836274139F5B1961244749193A00F(NULL);
		return L_0;
	}
}
// Method Definition Index: 53050
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void AppLovin__cctor_mABBAAA2DB83B8DAE8F5C085C24D5FC5F26D90D72 (const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&AppLovin_tA0CCF5779C045B9E8636E6967DE80A1575F4C1C8_il2cpp_TypeInfo_var);
		s_Il2CppMethodInitialized = true;
	}
	{
		//<source_info:E:/Projects/Sigma/BSG2/Assets/GoogleMobileAds/Mediation/AppLovin/Api/AppLovin.cs:21>
		RuntimeObject* L_0;
		L_0 = AppLovin_GetAppLovinClient_m1A62B600BE996A7BFD82213F6230AE3D3B968365(NULL);
		((AppLovin_tA0CCF5779C045B9E8636E6967DE80A1575F4C1C8_StaticFields*)il2cpp_codegen_static_fields_for(AppLovin_tA0CCF5779C045B9E8636E6967DE80A1575F4C1C8_il2cpp_TypeInfo_var))->___client = L_0;
		Il2CppCodeGenWriteBarrier((void**)(&((AppLovin_tA0CCF5779C045B9E8636E6967DE80A1575F4C1C8_StaticFields*)il2cpp_codegen_static_fields_for(AppLovin_tA0CCF5779C045B9E8636E6967DE80A1575F4C1C8_il2cpp_TypeInfo_var))->___client), (void*)L_0);
		return;
	}
}
#ifdef __clang__
#pragma clang diagnostic pop
#endif
// Method Definition Index: 53038
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR AppLovinClient_t992E9401D2473450B09ED70E1119FC56E3577638* AppLovinClient_get_Instance_m64347CAD91B05FDEA244D5F71C012A7F59578434_inline (const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&AppLovinClient_t992E9401D2473450B09ED70E1119FC56E3577638_il2cpp_TypeInfo_var);
		s_Il2CppMethodInitialized = true;
	}
	{
		//<source_info:E:/Projects/Sigma/BSG2/Assets/GoogleMobileAds/Mediation/AppLovin/Platforms/Android/AppLovinClient.cs:34>
		il2cpp_codegen_runtime_class_init_inline(AppLovinClient_t992E9401D2473450B09ED70E1119FC56E3577638_il2cpp_TypeInfo_var);
		AppLovinClient_t992E9401D2473450B09ED70E1119FC56E3577638* L_0 = ((AppLovinClient_t992E9401D2473450B09ED70E1119FC56E3577638_StaticFields*)il2cpp_codegen_static_fields_for(AppLovinClient_t992E9401D2473450B09ED70E1119FC56E3577638_il2cpp_TypeInfo_var))->___instance;
		return L_0;
	}
}
