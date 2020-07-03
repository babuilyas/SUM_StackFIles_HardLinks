# SUM_StackFiles_HardLinks
The project is aimed to assists SAP BASIS and technical consultants dealing regularly with Maintenance Planner and SUM tool.

### Maintenance Planner
Usually consultants plan the target system and generates Stack XML and Push Files to Download Basket and then you know well what you do.

### Whats New
Using this method, download 1 file only once and this can save plenty of disk space and more importantly reduces your wait time.
Additionally this can help you organize the data in more professional way being in SAP BASIS/Technical job.

## How-to it works:
1. After pushing files to donwload basket, goto additonal downloads and download **Export to Excel**
2. Now put **Stack XML** and **Export to Excel** in a separate **Folder** and move this folder the **drive** where most of your already download exists
   - I created foloer called **FilesDump** where all SAP files resides
3. Rename **Export to Excel** to **stackfiles.xls**
4. Copy **Generate_HardLinks.exe** to the folder and **execute**
5. The executable generates hard links to already existing files
   - and generates js file with missing files to donwload
6. Now goto SWDC Download Basket and scroll to end so all files are loaded and select all
7. Copy the js and run the code snippet in browser developer console and execute
   - the script will unselect/uncheck all files you need
8. Remove rest of files from the Basket and Download
9. Repeat 4, **execute** and it will create remaining links and you're ready to go


The project is only available for windows but can used alternatively for UNIX and other environments. 

