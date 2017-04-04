#include "StdAfx.h"
#include "RTDBHandler.h"
#include "CSharpAdapter.h"

extern DataFileInfo g_pointlist;
#define MAX_SQL_CMD_LEN	1024

rowid_t myRowId; 
u_int16_t myTableId;
CSharpCallback _CSharpCallback;

RTDBHandler::RTDBHandler()
	:m_bRtdbConnected(false)
{
}


RTDBHandler::~RTDBHandler(void)
{
}

bool RTDBHandler::ConnSupervisor()
{

	return true;
}

void RTDBHandler::DisconnSupervisor()
{

}

bool RTDBHandler::ConnRTDB(CSharpCallback cSharpCallBack, char * _TagName)
{
	//rtdb_process_t *m_pRTproc1 = NULL;
	int nRet;

	_CSharpCallback = cSharpCallBack;

	// 1. Connect RTDB
	nRet = rtdb_connect("localhost", DB_RT_USERNAME, DB_RT_PASSWORD, "CMSDataPorting", &m_pRTproc);
	if (E_NO_ERROR != nRet)
	{
		wcout<<_T("Connect RTDB failed: ")<<nRet<<endl;
		return false;
	}

	// 2. Use runtime database
	nRet = rtdb_use_db(m_pRTproc, S_runtime);
	if (E_NO_ERROR != nRet)
	{
		wcout<<_T("Cannot Runtime database: ")<<nRet<<endl;
		return false;
	}

	// 3. Get connection handle
	nRet = rtdb_get_fd(m_pRTproc, &m_rtdb_fd);
	if (E_NO_ERROR != nRet)
	{
		wcout<<_T("Cannot get RTDB fd: ")<<nRet<<endl;
		return false;
	}

	ROWID_ZERO(&myRowId);
	nRet = rtdb_find_s(m_pRTproc, T_AnalogBase, myRowId, A_TagName, OP_EQ, _TagName, &myRowId, &myTableId);
	// 4. Register RTDB notification
	RTDBRegisterInput(m_rtdb_fd, (LoopCallback) HandleRTDB_msg, this, NULL);

	// 5. Register local name
	nRet = rtdb_register_name(m_pRTproc, "CMSDataPorting", RT_GLOBAL_SCOPE, 1);

	m_bRtdbConnected = true;
	return true;
}

void RTDBHandler::DisconnRTDB()
{

}

void RTDBHandler::HandleRTDB_msg( u_int32_t socket_fd, void *arg )
{
//	wcout<<_T("RTDB message received!")<<endl;

	u_int32_t msgtype;
	u_int16_t atid, opcode;
	int32_t token1, token2;
	rowid_t rowid;
	u_int32_t noteid;
	double d; int32_t k;
	void *msg;
	struct timeval srctime, seqtime;

	PointInfo *pt = nullptr;

	int nRet;
	bool bRet;

	do 
	{
		nRet = rtdb_recv_msg(socket_fd, &msg, &msgtype);
		if ((nRet != E_NO_ERROR) && (nRet != E_NO_SOCKET_DATA)) {
			wcout<<_T("HandleRTDB@rtdb_recv_msg failed! Err: ")<<nRet<<endl;

			//reset_rtdb();
			return;
		}

		switch(msgtype)
		{
		case RT_EVENT_NOTIFICATION:
			nRet = rtdb_decode_notify_record(msg, &rowid, &opcode, 0, &atid,
				0, &srctime, &seqtime, &noteid,
				&token1, &token2);

			if (nRet != E_NO_ERROR) {
				wcout<<_T("HandleRTDB@rtdb_decode_notify_record failed! Err: ")<<nRet<<endl;
				//reset_rtdb();
				return ;
			}

			if (CMS_DATAPORTING_APP == token2)
			{
				switch (token1)
				{
				/* Analog point updates */
				case NOTE_ABASE_DQ:
				case NOTE_DBASE_DQ:
					//wcout<<_T("ABASE DQ notify received!")<<endl;
					nRet = rtdb_decode_value_i(msg, &k);

					if (nRet != E_NO_ERROR) {
						wcout<<_T("HandleRTDB@Failed to decode DBASE Status - Err ")<<nRet<<endl;
						return ;
					}

					bRet = g_pointlist.LocatePoint(rowid, &pt);
					if (!bRet)
					{
						// If cannot find this rowid in my point list, then ignore
						return;
					}

					if (NULL == pt)
					{
						wcout<<_T("HandleRTDB@ point located, but null pointer received!")<<endl;
						return;
					}

					pt->nDataQuality = k;
					break;

				case NOTE_ABASE_VALUE:
					//wcout<<_T("ABASE value notify received!")<<endl;
					nRet = rtdb_decode_value_d(msg, &d);

					if (nRet != E_NO_ERROR) {
						wcout << _T("HandleRTDB@Failed to decode ABASE Value! Err: ") << nRet; wcout << _T("RowId: ") << hex << rowid.key << endl;
						return ;
					}

					if (rowid.key == myRowId.key){
						_CSharpCallback(d);
					}

					if ((_isnan(d)) > 0)
						d = 0.0;
					if ((_finite(d)) == 0)
						d = 0.0;				

					bRet = g_pointlist.LocatePoint(rowid, &pt);
					if (!bRet)
					{
						// If cannot find this rowid in my point list, then ignore
						return;
					}

					if (NULL == pt)
					{
						wcout<<_T("HandleRTDB@ point located, but null pointer received!")<<endl;
						return;
					}

					pt->value = d;

					break;

// 				case NOTE_DBASE_DQ:
// 					wcout<<_T("DBASE DQ notify received!")<<endl;
// 					break;

				case NOTE_DBASE_STATUS:
					//wcout<<_T("DBASE value notify received!")<<endl;
					nRet = rtdb_decode_value_i(msg, &k);

					if (nRet != E_NO_ERROR) {
						wcout<<_T("HandleRTDB@Failed to decode DBASE Status - Err ")<<nRet<<endl;
						return ;
					}

					bRet = g_pointlist.LocatePoint(rowid, &pt);
					if (!bRet)
					{
						// If cannot find this rowid in my point list, then ignore
						return;
					}

					if (NULL == pt)
					{
						wcout<<_T("HandleRTDB@ point located, but null pointer received!")<<endl;
						return;
					}

					//wcout<<_T("HandleRTDB@ ")<<pt->strTagName<<_T(" status changed to ")<<k<<endl;
					//pt->status = k;
					pt->value = k;
					break;

				default:
					break;
				}
			}

			break;

		case RT_NAME_NOTIFICATION:
			break;

		default:
			break;
		}


	} while (RTDBCheckInput(socket_fd));
}

