using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrayerPal.Services
{

    public class NearestMosquesService
    {
        HttpClient httpClient;

        public NearestMosquesService()
        {
            httpClient = new HttpClient();
        }

        public async Task<MosqueApiResponse> GetMosques(double latitude, double longitude, int proximity_radius)
        {
            try
            {
                var formattedUrl = getUrl(latitude, longitude, proximity_radius);
                Console.WriteLine($"Date: {formattedUrl}");
 



                using HttpClient client = new HttpClient();

                HttpResponseMessage response = await client.GetAsync(formattedUrl);

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(responseBody); // Log the response body for debugging

                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    try
                    {
                        MosqueApiResponse mosqueApiResponse = JsonSerializer.Deserialize<MosqueApiResponse>(responseBody, options);
                        if (mosqueApiResponse != null)
                        {
                            //Console.WriteLine(mosqueApiResponse.results.ToString());
                            //Console.WriteLine(mosqueApiResponse.status.ToString());
                            //Console.WriteLine(mosqueApiResponse.error_message.ToString());
                            //Console.WriteLine(mosqueApiResponse.info_messages.ToString());
                            //Console.WriteLine(mosqueApiResponse.next_page_token.ToString());


                            if (mosqueApiResponse.Status == null)
                            {
                                Console.WriteLine("Status is Null");
                            }
                            if (mosqueApiResponse.Results == null){
                                Console.WriteLine("results is Null");
                            }
                            if (mosqueApiResponse.Next_Page_Token == null)
                            {
                                Console.WriteLine("nextpagetoken is Null");
                            }

                            Console.WriteLine(mosqueApiResponse);
                            Console.WriteLine(mosqueApiResponse.Next_Page_Token);
                            Console.WriteLine(mosqueApiResponse.Results);
                            Console.WriteLine(mosqueApiResponse.Status);







                            return mosqueApiResponse;
                        }
                        else
                        {
                            Debug.WriteLine("Deserialized object is null. Check JSON structure and class definition.");
                            return null;
                        }
                    }
                    catch (JsonException ex)
                    {
                        Debug.WriteLine($"Error deserializing JSON: {ex.Message}");
                        await Shell.Current.DisplayAlert("Error!", "Error deserializing JSON", "OK");
                        // Handle JSON deserialization error
                        return null;
                    }
                }
                else
                {
                    Debug.WriteLine($"Google map api responded unsuccessfully. Status code:{response.StatusCode} {response.ReasonPhrase}");
                    await Shell.Current.DisplayAlert("Error!", $"Google map api responded unsuccessfully. Status code:{response.StatusCode} {response.ReasonPhrase}", "OK");



                    // Handle unsuccessful response
                    return null;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                //await Shell.Current.DisplayAlert("Error!", ex.Message, "OK");

                // Handle exception
                return null;
            }
        }


        private String getUrl(double latitude, double longitude, int proximity_radius)
        {

            StringBuilder googlePlacesUrl = new StringBuilder("https://maps.googleapis.com/maps/api/place/nearbysearch/json?");
            googlePlacesUrl.Append("location=" + latitude + "," + longitude);
            googlePlacesUrl.Append("&radius=" + proximity_radius);
            googlePlacesUrl.Append("&types=mosque");
            googlePlacesUrl.Append("&sensor=true");
            googlePlacesUrl.Append("&key=" + "AIzaSyDRz9zV1q8Kux3HUx9Fst_fqsuf1dvY6Aw");
            return googlePlacesUrl.ToString();
        }

    }
}
