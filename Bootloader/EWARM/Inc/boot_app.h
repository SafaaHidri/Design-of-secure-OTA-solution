
/* Define to prevent recursive inclusion -------------------------------------*/
#ifndef __BOOT_APP_H
#define __BOOT_APP_H

/* Includes ------------------------------------------------------------------*/
#include "stm32l0xx_hal.h"
#include "flash.h"


/* Exported types ------------------------------------------------------------*/
/* Exported constants --------------------------------------------------------*/
#define WRITE_FRAME_SIZE    72
#define ERASE_FRAME_SIZE    4
#define JUMP_FRAME_SIZE     1
#define CHECK_FRAME_SIZE    12
#define HOOK_UP_FRAME_SIZE  1
/* Exported functions ------------------------------------------------------- */
uint32_t boot_Engine(uint8_t* aRxBuffer,uint16_t* NextFrameSize);
void     JumpToApplication(void);
uint32_t boot_CheckApp(void);

#endif  /* __BOOT_APP_H */

