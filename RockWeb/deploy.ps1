# This script is run by AppVeyor's deploy agent after the deploy
 
 # move content directory back from temp
 Move-Item c:\webdata\rock.ccvonline.com\Content c:\webdata\rock.ccvonline.com\docs 
 
 # copy new connection string file
 Copy-Item c:\webdata\rock.ccvonline.com\config\web.ConnectionStrings.config c:\webdata\rock.ccvonline.com\docs