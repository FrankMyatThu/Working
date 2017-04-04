/*******************************************************************************
| File: app_logmsg.cpp	    | Module: OPCServer				       |
|------------------------------------------------------------------------------|
| Project: 05      | Targeted Hardware: PC	     | Targeted OS: Windows    |
|==============================================================================|
|                       Revision History                                       |
|                       ----------------                                       |
|  Date       By   Version            Comments                                 |
|(dd/mm/yyyy)                                                                  |
|----------|-----|-------|-----------------------------------------------------|
|10/10/2006| SAK | 1.0.2 |					               |
*******************************************************************************/

/*
*    Copyright (c) Willowglen Services Pte Ltd 2006.
*
*    No portion of this code may be reused, modified or
*    distributed in any way without the expressed written
*    consent of Willowglen Services Pte. Ltd.
*
*/

/*----------------------------------------------------------------------*
* Containing functions to manipulate error log. Functionalities implemented
* here are:
* 1) Initialization of log file and log version file.
* 2) Message logging to log file.
* 3) Controlling log file size, once a log file exceed a preset length,
*    it will be moved to a old-of-date log file named as:  APP_ERRLOG.versionNo
*    and then create a new log file as the current log file
*    (the current log file is always named APP_ERRLOG).
* 4) If you want to print the usec then set the usec_mode varibale to 1
*    Print format will be 
*		Sat Aug 19 13:26:03 2006 [usec].000103 Sample          DEBUG  db_app_register() -- name Sample registered
*
* Rotation of Log Files
* ---------------------
* A number of log files are kept of the form:
* APP_ERRLOG		current LOG file
* APP_ERRLOG.0	previous file
* ...
* APP_ERRLOG.N	oldest file
* 
* The following setting are defined in app_logmsg.h:-
* 1. #define LOGMSG_FILE_NAME		"APP_ERRLOG"
* 2. #define LOGMSG_FILE_DIR		"errlog"
* 3. #define DEFAULT_LOG_SIZE	102400	- Default size of log file 
* 4. #define DEFAULT_LOG_COUNT	5	- Default log file count.
* 
* The application Name is defined in globals.h 
* #define APPLICATION_NAME		"Sample"
*
* Usage : app_errlog (int32_t pri, const char *format, ...)
*         pri - Priority code that refer to Type of Message defined in syslink.h
*				 #define  ELOG_PANIC     1 -Priority for fatal error.
*               #define  ELOG_ERROR     2 -Priority for normal error.
*               #define  ELOG_WARN      3 -Priority for warning message.
*               #define  ELOG_ADVISE    4 -Priority for advisary information.
*               #define  ELOG_STATE     5 -Priority for state information when necessary.
*               #define  ELOG_DEBUG     6 -Priority for debugging.
*         format and arg - similar to printf().
*
* The location of the log file will be at 
*           $SCADA_PROJECT/LOGMSG_FILE_DIR
*
* Usage : 
* 1. Call applog_init_log (int32_t no_of_log_file, int32_t log_file_size). 
*    with number of Log files and logfile size.
*
* 2. replace those function name errlog() to applog_errlog() and keep all the 
*          arguments unchanged. 
*          The location of the log file will be at 
*           $SCADA_PROJECT/LOGMSG_FILE_DIR.
* 3. Before Application termination call applog_close_app_log();
*----------------------------------------------------------------------*/
#include "stdafx.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

#include <sys/types.h>
#include <sys/stat.h>
#include <stdio.h>
#include <stdlib.h>
#include <fcntl.h>
#include <string.h>
#include <errno.h>

#ifndef _ARCH_I386_NTVC_
#include <sys/param.h>
#include <sys/time.h>
#include <unistd.h>
#include <sys/uio.h>
#include <stdarg.h>
#else
#include <io.h>
#include <time.h>
#endif

#include <syslink.h>
//#include "globals.h"
#include "app_logmsg.h"	/* Macro & defintiion */

