#include "StdAfx.h"
#include "ConfigInfo.h"


ConfigInfo::ConfigInfo(void):optind(0),
	m_nLogFileSize(1024),
	m_nNumOfLogFiles(5)
{
	// Temporary
	memset(m_szGroupName, 0, sizeof(m_szGroupName));
	_tcscpy(m_szGroupName, _T("tmpGroup"));
}


ConfigInfo::~ConfigInfo(void)
{
}

bool ConfigInfo::LoadInitFile()
{
//	TempInit();
	CMSInit();

	wifstream ifs;



	m_bLoaded = true;
	return true;
}

bool ConfigInfo::ExtractArgs(int argc, _TCHAR* argv[])
{
	int ch = 0, number = 0;

	if (argc == 1) {
		PrintUsage();
	} 
	else{
		while ((ch = xgetopt(argc, argv, _T("i:?"))) != EOF) 
		{
			switch (ch)
			{
			case 'i': 
				number = _ttoi(xoptarg);
				if(number < 0 )
					number=1;
				m_nInitFileNo = (int8_t)number;
				wcout<<_T("Init file number is : ")<<m_nInitFileNo<<endl;
				break;

			case '?':
			default:
				PrintUsage();
				break;


			}
		}//end of while
	}//end of else

	return true;
}

inline void ConfigInfo::PrintUsage()
{
	wcout<<_T("No input argument. Print usage info")<<endl;
}

int ConfigInfo::xgetopt(int argc, TCHAR *argv[], TCHAR *optstring)
{
	static TCHAR *next = NULL;

	if (optind == 0)
	{
		next = NULL;
	}
	optarg = NULL;

	if (next == NULL || *next == _T('\0'))
	{
		if (optind == 0)
		{
			optind++;
		}
		if (optind >= argc || argv[optind][0] != _T('-') || argv[optind][1] == _T('\0'))
		{
			optarg = NULL;
			if (optind < argc)
			{
				xoptarg = argv[optind];
			}
			return EOF;
		}
		if (_tcscmp(argv[optind], _T("--")) == 0)
		{
			optind++;
			optarg = NULL;
			if (optind < argc)
			{
				xoptarg = argv[optind];
			}
			return EOF;
		}
		next = argv[optind];
		next++;		// skip past -
		optind++;
	}   

	TCHAR c = *next++;
	TCHAR *cp = _tcschr(optstring, c);

	if (cp == NULL || c == _T(':'))
	{
		return _T('?');
	}
	cp++;
	if (*cp == _T(':'))
	{
		if (*next != _T('\0'))
		{
			xoptarg = next;
			next = NULL;
		}
		else if (optind < argc)
		{
			xoptarg = argv[optind];
			optind++;
		}
		else
		{
			return _T('?');
		}
	}
	return c;

}

void ConfigInfo::TempInit()
{
	m_strDataSrc = _T("OfficeDS");
	m_strDBUser = _T("sa");
	m_strDBPassword = _T("wellcome");
	m_bConnSupervisor = false;
	m_nDebugLevel = 1;

	m_strSFTPIP = _T("192.3.20.5");
	m_strSFTPPort = _T("22");
	m_strSFTPUser = _T("fubin");
	m_strSFTPPassword = _T("fubin123");

	m_bLoaded = true;
}

void ConfigInfo::CMSInit()
{
	m_strDataSrc = _T("hts_tbcc");
	m_strDBUser = _T("hts_tbcc1");
	m_strDBPassword = _T("@hts_cms123");
	m_bConnSupervisor = true;
	m_nDebugLevel = 1;

	m_strSFTPIP = _T("10.206.86.52");
	m_strSFTPPort = _T("22");
	m_strSFTPUser = _T("willowglen");
	m_strSFTPPassword = _T("d@tapu5h");

	m_bLoaded = true;
}

