#include "Windows.h"

#ifdef __cplusplus
extern "C"
{
#endif

#define DLL __declspec(dllexport)
	typedef void(__stdcall * CSharpCallback)(int);
	DLL void DoWork_CSharpCallback(CSharpCallback _CSharpCallback, char * _TagName);

#ifdef __cplusplus
}
#endif