#include "StdAfx.h"
#include "DatabaseHandler.h"


DatabaseHandler::DatabaseHandler(wstring dataSrc, wstring username, wstring password)
	:m_strDataSrc(dataSrc),
	m_strUserName(username),
	m_strPassword(password),
	m_bConnected(false)
{
	bool bRet = ConnDB();
	if (!bRet)
	{
		wcout<<_T("Connect to SQL Server failed")<<endl;
	}
	else
	{
		wcout<<_T("Database connected!")<<endl;
	}
}


DatabaseHandler::~DatabaseHandler(void)
{
}

bool DatabaseHandler::LoadPointInfo( DataFileInfo &ptlist )
{
	int nRet;

	nRet = LoadFileTypeList(ptlist.m_vecFileTypeList);
	if (!nRet)
	{
		wcout<<_T("Failed to load FileType list from database!")<<endl;
		return false;
	}

	vector<FileType>::iterator pos = ptlist.m_vecFileTypeList.begin();
	while (pos != ptlist.m_vecFileTypeList.end())
	{
//		nRet = LoadPointsByType(pos->PointId, pos->Ix, &(*pos), ptlist.m_listPointList);
//		nRet = LoadPointsByType(pos->PointId, pos->Ix, &(*pos), ptlist);
		nRet = LoadPointsByType(&(*pos), ptlist);

		if (!nRet)
		{
			wcout<<_T("Failed to load points of type ")<<(*pos).strFileType<<endl;
			return false;
		}
		else
		{
			wcout<<pos->nPointCnt<<_T(" points under file type ")<<(*pos).strFileType<<_T(" are loaded successfully")<<endl;
		}
		pos++;
	}

	return true;
}

