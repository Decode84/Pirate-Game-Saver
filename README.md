# Pirate Game Server

This is a fairly simple application, all it does is copy game files to another folder. It has a pattern regonition. So you can specify which files you want to copy. Later on I would like to add functionality to store them on a server or do something like Steam Cloud. But this is quite simple and useful for now. 

The software generates a .json file where the following can be specified. 

```json
[
  {
    "GameName": "The Witcher 3",
    "SavedFilesFolderPath": "C:\\Users\\USER\\Documents\\The Witcher 3\\gamesaves",
    "FilePatterns": [
      "*.sav",
      "*.png"
    ],
    "DestinationFolderPath": "C:\\Users\\USER\\Documents\\SavedFiles\\The Witcher 3"
  },
  {
    "GameName": "Bioshock",
    "SavedFilesFolderPath": "C:\\Users\\USER\\Documents\\BioshockHD\\BioShock",
    "FilePatterns": [
      "*.bsb"
    ],
    "DestinationFolderPath": "C:\\Users\\USER\\Documents\\SavedFiles\\Bioshock"
  }
]
```