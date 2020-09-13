# Delete Folder Content Windows Service
This project is a simple Delete Folder Content Windows Service.


## How to install Windows Service
 - sc.exe create "Delete Folder Content Worker" binpath= C:\path\to\exe\DeleteFolderContentService.exe start= auto

## How to delete Windows Service
 - sc.exe delete "Delete Folder Content Worker" binpath= C:\path\to\exe\DeleteFolderContentService.exe start= auto