static int32_t s_hLogFile = 0;	/* Logfile handle */
static int32_t logfd_tmr = 0;	/* Logfile handle */
TCHAR* g_szRuntime_path;		/* Syslink Project Path */
int32_t g_usec_mode = 0;		/* To display usec for time stampe */
int32_t var_log_count = DEFAULT_LOG_COUNT;	/* Cyclic for 5 files */
int32_t var_log_size = DEFAULT_LOG_SIZE;	/* 1 MByte logfile size */

BOOL g_LogFileSuccessfullyInit = FALSE;		/* Determines if the log file functionality has been successfully initialised */

int32_t log_to_app_log_file;	/* Put all the errlog to applog_errlog() function */

CRITICAL_SECTION g_criticalSection;	/* To create critical section for applog_errlog function */
CString csApplog;
int32_t iApplog = 0;

/*----------------------------------------------------------------------*
* applog_init_app_log -- Initialization of errlog utility.
* For example, to open log file and version file and store the
* file descriptors in some static variables.
*----------------------------------------------------------------------*/
int32_t applog_init_app_log(int32_t no_of_log_file, int32_t log_file_size)
{
    TCHAR log_file[512];
	TCHAR log_dir[512];
	
    var_log_count = no_of_log_file;	/* No of Cyclic files */
    var_log_size = log_file_size;	/* 1 MByte logfile size */

	InitializeCriticalSection(&g_criticalSection);
	
    /* Insure that environment variables set up properly */
    g_szRuntime_path = _tgetenv (_T("SCADA_PROJECT"));
    if (g_szRuntime_path == NULL || _tcslen (g_szRuntime_path) == 0)
    {
		_ftprintf (stderr, _T("CRITICAL ERROR: Environment variable SCADA_PROJECT not set\n"));
		exit (-1);
    }

	/* Create dir if it not exist */
	_stprintf(log_dir, _T("%s/%s/%s"), g_szRuntime_path, VARIABLE_T(LOGMSG_FILE_DIR), g_cfg.m_szGroupName);
	_tmkdir(log_dir);

    /* Open the log file. */
//	_stprintf (log_file, _T("%s/%s/%s"), g_szRuntime_path, VARIABLE_T(LOGMSG_FILE_DIR), VARIABLE_T(LOGMSG_FILE_NAME));
	_stprintf (log_file, _T("%s/%s/%s/%s"), g_szRuntime_path, VARIABLE_T(LOGMSG_FILE_DIR), g_cfg.m_szGroupName, VARIABLE_T(LOGMSG_FILE_NAME));
    s_hLogFile = _topen (log_file, O_WRONLY | O_CREAT | O_APPEND, 0644);		/* 0644 is an octal number. Meaning here: chmod 0644 i.e. rw r r */
	
    if (s_hLogFile == -1)
    {
		_ftprintf (stderr, _T("%s: cannot open log file %s\n"), VARIABLE_T(APPLICATION_NAME), log_file);
		return -1;
    }
	
    return E_NO_ERROR;
}

/*----------------------------------------------------------------------*
* int_close_log -- Close the log file
*----------------------------------------------------------------------*/
void applog_close_app_log ()
{
    close (s_hLogFile);
}

