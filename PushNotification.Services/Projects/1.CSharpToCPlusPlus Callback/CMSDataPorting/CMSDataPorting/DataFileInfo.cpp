#include "StdAfx.h"
#include "DataFileInfo.h"


DataFileInfo::DataFileInfo(void)
{
}


DataFileInfo::~DataFileInfo(void)
{
}

bool DataFileInfo::InsertPoint(PointInfo pt)
{
	m_listPointList.push_back(pt);
	return true;
}

bool DataFileInfo::LocatePoint( rowid_t rowid, PointInfo **outPt )
{

	//PointInfo *pt = NULL;

	//pt = &(*m_listPointList.begin());

	list<PointInfo>::iterator iter;
	
	//for_each()
	auto func = [&](PointInfo &pt)
	{return pt.rowid.ip_addr==rowid.ip_addr && pt.rowid.key==rowid.key;};

	iter = find_if(m_listPointList.begin(), m_listPointList.end(), func);
	if (m_listPointList.end() == iter)
	{
	//	wcout<<_T("Not able to locate point rowid: ")<<rowid.ip_addr<<_T(":0x")<<hex<<rowid.key<<endl;
		return false;
	}

	*outPt = &(*iter);
	return true;
}