bool RTDBHandler::LoadRTDBData()
{
	int nRet;
	size_t cmdLen = 0;
	rowid_t rowid;
	int32_t iColumnNo;
	double value;
	u_int32_t status;
	u_int16_t dataQuality;

	char sql_cmd[MAX_SQL_CMD_LEN];
	memset(sql_cmd, 0, sizeof(sql_cmd));

	wstring strSQLCmd, strBaseTableName, strAttrName;

	list<PointInfo>::iterator iterPt;

	for (iterPt=g_pointlist.m_listPointList.begin(); iterPt!=g_pointlist.m_listPointList.end(); iterPt++)
	{
		// Check I/O type
		if (0 == wcscmp(iterPt->strIOType.c_str(), _T("A")))
		{
			strBaseTableName = _T("AnalogBase");
			strAttrName = _T("Value");
		}
		else if (0 == wcscmp(iterPt->strIOType.c_str(), _T("D")))
		{
			strBaseTableName = _T("DigitalBase");
			strAttrName = _T("Status");
		}		 

		strSQLCmd = _T("select ");
		strSQLCmd += strAttrName + _T(", DataQuality from ");
		strSQLCmd += strBaseTableName + _T(" where TagName=='");
		strSQLCmd += iterPt->strTagName + _T("'");

		wcstombs_s(&cmdLen, sql_cmd,(size_t)MAX_SQL_CMD_LEN, strSQLCmd.c_str(),(size_t)MAX_SQL_CMD_LEN);

		/* Set the RTDB to runtime */
		nRet = rtdb_use_db(m_pRTproc, S_runtime);	
		if (nRet != E_NO_ERROR) {
			wcout<<_T("Unable to set RTDB database to runtime. Err: ")<<nRet<<endl;
			reset_rtdb();
			return false;
		}

		nRet = rtdb_cmd(m_pRTproc, sql_cmd);

		if (nRet != E_NO_ERROR) {
			wcout<<_T("extract_stationInfo@rtdb_fcmd failed. Err: ")<<nRet<<endl;
			return false;
		}

		/* Check on the SQL execute statement return process */
		nRet = rtdb_sql_exec(m_pRTproc);

		if (nRet != E_NO_ERROR) {
			wcout<<_T("rtdb_sql_exec failed. Err: ")<<nRet<<endl;
			return false;
		}

		iColumnNo = 1;
		if (0 == wcscmp(iterPt->strIOType.c_str(), _T("A")))
		{
			nRet = rtdb_bind(m_pRTproc, iColumnNo++, DOUBLE_TYPE, 0, (char *) & value);
			if (nRet != E_NO_ERROR) {
				wcout<<_T("rtdb_bind Value failed. Err: ")<<nRet<<endl;
				return false;
			}
		}
		else if (0 == wcscmp(iterPt->strIOType.c_str(), _T("D")))
		{
			nRet = rtdb_bind(m_pRTproc, iColumnNo++, U_LONG_TYPE, 0, (char *) & status);
			if (nRet != E_NO_ERROR) {
				wcout<<_T("rtdb_bind Value failed. Err: ")<<nRet<<endl;
				return false;
			}
		}

		nRet = rtdb_bind(m_pRTproc, iColumnNo++, U_SHORT_TYPE, 0, (char *) & dataQuality);
		if (nRet != E_NO_ERROR) {
			wcout<<_T("rtdb_bind Value failed. Err: ")<<nRet<<endl;
			return false;
		}

		while (rtdb_next_row(m_pRTproc, &rowid) != E_NO_MORE_ROWS) 
		{
			if (0 == wcscmp(iterPt->strIOType.c_str(), _T("A")))
			{
				// Set the given double-precision floating point value to zero if it is NaN(Not A Number) or Inf(Infinity)
				if ((_isnan(value)) > 0)
					value = 0.0;
		
				if ((_finite(value)) == 0)
					value = 0.0;
		
				/* Value to big to store to database */
				if (is_huge_value(&value) != 0)
					value = 0.0;

				iterPt->value = value;
			}
			else if (0 == wcscmp(iterPt->strIOType.c_str(), _T("D")))
			{
				iterPt->status = status;
			}

			iterPt->nDataQuality = dataQuality;
		}
	}
	return true;
}