bool DatabaseHandler::ConnDB()
{
	struct _odbc_errmsg odbc_errmsg;
	int32_t ret;
	int16_t info_size;
	char szODBCDriverVer[DB_MAX_NAME_LEN]; /* ODBC Driver version */
	memset(szODBCDriverVer, 0, DB_MAX_NAME_LEN);

	
    /* SQLAllocEnv() function is use to allocated memory for an
    ** environment handle. 
    ** - Allocate An Environment Handle. */
    ret = SQLAllocEnv(&m_hEnv);

    if (ret < SQL_SUCCESS) {
        wcout<<_T("odbc_startup@Failed to allocate environment handle")<<endl;
        check_sql_msg(ret, m_hEnv, m_hDBC, m_hStmt, &odbc_errmsg);
        return false;
    }

    /* Set the ODBC application version to 3.x */
    /* SQLSetEnvAttr sets attributes that govern aspects of environments */
    ret = SQLSetEnvAttr(m_hEnv, SQL_ATTR_ODBC_VERSION, (SQLPOINTER)
		SQL_OV_ODBC3, SQL_IS_UINTEGER);
	
    if (ret < SQL_SUCCESS) {
        check_sql_msg(ret, m_hEnv, m_hDBC, m_hStmt, &odbc_errmsg);
        wcout<<_T("odbc_startup@Failed to set ODBC version");
        return false;
    }

	/* SQLAllocConnect() function is use to allocated memory for a
    ** Connection handle (within a specified environment.
    **
    ** - Allocate An Connection Handle. */
    ret = SQLAllocConnect(m_hEnv, &m_hDBC);

    if (ret < SQL_SUCCESS) {
		wcout<<_T("odbc_startup@Failed to allocate connection handle");
        check_sql_msg(ret, m_hEnv, m_hDBC, m_hStmt, &odbc_errmsg);
        return false;
    }

	ret = SQLConnect(m_hDBC,(SQLWCHAR *)m_strDataSrc.c_str(), SQL_NTSL, 
							(SQLWCHAR *)m_strUserName.c_str(), SQL_NTSL, 
							(SQLWCHAR *)m_strPassword.c_str(), SQL_NTSL);

    if (ret < SQL_SUCCESS) {
        wcout<<_T("ODBC Connect: Failed to connect to ")<<m_strDataSrc<<endl;
		wcout<<_T("Username: ")<<m_strUserName<<_T("Password: ")<<m_strPassword<<endl;
        check_sql_msg(ret, m_hEnv, m_hDBC, m_hStmt, &odbc_errmsg);
        return false;
    }

	ret = SQLGetInfo(m_hDBC, SQL_DBMS_NAME, (SQLPOINTER) & m_szODBCDBMSName,
		sizeof(m_szODBCDBMSName), &info_size);
	
    if (ret < SQL_SUCCESS) {
        wcout<<_T("Failed to get DBMS Name");
        check_sql_msg(ret, m_hEnv, m_hDBC, m_hStmt, &odbc_errmsg);
        return false;
    }
	
//	if (g_GlobalVars.GetDebugLevel() > 0)
//		applog_errlog_tmr(ELOG_DEBUG, _T("odbc_startup@DBMS is %s."), szODBCDBMSName);
	
    ret = SQLGetInfo(m_hDBC, SQL_DATABASE_NAME, (SQLPOINTER) & m_szODBCDBName,
		sizeof(m_szODBCDBName), &info_size);
	
    if (ret < SQL_SUCCESS) {
//	applog_errlog_tmr(ELOG_ERROR, _T("odbc_startup@Failed to get Database Name"));
        check_sql_msg(ret, m_hEnv, m_hDBC, m_hStmt, &odbc_errmsg);
        return false;
    }
	
// 	if (g_GlobalVars.GetDebugLevel() > 0)
// 		applog_errlog_tmr(ELOG_DEBUG, _T("odbc_startup@Database is %s."), szODBCDBName);
	
    ret = SQLGetInfo(m_hDBC, SQL_SERVER_NAME, (SQLPOINTER) & m_szODBCServerName,
		sizeof(m_szODBCServerName), &info_size);
	
    if (ret < SQL_SUCCESS) {
        wcout<<_T("odbc_startup@Failed to get Database Name");
        check_sql_msg(ret, m_hEnv, m_hDBC, m_hStmt, &odbc_errmsg);
        return false;
    }
	
    ret = SQLGetInfo(m_hDBC, SQL_DRIVER_ODBC_VER, (SQLPOINTER) & szODBCDriverVer,
		sizeof(szODBCDriverVer), &info_size);
	
    if (ret < SQL_SUCCESS) {
//		applog_errlog_tmr(ELOG_ERROR, _T("odbc_startup@Failed to get ODBC Driver version"));
        check_sql_msg(ret, m_hEnv, m_hDBC, m_hStmt, &odbc_errmsg);
        return false;
    }

// 	if (g_GlobalVars.GetDebugLevel() > 0)
// 		applog_errlog_tmr(ELOG_DEBUG, _T("odbc_startup@Server is %s."), szODBCServerName);
	
	m_bConnected = TRUE;
	return true;
}

void DatabaseHandler::check_sql_msg(int32_t ret, SQLHENV hEnv, SQLHDBC hDBC, SQLHSTMT hStmt,
struct _odbc_errmsg * odbc_errmsg)
{
	check_sql(ret, hEnv, hDBC, hStmt, odbc_errmsg);

	if (odbc_errmsg != NULL)
		odbc_display_msg(odbc_errmsg);
}

void DatabaseHandler::odbc_display_msg(struct _odbc_errmsg * odbc_errmsg)
{	

    struct _odbc_errmsg lodbc_errmsg;
	struct timeval time_now;

    memset(&lodbc_errmsg, 0, sizeof(struct _odbc_errmsg));

    lodbc_errmsg = *odbc_errmsg;

	/* Check if message is repeated message */
	if (lodbc_errmsg.iNativeErr == last_odbc_errmsg.iNativeErr) 
	{
		/* Message is the same, check whether it's time to update again */
		gettimeofday(&time_now, NULL);
		
		if (time_now.tv_sec < (last_errmsg_update_time.tv_sec + last_errmsg_interval * 60)) 
		{
			/* Not time to update yet, return */
			errmsg_update_flag = 0;
			return;
		}	
	}
	errmsg_update_flag = TRUE_FLAG;

/*
    write_hts_log("odbc_display_msg@Server Message: SQLState %s",
           lodbc_errmsg.szSQLState);
    write_hts_log("odbc_display_msg@Server Error No %d::%s",
           lodbc_errmsg.iNativeErr,
           lodbc_errmsg.szErrMsg);
*/

	/* Update last msg time */
	gettimeofday(&last_errmsg_update_time, NULL);
	memcpy(&last_odbc_errmsg, &lodbc_errmsg, sizeof(struct _odbc_errmsg));
}

