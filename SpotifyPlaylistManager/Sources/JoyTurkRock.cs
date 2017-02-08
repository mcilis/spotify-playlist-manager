using System;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Configuration;
using System.Threading.Tasks;
using SpotifyPlaylistManager.Models;

namespace SpotifyPlaylistManager.Sources
{
    public class JoyTurkRock
    {
        public static async Task<Song> GetCurrentSongAsync()
        {
            // {"data": {"current_song": {"title": "Ya\u015famak \u0130stemem", "artist": "Yavuz \u00c7etin"}}}

            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage()
                {
                    RequestUri = new Uri(ConfigurationManager.AppSettings["Source.JoyTurkRock"]),
                    Method = HttpMethod.Get
                };

                var response = await client.SendAsync(request);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    try
                    {
                        var currentSongResult = JObject.Parse(responseContent);
                        var song = new Song
                        {
                            Artist = currentSongResult["data"]["current_song"]["artist"].ToString().Trim(),
                            TrackName = currentSongResult["data"]["current_song"]["title"].ToString().Trim()
                        };
                        FileHelper.LogTrace($"JoyTurkRock.GetCurrentSongAsync(): {song.Artist} - {song.TrackName}");
                        return song;
                    }
                    catch (Exception exception)
                    {
                        FileHelper.LogError($"JoyTurkRock.GetCurrentSongAsync \n Error: {exception.Message} \n Content: {responseContent}");
                    }
                }
            }
            return null;
        }
    }
}

