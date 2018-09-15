/* Includes ------------------------------------------------------------------*/
#include "flash.h"

static FLASH_EraseInitTypeDef EraseInitStruct;
uint32_t NbOfPages = 0;
/**
  * @brief  Unlocks Flash for write access
  * @param  None
  * @retval None
  */
void FLASH_Init(void)
{
  /* Unlock the Program memory */
  HAL_FLASH_Unlock();

  /* Clear all FLASH flags */
  __HAL_FLASH_CLEAR_FLAG(FLASH_FLAG_WRPERR | FLASH_FLAG_PGAERR | FLASH_FLAG_SIZERR |
                         FLASH_FLAG_OPTVERR | FLASH_FLAG_RDERR | FLASH_FLAG_FWWERR |
                         FLASH_FLAG_NOTZEROERR);
  /* Unlock the Program memory */
  HAL_FLASH_Lock();
}
uint32_t data;
uint32_t WriteFlash( uint32_t Address,uint8_t* Buffer , uint8_t size)
{
   uint32_t i; 
   
   uint32_t result = HAL_OK;
   uint8_t* pBuffer =Buffer;
   uint32_t WriteAddress = Address;
   
   HAL_FLASH_Unlock(); 
   
   for(i=0;i<(size/4);i++)
   {
        data  =  (uint32_t)(Buffer[0] + (Buffer[1] << 8) 
                    + (Buffer[2] << 16) + (Buffer[3] << 24));;
		Buffer+=4;
        
        if (HAL_OK != HAL_FLASH_Program(FLASH_TYPEPROGRAM_WORD, (uint32_t)(WriteAddress), data))       
        {
          result = HAL_ERROR;
          break;
        }		
		
		WriteAddress+=4;
   }
   
   HAL_FLASH_Lock(); 
	
   return result;
}


uint32_t EraseFlash( uint32_t startAdress,uint32_t EndAdress)
{
	uint32_t PageError ; 
    uint32_t result = HAL_OK;
	
	NbOfPages = (EndAdress - startAdress + 1) >> 7;
	
    // Sector erase:
    EraseInitStruct.TypeErase = FLASH_TYPEERASE_PAGES ; 
    // Set the number of sectors to erase
    EraseInitStruct.NbPages = NbOfPages;        
    // Set the initial sector to erase
    EraseInitStruct.PageAddress = startAdress;       
    // perform erase
    HAL_FLASH_Unlock();
    
	if (HAL_FLASHEx_Erase(&EraseInitStruct, &PageError) != HAL_OK)
    {   
        result = HAL_ERROR;
    } 
	
    HAL_FLASH_Lock(); 
	
	return result;
}