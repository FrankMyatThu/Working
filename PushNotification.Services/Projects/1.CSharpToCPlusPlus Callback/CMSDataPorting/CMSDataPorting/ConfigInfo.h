#pragma once

#include "stdafx.h"
class ConfigInfo
{
public:
	ConfigInfo(void);
	~ConfigInfo(void);

	bool ExtractArgs(int argc, _TCHAR* argv[]);
	bool LoadInitFile();

	bool IsLoaded()const {return m_bLoaded;}

	wstring GetDataSrc() const {return m_strDataSrc;}
	wstring GetDBUser() const {return m_strDBUser;}
	wstring GetDBPassword() const {return m_strDBPassword;}
	bool IsConnSupervisor() const {return m_bConnSupervisor;}
	int GetDebugLevel() const {return m_nDebugLevel;}

	wstring GetSFTPIP() const {return m_strSFTPIP;}
	wstring GetSFTPPort() const {return m_strSFTPPort;}
	wstring GetSFTwPUser() const {return m_strSFTPUser;}
	wstring GetSFTPPassword() const {return m_strSFTPPassword;}


private:
	void PrintUsage();
	int xgetopt(int argc, TCHAR *argv[], TCHAR *optstring);

	void TempInit();
	void CMSInit();


private:
	wstring m_strDataSrc;
	wstring m_strDBUser;
	wstring m_strDBPassword;
	bool m_bConnSupervisor;
	int m_nDebugLevel;

	wstring m_strSFTPIP;
	wstring m_strSFTPPort;
	wstring m_strSFTPUser;
	wstring m_strSFTPPassword;

	bool m_bLoaded;
	int m_nInitFileNo;

	int m_nNumOfLogFiles;
	int m_nLogFileSize;

private:
	TCHAR*	xoptarg;	// global argument pointer
	int	optind; 	// global argv index


// Temporary
public:
	wchar_t m_szGroupName[64];


};

