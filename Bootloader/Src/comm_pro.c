
/* Includes ------------------------------------------------------------------*/
#include "main.h"
#include "com_pro.h"

/* Private typedef -----------------------------------------------------------*/
/* Private define ------------------------------------------------------------*/
#define ACK 0x06
#define NACK  0x16

/* Private macro -------------------------------------------------------------*/
/* Private variables ---------------------------------------------------------*/
/* UART handler declaration */
UART_HandleTypeDef UartHandle;
/* DMA Handle declaration */
DMA_HandleTypeDef     DmaHandle;
__IO ITStatus Packet_Recvd         = RESET;
__IO ITStatus Packet_TxInProgress  = RESET;

/* Public functions ---------------------------------------------------------*/
static void Error_Handler(void);
/**
  * @brief  uart_init.
  * @param  None
  * @retval None
  */
void   pro_Init(ProInterface interface)
{	
	if(interface == UART_IF)
	{
	  /*##-1- Configure the UART peripheral ######################################*/
	  /* Put the USART peripheral in the Asynchronous mode (UART Mode) */
	  /* UART1 configured as follow:
		  - Word Length = 8 Bits
		  - Stop Bit = One Stop bit
		  - Parity = None
		  - BaudRate = 9600 baud
		  - Hardware flow control disabled (RTS and CTS signals) */
	  UartHandle.Instance        = USARTx;
	  UartHandle.Init.BaudRate   = 115200;
	  UartHandle.Init.WordLength = UART_WORDLENGTH_8B;
	  UartHandle.Init.StopBits   = UART_STOPBITS_1;
	  UartHandle.Init.Parity     = UART_PARITY_NONE;
	  UartHandle.Init.HwFlowCtl  = UART_HWCONTROL_NONE;
	  UartHandle.Init.Mode       = UART_MODE_TX_RX;
	  
	  HAL_UART_Init(&UartHandle);
          
          
           /*## -1- Enable DMA1 clock #################################################*/
           __HAL_RCC_DMA1_CLK_ENABLE();
            
           /*## -2- Select the DMA functional Parameters ###############################*/
           DmaHandle.Init.Direction = DMA_MEMORY_TO_MEMORY;          /* M2M transfer mode                */           
           DmaHandle.Init.PeriphInc = DMA_PINC_ENABLE;               /* Peripheral increment mode Enable */                 
           DmaHandle.Init.MemInc = DMA_MINC_ENABLE;                  /* Memory increment mode Enable     */                   
           DmaHandle.Init.PeriphDataAlignment = DMA_PDATAALIGN_WORD; /* Peripheral data alignment : Word */    
           DmaHandle.Init.MemDataAlignment = DMA_MDATAALIGN_WORD;    /* memory data alignment : Word     */     
           DmaHandle.Init.Mode = DMA_NORMAL;                         /* Normal DMA mode                  */  
           DmaHandle.Init.Priority = DMA_PRIORITY_HIGH;              /* priority level : high            */  
           
            /*## -5- Initialize the DMA stream ##########################################*/
           if(HAL_DMA_Init(&DmaHandle) != HAL_OK)
            {
             /* Initialization Error */
              Error_Handler();  
            } 

	}
}
static void Error_Handler(void)
{
    /* Turn LED2 off */
    BSP_LED_Off(LED2);
    while(1)
    {
    }
}
uint8_t Packet_Receive(uint8_t* data, uint16_t length) 
{
  uint8_t s32RetVal = HAL_OK;
  while(Packet_TxInProgress ==SET){};
      if(HAL_UART_Receive_DMA(&UartHandle, (uint8_t *)data,length) != HAL_OK)
      {
        s32RetVal = HAL_ERROR;
      }
 

   return(s32RetVal);
}

uint8_t Packet_Send(uint8_t* data, uint16_t length) 
{
	uint8_t s32RetVal = HAL_OK;
	
  if(HAL_UART_Transmit_DMA(&UartHandle, (uint8_t*)data, length)!= HAL_OK)
  {
    s32RetVal  = HAL_ERROR;
  }
  else
  {
    Packet_TxInProgress = SET ;   //flag to avoid using the driver until onging operation finishs
  }

   return(s32RetVal);
}

uint8_t  Packet_Received(void)
{
	uint8_t s32RetVal = HAL_OK;
	
  if(Packet_Recvd == RESET)
  {
    s32RetVal = HAL_ERROR;
  }
  else
  {
	Packet_Recvd = RESET;
	s32RetVal = HAL_OK; 
  }

   return(s32RetVal);
}

void Send_ACK(void)
{
    uint8_t msg[2] = {ACK, ACK};
    (void)Packet_Send(msg,2);
}

void Send_NACK(void)
{
   uint8_t msg[2] = {NACK, NACK};
   (void)Packet_Send(msg,2);
}

/**
  * @brief  Tx Transfer completed callback
  * @param  UartHandle: UART handle. 
  * @note   This example shows a simple way to report end of DMA Tx transfer, and 
  *         you can add your own implementation. 
  * @retval None
  */
void HAL_UART_TxCpltCallback(UART_HandleTypeDef *UartHandle)
{
  Packet_TxInProgress = RESET;
}

/**
  * @brief  Rx Transfer completed callback
  * @param  UartHandle: UART handle
  * @note   This example shows a simple way to report end of DMA Rx transfer, and 
  *         you can add your own implementation.
  * @retval None
  */
void HAL_UART_RxCpltCallback(UART_HandleTypeDef *UartHandle)
{
  Packet_Recvd = SET;
}

/**
  * @brief  UART error callbacks
  * @param  UartHandle: UART handle
  * @note   This example shows a simple way to report transfer error, and you can
  *         add your own implementation.
  * @retval None
  */
void HAL_UART_ErrorCallback(UART_HandleTypeDef *UartHandle)
{
  while(1)
  {
  }
}

/**
  * @}
  */

/**
  * @}
  */

/************************ (C) COPYRIGHT STMicroelectronics *****END OF FILE****/
