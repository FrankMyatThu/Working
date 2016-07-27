-- --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
-- Create DB Script(s)
-- --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
CREATE TABLE `tbl_MusicDictionary` (
	`ID`	INTEGER PRIMARY KEY,
	`Srno`	INTEGER NOT NULL,
	`FileName`	TEXT NOT NULL,
	`EnglishTitle`	TEXT NOT NULL,
	`MyanmarTitle`	TEXT NOT NULL,
	`AlbumName`	TEXT,
	`AlbumArt`	TEXT,
	`Length`	TEXT,
	`Genre`	TEXT,
	`Lyric`	TEXT,
	`IsFavorite`	TEXT,
	`PlayingStatus`	TEXT
);
CREATE TABLE `tbl_Setting` (
	`IsFavoriteOn`	TEXT,
	`RepeatStatus`	TEXT,
	`IsShuffleOn`	TEXT,
	`MyanmarFontName`	TEXT
);
-- --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
-- Insert Script(s)
-- --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
INSERT INTO tbl_MusicDictionary
(ID, Srno, FileName, EnglishTitle, MyanmarTitle, AlbumName, AlbumArt, Length, Genre, Lyric, IsFavorite, PlayingStatus)
VALUES
(1, 1, 'a0171721', 'Ae Thal', 'ဧည့္သည္', 'Ninzi May', 'album_art', '2:42', 'Pop', 'ဧည့္သည္', 'false', 'PlayingStatus_New'),
(2, 2, 'b0271721', 'Mu Yar Mar Yar Moe', 'မူရာမာယာမိုး', 'Ninzi May', 'album_art', '2:04', 'Pop', 'မူရာမာယာမိုး', 'false', 'PlayingStatus_New'),
(3, 3, 'c0371721', 'Ti Ah Mo', 'တီအားမို', 'Ninzi May', 'album_art', '2:19', 'Pop', 'တီအားမို', 'false', 'PlayingStatus_New'),
(4, 4, 'd0471721', 'Sane Yet Lai Ah', 'စိမ္းရက္ေလအား', 'Ninzi May', 'album_art', '1:56', 'Pop', 'စိမ္းရက္ေလအား', 'false', 'PlayingStatus_New'),
(5, 5, 'e0571721', 'Cherry Lan', 'ခ်ယ္ရီလမ္း', 'Ninzi May', 'album_art', '3:21', 'Rock', 'ခ်ယ္ရီလမ္း', 'false', 'PlayingStatus_New'),
(6, 6, 'f0671721', 'Nway Oo Pone Pyin', 'ေႏြဦးပံုျပင္', 'Ninzi May', 'album_art', '3:16', 'Pop', 'ေႏြဦးပံုျပင္', 'false', 'PlayingStatus_New'),
(7, 7, 'g0771721', 'Na Lone Thar Myoe Taw', 'ႏွလံုးသားၿမိဳ႕ေတာ္', 'Ninzi May', 'album_art', '3:06', 'Pop', 'ႏွလံုးသားၿမိဳ႕ေတာ္', 'false', 'PlayingStatus_New'),
(8, 8, 'h0871721', 'Ta Khar Ka Lat Saung', 'တစ္ခါကလက္ေဆာင္', 'Ninzi May', 'album_art', '3:54', 'Pop', 'တခါကလက္ေဆာင္', 'false', 'PlayingStatus_New'),
(9, 9, 'i0971721', 'Bae Thu Ko Lauk Chit Tha Lae', 'ဘယ္သူကိုယ့္ေလာက္ခ်စ္သလဲ', 'Ninzi May', 'album_art', '5:04', 'Pop', 'ဘယ္သူကိုယ့္ေလာက္ခ်စ္သလဲ', 'false', 'PlayingStatus_New'),
(10, 10, 'j1071721', 'Min Thi Naing Ma Lar', 'မင္းသိႏိုင္မလား', 'Ninzi May', 'album_art', '4:50', 'Pop', 'မင္းသိႏိုင္မလား', 'false', 'PlayingStatus_New'),
(11, 11, 'k1171721', 'A Chit Htet Ma Ka', 'အခ်စ္ထက္မက', 'Ninzi May', 'album_art', '5:04', 'Pop', 'အခ်စ္ထက္မက', 'false', 'PlayingStatus_New'),
(12, 12, 'l1271721', 'Pyaw Par Sae Thu Nge Chin', 'ေပ်ာ္ပါေစသူငယ္ခ်င္း', 'Ninzi May', 'album_art', '03:49', 'Pop', 'ေပ်ာ္ပါေစသူငယ္ခ်င္း', 'false', 'PlayingStatus_New');
INSERT INTO tbl_Setting
(IsFavoriteOn, RepeatStatus, IsShuffleOn, MyanmarFontName)
VALUES 
('false', 'ALL', 'false', 'Zawgyi');