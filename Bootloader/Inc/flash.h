
/* Define to prevent recursive inclusion -------------------------------------*/
#ifndef __FLASH_H
#define __FLASH_H

/* Includes ------------------------------------------------------------------*/
#include "stm32l0xx_hal.h"

/* Exported types ------------------------------------------------------------*/
/* Exported constants --------------------------------------------------------*/


/* Error code */
enum 
{
  FLASHIF_OK = 0,
  FLASHIF_ERASEKO,
  FLASHIF_WRITINGCTRL_ERROR,
  FLASHIF_WRITING_ERROR,
  FLASHIF_PROTECTION_ERRROR
};

/* protection type */  
enum{
  FLASHIF_PROTECTION_NONE         = 0,
  FLASHIF_PROTECTION_PCROPENABLED = 0x1,
  FLASHIF_PROTECTION_WRPENABLED   = 0x2,
  FLASHIF_PROTECTION_RDPENABLED   = 0x4,
};

/* protection update */
enum {
	FLASHIF_WRP_ENABLE,
	FLASHIF_WRP_DISABLE
};

/* Define the address from where user application will be loaded.
   Note: this area is reserved for the IAP code                  */
#define FLASH_PAGE_STEP         FLASH_PAGE_SIZE           /* Size of page : 2 Kbytes */

/* Notable Flash addresses */
#define BOOT_FLAG_ADDRESS           0x08001000
#define APPLICATION_START_ADDRESS   0x08008000
#define FLASH_USER_END_ADDR         0x0800A000

/* Define the user application size */
#define USER_FLASH_SIZE               ((uint32_t)FLASH_USER_END_ADDR - APPLICATION_START_ADDRESS) /* Small default template application */


/* Exported macro ------------------------------------------------------------*/
/* ABSoulute value */
#define ABS_RETURN(x,y)               (((x) < (y)) ? (y) : (x))

#define  ARRAY4_TO_U32_LE(a)     ( (((uint32_t)a[3]) << 24) + (((uint32_t)a[2]) << 16) + (((uint32_t)a[1]) << 8) + ((uint32_t)a[0]) )

/* Get the number of sectors from where the user program will be loaded */

/* Compute the mask to test if the Flash memory, where the user program will be
  loaded, is write protected */
#define FLASH_PROTECTED_SECTORS       (~(uint32_t)((1 << FLASH_SECTOR_NUMBER) - 1))
/* Exported functions ------------------------------------------------------- */
void FLASH_Init(void);
uint32_t WriteFlash( uint32_t Address,uint8_t* Buffer , uint8_t size);
uint32_t EraseFlash( uint32_t startAdress,uint32_t EndAdress);

#endif  /* __FLASH_H */