/*----------------------------------------------------------------------*
* logmsg -- Log a message into the log file with time and user id
*----------------------------------------------------------------------*
* Return Codes:
* -1 when unable to write to error log
*  0 on success
*----------------------------------------------------------------------*/
int32_t logmsg(int32_t pri, const TCHAR* pszMsg, const TCHAR* pszLoginName)
{
    TCHAR line[SPD_MAXLINE];
    TCHAR buf[SPD_MAXLINE];
    TCHAR* pri_str[] ={ _T("???"), _T("PANIC"), _T("ERROR"), _T("WARN"), _T("ADVISE"), _T("STATE"), _T("DEBUG") };
    struct tm* tm_now;
    time_t now;
    int length;
    int k, err;
    struct stat filestat;
    struct timeval gdate;
	
    if (g_nLogToAppLogFile > 0)
    {
        /* Get size of current errlog file */
        if (fstat (s_hLogFile, &filestat) == -1)
            return -1;
		
			/*-----------------------------------------------------------------------
			* If the Log file is larger than limit, it is backed up to an history file
			* and a new log file with the same name is created
        *----------------------------------------------------------------------*/
		if (filestat.st_size >= var_log_size)
		{
			close (s_hLogFile);
            /* Remove the oldest Log file(s) if exists */
            for (k = var_log_count - 1; k < var_log_count + 4; k++)
			{
//				_stprintf(buf, _T("%s/%s/%s.%d"), g_szRuntime_path, VARIABLE_T(LOGMSG_FILE_DIR), VARIABLE_T(LOGMSG_FILE_NAME), k);
				_stprintf(buf, _T("%s/%s/%s/%s.%d"), g_szRuntime_path, VARIABLE_T(LOGMSG_FILE_DIR), g_cfg.m_szGroupName, VARIABLE_T(LOGMSG_FILE_NAME), k);
				if (_taccess (buf, R_OK | W_OK) == 0)
					_tunlink (buf);
			}
			
            /* Rename files 3 to 4, 2 to 3, 1 to 2, 0 to 1 */
            for (k = var_log_count - 2; k >= 0; k--)
			{
// 				_stprintf (buf, _T("%s/%s/%s.%d"), g_szRuntime_path, VARIABLE_T(LOGMSG_FILE_DIR), VARIABLE_T(LOGMSG_FILE_NAME), k);
// 				_stprintf (line, _T("%s/%s/%s.%d"), g_szRuntime_path, VARIABLE_T(LOGMSG_FILE_DIR), VARIABLE_T(LOGMSG_FILE_NAME), k + 1);

				_stprintf (buf, _T("%s/%s/%s/%s.%d"), g_szRuntime_path, VARIABLE_T(LOGMSG_FILE_DIR), g_cfg.m_szGroupName, VARIABLE_T(LOGMSG_FILE_NAME), k);
				_stprintf (line, _T("%s/%s/%s/%s.%d"), g_szRuntime_path, VARIABLE_T(LOGMSG_FILE_DIR), g_cfg.m_szGroupName, VARIABLE_T(LOGMSG_FILE_NAME), k+1);

                
				if (_taccess (line, R_OK | W_OK) != 0)
                    _trename (buf, line);
            }
			
            /* Move current file to LOG.0 */
// 			_stprintf (line, _T("%s/%s/%s.0"), g_szRuntime_path, VARIABLE_T(LOGMSG_FILE_DIR), VARIABLE_T(LOGMSG_FILE_NAME));
// 			_stprintf (buf, _T("%s/%s/%s"), g_szRuntime_path, VARIABLE_T(LOGMSG_FILE_DIR), VARIABLE_T(LOGMSG_FILE_NAME));

			memset(buf, 0, SPD_MAXLINE);
			memset(line, 0, SPD_MAXLINE);
			_stprintf (line, _T("%s/%s/%s/%s.0"), g_szRuntime_path, VARIABLE_T(LOGMSG_FILE_DIR), g_cfg.m_szGroupName, VARIABLE_T(LOGMSG_FILE_NAME));
			_stprintf (buf, _T("%s/%s/%s/%s"), g_szRuntime_path, VARIABLE_T(LOGMSG_FILE_DIR), g_cfg.m_szGroupName, VARIABLE_T(LOGMSG_FILE_NAME));

			int ret = _trename (buf, line);
            if (ret != 0)
			{
                _ftprintf (stderr, _T("%s: rename %s to %s error\n"), VARIABLE_T(APPLICATION_NAME), buf, line);
                return -1;
            }
            /* Create a new log file */
            s_hLogFile = _topen (buf, O_WRONLY | O_CREAT | O_APPEND);
            if (s_hLogFile == -1 || _tchmod (buf, 0644) == -1)
			{
                _ftprintf (stderr, _T("%s/Open: cannot open the log file\n"), VARIABLE_T(APPLICATION_NAME));
                return -1;
            }
        }
    }
	
    if (pri < ELOG_PANIC || pri > ELOG_DEBUG)
        pri = ELOG_ERROR;
	
    gettimeofday (&gdate, NULL);
    now = (time_t) gdate.tv_sec;
    tm_now = localtime (&now);
	
    /* Check for invalid pszLoginName */
    if (pszLoginName != NULL)
    {
        length = _tcslen (pszLoginName);
		
        if (length > 15 || length == 0)
			pszLoginName = _T("INVALID");
    }
	
    /* Check legal priority */
    if (pri > ELOG_DEBUG)
        pri = 0;
	
    /* Print time followed by pszLoginName and string */
#if !defined(_ARCH_I386_NTVC_)
    _tcsftime (buf, SPD_MAXLINE, "%D %T", tm_now);
    if (g_usec_mode)
        _stprintf (line, "%s.%06d %-15s %-6s %s\n", buf, gdate.tv_usec, pszLoginName, pri_str[pri], pszMsg);
    else
        _stprintf (line, "%s %-15s %-6s %s\n", buf, pszLoginName, pri_str[pri], pszMsg);
#else
    if (g_usec_mode)
    {
		USES_CONVERSION;
        _stprintf (line, _T("%.24s [usec].%06d %-15s %-6s %s\n"), A2CW(asctime(tm_now)), gdate.tv_usec, pszLoginName, pri_str[pri], pszMsg);
    }
    else
    {
		USES_CONVERSION;
		_stprintf (line, _T("%.24s %-15s %-6s %s\n"), A2CW(asctime (tm_now)), pszLoginName, pri_str[pri], pszMsg);
    }
#endif
	
    if (g_nLogToAppLogFile > 0)
    {
        /* Write to the log file */
        length = _tcslen (line);
		USES_CONVERSION;
        err = write (s_hLogFile, W2CA(line), length);
		
        if (err == -1)
		{
            if (errno == ENOSPC)
			{
                /* %%% */
                /* We may want to mark a 'File System Full' bit in the database */
                /* We could also remove any core files... */
                /* We could also trim down the number of log files */
            }
            return -1;
        }
    }
    else
    {
        _ftprintf (stderr, line);  
    }
    return 1;
}

