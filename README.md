# delete-folder-content-windows-service
This project is a simple Delete Folder Content Windows Service.


## Create Windows Service
	- sc.exe create "Delete Folder Content Worker" binpath= C:\temp\WindowsServices\DeleteFolderContentWorker\DeleteFolderContentService.exe start= auto

## Delete Windows Service
	- sc.exe delete "Delete Folder Content Worker" binpath= C:\temp\WindowsServices\DeleteFolderContentWorker\DeleteFolderContentService.exe start= auto