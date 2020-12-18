# Rewrite to C#

# Check if not Null, and return
def validateInt(x):
  if x is not None:
    return str(x)
  return '0'

# Create string/int for MAL XML file

# Add texts on beginning of file
def line_prepender(filename, line):
    with open(filename, 'r+', encoding='utf-8') as f:
        content = f.read()
        f.seek(0, 0)
        f.write(line + '\n' + content)

# Global Vars

# Get Username
userName = input("Enter your Anilist Username: ")

# Query for User ID
queryUser = '''
query ($userName: String) { User (search: $userName)
	{
		id
	}
}
'''
varUser = {
  'userName': "'" + userName + "'"
}

# Start App
# Get USER ID, from USERNAME
try:
  response = requests.post(url, json={'query': queryUser, 'variables': varUser})
except:
  print("Internet error! Check your connection.")
  exit

# If successful, get User ID from Username
if (response.status_code == 200):
  jsonParsed = json.loads(response.content)
  userID = jsonParsed["data"]["User"]["id"]
  print("User ID: " + str(userID))
else:
  print("Cannot get User!")
  print(response.content)
  userID = 0

if userID > 0:
  # Query for Anime
  query = '''
  query ($userID: Int, $MEDIA: MediaType) {
  MediaListCollection (userId: $userID, type: $MEDIA) { 
    lists {
      status
      entries
      {
        status
        completedAt {
          year
          month
          day
        }
        startedAt {
          year
          month
          day
        }
        progress
        progressVolumes
        score
        notes
        media
        {
          id
          idMal
          season
          seasonYear
          format
          source
          episodes
          chapters
          volumes
          title
          {
            english
            romaji
          }
          description
          coverImage
          {
            medium
          }
          synonyms
        }
      }
    }
  }
  }
  '''  

  # Get Anime List
  outputAnime = "anime.json"
  xmlAnime = "anime.xml"
  if not (os.path.exists(outputAnime)):
      varQueryAnime = {
          'userID': userID,
          'MEDIA' : 'ANIME'
      }

      # jsonResult = ""
      response = requests.post(url, json={'query': query, 'variables': varQueryAnime})
      if (response.status_code == 200):
        jsonParsed = json.loads(response.content)
        listAnime = jsonParsed["data"]["MediaListCollection"]["lists"]
        print("anime list is generated!")
        
        # Write to json file
        write_append(outputAnime, '[\n')

        # Count Manga entries
        cTotal = 0
        cWatch = 0
        cComplete = 0
        cHold = 0
        cDrop = 0
        cPtw = 0

        # Iterate over the MediaCollection List
        for anime in listAnime:
          animeInfo = anime["entries"]
          # Iterate over the anime information, inside the entries
          for entry in animeInfo:
            # Write to json file
            jsontoAdd = entry_json(entry, 'anime')
            write_append(outputAnime, jsontoAdd)

            # Write to MAL Xml File
            malID = validateInt(entry["media"]["idMal"])
            if malID != '0':
              # Get XML strings
              xmltoWrite = entry_animexml(malID, entry)
              # Write to xml file
              write_append(xmlAnime, xmltoWrite)
            
              # Add count
              malstatus = validateStr(entry["status"])
              if (malstatus == "COMPLETED"):
                cComplete = cComplete + 1
              elif (malstatus == "PAUSED"):
                cHold = cHold + 1
              elif (malstatus == "CURRENT"):
                cWatch = cWatch + 1
              elif (malstatus == "DROPPED"):
                cDrop = cDrop + 1
              elif (malstatus == "PLANNING"):
                cPtw = cPtw + 1

        # Delete last comma ',', in json file
        write_remove(outputAnime, 3)

        # Write ']' at the end, to json file
        write_append(outputAnime, '\n]')
        
        # Write to MAL xml file
        write_append(xmlAnime, '</myanimelist>')

        # Total counts
        cTotal = cWatch + cComplete + cHold + cDrop + cPtw

        malprepend = '<?xml version="1.0" encoding="UTF-8" ?>\n<myanimelist>\n'
        malprepend += '\t<myinfo>\n'
        malprepend += '\t\t' + toMalval('', 'user_id') + '\n'
        malprepend += '\t\t' + toMalval(userName, 'user_name') + '\n'
        malprepend += '\t\t' + toMalval('1', 'user_export_type') + '\n'
        malprepend += '\t\t' + toMalval(str(cTotal), 'user_total_anime') + '\n'
        malprepend += '\t\t' + toMalval(str(cWatch), 'user_total_watching') + '\n'
        malprepend += '\t\t' + toMalval(str(cComplete), 'user_total_completed') + '\n'
        malprepend += '\t\t' + toMalval(str(cHold), 'user_total_onhold') + '\n'
        malprepend += '\t\t' + toMalval(str(cDrop), 'user_total_dropped') + '\n'
        malprepend += '\t\t' + toMalval(str(cPtw), 'user_total_plantowatch') + '\n'
        malprepend += '\t</myinfo>\n'
        line_prepender(xmlAnime, malprepend)

        # Done anime
        print("Done! File generated: " + outputAnime)
        print("Done! File generated: " + xmlAnime)

      else:
        print("Anime Request Error! [Status code: " + str(response.status_code) + "]")
        print(response.content)
  else:
    print("anime.json file already exist!")

  #############################################################################################################  
  # Get Manga List
  outputManga = "manga.json"
  xmlManga = "manga.xml"
  if not (os.path.exists(outputManga)):
    varQueryManga = {
        'userID': userID,
        'MEDIA' : 'MANGA'
    }

    response = requests.post(url, json={'query': query, 'variables': varQueryManga})
    if (response.status_code == 200):
      jsonParsed = json.loads(response.content)
      listManga = jsonParsed["data"]["MediaListCollection"]["lists"]
      print("manga list is generated!")
      
      # Write to json file
      write_append(outputManga, '[\n')

      # Count Manga entries
      cTotal = 0
      cRead = 0
      cComplete = 0
      cHold = 0
      cDrop = 0
      cPtr = 0

      # Iterate over the MediaCollection List
      for manga in listManga:
        mangaInfo = manga["entries"]
        # Iterate over the manga information, inside the entries
        for entry in mangaInfo:
          # Write to json file
          jsontoAdd = entry_json(entry, 'manga')
          write_append(outputManga, jsontoAdd)

          # Write to MAL Xml File
          malID = validateInt(entry["media"]["idMal"])
          if malID != '0':
            # Get XML strings
            xmltoWrite = entry_mangaxml(malID, entry)
            # Write to xml file
            write_append(xmlManga, xmltoWrite)

            # Add count
            malstatus = validateStr(entry["status"])
            if (malstatus == "COMPLETED"):
              cComplete = cComplete + 1
            elif (malstatus == "PAUSED"):
              cHold = cHold + 1
            elif (malstatus == "CURRENT"):
              cRead = cRead + 1
            elif (malstatus == "DROPPED"):
              cDrop = cDrop + 1
            elif (malstatus == "PLANNING"):
              cPtr = cPtr + 1

      # Delete last comma ',', in json file
      write_remove(outputManga, 3)

      # Write ']' at the end, to json file
      write_append(outputManga, '\n]')
      
      # Write to MAL xml file
      write_append(xmlManga, '</myanimelist>')

      # Total counts
      cTotal = cRead + cComplete + cHold + cDrop + cPtr

      malprepend = '<?xml version="1.0" encoding="UTF-8" ?>\n<myanimelist>\n'
      malprepend += '\t<myinfo>\n'
      malprepend += '\t\t' + toMalval('', 'user_id') + '\n'
      malprepend += '\t\t' + toMalval(userName, 'user_name') + '\n'
      malprepend += '\t\t' + toMalval('2', 'user_export_type') + '\n'
      malprepend += '\t\t' + toMalval(str(cTotal), 'user_total_manga') + '\n'
      malprepend += '\t\t' + toMalval(str(cRead), 'user_total_reading') + '\n'
      malprepend += '\t\t' + toMalval(str(cComplete), 'user_total_completed') + '\n'
      malprepend += '\t\t' + toMalval(str(cHold), 'user_total_onhold') + '\n'
      malprepend += '\t\t' + toMalval(str(cDrop), 'user_total_dropped') + '\n'
      malprepend += '\t\t' + toMalval(str(cPtr), 'user_total_plantoread') + '\n'
      malprepend += '\t</myinfo>\n'
      line_prepender(xmlManga, malprepend)

      # Done manga
      print("Done! File generated: " + outputManga)
      print("Done! File generated: " + xmlManga)

    else:
      print("Manga Request Error! [Status code: " + str(response.status_code) + "]")
      print(response.content)
  else:
    print("manga.json file already exist!")