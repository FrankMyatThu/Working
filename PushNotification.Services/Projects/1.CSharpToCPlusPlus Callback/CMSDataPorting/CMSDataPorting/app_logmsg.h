/*******************************************************************************
| File: app_logmsg.h		| Module: OPCServer	                       |
|------------------------------------------------------------------------------|
| Project: 05      | Targeted Hardware: PC	     | Targeted OS: Windows    |
|==============================================================================|
|                       Revision History                                       |
|                       ----------------                                       |
|  Date       By   Version            Comments                                 |
|(dd/mm/yyyy)                                                                  |
|----------|-----|-------|-----------------------------------------------------|
|10/10/2006| SAK | 1.0.2 | - Modifications for Proj 371 (EMA)                  |
|07Oct2007 | LSP | 2.0.0 | - Modifications for OPC Client, to support UNICODE  |
*******************************************************************************/

/*
 *    Copyright (c) Willowglen Services Pte Ltd 2006.
 *
 *    No portion of this code may be reused, modified or
 *    distributed in any way without the expressed written
 *    consent of Willowglen Services Pte. Ltd.
 *
 */

#ifndef _APP_LOG_MSG_H_
#define _APP_LOG_MSG_H_

/* Macro for app_errlog() in app_logmsg.cpp */
#define LOGMSG_FILE_NAME		"OPCClient_ErrLog"
#define LOGMSG_FILE_NAME_TMR	"OPCClient_ErrLog_TmrThr"
#define LOGMSG_FILE_DIR			"errlog"
#define DEFAULT_LOG_SIZE	1024000	/* Default size of log file */
#define DEFAULT_LOG_COUNT	5		/* Default log file count */

#define VARIABLE_T(n)			_T(##n)		// Generic Text mapping for variables representing strings
#define APPLICATION_NAME	"CMSDataPorting"

// Global variables
extern BOOL g_LogFileSuccessfullyInit;	// Determines if the log file functionality has been successfully initialised 

/* error log in logmsg */
extern int32_t applog_init_app_log (int32_t no_of_log_file, int32_t log_file_size);
extern void applog_close_app_log ();
extern int32_t applog_errlog_d(int32_t debugLvl, int32_t pri, const TCHAR* format, ...);
extern int32_t applog_errlog (int32_t pri, const TCHAR* format, ...);

/* This shall be static function */
extern int32_t logmsg (int32_t pri, const TCHAR* msg, const TCHAR* loginname);

extern int32_t applog_init_app_log_tmr (int32_t no_of_log_file, int32_t log_file_size);
extern void applog_close_app_log_tmr ();
extern int32_t applog_errlog_tmr_d(int32_t debugLvl, int32_t pri, const TCHAR* format, ...);
extern int32_t applog_errlog_tmr (int32_t pri, const TCHAR* format, ...);

/* This shall be static function */
extern int32_t logmsg_tmr (int32_t pri, const TCHAR* msg, const TCHAR* loginname);

extern ConfigInfo g_cfg;
int g_nLogToAppLogFile = 1;

#endif
