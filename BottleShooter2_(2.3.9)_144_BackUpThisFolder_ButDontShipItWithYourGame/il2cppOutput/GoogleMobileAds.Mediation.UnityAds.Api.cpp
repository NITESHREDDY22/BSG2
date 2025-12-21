#include "pch-cpp.hpp"

#ifndef _MSC_VER
# include <alloca.h>
#else
# include <malloc.h>
#endif


#include <limits>


template <typename T1, typename T2>
struct InterfaceActionInvoker2
{
	typedef void (*Action)(void*, T1, T2, const RuntimeMethod*);

	static inline void Invoke (Il2CppMethodSlot slot, RuntimeClass* declaringInterface, RuntimeObject* obj, T1 p1, T2 p2)
	{
		const VirtualInvokeData& invokeData = il2cpp_codegen_get_interface_invoke_data(slot, obj, declaringInterface);
		((Action)invokeData.methodPtr)(obj, p1, p2, invokeData.method);
	}
};

struct IUnityAdsClient_t822362A0D3BE071413BE4C523F8E8EA9567EA630;
struct String_t;
struct UnityAdsClient_t573FCBD6AFA3AA04E6F7D4E91B32C0D022046EF6;

IL2CPP_EXTERN_C RuntimeClass* IUnityAdsClient_t822362A0D3BE071413BE4C523F8E8EA9567EA630_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* UnityAdsClient_t573FCBD6AFA3AA04E6F7D4E91B32C0D022046EF6_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* UnityAds_t13904FB5EDF559D67E845230A99EB90F1AAE49E1_il2cpp_TypeInfo_var;


IL2CPP_EXTERN_C_BEGIN
IL2CPP_EXTERN_C_END

#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
struct U3CModuleU3E_tEDCD21B2773F5CCC6F62D5B208EB4C1098C60466 
{
};
struct String_t  : public RuntimeObject
{
	int32_t ____stringLength;
	Il2CppChar ____firstChar;
};
struct UnityAds_t13904FB5EDF559D67E845230A99EB90F1AAE49E1  : public RuntimeObject
{
};
struct UnityAdsClient_t573FCBD6AFA3AA04E6F7D4E91B32C0D022046EF6  : public RuntimeObject
{
};
struct UnityAdsClientFactory_t11CA4F16CC40D87ECED7052E048C20AB89C9F870  : public RuntimeObject
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
struct String_t_StaticFields
{
	String_t* ___Empty;
};
struct UnityAds_t13904FB5EDF559D67E845230A99EB90F1AAE49E1_StaticFields
{
	RuntimeObject* ___client;
};
struct UnityAdsClient_t573FCBD6AFA3AA04E6F7D4E91B32C0D022046EF6_StaticFields
{
	UnityAdsClient_t573FCBD6AFA3AA04E6F7D4E91B32C0D022046EF6* ___instance;
};
struct Boolean_t09A6377A54BE2F9E6985A8149F19234FD7DDFE22_StaticFields
{
	String_t* ___TrueString;
	String_t* ___FalseString;
};
#ifdef __clang__
#pragma clang diagnostic pop
#endif



IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR UnityAdsClient_t573FCBD6AFA3AA04E6F7D4E91B32C0D022046EF6* UnityAdsClient_get_Instance_m67735BD45A329D6805B7FBD840E584DBA6CF31C2_inline (const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR RuntimeObject* UnityAdsClientFactory_CreateUnityAdsClient_m338877F8BF001B3816F3518C24A2DD8AA1A7716B (const RuntimeMethod* method) ;
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
// Method Definition Index: 53063
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR RuntimeObject* UnityAdsClientFactory_CreateUnityAdsClient_m338877F8BF001B3816F3518C24A2DD8AA1A7716B (const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&UnityAdsClient_t573FCBD6AFA3AA04E6F7D4E91B32C0D022046EF6_il2cpp_TypeInfo_var);
		s_Il2CppMethodInitialized = true;
	}
	{
		//<source_info:E:/Projects/Sigma/BSG2/Assets/GoogleMobileAds/Mediation/UnityAds/Api/UnityAdsClientFactory.cs:26>
		il2cpp_codegen_runtime_class_init_inline(UnityAdsClient_t573FCBD6AFA3AA04E6F7D4E91B32C0D022046EF6_il2cpp_TypeInfo_var);
		UnityAdsClient_t573FCBD6AFA3AA04E6F7D4E91B32C0D022046EF6* L_0;
		L_0 = UnityAdsClient_get_Instance_m67735BD45A329D6805B7FBD840E584DBA6CF31C2_inline(NULL);
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
// Method Definition Index: 53064
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void UnityAds_SetConsentMetaData_m6750BC3ACB6E2768D0C95A29ECD4E08EB8E50450 (String_t* ___0_key, bool ___1_metaDataValue, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&IUnityAdsClient_t822362A0D3BE071413BE4C523F8E8EA9567EA630_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&UnityAds_t13904FB5EDF559D67E845230A99EB90F1AAE49E1_il2cpp_TypeInfo_var);
		s_Il2CppMethodInitialized = true;
	}
	{
		//<source_info:E:/Projects/Sigma/BSG2/Assets/GoogleMobileAds/Mediation/UnityAds/Api/UnityAds.cs:26>
		il2cpp_codegen_runtime_class_init_inline(UnityAds_t13904FB5EDF559D67E845230A99EB90F1AAE49E1_il2cpp_TypeInfo_var);
		RuntimeObject* L_0 = ((UnityAds_t13904FB5EDF559D67E845230A99EB90F1AAE49E1_StaticFields*)il2cpp_codegen_static_fields_for(UnityAds_t13904FB5EDF559D67E845230A99EB90F1AAE49E1_il2cpp_TypeInfo_var))->___client;
		String_t* L_1 = ___0_key;
		bool L_2 = ___1_metaDataValue;
		NullCheck(L_0);
		InterfaceActionInvoker2< String_t*, bool >::Invoke(0, IUnityAdsClient_t822362A0D3BE071413BE4C523F8E8EA9567EA630_il2cpp_TypeInfo_var, L_0, L_1, L_2);
		//<source_info:E:/Projects/Sigma/BSG2/Assets/GoogleMobileAds/Mediation/UnityAds/Api/UnityAds.cs:27>
		return;
	}
}
// Method Definition Index: 53065
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void UnityAds__cctor_mF506437B549C42B08AB80AF8901A0929AFB8098D (const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&UnityAds_t13904FB5EDF559D67E845230A99EB90F1AAE49E1_il2cpp_TypeInfo_var);
		s_Il2CppMethodInitialized = true;
	}
	{
		//<source_info:E:/Projects/Sigma/BSG2/Assets/GoogleMobileAds/Mediation/UnityAds/Api/UnityAds.cs:21>
		//<source_info:E:/Projects/Sigma/BSG2/Assets/GoogleMobileAds/Mediation/UnityAds/Api/UnityAds.cs:22>
		RuntimeObject* L_0;
		L_0 = UnityAdsClientFactory_CreateUnityAdsClient_m338877F8BF001B3816F3518C24A2DD8AA1A7716B(NULL);
		((UnityAds_t13904FB5EDF559D67E845230A99EB90F1AAE49E1_StaticFields*)il2cpp_codegen_static_fields_for(UnityAds_t13904FB5EDF559D67E845230A99EB90F1AAE49E1_il2cpp_TypeInfo_var))->___client = L_0;
		Il2CppCodeGenWriteBarrier((void**)(&((UnityAds_t13904FB5EDF559D67E845230A99EB90F1AAE49E1_StaticFields*)il2cpp_codegen_static_fields_for(UnityAds_t13904FB5EDF559D67E845230A99EB90F1AAE49E1_il2cpp_TypeInfo_var))->___client), (void*)L_0);
		return;
	}
}
#ifdef __clang__
#pragma clang diagnostic pop
#endif
// Method Definition Index: 53044
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR UnityAdsClient_t573FCBD6AFA3AA04E6F7D4E91B32C0D022046EF6* UnityAdsClient_get_Instance_m67735BD45A329D6805B7FBD840E584DBA6CF31C2_inline (const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&UnityAdsClient_t573FCBD6AFA3AA04E6F7D4E91B32C0D022046EF6_il2cpp_TypeInfo_var);
		s_Il2CppMethodInitialized = true;
	}
	{
		//<source_info:E:/Projects/Sigma/BSG2/Assets/GoogleMobileAds/Mediation/UnityAds/Platforms/Android/UnityAdsClient.cs:35>
		il2cpp_codegen_runtime_class_init_inline(UnityAdsClient_t573FCBD6AFA3AA04E6F7D4E91B32C0D022046EF6_il2cpp_TypeInfo_var);
		UnityAdsClient_t573FCBD6AFA3AA04E6F7D4E91B32C0D022046EF6* L_0 = ((UnityAdsClient_t573FCBD6AFA3AA04E6F7D4E91B32C0D022046EF6_StaticFields*)il2cpp_codegen_static_fields_for(UnityAdsClient_t573FCBD6AFA3AA04E6F7D4E91B32C0D022046EF6_il2cpp_TypeInfo_var))->___instance;
		return L_0;
	}
}
