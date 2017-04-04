#include "CSharpAdapter.h"
#pragma once

//for rtdb notification register
#define CMS_DATAPORTING_APP		0x05


class RTDBHandler
{
public:
//	RTDBHandler(DataFileInfo &pointList);
	RTDBHandler();
	~RTDBHandler(void);

	bool ConnSupervisor();
	void DisconnSupervisor();

	bool ConnRTDB(CSharpCallback cSharpCallback, char * _TagName);
	void DisconnRTDB();

	bool LoadRTDBData();

	static void HandleRTDB_msg(u_int32_t socket_fd, void *arg);

	bool SetNotify();

	void reset_rtdb();

private:
	int32_t RTDBHandler::is_huge_value(double *dValue);


private:
	rtdb_process_t *m_pRTproc;
	SOCKET m_rtdb_fd;

	bool m_bRtdbConnected;

//	DataFileInfo &m_rPtList;




};

