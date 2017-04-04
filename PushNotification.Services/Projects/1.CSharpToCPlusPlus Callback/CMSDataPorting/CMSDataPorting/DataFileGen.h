#pragma once

/* Define the bit representation for the data quality block */
#define DQ_MANUALLYSET		0x0001  /* Data Quality (Manually Set) */
#define DQ_BLOCKED			0x0002  /* Data Quality (Blocked Mode) */
#define DQ_CTRLINHIBIT		0x0004  /* Data Quality (Tagged Mode) */
#define DQ_TELEMFAIL		0x0008  /* Data Quality (Telemetry Fail) */
#define DQ_TESTMODE			0x0010  /* Data Quality (Test Mode) */
#define DQ_CALCFAIL			0x0020  /* Data Quality (Calc Fail) */
#define DQ_LINKFAIL			0x0040  /* Data Quality (Link Fail) */
#define DQ_POINTFAULT		0x0080  /* Data Quality (Point Fault) */
#define DQ_ALARMINHIBIT		0x0100	/* Data Quality (Alarm Inhibit) */
#define DQ_UNREASONABLE		0x0200  /* Data Quality (Unreasonable) */
#define DQ_FORCETOZERO		0x0400	/* Data Quality (ForceToZero) */
#define DQ_NOTREFRESH		0x0800	/* Data Quality (Not Refresh) */
#define DQ_CHATTERING		0x1000	/* Data Quality (Chattering) */
#define DQ_WARNING			0x2000	/* Data Quality (Warning) */
#define DQ_INFORMATION		0x4000	/* Data Quality (Information) */
#define DQ_PERMITTOWORK		0x8000	/* Data Quality (PermitToWork) */

class DataFileGen
{
public:
	DataFileGen(ConfigInfo &cfgInfo);
	~DataFileGen(void);

	bool GenerateByFileType(wstring strFileType);
	bool GenerateAll(vector<FileType> &vecFileTypeList);
	bool SendFiles();

private:
	bool WriteLine(wstring strFileName, wstring strLine, bool bTruncate=false);
	wstring GetDQString(unsigned int nDQ);

	const unsigned m_cnTagsPerFile;

	vector<wstring> m_vecGeneratedFileNames;

	ConfigInfo &m_rConfigInfo;

	wstring m_strLocalFilePath;
	wstring m_strRemoteFilePath;
	wstring m_strWinSCPPath;

};

