// CMSDataPorting.cpp : Defines the entry point for the console application.

#include "stdafx.h"
#include "CSharpAdapter.h"
#include <stdio.h>

// Global var
DataFileInfo g_pointlist;
ConfigInfo g_cfg;

DLL void DoWork_CSharpCallback(CSharpCallback _CSharpCallback, char * _TagName)
{	
	bool bRet;
	RTDBHandler rtdbHandler;	
	bRet = rtdbHandler.ConnRTDB(_CSharpCallback, _TagName);
	bRet = rtdbHandler.SetNotify();
	
	int nRet;	
	while (1)
	{
		nRet = RTDBWaitEvent(200);
		if ((nRet != 0) && (nRet != E_TIMED_OUT)) {			
			rtdbHandler.reset_rtdb();
			break;
		}
	}

	return;
}