bool RTDBHandler::SetNotify()
{
	int nRet;
	u_int32_t token;

	/* Set the database to be used */
	nRet = rtdb_use_db(m_pRTproc, S_runtime);

	if (nRet != E_NO_ERROR) {
		wcout<<_T("SetNotify@Failed to set to runtime (rtdb) - Err ")<<nRet<<endl;
		reset_rtdb();
		return false;
	}

	/* BASE_NOTIFY_READY */
	/* Base Value/Status Change Notification */
	/* Set notify on changes in value at the AnalogBase table */
	nRet = rtdb_table_notify(m_pRTproc, T_AnalogBase, A_Value,
		NOTE_ABASE_VALUE, CMS_DATAPORTING_APP, &token);

	if (nRet != E_NO_ERROR) {
		wcout<<_T("SetNotify@Failed to set notify for AnalogBase Value. Err: ")<<nRet<<endl;
		reset_rtdb();
		return false;
	}

	/* Set notify on changes in status at the DigitalBase table */
	nRet = rtdb_table_notify(m_pRTproc, T_DigitalBase, A_Status,
		NOTE_DBASE_STATUS, CMS_DATAPORTING_APP, &token);

	if (nRet != E_NO_ERROR) {
		wcout<<_T("SetNotify@Failed to set notify-DigitalBase Status. Err: ")<<nRet<<endl;
		reset_rtdb();
		return false;
	}

	/* Base Data Quality Change Notification */
	/* Set notify on changes in DQ flag at the AnalogBase table */
	nRet = rtdb_table_notify(m_pRTproc, T_AnalogBase, A_DataQuality,
		NOTE_ABASE_DQ, CMS_DATAPORTING_APP, &token);

	if (nRet != E_NO_ERROR) {
		wcout<<_T("SetNotifyHTS@Failed to set notify-AnalogBase DataQuality. Err: ")<<nRet<<endl;
		reset_rtdb();
		return false;
	}

	/* Set notify on changes in DQ flag at the DigitalBase table */
	nRet = rtdb_table_notify(m_pRTproc, T_DigitalBase, A_DataQuality,
		NOTE_DBASE_DQ, CMS_DATAPORTING_APP, &token);

	if (nRet != E_NO_ERROR) {
		wcout<<_T("SetNotifyHTS@Failed to set notify-DigitalBase DataQuality. Err: ")<<nRet<<endl;
		reset_rtdb();
		return false;
	}

	return true;
}

void RTDBHandler::reset_rtdb()
{
	rtdb_close(m_pRTproc);
	m_pRTproc = NULL;

	RTDBUnRegisterInput(m_rtdb_fd);

	wcout<<_T("reset_rtdb@done");
}

/*----------------------------------------------------------------------*
 * is_huge_value() - To check if value is out of range
 * Input        : dValue - value to check
 * Output       : None
 * Return Codes : 0 if value is normal
 *				  1 if value exceeds higher range
 *				  -1 if value is less than lower range
 *----------------------------------------------------------------------*/
int32_t
RTDBHandler::is_huge_value(double *dValue)
{
    /* Beware FIREBIRD double range is smaller than SYBASE */

    if (*dValue > OOR_POS_VALUE_IB)
        return 1;
    else
        if (*dValue < OOR_NEG_VALUE_IB)
            return -1;
        else
            return 0;
}