/*
{
	"result": true,
	"messages": [],
	"data": {
		"current_song": {
			"id": "1298",
			"song_db_id": "46203",
			"title": "Ya\u015famak \u0130stemem",
			"artist_id": "3003",
			"artist": "Yavuz \u00c7etin",
			"feat_artist_id": "",
			"feat_artist": "",
			"cover_art": "http:\/\/mediacdn.karnaval.com\/media\/album_media\/5479\/albumcover_400x400\/cover_5479.jpg?v=270612020605",
			"biography": "Yavuz \u00c7etin (Alt\u0131n \u00c7ocuk) , T\u00fcrk m\u00fczisyen, gitarist.\n\nGer\u00e7ek Ad\u0131 : Yavuz \u00c7etin\nDo\u011fum Tarihi : 25 Eyl\u00fcl 1970\nDo\u011fum Yeri : Samsun , T\u00fcrkiye \n\nHakk\u0131nda:\n\n-\u0130lk adl\u0131 alb\u00fcm\u00fcn\u00fc Stop M\u00fczik\u2019ten \u00e7\u0131kar\u0131r. Alb\u00fcm\u00fcnde yer alan, Erke\u011fin Olmak istiyorum, ayr\u0131ca Sinan \u00c7etin\u2019in y\u00f6netti\u011fi Propaganda filminde kullan\u0131lan, Erkan O\u011fur\u2019un perdesiz gitar performans\u0131n\u0131n da yer ald\u0131\u011f\u0131 D\u00fcnya isimli enstr\u00fcmantal \u015farkisi en bilinenleridir.\n-MFO ile konserlerde \u00e7almaya ve Yavuz \u00c7etin Group isimli grubuyla bar performans\u0131n\u0131 devam etti\u011fi sure i\u00e7erisinde, ikinci alb\u00fcm \u00e7al\u0131\u015fmalar\u0131na da baslar.\n-\u0130kinci alb\u00fcm\u00fc \"Sat\u0131l\u0131k\" i\u00e7in st\u00fcdyoya girer. S\u00f6z\u00fc, m\u00fczi\u011fi ve d\u00fczenlemeleri kendisine ait bir \u00e7al\u0131\u015fmaya son kez imza atar.",
			"biography_html": "Yavuz \u00c7etin (Alt\u0131n \u00c7ocuk) , T\u00fcrk m\u00fczisyen, gitarist.<br \/>\n<br \/>\nGer\u00e7ek Ad\u0131 : Yavuz \u00c7etin<br \/>\nDo\u011fum Tarihi : 25 Eyl\u00fcl 1970<br \/>\nDo\u011fum Yeri : Samsun , T\u00fcrkiye <br \/>\n<br \/>\nHakk\u0131nda:<br \/>\n<br \/>\n-\u0130lk adl\u0131 alb\u00fcm\u00fcn\u00fc Stop M\u00fczik\u2019ten \u00e7\u0131kar\u0131r. Alb\u00fcm\u00fcnde yer alan, Erke\u011fin Olmak istiyorum, ayr\u0131ca Sinan \u00c7etin\u2019in y\u00f6netti\u011fi Propaganda filminde kullan\u0131lan, Erkan O\u011fur\u2019un perdesiz gitar performans\u0131n\u0131n da yer ald\u0131\u011f\u0131 D\u00fcnya isimli enstr\u00fcmantal \u015farkisi en bilinenleridir.<br \/>\n-MFO ile konserlerde \u00e7almaya ve Yavuz \u00c7etin Group isimli grubuyla bar performans\u0131n\u0131 devam etti\u011fi sure i\u00e7erisinde, ikinci alb\u00fcm \u00e7al\u0131\u015fmalar\u0131na da baslar.<br \/>\n-\u0130kinci alb\u00fcm\u00fc \"Sat\u0131l\u0131k\" i\u00e7in st\u00fcdyoya girer. S\u00f6z\u00fc, m\u00fczi\u011fi ve d\u00fczenlemeleri kendisine ait bir \u00e7al\u0131\u015fmaya son kez imza atar.",
			"biography_de": "",
			"biography_html_de": "",
			"biography_en": "",
			"biography_html_en": "",
			"biography_image": "http:\/\/mediacdn.karnaval.com\/media\/artist_media\/3003\/biography_270x202\/photo_3003_0.jpg?v=2812120320",
			"biography_image_16_9": "http:\/\/mediacdn.karnaval.com\/media\/artist_media\/3003\/biography_1280x720\/photo_3003_0.jpg?v=2812120320",
			"big_biography_image": "http:\/\/mediacdn.karnaval.com\/media\/artist_media\/3003\/biography_640x474\/photo_3003_0.jpg?v=2812120320",
			"big_biography_image_16_9": "http:\/\/mediacdn.karnaval.com\/media\/artist_media\/3003\/biography_1920x1080\/photo_3003_0.jpg?v=2812120320",
			"biography_image_list": {
				"image": ["http:\/\/mediacdn.karnaval.com\/media\/artist_media\/3003\/biography_270x202\/photo_3003_0.jpg?v=2812120320", "http:\/\/mediacdn.karnaval.com\/media\/artist_media\/3003\/biography_270x202\/photo_3003_1.jpg?v=2812120321", "http:\/\/mediacdn.karnaval.com\/media\/artist_media\/3003\/biography_270x202\/photo_3003_2.jpg?v=2812120322"]
			},
			"biography_image_list_16_9": {
				"image": ["http:\/\/mediacdn.karnaval.com\/media\/artist_media\/3003\/biography_1280x720\/photo_3003_0.jpg?v=2812120320", "http:\/\/mediacdn.karnaval.com\/media\/artist_media\/3003\/biography_1280x720\/photo_3003_1.jpg?v=2812120321", "http:\/\/mediacdn.karnaval.com\/media\/artist_media\/3003\/biography_1280x720\/photo_3003_2.jpg?v=2812120322"]
			},
			"big_biography_image_list": {
				"image": ["http:\/\/mediacdn.karnaval.com\/media\/artist_media\/3003\/biography_640x474\/photo_3003_0.jpg?v=2812120320", "http:\/\/mediacdn.karnaval.com\/media\/artist_media\/3003\/biography_640x474\/photo_3003_1.jpg?v=2812120321", "http:\/\/mediacdn.karnaval.com\/media\/artist_media\/3003\/biography_640x474\/photo_3003_2.jpg?v=2812120322"]
			},
			"big_biography_image_list_16_9": {
				"image": ["http:\/\/mediacdn.karnaval.com\/media\/artist_media\/3003\/biography_1920x1080\/photo_3003_0.jpg?v=2812120320", "http:\/\/mediacdn.karnaval.com\/media\/artist_media\/3003\/biography_1920x1080\/photo_3003_1.jpg?v=2812120321", "http:\/\/mediacdn.karnaval.com\/media\/artist_media\/3003\/biography_1920x1080\/photo_3003_2.jpg?v=2812120322"]
			},
			"biography_image_mobile": "http:\/\/mediacdn.karnaval.com\/media\/artist_media\/3003\/biography_1280x720\/photo_3003_0.jpg?v=2812120320",
			"biography_image_mobile_hq": "http:\/\/mediacdn.karnaval.com\/media\/artist_media\/3003\/biography_1920x1080\/photo_3003_0.jpg?v=2812120320",
			"duration": "5:47",
			"duration_sec": "347",
			"ticketmaster": "",
			"buy_now_url": "",
			"timestamp": "1485512209.4582",
			"artist_2_id": "0",
			"artist_2": "",
			"artist_3_id": "0",
			"artist_3": "",
			"artist_4_id": "0",
			"artist_4": "",
			"artist_5_id": "0",
			"artist_5": "",
			"song_artist_twitter_id": "",
			"url_share": "http:\/\/karnaval.com\/sarkilar\/yasamak-istemem-46203",
			"sanitized_url": "yasamak-istemem",
			"artist_sanitized_url": "yavuz-cetin",
			"feat_artist_sanitized_url": "yavuz-cetin",
			"artist_2_sanitized_url": "",
			"artist_3_sanitized_url": "",
			"artist_4_sanitized_url": "",
			"artist_5_sanitized_url": "",
			"lyrics": "",
			"lyrics_html": "",
			"position": "129",
			"current_time": "1485512338.98",
			"time_elapsed": "129521.75415039"
		},
		"next_song": {
			"id": "400",
			"song_db_id": "41913",
			"title": "Mecburen",
			"artist_id": "229",
			"artist": "MF\u00d6",
			"feat_artist_id": "",
			"feat_artist": "",
			"cover_art": "http:\/\/mediacdn.karnaval.com\/media\/album_media\/7449\/albumcover_400x400\/cover_7449.jpg?v=301112032802",
			"biography": "Mazhar Alanson, Fuat G\u00fcner ve \u00d6zkan U\u011fur\u2019dan olu\u015fan T\u00fcrk pop ve rock m\u00fczik grubu.\n\nHakk\u0131nda:\n- Mazhar Alanson ve Fuat G\u00fcner, \u00f6ncelikle Kayg\u0131s\u0131zlar grubunda Ali Serdar, Semih Oksay ve Fikret K\u0131z\u0131lok gibi isimlerle, sonras\u0131nda ise Bar\u0131\u015f Man\u00e7o ile beraber \u00e7al\u0131\u015ft\u0131lar. Bu d\u00f6nemde \u00d6zkan U\u011fur da Kurtalan Ekspres, Erkin Koray, Ersen ve Dada\u015flar'la beraberdi.\n- 1984'te \"Ele G\u00fcne Kar\u015f\u0131 Yapayaln\u0131z\" alb\u00fcm\u00fcn\u00fc \u00e7\u0131karan grup, bu alb\u00fcmle bir y\u0131l boyunca zirvede kalmay\u0131 ba\u015fard\u0131. Bu alb\u00fcmden sonra 1985'te \"Peki Peki Anlad\u0131k\", 1986'da \"Vak The Rock\", 1987'de \"No Problem\", 1989'da \"The Best Of MF\u00d6\", 1990'da \"Geldiler\", 1992'de \"Agannaga R\u00fc\u015fvet\" ve \"D\u00f6nmem Yolumdan\" 1995'te \"M.V.A.B\", 2003'te \"MF\u00d6\" single ve \"Collection\", 2006'da da \"AGU\" alb\u00fcmleri piyasaya \u00e7\u0131km\u0131\u015ft\u0131r.\n- MF\u00d6, \u00fclkemizi Eurovision \u015eark\u0131 Yar\u0131\u015fmas\u0131'nda iki kere temsil etmi\u015ftir.  \u0130lk temsil etti\u011fi 1985 senesinde sunucunun Mazhar, Fuat, \u00d6zkan ismini okuyamad\u0131\u011f\u0131ndan gruba k\u0131saca \"MF\u00d6\" olarak hitap etmesiyle 1985'ten sonra grup MF\u00d6 ad\u0131yla an\u0131lmaya ba\u015flam\u0131\u015ft\u0131r.\n\n\u0130lk Alb\u00fcm: Ele G\u00fcne Kar\u015f\u0131 Yapayaln\u0131z\n\u0130lk Single: A\u015f\u0131k Oldum",
			"biography_html": "Mazhar Alanson, Fuat G\u00fcner ve \u00d6zkan U\u011fur\u2019dan olu\u015fan T\u00fcrk pop ve rock m\u00fczik grubu.<br \/>\n<br \/>\nHakk\u0131nda:<br \/>\n- Mazhar Alanson ve Fuat G\u00fcner, \u00f6ncelikle Kayg\u0131s\u0131zlar grubunda Ali Serdar, Semih Oksay ve Fikret K\u0131z\u0131lok gibi isimlerle, sonras\u0131nda ise Bar\u0131\u015f Man\u00e7o ile beraber \u00e7al\u0131\u015ft\u0131lar. Bu d\u00f6nemde \u00d6zkan U\u011fur da Kurtalan Ekspres, Erkin Koray, Ersen ve Dada\u015flar'la beraberdi.<br \/>\n- 1984'te \"Ele G\u00fcne Kar\u015f\u0131 Yapayaln\u0131z\" alb\u00fcm\u00fcn\u00fc \u00e7\u0131karan grup, bu alb\u00fcmle bir y\u0131l boyunca zirvede kalmay\u0131 ba\u015fard\u0131. Bu alb\u00fcmden sonra 1985'te \"Peki Peki Anlad\u0131k\", 1986'da \"Vak The Rock\", 1987'de \"No Problem\", 1989'da \"The Best Of MF\u00d6\", 1990'da \"Geldiler\", 1992'de \"Agannaga R\u00fc\u015fvet\" ve \"D\u00f6nmem Yolumdan\" 1995'te \"M.V.A.B\", 2003'te \"MF\u00d6\" single ve \"Collection\", 2006'da da \"AGU\" alb\u00fcmleri piyasaya \u00e7\u0131km\u0131\u015ft\u0131r.<br \/>\n- MF\u00d6, \u00fclkemizi Eurovision \u015eark\u0131 Yar\u0131\u015fmas\u0131'nda iki kere temsil etmi\u015ftir.  \u0130lk temsil etti\u011fi 1985 senesinde sunucunun Mazhar, Fuat, \u00d6zkan ismini okuyamad\u0131\u011f\u0131ndan gruba k\u0131saca \"MF\u00d6\" olarak hitap etmesiyle 1985'ten sonra grup MF\u00d6 ad\u0131yla an\u0131lmaya ba\u015flam\u0131\u015ft\u0131r.<br \/>\n<br \/>\n\u0130lk Alb\u00fcm: Ele G\u00fcne Kar\u015f\u0131 Yapayaln\u0131z<br \/>\n\u0130lk Single: A\u015f\u0131k Oldum",
			"biography_de": "",
			"biography_html_de": "",
			"biography_en": "",
			"biography_html_en": "",
			"biography_image": "http:\/\/mediacdn.karnaval.com\/media\/artist_media\/229\/biography_270x202\/photo_229_0.jpg?v=1110160120",
			"biography_image_16_9": "http:\/\/mediacdn.karnaval.com\/media\/artist_media\/229\/biography_1280x720\/photo_229_0.jpg?v=1110160119",
			"big_biography_image": "http:\/\/mediacdn.karnaval.com\/media\/artist_media\/229\/biography_640x474\/photo_229_0.jpg?v=1110160120",
			"big_biography_image_16_9": "http:\/\/mediacdn.karnaval.com\/media\/artist_media\/229\/biography_1920x1080\/photo_229_0.jpg?v=1110160119",
			"biography_image_list": {
				"image": ["http:\/\/mediacdn.karnaval.com\/media\/artist_media\/229\/biography_270x202\/photo_229_0.jpg?v=1110160120"]
			},
			"biography_image_list_16_9": {
				"image": ["http:\/\/mediacdn.karnaval.com\/media\/artist_media\/229\/biography_1280x720\/photo_229_0.jpg?v=1110160119"]
			},
			"big_biography_image_list": {
				"image": ["http:\/\/mediacdn.karnaval.com\/media\/artist_media\/229\/biography_640x474\/photo_229_0.jpg?v=1110160120"]
			},
			"big_biography_image_list_16_9": {
				"image": ["http:\/\/mediacdn.karnaval.com\/media\/artist_media\/229\/biography_1920x1080\/photo_229_0.jpg?v=1110160119"]
			},
			"biography_image_mobile": "http:\/\/mediacdn.karnaval.com\/media\/artist_media\/229\/biography_1280x720\/photo_229_0.jpg?v=1110160119",
			"biography_image_mobile_hq": "http:\/\/mediacdn.karnaval.com\/media\/artist_media\/229\/biography_1920x1080\/photo_229_0.jpg?v=1110160119",
			"duration": "3:01",
			"duration_sec": "181",
			"ticketmaster": "http:\/\/www.biletix.com\/etkinlik\/TKTA3\/TURKIYE\/tr",
			"buy_now_url": "",
			"timestamp": "1485512.21",
			"artist_2_id": "0",
			"artist_2": "",
			"artist_3_id": "0",
			"artist_3": "",
			"artist_4_id": "0",
			"artist_4": "",
			"artist_5_id": "0",
			"artist_5": "",
			"song_artist_twitter_id": "",
			"url_share": "http:\/\/karnaval.com\/sarkilar\/mecburen-41913",
			"sanitized_url": "mecburen",
			"artist_sanitized_url": "mfo",
			"feat_artist_sanitized_url": "mfo",
			"artist_2_sanitized_url": "",
			"artist_3_sanitized_url": "",
			"artist_4_sanitized_url": "",
			"artist_5_sanitized_url": "",
			"lyrics": "Erken kalkmak mecburen <br \/>\n\u0130\u015fe gitmek mecburen <br \/>\nEve d\u00f6nmek mecburen <br \/>\nMecburiyetten <br \/>\n<br \/>\nOh sesleri of olunca <br \/>\nHer kafadan ses \u00e7\u0131k\u0131nca <br \/>\n\u015ea\u015f\u0131r\u0131nca bunal\u0131nca <br \/>\nMecburiyetten <br \/>\n<br \/>\nMecburen mecburen <br \/>\nMecburen mecburen <br \/>\nMecburen mecburen <br \/>\nMecburiyetten <br \/>\n<br \/>\nOlan olsun b\u0131rakt\u0131m <br \/>\nAnlam\u0131 yok zorlaman\u0131n <br \/>\n\u015eans kadere inand\u0131m <br \/>\nMecburiyetten <br \/>\n<br \/>\nBir\u00e7ok g\u00fczeller sevdim <br \/>\nBirini biraz fazla <br \/>\nG\u00f6n\u00fcl e\u015fit sevmiyor <br \/>\nMecburiyetten <br \/>\n<br \/>\nMecburen mecburen <br \/>\nMecburen mecburen <br \/>\nMecburen mecburen <br \/>\nMecburiyetten",
			"lyrics_html": "Erken kalkmak mecburen <br \/>\n\u0130\u015fe gitmek mecburen <br \/>\nEve d\u00f6nmek mecburen <br \/>\nMecburiyetten <br \/>\n<br \/>\nOh sesleri of olunca <br \/>\nHer kafadan ses \u00e7\u0131k\u0131nca <br \/>\n\u015ea\u015f\u0131r\u0131nca bunal\u0131nca <br \/>\nMecburiyetten <br \/>\n<br \/>\nMecburen mecburen <br \/>\nMecburen mecburen <br \/>\nMecburen mecburen <br \/>\nMecburiyetten <br \/>\n<br \/>\nOlan olsun b\u0131rakt\u0131m <br \/>\nAnlam\u0131 yok zorlaman\u0131n <br \/>\n\u015eans kadere inand\u0131m <br \/>\nMecburiyetten <br \/>\n<br \/>\nBir\u00e7ok g\u00fczeller sevdim <br \/>\nBirini biraz fazla <br \/>\nG\u00f6n\u00fcl e\u015fit sevmiyor <br \/>\nMecburiyetten <br \/>\n<br \/>\nMecburen mecburen <br \/>\nMecburen mecburen <br \/>\nMecburen mecburen <br \/>\nMecburiyetten",
			"position": "2426",
			"current_time": "1485512338.98",
			"time_elapsed": "1484026826770"
		},
		"previous_song": {
			"id": "1010",
			"song_db_id": "41397",
			"title": "Anatolia",
			"artist_id": "10717",
			"artist": "Pentagram",
			"feat_artist_id": "",
			"feat_artist": "",
			"cover_art": "http:\/\/mediacdn.karnaval.com\/media\/album_media\/15383\/albumcover_400x400\/cover_15383.jpg?v=010914113434",
			"biography": "1986 y\u0131l\u0131nda kurulan T\u00fcrk heavy metal m\u00fczik grubu.\n\nHakk\u0131nda:\n- Grupta; Hakan Utanga\u00e7, Tarkan G\u00f6z\u00fcb\u00fcy\u00fck, G\u00f6kalp Ergen, Metin T\u00fcrkcan ve Cenk \u00dcnn\u00fc yer almaktad\u0131r.\n- M\u00fczikal kariyerleri grup isimleriyle yay\u0131mlad\u0131klar\u0131 ve 30.000'lik sat\u0131\u015f rakam\u0131yla metal m\u00fczik tarz\u0131nda bir rekora imza atan alb\u00fcmle ba\u015flam\u0131\u015ft\u0131r.\n- \"Anatolia\" adl\u0131 alb\u00fcmlerinin yurtd\u0131\u015f\u0131nda da ilgi \u00e7ekmesiyle Danimarka ve Almanya\u2019da konser vermi\u015ftir.\n- 2001 y\u0131l\u0131nda \u00e7\u0131kan \"Unspoken\" adl\u0131 alb\u00fcmle birlikte grup yurtd\u0131\u015f\u0131nda Mezarkabul olarak tan\u0131nm\u0131\u015ft\u0131r.\n- Tamam\u0131 T\u00fcrk\u00e7e s\u00f6zl\u00fc \u015fark\u0131lardan olu\u015fan \"Bir\" 2002 y\u0131l\u0131nda sat\u0131\u015fa sunulmu\u015ftur.\n- Uzun bir s\u00fcre sadece verdikleri konserlerle m\u00fczi\u011fe devam eden grup, 2012 y\u0131l\u0131nda \"MMXII\" adl\u0131 alb\u00fcm\u00fc yay\u0131mlam\u0131\u015ft\u0131r.\n\n\u0130lk Alb\u00fcm: Pentagram",
			"biography_html": "1986 y\u0131l\u0131nda kurulan T\u00fcrk heavy metal m\u00fczik grubu.<br \/>\n<br \/>\nHakk\u0131nda:<br \/>\n- Grupta; Hakan Utanga\u00e7, Tarkan G\u00f6z\u00fcb\u00fcy\u00fck, G\u00f6kalp Ergen, Metin T\u00fcrkcan ve Cenk \u00dcnn\u00fc yer almaktad\u0131r.<br \/>\n- M\u00fczikal kariyerleri grup isimleriyle yay\u0131mlad\u0131klar\u0131 ve 30.000'lik sat\u0131\u015f rakam\u0131yla metal m\u00fczik tarz\u0131nda bir rekora imza atan alb\u00fcmle ba\u015flam\u0131\u015ft\u0131r.<br \/>\n- \"Anatolia\" adl\u0131 alb\u00fcmlerinin yurtd\u0131\u015f\u0131nda da ilgi \u00e7ekmesiyle Danimarka ve Almanya\u2019da konser vermi\u015ftir.<br \/>\n- 2001 y\u0131l\u0131nda \u00e7\u0131kan \"Unspoken\" adl\u0131 alb\u00fcmle birlikte grup yurtd\u0131\u015f\u0131nda Mezarkabul olarak tan\u0131nm\u0131\u015ft\u0131r.<br \/>\n- Tamam\u0131 T\u00fcrk\u00e7e s\u00f6zl\u00fc \u015fark\u0131lardan olu\u015fan \"Bir\" 2002 y\u0131l\u0131nda sat\u0131\u015fa sunulmu\u015ftur.<br \/>\n- Uzun bir s\u00fcre sadece verdikleri konserlerle m\u00fczi\u011fe devam eden grup, 2012 y\u0131l\u0131nda \"MMXII\" adl\u0131 alb\u00fcm\u00fc yay\u0131mlam\u0131\u015ft\u0131r.<br \/>\n<br \/>\n\u0130lk Alb\u00fcm: Pentagram",
			"biography_de": "",
			"biography_html_de": "",
			"biography_en": "",
			"biography_html_en": "",
			"biography_image": "http:\/\/mediacdn.karnaval.com\/media\/artist_media\/10717\/biography_270x202\/photo_10717_0.jpg?v=0209140514",
			"biography_image_16_9": "http:\/\/mediacdn.karnaval.com\/media\/artist_media\/10717\/biography_1280x720\/photo_10717_0.jpg?v=0209140514",
			"big_biography_image": "http:\/\/mediacdn.karnaval.com\/media\/artist_media\/10717\/biography_640x474\/photo_10717_0.jpg?v=0209140514",
			"big_biography_image_16_9": "http:\/\/mediacdn.karnaval.com\/media\/artist_media\/10717\/biography_1920x1080\/photo_10717_0.jpg?v=0209140514",
			"biography_image_list": {
				"image": ["http:\/\/mediacdn.karnaval.com\/media\/artist_media\/10717\/biography_270x202\/photo_10717_0.jpg?v=0209140514"]
			},
			"biography_image_list_16_9": {
				"image": ["http:\/\/mediacdn.karnaval.com\/media\/artist_media\/10717\/biography_1280x720\/photo_10717_0.jpg?v=0209140514"]
			},
			"big_biography_image_list": {
				"image": ["http:\/\/mediacdn.karnaval.com\/media\/artist_media\/10717\/biography_640x474\/photo_10717_0.jpg?v=0209140514"]
			},
			"big_biography_image_list_16_9": {
				"image": ["http:\/\/mediacdn.karnaval.com\/media\/artist_media\/10717\/biography_1920x1080\/photo_10717_0.jpg?v=0209140514"]
			},
			"biography_image_mobile": "http:\/\/mediacdn.karnaval.com\/media\/artist_media\/10717\/biography_1280x720\/photo_10717_0.jpg?v=0209140514",
			"biography_image_mobile_hq": "http:\/\/mediacdn.karnaval.com\/media\/artist_media\/10717\/biography_1920x1080\/photo_10717_0.jpg?v=0209140514",
			"duration": "4:06",
			"duration_sec": "246",
			"ticketmaster": "",
			"buy_now_url": "",
			"timestamp": "1485511959.2367",
			"artist_2_id": "0",
			"artist_2": "",
			"artist_3_id": "0",
			"artist_3": "",
			"artist_4_id": "0",
			"artist_4": "",
			"artist_5_id": "0",
			"artist_5": "",
			"song_artist_twitter_id": "",
			"url_share": "http:\/\/karnaval.com\/sarkilar\/anatolia-41397",
			"sanitized_url": "anatolia",
			"artist_sanitized_url": "pentagram",
			"feat_artist_sanitized_url": "pentagram",
			"artist_2_sanitized_url": "",
			"artist_3_sanitized_url": "",
			"artist_4_sanitized_url": "",
			"artist_5_sanitized_url": "",
			"lyrics": "Sonsuz karanl\u0131k bu yasl\u0131 g\u00fcn\u00fcmde<br \/>\nyad insan\u011flu bu durmaz s\u00f6z\u00fcnde<br \/>\nnerden bilinmez bu kin g\u00f6zlerinde<br \/>\nyans\u0131r bu korkum sararm\u0131\u015f y\u00fcz\u00fcmde<br \/>\nhalim bilmez, derdim sormaz<br \/>\nzor an\u0131mda, sahip \u00e7\u0131kmaz<br \/>\nboyle \u015fans\u0131z mertlik olmaz<br \/>\nbu ihanet cezas\u0131z kalmaz<br \/>\n<br \/>\nanatolia, anatolia<br \/>\nsevgim seninle bu zor g\u00fcnlerinde<br \/>\nanatolia, anatolia<br \/>\nkalemi k\u0131r cezam\u0131 kes ama onurumu geri ver<br \/>\n<br \/>\nbehey anla derdim bu son \u00e7\u0131\u011fl\u0131\u011f\u0131mda<br \/>\nyorgun bu toprak \u00f6l\u00fcm \u00e7ok yak\u0131nda<br \/>\n<br \/>\ndo\u011fdu\u011fun yer, bu eski d\u00fcnya<br \/>\ndoydu\u011fun yer, bu ya\u015fl\u0131 d\u00fcnya<br \/>\nbir g\u00fcn gelmi\u015f, g\u00fclmez olmu\u015f<br \/>\nismi art\u0131k an\u0131lmaz olmu\u015f<br \/>\n<br \/>\nanatolia, anatolia<br \/>\nsevgim seninle bu zor g\u00fcnlerinde<br \/>\nanatolia, anatolia<br \/>\nkalemi k\u0131r cezam\u0131 kes ama onurumu geri ver<br \/>\n<br \/>\nonurumu geri ver!!! owaki",
			"lyrics_html": "Sonsuz karanl\u0131k bu yasl\u0131 g\u00fcn\u00fcmde<br \/>\nyad insan\u011flu bu durmaz s\u00f6z\u00fcnde<br \/>\nnerden bilinmez bu kin g\u00f6zlerinde<br \/>\nyans\u0131r bu korkum sararm\u0131\u015f y\u00fcz\u00fcmde<br \/>\nhalim bilmez, derdim sormaz<br \/>\nzor an\u0131mda, sahip \u00e7\u0131kmaz<br \/>\nboyle \u015fans\u0131z mertlik olmaz<br \/>\nbu ihanet cezas\u0131z kalmaz<br \/>\n<br \/>\nanatolia, anatolia<br \/>\nsevgim seninle bu zor g\u00fcnlerinde<br \/>\nanatolia, anatolia<br \/>\nkalemi k\u0131r cezam\u0131 kes ama onurumu geri ver<br \/>\n<br \/>\nbehey anla derdim bu son \u00e7\u0131\u011fl\u0131\u011f\u0131mda<br \/>\nyorgun bu toprak \u00f6l\u00fcm \u00e7ok yak\u0131nda<br \/>\n<br \/>\ndo\u011fdu\u011fun yer, bu eski d\u00fcnya<br \/>\ndoydu\u011fun yer, bu ya\u015fl\u0131 d\u00fcnya<br \/>\nbir g\u00fcn gelmi\u015f, g\u00fclmez olmu\u015f<br \/>\nismi art\u0131k an\u0131lmaz olmu\u015f<br \/>\n<br \/>\nanatolia, anatolia<br \/>\nsevgim seninle bu zor g\u00fcnlerinde<br \/>\nanatolia, anatolia<br \/>\nkalemi k\u0131r cezam\u0131 kes ama onurumu geri ver<br \/>\n<br \/>\nonurumu geri ver!!! owaki",
			"position": "379",
			"current_time": "1485512338.98",
			"time_elapsed": "379743.25415039"
		},
		"check": "0.00065302848815918"
	}
}

*/
