# This script is run by AppVeyor's deploy agent after the deploy
Import-Module WebAdministration
 
# start website
Start-WebSite 'RockRMS'

# move content directory back from temp
Move-Item c:\webdata\rock.ccvonline.com\temp\Content c:\webdata\rock.ccvonline.com\docs 

# copy new connection string file
Copy-Item c:\webdata\rock.ccvonline.com\config\web.ConnectionStrings.config c:\webdata\rock.ccvonline.com\docs