int32_t applog_errlog_d(int32_t debugLvl, int32_t pri, const TCHAR* format, ...)
{
    if (g_LogFileSuccessfullyInit == FALSE)
    {
		return -1;
    }
    if (g_cfg.GetDebugLevel() >= debugLvl) 
    {
		va_list args;
		va_start(args, format);
		CString csLine;
		csLine.FormatV(format, args);
		csLine.TrimLeft();
		csLine.TrimRight();
		/* Write to log file */
		logmsg(pri, csLine, VARIABLE_T(APPLICATION_NAME));
    }
    return 1;
}

/*---------------------------------------------------------------------*
* applog_errlog -- a internal version of the errlog function used only by the
* supervisor itself.  It just writes a line of message into the log file directly.
*---------------------------------------------------------------------*/
int32_t applog_errlog(int32_t pri, const TCHAR* format, ...)
{
    if (g_LogFileSuccessfullyInit == FALSE)
		return -1;

	EnterCriticalSection(&g_criticalSection);

    va_list args;
    va_start(args, format);
    CString csLine;
    csLine.FormatV(format, args);
    csLine.TrimLeft();
    csLine.TrimRight();
	
    if (csApplog.Compare(csLine) == 0 && iApplog < 20) 
	{
		iApplog++;
		LeaveCriticalSection(&g_criticalSection); 
		return 1;
    }
    
	int size = csApplog.GetLength();

// 	if (0 == csApplog.GetData()->nRefs)
// 	{
// 		sleep(100);
// 	}

    csApplog.Empty();
    csApplog = csLine;
    
    iApplog = 0;
	
    /* Write to log file */
    logmsg(pri, csLine, VARIABLE_T(APPLICATION_NAME));
	// 16Nov2007 end

	LeaveCriticalSection(&g_criticalSection); 

    return 1;
}


//////////////////////////////////////////////////////////////////////////