bool DatabaseHandler::LoadFileTypeList(vector<FileType> &vecFileTypeList)
{
	wstring strSQLCmd;
	int nRet;

	strSQLCmd = _T("select FileType, PointId, Ix, FilterString, IOType from ");
	strSQLCmd += m_szODBCDBName;
	strSQLCmd += _T(".dbo.DataFileType order by FileType, PointId, Ix");

	nRet = SQLAllocHandle(SQL_HANDLE_STMT, m_hDBC, &m_hStmt);
	if (nRet < SQL_SUCCESS) {
		wcout<<_T("LoadFileTypeList: Failed to allocate handle");
		return false;
	}

	nRet = SQLPrepare(m_hStmt, (SQLWCHAR*)strSQLCmd.c_str(), strSQLCmd.size());
	if (nRet < SQL_SUCCESS) {
		wcout<<_T("LoadFileTypeList: Failed to prepare SQL command");
		SQLFreeHandle(SQL_HANDLE_STMT, m_hStmt);
		return false;
	}

	nRet = SQLExecute(m_hStmt);
	if (nRet < SQL_SUCCESS) {
		wcout<<_T("LoadFileTypeList: Failed to run SQL command");
		SQLFreeHandle(SQL_HANDLE_STMT, m_hStmt);
		return false;
	}

	while(SQLFetch(m_hStmt) != SQL_NO_DATA_FOUND)
	{
		FileType fileType;

		wchar_t strTmpFileType[32];		
		wchar_t strTmpIOType[32];
		wchar_t strTmpFilterStr[32];

		memset(strTmpFileType, 0, sizeof(strTmpFileType));
		memset(strTmpIOType, 0, sizeof(strTmpIOType));

		nRet += SQLGetData(m_hStmt, 1, SQL_C_WCHAR, &strTmpFileType, DB_MAX_NAME_LEN, NULL);
		nRet += SQLGetData(m_hStmt, 2, SQL_C_LONG, &fileType.PointId, 0, NULL);
		nRet += SQLGetData(m_hStmt, 3, SQL_C_LONG, &fileType.Ix, 0, NULL);
		nRet += SQLGetData(m_hStmt, 4, SQL_C_WCHAR, &strTmpFilterStr, DB_MAX_NAME_LEN, NULL);
		nRet += SQLGetData(m_hStmt, 5, SQL_C_WCHAR, &strTmpIOType, DB_MAX_NAME_LEN, NULL);

		fileType.strFileType = strTmpFileType;
		fileType.strIOType = strTmpIOType;
		fileType.strFilterStr = strTmpFilterStr;

		if (nRet >= 0)
		{
			vecFileTypeList.push_back(fileType);
			wcout<<_T("File Type: ")<<fileType.strFileType<<_T("\tPointId: ")<<fileType.PointId<<_T("\t Ix: ")<<fileType.Ix<<endl;
		}
		else
		{
			wcout<<_T("LoadFileTypeList: SQLFetch failed!")<<endl;
		}
	}

	SQLFreeHandle(SQL_HANDLE_STMT, m_hStmt);

	return true;
}

