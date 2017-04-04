#include "StdAfx.h"
#include "DataFileGen.h"

extern DataFileInfo g_pointlist;

DataFileGen::DataFileGen(ConfigInfo &cfgInfo)
	:m_cnTagsPerFile(15),
	m_rConfigInfo(cfgInfo),
// 	m_strLocalFilePath(_T("c:\\tmp\\CMSDataFile\\")),
// 	m_strRemoteFilePath(_T("/home/devel/fubin/CMSDataFile/")),
// 	m_strWinSCPPath(_T("C:\\Program Files (x86)\\WinSCP\\"))

	m_strLocalFilePath(_T("D:\\CMSDataFile\\")),
	m_strRemoteFilePath(_T("/CMSDataFile/")),
	m_strWinSCPPath(_T("C:\\Program Files (x86)\\WinSCP\\"))

{

}


DataFileGen::~DataFileGen(void)
{

}

bool DataFileGen::GenerateByFileType( wstring strFileType )
{
	bool bRet;

	list<PointInfo> ptList;
	list<PointInfo>::iterator iterPt;

	// 1. Shortlist the points that match this FileType
	for (iterPt=g_pointlist.m_listPointList.begin();
		iterPt!= g_pointlist.m_listPointList.end();
		iterPt++)
	{
		if (0 == iterPt->strFileType.compare(strFileType))
		{
			ptList.push_back(*iterPt);
		}
	}

	if (0 == ptList.size())
	{
		wcout<<_T("There's no point under type ")<<strFileType<<endl;
		return false;
	}

	// 2. Format content
	wstring strFileName;
	wstring strContent;
	wstring strHeader, strBody;
	unsigned nFileSeqNo, nTotalFiles;
	unsigned nPtIndex=1;


	// 2.1 Get total file count, and init file seq No.
	if (ptList.size()>m_cnTagsPerFile)
	{
		nTotalFiles = ptList.size()/m_cnTagsPerFile + 1;
		nFileSeqNo = 1;
	}
	else
	{
		nTotalFiles = 1;
		nFileSeqNo = 0;
	}

	// 2.2 Get current date/time string
	time_t rawtime;
	struct tm timeinfo;
	const unsigned cuTextBuffer = 256;
	wchar_t wchDate[cuTextBuffer];
	wchar_t wchTime[cuTextBuffer];

	time (&rawtime);
	localtime_s(&timeinfo, &rawtime);

	wcsftime(wchDate,cuTextBuffer,_T("%d%m%Y"),&timeinfo);
	wcsftime(wchTime,cuTextBuffer,_T("%H:%M"),&timeinfo);

	const wstring strComma = _T(",");

	// 2.2 Format content
	for (iterPt = ptList.begin();
		iterPt!=ptList.end() && nFileSeqNo<=nTotalFiles;
		iterPt++,nPtIndex++)
	{
// 		if (0 == ptList.size())
// 		{
// 			wcout<<_T("There no points under FileType: ")<<strFileType<<endl;
// 			break;
// 		}

		// 2.2.1 If nPtIndex == 1, then init a new file
		if (1 == nPtIndex)
		{
			wchar_t fileNameBuffer[cuTextBuffer];
			wcsftime(fileNameBuffer,cuTextBuffer,_T("%Y%m%d%H%M_"),&timeinfo);

			if (0 == nFileSeqNo)
			{
				strFileName = wstring(fileNameBuffer) + strFileType + _T(".csv");
			}
			else
			{
				strFileName =wstring(fileNameBuffer) + strFileType + to_wstring((_ULonglong)nFileSeqNo) + _T(".csv");
			}

			wcout<<strFileName<<endl;

			strHeader = _T("Date,Time");
			strBody = wstring(wchDate)+strComma+wstring(wchTime);
		}

		// 2.2.2 Format content
		strHeader += strComma + iterPt->strTagName + strComma + _T("QF");
		strBody += strComma + to_wstring((long double)iterPt->value)+strComma
			+GetDQString(iterPt->nDataQuality);

		// 2.2.3 If already 15 tags in this file, then reset counters
		// and write into the file
		if (m_cnTagsPerFile == nPtIndex)
		{
			nPtIndex = 0;
			nFileSeqNo++;

			bRet = WriteLine(strFileName, strHeader);
			if (!bRet)
			{
				wcout<<_T("Write file header failed for ")<<strFileName<<endl;
				continue;
			}

			bRet = WriteLine(strFileName, strBody);
			if (!bRet)
			{
				wcout<<_T("Write file body failed for ")<<strFileName<<endl;
				continue;
			}

			// If data file successfully generated, then record the file name
			m_vecGeneratedFileNames.push_back(strFileName);

			strHeader = _T("");
			strBody = _T("");
		}
	}

	// If Header & Body are not empty, them write them into the last file:
	if (!strHeader.empty() &&
		!strBody.empty())
	{
		// Write the last file
		bRet = WriteLine(strFileName, strHeader);
		if (!bRet)
		{
			wcout<<_T("Write file header failed for ")<<strFileName<<endl;
			return false;
		}
		bRet = WriteLine(strFileName, strBody);
		if (!bRet)
		{
			wcout<<_T("Write file body failed for ")<<strFileName<<endl;
			return false;
		}

		// If data file successfully generated, then record the file name
		m_vecGeneratedFileNames.push_back(strFileName);
	}

	return true;
}