/*----------------------------------------------------------------------*
* applog_init_app_log -- Initialization of errlog utility.
* For example, to open log file and version file and store the
* file descriptors in some static variables.
*----------------------------------------------------------------------*/
int32_t applog_init_app_log_tmr(int32_t no_of_log_file, int32_t log_file_size)
{
    TCHAR log_file[512];
	TCHAR log_dir[512];
	
    var_log_count = no_of_log_file;	/* No of Cyclic files */
    var_log_size = log_file_size;	/* 1 MByte logfile size */
	
    /* Insure that environment variables set up properly */
    g_szRuntime_path = _tgetenv (_T("SCADA_PROJECT"));
    if (g_szRuntime_path == NULL || _tcslen (g_szRuntime_path) == 0)
    {
		_ftprintf (stderr, _T("CRITICAL ERROR: Environment variable SCADA_PROJECT not set\n"));
		exit (-1);
    }

	/* Create dir if it not exist */
	_stprintf(log_dir, _T("%s/%s/%s"), g_szRuntime_path, VARIABLE_T(LOGMSG_FILE_DIR), g_cfg.m_szGroupName);
	_tmkdir(log_dir);

    /* Open the log file. */
//	_stprintf (log_file, _T("%s/%s/%s"), g_szRuntime_path, VARIABLE_T(LOGMSG_FILE_DIR), VARIABLE_T(LOGMSG_FILE_NAME_TMR));
    _stprintf (log_file, _T("%s/%s/%s/%s"), g_szRuntime_path, VARIABLE_T(LOGMSG_FILE_DIR), g_cfg.m_szGroupName, VARIABLE_T(LOGMSG_FILE_NAME_TMR));

    logfd_tmr = _topen (log_file, O_WRONLY | O_CREAT | O_APPEND, 0644);		/* 0644 is an octal number. Meaning here: chmod 0644 i.e. rw r r */
	
    if (logfd_tmr == -1)
    {
		_ftprintf (stderr, _T("%s: cannot open log file (tmr) %s\n"), VARIABLE_T(APPLICATION_NAME), log_file);
		return -1;
    }
	
    return E_NO_ERROR;
}

/*----------------------------------------------------------------------*
* int_close_log -- Close the log file
*----------------------------------------------------------------------*/
void applog_close_app_log_tmr()
{
    close (logfd_tmr);
}

int32_t applog_errlog_tmr_d(int32_t debugLvl, int32_t pri, const TCHAR* format, ...)
{
    if (g_LogFileSuccessfullyInit == FALSE)
    {
		return -1;
    }
    if (g_cfg.GetDebugLevel() >= debugLvl) 
    {
		va_list args;
		va_start(args, format);
		CString csLine;
		csLine.FormatV(format, args);
		csLine.TrimLeft();
		csLine.TrimRight();
		/* Write to log file */
		logmsg_tmr(pri, csLine, VARIABLE_T(APPLICATION_NAME));
    }
    return 1;
}

#define OPCLOGFILE	"C:\\opclog.txt"
#define OPCLOGFILE_SZ	10240000
#define OPCLOGFILE_VER	3


int32_t
opclog_msg(int32_t pri, const TCHAR* pszMsg, const TCHAR* pszLoginName)
{
    char string[256];
    FILE *pfile = NULL;
    struct  stat buf;
    static int opcvernum = 0;
	
    if ((pfile = fopen(OPCLOGFILE, "a")) != NULL) {
		
		fprintf(pfile, "%s: %s\n", pszLoginName, pszMsg);
        fclose(pfile);
		pfile = NULL;
		
		if (stat(OPCLOGFILE, &buf) == 0) {
			
            /* maintain the file size */
            if (buf.st_size > OPCLOGFILE_SZ) {
				strcpy(string, OPCLOGFILE);
				
				/* rename the old file with the version number */
				if (opcvernum == OPCLOGFILE_VER)   opcvernum = 0;
				sprintf(string, "%s%s%d", string, ".", opcvernum);
				
				/* blow the old file away */
				if (access(string, F_OK) == 0) unlink(string);
				rename(OPCLOGFILE, string);
				opcvernum++;
			}
		}
		
    }
	
	
    return 0;
}
/*---------------------------------------------------------------------*
* applog_errlog_tmr -- a internal version of the errlog function used only by the
* timer thread itself.  It just writes a line of message into the log file directly.
*---------------------------------------------------------------------*/
int32_t applog_errlog_tmr(int32_t pri, const TCHAR* format, ...)
{
    if (g_LogFileSuccessfullyInit == FALSE)
		return -1;
    va_list args;
    va_start(args, format);
    CString csLine;
    csLine.FormatV(format, args);
	//    csLine.TrimLeft();
	//  csLine.TrimRight();
    /* Write to log file */

	std::wcout<<L"errlogTmr: "<<format<<std::endl;

    logmsg_tmr(pri, csLine, VARIABLE_T(APPLICATION_NAME));
	
	
    return 1;
}

