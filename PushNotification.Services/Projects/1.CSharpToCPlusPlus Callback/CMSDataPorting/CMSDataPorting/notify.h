/* $Id$ */
/*----------------------------------------------------------------------*
 *    Copyright (c) Willowglen MSC Bhd 1999-2001.
 *
 *    No portion of this code may be reused, modified or
 *    distributed in any way without the expressed written
 *    consent of Willowglen MSC Bhd.
 *
 *    Application :  Historical Transfer Service (HTS)
 *    Description :  Define all Notify Code Number for the HTS
 *----------------------------------------------------------------------*/
#ifndef _HTS_NOTIFY_H_
#define _HTS_NOTIFY_H_

/* Define the maximum number of data to update to HistCurr for each loop */
#define MAX_NOTIFY_UPDATE	10

/* 
** Below is the definition for detail notification code for attribute 
** that came back and the list of notify that we will received 
*/
#define NOTE_ABASE_VALUE            0x01 /* 
				     ** Notification for value in 
				     ** AnalogBase 
				     */
#define NOTE_DBASE_STATUS           0x02 /* 
				     ** Notification for status in 
				     ** DigitalBase 
				     */
#define NOTE_MBASE_STATUS	        0x03 /* 
				     ** Notification for status in 
				     ** MiscBase 
				     */
#define NOTE_ABASE_DQ               0x11 /* 
				     ** Notification for data quality 
				     ** in AnalogBase 
				     */
#define NOTE_DBASE_DQ               0x12 /* 
				     ** Notification for data quality 
				     ** in DigitalBase 
				     */
#define NOTE_MBASE_DQ		        0x13 /* 
				     ** Notification for data quality 
				     ** in MiscBase 
				     */
#define NOTE_ABASE_INSERT           0x21 /* 
				     ** Notification for insert in 
				     ** AnalogBase 
				     */
#define NOTE_DBASE_INSERT	        0x22 /* 
				     ** Notification for insert in 
				     ** DigitalBase 
				     */
#define NOTE_MBASE_INSERT	        0x23 /* 
				     ** Notification for insert in 
				     ** MiscBase 
				     */
#define NOTE_SYSNAME_INSERT         0x25 /* 
				     ** Notification for insert in 
				     ** SYSNAME 
				     */

#define NOTE_ABASE_DELETE           0x31 /* 
				     ** Notification for delete in 
				     ** AnalogBase 
				     */
#define NOTE_DBASE_DELETE	        0x32 /* 
				     ** Notification for delete in 
				     ** DigitalBase 
				     */
#define NOTE_MBASE_DELETE	        0x33 /* 
				     ** Notification for delete in 
				     ** MiscBase 
				     */
#define NOTE_SYSNAME_DELETE         0x35 /* 
				     ** Notification for delete in 
				     ** SYSNAME 
				     */
#define NOTE_HTSNAME_DELETE         0x36 /* 
				     ** Notification for delete in 
				     ** HTSNAME 
				     */
#define NOTE_INSERT_ALMEVT          0x41 /* 
				     ** Notification for insert in 
				     ** ALMEVT 
				     */
#define NOTE_CHG_FLAGACK            0x42 /* 
				     ** Notification for changes of 
				     ** Falg Acknowledgement */

#define NOTE_CHG_MOVE_STATUS        0x51 /* 
				     ** Notification for changes of 
				     ** move status 
				     */

#define NOTE_INSERT_CACCESS         0x61 /* Notification for insert of 
					card access log */

#ifdef _GEOPLIN_PROJECT_
#define NOTE_CHG_CP_PASSWORD        0x71 /* Notificaion for Password changes in
				      * Custom Permission table for Slovenia
				      * (C20) project */
#endif

#define NOTE_CHG_CARD_READER        0x81

#define NOTE_CHG_CARD_USER          0x82

#endif /* _HTS_NOTIFY_H_ */
