select Id, NickName, LastName, Email from Person where dbo.[ufnUtility_RemoveNonAlphaCharacters](replace(replace(replace(Email, '_', ''), '@', ''), '.', '')) != replace(replace(replace(Email, '_', ''), '@', ''), '.', '')