/*----------------------------------------------------------------------*
* logmsg_tmr -- Log a message into the log file with time and user id
*----------------------------------------------------------------------*
* Return Codes:
* -1 when unable to write to error log
*  0 on success
*----------------------------------------------------------------------*/
int32_t logmsg_tmr(int32_t pri, const TCHAR* pszMsg, const TCHAR* pszLoginName)
{
    TCHAR line[SPD_MAXLINE];
    TCHAR buf[SPD_MAXLINE];
    TCHAR* pri_str[] ={ _T("???"), _T("PANIC"), _T("ERROR"), _T("WARN"), _T("ADVISE"), _T("STATE"), _T("DEBUG") };
    struct tm *tm_now;
    time_t now;
    int length;
    int k, err;
    struct stat filestat;
    struct timeval gdate;

#ifdef USE_STD_OPCLOG
    opclog_msg(pri, pszMsg, pszLoginName);
#else
    if (g_nLogToAppLogFile > 0)
    {
        /* Get size of current errlog file */
			if (fstat (logfd_tmr, &filestat) == -1)
			{
				return -1;
			}
		
		/*-----------------------------------------------------------------------
		* If the Log file is larger than limit, it is backed up to an history file
		* and a new log file with the same name is created
        *----------------------------------------------------------------------*/
		if (filestat.st_size >= var_log_size)
		{
			close (logfd_tmr);
			
			/* Remove the oldest Log file(s) if exists */
			for (k = var_log_count - 1; k < var_log_count + 4; k++)
			{
//				_stprintf (buf, _T("%s/%s/%s.%d"), g_szRuntime_path, LOGMSG_FILE_DIR, LOGMSG_FILE_NAME_TMR, k);
				_stprintf (buf, _T("%s/%s/%s/%s.%d"), g_szRuntime_path, VARIABLE_T(LOGMSG_FILE_DIR), g_cfg.m_szGroupName, VARIABLE_T(LOGMSG_FILE_NAME_TMR), k);
				if (_taccess (buf, R_OK | W_OK) == 0)
					_tunlink (buf);
			}
			
			/* Rename files 3 to 4, 2 to 3, 1 to 2, 0 to 1 */
			for (k = var_log_count - 2; k >= 0; k--)
			{
// 				_stprintf (buf, _T("%s/%s/%s.%d"), g_szRuntime_path, LOGMSG_FILE_DIR, LOGMSG_FILE_NAME_TMR, k);
// 				_stprintf (line, _T("%s/%s/%s.%d"), g_szRuntime_path, LOGMSG_FILE_DIR, LOGMSG_FILE_NAME_TMR, k + 1);

				_stprintf (buf, _T("%s/%s/%s/%s.%d"), g_szRuntime_path, VARIABLE_T(LOGMSG_FILE_DIR), g_cfg.m_szGroupName, VARIABLE_T(LOGMSG_FILE_NAME_TMR), k);
				_stprintf (line, _T("%s/%s/%s/%s.%d"), g_szRuntime_path, VARIABLE_T(LOGMSG_FILE_DIR), g_cfg.m_szGroupName, VARIABLE_T(LOGMSG_FILE_NAME_TMR), k + 1);
				if (_taccess (line, R_OK | W_OK) != 0)
				{
					int ret = _trename (buf, line);
					if (0 != ret)
					{
						std::wcerr<<L"Rename "<<buf<<L" to "<<line<<L" failed:"<<ret<<std::endl;
					}
				}
			}
			
			/* Move current file to LOG.0 */
// 			_stprintf (line, _T("%s/%s/%s.0"), g_szRuntime_path, LOGMSG_FILE_DIR, LOGMSG_FILE_NAME_TMR);
// 			_stprintf (buf, _T("%s/%s/%s"), g_szRuntime_path, LOGMSG_FILE_DIR, LOGMSG_FILE_NAME_TMR);

			_stprintf (line, _T("%s/%s/%s/%s.0"), g_szRuntime_path, VARIABLE_T(LOGMSG_FILE_DIR), g_cfg.m_szGroupName, VARIABLE_T(LOGMSG_FILE_NAME_TMR));
			_stprintf (buf, _T("%s/%s/%s/%s"), g_szRuntime_path, VARIABLE_T(LOGMSG_FILE_DIR), g_cfg.m_szGroupName, VARIABLE_T(LOGMSG_FILE_NAME_TMR));
			if (_trename (buf, line) != 0)
			{
				_ftprintf (stderr, _T("%s: rename %s to %s error\n"), VARIABLE_T(APPLICATION_NAME), buf, line);
				return -1;
			}
			/* Create a new log file */
			logfd_tmr = _topen (buf, O_WRONLY | O_CREAT | O_APPEND);
			if (logfd_tmr == -1 || _tchmod (buf, 0644) == -1)
			{
				_ftprintf (stderr, _T("%s/Open: cannot open the log file\n"), VARIABLE_T(APPLICATION_NAME));
				return -1;
			}
        }
    }
	
    if (pri < ELOG_PANIC || pri > ELOG_DEBUG)
        pri = ELOG_ERROR;

	
    gettimeofday (&gdate, NULL);
    now = (time_t) gdate.tv_sec;
    tm_now = localtime (&now);
	
    /* Check for invalid pszLoginName */
    if (pszLoginName != NULL){
        length = _tcslen (pszLoginName);
		
        if (length > 15 || length == 0)
			pszLoginName = _T("INVALID");
    }
	
    /* Check legal priority */
    if (pri > ELOG_DEBUG)
        pri = 0;
	
    /* Print time followed by pszLoginName and string */
#if !defined(_ARCH_I386_NTVC_)
    strftime (buf, SPD_MAXLINE, "%D %T", tm_now);
    if (g_usec_mode)
        _stprintf (line, "%s.%06d %-15s %-6s %s\n", buf, gdate.tv_usec, pszLoginName, pri_str[pri], pszMsg);
    else
        _stprintf (line, "%s %-15s %-6s %s\n", buf, pszLoginName, pri_str[pri], pszMsg);
#else
	//    if (g_usec_mode)
	//        _stprintf (line, _T("%.24s [usec].%06d %-15s %-6s %s\n"), asctime (tm_now),
	//                    gdate.tv_usec, pszLoginName, pri_str[pri], pszMsg);
	//    else
	//        _stprintf (line, _T("%.24s %-15s %-6s %s\n"), asctime (tm_now), pszLoginName,
	//                    pri_str[pri], pszMsg);
    if (g_usec_mode)
    {
		USES_CONVERSION;
        _stprintf (line, _T("%.24s [usec].%06d %-15s %-6s %s\n"), A2CW(asctime (tm_now)), gdate.tv_usec, pszLoginName, pri_str[pri], pszMsg);
    }
    else
    {
		USES_CONVERSION;
		_stprintf (line, _T("%.24s %-15s %-6s %s\n"), A2CW(asctime (tm_now)), pszLoginName, pri_str[pri], pszMsg);
    }
#endif
	
    if (g_nLogToAppLogFile > 0)
    {
        /* Write to the log file */
        length = _tcslen (line);
		USES_CONVERSION;
        err = write (logfd_tmr, W2CA(line), length);
		
        if (err == -1)
		{
            if (errno == ENOSPC)
			{
                /* %%% */
                /* We may want to mark a 'File System Full' bit in the database */
                /* We could also remove any core files... */
                /* We could also trim down the number of log files */
            }
            return -1;
        }
    }
    else
    {
        _ftprintf (stderr, line);  
    }
#endif
    return 1;
}
