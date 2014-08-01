The files in this directory are used to install the Rock RMS.

CREATING INSTALLER ZIP FILE
<><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><>


___ 1. Download the source from the GitHub release tag

___ 2. Build solution in Visual Studio (IMPORTANT: build in Release mode) After downloading you may
       need to make the RockWeb project the startup project.

___ 3. Edit web.config
         
        * Delete the line:   <add key="AutoMigrateDatabase" value="False"/>
        * Set RunJobsInIISContext = true
        * Turn off debug <compilation debug="false"

___ 4. Copy the Rock.x.y.z.nupkg file from
       https://github.com/SparkDevNetwork/Rock-UpdatePackageBuilder/tree/master/InstallerArtifacts
       to the App_Data/Packages folder.  Remove any earlier versions of the Rock.*.nupkg file.
       
___ 5. Copy the RockUpdate-X-Y-Z.x.y.z.nupkg file from
       https://github.com/SparkDevNetwork/Rock-UpdatePackageBuilder/tree/master/InstallerArtifacts
       to the App_Data/Packages folder.
       
___ 6. Delete the following files from the RockWeb directory

       * .gitignore (do a search as there are files in several directory)
        *.pdb (do a search as there are several files )
        * Settings.StyleCop

___ 7. Zip up the RockWeb directory leaving out the following files:


___ 8. Rename zip file 'rock-install-latest.zip'

___ 9. Move copy of zip to ./Installers/RockInstaller/Install Versions/vX.Y.Z/ so that it  
       will be in source control

___ 10. Overwrite with snapshot zip file to Azure Blog storage (rockrms/install/<version>/Data)
        Note the <version> label is the installer version not the Rock version. This should not
        be incremented except when the installer scripts get updated.

CREATING SQL FILE
<><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><>

___ 1. Add a web.ConnectionStrings.config to the downloaded project above and give it a 
       fresh database name.

___ 2. Run 'update-database' in Visual Studio package manager so that a new database is made.

___ 3. Open SQL Server Manager

___ 4. Right-click on the new database and select 'Tasks > Generate Scripts'

___ 5. Click 'Next' until you see the screen with the 'Advanced' button.

___ 6. Save the file as 'sql-install.sql'

___ 7. Click the 'Advanced' button and change the setting for 'Types of data to script' to 
       'schema and data'.

___ 8. Click 'Next' then 'Finished'

___ 9. Open the SQL file and make the following edits:
         * delete from the start of the file to the beginning of the first comment of the first stored proc
         * remove the follow four lines from the end of the script
              USE [master]
              GO
              ALTER DATABASE [RockRMS_NewDbName] SET  READ_WRITE 
              GO

___ 10. Zip the file into a new file named 'sql-latest.sql'

___ 11. Move copy of zip to ./Installers/RockInstaller/Install Versions/vX.Y.Z/ so that 
        it will be in source control

___ 12. Overwrite with snapshot zip file to Azure Blog storage
        (rockrms/install/<version>/Data)