bool DataFileGen::WriteLine(wstring strFileName, wstring strLine, bool bTruncate )
{

	wofstream ofs;
	//wstring strFilePath(_T("c:\\tmp\\CMSDataFile\\"));

	strFileName = m_strLocalFilePath+strFileName;//+_T(".csv");

	if (bTruncate)
	{
		ofs.open (strFileName.c_str(), std::ofstream::out | std::ofstream::trunc);
	}
	else
	{
		ofs.open (strFileName.c_str(), std::ofstream::out | std::ofstream::app);
	}

	ofs << strLine.c_str()<<endl;

	ofs.close();

	return true;
}

std::wstring DataFileGen::GetDQString( unsigned int nDQ )
{
	if (nDQ & DQ_TELEMFAIL)
	{
		return wstring(_T("F"));
	}
	
	if (nDQ & DQ_MANUALLYSET)
	{
		return wstring(_T("M"));
	}

	if (nDQ & DQ_BLOCKED)
	{
		return wstring(_T("B"));
	}

	return wstring(_T("N"));
}

bool DataFileGen::GenerateAll(vector<FileType> &vecFileTypeList)
{
	wstring strFileType;
	bool bRet;

	if (0 == vecFileTypeList.size())
	{
		wcout<<_T("The general File Type list is empty!")<<endl;
		return false;
	}

	vector<FileType>::iterator iter = vecFileTypeList.begin();
	for (;iter!=vecFileTypeList.end(); iter++)
	{
		// There might be duplicate file type in the list,
		// so if current one has been processed already, then skip.
		if (0 == strFileType.compare(iter->strFileType))
		{
			continue;
		}

		strFileType = iter->strFileType;
		bRet = GenerateByFileType(strFileType);

		if (!bRet)
		{
			wcout<<_T("Generate ")<<strFileType<<_T(" files failed!")<<endl;
		//	return false;
		}
	}

	return true;
}

bool DataFileGen::SendFiles()
{
	wstring strWinSCPScrip;
	wstring strScripFileName(_T("run.txt"));
	HINSTANCE nRet;

	strWinSCPScrip = _T("option confirm off\n");
	strWinSCPScrip += _T("open ")+ m_rConfigInfo.GetSFTwPUser()+_T(":")+m_rConfigInfo.GetSFTPPassword()
		+ _T("@")+m_rConfigInfo.GetSFTPIP()+_T(":")+m_rConfigInfo.GetSFTPPort()+_T("\n");

	vector<wstring>::iterator iter = m_vecGeneratedFileNames.begin();
	for (;iter!=m_vecGeneratedFileNames.end(); iter++)
	{
		strWinSCPScrip += _T("put ")+m_strLocalFilePath+*iter+_T(" ")
			+m_strRemoteFilePath+*iter+_T("_\n");
	}

	// rename transfered files on server side
	for (iter=m_vecGeneratedFileNames.begin(); iter!=m_vecGeneratedFileNames.end(); iter++)
	{
		strWinSCPScrip += _T("mv ")+m_strRemoteFilePath+*iter+_T("_ ")
			+m_strRemoteFilePath+*iter+_T("\n");
	}

	strWinSCPScrip += _T("close\nexit");

	WriteLine(strScripFileName, strWinSCPScrip, true);
	
	wstring strWinSCPParameter = _T("/console /script=")+m_strLocalFilePath+strScripFileName;
	nRet = ShellExecuteW(NULL, _T("open"), _T("WinSCP.exe"), strWinSCPParameter.c_str(), m_strWinSCPPath.c_str(),SW_SHOW);
	
	m_vecGeneratedFileNames.clear();

	return true;
}




