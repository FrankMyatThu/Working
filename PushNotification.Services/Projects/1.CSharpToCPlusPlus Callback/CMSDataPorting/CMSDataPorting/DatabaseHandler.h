#pragma once

#include <SQL.H>
#include <SQLTYPES.H>
#include <odbc_lib.h>
#include <sqlext.h>
#include <hts_call.h>


class DatabaseHandler
{
public:
	DatabaseHandler(wstring dataSrc, wstring username, wstring password);
	~DatabaseHandler(void);

	bool LoadPointInfo(DataFileInfo &ptlist);
private:

	bool ConnDB();
	void check_sql_msg(int32_t ret, SQLHENV hEnv, SQLHDBC hDBC, SQLHSTMT hStmt, struct _odbc_errmsg * odbc_errmsg);
	void odbc_display_msg(struct _odbc_errmsg * odbc_errmsg);

	bool LoadFileTypeList(vector<FileType> &vecFileTypeList);
//	bool LoadPointsByType(int PointId, int Ix, FileType *pFileType, list<PointInfo> &ptListOut);
//	bool LoadPointsByType(int PointId, int Ix, FileType *pFileType, DataFileInfo &ptList);
	bool LoadPointsByType(FileType *pFileType, DataFileInfo &ptList);

private:

	wstring m_strDataSrc;
	wstring m_strUserName;
	wstring m_strPassword;

	SQLHSTMT m_hStmt; /* ODBC Statement Handler */
	SQLHDBC m_hDBC;  /* ODBC Connection Handler */
	SQLHENV m_hEnv;  /* ODBC Environment Handler */

	struct _odbc_errmsg last_odbc_errmsg;
	struct timeval last_errmsg_update_time; /* The last error message update time from Sybase server */
	int32_t last_errmsg_interval;		/* The interval to print out repeated error message in minutes, default DEF_ERR_MSG_INTERVAL */
	u_int8_t errmsg_update_flag;	/* The global flag to indicate whether to print out the error message or not */

	wchar_t m_szODBCDBName[DB_MAX_TEXT_LEN];	/* DB name when using ODBC library */
	wchar_t m_szODBCDBMSName[DB_MAX_TEXT_LEN];/* DBMS Name when using ODBC library */
	wchar_t m_szODBCServerName[DB_MAX_TEXT_LEN];		/* Server name when using ODBC lib */

	bool m_bConnected;

};

