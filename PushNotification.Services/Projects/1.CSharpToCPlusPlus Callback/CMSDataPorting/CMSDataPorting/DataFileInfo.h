#pragma once

typedef struct _PointInfo
{
public:
	_PointInfo():value(0),
				status(0),
				nDataQuality(0)
	  {
		  ROWID_ZERO(&rowid);
	  }

	rowid_t	rowid;
	wstring	strTagName;
	double	value;
	int		status;
	int		nDataQuality;

	wstring strFileType;
	wstring strIOType;

}PointInfo;

typedef struct _FileType
{
	wstring strFileType;
	int		PointId;
	int		Ix;
	wstring strFilterStr;
	wstring	strIOType;
	unsigned nPointCnt;
}FileType;

class DataFileInfo
{
public:
	DataFileInfo(void);
	~DataFileInfo(void);

	bool InsertPoint(PointInfo pt);

	//PointInfo&	LocatePoint(rowid_t rowid);
	bool LocatePoint( rowid_t rowid, PointInfo **outPt );

	vector<FileType>	m_vecFileTypeList;

	list<PointInfo>	m_listPointList;

private:
//	map<PointInfo, DataFileType> m_mapPointList;

};


