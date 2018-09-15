
/* Includes ------------------------------------------------------------------*/
#include "main.h"
#include "boot_app.h"

#define HOOK  0x46
#define ERASE 0x45
#define WRITE 0x57
#define CHECK 0x43
#define JUMP  0x4A

#define BOOT_FLAG_ADDRESS           0x08001000
#define APPLICATION_START_ADDRESS   0x08008000
#define FLASH_USER_END_ADDR         0x0800A000  

/* Private function prototypes -----------------------------------------------*/
static uint32_t boot_Erase(uint8_t* aRxBuffer);
static uint32_t boot_Write(uint8_t* aRxBuffer);
static uint32_t boot_Check(uint8_t* aRxBuffer);
static uint32_t boot_Hook (uint8_t* aRxBuffer);

/* Private functions ---------------------------------------------------------*/

/**
  * @brief  boot_Engine.
  * @param  None
  * @retval None
  */
uint32_t boot_Engine(uint8_t* aRxBuffer,uint16_t* NextFrameSize)
{
    uint32_t result     = HAL_OK;
    static uint8_t WriteSeqNbr     = 0;
    static uint8_t TotalWriteFrame = 0;

    switch(aRxBuffer[0])
    {
		case HOOK:
            result = boot_Hook(&aRxBuffer[1]);
			*NextFrameSize = ERASE_FRAME_SIZE;
            break;
        case ERASE:
            result = boot_Erase(&aRxBuffer[1]);
			*NextFrameSize = WRITE_FRAME_SIZE;
            break;
        case WRITE:
		    TotalWriteFrame = aRxBuffer[1];
		    WriteSeqNbr     = aRxBuffer[2];
            result = boot_Write(&aRxBuffer[3]);
			if(WriteSeqNbr <= TotalWriteFrame)
			{
			  *NextFrameSize = WRITE_FRAME_SIZE;
			}
			else
			{
			  *NextFrameSize = CHECK_FRAME_SIZE;
			}
            break;
        case CHECK:
            result = boot_Check(&aRxBuffer[1]);
			*NextFrameSize = JUMP_FRAME_SIZE;
            break;
        case JUMP:
            JumpToApplication();
            break;
        default: // Unsupported command
            result = HAL_ERROR;
            break;   
    }
	
	return result;  
}

static uint32_t boot_Hook(uint8_t* aRxBuffer)
{   
   //Authentification will be done Here
    uint32_t result = HAL_OK;

	
	return result;
}


static uint32_t boot_Erase(uint8_t* aRxBuffer)
{   
    uint32_t result = HAL_OK;

    if(aRxBuffer[0] == 0xFF)
    {
        // global erase: not supported
        result =HAL_ERROR;
    }
    else     
    {  
		if (EraseFlash( APPLICATION_START_ADDRESS,FLASH_USER_END_ADDR) != HAL_OK)
        { 
            result =HAL_ERROR;
        }
		else
	    {
			result =HAL_OK;
		}
    }
    
	return result;
}


static uint32_t boot_Write(uint8_t* aRxBuffer)
{   
    uint32_t result = HAL_OK;
    uint32_t startingAddress;
    
    // Get starting address
    startingAddress = (uint32_t)(aRxBuffer[0] + (aRxBuffer[1] << 8) 
                    + (aRxBuffer[2] << 16) + (aRxBuffer[3] << 24));
     
    //write data into flash	
	if (WriteFlash(startingAddress, &aRxBuffer[5],(uint8_t)aRxBuffer[4]) != HAL_OK)
    { 
        result =HAL_ERROR;
    }
	else
	{
		result =HAL_OK;
	}
	
	return result;
}


static uint32_t boot_Check(uint8_t* aRxBuffer)
{
	uint32_t result = HAL_OK;
  /*  startingAddress = 0;
    uint32_t address;
    uint32_t *data;
    uint32_t crcResult;*/
    // Receive the starting address and checksum
    // Address = 4 bytes
    // Checksum = 1 byte
   /*if(HAL_UART_Receive_DMA(&UartHandle, (uint8_t *)aRxBuffer,4) != HAL_OK)
              {
                 Error_Handler();
              }*/
    

    
    
    // Set the starting address
  /* startingAddress = (uint8_t *)(aRxBuffer[0] + (aRxBuffer[1] << 8) 
                    + (aRxBuffer[2] << 16) + (aRxBuffer[3] << 24));*/
    
    // Receive the ending address and checksum
    // Address = 4 bytes
    // Checksum = 1 byte
  /* if(HAL_UART_Receive_DMA(&UartHandle, (uint8_t *)aRxBuffer,4) != HAL_OK)
              {
                 Error_Handler();
              }*/
    
 
    
    
    // Set the starting address
   /* endingAddress = aRxBuffer[0] + (aRxBuffer[1] << 8) 
                    + (aRxBuffer[2] << 16) + (aRxBuffer[3] << 24);
    
    __HAL_RCC_CRC_CLK_ENABLE();
   
    data = (uint32_t *)((__IO uint32_t*) startingAddress);
   for(address =(uint32_t) startingAddress; address < endingAddress; address += 4)
    {
        data = (uint32_t *)((__IO uint32_t*) address);
        crcResult = HAL_CRC_Accumulate(&CrcHandle,data,1);
    }
    
   __HAL_RCC_CRC_CLK_DISABLE();
    if(crcResult == 0x00)
    {*/
  /*  }
    else
    {
  
    }*/
    
    return result;
}

//Check the Hash of App and compqreted to the stored value if it is correct return Ok else return error
uint32_t boot_CheckApp(void)
{
	uint32_t result = HAL_OK;

    return result;
}

void JumpToApplication(void)
{
    if (((*(__IO uint32_t*)APPLICATION_START_ADDRESS) & 0x2FFE0000 ) == 0x20000000)
    {
       //REset all periphearls.
        HAL_DeInit();
  
        // Disable any interrupt set by bootloader
       __disable_irq();
       __ISB();
       
        /* Get the main application start address */
        uint32_t jump_address = *(__IO uint32_t *)(APPLICATION_START_ADDRESS + 4);

        /* Set the main stack pointer to to the application start address */
        __set_MSP(*(__IO uint32_t *)APPLICATION_START_ADDRESS);

        // Create function pointer for the main application
        void (*pmain_app)(void) = (void (*)(void))(jump_address);

        // Now jump to the main application
        pmain_app();
    }   
}
