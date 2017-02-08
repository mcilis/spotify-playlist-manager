using System;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Configuration;
using System.Threading.Tasks;
using SpotifyPlaylistManager.Models;

namespace SpotifyPlaylistManager.Sources
{
    public class JoyFm
    {
        public static async Task<Song> GetCurrentSongAsync()
        {
            // {"data": {"current_song": {"title": "Could You Be Loved", "artist": "Bob Marley"}}}

            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage()
                {
                    RequestUri = new Uri(ConfigurationManager.AppSettings["Source.JoyFm"]),
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
                        FileHelper.LogTrace($"JoyFm.GetCurrentSongAsync(): {song.Artist} - {song.TrackName}");
                        return song;
                    }
                    catch (Exception exception)
                    {
                        FileHelper.LogError($"JoyFm.GetCurrentSongAsync \n Error: {exception.Message} \n Content: {responseContent}");
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
			"id": "7131",
			"song_db_id": "81215",
			"title": "J'sais pas",
			"artist_id": "16466",
			"artist": "Brigitte",
			"feat_artist_id": "",
			"feat_artist": "",
			"cover_art": "http:\/\/mediacdn.karnaval.com\/media\/album_media\/25070\/albumcover_400x400\/cover_25070.jpg?v=101016025232",
			"biography": "2008 y\u0131l\u0131nda Sylvie Hoarau ve Aur\u00e9lie Saada taraf\u0131ndan kurulan indie folk ikilisi.\n\nHakk\u0131nda:\n- M\u00fczikal kariyeri 2011 y\u0131l\u0131nda yay\u0131mlad\u0131\u011f\u0131 \"Et vous, tu m'aimes?\" isimli alb\u00fcmle ba\u015flad\u0131.\n- \"Battez-vous\", \"Oh la la\" ve \"Coeur de chewing gum\" \u015fark\u0131lar\u0131 m\u00fczik listelerinde ba\u015far\u0131l\u0131 oldu.\n- 2012 y\u0131l\u0131nda \"Encore\" ve 2014'te \"\u00c0 bouche que veux-tu\" alb\u00fcmleri piyasaya \u00e7\u0131kt\u0131.\n\n\u0130lk Alb\u00fcm: Et vous, tu m'aimes?\n\u0130lk Single: Ma Benz",
			"biography_html": "2008 y\u0131l\u0131nda Sylvie Hoarau ve Aur\u00e9lie Saada taraf\u0131ndan kurulan indie folk ikilisi.<br \/>\n<br \/>\nHakk\u0131nda:<br \/>\n- M\u00fczikal kariyeri 2011 y\u0131l\u0131nda yay\u0131mlad\u0131\u011f\u0131 \"Et vous, tu m'aimes?\" isimli alb\u00fcmle ba\u015flad\u0131.<br \/>\n- \"Battez-vous\", \"Oh la la\" ve \"Coeur de chewing gum\" \u015fark\u0131lar\u0131 m\u00fczik listelerinde ba\u015far\u0131l\u0131 oldu.<br \/>\n- 2012 y\u0131l\u0131nda \"Encore\" ve 2014'te \"\u00c0 bouche que veux-tu\" alb\u00fcmleri piyasaya \u00e7\u0131kt\u0131.<br \/>\n<br \/>\n\u0130lk Alb\u00fcm: Et vous, tu m'aimes?<br \/>\n\u0130lk Single: Ma Benz",
			"biography_de": "",
			"biography_html_de": "",
			"biography_en": "",
			"biography_html_en": "",
			"biography_image": "http:\/\/mediacdn.karnaval.com\/media\/artist_media\/16466\/biography_270x202\/photo_16466_0.jpg?v=1010160254",
			"biography_image_16_9": "http:\/\/mediacdn.karnaval.com\/media\/artist_media\/16466\/biography_1280x720\/photo_16466_0.jpg?v=1010160253",
			"big_biography_image": "http:\/\/mediacdn.karnaval.com\/media\/artist_media\/16466\/biography_640x474\/photo_16466_0.jpg?v=1010160254",
			"big_biography_image_16_9": "http:\/\/mediacdn.karnaval.com\/media\/artist_media\/16466\/biography_1920x1080\/photo_16466_0.jpg?v=1010160253",
			"biography_image_list": {
				"image": ["http:\/\/mediacdn.karnaval.com\/media\/artist_media\/16466\/biography_270x202\/photo_16466_0.jpg?v=1010160254"]
			},
			"biography_image_list_16_9": {
				"image": ["http:\/\/mediacdn.karnaval.com\/media\/artist_media\/16466\/biography_1280x720\/photo_16466_0.jpg?v=1010160253"]
			},
			"big_biography_image_list": {
				"image": ["http:\/\/mediacdn.karnaval.com\/media\/artist_media\/16466\/biography_640x474\/photo_16466_0.jpg?v=1010160254"]
			},
			"big_biography_image_list_16_9": {
				"image": ["http:\/\/mediacdn.karnaval.com\/media\/artist_media\/16466\/biography_1920x1080\/photo_16466_0.jpg?v=1010160253"]
			},
			"biography_image_mobile": "http:\/\/mediacdn.karnaval.com\/media\/artist_media\/16466\/biography_1280x720\/photo_16466_0.jpg?v=1010160253",
			"biography_image_mobile_hq": "http:\/\/mediacdn.karnaval.com\/media\/artist_media\/16466\/biography_1920x1080\/photo_16466_0.jpg?v=1010160253",
			"duration": "2:50",
			"duration_sec": "170",
			"ticketmaster": "",
			"buy_now_url": "",
			"timestamp": "1485512236.0941",
			"artist_2_id": "0",
			"artist_2": "",
			"artist_3_id": "0",
			"artist_3": "",
			"artist_4_id": "0",
			"artist_4": "",
			"artist_5_id": "0",
			"artist_5": "",
			"song_artist_twitter_id": "",
			"url_share": "http:\/\/karnaval.com\/sarkilar\/jsais-pas-81215",
			"sanitized_url": "jsais-pas",
			"artist_sanitized_url": "brigitte",
			"feat_artist_sanitized_url": "brigitte",
			"artist_2_sanitized_url": "",
			"artist_3_sanitized_url": "",
			"artist_4_sanitized_url": "",
			"artist_5_sanitized_url": "",
			"lyrics": "J'ai chaud <br \/>\nHa ha ha, ha ha ha ha ha ... <br \/>\nJ'ai chaud <br \/>\nHa ha ha, ha ha ha ha ha ... <br \/>\nJ'ai chaud <br \/>\nHa ha ha, ha ha ha ha ha ...<br \/>\nJ'ai peur<br \/>\nHa ha ha, ha ha ha ha ha ...<br \/>\nJ'ai...<br \/>\n<br \/>\nJ'sais pas<br \/>\nJ'sais pas<br \/>\n<br \/>\nJ'sais pas<br \/>\nJ'sais pas<br \/>\n<br \/>\nJ'ai chaud<br \/>\nJ'sais pas<br \/>\nHa ha ha, ha ha ha ha ha ...<br \/>\nJ'ai chaud<br \/>\nHa ha ha, ha ha ha ha ha ...<br \/>\nJ'ai peur<br \/>\nJ'ai peur<br \/>\n<br \/>\nJ'ai peur !",
			"lyrics_html": "J'ai chaud <br \/>\nHa ha ha, ha ha ha ha ha ... <br \/>\nJ'ai chaud <br \/>\nHa ha ha, ha ha ha ha ha ... <br \/>\nJ'ai chaud <br \/>\nHa ha ha, ha ha ha ha ha ...<br \/>\nJ'ai peur<br \/>\nHa ha ha, ha ha ha ha ha ...<br \/>\nJ'ai...<br \/>\n<br \/>\nJ'sais pas<br \/>\nJ'sais pas<br \/>\n<br \/>\nJ'sais pas<br \/>\nJ'sais pas<br \/>\n<br \/>\nJ'ai chaud<br \/>\nJ'sais pas<br \/>\nHa ha ha, ha ha ha ha ha ...<br \/>\nJ'ai chaud<br \/>\nHa ha ha, ha ha ha ha ha ...<br \/>\nJ'ai peur<br \/>\nJ'ai peur<br \/>\n<br \/>\nJ'ai peur !",
			"position": "222",
			"current_time": "1485512458.9315",
			"time_elapsed": "222837.42797852"
		},
		"next_song": {
			"id": "",
			"song_db_id": "",
			"title": "",
			"artist_id": "",
			"artist": "",
			"feat_artist_id": "",
			"feat_artist": "",
			"cover_art": "",
			"biography": "",
			"biography_html": "",
			"biography_de": "",
			"biography_html_de": "",
			"biography_en": "",
			"biography_html_en": "",
			"biography_image": "",
			"biography_image_16_9": "",
			"big_biography_image": "",
			"big_biography_image_16_9": "",
			"biography_image_list": "",
			"biography_image_list_16_9": "",
			"big_biography_image_list": "",
			"big_biography_image_list_16_9": "",
			"biography_image_mobile": "",
			"biography_image_mobile_hq": "",
			"duration": "",
			"duration_sec": "",
			"ticketmaster": "",
			"buy_now_url": "",
			"timestamp": "",
			"artist_2_id": "",
			"artist_2": "",
			"artist_3_id": "",
			"artist_3": "",
			"artist_4_id": "",
			"artist_4": "",
			"artist_5_id": "",
			"artist_5": "",
			"song_artist_twitter_id": "",
			"url_share": "",
			"sanitized_url": "",
			"artist_sanitized_url": "",
			"feat_artist_sanitized_url": "",
			"artist_2_sanitized_url": "",
			"artist_3_sanitized_url": "",
			"artist_4_sanitized_url": "",
			"artist_5_sanitized_url": "",
			"lyrics": "",
			"lyrics_html": "",
			"position": "1258",
			"current_time": "1485512458.9315",
			"time_elapsed": "1485512458931.5"
		},
		"previous_song": {
			"id": "5094",
			"song_db_id": "7682",
			"title": "Could You Be Loved",
			"artist_id": "1392",
			"artist": "Bob Marley",
			"feat_artist_id": "",
			"feat_artist": "",
			"cover_art": "http:\/\/mediacdn.karnaval.com\/media\/album_media\/4155\/albumcover_400x400\/cover_4155.jpg?v=010612022129",
			"biography": "Bilinen ger\u00e7ek ad\u0131yla Robert Nesta Marley, d\u00fcnyaca \u00fcnl\u00fc Jamaikal\u0131 reggae sanat\u00e7\u0131s\u0131.\n\nGer\u00e7ek Ad\u0131: Robert Nesta Marley\nDo\u011fum Yeri: Jamaika\nDo\u011fum Tarihi: 6 \u015eubat 1945\n\nHakk\u0131nda:\n- Profesyonel anlamda m\u00fczi\u011fe The Wailers grubu ile ba\u015flam\u0131\u015ft\u0131r. \n- \u0130lk alb\u00fcm\u00fc 1965 y\u0131l\u0131nda yay\u0131nlanm\u0131\u015ft\u0131r.\n- 130\u2019un \u00fczerinde pla\u011f\u0131 bulunan bir reggae efsanesidir.\n- Reggae m\u00fczi\u011finin sadece Jamaika s\u0131n\u0131rlar\u0131nda kalmamas\u0131n\u0131 sa\u011flay\u0131p onu b\u00fct\u00fcn d\u00fcnyaya duyuran en \u00f6nemli isimlerden biridir.\n\n\u0130lk Alb\u00fcm: The Wailing Wailers",
			"biography_html": "Bilinen ger\u00e7ek ad\u0131yla Robert Nesta Marley, d\u00fcnyaca \u00fcnl\u00fc Jamaikal\u0131 reggae sanat\u00e7\u0131s\u0131.<br \/>\n<br \/>\nGer\u00e7ek Ad\u0131: Robert Nesta Marley<br \/>\nDo\u011fum Yeri: Jamaika<br \/>\nDo\u011fum Tarihi: 6 \u015eubat 1945<br \/>\n<br \/>\nHakk\u0131nda:<br \/>\n- Profesyonel anlamda m\u00fczi\u011fe The Wailers grubu ile ba\u015flam\u0131\u015ft\u0131r. <br \/>\n- \u0130lk alb\u00fcm\u00fc 1965 y\u0131l\u0131nda yay\u0131nlanm\u0131\u015ft\u0131r.<br \/>\n- 130\u2019un \u00fczerinde pla\u011f\u0131 bulunan bir reggae efsanesidir.<br \/>\n- Reggae m\u00fczi\u011finin sadece Jamaika s\u0131n\u0131rlar\u0131nda kalmamas\u0131n\u0131 sa\u011flay\u0131p onu b\u00fct\u00fcn d\u00fcnyaya duyuran en \u00f6nemli isimlerden biridir.<br \/>\n<br \/>\n\u0130lk Alb\u00fcm: The Wailing Wailers",
			"biography_de": "Geboren als Robert Nesta Marley, weltweit bekannter jamaikanischer Reggae-K\u00fcnstler.\nB\u00fcrgerlicher Name: Robert Nesta Marley\u21b5Geburtsort: Jamaika\u21b5Geburtsdatum: 6 . Februar 1945\nDaten:\n Professionelle begann er mit der Band The Wailers Musik zu spielen. \n Sein erstes Album erschien 1965.\n Er ist eine Reggae-Legende mit \u00fcber 130Schallplatten.\n Er ist ebenso einer der bedeutendsten Namen, der daf\u00fcr sorgte, dass die Reggae-Musik nicht nur mit Jamaika eingegrenzt blieb, sondern in der ganzen Welt bekannt wurde.\nErstes Album: The Wailing Wailers",
			"biography_html_de": "Geboren als Robert Nesta Marley, weltweit bekannter jamaikanischer Reggae-K\u00fcnstler.<br \/>\nB\u00fcrgerlicher Name: Robert Nesta Marley\u21b5Geburtsort: Jamaika\u21b5Geburtsdatum: 6 . Februar 1945<br \/>\nDaten:<br \/>\n Professionelle begann er mit der Band The Wailers Musik zu spielen. <br \/>\n Sein erstes Album erschien 1965.<br \/>\n Er ist eine Reggae-Legende mit \u00fcber 130Schallplatten.<br \/>\n Er ist ebenso einer der bedeutendsten Namen, der daf\u00fcr sorgte, dass die Reggae-Musik nicht nur mit Jamaika eingegrenzt blieb, sondern in der ganzen Welt bekannt wurde.<br \/>\nErstes Album: The Wailing Wailers",
			"biography_en": "",
			"biography_html_en": "",
			"biography_image": "http:\/\/mediacdn.karnaval.com\/media\/artist_media\/1392\/biography_270x202\/photo_1392_1.jpg?v=1012120152",
			"biography_image_16_9": "http:\/\/mediacdn.karnaval.com\/media\/artist_media\/1392\/biography_1280x720\/photo_1392_1.jpg?v=1012120152",
			"big_biography_image": "http:\/\/mediacdn.karnaval.com\/media\/artist_media\/1392\/biography_640x474\/photo_1392_1.jpg?v=1012120152",
			"big_biography_image_16_9": "http:\/\/mediacdn.karnaval.com\/media\/artist_media\/1392\/biography_1920x1080\/photo_1392_1.jpg?v=1012120152",
			"biography_image_list": {
				"image": ["http:\/\/mediacdn.karnaval.com\/media\/artist_media\/1392\/biography_270x202\/photo_1392_1.jpg?v=1012120152", "http:\/\/mediacdn.karnaval.com\/media\/artist_media\/1392\/biography_270x202\/photo_1392_2.jpg?v=1012120153", "http:\/\/mediacdn.karnaval.com\/media\/artist_media\/1392\/biography_270x202\/photo_1392_3.jpg?v=1012120153"]
			},
			"biography_image_list_16_9": {
				"image": ["http:\/\/mediacdn.karnaval.com\/media\/artist_media\/1392\/biography_1280x720\/photo_1392_1.jpg?v=1012120152", "http:\/\/mediacdn.karnaval.com\/media\/artist_media\/1392\/biography_1280x720\/photo_1392_2.jpg?v=1012120153", "http:\/\/mediacdn.karnaval.com\/media\/artist_media\/1392\/biography_1280x720\/photo_1392_3.jpg?v=1012120153"]
			},
			"big_biography_image_list": {
				"image": ["http:\/\/mediacdn.karnaval.com\/media\/artist_media\/1392\/biography_640x474\/photo_1392_1.jpg?v=1012120152", "http:\/\/mediacdn.karnaval.com\/media\/artist_media\/1392\/biography_640x474\/photo_1392_2.jpg?v=1012120153", "http:\/\/mediacdn.karnaval.com\/media\/artist_media\/1392\/biography_640x474\/photo_1392_3.jpg?v=1012120153"]
			},
			"big_biography_image_list_16_9": {
				"image": ["http:\/\/mediacdn.karnaval.com\/media\/artist_media\/1392\/biography_1920x1080\/photo_1392_1.jpg?v=1012120152", "http:\/\/mediacdn.karnaval.com\/media\/artist_media\/1392\/biography_1920x1080\/photo_1392_2.jpg?v=1012120153", "http:\/\/mediacdn.karnaval.com\/media\/artist_media\/1392\/biography_1920x1080\/photo_1392_3.jpg?v=1012120153"]
			},
			"biography_image_mobile": "http:\/\/mediacdn.karnaval.com\/media\/artist_media\/1392\/biography_1280x720\/photo_1392_1.jpg?v=1012120152",
			"biography_image_mobile_hq": "http:\/\/mediacdn.karnaval.com\/media\/artist_media\/1392\/biography_1920x1080\/photo_1392_1.jpg?v=1012120152",
			"duration": "3:47",
			"duration_sec": "227",
			"ticketmaster": "",
			"buy_now_url": "",
			"timestamp": "1485511999.3741",
			"artist_2_id": "0",
			"artist_2": "",
			"artist_3_id": "0",
			"artist_3": "",
			"artist_4_id": "0",
			"artist_4": "",
			"artist_5_id": "0",
			"artist_5": "",
			"song_artist_twitter_id": "",
			"url_share": "http:\/\/karnaval.com\/sarkilar\/could-you-be-loved-7682",
			"sanitized_url": "could-you-be-loved",
			"artist_sanitized_url": "bob-marley",
			"feat_artist_sanitized_url": "bob-marley",
			"artist_2_sanitized_url": "",
			"artist_3_sanitized_url": "",
			"artist_4_sanitized_url": "",
			"artist_5_sanitized_url": "",
			"lyrics": "Could you be loved and be loved?<br \/>\nCould you be loved and be loved?<br \/>\n<br \/>\nDon't let them fool ya<br \/>\nOr even try to school ya! Oh, no!<br \/>\nWe've got a mind of our own<br \/>\nSo go to hell if what you're thinking is not right!<br \/>\nLove would never leave us alone<br \/>\nAy, in the darkness there must come out to light<br \/>\n<br \/>\nCould you be loved and be loved?<br \/>\nCould you be loved, wo now! - and be loved?<br \/>\n<br \/>\n(The road of life is rocky and you may stumble too)<br \/>\n(So while you point your fingers someone else is judging you)<br \/>\nLove your brotherman!<br \/>\n(Could you be - could you be - could you be loved?)<br \/>\n(Could you be - could you be loved?)<br \/>\n(Could you be - could you be - could you be loved?)<br \/>\n(Could you be - could you be loved?)<br \/>\n<br \/>\nDon't let them change ya, oh!<br \/>\nOr even rearrange ya! Oh, no!<br \/>\nWe've got a life to live<br \/>\nThey say: only - only<br \/>\nOnly the fittest of the fittest shall survive<br \/>\nStay alive! Eh!<br \/>\n<br \/>\nCould you be loved and be loved?<br \/>\nCould you be loved, wo now! - and be loved?<br \/>\n<br \/>\n(You ain't gonna miss your water until your well runs dry)<br \/>\n(No matter how you treat him, the man will never be satisfied.)<br \/>\nSay something! (Could you be - could you be - could you be loved?)<br \/>\n(Could you be - could you be loved?)<br \/>\nSay something! Say something!<br \/>\n(Could you be - could you be - could you be loved?)<br \/>\nSay something! (Could you be - could you be loved?)<br \/>\nSay something! Say something! (Say something!)<br \/>\nSay something! Say something! (Could you be loved?)<br \/>\nSay something! Say something! Reggae, reggae!<br \/>\nSay something! Rockers, rockers!<br \/>\nSay something! Reggae, reggae!<br \/>\nSay something! Rockers, rockers!<br \/>\nSay something! (Could you be loved?)<br \/>\nSay something! Uh!<br \/>\nSay something! Come on!<br \/>\nSay something! (Could you be - could you be - could you be loved?)<br \/>\nSay something! (Could you be - could you be loved?)<br \/>\nSay something! (Could you be - could you be - could you be loved?)<br \/>\nSay something! (Could you be - could you be loved?)",
			"lyrics_html": "Could you be loved and be loved?<br \/>\nCould you be loved and be loved?<br \/>\n<br \/>\nDon't let them fool ya<br \/>\nOr even try to school ya! Oh, no!<br \/>\nWe've got a mind of our own<br \/>\nSo go to hell if what you're thinking is not right!<br \/>\nLove would never leave us alone<br \/>\nAy, in the darkness there must come out to light<br \/>\n<br \/>\nCould you be loved and be loved?<br \/>\nCould you be loved, wo now! - and be loved?<br \/>\n<br \/>\n(The road of life is rocky and you may stumble too)<br \/>\n(So while you point your fingers someone else is judging you)<br \/>\nLove your brotherman!<br \/>\n(Could you be - could you be - could you be loved?)<br \/>\n(Could you be - could you be loved?)<br \/>\n(Could you be - could you be - could you be loved?)<br \/>\n(Could you be - could you be loved?)<br \/>\n<br \/>\nDon't let them change ya, oh!<br \/>\nOr even rearrange ya! Oh, no!<br \/>\nWe've got a life to live<br \/>\nThey say: only - only<br \/>\nOnly the fittest of the fittest shall survive<br \/>\nStay alive! Eh!<br \/>\n<br \/>\nCould you be loved and be loved?<br \/>\nCould you be loved, wo now! - and be loved?<br \/>\n<br \/>\n(You ain't gonna miss your water until your well runs dry)<br \/>\n(No matter how you treat him, the man will never be satisfied.)<br \/>\nSay something! (Could you be - could you be - could you be loved?)<br \/>\n(Could you be - could you be loved?)<br \/>\nSay something! Say something!<br \/>\n(Could you be - could you be - could you be loved?)<br \/>\nSay something! (Could you be - could you be loved?)<br \/>\nSay something! Say something! (Say something!)<br \/>\nSay something! Say something! (Could you be loved?)<br \/>\nSay something! Say something! Reggae, reggae!<br \/>\nSay something! Rockers, rockers!<br \/>\nSay something! Reggae, reggae!<br \/>\nSay something! Rockers, rockers!<br \/>\nSay something! (Could you be loved?)<br \/>\nSay something! Uh!<br \/>\nSay something! Come on!<br \/>\nSay something! (Could you be - could you be - could you be loved?)<br \/>\nSay something! (Could you be - could you be loved?)<br \/>\nSay something! (Could you be - could you be - could you be loved?)<br \/>\nSay something! (Could you be - could you be loved?)",
			"position": "459",
			"current_time": "1485512458.9315",
			"time_elapsed": "459557.42822266"
		},
		"check": "0.00062179565429688"
	}
}
 */
