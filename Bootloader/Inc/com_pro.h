
/* Define to prevent recursive inclusion -------------------------------------*/
#ifndef __COM_PRO_H
#define __COM_PRO_H

/* Includes ------------------------------------------------------------------*/
#include "stm32l0xx_hal.h"

typedef enum
{
	UART_IF =0,
	USB_IF       ,
	ETHERNET_IF  ,
	BLE_IF      ,
	InterfqceNbr
}ProInterface;

/* Exported types ------------------------------------------------------------*/
/* Exported constants --------------------------------------------------------*/

/* Exported functions ------------------------------------------------------- */
void     pro_Init(ProInterface interface);
uint8_t  Packet_Receive(uint8_t* data, uint16_t length);
uint8_t  Packet_Send(uint8_t* data, uint16_t length);
uint8_t  Packet_Received(void);
void Send_NACK(void);
void Send_ACK(void);

#endif  /* __COM_PRO_H */