//bool DatabaseHandler::LoadPointsByType( int PointId, int Ix, FileType *pFileType, list<PointInfo> &ptListOut )
//bool DatabaseHandler::LoadPointsByType( int PointId, int Ix, FileType *pFileType, DataFileInfo &ptList )
bool DatabaseHandler::LoadPointsByType( FileType *pFileType, DataFileInfo &ptList )
{
	wstring strSQLCmd;
	int nRet;

	if (!pFileType)
	{
		wcout<<_T("LoadPointsByType: Empty FileType pointer!");
		return false;
	}

	if (0 == wcscmp(pFileType->strFileType.c_str(), _T("FRKNJ")))
	{
		strSQLCmd = _T("select IPAddress, RowId, TagName from ");
		strSQLCmd += m_szODBCDBName;
		strSQLCmd += _T(".dbo.HistCurr where Description like '%");
		strSQLCmd += pFileType->strFilterStr + _T("%' ");
		strSQLCmd += _T("and Description not like '%de%e%'");
	}
	else if (0 == pFileType->Ix &&
		0 == pFileType->PointId)
	{
		strSQLCmd = _T("select IPAddress, RowId, TagName from ");
		strSQLCmd += m_szODBCDBName;
		strSQLCmd += _T(".dbo.HistCurr where Description like '%");
		strSQLCmd += pFileType->strFilterStr + _T("%'");
	}
	else
	{
		strSQLCmd = _T("select IPAddress, RowId, TagName from ");
		strSQLCmd += m_szODBCDBName;
		strSQLCmd += _T(".dbo.PointsByStation where PointId=") + to_wstring((_Longlong)pFileType->PointId) + _T("and Ix=") + to_wstring((_Longlong)pFileType->Ix);
		strSQLCmd += _T(" and GroupId<100");
	}

	nRet = SQLAllocHandle(SQL_HANDLE_STMT, m_hDBC, &m_hStmt);
	if (nRet < SQL_SUCCESS) {
		wcout<<_T("LoadPointsByType: Failed to allocate handle");
		return false;
	}

	nRet = SQLPrepare(m_hStmt, (SQLWCHAR*)strSQLCmd.c_str(), strSQLCmd.size());
	if (nRet < SQL_SUCCESS) {
		wcout<<_T("LoadPointsByType: Failed to prepare SQL command");
		SQLFreeHandle(SQL_HANDLE_STMT, m_hStmt);
		return false;
	}

	nRet = SQLExecute(m_hStmt);
	if (nRet < SQL_SUCCESS) {
		wcout<<_T("LoadPointsByType: Failed to run SQL command");
		SQLFreeHandle(SQL_HANDLE_STMT, m_hStmt);
		return false;
	}

	int nIndex = 0;
	while(SQLFetch(m_hStmt) != SQL_NO_DATA_FOUND)
	{
		PointInfo pt;

		wchar_t strTmpTagName[64];
		memset(strTmpTagName, 0, sizeof(strTmpTagName));

		nIndex++;

		nRet += SQLGetData(m_hStmt, 1, SQL_C_LONG, &pt.rowid.ip_addr, 0, NULL);
		nRet += SQLGetData(m_hStmt, 2, SQL_C_LONG, &pt.rowid.key, 0, NULL);
		nRet += SQLGetData(m_hStmt, 3, SQL_C_WCHAR, &strTmpTagName, DB_MAX_NAME_LEN, NULL);

		pt.strTagName = strTmpTagName;
		pt.strFileType = pFileType->strFileType;
		pt.strIOType = pFileType->strIOType;

		if (nRet >= 0)
		{
			//ptListOut.push_back(pt);
			ptList.InsertPoint(pt);
			//wcout<<to_wstring((_Longlong)nIndex)<<_T("\tTagName: ")<<pt.strTagName<<_T("\tIP: ")<<pt.rowid.ip_addr<<_T("\tRowId: ")<<pt.rowid.key<<endl;
		}
		else
		{
			wcout<<_T("LoadPointsByType: SQLFetch failed!")<<endl;
		}
	}

	pFileType->nPointCnt = nIndex;
	SQLFreeHandle(SQL_HANDLE_STMT, m_hStmt);
	return true;
}
